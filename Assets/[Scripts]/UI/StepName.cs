using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StepName : MonoBehaviour
{
    public TMP_Text stepName;
    public string stepNameString;
    // Start is called before the first frame update
    void Start()
    {
        stepNameString = "Left Jaw Glide";
        stepName.text = stepNameString;
    }

    // Update is called once per frame
    void Update()
    {
        stepName.text = stepNameString;
    }
}
