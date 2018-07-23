using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class theGodScript : MonoBehaviour {

	public GameObject[] carPrefabs;
	public GameObject roadManager;

	public GameObject theCar;

	public GameObject Gazbutton, Frenbutton;

	// Use this for initialization
	void Start () {
		int a = Random.Range (0, carPrefabs.Length - 1);
		createCar (a);
		createRoadManager ();
		butonlaraAta ();
	}

	void createCar(int prefabIndex){
		theCar = Instantiate (carPrefabs [prefabIndex]) as GameObject;
		theCar.transform.position = new Vector3 (0, 0.85f, 0);
	}

	void createRoadManager(){
		GameObject RoadManager;
		RoadManager = Instantiate (roadManager) as GameObject;
		RoadManager.transform.position = Vector3.zero;
	}

	void butonlaraAta(){
		Gazbutton.GetComponent<Button> ().onClick.AddListener (theCar.GetComponent<CarControlScript> ().Gaz);
		Frenbutton.GetComponent<Button> ().onClick.AddListener (theCar.GetComponent<CarControlScript> ().Brake);

	}



}
