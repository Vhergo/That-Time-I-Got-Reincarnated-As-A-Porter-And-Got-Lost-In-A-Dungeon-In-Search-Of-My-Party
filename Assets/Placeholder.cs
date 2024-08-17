using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
    public int strength = 0;
    public static Placeholder Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Nice()
    {
        if(strength > 0)
        {
            Debug.Log("Placed!!!! + " + gameObject.GetInstanceID().ToString());
            gameObject.GetComponent<SpriteRenderer>().color = new Color(10, 204, 102);
        }
    }
}
