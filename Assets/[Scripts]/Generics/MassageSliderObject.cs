using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MassageSliderObject : MonoBehaviour
{
    public LineRenderer lr_outline;
    public LineRenderer lr_filled;
    public List<int> points = new List<int>();
    private float sliderValue = 0f;
    private int currentIndex = 0;

    public List<BoxCollider> colliders = new List<BoxCollider>();
    public float colliderWidth = 0.1f;
    public Action<MassageSliderObject,int, GameObject,Collider> OnColliding;
    private float sliderfloat = 0f;

    public void Init()
    {
       lr_outline.positionCount = points.Count;
       lr_filled.positionCount = currentIndex + 2;
        UpdateColliders();
    }
    public void UpdateSlider(List<GameObject> gameObjects)
    {
        lr_outline.positionCount = points.Count;
        lr_filled.positionCount = currentIndex + 2;


        for (int i = 0; i < points.Count; i++)
        {
            int p = points[i];
            lr_outline.SetPosition(i, gameObjects[p].transform.position);
        }
        if (currentIndex < lr_outline.positionCount)
        {
            // set collider position
            for (int i = 0; i < lr_outline.positionCount - 1; i++)
            {
                PositionColliderBetweenPoints(i, lr_outline.GetPosition(i), lr_outline.GetPosition(i + 1));
            }

            //fill line renderer
            for (int i = 0; i < lr_filled.positionCount; i++)
            {
                if (i == lr_filled.positionCount - 1)
                {
                    Vector3 startPosition = lr_outline.GetPosition(i - 1);
                    Vector3 endPosition = lr_outline.GetPosition(i);

                    // direction of vector from end to start
                    Vector3 direction = endPosition - startPosition;

                    // Calculate percentage of vector
                    Vector3 percentagePosition = startPosition + direction * sliderfloat;
                    // Set positions in lr2
                    lr_filled.SetPosition(i, percentagePosition);
                }
                else
                {   
                    lr_filled.SetPosition(i, lr_outline.GetPosition(i));
                }
            }
        }
    }
    // Update colliders, remove current colliders and create new one
    void UpdateColliders()
    {
        // Remove old colliders
        foreach (var collider in colliders)
        {
            Destroy(collider);
        }
        colliders = new List<BoxCollider>();

        // Create new colliders
        for (int i = 0; i < lr_outline.positionCount - 1; i++)
            CreateColliderBetweenPoints(i, lr_outline.GetPosition(i), lr_outline.GetPosition(i + 1));
    }
    // Create colliders
    void CreateColliderBetweenPoints(int id, Vector3 start, Vector3 end)
    {
        GameObject colliderObject = new GameObject("LineSegmentCollider");
        colliderObject.transform.parent = this.transform;
        LineCollisionResponse lcr = colliderObject.AddComponent<LineCollisionResponse>();
        lcr.SetId(id);
        lcr.SetIgnoreName("LineSegmentCollider");
        lcr.Subscribe(CollideWithLine);

        BoxCollider boxCollider = colliderObject.AddComponent<BoxCollider>();
        colliders.Add(boxCollider);
        boxCollider.isTrigger = true;

        // Position the collider
        Vector3 middlePoint = (start + end) / 2;
        boxCollider.transform.position = middlePoint;

        // Rotate the collider to align with the line segment
        Vector3 direction = end - start;
        boxCollider.transform.rotation = Quaternion.LookRotation(direction);

        // Scale the collider to match the line segment
        boxCollider.size = new Vector3(colliderWidth, colliderWidth, direction.magnitude);
    }
    // position collider points
    public void PositionColliderBetweenPoints(int index, Vector3 start, Vector3 end)
    {
        BoxCollider boxCollider = colliders[index];

        // Position the collider
        Vector3 middlePoint = (start + end) / 2;
        boxCollider.transform.position = middlePoint;

        // Rotate the collider to align with the line segment
        Vector3 direction = end - start;
        boxCollider.transform.rotation = Quaternion.LookRotation(direction);

        // Scale the collider to match the line segment
        boxCollider.size = new Vector3(colliderWidth, colliderWidth, direction.magnitude);
    }
    // when colliding with line
    void CollideWithLine(int id, GameObject goObject, Collider other)
    {
        OnColliding?.Invoke(this,id,goObject,other);
    }
    internal int GetCurrentIndex()
    {
        return currentIndex;
    }
    public void FillCurrentPoint(float speed = 1)
    {
        if (GetCurrentIndex() >= points.Count) return;

        // to do:  Utilize fill variable

        sliderfloat += Time.deltaTime * speed;
        sliderfloat = Mathf.Clamp(sliderfloat, 0.0f, 1.0f);
        //sliderfloat = fill;
        if (sliderfloat >= 1)
            NextPoint();
    }
    void NextPoint()
    {
        sliderfloat = 0;
        currentIndex++;
    }
    public void Subscribe(Action<MassageSliderObject,int, GameObject, Collider> action)
    {
        OnColliding += action;
    }

}
