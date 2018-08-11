using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCarManager : MonoBehaviour {
	private GameObject PotantielCarLocations;
	public GameObject[] BotCarPrefabs;
	private Transform BotCarParent;
	[SerializeField]
	private GameObject theCar;
	public GameObject theGod;
	private Vector3 Position, pos;
	public int CreatedBotCars = 0;
	public int limitCreatingBots = 3;
	private float unitToKmH, skorHiz;
	// Use this for initialization
	void Start () {
		BotCarParent = GameObject.FindGameObjectWithTag ("BotCarParent").transform;
		PotantielCarLocations = GameObject.FindGameObjectWithTag ("PotansiyelArabaKonumları");
		theCar = GameObject.FindGameObjectWithTag ("Player");
		theGod = GameObject.FindGameObjectWithTag ("theGod");
		unitToKmH = theGod.GetComponent<theGodScript> ().unitToKmH;
		skorHiz = theGod.GetComponent<theGodScript> ().skorHiz;
		StartCoroutine (BotCarRegulator ());
	}

	void FixedUpdate(){
		Position = PotantielCarLocations.transform.position;
	}

	void spawnCar(int carPrefabIndex, int locationIndex){
		GameObject BotCar;
		float carLength = 0;
		BotCar = Instantiate (BotCarPrefabs [carPrefabIndex]) as GameObject;
		if (CreatedBotCars != 0) {
			carLength = BotCar.transform.GetChild(0).GetComponent<MeshFilter> ().sharedMesh.bounds.size.z;
		}
		Vector3 pos = PotantielCarLocations.transform.GetChild (locationIndex).transform.localPosition;
		Position.z += carLength * 2;
		BotCar.transform.SetParent (BotCarParent);
		Position.x = pos.x;
		Position.y = pos.y;

		Quaternion quat;
		quat = BotCar.transform.rotation;
		quat.x = quat.z = 0; quat.y = 180;
		BotCar.transform.rotation = quat;
		if (ITAGOOTP (Position)) {
			BotCar.transform.position = Position;
			CreatedBotCars++;
		} else {
			
		}

	}

	void transformOldCar(int locationIndex){
		Vector3 pos = PotantielCarLocations.transform.GetChild (locationIndex).transform.localPosition;
		Transform a = BotCarParent.GetChild(0).transform;
		float carLength = a.transform.GetChild(0).GetComponent<MeshFilter> ().sharedMesh.bounds.size.z;
		a.parent = null;
		pos.z = Position.z + carLength * 2;
		if (ITAGOOTP (pos)) {
			a.position = pos;
			Quaternion quat;
			quat = a.rotation;
			quat.x = quat.z = 0; quat.y = 180;
			a.rotation = quat;
			a.SetParent (BotCarParent);
		} else {
			
		}


	}

	IEnumerator BotCarRegulator(){
		float hiz, enKucukHiz, enBuyukHiz;
		CarControllerScript theCarS = theCar.GetComponent<CarControllerScript> ();
		enKucukHiz = theCarS.enKucukHiz;
		enBuyukHiz = theCarS.enBuyukHiz;
		hiz = theCarS.hiz;

		if (hiz >= enKucukHiz && Time.timeScale == 1) {
			if (BotCarParent.childCount <= limitCreatingBots) {
				spawnCar (Random.Range (0, BotCarPrefabs.Length), Random.Range (0, 4));
			} else {
				transformOldCar (Random.Range (0, 4));
			}
		}


		float delayTime;
		if (enBuyukHiz != skorHiz - 10) {
			if (hiz * unitToKmH >= skorHiz || theCarS.hiz >= theCarS.enBuyukHiz) {
				delayTime = 0.5f;
			} else if (hiz * unitToKmH < skorHiz && hiz * unitToKmH > skorHiz - 10) {
				delayTime = 1f;
			} else {
				delayTime = 1.5f;
			}
		} else {
			if (hiz * unitToKmH >= skorHiz || theCarS.hiz >= theCarS.enBuyukHiz) {
				delayTime = 0.5f;
			} else if (hiz * unitToKmH < skorHiz && hiz * unitToKmH > skorHiz - 11) {
				delayTime = 1f;
			} else {
				delayTime = 1.5f;
			}
		}
		yield return new WaitForSecondsRealtime (delayTime);
		StartCoroutine (BotCarRegulator ());
	}

	private bool ITAGOOTP(Vector3 pos){
		//O pozisyonda hiç bir nesne var mı?
		//Is There Any GameObject On The Position

		RaycastHit hit;
		bool a = Physics.Raycast (pos, transform.TransformDirection (Vector3.down), out hit, 2);
		if (a) {
			if (hit.collider.gameObject.CompareTag ("BotCar")) {
				return false;
			} else {
				return true;
			}
		} else {
			return true;
		}

	}
}
