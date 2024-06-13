using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccupointMassage : MonoBehaviour
{
    //add an event here
    AccupointSO accupointData;
    Material mat;
    bool isActivated = false;
    int order = 0;
    public delegate void OnActivate(int order);
    // Declare a delegate variable
    public static event OnActivate onActivate;
    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        mat.color = Color.red; 
        isActivated = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isActivated)
            return;
        onActivate?.Invoke(order);
        isActivated = true;
        mat.color = Color.green;
    }
    public void SetData(AccupointSO data)
    {
        accupointData = data;
    }
    public AccupointSO GetData()
    {
        return accupointData;
    }
    public void SetOrder(int data)
    {
        order = data;
    }
    public int GetOrder()
    {
        return order;
    }
    
    public bool GetActive()
    {
        return isActivated;
    }
    public void reset()
    {
        isActivated = false;
        mat.color = Color.red;
    }
}
