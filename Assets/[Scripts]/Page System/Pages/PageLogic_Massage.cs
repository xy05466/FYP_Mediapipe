using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
    public class PageLogic_Massage : PageLogic
    {
        //public FacepointManager_Jaden facepointManager_Jaden;
        //public AccupointsMassageManagerFinal accupointsMassageManagerFinal;

        [Header("UI")]
        public TMP_Text tmp_title;
        public TMP_Text tmp_titleSub;
        public TMP_Text tmp_description;
        public MassageSliderObject massageSliderObjectPrefeb;
        private List<MassageSliderObject> massageSliderObjects = new List<MassageSliderObject>();
        [Header("settings")]
        public bool enableAllHandPointCollision = true;
        public List<FaceHandPoint.HANDPOINT> list_handpoints_interact = new List<FaceHandPoint.HANDPOINT>();
        public float massageSpeed = 1f;
        public float massageDistance = 0.1f;
        public float massageFirstDistance = 0.05f;


        [Header("Steps")]
        public List<SliderStep> list_steps = new List<SliderStep>();
        private int currentStepIndex = 0;
        private int slidersDoneCount = 0;
        bool massageEnd = false;
        float subProgress;
        int totalSliders;

        public List<MaskMoving> maskMoving = new List<MaskMoving>();
        public List<MaskMovingList> OuterList = new List<MaskMovingList>();

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
                foreach (var item in step.curveDatas)
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
                MassageSliderObject mso = Instantiate(massageSliderObjectPrefeb, transform);
                mso.transform.localPosition = Vector3.zero;
                mso.gameObject.SetActive(false);
                massageSliderObjects.Add(mso);
            }

            GameObject[] spriteMasks = GameObject.FindGameObjectsWithTag("SpriteMask");
            //GameObject[] subSpriteMasks = GameObject.FindGameObjectsWithTag("SubSpriteMask");

            foreach (GameObject obj in spriteMasks)
            {
                MaskMoving mask = obj.GetComponent<MaskMoving>();
                if (mask != null)
                {
                    maskMoving.Add(mask);
                    mask.gameObject.SetActive(false);
                }
            }

            foreach (MaskMovingList maskList in OuterList)
            {
                foreach (MaskMoving mask in maskList.MovingList)
                {
                    mask.gameObject.SetActive(false);
                }
            }

            // Subscribe slider events
            foreach (var mso in massageSliderObjects)
            {
                mso.SubscribeEvent_Ended(OnSliderEnded);
                mso.SubscribeEvent_Update(OnSliderUpodate);
            }

            SetSliders(true);

            // set up all sliders
            foreach (var massageSlider in massageSliderObjects)
                massageSlider.SetUp();

            UpdateUI();
            totalSliders = GetTotalSliders();
        }
        public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
        {
            if (!HasFacepointsLoaded) return;
            // loop all sliders base on steps
            for (int i = 0; i < list_steps[currentStepIndex].curveDatas.Count; i++)
            {
                CurveData curveData = list_steps[currentStepIndex].curveDatas[i];
                MassageSliderObject msoI = massageSliderObjects[i];

                for (int j = 0; j < curveData.points.Length; j++)
                {
                    var id = curveData.points[j];
                    msoI.SetPointPosition(j, facepointsObj[id].transform.position);
                }
                msoI.UpdatePosition();
            }


            UpdateSliders(handpointsObj_1);
            UpdateSliders(handpointsObj_2);

        }

        private void UpdateSliders(List<HandPointAnnotationObject> handpointsObj)
        {
            // check if certain fingeer points interacting
            // with current sliedr position
            if (handpointsObj != null && handpointsObj.Count > 0 && handpointsObj[0].gameObject.activeInHierarchy)
            {
                if (enableAllHandPointCollision)
                {
                    for (int i = 0; i < handpointsObj.Count; i++)
                    {
                        // loop all sliders
                        for (int j = 0; j < list_steps[currentStepIndex].curveDatas.Count; j++)
                        {
                            massageSliderObjects[j].UpdateSlider(handpointsObj[i].gameObject, massageDistance, massageSpeed, massageFirstDistance);
                            if (massageSliderObjects[j].GetValue() > 0f)
                            {
                                CheckIndexMask(j);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in list_handpoints_interact)
                    {
                        int handItem = (int)item;
                        // loop all sliders
                        for (int i = 0; i < list_steps[currentStepIndex].curveDatas.Count; i++)
                        {
                            massageSliderObjects[i].UpdateSlider(handpointsObj[handItem].gameObject, massageDistance, massageSpeed, massageFirstDistance);
                        }
                    }
                }
            }
        }

        public override void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject)
        {

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
                foreach (var massageSliderData in list_steps[currentStepIndex].curveDatas)
                {
                    // set up the points on slider 
                    for (int i = 0; i < massageSliderData.points.Length; i++)
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
            for (int i = 0; i < list_steps[currentStepIndex].curveDatas.Count; i++)
                massageSliderObjects[i].gameObject.SetActive(active);
        }

        // Get the total sliders available based on current step
        private int GetCurrentTotalSliders()
        {
            return list_steps[currentStepIndex].curveDatas.Count;
        }

        private int GetTotalSliders()
        {
            int total = 0;
            foreach (var step in list_steps)
            {
                total += step.curveDatas.Count;
            }
            return total;
        }

        private void OnSliderEnded(MassageSliderObject massageSliderObject)
        {
            massageSliderObject.gameObject.SetActive(false);
            if (index < OuterList.Count)
            {
                MaskMovingList currentMaskOBJ = OuterList[index];
                foreach (MaskMoving mask in currentMaskOBJ.MovingList)
                {
                    mask.PlayAnim();
                    currentMaskOBJ.IsDone = true;
                    mask.gameObject.SetActive(false);
                }
            }

            slidersDoneCount++;
            // check if all current slider has completed , done
            if (slidersDoneCount >= list_steps[currentStepIndex].curveDatas.Count)
            {
                NextStep();
                subProgress = 0f;
            }
        }
        private void OnSliderUpodate(MassageSliderObject massageSliderObject, float sliderValue, float newValueAdd)
        {
            float totalSliderProgress = 0f;
            for (int i = 0; i < list_steps[currentStepIndex].curveDatas.Count; i++)
            {
                foreach (var item in massageSliderObjects)
                {
                    totalSliderProgress += item.GetValue();
                }
            }

            subProgress = totalSliderProgress /*/ list_steps[currentStepIndex].curveDatas.Count*/;
            Debug.Log("sub progress " + subProgress);
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

            UpdateUI();
        }
        public void UpdateUI()
        {
            if (tmp_title)
                tmp_title.text = list_steps[currentStepIndex].title;
            if (tmp_titleSub)
                tmp_titleSub.text = list_steps[currentStepIndex].titleSub;
            if (tmp_description)
                tmp_description.text = list_steps[currentStepIndex].description;
        }
        public void End()
        {
            massageEnd = true;
            foreach (var massageSlider in massageSliderObjects)
                massageSlider.gameObject.SetActive(false);
        }

        public override int GetPageProgress()
        {
            return slidersDoneCount;
        }
        public override int GetMaxPageProgress()
        {
            return GetTotalSliders();
        }

        public override float GetSubProgression()
        {
            return subProgress;
        }

        // Masking
        int index;
        void CheckIndexMask(int m)
        {
            Debug.Log("mask " + m + " " + massageSliderObjects[m].GetValue());
            if (index >= OuterList.Count) return;

            MaskMovingList currentMaskOBJ = OuterList[index];

            foreach (MaskMoving mask in currentMaskOBJ.MovingList)
            {
                if (mask.gameObject.CompareTag("SpriteMask"))
                {
                    mask.gameObject.SetActive(true);
                    mask.PercentageMovement(massageSliderObjects[m].GetValue() + 0.1f);
                }
            }

            if (currentMaskOBJ.IsDone)
            {
                index++;
                if (index < OuterList.Count)
                {
                    CheckIndexMask(m);
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
        public List<CurveData> curveDatas = new List<CurveData>();
        public List<MassageSliderPoints> massageSliders = new List<MassageSliderPoints>();
    }
    [Serializable]
    public class MassageSliderPoints
    {
        public List<int> points = new List<int>();
    }

    [Serializable]
    public class MaskMovingList
    {
        public List<MaskMoving> MovingList = new List<MaskMoving>();
        public bool IsDone;
    }

}