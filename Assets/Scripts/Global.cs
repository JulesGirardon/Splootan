using System;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int musicVolume = 50;
    public static int sfxVolume = 50;

    public static bool haptic = false;

    public static GameObject[] allStatues;
    public static List<GameObject> activeStatues;

    public static Dictionary<string, Vector3> statueScales = new Dictionary<string, Vector3>()
    {
        { "base", new Vector3(0.12f, 0.12f, 0.12f) },
        { "companioncube", new Vector3(0.33f, 0.33f, 0.33f) },
        { "duck", new Vector3(0.33f, 0.33f, 0.33f) },
        { "ImAiFairy", new Vector3(0.1342956f, 0.1342956f, 0.1342956f) },
        { "MayanStatue", new Vector3(0.233828f, 0.233828f, 0.233828f) },
        { "pot", new Vector3(0.33f, 0.33f, 0.33f) },
        { "UwU", new Vector3(0.1453455f, 0.1453455f, 0.1453455f) },
    };

    public static Dictionary<string, Vector3> statueLocation = new Dictionary<string, Vector3>()
    {
        { "base", new Vector3(0, 0f, 0.0015f) },
        { "companioncube", new Vector3(0, 0, 0.00464f) },
        { "duck", new Vector3(0, 0, 0.00275f) },
        { "ImAiFairy", new Vector3(0, 0, 0.001249999f) },
        { "MayanStatue", new Vector3(0, 0, 0.01211f) },
        { "pot", new Vector3(0, 0, 0.0014f) },
        { "UwU", new Vector3(0, 0, 0.00123f) },
    };

    public static void LoadStatues()
    {
        allStatues = Resources.LoadAll<GameObject>("Statues");
        Debug.Log("Statues chargées : " + allStatues.Length);

        activeStatues = new List<GameObject>();

        foreach (GameObject statue in allStatues)
        {
            if (statueScales.ContainsKey(statue.name))
            {
                statue.transform.localScale = statueScales[statue.name] * 100;
            }
            else
            {
                statue.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}
