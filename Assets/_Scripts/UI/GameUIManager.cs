using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private TMP_Text dungeonClearText;

    [Header("Lose Screen")]
    [SerializeField] private GameObject loseScreen;

    private PartyManager partyManager => PartyManager.Instance;

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
        CursorManager.Instance.ToggleCursor(true);
    }

    public void TurnOffSettings() {
        settingsPanel.SetActive(false);
        CursorManager.Instance.ToggleCursor(false);
    }

    private void OnExitToMenuButtonClick() {
        MySceneManager.Instance.SwitchScene(SceneEnum.MainMenuScene);
    }

    private void TurnOnWinScreen()
    {
        winScreen.SetActive(true);

        switch (partyManager.GetCurrentPartyMemberCount()) {
            case 0:
                dungeonClearText.text = partyManager.GetDungeonClearText(0);
                break;
            case 1:
                dungeonClearText.text = partyManager.GetDungeonClearText(1);
                break;
            case 2:
                dungeonClearText.text = partyManager.GetDungeonClearText(2);
                break;
            case 3:
                dungeonClearText.text = partyManager.GetDungeonClearText(3);
                break;
        }
    }
    private void TurnOnLoseScreen() => loseScreen.SetActive(true);
}
