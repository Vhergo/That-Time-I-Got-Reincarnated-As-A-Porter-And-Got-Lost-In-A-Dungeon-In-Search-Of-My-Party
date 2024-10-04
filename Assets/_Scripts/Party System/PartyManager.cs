using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }

    [Header("Party Settings")]
    [SerializeField] private int currentMemberCount;
    [SerializeField] private TextMeshProUGUI partyMemberCountText;
    [SerializeField] private List<GameObject> partyMembers;
    [SerializeField] private List<PartyMember> foundMembers;
    [SerializeField] private List<Transform> partyGroupSpots;
    private Player player;

    public static Action<int> OnPartyMemberFound;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        player = Player.Instance;

        UpdateMemberCountUI();
    }

    public Transform AddPartyMember(PartyMember partyMember)
    {
        foundMembers.Add(partyMember);
        OnPartyMemberFound?.Invoke(foundMembers.Count);
        currentMemberCount++;
        UpdateMemberCountUI();

        if (foundMembers.Count == partyMembers.Count) FoundAllPartyMembers();
        
        return partyGroupSpots[foundMembers.IndexOf(partyMember)];
    }

    private void FoundAllPartyMembers()
    {
        Debug.Log("Found all party members");
        // Do something when all party members are found
    }

    private void UpdateMemberCountUI()
    {
        partyMemberCountText.text = "Found: " + currentMemberCount + " of " + partyMembers.Count;
    }

    public int GetMaxPartyMemberCount()
    {
        return partyMembers.Count;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (Transform spot in partyGroupSpots) {
            Gizmos.DrawWireSphere(spot.position, 0.2f);
        }
    }
}
