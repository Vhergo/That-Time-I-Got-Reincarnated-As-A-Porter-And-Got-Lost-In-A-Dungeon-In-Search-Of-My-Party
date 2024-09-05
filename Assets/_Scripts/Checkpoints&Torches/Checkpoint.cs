using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint instance;
    public bool hasVisited;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        hasVisited = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasVisited & collision.gameObject.tag.Equals("Player"))
        {
            FearScript.instance.setCheckPointPos(this.transform.position);
        }
        
    }
}
