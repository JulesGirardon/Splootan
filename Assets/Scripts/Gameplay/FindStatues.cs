using System.Linq; // Nécessaire pour utiliser LINQ
using UnityEngine;
using UnityEngine.UI;

public class FindStatues : MonoBehaviour
{
    public Image[] images;
    public LeverController[] levers; // Référence aux leviers

    public static string correctStatueKey; // Clé de la statue correcte

    void Start()
    {
        // Vérifier qu'il y a au moins 3 images dans le tableau
        if (images == null || images.Length < 3)
        {
            Debug.LogWarning("Le tableau 'images' doit contenir au moins 3 images.");
            return;
        }

        Debug.Log("Nombre d'images assignées : " + images.Length);

        // Trouver la statue avec le pourcentage maximum
        correctStatueKey = Global
            .statuePercentage.OrderByDescending(entry => entry.Value)
            .FirstOrDefault()
            .Key;

        Debug.Log("StatueKey trouvée : " + correctStatueKey);

        // Charger l'image correcte depuis Resources
        Sprite correctSprite = Resources.Load<Sprite>("Images/" + correctStatueKey);

        if (correctSprite == null)
        {
            Debug.LogWarning("Aucun sprite trouvé dans Resources pour : " + correctStatueKey);
            return;
        }

        // Placer l'image correcte à un index aléatoire
        int correctIndex = UnityEngine.Random.Range(0, images.Length);
        images[correctIndex].sprite = correctSprite;

        Debug.Log("Image correcte placée à l'index : " + correctIndex);

        // Charger toutes les autres images disponibles dans Resources
        var allSprites = Resources.LoadAll<Sprite>("Images").ToList();

        // Retirer l'image correcte de la liste des sprites disponibles
        allSprites.RemoveAll(sprite => sprite.name == correctStatueKey);

        // Vérifier qu'il y a au moins 2 autres images disponibles
        if (allSprites.Count < 2)
        {
            Debug.LogWarning(
                "Pas assez d'images disponibles dans Resources pour remplir les autres emplacements."
            );
            return;
        }

        // Remplir les deux autres emplacements avec des images aléatoires
        int imageIndex = 0;
        foreach (var image in images)
        {
            if (imageIndex == correctIndex)
            {
                imageIndex++;
                continue; // Ne pas écraser l'image correcte
            }

            // Sélectionner une image aléatoire parmi les restantes
            int randomSpriteIndex = UnityEngine.Random.Range(0, allSprites.Count);
            image.sprite = allSprites[randomSpriteIndex];

            // Retirer l'image utilisée pour éviter les doublons
            allSprites.RemoveAt(randomSpriteIndex);

            imageIndex++;
        }

        Debug.Log("Images aléatoires placées dans les autres emplacements.");
    }
}
