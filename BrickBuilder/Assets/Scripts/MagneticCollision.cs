using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagneticCollision : MonoBehaviour {
    [Tooltip("Audio clip to play when this hologram aligned with others magnetically.")]
    public AudioClip MagneticClipSound;
    private AudioSource audioSource;
    public List<GameObject> DetectedGameObjects = new List<GameObject>();

    void Start()
    {
        EnableAudioFeedback();
    }
    private void EnableAudioFeedback()
    {
        // If this hologram has an audio clip, add an AudioSource with this clip.
        if (MagneticClipSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = MagneticClipSound;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
            audioSource.volume = 0.5f;
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "brick" && !DetectedGameObjects.Contains(other.gameObject))
        {
            DetectedGameObjects.Add(other.gameObject);
        }
        
    }

    void OnTriggerExit (Collider other)
    {
        DetectedGameObjects.Remove(other.gameObject);
    }

    void MagneticClip()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
