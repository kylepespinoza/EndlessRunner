using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFactory : EndlessRunnerElement {

    //our moving ground prefab (to be made into an array of pieces of different sizes)
    public GameObject[] groundPieces;
    public float distanceFromSpawnPoint = 0f;
    GameObject g;

    int groundPieceIndex;

    public List<GameObject> activeGroundPieces = new List<GameObject>();

    void Start () {
        CreateGroundPiece();
	}
	
	// Update is called once per frame
	void Update () {
        //GroundFactory creates a ground piece which moves along the z axis towards the player
        //Once the ground piece is 250f units away from the factory, another is created
        distanceFromSpawnPoint = transform.position.z - g.transform.position.z;
        if(distanceFromSpawnPoint >= 249f)
        {
            CreateGroundPiece();
        }
	}

    void CreateGroundPiece()
    {
        groundPieceIndex++;
        g = Instantiate(groundPieces[Random.Range(0, groundPieces.Length - 1)], transform.position, Quaternion.identity);
        activeGroundPieces.Add(g);
        g.transform.parent = App.view.gameObject.transform;
        distanceFromSpawnPoint = 0f;
    }


}
