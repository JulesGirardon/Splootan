using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    void Awake()
    {
        if (canvas)
        {
            canvas.SetActive(false);
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

            RectTransform rt = SpatialPanel.GetComponent<RectTransform>();

            rt.sizeDelta = new Vector2(rt.sizeDelta.x != 800 ? 800 : rt.sizeDelta.x, rt.sizeDelta.y);
        }
    }

    public void BackToPauseMenu()
    {
        RectTransform rt = SpatialPanel.GetComponent<RectTransform>();

        rt.sizeDelta = new Vector2(rt.sizeDelta.x != 400 ? 400 : rt.sizeDelta.x, rt.sizeDelta.y);

        if (MainMenu) MainMenu.SetActive(true);
        if (ChangeBuseMenu) ChangeBuseMenu.SetActive(false);

    }
}
