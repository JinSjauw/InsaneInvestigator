using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EvidenceCapture : MonoBehaviour
{
    [SerializeField] private GameEventChannel eventChannel;
    [SerializeField] private List<Interactable> itemsList;

    [SerializeField] private Vector3 boxSize;
    
    private void Start()
    {
        eventChannel.OnEvidenceShot += Shoot;
    }
    
    private void Shoot(object sender, EventArgs e)
    {
        Debug.Log("Shooting Evidence!");
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, boxSize, Vector3.down, quaternion.identity, 5, LayerMask.GetMask("Interactable"));
        if (hits.Length > 1)
        {
            Debug.Log("Score +1");
        }
    }
}

