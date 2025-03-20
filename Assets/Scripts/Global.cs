using System;
using System.Collections.Generic;
using UnityEngine;

// Permet d'enregistrer des variables
public static class Global
{
    public static int musicVolume = 50;
    public static int sfxVolume = 50;

    public static bool haptic = true;

    public static GameObject[] allStatues;
    public static List<GameObject> activeStatues;

    public static Dictionary<string, Vector3> statueScales = new Dictionary<string, Vector3>()
    {
        { "base", new Vector3(0.12f, 0.12f, 0.12f) },
        { "companioncube", new Vector3(0.33f, 0.33f, 0.33f) },
        { "duck", new Vector3(0.33f, 0.33f, 0.33f) },
        { "ImAiFairy", new Vector3(0.1342956f, 0.1342956f, 0.1342956f) },
        { "pot", new Vector3(0.33f, 0.33f, 0.33f) },
        { "UwU", new Vector3(0.1453455f, 0.1453455f, 0.1453455f) },
        { "statyue", new Vector3(0.15f, 0.15f, 0.15f) },
    };

    public static Dictionary<string, Vector3> statueLocation = new Dictionary<string, Vector3>()
    {
        { "base", new Vector3(0, 0f, 0.0015f) },
        { "companioncube", new Vector3(0, 0, 0.00464f) },
        { "duck", new Vector3(0, 0, 0.00275f) },
        { "ImAiFairy", new Vector3(0, 0, 0.001249999f) },
        { "pot", new Vector3(0, 0, 0.0014f) },
        { "UwU", new Vector3(0, 0, 0.00123f) },
        { "statyue", new Vector3(0, 0, 0.00805f) },
    };

    public static Dictionary<string, Quaternion> statueRotation = new Dictionary<
        string,
        Quaternion
    >()
    {
        { "pot", new Quaternion(0, 90f, 0, 0f) },
        { "statyue", new Quaternion(0, 90f, 0, 0f) },
    };

    public static Dictionary<string, float> statuePercentage = new Dictionary<string, float>()
    {
        { "base", 0f },
        { "companioncube", 0f },
        { "duck", 0f },
        { "ImAiFairy", 0f },
        { "pot", 0f },
        { "UwU", 0f },
        { "statyue", 0f },
    };

    public static void LoadStatues()
    {
        allStatues = Resources.LoadAll<GameObject>("Statues");
        Debug.Log("Statues charg es : " + allStatues.Length);

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
