using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ContrabandDispenser : MonoBehaviour
{
    [SerializeField] private List<Transform> contrabandList;
    [SerializeField] private Transform dispensePoint;
    [SerializeField] private float dispensePower;
    
    public void OnDispense(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            //Select random piece of contraband
            Rigidbody contrabandBody = Instantiate(contrabandList[Random.Range(0, contrabandList.Count)],
                dispensePoint.position, quaternion.identity).GetComponent<Rigidbody>();
            //Shoot it forwards
            contrabandBody.transform.up = dispensePoint.forward;
            contrabandBody.AddForce((dispensePoint.forward * dispensePower) * contrabandBody.mass, ForceMode.Impulse);
            contrabandBody.transform.forward = Vector3.up;
        }
    }
}
