using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [Tooltip("Main menu game object")]
    public GameObject mainMenu;

    [Tooltip("Main menu game object")]
    public GameObject optionMenu;

    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public TextMeshProUGUI textQuality;
    private int indexQuality = 0;
    private string[] nameQuality = { "Très basse", "Basse", "Moyenne", "Haute", "Très haute", "Ultra" };

    private void Awake()
    {
        musicVolumeSlider.value = Global.musicVolume;
        sfxVolumeSlider.value = Global.sfxVolume;

        textQuality.text = nameQuality[QualitySettings.GetQualityLevel()];
    }

    public void LoadSceneGame()
    {
        Global.musicVolume = (int)musicVolumeSlider.value;
        Global.sfxVolume = (int)sfxVolumeSlider.value;

        SceneManager.LoadScene("SampleScene");
    }

    public void LoadOptionsMenu()
    {
        
        if (mainMenu)
        {
            // Hide the main menu
            mainMenu.SetActive(false);

            // Show the options menu
            if (optionMenu)
            {
                optionMenu.SetActive(true);
            }
        }
    }

    public void LoadMainMenu()
    {
        if (optionMenu)
        {
            // Hide the options menu
            optionMenu.SetActive(false);

            // Show the main menu
            if (mainMenu)
            {
                mainMenu.SetActive(true);
            }
        }
    }

    public void ChangeQuality()
    {
        indexQuality++;
        if (indexQuality >= nameQuality.Length) 
        {
            indexQuality = 0;
        }
        Debug.Log(indexQuality);
        textQuality.text = nameQuality[indexQuality];
        QualitySettings.SetQualityLevel(indexQuality, true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
