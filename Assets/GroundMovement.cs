﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour {

    public float speed;

	void Start () {
	}
	
	void Update () {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
	}
}
