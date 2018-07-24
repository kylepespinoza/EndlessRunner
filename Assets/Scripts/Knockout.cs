using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockout : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (!playerController.unconscious)
        {
            Debug.Log("Head Collision");
            StartCoroutine(playerController.Knockout_Recover_andReset());
        }
    }
}
