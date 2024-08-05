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
    private List<GameObject> _masks = new List<GameObject>();
    private Vector3 _startPosition;

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

        transform.position = bonePoints[0].position;

        var prevPosition = transform.position;
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PercentageMovement(float sliderValue)
    {
        if (_masks.Count >= bonePoints.Count * _numMasksPerBone) return;

        int currentBoneIndex = (int)(sliderValue * (bonePoints.Count - 1));
        transform.position = Vector3.Lerp(transform.position, bonePoints[currentBoneIndex].position, Time.deltaTime / bonePoints.Count);

        // NOTE: _prevBoneIndex should always be 1 less of currentBoneIndex
        var prevMaskPosition = _masks.Count <= 0 ? _startPosition : _masks[_masks.Count - 1].transform.position;
        if (_masks.Count <= 0 || Vector3.Distance(prevMaskPosition, transform.position) > _masks[_masks.Count - 1].transform.localScale.x)
        {
            float boneLength = Vector3.Distance(bonePoints[currentBoneIndex + 1].position, bonePoints[currentBoneIndex].position);
            float maskDiameter = boneLength / _numMasksPerBone; // diameter = scale
            // TODO: This should be wrong (and is actually still wrong). Correct solution should be [currentBoneIndex + 1] - [currentBoneIndex]
            Vector3 boneDirection = (bonePoints[currentBoneIndex].position - bonePoints[Mathf.Max(currentBoneIndex -1, 0)].position).normalized;
            
            GameObject mask = Instantiate(_maskPrefab);
            mask.transform.localScale = Vector3.one * maskDiameter;
            mask.transform.rotation = Quaternion.identity;
            mask.transform.position = prevMaskPosition + boneDirection * (maskDiameter);
            
            _masks.Add(mask);
        }
    }

    ////doesnt move with slider value
    //int subTempValue = 0;
    //public void SubPercentageMovement()
    //{
    //    int value = bonePoints.Count - 1;
    //    transform.position = Vector3.Lerp(transform.position, bonePoints[value].position, Time.deltaTime);
    //    if (Vector3.Distance(transform.position, bonePoints[value].position) < 0.2f && subTempValue != value)
    //    {
    //        subTempValue = value;
    //        GameObject temp = Mask.GetObj(bonePoints[_prevBoneIndex].position, transform.rotation);
    //        value--;
    //    }
    //}

    public void PlayAnim()
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
}
