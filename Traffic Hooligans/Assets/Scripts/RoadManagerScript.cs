using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManagerScript : MonoBehaviour {

	public GameObject yollarChild;
	public GameObject[] yolPrefabs;

	private Transform playerTransform;
	private float spawnZ = 0f;
	public float roadLength;
	public int yolSayisi = 10;

	public float toleranceOffsetDeletingRoad = 20f;
 
	// Use this for initialization
	void Start () {
		roadLength = yolPrefabs [0].GetComponent<MeshFilter> ().sharedMesh.bounds.size.z;
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;

		for (int i = 0; i <= yolSayisi; i++) {
			int a = Random.Range (0, yolPrefabs.Length - 1);
			spawnRoad (a);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerTransform.position.z > spawnZ - roadLength * yolSayisi) {
			int a = Random.Range (0, yolPrefabs.Length - 1);
			spawnRoad (a);
		}
	}

	private void spawnRoad(int b){
		GameObject yol;
		yol = Instantiate (yolPrefabs [b]) as GameObject;
		yol.transform.SetParent (yollarChild.transform);
		yol.transform.position = Vector3.forward * spawnZ;
		spawnZ += roadLength;

	}


}
