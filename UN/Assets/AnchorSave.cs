using UnityEngine;
using Meta.XR.BuildingBlocks;
using UnityEngine;
using Meta.WitAi.Json;
using Meta.WitAi.CallbackHandlers;

public class AnchorSave : MonoBehaviour
{
    public GameObject anchorPrefab;
    
    
        // Called by Wit.ai with the intent and entities - voice command to save_anchor
        public void OnWitResponse(WitResponseNode response)
        {
            string intent = response["intents"]?[0]?["name"]?.Value;
            if (intent != "save_anchor") return;

            string label = response["entities"]["object:object"]?[0]?["value"]?.Value;
            if (string.IsNullOrEmpty(label))
            {
                Debug.LogWarning("No label provided in voice command.");
                return;
            }

            CreateSpatialAnchor(); // put label in (label) ?
        }
        
        
    

    public void CreateSpatialAnchor()
    {
        GameObject prefab = Instantiate(
            anchorPrefab,
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
            OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));

        prefab.AddComponent<OVRSpatialAnchor>();
        
        
    }
}