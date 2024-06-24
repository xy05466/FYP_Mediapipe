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

    public List<BoxCollider> colliders = new List<BoxCollider>();
    public float colliderWidth = 0.1f;
    public Action<int,GameObject,Collider> OnColliding;
    public void UpdateColliders()
    {
        // Remove old colliders
        foreach (var collider in GetComponents<BoxCollider>())
        {
            Destroy(collider);
        }

        // Create new colliders
        for (int i = 0; i < lr_outline.positionCount - 1; i++)
        {
            CreateColliderBetweenPoints(i, lr_outline.GetPosition(i), lr_outline.GetPosition(i + 1));
        }
    }
    public void CreateColliderBetweenPoints(int id, Vector3 start, Vector3 end)
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
    void CollideWithLine(int id, GameObject goObject, Collider other)
    {
        OnColliding?.Invoke(id,goObject,other);
    }
    public void Subscribe(Action<int,GameObject, Collider> action)
    {
        OnColliding += action;
    }
}
