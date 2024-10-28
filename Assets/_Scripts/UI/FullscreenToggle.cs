using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{

    [SerializeField] private Button fullscreenButton;
    [SerializeField] private GameObject turnOnIcon;
    [SerializeField] private GameObject turnOffIcon;
    [SerializeField] private bool isFullscreen;


    private void OnEnable()
    {
        GameUIManager.OnMenuOpen += ShowFullscreenButton;
        GameUIManager.OnMenuClose += HideFullscreenButton;
    }

    private void OnDisable()
    {
        GameUIManager.OnMenuOpen -= ShowFullscreenButton;
        GameUIManager.OnMenuClose -= HideFullscreenButton;
    }

    private void Start()
    {
        fullscreenButton.onClick.AddListener(ToggleFullscreen);
        turnOnIcon.SetActive(!Screen.fullScreen);
        turnOffIcon.SetActive(Screen.fullScreen);
    }

    private void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        SetFullscreen();
        HandleFullscreenIcons();
    }

    private void HandleFullscreenIcons()
    {
        turnOnIcon.SetActive(!isFullscreen);
        turnOffIcon.SetActive(isFullscreen);
    }

    private void SetFullscreen() => Screen.fullScreen = isFullscreen;

    private void ShowFullscreenButton()
    {
        fullscreenButton.gameObject.SetActive(true);
    }

    private void HideFullscreenButton()
    {
        fullscreenButton.gameObject.SetActive(false);
    }
}
