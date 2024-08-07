using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class MaskMoving : MonoBehaviour
{
    [SerializeField] private GameObject _maskPrefab;
    [SerializeField] private int _numMasksPerBone = 1; // the higher the value, the smaller the masks, but more masks are spawned
    private List<GameObject> _masks = new List<GameObject>();

    private Vector3 currPos;
    public List<Transform> bonePoints = new List<Transform>();
    public SpriteRenderer spriteRenderer;
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
        gameObject.SetActive(false);
        currPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PercentageMovement(float sliderValue)
    {
        if (_masks.Count >= bonePoints.Count * _numMasksPerBone) return;

        int currentBoneIndex = (int)((bonePoints.Count - 1) * sliderValue)+1;
        if (currentBoneIndex >= bonePoints.Count ) return;
        gameObject.SetActive(true);
        transform.position = Vector3.Lerp(transform.position, bonePoints[currentBoneIndex].position, Time.deltaTime / bonePoints.Count);
        if(Vector2.Distance(transform.position,currPos) > 0.35f)
        {
            GameObject mask = Instantiate(_maskPrefab);
            currPos = transform.position;
            mask.transform.position = transform.position;
            _masks.Add(mask);
        }
    }

    public void PlayAnim()
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        gameObject.SetActive(false);
        canScale = true;
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
}
