using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float ricochetAmount;
    
    private Rigidbody projectileBody;
    private Vector3 direction;
    private int collisionCounter;

    private Vector3 lastPosition;
    private Vector3 currentPosition;

    //private Vector3 impactPoint;

    #region Unity Functions
    
    // Start is called before the first frame update
    private void Awake()
    {
        projectileBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectCollision();
    }
    
    #endregion

    #region Private Functions

    private void DetectCollision()
    {
        //Raycast between current position && lastPosition
        currentPosition = transform.position;
        if (Physics.Linecast(lastPosition, currentPosition, out RaycastHit hit))
        {
            Debug.Log("Collided! " + collisionCounter);
            //impactPoint = hit.point;
            if (ricochetAmount > 0 && collisionCounter < ricochetAmount)
            {
                Ricochet(hit);
            }
            else
            {
                PlayImpact();
                Disappear();
            }
        }

        lastPosition = currentPosition;
    }

    private void PlayImpact()
    {
        //Play Impact VFX
        
    }

    private void Ricochet(RaycastHit impact)
    {
        collisionCounter++;
        //Ricochet, 
        //Reset Rigidbody
        projectileBody.velocity = Vector3.zero;
        //Find new Direction
        Vector3 newDirection = Vector3.Reflect(impact.point - lastPosition, impact.normal);
        transform.position = impact.point;
        lastPosition = impact.point;
        newDirection.y = 0;
        projectileBody.AddForce((newDirection.normalized * speed) * projectileBody.mass, ForceMode.Impulse);
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Public Functions

    public void Launch(Vector3 _direction)
    {
        lastPosition = transform.position;
        transform.forward = _direction;
        direction = _direction;
        projectileBody.AddForce((direction * speed) * projectileBody.mass, ForceMode.Impulse);
        
        Debug.Log("Launched! " + direction);
    }

    #endregion
}
