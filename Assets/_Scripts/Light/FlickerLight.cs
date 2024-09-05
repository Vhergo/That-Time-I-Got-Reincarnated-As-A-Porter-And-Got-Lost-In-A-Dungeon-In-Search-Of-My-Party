using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight : MonoBehaviour
{
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    [SerializeField] private float lowIntensity;
    [SerializeField] private float highIntensity;
    [SerializeField] private float colliderRadius;

    private float torchStrength;
    private float interval = 1;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        torchStrength = gameObject.transform.parent.gameObject.GetComponent<Placeholder>().strength;
    }

    void Update()
    {
        torchStrength = gameObject.transform.parent.gameObject.GetComponent<Placeholder>().strength;
        gameObject.GetComponent<Light2D>().pointLightInnerRadius = innerRadius * (torchStrength / 100);
        gameObject.GetComponent<Light2D>().pointLightOuterRadius = outerRadius * (torchStrength / 100);
        
        timer += Time.deltaTime;
        if (timer > interval)
        {
            gameObject.GetComponent<Light2D>().intensity = Random.Range(lowIntensity, highIntensity);
            interval = Random.Range(0f, 0.1f);
            timer = 0;
        }

        gameObject.GetComponent<CircleCollider2D>().radius = colliderRadius * (torchStrength / 100);
        if (torchStrength != 0.0f)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
            FearScript.instance.inLight = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            FearScript.instance.inLight = false;
    }
}
