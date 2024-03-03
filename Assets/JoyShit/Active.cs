using UnityEngine;

public class Active : MonoBehaviour
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

    void ActivateColleagues()
    {
       SetColleagueColor(gameObject, activeColor);
    }

    void DeactivateColleagues()
    {
        SetColleagueColor(gameObject, inactiveColor);
        agentBody.isKinematic = false;
        agentBody.useGravity = true;
        agentBody.AddTorque(Vector3.forward * 10);
        shockVFX.Play();
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

