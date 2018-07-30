using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class garageManager : MonoBehaviour {

	public GameObject[] playerCarPrefabs;
	GameObject theCar;
	public Vector3 startPosition;
	public Quaternion quat;
	public Button nextButton, beforeButton;
	public TextMeshProUGUI hizValue, frenValue, manevraValue;
	public TextMeshProUGUI moneyText, scoreText;
	private GameObject kilimler;
	public float rotateSpeed = 10f;
	bool kilimleriDondurelimMi = true, ustBarGuncellensin = false;
	public int prefabIndex = 0;
	private Button hizUpgradeButton, brakeUpgradeButton, manevraUpgradeButton;

	// Use this for initialization
	void Start () {
		kilimler = GameObject.FindGameObjectWithTag ("kilim");
		CreateCar ();	
		ButonaAta ();
		TextSettings ();
		hizUpgradeButton = GameObject.FindGameObjectWithTag ("hizUpgradeButton").GetComponent<Button>();
		brakeUpgradeButton = GameObject.FindGameObjectWithTag ("brakeUpgradeButton").GetComponent<Button>();
		manevraUpgradeButton = GameObject.FindGameObjectWithTag ("manevraUpgradeButton").GetComponent<Button>();
	}

	void kilimleriDondur(bool izin){
		if (izin) {
			kilimler.transform.Rotate (0, rotateSpeed, 0);
		}
	}

	void ButonaAta(){
		nextButton.onClick.AddListener (NextButton);
		beforeButton.onClick.AddListener (BeforeButton);
	}

	void Update(){
		kilimleriDondur (kilimleriDondurelimMi);
		TextSettings ();
		hizValue.SetText (((int)(theCar.GetComponent<CarControllerScript> ().enBuyukHiz)).ToString());
		frenValue.SetText (((int)theCar.GetComponent<CarControllerScript>().brakeForce / 100).ToString());
		manevraValue.SetText (((int)theCar.GetComponent<CarControllerScript> ().manevraBecerisi * 40).ToString());
		
	}

	private void CreateCar(){
		
		theCar = Instantiate (playerCarPrefabs [prefabIndex]) as GameObject;
		theCar.GetComponent<Animator> ().StopPlayback ();
		theCar.GetComponent<Animator> ().enabled = false;
		theCar.transform.position = startPosition;
		theCar.transform.rotation = quat;
		theCar.transform.SetParent (kilimler.transform);
		kilimleriDondurelimMi = true;

		hizValue.SetText (((int)(theCar.GetComponent<CarControllerScript> ().enBuyukHiz)).ToString());
		frenValue.SetText (((int)theCar.GetComponent<CarControllerScript>().brakeForce / 100).ToString());
		manevraValue.SetText (((int)theCar.GetComponent<CarControllerScript> ().manevraBecerisi * 40).ToString());

	}

	public void NextButton(){
		prefabIndex++;
		if (playerCarPrefabs.Length > prefabIndex) {
			kilimleriDondurelimMi = false;
			kilimler.transform.position = Vector3.zero;
			kilimler.transform.rotation = new Quaternion (0, 0, 0, 0);
			DeActiveCurrentCar ();
			CreateCar ();
			kilimleriDondurelimMi = true;
			ControlButtons ();
		}

	}

	public void BeforeButton(){
		prefabIndex--;
		if (prefabIndex >= 0) {
			kilimleriDondurelimMi = false;
			kilimler.transform.position = Vector3.zero;
			kilimler.transform.rotation = new Quaternion (0, 0, 0, 0);
			DeActiveCurrentCar ();
			CreateCar ();
			kilimleriDondurelimMi = true;
			ControlButtons ();
		}

	}

	void DeActiveCurrentCar(){
		GameObject.FindGameObjectWithTag ("Player").SetActive(false);
		Destroy (GameObject.FindGameObjectWithTag ("Player"));
	}

	void ControlButtons(){
		
			if (prefabIndex+1 >= playerCarPrefabs.Length) {
				nextButton.gameObject.SetActive (false);
			} else {
				nextButton.gameObject.SetActive (true);
			}
		
			if (prefabIndex == 0) {
				beforeButton.gameObject.SetActive (false);
			} else {
				beforeButton.gameObject.SetActive (true);
			}

		hizUpgradeButton.gameObject.SetActive (true);
		hizUpgradeButton.enabled = true;

		brakeUpgradeButton.gameObject.SetActive (true);
		brakeUpgradeButton.enabled = true;

		manevraUpgradeButton.gameObject.SetActive (true);
		manevraUpgradeButton.enabled = true;

	}

	void TextSettings(){
		moneyText.text = PlayerPrefs.GetInt ("para", 0).ToString();
		scoreText.text = "En yüksek: " + PlayerPrefs.GetInt ("skorBest", 0).ToString ();
	}
}
