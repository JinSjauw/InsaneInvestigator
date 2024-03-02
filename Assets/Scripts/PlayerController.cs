using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    //Settings
    [SerializeField] private bool debug;

    [Header("Component References")] 
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerBody;
    
    [Header("Gravity Scale")] 
    [SerializeField] private float gravity;

    [Header("Locomotion - Suspension")] 
    [SerializeField] private float rideHeight;
    //[SerializeField] private float rayLength;
    [SerializeField] private float rideSpringStrength;
    [SerializeField] private float rideSpringDamper;

    [FormerlySerializedAs("maxSpeed")]
    [Header("Locomotion - Moving")] 
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float maxSprintSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private AnimationCurve accelerationFactor;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private AnimationCurve maxAccelerationFactor;

    [Header("Locomotion - Impulses")] 
    [SerializeField] private float jumpHeight;
    [SerializeField] private float dodgeLength;
    [SerializeField] private float invincibilityTime;
    
    [Header("Locomotion - Rotation")] 
    [SerializeField] private float rotationSpeed;
    
    [Header("Locomotion - Targeting")] 
    [SerializeField] private Transform trackedTarget;
    
    [Header("Combat - Health")]
    [SerializeField] private float maxHealth;
    private float m_Health;

    [Header("Combat - Projectile")] 
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Transform projectilePrefab;

    [Header("Interaction")] 
    [SerializeField] private float interactionRadius;
    private Active closestInteractable;
    
    //Component Refs
    private Rigidbody m_PlayerBody;

    //Physics
    private Vector3 m_GoalVelocity;

    private Vector2 m_MouseInput;
    private Vector2 m_MoveInput;
    private bool m_IsSprinting;
    private bool m_IsDodging;
    private bool m_IsGrounded;

    //Camera
    private float m_VerticalRotation;
    private float m_HorizontalRotation;
    
    
    #region Unity Functions

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        m_PlayerBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        m_Health = maxHealth;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (debug)
        {
            //Log("Move Input: " + m_MoveInput);
        }

        CheckForInteractables();
        MoveBody(m_MoveInput);
        HoverBody();
    }

    private void Update()
    {
        Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit cameraRayHit, Int32.MaxValue, LayerMask.GetMask("Ground")))
        {
            m_MouseInput = new Vector2(cameraRayHit.point.x, cameraRayHit.point.z);
            //Log("Mouse Input:" + m_MouseInput);
        }
        LookAtMouse();
    }

    #endregion
    
    #region Private Functions

    private void HoverBody()
    {
        //Cast ray downwards
        Vector3 down = transform.TransformDirection(Vector3.down);
        m_PlayerBody.AddForce(down * (m_PlayerBody.mass * gravity));
        if (Physics.Raycast(transform.position, down, out RaycastHit rayHit, rideHeight, LayerMask.GetMask("Ground")))
        {
            m_IsGrounded = true;
            Debug.DrawLine(transform.position, rayHit.point, Color.red);
            
            Vector3 velocity = m_PlayerBody.velocity;

            float downDirVelocity = Vector3.Dot(down, velocity);

            float offset = rayHit.distance - rideHeight;

            float springForce = m_PlayerBody.mass * ((offset * rideSpringStrength) - (downDirVelocity * rideSpringDamper));
            
            m_PlayerBody.AddForce(down * springForce);
        }
        else
        {
            m_IsGrounded = false;
        }
    }

    private void MoveBody(Vector2 moveDir)
    {
        Vector3 goalDir = new Vector3(moveDir.x, 0, moveDir.y);
        goalDir = transform.TransformDirection(goalDir);
        Vector3 unitVelocity = m_GoalVelocity.normalized;

        if (goalDir != Vector3.zero)
        {
            playerBody.forward = goalDir;
        }
        
        float velocityDot = Vector3.Dot(goalDir, unitVelocity);

        float accel = acceleration * accelerationFactor.Evaluate(velocityDot);
        
        Vector3 goalVelocity = goalDir * (m_IsSprinting ? maxSprintSpeed : maxWalkSpeed);
        //Log("goalVelocity: " + goalVelocity + "m_GoalVelocity: " + m_GoalVelocity + " accel: " + accel * Time.fixedDeltaTime);
        m_GoalVelocity = Vector3.MoveTowards(m_GoalVelocity, goalVelocity,
            accel * Time.fixedDeltaTime);

        Vector3 neededAcceleration = (m_GoalVelocity - m_PlayerBody.velocity) / Time.fixedDeltaTime;
        
        float maxAccel = maxAcceleration * maxAccelerationFactor.Evaluate(velocityDot);

        neededAcceleration = Vector3.ClampMagnitude(neededAcceleration, maxAccel);
        
        m_PlayerBody.AddForce(Vector3.Scale(neededAcceleration * m_PlayerBody.mass, new Vector3(1, 0, 1)));
    }

    private void RotateToMoveDirection()
    {
        playerBody.forward = new Vector3(m_MoveInput.x, 0, m_MoveInput.y);
    }
    
    private void LookAtMouse()
    {
        //m_VerticalRotation -= m_MouseInput.y * rotationSpeed;
        m_VerticalRotation = Mathf.Clamp(m_VerticalRotation, -90f, 90f);
        m_HorizontalRotation = m_MouseInput.x * rotationSpeed;
        
        //transform.LookAt(new Vector3(m_MouseInput.x, transform.position.y, m_MouseInput.y), Vector3.up);
        
        //Log("Mouse Position: " + m_MouseInput);
        
        //playerCamera.transform.localEulerAngles = Vector3.right * m_VerticalRotation;
    }

    private void Jump()
    {
        animator.SetBool("IsGrounded", m_IsGrounded);
        if (!m_IsGrounded) return;
        Vector3 playerVelocity = m_PlayerBody.velocity;
        m_PlayerBody.velocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        m_PlayerBody.AddForce(m_PlayerBody.mass * ( Vector3.up * jumpHeight), ForceMode.Impulse);
        animator.Play("Jump");
    }

    private void Dodge()
    {
        if (!m_IsGrounded) return;
        m_PlayerBody.AddForce(m_PlayerBody.mass * ( transform.TransformDirection(new Vector3(m_MoveInput.x, 0, m_MoveInput.y)) * dodgeLength), ForceMode.Impulse);
    }
    
    private void Shoot()
    {
        //Spawn projectile
        //Launch Projectile
        //Projectile Can Ricochet
        Vector3 aimingDirection = firingPoint.forward;
        aimingDirection.y = 0;
        Instantiate(projectilePrefab, firingPoint.position, quaternion.identity).GetComponent<Projectile>().Launch(aimingDirection);
    }

    private void Interact()
    {
        if (closestInteractable != null)
        {
            Debug.Log("Interacted! " + closestInteractable.name);
            closestInteractable.Interact();
        }
    }

    private void CheckForInteractables()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, LayerMask.GetMask("Interactable"));
        
        if (hits.Length <= 0)
        {
            closestInteractable = null;
            return;
        }
        
        if (hits.Length > 1)
        {
            float closestDistance = interactionRadius * 2;
            closestInteractable = hits[0].GetComponent<Active>();
            foreach (Collider hit in hits)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (closestDistance > distance)
                {
                    closestDistance = distance;
                    closestInteractable = hit.GetComponent<Active>();
                }
            }
        }
        else
        {
            closestInteractable = hits[0].GetComponent<Active>();
        }
        
        //Debug.Log("Hit: " + closestInteractable);
    }
    
    private void Log(string _msg)
    {
        if (!debug) return;
        Debug.Log("[PlayerController]: " + _msg);
    }

    #endregion

    #region Public Functions
    //Interface Functions
    public void TakeDamage(float damage)
    {
        m_Health -= damage;

        float fillValue = Mathf.Clamp01(m_Health / maxAcceleration);

        if (m_Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        
    }

    #endregion
    
    #region Input Callbacks

    public void OnMove(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            animator.SetFloat("Blend", m_IsSprinting ? .8f : .35f);
            m_MoveInput = _context.ReadValue<Vector2>();
            //Update the vector;
        }

        if (_context.canceled)
        {
            animator.SetFloat("Blend", 0f);
            m_MoveInput = Vector2.zero;
        }
    }

    public void OnSprint(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            Log("[OnSprint]: SPRINTING");
            m_IsSprinting = true;
        }

        if (_context.canceled)
        {
            m_IsSprinting = false;
        }
    }

    public void OnDodge(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            Log("[OnDodge]: DODGING");
            Dodge();
        }
    }

    public void OnJump(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            Log("[OnJump]: JUMP!");
            Jump();
        }
    }

    public void OnShoot(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            Log("[OnShoot]: SHOOT!");
            Shoot();
        }
    }

    public void OnInteract(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            Interact();
            //Check If anything is inRange
        }
    }
    
    #endregion

    
}
