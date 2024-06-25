using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_Essence : PageLogic_Extension
{
    public FacepointManager_Jaden facepointManager_Jaden;
    public AccupointsMassageManagerFinal accupointsMassageManagerFinal;


    public MassageSliderObject massageSliderObjectPrefeb;
    public List<MassageSliderObject> massageSliderObjects;
    [Header("TESTING point")]
    public List<SliderStep> list_steps = new List<SliderStep>();
    private int currentStepIndex = 0;
    //lr.GetPosition(i-1) / 
    public override void StartPage()
    {
        //accupointsMassageManagerFinal?.StartFunc();

        //lr.materials[0].SetTexture("_MainTex", testSprite);

        int maxSpawn = 0;
        int tempcCount = 0;
        // find max number of slider needs to spawn
        foreach (var step in list_steps) {
            foreach (var item in step.massageSliders)
            {
                tempcCount++;
            }
            if (tempcCount > maxSpawn) { 
                maxSpawn = tempcCount; 
            }
            tempcCount = 0;
        }

        for (int i = 0; i < maxSpawn; i++)
        {
            MassageSliderObject mso = Instantiate(massageSliderObjectPrefeb,transform);
            mso.transform.localPosition = Vector3.zero;
            massageSliderObjects.Add(mso);
        }
        // Init all slider
        foreach (var massageSlider in massageSliderObjects)
        {
            massageSlider.Init();
            // subscribe all slider events
            massageSlider.SubscribeEvent_Colliding(OnCollidingWithSlider);
            massageSlider.SubscribeEvent_NextPoint(OnSliderNextPoint);
            massageSlider.SubscribeEvent_Ended(OnSliderEnded);
        }
        StartStep();
    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (facepointsObj == null || facepointsObj.Count <= 0) return;
        //if (accupointsMassageManagerFinal.gameObject.activeInHierarchy)
        //    accupointsMassageManagerFinal.UpdateScene();
        foreach (var massageSlider in massageSliderObjects)
            massageSlider.UpdateSlider(facepointsObj);
    }
    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
        foreach (var massageSliderObject in massageSliderObjects)
        {
            if(massageSliderObject.GetCurrentIndex() >= massageSliderObject.points.Count) return;
            //if (points[currentIndex] == facePointIndex)
            //{
            //    currentIndex++;
            //    sliderfloat = 0;
            //}
            //if (currentIndex > 0 && points[currentIndex - 1] == facePointIndex)
            //{
            //    sliderfloat += Time.deltaTime;
            //    sliderfloat = Mathf.Clamp(sliderfloat, 0.0f, 1.0f);
            //}
        
            // if collide with current facepoint
            if (CollideWithFacePoint(massageSliderObject.points[massageSliderObject.GetCurrentIndex()], facePointIndex))
            {
                //FillCurrentPoint();
            }
        }
    }
    public override void EndPage()
    {
    }
    public override void ResetPage()
    {
    }
    public override bool IsPageDone()
    {
        //return accupointsMassageManagerFinal.IsDone();
        return false;
    }
    public override void OnFaceAnnotationActive(bool active)
    {
        //accupointsMassageManagerFinal.gameObject.SetActive(active);
    }
    public override void OnHandAnnotationActive(bool active)
    {
    }
    void StartStep()
    {
        int currentSlider = 0;

        // loop through each slider data base on current step
        foreach (var massageSliderData in list_steps[currentStepIndex].massageSliders)
        {
            // set up the points on slider 
            for (int i = 0; i < massageSliderData.points.Count; i++)
            {
                int point = massageSliderData.points[i];
                massageSliderObjects[currentSlider].AddPoint(point);

            }
            currentSlider++;
        }

    }

    // Get the total sliders available based on current step
    int GetCurrentTotalSliders()
    {
        return list_steps[currentStepIndex].massageSliders.Count;
    }
    void OnCollidingWithSlider(MassageSliderObject massageSliderObject, int id, GameObject goObject, Collider other)
    {
        if (massageSliderObject.GetCurrentIndex() >= massageSliderObject.points.Count) return;
        if (other.gameObject.tag == "Handpoint" && id == massageSliderObject.GetCurrentIndex())
        {
            Vector3 startPoint = massageSliderObject.lr_outline.GetPosition(massageSliderObject.GetCurrentIndex());
            Vector3 endpoint = massageSliderObject.lr_outline.GetPosition(massageSliderObject.GetCurrentIndex() + 1);

            // Calculate the total distance
            float totalDistance = Vector3.Distance(startPoint, massageSliderObject.lr_filled.GetPosition(massageSliderObject.lr_filled.positionCount - 1));

            // Calculate the distance from start to the check position
            float checkDistance = Vector3.Distance(startPoint, other.gameObject.transform.position);


            float totalDistance2 = Vector3.Distance(startPoint, endpoint);

            if (checkDistance > totalDistance)
                massageSliderObject.FillCurrentPoint(1 / totalDistance2);
        }
    }

    void OnSliderNextPoint(int point)
    {
        Debug.Log("slider next :" + point);
    }
    void OnSliderEnded()
    {
        Debug.Log("slider ended");
    }
}

[Serializable]
public class SliderStep
{
    public List<MassageSliderPoints> massageSliders = new List<MassageSliderPoints>();
}
[Serializable]
public class MassageSliderPoints
{
    public List<int> points = new List<int>();
}

