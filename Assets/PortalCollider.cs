using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollider : MonoBehaviour
{
    private LiftController liftController;
    // Start is called before the first frame update
    void Start()
    {
        liftController = transform.parent.GetComponent<LiftController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        liftController.LoadNextLevel();
    }
}
