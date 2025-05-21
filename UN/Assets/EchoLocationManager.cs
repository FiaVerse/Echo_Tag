using UnityEngine;
using System.Collections;

public class EcholocationManager : MonoBehaviour
{
    public AudioClip pingSound;
    public GameObject echoPulsePrefab;
    public float echoRange = 10f;
    public float pingCooldown = 1.5f;

    private AudioSource playerAudio;
    private Transform userHead;
    private bool canPing = true;

    void Start()
    {
        userHead = Camera.main.transform;
        playerAudio = gameObject.AddComponent<AudioSource>();
        playerAudio.spatialBlend = 0f; // 2D sound from player
    }

   

    IEnumerator DoEcholocationPing()
    {
        canPing = false;

        // Audio ping from user
        playerAudio.PlayOneShot(pingSound);

        // Visual pulse from user
        GameObject pulse = Instantiate(echoPulsePrefab, userHead.position, Quaternion.identity);
        StartCoroutine(ExpandPulse(pulse));

        yield return new WaitForSeconds(pingCooldown);
        canPing = true;
    }

    IEnumerator ExpandPulse(GameObject pulse)
    {
        float duration = 1.2f;
        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * echoRange;

        Material mat = pulse.GetComponent<MeshRenderer>().material;
        Color startColor = mat.color;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            pulse.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            if (mat != null)
                mat.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(pulse);
    }
}