using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemAudioFeedback : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip proximitySound; // Assign a subtle ping sound in Inspector

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        /*audioSource.spatialBlend = 1.0f; // Full 3D spatialization
        audioSource.minDistance = 0.3f; // Start hearing at 0.3m
        audioSource.maxDistance = 3.0f; // Max hearing distance
        audioSource.loop = true;
        audioSource.clip = proximitySound;*/
    }

    public void EnableFeedback(bool enable)
    {
        if(enable && !audioSource.isPlaying) audioSource.Play();
        else audioSource.Stop();
    }
}