using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class garageManager : MonoBehaviour {

	public GameObject[] playerCarPrefabs;
	public Vector3 startPosition;
	public Quaternion quat;
	public Button nextButton, beforeButton;
	public TextMeshProUGUI hizValue, frenValue, manevraValue;
	public TextMeshProUGUI moneyText, scoreText;
	private GameObject kilimler;
	public float rotateSpeed = 10f;
	bool kilimleriDondurelimMi = true, ustBarGuncellensin = false;
	public int prefabIndex = 0;

	// Use this for initialization
	void Start () {
		kilimler = GameObject.FindGameObjectWithTag ("kilim");
		CreateCar ();	
		ButonaAta ();
		TextSettings ();
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
		if (ustBarGuncellensin) {
			TextSettings ();
		}
	}

	private void CreateCar(){
		GameObject theCar;
		theCar = Instantiate (playerCarPrefabs [prefabIndex]) as GameObject;
		theCar.GetComponent<Animator> ().StopPlayback ();
		theCar.GetComponent<Animator> ().enabled = false;
		theCar.transform.position = startPosition;
		theCar.transform.rotation = quat;
		theCar.transform.SetParent (kilimler.transform);
		kilimleriDondurelimMi = true;

		hizValue.SetText (((int)(theCar.GetComponent<CarControllerScript> ().enBuyukHiz) * (int)(theCar.GetComponent<CarControllerScript>().maxMotorForce) / 2000).ToString());
		frenValue.SetText (((int)theCar.GetComponent<CarControllerScript>().brakeForce / 100).ToString());
		manevraValue.SetText (((int)theCar.GetComponent<CarControllerScript> ().manevraBecerisi * 10).ToString());

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

	}

	void TextSettings(){
		moneyText.text = PlayerPrefs.GetInt ("para").ToString();
		scoreText.text = "En yüksek: " + PlayerPrefs.GetInt ("skorBest").ToString ();
	}
}
