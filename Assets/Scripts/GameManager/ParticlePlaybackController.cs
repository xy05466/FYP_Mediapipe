using UnityEngine;

public class ParticlePlaybackController : MonoBehaviour
{
    public ParticleSystem currParticleSystem;
    public float customTime;

    void Start()
    {
        // Ensure that the particle system is assigned
        if (currParticleSystem == null)
        {
            Debug.LogError("Particle System is not assigned!");
            return;
        }

        // Calculate the total duration of the particle system
        float particleDuration = CalculateParticleDuration(currParticleSystem);

        // Calculate the playback speed based on the custom set time
        float playbackSpeed = particleDuration / customTime;

        // Adjust the playback speed of the particle system
        //currParticleSystem.playbackSpeed = playbackSpeed;
        var main = currParticleSystem.main;
        main.simulationSpeed = playbackSpeed;

        // Start playing the particle system
        currParticleSystem.Play();
    }

    float CalculateParticleDuration(ParticleSystem ps)
    {
        // Get the main module of the particle system
        var main = ps.main;

        // Calculate the total duration based on start lifetime and duration
        float totalDuration = main.startLifetime.constant + main.duration;

        return totalDuration;
    }
}