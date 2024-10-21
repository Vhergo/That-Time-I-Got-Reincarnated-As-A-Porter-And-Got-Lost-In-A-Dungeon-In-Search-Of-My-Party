using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class FinalBossChase : MonoBehaviour
{
    [SerializeField] private PlayableAsset bossChaseCutscene;
    [SerializeField] private PlayableAsset dungeonClearedCutscene;
    [SerializeField] private GameObject corridorBlock;
    [SerializeField] private AudioSource chaseRumble;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stopDistance;
    
    private bool canMove;
    private Vector3 startPosition;
    private Transform target;

    private int chaseTentacleCount;

    private void OnEnable()
    {
        ChaseTentacle.OnChaseTentacleDeath += ChaseTentacleDeath;
        FearManager.OnPlayerRespawn += ResetBossChase;
    }

    private void OnDisable()
    {
        ChaseTentacle.OnChaseTentacleDeath -= ChaseTentacleDeath;
        FearManager.OnPlayerRespawn -= ResetBossChase;
    }

    private void Start()
    {
        target = Player.Instance.transform;
        startPosition = transform.position;
        chaseTentacleCount = transform.childCount;
        chaseRumble = GetComponent<AudioSource>();

        foreach (Light2D light in GetComponentsInChildren<Light2D>()) {
            light.enabled = false;
        }
    }

    private void Update()
    {
        if (!canMove) return;
        if (target.position.x < transform.position.x + stopDistance) return;

        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    private void ChaseTentacleDeath()
    {
        StopBossChase();
        chaseTentacleCount--;
        if (chaseTentacleCount <= 0) {
            CutsceneManager.Instance.PlayCutscene(dungeonClearedCutscene);
            chaseRumble.Stop();
        }
    }

    public void StartBossChase()
    {
        chaseRumble.Play();
        canMove = true;

        foreach (Light2D light in GetComponentsInChildren<Light2D>()) {
            light.enabled = true;
        }
    }

    public void ResetBossChase() => transform.position = startPosition;
    public void StopBossChase() => canMove = false;

    public void RemoveCorridorBlock() => corridorBlock.SetActive(false);

    private void ToggleTentacles(bool active)
    {
        gameObject.SetActive(active);
    }
}
