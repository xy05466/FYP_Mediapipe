using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
public class Boid : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Position;
    private Vector2 Velocity;
    [SerializeField] private float steeringTime;
    private Vector2 Acceleration;
    private BoidSpawner boidSpawner;
    [SerializeField] private Transform target;
    private Boid[] boids;
    private List<Boid> BoidsList = new List<Boid>();
    private float maxForce = 0.2f;
    private float maxSpeed = 4f;

    private void Start()
    {
        boidSpawner = GameObject.FindGameObjectWithTag("spawner").GetComponent<BoidSpawner>();
        target = GameObject.FindGameObjectWithTag("target").GetComponent<Transform>();

        GameObject[] boids2Add = GameObject.FindGameObjectsWithTag("Boid");
        boids = new Boid[boids2Add.Length];
        for (int i = 0; i < boids2Add.Length; i++)
        {
            boids[i] = boids2Add[i].GetComponent<Boid>();
            BoidsList.Add(boids[i]);
        }

        // Initialize position, velocity, and acceleration
        Position = new Vector2(Random.Range(0f, Camera.main.GetHorizontalFieldOfView()),Random.Range(0f, Camera.main.GetVerticalFieldOfView(60)));
        Velocity = new Vector2(Random.Range(-2f, 4f), Random.Range(-2f, 4f));
        Acceleration = Vector2.zero;
    }

    private void Update()
    {
        //velocity to change based on acceleration and position to change based on velocity
        Velocity += Acceleration * Time.deltaTime;
        Velocity = Velocity.normalized * maxSpeed;
        Position += new Vector2(Velocity.x, Velocity.y) * Time.deltaTime;
        transform.position = Position;

        //Flock();
        //to keep the boids in the screen at all times
        Edges();
    }

    private void FixedUpdate()
    {
        Vector2 flockingVector = Flock();
        Vector2 Steeringvector = Seek();
        var expectedEndPosition = Position +
            Vector2.ClampMagnitude(Steeringvector + flockingVector, maxSpeed);
        Vector2.SmoothDamp(Position, expectedEndPosition, ref Velocity, steeringTime);

        Velocity += Acceleration * Time.deltaTime;
        Position += new Vector2(Velocity.x, Velocity.y) * Time.deltaTime;
        transform.position = Position;

        float angle = Mathf.Atan2(expectedEndPosition.y, expectedEndPosition.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, angle);
    }

    private Vector2 Flock()
    {
        //basically calls every boid in the scene to find the nearest partner and align themselves with it
        Vector2 alignment = Align();
        Vector2 cohesion = Cohesion();
        Vector2 separation = Separation();
        separation *= boidSpawner.separationSlider;
        cohesion *= boidSpawner.cohesionSlider;
        alignment *= boidSpawner.alignmentSlider;
        Acceleration += alignment;
        Acceleration += cohesion;
        Acceleration += separation;
        Vector2 flockingVector = Acceleration;
        //Vector2 flockingVector = alignment + cohesion + separation;
        return flockingVector;
    }

    private Vector2 Align()
    {
        float perceptionRadius = 50f;
        Vector2 steering = Vector2.zero;
        int total = 0;

        //for each boid that is in the boidGO list
        foreach (Boid boid in BoidsList)
        {
            //distance between this boid and whichever boid that is in the list
            float dist = Vector2.Distance(Position, boid.Position);
            if (dist < perceptionRadius && boid.GetInstanceID() != GetInstanceID())
            {
                //adding all the positions of the boids in the list
                steering += boid.Velocity;
                total++;
            }
        }

        if (total > 0)
        {
            //calculating the average velocity for each boid
            steering /= total;
            //making it so that the steering vector will not exceed the maxspeed without changing the direction
            steering = steering.normalized * maxSpeed;
            //calculating the steering force required to adjust the current velocity to the desired velocity
            steering -= Velocity;
            //making it so that the steering vector will not exceed the force being applied
            steering = Vector2.ClampMagnitude(steering, maxForce);
        }
        return steering;
    }

    private Vector2 Cohesion()
    {
        float perceptionRadius = 50f;
        Vector2 steering = Vector2.zero;
        int total = 0;

        //for each boid that is in the boidGO list
        foreach (Boid boid in BoidsList)
        {
            //distance between this boid and whichever boid that is in the list
            float dist = Vector2.Distance(Position, boid.Position);
            if (dist < perceptionRadius && boid.GetInstanceID() != GetInstanceID())
            {
                //adding all the positions of the boids in the list
                steering += boid.Position;
                total++;
            }
        }

        if (total > 0)
        {
            //calculating the average velocity for each boid
            steering /= total;
            //current boid to steer towards the direction of the average position
            steering -= Position;
            //making it so that the steering vector will not exceed the maxspeed
            steering = steering.normalized * maxSpeed;
            //calculating the steering force required
            steering -= Velocity;
            //making it so that the steering vector will not exceed the force being applied
            steering = Vector2.ClampMagnitude(steering, maxForce);
        }
        //returning the vector at which the boid is steer towards
        return steering;
    }

    private Vector2 Separation()
    {
        float perceptionRadius = 10f;
        Vector2 steering = Vector2.zero;
        int total = 0;

        //for each boid that is in the boidGO list
        foreach (Boid boid in BoidsList)
        {
            //distance between this boid and whichever boid that is in the list
            float dist = Vector2.Distance(Position, boid.Position);
            if (dist < perceptionRadius && boid.GetInstanceID() != GetInstanceID())
            {
                //getting the direction from position to targetted boid's position
                Vector2 Diff = Position - boid.Position;
                Diff /= Mathf.Pow(dist, 2);
                //adding all the positions of the boids in the list
                steering += Diff;
                //total number of velocities being added
                total++;
            }
        }

        if (total > 0)
        {
            //calculating the average velocity
            steering /= total;
            //making it so that the steering vector will not exceed the maxspeed
            steering = steering.normalized * maxSpeed;
            //calculating the steering force required
            steering -= Velocity;
            //making it so that the steering vector will not exceed the force being applied
            steering = Vector2.ClampMagnitude(steering, maxForce);
        }
        //returning the vector at which the boid is steering towards
        return steering;
    }

    private Vector2 Seek()
    {
        Vector3 newPos = new Vector3(Position.x, Position.y, 0f);
        var desiredVelocity = Vector2.ClampMagnitude(target.position - newPos, maxSpeed);
        var steeringVector = desiredVelocity - Velocity;
        return steeringVector * boidSpawner.SeekWeight;
    }

    private void Edges()
    {
        if (Position.x > Camera.main.GetHorizontalFieldOfView()) 
            Position.x = 0;
        else if (Position.x < 0) 
            Position.x = Camera.main.GetHorizontalFieldOfView();

        if (Position.y > Camera.main.GetVerticalFieldOfView(60)) 
            Position.y = 0;
        else if (Position.y < 0) 
            Position.y = Camera.main.GetVerticalFieldOfView(60);
    }

}
