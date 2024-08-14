using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression_Masking : ProgressionComponent
{


    public List<MaskMoving> maskMoving = new List<MaskMoving>();
    public List<MaskMovingList> OuterList = new List<MaskMovingList>();
    private void Start()
    {
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

    }
    public override void OnProgression(int currentProgression, int maxProgression)
    {

    }

 

    public override void UpdateProgression(float subProgression, int currentProgress, int maxProgress)
    {
        CheckIndexMask(currentProgress, subProgression);
        Debug.Log(subProgression + " " + currentProgress + " " + maxProgress);
    }

    // Masking
    int index;
    void CheckIndexMask(int m, float value)
    {
        if (m >= OuterList.Count) return;

        MaskMovingList currentMaskOBJ = OuterList[m];

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
