using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accupoint : MonoBehaviour
{
    public float maxTimer = 3.0f;
    [SerializeField] AccupointSO accupointData;
    public float timer;
    Material mat;
    // Colors are in this order as we are going to be lerping from 1 (timer / maxTimer) to 0.
    // As such, we will be going from the last color in the array to the first. Hence, we put red at the back and green in front.
    public Color[] colors = { Color.red, Color.yellow, Color.green };

    public void SetData(AccupointSO data)
    {
        accupointData = data;
    }
    void RepositionPoint(Vector3 newPoint)
    {
        gameObject.transform.position = newPoint;
    }
    public AccupointSO GetData()
    {
        return accupointData;
    }
    private void Awake()
    {
        timer = 0;
        mat = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        //Debug.Log(transform.position.x + ", " + transform.position.y + ", " + transform.position.z + " 2");
        if (timer < maxTimer)
        {
            // test implementation from https://gamedev.stackexchange.com/questions/98740/how-to-color-lerp-between-multiple-colors
            float scaledTime = (timer / maxTimer)   * (colors.Length - 1);
            //if (scaledTime > 0)
            //Debug.Log(scaledTime);
            Color oldColor = colors[(int)scaledTime];
            Color newColor;
            if (timer >= maxTimer)
            {
                newColor = colors[colors.Length - 1];
            }
            else
            {
                newColor = colors[(int)(scaledTime + 1f)];
            }
            float newT = scaledTime - Mathf.Floor(scaledTime);
            // old implementation
            mat.color = Color.Lerp(oldColor, newColor, newT);
        }
        else
        {
            //gameObject.SetActive(false);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        timer += Time.deltaTime;
    }

}
