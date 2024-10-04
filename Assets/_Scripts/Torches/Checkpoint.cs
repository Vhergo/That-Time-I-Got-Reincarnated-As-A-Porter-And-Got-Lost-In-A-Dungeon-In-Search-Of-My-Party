using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint Instance;
    public bool hasVisited;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hasVisited = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasVisited & collision.gameObject.tag.Equals("Player"))
        {
            FearManager.Instance.SetCheckPointPos(this.transform.position);
        }
        
    }
}
