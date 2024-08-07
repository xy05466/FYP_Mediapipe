using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_Massage : PageLogic_Extension
{
    //public FacepointManager_Jaden facepointManager_Jaden;
    //public AccupointsMassageManagerFinal accupointsMassageManagerFinal;

    [Header("UI")]
    public TMP_Text tmp_title;
    public TMP_Text tmp_titleSub;
    public TMP_Text tmp_description;
    public MassageSliderObjectV2 massageSliderObjectPrefeb;
    private List<MassageSliderObjectV2> massageSliderObjects = new List<MassageSliderObjectV2>();
    [Header("settings")]
    public bool enableAllHandPointCollision = true;
    public List<FaceHandPoint.HANDPOINT> list_handpoints_interact = new List<FaceHandPoint.HANDPOINT>();
    public float massageSpeed = 1f;
    public float massageDistance = 0.1f;
    public float massageFirstDistance = 0.05f;
    public List<MaskMoving> maskMoving = new List<MaskMoving>();
    public List<MaskMoving> subMaskMoving = new List<MaskMoving>();
    public List<MaskMoving> OTMaskMoving = new List<MaskMoving>();

    
    [Header("Steps")]
    public List<SliderStep> list_steps = new List<SliderStep>();
    private int currentStepIndex = 0;
    private int slidersDoneCount = 0;
    bool massageEnd = false;

    public override void Initialize()
    {
    }
    //lr.GetPosition(i-1) / 
    public override void StartPage()
    {
        int maxSpawn = 0;
        int tempcCount = 0;
        // find max number of slider needs to spawn
        foreach (var step in list_steps)
        {
            foreach (var item in step.massageSliders)
            {
                tempcCount++;
            }
            if (tempcCount > maxSpawn)
            {
                maxSpawn = tempcCount;
            }
            tempcCount = 0;
        }

        // Instantiate massage slider base on most slider in steps
        for (int i = 0; i < maxSpawn; i++)
        {
            MassageSliderObjectV2 mso = Instantiate(massageSliderObjectPrefeb, transform);
            mso.transform.localPosition = Vector3.zero;
            mso.gameObject.SetActive(false);
            massageSliderObjects.Add(mso);
        }


        GameObject[] spriteMasks = GameObject.FindGameObjectsWithTag("SpriteMask");
        GameObject[] subSpriteMasks = GameObject.FindGameObjectsWithTag("SubSpriteMask");
        GameObject[] OTSpriteMasks = GameObject.FindGameObjectsWithTag("OTSpriteMask");
        Debug.Log("total :"+spriteMasks.Length);
        foreach (GameObject obj in spriteMasks)
        {
            MaskMoving mask = obj.GetComponent<MaskMoving>();
            if (mask != null)
            {
                maskMoving.Add(mask);
            }
        }

        foreach (GameObject obj in subSpriteMasks)
        {
            MaskMoving mask = obj.GetComponent<MaskMoving>();
            if (mask != null)
            {
                subMaskMoving.Add(mask);
            }
        }

        foreach (GameObject obj in OTSpriteMasks)
        {
            MaskMoving mask = obj.GetComponent<MaskMoving>();
            if (mask != null)
            {
                OTMaskMoving.Add(mask);
            }
        }

        //// Init all slider
        //foreach (var massageSlider in massageSliderObjects2)
        //{
        //    massageSlider.Init();
        //    // subscribe all slider events
        //    massageSlider.SubscribeEvent_Colliding(OnCollidingWithSlider);
        //    massageSlider.SubscribeEvent_NextPoint(OnSliderNextPoint);
        //    massageSlider.SubscribeEvent_Ended(OnSliderEnded);
        //}

        // Subscribe slider events
        foreach (var mso in massageSliderObjects)
        {
            mso.SubscribeEvent_Ended(OnSliderEnded);
        }

        SetSliders(true);

        // set up all sliders
        foreach (var massageSlider in massageSliderObjects)
            massageSlider.SetUp();
    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
    {
        if (!FacepointsLoaded) return;
        //massageSliderObjectPrefeb2.UpdatePosition(facepointsObj[massageSliderObjectPrefeb2.startLandmark].transform.position, facepointsObj[massageSliderObjectPrefeb2.endLandmark].transform.position);

        // loop all sliders base on steps
        for (int i = 0; i < list_steps[currentStepIndex].massageSliders.Count; i++)
        {
            MassageSliderPoints massageSliderDataI = list_steps[currentStepIndex].massageSliders[i];
            MassageSliderObjectV2 msoI = massageSliderObjects[i];

            for (int j = 0; j < massageSliderDataI.points.Count; j++)
            {
                var id = massageSliderDataI.points[j];
                msoI.SetPointPosition(j, facepointsObj[id].transform.position);
            }
        }
      

        // check if certain fingeer points interacting
        // with current sliedr position
        if (handpointsObj_1 != null && handpointsObj_1.Count > 0)
        {
            if (enableAllHandPointCollision)
            {
                for (int i = 0; i < handpointsObj_1.Count; i++)
                {
                    // loop all sliders
                    for (int j = 0; j < list_steps[currentStepIndex].massageSliders.Count; j++)
                    {
                        massageSliderObjects[j].UpdateSlider(handpointsObj_1[i].gameObject, massageDistance, massageSpeed, massageFirstDistance);
                        MovementOfMask(j);
                    }
                }
            }
            else
            {
                foreach (var item in list_handpoints_interact)
                {
                    int handItem = (int)item;
                    // loop all sliders
                    for (int i = 0; i < list_steps[currentStepIndex].massageSliders.Count; i++)
                    {
                        massageSliderObjects[i].UpdateSlider(handpointsObj_1[handItem].gameObject, massageDistance, massageSpeed, massageFirstDistance);
                    }
                }
            }
        }
        //// Update all  massage sliders
        //foreach (var massageSlider in massageSliderObjects)
        //    massageSlider.UpdateSlider(facepointsObj);
    }
    public override void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject)
    {
        //foreach (var massageSliderObject in massageSliderObjects)
        //{
        //    if(massageSliderObject.GetCurrentIndex() >= massageSliderObject.points.Count) return;
        //    //if (points[currentIndex] == facePointIndex)
        //    //{
        //    //    currentIndex++;
        //    //    sliderfloat = 0;
        //    //}
        //    //if (currentIndex > 0 && points[currentIndex - 1] == facePointIndex)
        //    //{
        //    //    sliderfloat += Time.deltaTime;
        //    //    sliderfloat = Mathf.Clamp(sliderfloat, 0.0f, 1.0f);
        //    //}
        
        //    //// if collide with current facepoint
        //    //if (CollideWithFacePoint(massageSliderObject.points[massageSliderObject.GetCurrentIndex()], facePointIndex))
        //    //{
        //    //    //FillCurrentPoint();
        //    //}
        //}
    }
    public override void EndPage()
    {
        massageEnd = false;
    }
    public override void ResetPage()
    {
    }
    public override bool IsPageDone()
    {
        //return accupointsMassageManagerFinal.IsDone();
        return massageEnd;
    }
    public override void OnFaceAnnotationActive(bool active)
    {
        //accupointsMassageManagerFinal.gameObject.SetActive(active);
        //ActivateSliders(active);
        ActivateSliders(active);
    }
    public override void OnHandAnnotationActive(int actives)
    {
    }
    private void SetSliders(bool activate)
    {
        if (massageSliderObjects.Count <= 0) return;

        int currentSlider = 0;
        // disable all slider
        for (int i = 0; i < massageSliderObjects.Count; i++)
            massageSliderObjects[i].gameObject.SetActive(false);

        if (activate)
        {
            // loop through each slider data base on current step
            foreach (var massageSliderData in list_steps[currentStepIndex].massageSliders)
            {
                // set up the points on slider 
                for (int i = 0; i < massageSliderData.points.Count; i++)
                {
                    int point = massageSliderData.points[i];
                    massageSliderObjects[currentSlider].gameObject.SetActive(activate);
                    //massageSliderObjects2[currentSlider].AddPoint(point);
                }
                currentSlider++;
            }
        }
      
    }
    private void ActivateSliders(bool active)
    {
        if (massageSliderObjects.Count <= 0) return;
        for (int i = 0; i < list_steps[currentStepIndex].massageSliders.Count; i++)
            massageSliderObjects[i].gameObject.SetActive(active);
    }

    // Get the total sliders available based on current step
    int GetCurrentTotalSliders()
    {
        return list_steps[currentStepIndex].massageSliders.Count;
    }
   
    // On colliding with massage slider
    //void OnCollidingWithSlider(MassageSliderObject massageSliderObject, int sliderId, GameObject goObject, Collider other)
    //{
    //    if (massageSliderObject.GetCurrentIndex() >= massageSliderObject.points.Count) return;

    //    if (other.gameObject.tag == "Handpoint")
    //    {
    //        if(sliderId == massageSliderObject.GetCurrentIndex() || 
    //            sliderId == massageSliderObject.GetCurrentIndex()+1 && massageSliderObject.GetPercentageSegmantFilled() >= 0.8f)
    //        {
    //            if (!enableAllHandPointCollision)
    //            {
    //                bool proceed = false;
    //                foreach (var item in list_handpoints_interact)
    //                {
    //                    if (other.gameObject.transform.GetSiblingIndex() == (int)item)
    //                    {
    //                        proceed = true;
    //                        break;
    //                    }
    //                }

    //                if (!proceed) { return; }
    //            }

    //            Vector3 startPoint = massageSliderObject.lr_outline.GetPosition(massageSliderObject.GetCurrentIndex());
    //            Vector3 endpoint = massageSliderObject.lr_outline.GetPosition(massageSliderObject.GetCurrentIndex() + 1);

    //            // Calculate the total distance
    //            float totalDistance = Vector3.Distance(startPoint, massageSliderObject.lr_filled.GetPosition(massageSliderObject.lr_filled.positionCount - 1));

    //            // Calculate the distance from start to the check position
    //            float checkDistance = Vector3.Distance(startPoint, other.gameObject.transform.position);


    //            float totalDistance2 = Vector3.Distance(startPoint, endpoint);

    //            if (checkDistance > totalDistance)
    //                massageSliderObject.FillCurrentPoint(1 / totalDistance2);
    //        }
    //    }
    //}

    // on first collider with next point in the slider,
    // the points are usually the face landmarks
    //void OnSliderNextPoint(int point)
    //{
    //    Debug.Log("slider next :" + point);
    //}
    void OnSliderEnded(MassageSliderObjectV2 massageSliderObject)
    {
        for (int i = 0; i < maskMoving.Count; i++)
        {
            maskMoving[i].PlayAnim();
            for (int j = 0; j < subMaskMoving.Count; j++)
            {
                subMaskMoving[j].PlayAnim();
            }
        }
        slidersDoneCount++;
        // check if all current slider has completed , done
        if(slidersDoneCount >= list_steps[currentStepIndex].massageSliders.Count )
        {
            NextStep();
        }
    }
    // go to next step 
    void NextStep()
    {
        if (currentStepIndex >= list_steps.Count - 1)
        {
            End();
            return;
        };

        currentStepIndex++;
        // reset all slider
        foreach (var massageSlider in massageSliderObjects)
            massageSlider.ResetSlider();

        SetSliders(true);
        if(tmp_title)
            tmp_title.text = list_steps[currentStepIndex].title;
        if(tmp_titleSub)
            tmp_titleSub.text = list_steps[currentStepIndex].titleSub;
        if(tmp_description)
            tmp_description.text = list_steps[currentStepIndex].description;


    }
    public void End()
    {
        massageEnd = true;
        foreach (var massageSlider in massageSliderObjects)
            massageSlider.gameObject.SetActive(false);
    }

    void MovementOfMask(int massageSliderOBJ)
    {
        for (int maskMovingIndex = 0; maskMovingIndex < maskMoving.Count; maskMovingIndex++)
        {
            if (massageSliderObjects[massageSliderOBJ].GetValue() > 0f)
            {
                maskMoving[maskMovingIndex].PercentageMovement(massageSliderObjects[massageSliderOBJ].GetValue() + 0.1f);
            }

            foreach (MaskMoving subSpriteMask in subMaskMoving)
            {
                if (subSpriteMask.canMove)
                {
                    subSpriteMask.PercentageMovement(massageSliderObjects[massageSliderOBJ].GetValue() + 0.1f);
                }

                float distance = Vector2.Distance(maskMoving[maskMovingIndex].transform.position, subSpriteMask.transform.position);

                if (distance < 0.1f)
                {
                    subSpriteMask.canMove = true;
                }
            }
        }

    }
}

[Serializable]
public class SliderStep
{
    public string stepName;
    public string title;
    public string titleSub;
    [TextArea]
    public string description;
    public List<MassageSliderPoints> massageSliders = new List<MassageSliderPoints>();
}
[Serializable]
public class MassageSliderPoints
{
    public List<int> points = new List<int>();
}

