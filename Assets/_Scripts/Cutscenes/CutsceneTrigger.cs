using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableAsset cutscene;

    public void TriggerCutscene()
    {
        CutsceneManager.Instance.PlayCutscene(cutscene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            TriggerCutscene();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
