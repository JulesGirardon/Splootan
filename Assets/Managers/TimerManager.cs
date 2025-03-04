using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    private float currentTime = 0.0f;
    private bool timeRunning = false;
    private float limitTime;
    private GameObject statueRef;
    private int lastAlarmMinute = -1;
    private List<GameObject> alarmList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {

        if (timeRunning)
        {
            currentTime += Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0.0f, limitTime);

            int currentMinute = Mathf.FloorToInt(currentTime / 60);

            Debug.Log(currentMinute);
            Debug.Log(lastAlarmMinute);

            if (currentMinute > lastAlarmMinute && currentMinute < limitTime / 60)
            {
                Debug.Log("Je suis ici !");
                lastAlarmMinute = currentMinute;
                SpawnRandomAlarm();
            }
            
            if (currentTime >= limitTime)
            {
                timeRunning = false;
                currentTime = 0.0f;
                Debug.Log("Timer terminé !");
            }
        }
    }

    public void StartTimer(float duration, GameObject statue, List<GameObject> alarms)
    {
        limitTime = duration;
        timeRunning = true;
        statueRef = statue;
        alarmList = alarms;
    }

    private void SpawnRandomAlarm()
    {
        if (alarmList != null && alarmList.Count > 0)
        {
            GameObject randomAlarmPrefab = alarmList[Random.Range(0, alarmList.Count)];
            BonusMalusManager.Instance.SpawnAlarm(randomAlarmPrefab);
        }
    }

    public float GetTimeRemaining()
    {
        return limitTime - currentTime;
    }

    public float GetTimer()
    {
        return currentTime;
    }
}
