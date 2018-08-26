using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManagerScript : MonoBehaviour {

	private GameObject yollarParent;
	public GameObject[] yolPrefabs;
	private Transform playerTransform;
	private float spawnZ = -5f;
	public float roadLength;
	public int yolSayisi = 10;
	public float toleranceOffset = 20f;
 
	// Use this for initialization
	void Start () {
		roadLength = yolPrefabs [0].GetComponent<MeshFilter> ().sharedMesh.bounds.size.z;
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		yollarParent = GameObject.FindGameObjectWithTag ("yollarParent");
		for (int i = 0; i <= yolSayisi; i++) {
			int a = Random.Range (0, yolPrefabs.Length);
			spawnRoad (a);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerTransform.position.z - toleranceOffset> spawnZ - roadLength * yolSayisi) {
			//int a = Random.Range (0, yolPrefabs.Length);
			//spawnRoad (a);
			transformOldRoad ();
		}
	}

	private void spawnRoad(int b){
		GameObject yol;
		yol = Instantiate (yolPrefabs [b]) as GameObject;
		yol.transform.SetParent (yollarParent.transform);
		yol.transform.position = Vector3.forward * spawnZ;
		spawnZ += roadLength;

	}

	private void transformOldRoad(){
		Transform a = yollarParent.transform.GetChild (0).transform;
		a.parent = null;
		a.SetParent (yollarParent.transform);
		a.position = Vector3.forward * spawnZ;

		spawnZ += roadLength;
	}


}
