using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccupointSlider : MonoBehaviour
{

    public RhythmSlider rhythmSlider;

    //Event for OnEnterSlider
    //public delegate void OnRhythmSliderEnter();
    //public static event OnRhythmSliderEnter onRhythmSliderEnter;
    ////Event for OnExitSlider
    //public delegate void OnRhythmSliderExit();
    //public static event OnRhythmSliderExit onRhythmSliderExit;

    public float distanceTraveled = 0.0f;
    public float MaxDistance;
    public float speed = 0.0f;

    bool CollisionDetected;
    public bool destroyObject = true;
    // Start is called before the first frame update
    void Start()
    {
        //RhythmGameController.onStartMoveSlider += StartMove;
        //find max distance here
        MaxDistance = rhythmSlider.LUT[rhythmSlider.LUT.Length - 1];
        
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceTraveled >= MaxDistance && destroyObject)
        {
            Destroy(transform.parent.gameObject);
        }
    }
    void FixedUpdate()
    {
        float distanceToMove = speed * Time.deltaTime;
        // Map distance to corresponding t value
        float tValue = rhythmSlider.DistToT(rhythmSlider.LUT, distanceTraveled);

        // Calculate character position using cubic Bezier curve equation
        Vector3 characterPosition = rhythmSlider.RenderingOfGraph(tValue);

        // Update character position
        transform.position = characterPosition;

        distanceTraveled += distanceToMove;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //onRhythmSliderEnter?.Invoke();
        if(speed != 0)
        {
            CollisionDetected = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //onRhythmSliderExit?.Invoke();
        if (speed != 0)
        {
            CollisionDetected = false;

        }
    }
    public void StartMove()
    {
        speed = rhythmSlider.speed;
    }
}
