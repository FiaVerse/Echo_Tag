
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class AnchorTagger : MonoBehaviour
{
    public string anchorName = "keys";
    public GameObject visualPrefab;

    public void TagHere()
    {
        var anchorGO = new GameObject($"Anchor_{anchorName}");
        anchorGO.transform.position = Camera.main.transform.position;
        anchorGO.transform.rotation = Quaternion.identity;

        var anchor = anchorGO.AddComponent<OVRSpatialAnchor>();
        anchor.Save((success) =>
        {
            if (success && visualPrefab)
                Instantiate(visualPrefab, anchorGO.transform);
        });
    }
}
