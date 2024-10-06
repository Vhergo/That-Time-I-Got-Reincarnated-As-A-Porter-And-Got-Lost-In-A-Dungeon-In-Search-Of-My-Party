using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDataManager : MonoBehaviour
{
    public static SettingsDataManager Instance { get; private set; }

    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject slider;
    [SerializeField] private GameObject toggle;

    private void Awake()
    {
        if (Instance == null)  Instance = this;
        else  Destroy(gameObject);
    }

    public GameObject GetSettingsUI() => settings;
    public GameObject GetSliderUI() => slider;
    public GameObject GetToggleUI() => toggle;

}
