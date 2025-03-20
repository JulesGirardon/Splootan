using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEngine;

public class FireAlarm : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject Statue;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        PaintOnGeneratedTexture statue = other.GetComponent<PaintOnGeneratedTexture>();
        if (statue == null)
            return;

        Debug.Log("Collision detected with: " + other.name);

        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        Debug.Log("Number of collision events: " + numCollisionEvents);

        Collider col = other.GetComponent<Collider>();
        if (col == null)
            return;

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Debug.Log("Processing collision event " + i);
            Vector3 direction = (
                other.transform.position - collisionEvents[i].intersection
            ).normalized;
            Ray ray = new Ray(collisionEvents[i].intersection + direction * 0.01f, direction);
            if (col.Raycast(ray, out RaycastHit hit, 10f))
            {
                Vector3 vtx = hit.collider.transform.InverseTransformPoint(hit.point);
                statue.EraseAtVertex(vtx);
            }
            else
            {
                Debug.LogWarning("Raycast did not hit collider for UV conversion");
            }
        }
    }
}
