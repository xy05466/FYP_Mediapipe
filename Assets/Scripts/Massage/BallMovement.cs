using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public JadenCurveManager path;

    public bool isColliding;
    private Vector2[] points;
    private int currentIndex = 0;

    float elapsed = 0f;
    public float duration = 0.5f;
    //time pressed
    public delegate void OnComplete();
    // Declare a delegate variable
    public static event OnComplete onComplete;
    public bool reachEnd;
    public GameObject flowerPrefab;
    List<FlowerSpawn> flowerSpawnIndex;
    private void Start()
    {
        //points = path.CalculateEvenlySpacedPoints(0.3f);
        SilderSet.resetBallPos += reset;
    }
    void Update()
    {
        points = path.CalculateEvenlySpacedPoints(0.3f);
        flowersToSpawn();
        //for(var pointsInCount = 0; points.Length < pointsInCount )
        if (!isColliding)
            return;
        //else if(points == null)
        elapsed += Time.deltaTime;
        if (currentIndex < points.Length - 1)
        {
            Vector3 start = new Vector3(points[currentIndex].x, points[currentIndex].y, 0f);
            Vector3 end = new Vector3(points[currentIndex + 1].x, points[currentIndex + 1].y, 0f);

            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            // Move the ball towards the current point
            //transform.position = Vector3.Lerp(points[currentIndex], points[currentIndex + 1], elapsed / duration);
            //move on to next point
            if (elapsed >= duration)
            {
                currentIndex++;
                elapsed = 0;
            }
        }
        if (currentIndex >= points.Length - 1 && reachEnd == false)
        {
            reachEnd = true;
            onComplete?.Invoke();
        }
        for (int numFlower2Spawn = 0; numFlower2Spawn < flowerSpawnIndex.Count; numFlower2Spawn++)
        {
            //accoutn for if flower posindex is greater tahn currentindex
            //this is due to the player being able to move back and forth
            //this will prevent out of range errors
            if (flowerSpawnIndex[numFlower2Spawn].posIndex > currentIndex)
                return;
            if(currentIndex == flowerSpawnIndex[numFlower2Spawn].posIndex)
            {
                if (flowerSpawnIndex[numFlower2Spawn].spawned)
                    return;
                //spawn flower
                GameObject newFlower = Instantiate(flowerPrefab);
                newFlower.transform.position = points[currentIndex];
                newFlower.transform.localPosition = newFlower.transform.localPosition.WithZ(0f);
            }
        }
        //transform.localPosition.WithZ(0f);
        Vector3 currentLocalPosition;
        currentLocalPosition = transform.localPosition;
        transform.localPosition = currentLocalPosition.WithZ(0f);

    }
    //handle collision
    private void OnTriggerStay(Collider other)
    {
        isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;

    }
    public void reset()
    {
        isColliding = false;
        reachEnd = false;
        currentIndex = 0;
        transform.localPosition = Vector3.zero;
    }
    //handle how many flowers to spawn
    //only call this code once
    public void flowersToSpawn()
    {
        int numOfPoints = points.Length;
        //Debug.Log(numOfPoints);
        //if greater tahn 65 points, spawn 3 points instead
        if(numOfPoints > 65)
        {
            //spawn 3 points
            FlowerUpdate(3, numOfPoints);
        }
        else
        {
            //spawn 2 points
            FlowerUpdate(2, numOfPoints);

        }
    }
    public void FlowerUpdate(int numOfFlowers, int numOfPoints)
    {
        int positionOfPoints = numOfPoints / numOfFlowers + 1;
        for (var index = 0; index < numOfFlowers; index++)
        {
            if (flowerSpawnIndex[index].spawned)
                return;
            if (!flowerSpawnIndex[index].init)
            {
                FlowerSpawn flowerSpawn = new FlowerSpawn();
                flowerSpawnIndex.Add(flowerSpawn);
                flowerSpawnIndex[index].init = true;
            }
            //spawnpoints will be position of points * index
            flowerSpawnIndex[index].posIndex = positionOfPoints * index + 1;


        }
    }
}
public class FlowerSpawn
{
    public bool init = false;
    public bool spawned = false;
    public int posIndex;
}
