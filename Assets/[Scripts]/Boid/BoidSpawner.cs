using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField] private Boid Boids;
    [Range(0, 1)] public float alignmentSlider;
    [Range(0, 1)] public float cohesionSlider;
    [Range(0, 1)] public float separationSlider;
    [Range(0, 1)] public float SeekWeight;

    // Start is called before the first frame update
    void Start()
    {
        Show();
    }

    private void Show()
    {
        for (int i = 0; i < 40; i++)
        {
            Instantiate(Boids, Boids.Position, Quaternion.identity);
        }
    }
}
