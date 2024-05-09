using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public PathCreator pathCreator;
    public Path path;
    public float speed = 5f;
    public bool isColliding;
    private Vector2[] points;
    private int currentIndex = 0;

    void Start()
    {
        path = pathCreator.GetComponent<PathCreator>().path;
        points = path.CalculateEvenlySpacedPoints(0.1f); // Adjust spacing as needed
    }

    void Update()
    {
        if (currentIndex < points.Length)
        {
            // Move the ball towards the current point
            transform.position = Vector2.MoveTowards(transform.position, points[currentIndex], speed * Time.deltaTime);

            // Check if the ball has reached the current point
            if (Vector2.Distance(transform.position, points[currentIndex]) < 0.01f)
            {
                currentIndex++; // Move to the next point
            }
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("YourCollisionTag"))
    //    {
    //        isColliding = true; // Set collision flag to true when collision occurs
    //    }
    //}

    //void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("YourCollisionTag"))
    //    {
    //        isColliding = false; // Set collision flag to false when collision ends
    //    }
    //}
}
