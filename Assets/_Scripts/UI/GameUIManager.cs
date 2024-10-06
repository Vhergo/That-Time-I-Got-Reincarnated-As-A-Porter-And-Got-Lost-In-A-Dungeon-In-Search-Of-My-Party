using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }else {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button exitToMenuButton;

    [Header("Win Screen")]
    [SerializeField] private GameObject winScreen;

    [Header("Lose Screen")]
    [SerializeField] private GameObject loseScreen;

    private void OnEnable()
    {
        GameManager.OnGameOver += TurnOnLoseScreen;
        GameManager.OnDungeonCleared += TurnOnWinScreen;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= TurnOnLoseScreen;
        GameManager.OnDungeonCleared -= TurnOnWinScreen;
    }

    private void Start() {
        if (exitToMenuButton != null) exitToMenuButton.onClick.AddListener(OnExitToMenuButtonClick);

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void TurnOnSettings() {
        settingsPanel.SetActive(true);
    }

    public void TurnOffSettings() {
        settingsPanel.SetActive(false);
    }

    private void OnExitToMenuButtonClick() {
        MySceneManager.Instance.SwitchScene(SceneEnum.MainMenuScene);
    }

    private void TurnOnWinScreen() => winScreen.SetActive(true);
    private void TurnOnLoseScreen() => loseScreen.SetActive(true);
}
