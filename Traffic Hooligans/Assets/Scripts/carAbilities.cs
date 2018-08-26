using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class carAbilities : MonoBehaviour {
	public string gOName;
	public string garajSahneIsmi = "Garage";
	int hizUpgradeLevel, brakeUpgradeLevel, manevraUpgradeLevel;
	public int myPrice;
	private Button hizUpgradeButton, brakeUpgradeButton, manevraUpgradeButton;
	public Sprite[] sprites;
	[SerializeField]
	private Image upgradeLevelSpeed, upgradeLevelBrake, upgradeLevelManevra;
	GameObject garageManager;
	[SerializeField]
	private TextMeshProUGUI kacParaLazim;
	public int ozellikGelistirmeUcreti = 400;
	// Use this for initialization
	void Start () {
		gOName = this.gameObject.name;
		if (SceneManager.GetActiveScene ().name == garajSahneIsmi && PlayerPrefs.GetString(gOName,"satinAlinmadi") == "satinAlindi") {
			hizUpgradeButton = GameObject.FindGameObjectWithTag ("hizUpgradeButton").GetComponent<Button> ();
			brakeUpgradeButton = GameObject.FindGameObjectWithTag ("brakeUpgradeButton").GetComponent<Button> ();
			manevraUpgradeButton = GameObject.FindGameObjectWithTag ("manevraUpgradeButton").GetComponent<Button> ();
			hizUpgradeLevel = PlayerPrefs.GetInt (gOName + "hiz", 1);
			brakeUpgradeLevel = PlayerPrefs.GetInt (gOName + "brake", 1);
			manevraUpgradeLevel = PlayerPrefs.GetInt (gOName + "manevra", 1);
			upgradeLevelSpeed = GameObject.FindGameObjectWithTag ("upgradeLevelSpeed").GetComponent<Image> ();
			upgradeLevelBrake = GameObject.FindGameObjectWithTag ("upgradeLevelBrake").GetComponent<Image> ();
			upgradeLevelManevra = GameObject.FindGameObjectWithTag ("upgradeLevelManevra").GetComponent<Image> ();
			garageManager = GameObject.FindGameObjectWithTag ("garageManager");
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
		if (a <= 2 && PlayerPrefs.GetFloat ("para", 0) >= (a + 1) * ozellikGelistirmeUcreti) {
			PlayerPrefs.SetInt (gOName + ability, a + 1);
			if (ability == "hiz") {
				upgradeLevelSpeed.sprite = sprites [a];
				GetComponent<CarControllerScript> ().enBuyukHiz += 2;
				GetComponent<CarControllerScript> ().maxMotorForce += 100;
				PlayerPrefs.SetFloat (gOName + "motorvalue", GetComponent<CarControllerScript> ().maxMotorForce);
				PlayerPrefs.SetFloat (gOName + ability + "value", GetComponent<CarControllerScript> ().enBuyukHiz);
			} else if (ability == "brake") {
				upgradeLevelBrake.sprite = sprites [a];
				GetComponent<CarControllerScript> ().brakeForce += 500;
				PlayerPrefs.SetFloat (gOName + ability + "value", GetComponent<CarControllerScript> ().brakeForce);
			} else if (ability == "manevra") {
				upgradeLevelManevra.sprite = sprites [a];
				GetComponent<CarControllerScript> ().manevraBecerisi += 0.25f / 2;
				PlayerPrefs.SetFloat (gOName + ability + "value", GetComponent<CarControllerScript> ().manevraBecerisi);
			}

			PlayerPrefs.SetFloat ("para", PlayerPrefs.GetFloat ("para") - (a + 1) * ozellikGelistirmeUcreti);
		} else {
			garageManager.GetComponent<garageManager> ().paraYokPanel.SetActive (true);
			kacParaLazim = garageManager.GetComponent<garageManager> ().kacParaLazim;
			kacParaLazim.SetText (((a + 1) * ozellikGelistirmeUcreti - PlayerPrefs.GetFloat("para", 0)).ToString() + " PARA LAZIM!");
		}
	}

	void Update(){
		butonlariVeResimleriKontrolEt ();
		OzellikleriGecir ();
	}

	void butonlariVeResimleriKontrolEt(){
		if (SceneManager.GetActiveScene ().name == garajSahneIsmi) {
			upgradeLevelSpeed = GameObject.FindGameObjectWithTag ("upgradeLevelSpeed").GetComponent<Image> ();
			upgradeLevelBrake = GameObject.FindGameObjectWithTag ("upgradeLevelBrake").GetComponent<Image> ();
			upgradeLevelManevra = GameObject.FindGameObjectWithTag ("upgradeLevelManevra").GetComponent<Image> ();


			if (PlayerPrefs.GetString (gOName, "satinAlinmadi") == "satinAlindi") {
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

			}

			upgradeLevelSpeed.sprite = sprites [PlayerPrefs.GetInt (gOName + "hiz", 1) - 1];
			upgradeLevelBrake.sprite = sprites [PlayerPrefs.GetInt (gOName + "brake", 1) - 1];
			upgradeLevelManevra.sprite = sprites [PlayerPrefs.GetInt (gOName + "manevra", 1) - 1];
		}


	}

	void OzellikleriGecir(){
		float enBuyukHiz, maxMotorForce, brakeForce, manevraBecerisi;

		maxMotorForce = PlayerPrefs.GetFloat (gOName + "motorvalue", GetComponent<CarControllerScript> ().maxMotorForce); 
		enBuyukHiz = PlayerPrefs.GetFloat (gOName + "hizvalue", GetComponent<CarControllerScript>().enBuyukHiz);
		brakeForce = PlayerPrefs.GetFloat (gOName + "brakevalue", GetComponent<CarControllerScript>().brakeForce);
		manevraBecerisi = PlayerPrefs.GetFloat (gOName + "manevravalue", GetComponent<CarControllerScript>().manevraBecerisi);

		GetComponent<CarControllerScript> ().maxMotorForce = maxMotorForce;
		GetComponent<CarControllerScript> ().enBuyukHiz = enBuyukHiz;
		GetComponent<CarControllerScript> ().brakeForce = brakeForce;
		GetComponent<CarControllerScript> ().manevraBecerisi = manevraBecerisi;
	
	}
		
}
