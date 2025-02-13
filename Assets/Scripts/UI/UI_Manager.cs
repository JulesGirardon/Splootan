using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Tooltip("Canvas game object")]
    public GameObject canvas;

    [Tooltip("Input action reference to show the UI")]
    public InputActionReference showUIReference;

    [Tooltip("Spatial panel game object")]
    public GameObject SpatialPanel;

    [Tooltip("Main menu game object")]
    public GameObject MainMenu;

    [Tooltip("Main menu game object")]
    public GameObject ChangeBuseMenu;

    [Tooltip("Dropdown for buse selection")]
    public TMP_Dropdown dropdownBuse;

    public TMP_Text buseDescription;

    void Awake()
    {
        if (canvas) canvas.SetActive(false);
        if (ChangeBuseMenu) ChangeBuseMenu.SetActive(false);

        if (dropdownBuse)
        {
            dropdownBuse.onValueChanged.AddListener(OnDropdownValueChanged);

            foreach (Nuzzle nuzzle in Global.nuzzles)
            {
                dropdownBuse.options.Add(new TMP_Dropdown.OptionData(nuzzle.title));
            }

            buseDescription.text = Global.nuzzles[0].description;
        }

        showUIReference.action.performed += showUI;
    }

    void showUI(InputAction.CallbackContext context)
    {
        if (canvas)
        {
            canvas.SetActive(canvas.activeSelf ? false : true);
        }
    }

    public void BackToGame()
    {
        if (canvas)
        {
            canvas.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ChangeBuse()
    {
        if (SpatialPanel)
        {
            if (MainMenu) MainMenu.SetActive(false);
            if (ChangeBuseMenu) ChangeBuseMenu.SetActive(true);
        }
    }

    public void BackToPauseMenu()
    {
        if (MainMenu) MainMenu.SetActive(true);
        if (ChangeBuseMenu) ChangeBuseMenu.SetActive(false);

    }

    void OnDropdownValueChanged(int index)
    {
        buseDescription.text = Global.nuzzles[index].description;
    }

}
