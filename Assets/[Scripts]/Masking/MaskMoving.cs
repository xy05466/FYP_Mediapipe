using System.Collections.Generic;
using UnityEngine;

namespace SDM_FythmPipe
{
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
            if (animator != null)
                animator.enabled = false;
         
            if (bonePoints.Count > 0)
            {
                transform.position = bonePoints[0].position;
            }
            currPos = transform.position;
        }

        public void PercentageMovement(float sliderValue)
        {
            if (_masks.Count >= bonePoints.Count * _numMasksPerBone || bonePoints.Count < 0) return;

            int currentBoneIndex = (int)((bonePoints.Count - 1) * sliderValue)+1;
            if (currentBoneIndex >= bonePoints.Count ) return;
            transform.position = Vector3.Lerp(transform.position, bonePoints[currentBoneIndex].position, Time.deltaTime / bonePoints.Count);
            if(Vector2.Distance(transform.position,currPos) > 0.4f)
            {
                GameObject mask = Instantiate(_maskPrefab);
                currPos = transform.position;
                mask.transform.position = transform.position;
                _masks.Add(mask);
            }
        }

        // Play animation
        public void PlayAnim()
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            canScale = true;
            if (animator != null)
            {
                animator.enabled = true;
            }
        }
    }

}