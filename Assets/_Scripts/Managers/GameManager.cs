using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private MySceneManager mySceneManager;

    public static Action OnGameOver;
    public static Action OnDungeonCleared;

    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start() => mySceneManager = MySceneManager.Instance;

    public void GameOver()
    {
        Debug.Log("Game Over");
        MySceneManager.Instance.PauseGame();
        OnGameOver?.Invoke();
    }

    public void DungeonCleared()
    {
        Debug.Log("Dungeon Cleared!");
        MySceneManager.Instance.PauseGame();
        OnDungeonCleared?.Invoke();
    }
}
