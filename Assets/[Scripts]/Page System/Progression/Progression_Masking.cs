using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDM_FythmPipe
{
    public class Progression_Masking : ProgressionComponent
    {
        public List<MaskMoving> maskMoving = new List<MaskMoving>();
        public List<MaskMovingList> OuterList = new List<MaskMovingList>();
        private void Start()
        {
            GameObject[] spriteMasks = GameObject.FindGameObjectsWithTag("SpriteMask");
            foreach (Component go in transform.GetComponentsInChildren<Component>(true))
            {
                if (go.gameObject.CompareTag("SpriteMask"))
                {
                    MaskMoving mask = go.gameObject.GetComponent<MaskMoving>();
                    if (mask != null)
                    {
                        maskMoving.Add(mask);
                        mask.gameObject.SetActive(false);
                    }
                }
            }

            Debug.Log("countt" + spriteMasks.Length);
            //GameObject[] subSpriteMasks = GameObject.FindGameObjectsWithTag("SubSpriteMask");

            //foreach (GameObject obj in spriteMasks)
            //{
            //    MaskMoving mask = obj.GetComponent<MaskMoving>();
            //    if (mask != null)
            //    {
            //        maskMoving.Add(mask);
            //        mask.gameObject.SetActive(false);
            //    }
            //}

            foreach (MaskMovingList maskList in OuterList)
            {
                foreach (MaskMoving mask in maskList.MovingList)
                {
                    mask.gameObject.SetActive(false);
                }
            }

        }
        public override void OnProgression(int currentProgression, int maxProgression)
        {

        }

        int index = 0;

        public override void UpdateProgression(float subProgression, int currentProgress, int maxProgress)
        {
            //CheckIndexMask(currentProgress, subProgression);
            Debug.Log(subProgression + " " + currentProgress + " " + maxProgress);


            if (index < OuterList.Count)
            {
                MaskMovingList currentMaskOBJ = OuterList[index];

                foreach (MaskMoving mask in currentMaskOBJ.MovingList)
                {
                    if (mask.gameObject.CompareTag("SpriteMask"))
                    {
                        mask.gameObject.SetActive(true);
                        mask.PercentageMovement(subProgression + 0.1f);
                    }
                }
                if (index != currentProgress)
                {
                    currentMaskOBJ.IsDone = true;
                    foreach (MaskMoving mask in currentMaskOBJ.MovingList)
                    {
                        mask.PlayAnim();
                        currentMaskOBJ.IsDone = true;
                        mask.gameObject.SetActive(false);
                    }
                    index++;
                }
            }


            //if(index != currentProgress)
            //{
            //    if (index < OuterList.Count)
            //    {
            //        MaskMovingList currentMaskOBJ = OuterList[index];
            //        foreach (MaskMoving mask in currentMaskOBJ.MovingList)
            //        {
            //            mask.PlayAnim();
            //            currentMaskOBJ.IsDone = true;
            //            mask.gameObject.SetActive(false);
            //        }
            //    }
            //    index = currentProgress;
            //}

        }

        // Masking
        void CheckIndexMask(int m, float value)
        {
            if (index >= OuterList.Count) return;
            Debug.Log("mask " + value);

            MaskMovingList currentMaskOBJ = OuterList[index];

            foreach (MaskMoving mask in currentMaskOBJ.MovingList)
            {
                if (mask.gameObject.CompareTag("SpriteMask"))
                {
                    mask.gameObject.SetActive(true);
                    mask.PercentageMovement(value + 0.1f);
                }
            }


            if (currentMaskOBJ.IsDone)
            {
                index++;
                if (index < OuterList.Count)
                {
                    CheckIndexMask(m, value);
                }
            }
        }
    }
}