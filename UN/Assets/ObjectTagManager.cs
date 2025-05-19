using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Json;
using Oculus.SpatialAnchor;

public class ObjectTagManager : MonoBehaviour
{
    private Dictionary<string, OVRSpatialAnchor> anchorDictionary = new Dictionary<string, OVRSpatialAnchor>();

    void Start()
    {
        // Assuming you have a Wit service attached
        Wit wit = FindObjectOfType<Wit>();
        if (wit != null)
        {
            wit.events.OnResponse += OnWitResponse;
        }
    }

    private void OnWitResponse(WitResponseNode response)
    {
        string text = response["text"];
        if (string.IsNullOrEmpty(text)) return;

        if (text.ToLower().Contains("tag my"))
        {
            string item = text.ToLower().Replace("tag my", "").Trim();
            CreateAnchor(item);
        }
        else if (text.ToLower().Contains("where are my"))
        {
            string item = text.ToLower().Replace("where are my", "").Trim();
            FindAnchor(item);
        }
    }

    void CreateAnchor(string itemName)
    {
        OVRSpatialAnchor anchor = gameObject.AddComponent<OVRSpatialAnchor>();
        anchor.Save((OVRSpatialAnchor anchorObj, bool success) =>
        {
            if (success)
            {
                anchorDictionary[itemName] = anchorObj;
                Debug.Log($"{itemName} anchor saved.");
                Speak($"{itemName} saved.");
            }
            else
            {
                Debug.LogWarning($"Failed to save anchor for {itemName}");
                Speak($"Could not save {itemName}.");
            }
        });
    }

    void FindAnchor(string itemName)
    {
        if (anchorDictionary.TryGetValue(itemName, out OVRSpatialAnchor anchor))
        {
            Vector3 position = anchor.transform.position;
            Debug.Log($"{itemName} is at {position}");
            Speak($"{itemName} is at coordinates {position}");
        }
        else
        {
            Debug.LogWarning($"{itemName} anchor not found.");
            Speak($"I couldn't find {itemName}.");
        }
    }

    void Speak(string message)
    {
        // Replace with your own TTS system
        Debug.Log("TTS: " + message);
        // e.g., Meta.WitAi.TTS.TTSService.Instance.Speak(message);
    }
}
