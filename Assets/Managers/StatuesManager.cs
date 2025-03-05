using UnityEngine;

public class StatuesManager : MonoBehaviour
{
    [Header("Statues")]

    [Tooltip("Platines où tournes les statues")]
    public GameObject[] platines;

    [Tooltip("Matériaux des statues")]
    public Material material;

    private void Awake()
    {
        Global.LoadStatues();
    }

    private void Start()
    {
        for (int i = 0; i < platines.Length; i++)
        {
            GameObject statue = Global.allStatues[Random.Range(0, Global.allStatues.Length)];

            while (Global.activeStatues.Contains(statue))
            {
                statue = Global.allStatues[Random.Range(0, Global.allStatues.Length)];
            }

            Global.activeStatues.Add(statue);

            GameObject statueInstantiate = Instantiate(statue, platines[i].transform.position, statue.transform.rotation);

            statueInstantiate.AddComponent<MeshCollider>();
            statueInstantiate.AddComponent<PaintOnGeneratedTexture>();

            Material statueMaterial = new Material(material);
            statueInstantiate.GetComponent<Renderer>().material = statueMaterial;

            statueInstantiate.transform.SetParent(platines[i].transform);

            statueInstantiate.transform.localPosition = Global.statueLocation[statue.name];
        }
    }
}
