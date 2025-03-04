using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Nuzzle", menuName = "Scriptable Objects/Nuzzle")]
public class Nuzzle : ScriptableObject
{
    [Tooltip("Nuzzle prefab")]
    public GameObject nuzzlePrefab;

    [Tooltip("Nuzzle title")]
    public string title;

    [Tooltip("Nuzzle description")]
    public string description;
}
