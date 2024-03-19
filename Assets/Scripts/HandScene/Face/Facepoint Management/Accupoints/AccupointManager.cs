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
    }
    void InitAccupoints()
    {
        // Create accupoints
        for (int i = 0; i < accupointSOs.Count; ++i)
        {
            Debug.Log((int)accupointSOs[i].pointTethering.w);
            Instantiate(accupointPrefab, _facepointManager.GetFacePoint((int)accupointSOs[i].pointTethering.w).GetComponent<Transform>().position, Quaternion.identity, gameObject.transform);
        }
        // Place accupoints

    }
}
