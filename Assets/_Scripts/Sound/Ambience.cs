using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    [SerializeField] private float fadeTime = 2;
    [SerializeField] private float maxVolume = 0.5f;
    private AudioSource source;

    private void Start() => source = GetComponent<AudioSource>();

    public void StartAmbience() => StartCoroutine(ToggleAmbience(true));
    public void StopAmbience() => StartCoroutine(ToggleAmbience(false));

    private IEnumerator ToggleAmbience(bool on)
    {
        if (!source.isPlaying) source.Play();
        float targetVolume = on ? maxVolume : 0;
        float startVolume = on ? 0 : maxVolume;
        float currentTime = 0;
        while (currentTime < fadeTime) {
            currentTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            StopAllCoroutines();
            StartAmbience();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            StopAllCoroutines();
            StopAmbience();
        }
    }
}
