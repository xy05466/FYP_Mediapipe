using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity.Sample;
public class APIRunner : MonoBehaviour
{
    [SerializeField] private TaskApiRunner apiRunner;
    // Start is called before the first frame update
    void Start()
    {
    if (apiRunner != null)
      apiRunner.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
