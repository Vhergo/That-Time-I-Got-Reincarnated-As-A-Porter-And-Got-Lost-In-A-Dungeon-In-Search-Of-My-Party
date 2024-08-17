using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderManager : MonoBehaviour
{
    public Dictionary<int, Tuple<int, int>> torches;
    public static PlaceholderManager instance;
    private int nextTorch = 1;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

        //Dictionary Key is the torch number, first int in tuple represents the instance id of the placeholder, second int in tuple represents the strength of torch
        torches.Add(1, new Tuple<int, int>(0, 100));
        torches.Add(2, new Tuple<int, int>(0, 69));
        torches.Add(2, new Tuple<int, int>(0, 42));
    }


    // Function needs to:
    //      - Check if all torches are currently being used
    //      - 
    public void setStrength(GameObject gameObject)
    {
        gameObject.GetComponent<Placeholder>().strength = 10;
    }
}
