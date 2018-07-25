using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class theGodScript : MonoBehaviour {

	public GameObject[] carPrefabs;
	public GameObject roadManagerP, BotCarCreatorP, potCarLocP;
	public GameObject theCar, BotCarCreator;
	public GameObject Gazbutton, Frenbutton, EngineStartbutton;
	public TextMeshProUGUI speedText, distanceText;
	private GameObject PotansiyelArabaKonumları;

	private Vector3 startPositionPlayer = new Vector3 (0, 0.85f, 0);
	public float posPOTOffsetZ = 20.08f;

	private float distance, hiz;

	private AudioSource audioS;
	// Use this for initialization
	void Start () {
		createCar (Random.Range (0, carPrefabs.Length - 1));
		createManagers ();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		butonaAta ();
		audioS = GetComponent<AudioSource> ();
		distance = 0;
	}

	void createCar(int prefabIndex){
		theCar = Instantiate (carPrefabs [prefabIndex]) as GameObject;
		theCar.transform.position = startPositionPlayer;
	}

	void createManagers(){
		GameObject RoadManager;

		RoadManager = Instantiate (roadManagerP) as GameObject;
		RoadManager.transform.position = Vector3.zero;

		BotCarCreator = Instantiate (BotCarCreatorP) as GameObject;
		BotCarCreator.transform.position = Vector3.zero;

		PotansiyelArabaKonumları = Instantiate (potCarLocP) as GameObject;
		PotansiyelArabaKonumları.transform.position = Vector3.zero;

	}

	void Update(){
		ArabaKonumlarıTransformFix ();
		TextSettings ();
		distance += hiz * Time.smoothDeltaTime / 125;
		//uzaklık = hiz * zaman
		//uzaklık birimi = km
		//hiz birimi = km / saat = km / 3600 saniye
		//zaman birim = saniye
		//km = km / 3600 saniye * 3600 saniye
	}

	void butonaAta(){
		EngineStartbutton.GetComponent<Button> ().onClick.AddListener (EngineStart);
	}

	void TextSettings(){
		hiz = theCar.GetComponent<Rigidbody> ().velocity.magnitude;
		speedText.SetText (((int)(hiz*2.5)).ToString () );

		Vector3 nowPOS = theCar.transform.position;
		float gidilenYolValue = Vector3.Distance (nowPOS, startPositionPlayer);
		distanceText.SetText (((int)gidilenYolValue / 360f * 2.95f).ToString ("F1") );
	}

	void EngineStart(){
		EngineStartbutton.GetComponent<Button> ().onClick.RemoveListener (EngineStart);
		audioS.Play ();
		Invoke ("butonGizle", audioS.clip.length);
	}

	public void butonGizle(){
		EngineStartbutton.SetActive (false);
		Frenbutton.SetActive (true);
		theCar.GetComponent<CarControllerScript> ().enabled = true;
	}

	public void Gaz(bool basili){
		theCar.GetComponent<CarControllerScript> ().Gaz(basili);
	}

	public void Brake(bool basili){
		theCar.GetComponent<CarControllerScript> ().Brake (basili);
	}

	void ArabaKonumlarıTransformFix(){
		Transform theCarT = theCar.transform;
		Transform potansiyelArabaKonumlarıT = PotansiyelArabaKonumları.transform;
		Vector3 posCar = theCarT.position;
		Vector3 posPot = potansiyelArabaKonumlarıT.position;

		Vector3 posRam = posPot;
		posRam.x = 0;
		posRam.z = posCar.z + posPOTOffsetZ;
		posRam.y = 0f;

		potansiyelArabaKonumlarıT.position = posRam;

		Quaternion quat = PotansiyelArabaKonumları.transform.rotation;
		quat.x = quat.y = quat.z = 0;

		Vector3 scale = PotansiyelArabaKonumları.transform.localScale;
		scale.x = scale.y = scale.z = 1;

		potansiyelArabaKonumlarıT.rotation = quat;
		potansiyelArabaKonumlarıT.localScale = scale;
	}



}
