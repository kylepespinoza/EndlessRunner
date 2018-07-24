using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSizer : MonoBehaviour {

    public float minimumSize, maximumSize;

	void Start () {
        transform.localScale = new Vector3(Random.Range(minimumSize, maximumSize), Random.Range(minimumSize, 2), Random.Range(minimumSize, maximumSize));
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
	}
	
	void Update () {
		
	}
}
