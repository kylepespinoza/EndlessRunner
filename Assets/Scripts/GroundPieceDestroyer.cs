using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPieceDestroyer : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Attempting to delete Ground Piece");
        Destroy(collision.transform.parent.gameObject);

    }

}
