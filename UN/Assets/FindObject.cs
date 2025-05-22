using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FindObject : MonoBehaviour
{
    public GameObject orb;
    
    


    void Start()
    {
        orb.SetActive(false);   
        
    }
    public void GuidingToObject()
    {
        orb.SetActive(true);
        
    }
}
