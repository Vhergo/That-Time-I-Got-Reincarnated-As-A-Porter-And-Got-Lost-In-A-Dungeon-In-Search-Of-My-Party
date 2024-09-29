using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FearManager : MonoBehaviour
{
    public static FearManager Instance { get; private set; }

    public bool isLit;
    public bool inRange;

    [SerializeField] private float fearMeter;
    [SerializeField] private int lives;
    [SerializeField] private Image fearMeterFill;
    [SerializeField] ColorVector fearMeterColor;
    [SerializeField] private Image dangerIndicator;
    private RectTransform dangerIndicatorRect;

    private float monsterFear;

    private Vector3? lastCheckpoint; // Nullable Vector3

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        dangerIndicator.enabled = false;
        dangerIndicatorRect = dangerIndicator.GetComponent<RectTransform>();
    }

    private void Update() => AdjustFearMeter();

    private void AdjustFearMeter()
    {
        if (isLit && inRange) fearMeter = Mathf.Clamp(fearMeter -= Time.deltaTime, 0, 100);
        else fearMeter = Mathf.Clamp(fearMeter += Time.deltaTime * (1 + monsterFear), 0, 100);

        float fearFillAmount = fearMeter / 100;
        fearMeterFill.fillAmount = fearFillAmount;
        fearMeterFill.color = Color.Lerp(fearMeterColor.baseColor, fearMeterColor.fearColor, fearFillAmount);

        UpdateDangerIndicatorPosition(fearFillAmount);

        if (fearMeter >= 100) ResetToCheckPoint();
    }

    private void UpdateDangerIndicatorPosition(float fillAmount)
    {
        RectTransform fearMeterRect = fearMeterFill.GetComponent<RectTransform>();

        float indicatorX = fearMeterRect.rect.xMin - 150f + (fearMeterRect.rect.width * fillAmount);

        Vector3 newPosition = dangerIndicatorRect.localPosition;
        newPosition.x = indicatorX;
        dangerIndicatorRect.localPosition = newPosition;
    }

    private void ResetToCheckPoint()
    {
        if (lastCheckpoint.HasValue && lives != 1) {
            Revive();
        } else {
            EndGame();
        }
    }

    public void SetCheckPointPos(Vector3 newCheckPointPos) => lastCheckpoint = newCheckPointPos;

    public void AddFear(float fearStrength)
    {
        fearMeter = Mathf.Clamp(fearMeter += fearStrength, 0, 100);
        StartCoroutine(DangerIndicator());
    }

    private IEnumerator DangerIndicator()
    {
        dangerIndicator.enabled = true;
        yield return new WaitForSeconds(2f);
        dangerIndicator.enabled = false;
    }

    public void IncreaseFear(float fearFactor) => monsterFear = fearFactor;
    public void DecreaseFear() => monsterFear = 1;

    private void Revive()
    {
        transform.position = (Vector3)lastCheckpoint;
        lives--;
        fearMeter = 0;
    }

    private void EndGame()
    {
        Debug.Log("GAME OVER! GAME OVER! GAME OVER! GAME OVER! GAME OVER!");
    }
}

[Serializable]
public class ColorVector
{
    public Color baseColor;
    public Color fearColor;
}
