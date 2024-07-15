using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
public class Boid : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Position;
    private Vector2 _velocity;
    public Rigidbody2D rb;
    [SerializeField] private MeshRenderer spawnArea;
    [SerializeField] private float steeringTime;
    private Vector2 Acceleration;
    private BoidSpawner boidSpawner;
    [SerializeField] private Transform target;
    private Boid[] boids;
    private List<Boid> BoidsList = new List<Boid>();
    private float maxForce = 0.7f;
    private float maxSpeed = 8f;

    private void Start()
    {
        spawnArea = GameObject.FindGameObjectWithTag("spawnarea").GetComponent<MeshRenderer>();

        rb = GetComponent<Rigidbody2D>();
        boidSpawner = GameObject.FindGameObjectWithTag("spawner").GetComponent<BoidSpawner>();
        target = GameObject.FindGameObjectWithTag("target").GetComponent<Transform>();

        rb.gravityScale = 0f;

        GameObject[] boids2Add = GameObject.FindGameObjectsWithTag("Boid");
        boids = new Boid[boids2Add.Length];
        for (int i = 0; i < boids2Add.Length; i++)
        {
            boids[i] = boids2Add[i].GetComponent<Boid>();
            BoidsList.Add(boids[i]);
        }

        // Initialize position, velocity, and acceleration
        Position = new Vector2(Random.Range(0f, Camera.main.GetHorizontalFieldOfView()),Random.Range(0f, Camera.main.GetVerticalFieldOfView(60)));
        //Position = new Vector2(Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y));
        //rb.velocity = new Vector2(Random.Range(-2f, 4f), Random.Range(-2f, 4f));
        Acceleration = Vector2.zero;
    }

    private void Update()
    {
        //velocity to change based on acceleration and position to change based on velocity
        rb.velocity += Acceleration * Time.deltaTime;
        rb.velocity = rb.velocity.normalized * maxSpeed;
        Position += new Vector2(rb.velocity.x, rb.velocity.y) * Time.deltaTime;
        transform.position = Position;


        Vector3 Velocity3D = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        transform.LookAt(transform.position + Velocity3D);

        Flock();
        //to keep the boids in the screen at all times
        //Edges();
    }

    private void FixedUpdate()
    {
        Vector2 Steeringvector = Seek();

        var expectedEndPosition = Position +
            Vector2.ClampMagnitude(Steeringvector + rb.velocity, maxSpeed);
        Vector2.SmoothDamp(Position, expectedEndPosition, ref _velocity, steeringTime);
        rb.velocity += _velocity;
    }

    private void Flock()
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
    }

    private Vector2 Align()
    {
        float perceptionRadius = 100f;
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
                steering += boid.rb.velocity;
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
            steering -= rb.velocity;
            //making it so that the steering vector will not exceed the force being applied
            steering = Vector2.ClampMagnitude(steering, maxForce);
        }
        return steering;
    }

    private Vector2 Cohesion()
    {
        float perceptionRadius = 100f;
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
            steering -= rb.velocity;
            //making it so that the steering vector will not exceed the force being applied
            steering = Vector2.ClampMagnitude(steering, maxForce);
        }
        //returning the vector at which the boid is steer towards
        return steering;
    }

    private Vector2 Separation()
    {
        float perceptionRadius = 20f;
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
            steering -= rb.velocity;
            //making it so that the steering vector will not exceed the force being applied
            steering = Vector2.ClampMagnitude(steering, maxForce);
        }
        //returning the vector at which the boid is steering towards
        return steering;
    }

    private Vector2 Seek()
    {
        var desiredVelocity = Vector2.ClampMagnitude(target.position - transform.position, maxSpeed);
        var steeringVector = desiredVelocity - rb.velocity;
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

    //[SerializeField] private Transform target;
    //[SerializeField] private float maxSpeed;
    //[SerializeField] private float steeringTime;
    //[SerializeField] private int lookAheadDist;
    //[SerializeField] private float avoidDistance;
    //[SerializeField] [Range(0, 1)] private float seekWeight;
    //[SerializeField] [Range(0, 1)] private float obstacleAvoidanceWeight;
    //private Rigidbody2D rb;
    //private Vector2 Velocity;


    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //}

    //private void FixedUpdate()
    //{
    //    Vector2 steeringVector = Seek() + ObstacleAvoidance();

    //    var expectedEndPos = transform.position + 
    //        Vector3.ClampMagnitude(rb.velocity + steeringVector, maxSpeed);

    //    Vector2.SmoothDamp(transform.position, expectedEndPos, ref Velocity, steeringTime);
    //    rb.velocity = Velocity;
    //    //converting rb.velocity to '3D' by just making the z = 0
    //    Vector3 Velocity3D = new Vector3(rb.velocity.x, rb.velocity.y, 0);
    //    transform.LookAt(transform.position + Velocity3D);
    //}

    //private Vector2 ObstacleAvoidance()
    //{
    //    //checking for collision
    //    RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, 2f, rb.velocity.normalized, lookAheadDist, LayerMask.GetMask("Obstacle"));
    //    var steeringVector = Vector2.zero;
    //    if (!hitInfo.collider)
    //    {
    //        return steeringVector;
    //    }

    //    Vector2 inDirection = new Vector2(-rb.velocity.y, rb.velocity.x);
    //    Vector2 newTransform = new Vector2(transform.position.x, transform.position.y); 

    //    var avoidDirection = Vector2.Perpendicular(inDirection).normalized;
    //    var targetPosition = hitInfo.point + avoidDirection * avoidDistance;
    //    var desiredVelocity = Vector2.ClampMagnitude(targetPosition - newTransform, maxSpeed);
    //    steeringVector = desiredVelocity - rb.velocity;
    //    Debug.DrawLine(-hitInfo.point, targetPosition, Color.green);
    //    return steeringVector * obstacleAvoidanceWeight;
    //}

    //private Vector2 Seek()
    //{
    //    var desiredVel = Vector2.ClampMagnitude(target.position - transform.position, maxSpeed);
    //    var steeringVector = desiredVel - rb.velocity;
    //    return steeringVector * seekWeight;
    //}
}
