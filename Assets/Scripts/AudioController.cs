using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    public static AudioController instance;

    [Header("Sound Settings")]
    [Range(0.0f, 1.0f)]
    public float volume;

    [Header("Sounds")]
    public List<AudioClip> popSounds;

    private AudioSource audioSource;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayPopSound() {
        AudioClip sound = popSounds[Random.Range(0, popSounds.Count)];
        audioSource.PlayOneShot(sound, volume);
    }
}
