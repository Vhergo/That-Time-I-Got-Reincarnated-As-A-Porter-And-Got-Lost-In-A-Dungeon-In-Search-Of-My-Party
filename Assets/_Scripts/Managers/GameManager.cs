using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }else {
            Destroy(gameObject);
        }
    }

    public void GameOver(bool complete = false)
    {
        Debug.Log("Game Over");
    }

    public void DungeonCleared()
    {
        Debug.Log("Dungeon Cleared!");
    }
}
