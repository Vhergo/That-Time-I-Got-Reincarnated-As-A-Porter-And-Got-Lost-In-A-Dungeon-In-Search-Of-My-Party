using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FearScript : MonoBehaviour
{
    [SerializeField] private float playerFearMeter;
    [SerializeField] private int lives;

    private Vector3? checkPointPos;

    public static FearScript instance;
    public bool inLight = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerFearMeter = 0;
        inLight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inLight)
        {
            playerFearMeter = Mathf.Clamp(playerFearMeter -= Time.deltaTime, 0, 100);
        }
        else
        {
            playerFearMeter = Mathf.Clamp(playerFearMeter += Time.deltaTime, 0, 100);
        }

        if (playerFearMeter >= 100)
            resetToCheckPoint();
    }

    private void resetToCheckPoint()
    {
        if (checkPointPos.HasValue && lives != 1)
        {
            this.transform.position = new Vector3(checkPointPos.Value.x, checkPointPos.Value.y, checkPointPos.Value.z);
            lives -= 1;
            playerFearMeter = 0;
        }
        else
        {
            endGame();
        }
    }

    public void setCheckPointPos(Vector3 newCheckPointPos)
    {
        checkPointPos = newCheckPointPos;
    }

    public void addFear(int fearStrength)
    {
        playerFearMeter = Mathf.Clamp(playerFearMeter += fearStrength, 0, 100);
    }

    private void endGame()
    {
        Debug.Log("GAME OVER! GAME OVER! GAME OVER! GAME OVER! GAME OVER!");
    }
}
