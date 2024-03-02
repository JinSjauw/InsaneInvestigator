using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    public GameObject[] colleaguesToActivate;
    public GameObject[] colleaguesToDeactivate;

    public Color activeColor = Color.green;
    public Color inactiveColor = Color.red;
    
    public void Interact()
    {
        ActivateColleagues();
        DeactivateColleagues();
    }

    void ActivateColleagues()
    {
        foreach (GameObject colleague in colleaguesToActivate)
        {
            SetColleagueColor(colleague, activeColor);
            //colleague.SetActive(true);
        }
    }

    void DeactivateColleagues()
    {
        foreach (GameObject colleague in colleaguesToDeactivate)
        {
            SetColleagueColor(colleague, inactiveColor); 
            //colleague.SetActive(false);
        }
    }

    void SetColleagueColor(GameObject colleague, Color color)
    {
        Renderer colleagueRenderer = colleague.GetComponent<Renderer>();
        if (colleagueRenderer != null)
        {
            //colleagueRenderer.sharedMaterial.color = color; 
            colleagueRenderer.material.SetColor("_BaseColor", color);
        }
    }
}

