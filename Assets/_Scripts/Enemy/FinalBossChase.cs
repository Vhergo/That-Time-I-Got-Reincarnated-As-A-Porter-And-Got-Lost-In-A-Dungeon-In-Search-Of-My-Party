using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;

public class FinalBossChase : MonoBehaviour
{
    [SerializeField] private PlayableAsset bossChaseCutscene;
    [SerializeField] private PlayableAsset dungeonClearedCutscene;
    [SerializeField] private GameObject corridorBlock;
    [SerializeField] private float moveSpeed;
    private bool canMove;
    private Vector3 startPosition;

    private int chaseTentacleCount;

    private void OnEnable()
    {
        ChaseTentacle.OnChaseTentacleDeath += ChaseTentacleDeath;
    }

    private void Start()
    {
        startPosition = transform.position;
        chaseTentacleCount = transform.childCount;
        ToggleTentacles(false);
    }

    private void Update()
    {
        if (canMove) {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    private void ChaseTentacleDeath()
    {
        chaseTentacleCount--;
        if (chaseTentacleCount <= 0) {
            StopBossChase();
            CutsceneManager.Instance.PlayCutscene(dungeonClearedCutscene);
        }
    }

    public void StartBossChase()
    {
        ToggleTentacles(true);
        canMove = true;
    }
    public void ResetBossChase() => transform.position = startPosition;
    public void StopBossChase() => canMove = false;

    public void RemoveCorridorBlock() => corridorBlock.SetActive(false);

    private void ToggleTentacles(bool active)
    {
        gameObject.SetActive(active);
    }
}
