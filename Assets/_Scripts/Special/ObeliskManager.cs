using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObeliskManager : MonoBehaviour
{
    public static ObeliskManager Instance { get; private set; }

    [SerializeField] private TMP_Text obeliskRegion;
    [SerializeField] private TMP_Text obeliskLore;
    [SerializeField] private float showDuration;
    private Obelisk currentObelisk;

    [Space(10)]
    [SerializeField] private AnimationClip showing;
    [SerializeField] private AnimationClip hiding;
    private Animator anim;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() => Obelisk.OnObeliskInteracted += SetObeliskInfo;
    private void OnDisable() => Obelisk.OnObeliskInteracted -= SetObeliskInfo;

    private void Start() => anim = GetComponent<Animator>();

    private void SetObeliskInfo(ObeliskData obeliskData, Obelisk obelisk)
    {
        currentObelisk = obelisk;

        obeliskRegion.text = obeliskData.obeliskRegion;
        obeliskLore.text = obeliskData.obeliskLore;
        StartCoroutine(ShowObeliskInfo());
    }

    private IEnumerator ShowObeliskInfo()
    {
        currentObelisk.CanInteract(false);
        anim.Play(showing.name);

        yield return new WaitForSeconds(showDuration);
        anim.Play(hiding.name);
        currentObelisk.CanInteract(true);
    }
}
