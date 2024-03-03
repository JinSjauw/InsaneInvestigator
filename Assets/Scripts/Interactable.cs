using UnityEngine;

public enum InteractableType
{
    SUSPECT = 0,
}

public class Interactable : MonoBehaviour
{
    [SerializeField] private ParticleSystem shockVFX;
    
    private Color activeColor = Color.green;
    private Color inactiveColor = Color.red;

    private Rigidbody agentBody;
    
    private void Start()
    {
        shockVFX.Stop();
        ActivateColleagues();
        agentBody = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        DeactivateColleagues();
    }
    
    private void ActivateColleagues()
    {
       SetColleagueColor(gameObject, activeColor);
    }

    private void DeactivateColleagues()
    {
        SetColleagueColor(gameObject, inactiveColor);
        agentBody.isKinematic = false;
        agentBody.useGravity = true;
        agentBody.AddTorque(Vector3.forward * 10);
        shockVFX.Play();
    }

    private void SetColleagueColor(GameObject colleague, Color color)
    {
        Renderer colleagueRenderer = colleague.GetComponent<Renderer>();
        if (colleagueRenderer != null)
        {
            //colleagueRenderer.sharedMaterial.color = color; 
            colleagueRenderer.material.SetColor("_BaseColor", color);
        }
    }
    
}

