using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadIntelligence : MonoBehaviour {

	public GameObject roadManager;
	private float roadLength;
	public float toleranceOffset = 20f;
	private Transform playerTransform;

	// Use this for initialization
	void Start () {
		roadLength = roadManager.GetComponent<RoadManagerScript> ().roadLength;
		playerTransform = playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if((playerTransform.position.z - transform.position.z - toleranceOffset) > roadLength){
			Destroy(this.gameObject);
		}
	}
}
