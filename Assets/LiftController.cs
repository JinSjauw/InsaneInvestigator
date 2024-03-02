using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiftController : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    private Animator animator;
    private Collider playerObject;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            animator.Play("DoorOpen");
            playerObject = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == playerObject)
        {
            animator.Play("DoorClose");
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
