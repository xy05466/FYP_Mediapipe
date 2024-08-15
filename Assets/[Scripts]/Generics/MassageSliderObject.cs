using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace SDM_FythmPipe
{
    public class MassageSliderObject : MonoBehaviour
    {
        [Header("Interacting object")]
        public GameObject targetObject;
        private SpriteRenderer rendererObject;
        public Color color_default = Color.white;
        public Color color_interacting = Color.green;
        public Color color_done = Color.gray;
        //public Sprite spr_default;
        //public Sprite spr_interacting;
        //public Sprite spr_done;

        public LineRenderer lr_outline;
        //public LineRenderer lr_filled;
        //private int fillCount = 0;
        public Vector3[] pathPoints = new Vector3[4] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        private int segmentCount = 10;
        List<float> percentageList = new List<float>();
        //public List<int> points = new List<int>();
        //private float sliderValue = 0f;
        //private int currentIndex = 0;

        //public Action<MassageSliderObjectV2, int, GameObject, Collider> OnColliding;

        private float sliderValue = 0f;
        private float targetValue = 0f;
        bool end = false;
        //bool move = false;
        public Action<MassageSliderObject> OnEnded;
        public Action<MassageSliderObject, float, float> OnSliderUpdate;

        private void Awake()
        {
            rendererObject = targetObject.GetComponent<SpriteRenderer>();
        }

        // Set up start end points
        public void SetUp()
        {
            lr_outline.positionCount = segmentCount;
            //lr_filled.positionCount = 0;

            for (int i = 0; i < segmentCount; i++)
            {
                float percentage = ((float)i / segmentCount);
                percentageList.Add(percentage);
                lr_outline.SetPosition(i, CalculateBezierPoint(percentage, pathPoints));
            }
        }
        public void SetPointPosition(int index, Vector3 pos)
        {
            pathPoints[index] = pos;
        }
        int tempCompIndex = 0;
        public void UpdatePosition()
        {
            // set the position for line renderer
            for (int i = 0; i < segmentCount; i++)
            {
                float percentage = ((float)i / segmentCount);
                lr_outline.SetPosition(i, CalculateBezierPoint(percentage, pathPoints) + Vector3.forward * -1f);
            }

            // set position for target
            Vector3 valuePosition = GetValuePosition();
            targetObject.transform.position = valuePosition + Vector3.forward * -1f;
        }
        // will move the slider
        public void UpdateSlider(GameObject compareObject, float distance = 1f, float speed = 1f, float firstEnterDistance = 0.05f)
        {
            if (end) return;


            float comp = ((float)(tempCompIndex + 1) / (float)lr_outline.positionCount);

            if (targetValue < comp)
            {
                if (Vector2.Distance(compareObject.transform.position, GetTargetPosition()) <= distance)
                {
                    OnSliderUpdate?.Invoke(this, targetValue, comp - targetValue);
                    targetValue = comp;
                }
            }
            else
            {
                tempCompIndex++;
            }

            // if completed
            if (targetValue >= 1f)
            {
                end = true;
                Debug.Log("ending");
                rendererObject.color = color_done;
                OnEnded?.Invoke(this);
            }

            sliderValue = Mathf.Lerp(sliderValue, targetValue, 5f * Time.deltaTime);


            //// Set fill position base on outline position
            //for (int i = 0; i < lr_filled.positionCount; i++)
            //    lr_filled.SetPosition(i, lr_outline.GetPosition(i));

        }

        public Vector3 GetValuePosition()
        {
            // This is for bezier path
            return CalculateBezierPoint(sliderValue, pathPoints);
        }

        public Vector3 GetTargetPosition()
        {
            // This is for bezier path
            return CalculateBezierPoint(targetValue, pathPoints);
        }
        public void ResetSlider()
        {
            sliderValue = 0f;
            targetValue = 0f;
            end = false;
            //lr_filled.positionCount = 0;
            rendererObject.color = color_default;
            tempCompIndex = 0;
        }
        public float GetValue()
        {
            return sliderValue;
        }

        // Calculated bezier path, done by Bryant Toh
        public Vector3 CalculateBezierPoint(float t, Vector3[] points)
        {
            Vector3 p0 = points[0];
            Vector3 p1 = points[1];
            Vector3 p2 = points[2];
            Vector3 p3 = points[3];

            //calculations for each coefficient of the formula
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            //implementation bezier curve formula
            Vector3 p = uuu * p0; // (1-t)^3 * p0
            p += 3 * uu * t * p1; // 3(1-t)^2 * t * p1
            p += 3 * u * tt * p2; // 3(1-t) * t^2 * p2
            p += ttt * p3; // t^3 * p3

            return p;
        }

        public void SubscribeEvent_Ended(Action<MassageSliderObject> action)
        {
            OnEnded += action;
        }
        public void SubscribeEvent_Update(Action<MassageSliderObject, float, float> action)
        {
            OnSliderUpdate += action;
        }



#if UNITY_EDITOR
        [CustomEditor(typeof(MassageSliderObject))]
        public class MassageSliderObjectEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (GUILayout.Button("Generate Curve"))
                {
                    MassageSliderObject massageSliderObject = (MassageSliderObject)target;
                    massageSliderObject.SetUp();
                    massageSliderObject.UpdatePosition();
                }
            }
        }
#endif
    }
}