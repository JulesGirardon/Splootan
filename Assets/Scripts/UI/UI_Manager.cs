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

    void Awake()
    {
        if (canvas) canvas.SetActive(false);

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

    public void BackToPauseMenu()
    {
        if (MainMenu) MainMenu.SetActive(true);

    }
}
