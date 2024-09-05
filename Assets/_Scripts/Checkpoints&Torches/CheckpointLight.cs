using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckpointLight : MonoBehaviour
{
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    [SerializeField] private float lowIntensity;
    [SerializeField] private float highIntensity;
    [SerializeField] private float colliderRadius;

    public static CheckpointLight instance;

    private float interval = 1;
    private float timer;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        gameObject.GetComponent<Light2D>().pointLightOuterRadius = 0;
        gameObject.GetComponent<Light2D>().pointLightInnerRadius = 0;
        gameObject.GetComponent<CircleCollider2D>().radius = colliderRadius;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    public void activateCheckpoint()
    {
        gameObject.GetComponent<Light2D>().pointLightOuterRadius = outerRadius;
        gameObject.GetComponent<Light2D>().pointLightInnerRadius = innerRadius;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            gameObject.GetComponent<Light2D>().intensity = Random.Range(lowIntensity, highIntensity);
            interval = Random.Range(0f, 0.1f);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            FearScript.instance.inLight = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            FearScript.instance.inLight = false;
    }
}
