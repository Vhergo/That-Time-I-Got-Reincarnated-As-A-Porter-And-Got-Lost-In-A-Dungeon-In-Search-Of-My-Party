using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (MySceneManager.Instance != null)
            Time.timeScale = 1;
    }
}
