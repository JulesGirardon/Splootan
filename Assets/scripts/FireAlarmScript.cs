using UnityEngine;

public class FireAlarm : MonoBehaviour
{
    public ParticleSystem waterEffect;
    public float cleaningDuration = 5f;
    public float cleaningRate = 0.02f;

    private bool isActive = false;

    void Start()
    {
        if (waterEffect != null)
        {
            waterEffect.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            isActive = true;
            if (waterEffect != null)
            {
                waterEffect.Play(); 
            }

            PaintOnGeneratedTexture[] paintedObjects = FindObjectsOfType<PaintOnGeneratedTexture>();
            foreach (var obj in paintedObjects)
            {
                StartCoroutine(obj.CleanPaint(cleaningDuration, cleaningRate));
            }
        }
    }
}
