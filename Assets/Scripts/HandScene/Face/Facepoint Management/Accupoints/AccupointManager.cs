using System.Collections.Generic;
using UnityEngine;

public class AccupointManager : MonoBehaviour
{
    [SerializeField] FacepointManager _facepointManager;
    [SerializeField] List<AccupointSO> accupointSOs;
    [SerializeField] GameObject accupointPrefab;
    List<Accupoint> accupoints;

    private void Awake()
    {
        accupoints = new List<Accupoint>();
    }
    private void Update()
    {
        // Every frame, check if accupoint is within the allowed xyz radius.
        // If not, reposition.
    }
    void InitAccupoints()
    {
        // Create accupoints
        // Place accupoints
    }
}
