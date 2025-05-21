using UnityEngine;
using Meta.XR.BuildingBlocks;
using UnityEngine;
using Meta.WitAi.Json;
using Meta.WitAi.CallbackHandlers;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AnchorPlacement : MonoBehaviour
{
    public OVRSpatialAnchor anchorPrefab;
    
    private Canvas canvas;
    private TextMeshProUGUI uuidText;
    private TextMeshProUGUI savedStatusText;
    //private List<OVRSpatialAnchor> anchors = new list<OVRSpatialAnchor>();
    private OVRSpatialAnchor lastCreatedAnchor;
    
    
        // Called by Wit.ai with the intent and entities
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
        OVRSpatialAnchor workingAnchor = Instantiate(
            anchorPrefab,
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
            OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));
        
        
        canvas = workingAnchor.gameObject.GetComponentInChildren<Canvas>();
        uuidText = canvas.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        savedStatusText = canvas.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        StartCoroutine(AnchorCreated(workingAnchor));
    }

    private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor)
    {
        while (!workingAnchor.Created && !workingAnchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }

        //Guid anchorGuid = workingAnchor.Uuid;
        //anchors.Add(workingAnchor);
        lastCreatedAnchor = workingAnchor;

       // uuidText.text = "UUID: " + anchorGuid.ToString();
        savedStatusText.text = "Not Saved";
    }
    
}