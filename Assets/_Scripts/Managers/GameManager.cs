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

    /// <Summary>
    ///     <param name = "complete" > Set to true if the game was actually completed </param>
    /// </Summary>
    public void GameOver(bool complete = false)
    {
        Debug.Log("Game Over");
    }
}
