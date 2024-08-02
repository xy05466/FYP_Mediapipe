using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class MaskMoving : MonoBehaviour
{
    [SerializeField] private GameObject _maskPrefab;
    [SerializeField] private int _numMasksPerBone = 1; // the higher the value, the smaller the masks, but more masks are spawned
    private Dictionary<int, float> _boneLengths = new Dictionary<int, float>();
    private List<GameObject> _masks = new List<GameObject>();
    private int _prevBoneIndex = 0;

    public List<Transform> bonePoints = new List<Transform>();
    public SpriteRenderer spriteRenderer;
    public MaskingObjectPool Mask;
    public Animator animator;
    [HideInInspector]
    public bool canMove = false;
    [HideInInspector]
    public bool canScale = false;
    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        
        if (animator != null)
            animator.enabled = false;

        transform.position = bonePoints.LastOrDefault().position;

        _prevBoneIndex = bonePoints.Count - 1;
        var prevPosition = transform.position;
        for(int i = 0; i < bonePoints.Count; i++)
        {
            _boneLengths[i] = Vector3.Magnitude(bonePoints[i].position - prevPosition);
            prevPosition = bonePoints[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PercentageMovement(float sliderValue)
    {
        int currentBoneIndex = bonePoints.Count - 1 - (int)(sliderValue * (bonePoints.Count - 1));
        transform.position = Vector3.Lerp(transform.position, bonePoints[currentBoneIndex].position, Time.deltaTime / bonePoints.Count);
        
        // TODO (KS to Bryant): We want to check in more granular steps, not only when prevBoneIndex != currentBoneIndex
        // But maybe even better(?) is, spawn the masks on every segment together when the startMask (this.transform.position) is also being lerped.
        // If this idea is better, put these 2 steps together in a coroutine
        if (_prevBoneIndex != currentBoneIndex)
        {
            // NOTE: KS try 2
            for (int i = _prevBoneIndex; i > currentBoneIndex; i--)
            {
                float boneLength = _boneLengths[i];
                float maskDiameter = boneLength / _numMasksPerBone; // diameter = scale
                for (int j = 0; j < _numMasksPerBone; j++)
                {
                    GameObject mask = Instantiate(_maskPrefab);
                    mask.transform.localScale = Vector3.one * maskDiameter;
                    mask.transform.rotation = Quaternion.identity;
                    mask.transform.position = bonePoints[i].position + (bonePoints[currentBoneIndex].position - bonePoints[i].position).normalized * (2 * j + 1) * (maskDiameter / 2);
                    _masks.Add(mask);
                }
            }
            _prevBoneIndex = currentBoneIndex;
        }
    }


    //doesnt move with slider value
    int subTempValue = 0;
    public void SubPercentageMovement()
    {
        int value = bonePoints.Count - 1;
        transform.position = Vector3.Lerp(transform.position, bonePoints[value].position, Time.deltaTime);
        if (Vector3.Distance(transform.position, bonePoints[value].position) < 0.2f && subTempValue != value)
        {
            subTempValue = value;
            GameObject temp = Mask.GetObj(bonePoints[_prevBoneIndex].position, transform.rotation);
            value--;
        }
    }

    public void PlayAnim()
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
}
