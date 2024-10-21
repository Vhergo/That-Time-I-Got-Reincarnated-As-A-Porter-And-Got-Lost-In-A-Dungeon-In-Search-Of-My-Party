using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    [SerializeField] private PlayableAsset introCutscene;
    private Dictionary<PlayableAsset, bool> cutscenes = new Dictionary<PlayableAsset, bool>();

    private PlayableDirector playableDirector;

    private bool canSkipCutscene;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) PlayCutscene(introCutscene);
    }

    public void PlayCutscene(PlayableAsset cutscene)
    {
        if (cutscene == null) return;
        //LoadCutsceneHasPlayed(cutscene.name);

        playableDirector.playableAsset = cutscene;
        playableDirector.Play();

        //SaveCutsceneHasPlayed(cutscene.name);
    }

    public void SkipCutscene()
    {
        playableDirector.time = playableDirector.duration;
        playableDirector.Evaluate();
        playableDirector.Stop();
    }

    public void SaveCutsceneHasPlayed(string cutsceneName)
    {
        PlayerPrefs.SetInt(cutsceneName, 1);
    }

    public void LoadCutsceneHasPlayed(string cutsceneName)
    {
        if (PlayerPrefs.GetInt(cutsceneName, 0) == 1) canSkipCutscene = true;
        else canSkipCutscene = false;
    }
}
