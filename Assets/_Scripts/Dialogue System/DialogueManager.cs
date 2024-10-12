using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue")]
    [SerializeField] private List<Dialogue> dialogues;
    [SerializeField] private float typeSpeed = 0.05f;
    private int currentDialogueIndex;
    private Dialogue currentDialogue;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Audio")]
    [SerializeField] private AudioSource typingSource;
    [SerializeField] private AudioClip monsterRoar;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Start()
    {
        currentDialogue = dialogues[currentDialogueIndex];
    }

    public void TriggerDialogue() => StartCoroutine(PlayDialogue());
    public IEnumerator PlayDialogue()
    {
        if (!dialoguePanel.activeSelf) dialoguePanel.SetActive(true);

        currentDialogue = dialogues[currentDialogueIndex];
        UpdateUI();

        dialogueText.maxVisibleCharacters = 0;
        int totalVisibleCharacters = currentDialogue.dialogueText.Length;
        int counter = 0;

        while (counter <= totalVisibleCharacters) {
            dialogueText.maxVisibleCharacters = counter;
            counter++;

            PlayTypingSound();
            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitForSeconds(currentDialogue.dialogueDuration);
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Count) {
            if (dialogues[currentDialogueIndex].cutscene != null) {
                CutsceneManager.Instance.PlayCutscene(dialogues[currentDialogueIndex].cutscene);
                yield return new WaitForSeconds(monsterRoar.length);
            }

            StartCoroutine(PlayDialogue());
        } else {
            HideDialogue();
        }
    }

    private void UpdateUI()
    {
        speakerName.text = currentDialogue.speaker.ToString();
        dialogueText.text = currentDialogue.dialogueText;
    }

    public void ShowDialogue() => dialoguePanel.SetActive(true);
    public void HideDialogue() => dialoguePanel.SetActive(false);

    private void PlayTypingSound()
    {
        typingSource.pitch = Random.Range(0.8f, 1.2f);
        typingSource.Play();
    }

}
