using UnityEngine;

public class BonusMalusManager : MonoBehaviour
{
    float nbParticles = 5000;
    private static BonusMalusManager _instance;

    public static BonusMalusManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("BonusMalusManager is null !!!");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// Spawns an alarm effect using the provided Alarm GameObject and aligns it based on the angle between this object and the given statue.
    /// </summary>
    public void SpawnAlarm(GameObject Alarm)
    {
        if (Alarm == null)
        {
            Debug.LogError("SpawnAlarm: Alarm is null!");
            return;
        }

        ParticleSystem Water = Alarm.GetComponent<ParticleSystem>();
        if (Water == null)
        {
            Debug.LogError("SpawnAlarm: No ParticleSystem found on Alarm!");
            return;
        }

        Water.Play();
    }


}
