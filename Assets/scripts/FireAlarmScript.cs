using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;

public class FireAlarm : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    void OnParticleCollision(GameObject other)
    {
        PaintOnGeneratedTexture statue = other.GetComponent<PaintOnGeneratedTexture>();
        Debug.Log("ouais ya ddes collisions");
        foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
        {
            Debug.Log("ouais ya ddes evnets");
            statue.EraseAtUV(collisionEvent.intersection);
        }
        
    }

}

