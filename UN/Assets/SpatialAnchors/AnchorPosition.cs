using UnityEngine;
using Meta.XR.BuildingBlocks;
using UnityEngine;
using Meta.WitAi.Json;
using Meta.WitAi;
using Meta.WitAi.CallbackHandlers;

public class AnchorPosition : MonoBehaviour
{
    public GameObject anchorPrefab;
    
     //   [MatchIntent("save_anchor")]
    public void CreateSpatialAnchor()
    {
        GameObject prefab = Instantiate(
            anchorPrefab,
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
            OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));
        
        prefab.AddComponent<OVRSpatialAnchor>();
        
    }
}