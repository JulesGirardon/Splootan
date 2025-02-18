using UnityEngine;

public class BonusMalusManager : MonoBehaviour
{
    private float nbParticles = 5000;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SpawnAlarm(GameObject Alarm)
    {
        ParticleSystem Water = Alarm.AddComponent<ParticleSystem>();

        // Main Module
        var main = Water.main;
        main.duration = 5f;
        main.startLifetime = 2f;
        main.startSize = 0.1f;
        main.startColor = new ParticleSystem.MinMaxGradient(new Color32(160, 155, 240, 255));
        main.maxParticles = (int)(nbParticles * main.startLifetime.constant);

        // Emission Module
        var emission = Water.emission;
        emission.rateOverTime = nbParticles;

        // Shape Module
        var shape = Water.shape;
        shape.angle = 20f;

        // Collision Module
        var collision = Water.collision;
        collision.type = ParticleSystemCollisionType.World;
        collision.bounce = 0f;
        collision.maxKillSpeed = 0f;
        collision.collidesWith = LayerMask.GetMask("Statues");
        collision.sendCollisionMessages = true;

        // Renderer Module
        var renderer = Water.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Mesh;
        renderer.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");

        Water.Play();
        Alarm.AddComponent<DestroyVFXScript>();
    }
}
