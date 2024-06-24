using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_Essence : PageLogic_Extension
{
    public FacepointManager_Jaden facepointManager_Jaden;
    public AccupointsMassageManagerFinal accupointsMassageManagerFinal;

    public BallMassageObject ballMassageObject;

    public List<int> points = new List<int>();
    private int currentIndex = 0;

    [Header("TESTING point")]
    public LineRenderer lr;
    public LineRenderer lr2;
    public Texture testSprite;
    public int positionCount =1; // should start at 1
    public Button bu_fill;
    Vector3 lerpPos = Vector3.zero;
    public float sliderfloat = 0.5f;
    //lr.GetPosition(i-1) / 
    public override void StartPage()
    {
        //accupointsMassageManagerFinal?.StartFunc();

        bu_fill.onClick.AddListener(Fill);
        //lr.materials[0].SetTexture("_MainTex", testSprite);
        lr.positionCount = points.Count;
        lr2.positionCount = positionCount;

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        UpdateColliders();
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (facepointsObj == null || facepointsObj.Count <= 0) return;
        //if (accupointsMassageManagerFinal.gameObject.activeInHierarchy)
        //    accupointsMassageManagerFinal.UpdateScene();
        lr.positionCount = points.Count;
        lr2.positionCount = currentIndex + 2;
        for (int i = 0; i < points.Count; i++)
        {
            int p = points[i];
            lr.SetPosition(i, facepointsObj[p].transform.position);
        }
        if(currentIndex < lr.positionCount)
        ballMassageObject.transform.position = lr.GetPosition(currentIndex);
        // set collider position
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            PositionColliderBetweenPoints(i,lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
        }

        //if(currentIndex > 2)
        //{
        //    Vector3 startPosition = lr.GetPosition(0);
        //    Vector3 endPosition = lr.GetPosition(1);

        //    // direction of vector from end to start
        //    Vector3 direction = endPosition - startPosition;

        //    // Calculate percentage of vector
        //    Vector3 percentagePosition = startPosition + direction * sliderfloat;
        //    // Set positions in lr2
        //    lr2.SetPosition(0, startPosition);
        //    lr2.SetPosition(1, percentagePosition);
        //}

        //fill line renderer
        for (int i = 0; i < lr2.positionCount; i++)
        {
            if(i== lr2.positionCount - 1)
            {
                Vector3 startPosition = lr.GetPosition(i-1);
                Vector3 endPosition = lr.GetPosition(i);

                // direction of vector from end to start
                Vector3 direction = endPosition - startPosition;

                // Calculate percentage of vector
                Vector3 percentagePosition = startPosition + direction * sliderfloat;
                // Set positions in lr2
                lr2.SetPosition(i, percentagePosition);
            }
            else
            {
                lr2.SetPosition(i, lr.GetPosition(i));
            }

        }

    }
    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
        if(currentIndex >=points.Count) return;
        //if (points[currentIndex] == facePointIndex)
        //{
        //    currentIndex++;
        //    sliderfloat = 0;
        //}
        //if (currentIndex > 0 && points[currentIndex - 1] == facePointIndex)
        //{
        //    sliderfloat += Time.deltaTime;
        //    sliderfloat = Mathf.Clamp(sliderfloat, 0.0f, 1.0f);
        //}
        
        // if collide with current facepoint
        if (CollideWithFacePoint(points[currentIndex],facePointIndex))
        {
            //FillCurrentPoint();
        }
    }
    void FillCurrentPoint(float fill)
    {
        if (currentIndex >=points.Count) return;

        // to do:  Utilize fill variable

        sliderfloat += Time.deltaTime;
        sliderfloat = Mathf.Clamp(sliderfloat, 0.0f, 1.0f);
        if (sliderfloat >= 1)
            NextPoint();
    }
    void NextPoint()
    {
        sliderfloat = 0;
        currentIndex++;
    }
    public override void EndPage()
    {
    }
    public override void ResetPage()
    {
    }

    public override bool IsPageDone()
    {
        //return accupointsMassageManagerFinal.IsDone();
        return false;
    }

    public override void OnFaceAnnotationActive(bool active)
    {
        //accupointsMassageManagerFinal.gameObject.SetActive(active);
    }

    public override void OnHandAnnotationActive(bool active)
    {
    }


    public void Fill()
    {
        positionCount++;
        Debug.Log(positionCount);
        if (positionCount > points.Count)
        {
            positionCount = points.Count;
        }
        lr2.positionCount = positionCount;
        lr2.SetPosition(positionCount - 1,lr2.GetPosition(positionCount-2));
        lerpPos = lr2.GetPosition(positionCount - 2);
    }

    public LineRenderer lineRenderer;
    public float colliderWidth = 0.1f;


    public void UpdateColliders()
    {
        // Remove old colliders
        foreach (var collider in GetComponents<BoxCollider>())
        {
            Destroy(collider);
        }

        // Create new colliders
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            CreateColliderBetweenPoints(i,lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
        }
    }
    List<BoxCollider> colliders = new List<BoxCollider>();
    private void CreateColliderBetweenPoints(int id,Vector3 start, Vector3 end)
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
    private void PositionColliderBetweenPoints(int index,Vector3 start, Vector3 end)
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
        if (currentIndex >= points.Count) return;
        if (other.gameObject.tag == "Handpoint" && id == currentIndex)
        {
            Vector3 startPoint = lr.GetPosition(currentIndex);
            Vector3 endpoint = lr.GetPosition(currentIndex + 1);
            // check 
            Debug.Log("startPoint "+ startPoint);
            Debug.Log("Closest point :" +other.ClosestPoint(goObject.transform.position));
            Debug.Log("lr current point :" +lr2.GetPosition(lr2.positionCount-1));
            Debug.Log("endpoint " + endpoint);

            // Calculate the total distance
            float totalDistance = Vector3.Distance(startPoint, lr2.GetPosition(lr2.positionCount - 1));

            // Calculate the distance from start to the check position
            float checkDistance = Vector3.Distance(startPoint, other.gameObject.transform.position);

            Debug.Log("totalDistance " + totalDistance);
            Debug.Log("checkDistance " + checkDistance);

            if(checkDistance > totalDistance)
                FillCurrentPoint(1);
        }
    }

}
