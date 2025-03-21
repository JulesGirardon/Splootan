using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    private float currentTime = 0.0f;
    private bool timeRunning = true;
    private float lastAlarmIntervalle = 0f;
    private float intervalle = 15f; // intervalle en secondes
    public List<GameObject> alarmList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void FixedUpdate()
    {
        if (timeRunning)
        {
            currentTime += Time.deltaTime;

            Debug.Log(currentTime);
            Debug.Log(lastAlarmIntervalle);

            if (currentTime > lastAlarmIntervalle)
            {
                Debug.Log("Je suis ici !");
                lastAlarmIntervalle = currentTime + intervalle;
                SpawnRandomAlarm();
            }
        }
    }

    private void SpawnRandomAlarm()
    {
        if (alarmList != null && alarmList.Count > 0)
        {
            GameObject randomAlarmPrefab = alarmList[Random.Range(0, alarmList.Count)];
            BonusMalusManager.Instance.SpawnAlarm(randomAlarmPrefab);
        }
    }

    public float GetTimer()
    {
        return currentTime;
    }
}
