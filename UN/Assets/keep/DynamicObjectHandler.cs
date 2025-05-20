using UnityEngine;

public class DynamicObjectHandler : MonoBehaviour
{
    public void OnFindObject(string[] values)
    {
        if (values.Length > 0)
        {
            string objectName = values[0];
            Debug.Log($"[Intent] Finding object: {objectName}");

            // Add logic to highlight or locate object
            GameObject found = GameObject.Find(objectName);
            if (found != null)
            {
                // Example feedback
                found.GetComponent<Renderer>().material.color = Color.yellow;
                Debug.Log($"Object {objectName} found and highlighted.");
            }
            else
            {
                Debug.Log($"Object {objectName} not found.");
            }
        }
    }

    public void OnSaveLocation(string[] values)
    {
        if (values.Length > 0)
        {
            string anchorName = values[0];
            Debug.Log($"[Intent] Saving location: {anchorName}");

            // Add logic to save this location as an anchor
            Vector3 location = transform.position; // Or camera.position
            PlayerPrefs.SetFloat(anchorName + "_x", location.x);
            PlayerPrefs.SetFloat(anchorName + "_y", location.y);
            PlayerPrefs.SetFloat(anchorName + "_z", location.z);
            PlayerPrefs.Save();

            Debug.Log($"Saved anchor '{anchorName}' at {location}");
        }
    }
}