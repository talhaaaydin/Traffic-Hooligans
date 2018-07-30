using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class carAbilities : MonoBehaviour {
	string gOName;
	public string garajSahneIsmi = "Garage";
	int hizUpgradeLevel, brakeUpgradeLevel, manevraUpgradeLevel;
	private Button hizUpgradeButton, brakeUpgradeButton, manevraUpgradeButton;
	public Sprite[] sprites;
	[SerializeField]
	private Image upgradeLevelSpeed, upgradeLevelBrake, upgradeLevelManevra;

	// Use this for initialization
	void Start () {
		gOName = name;
		if (SceneManager.GetActiveScene ().name == garajSahneIsmi) {
			hizUpgradeButton = GameObject.FindGameObjectWithTag ("hizUpgradeButton").GetComponent<Button> ();
			brakeUpgradeButton = GameObject.FindGameObjectWithTag ("brakeUpgradeButton").GetComponent<Button> ();
			manevraUpgradeButton = GameObject.FindGameObjectWithTag ("manevraUpgradeButton").GetComponent<Button> ();
			hizUpgradeLevel = PlayerPrefs.GetInt (gOName + "hiz", 1);
			brakeUpgradeLevel = PlayerPrefs.GetInt (gOName + "brake", 1);
			manevraUpgradeLevel = PlayerPrefs.GetInt (gOName + "manevra", 1);
			upgradeLevelSpeed = GameObject.FindGameObjectWithTag ("upgradeLevelSpeed").GetComponent<Image> ();
			upgradeLevelBrake = GameObject.FindGameObjectWithTag ("upgradeLevelBrake").GetComponent<Image> ();
			upgradeLevelManevra = GameObject.FindGameObjectWithTag ("upgradeLevelManevra").GetComponent<Image> ();
		}

	}

	public void Upgrade(string ability){
		int a = 0;
		switch(ability){
		case "hiz":
			a = hizUpgradeLevel;
			break;
		case "brake":
			a = brakeUpgradeLevel;
			break;
		case "manevra":
			a = manevraUpgradeLevel;
			break;
		}
		if (a <= 2) {
			PlayerPrefs.SetInt (gOName + ability, a + 1);
			if (ability == "hiz") {
				upgradeLevelSpeed.sprite = sprites [a];
				Debug.Log (GetComponent<CarControllerScript> ().enBuyukHiz);
				GetComponent<CarControllerScript> ().enBuyukHiz += 5;
				Debug.Log (GetComponent<CarControllerScript> ().enBuyukHiz);
				PlayerPrefs.SetFloat (gOName + ability+"value", GetComponent<CarControllerScript> ().enBuyukHiz);
			} else if (ability == "brake") {
				upgradeLevelBrake.sprite = sprites [a];
				GetComponent<CarControllerScript> ().brakeForce += (500 - 99 *GetComponent<CarControllerScript>().brakeForce) / 100;
				PlayerPrefs.SetFloat (gOName + ability+"value", GetComponent<CarControllerScript> ().brakeForce);
			} else if (ability == "manevra") {
				upgradeLevelManevra.sprite = sprites [a];
				GetComponent<CarControllerScript> ().manevraBecerisi += GetComponent<CarControllerScript> ().manevraBecerisi * 39 + 1;
				PlayerPrefs.SetFloat (gOName + ability+"value", GetComponent<CarControllerScript> ().manevraBecerisi);
			}
		}
	}

	void Update(){
		butonlariVeResimleriKontrolEt ();
		OzellikleriGecir ();
	}

	void butonlariVeResimleriKontrolEt(){
		if (SceneManager.GetActiveScene ().name == garajSahneIsmi) {
			hizUpgradeLevel = PlayerPrefs.GetInt (gOName + "hiz", 1);
			brakeUpgradeLevel = PlayerPrefs.GetInt (gOName + "brake", 1);
			manevraUpgradeLevel = PlayerPrefs.GetInt (gOName + "manevra", 1);
			if (hizUpgradeLevel <= 2) {
				hizUpgradeButton.gameObject.SetActive (true);
				hizUpgradeButton.enabled = true;
			} else {
				hizUpgradeButton.gameObject.SetActive (false);
				hizUpgradeButton.enabled = false;
			}	
			if (brakeUpgradeLevel <= 2) {
				brakeUpgradeButton.gameObject.SetActive (true);
				brakeUpgradeButton.enabled = true;
			} else {
				brakeUpgradeButton.gameObject.SetActive (false);
				brakeUpgradeButton.enabled = false;
			}

			if (manevraUpgradeLevel <= 2) {
				manevraUpgradeButton.gameObject.SetActive (true);
				manevraUpgradeButton.enabled = true;
			} else {
				manevraUpgradeButton.gameObject.SetActive (false);
				manevraUpgradeButton.enabled = false;
			}

			upgradeLevelSpeed.sprite = sprites [PlayerPrefs.GetInt (gOName + "hiz", 1) - 1];
			upgradeLevelBrake.sprite = sprites [PlayerPrefs.GetInt (gOName + "brake", 1) - 1];
			upgradeLevelManevra.sprite = sprites [PlayerPrefs.GetInt (gOName + "manevra", 1) - 1];
		}
	}

	void OzellikleriGecir(){
		float enBuyukHiz, brakeForce, manevraBecerisi;

		enBuyukHiz = PlayerPrefs.GetFloat (gOName + "hizvalue", GetComponent<CarControllerScript>().enBuyukHiz);
		brakeForce = PlayerPrefs.GetFloat (gOName + "brakevalue", GetComponent<CarControllerScript>().brakeForce);
		manevraBecerisi = PlayerPrefs.GetFloat (gOName + "manevravalue", GetComponent<CarControllerScript>().manevraBecerisi);
		GetComponent<CarControllerScript> ().enBuyukHiz = enBuyukHiz;
		GetComponent<CarControllerScript> ().brakeForce = brakeForce;
		GetComponent<CarControllerScript> ().manevraBecerisi = manevraBecerisi;
	
	}
		
}
