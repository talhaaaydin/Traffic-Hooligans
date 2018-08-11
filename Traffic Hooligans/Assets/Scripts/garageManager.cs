using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class garageManager : MonoBehaviour {

	public GameObject[] playerCarPrefabs;
	GameObject theCar;
	public GameObject lockPanel, paraYokPanel;
	public Vector3 startPosition;
	public Quaternion quat;
	public Button nextButton, beforeButton, playButton, buyButton;
	public TextMeshProUGUI hizValue, frenValue, manevraValue, carPriceText;
	public TextMeshProUGUI moneyText, scoreText, kacParaLazim;
	private GameObject kilimler;
	public float rotateSpeed = 10f;
	bool kilimleriDondurelimMi = true;
	public string baslangicArabasiIsim = "car3", anaSahneIsmi = "mainScene";
	public int prefabIndex = 0;
	private Button hizUpgradeButton, brakeUpgradeButton, manevraUpgradeButton;

	// Use this for initialization
	void Start () {
		baslangicArabasiIsim += "(Clone)";
		kilimler = GameObject.FindGameObjectWithTag ("kilim");
		hizUpgradeButton = GameObject.FindGameObjectWithTag ("hizUpgradeButton").GetComponent<Button>();
		brakeUpgradeButton = GameObject.FindGameObjectWithTag ("brakeUpgradeButton").GetComponent<Button>();
		manevraUpgradeButton = GameObject.FindGameObjectWithTag ("manevraUpgradeButton").GetComponent<Button>();
		CreateCar ();	
		TextSettings ();
		ButonaAta ();
		if (GameObject.FindGameObjectWithTag ("Player").name == baslangicArabasiIsim) {
			PlayerPrefs.SetString (baslangicArabasiIsim, "satinAlindi");
			UnLock ();
		}
	}

	void kilimleriDondur(bool izin){
		if (izin) {
			kilimler.transform.Rotate (0, rotateSpeed, 0);
		}
	}

	void ButonaAta(){
		nextButton.onClick.AddListener (NextButton);
		beforeButton.onClick.AddListener (BeforeButton);
		playButton.onClick.AddListener (PlayButton);
		buyButton.onClick.AddListener (BuyButton);
	}

	void PlayButton(){
		PlayerPrefs.SetInt ("theCarIndex", prefabIndex);
		SceneManager.LoadScene (anaSahneIsmi);
	}

	void BuyButton(){
		float price = float.Parse (carPriceText.text);
		if (PlayerPrefs.GetFloat ("para", 0) >= price) {
			PlayerPrefs.SetString (theCar.name, "satinAlindi");
			LockOrUnlockedCar (theCar.name);
		} else {
			paraYokPanel.SetActive (true);
			kacParaLazim.text = (price - PlayerPrefs.GetFloat ("para", 0)).ToString () + " PARA LAZIM!";
		}
	}

	void Update(){
		kilimleriDondur (kilimleriDondurelimMi);
		TextSettings ();
		hizValue.SetText (((int)(theCar.GetComponent<CarControllerScript> ().enBuyukHiz * theCar.GetComponent<CarControllerScript>().maxMotorForce / 6000)).ToString());
		frenValue.SetText (((int)(theCar.GetComponent<CarControllerScript>().brakeForce / 100)).ToString());
		manevraValue.SetText (Mathf.RoundToInt(theCar.GetComponent<CarControllerScript> ().manevraBecerisi * 40).ToString());
		
	}

	private void CreateCar(){
		
		theCar = Instantiate (playerCarPrefabs [prefabIndex]) as GameObject;
		theCar.GetComponent<Animator> ().StopPlayback ();
		theCar.GetComponent<Animator> ().enabled = false;
		theCar.transform.position = startPosition;
		theCar.transform.rotation = quat;
		theCar.transform.SetParent (kilimler.transform);
		kilimleriDondurelimMi = true;
		string theName = theCar.name;

		hizValue.SetText (((int)(theCar.GetComponent<CarControllerScript> ().enBuyukHiz * theCar.GetComponent<CarControllerScript>().maxMotorForce  / 6000)).ToString());
		frenValue.SetText (Mathf.RoundToInt(theCar.GetComponent<CarControllerScript>().brakeForce / 100).ToString());
		manevraValue.SetText (Mathf.RoundToInt(theCar.GetComponent<CarControllerScript> ().manevraBecerisi * 40).ToString());


		LockOrUnlockedCar (theName);

	}

	public void NextButton(){
		prefabIndex++;
		if (playerCarPrefabs.Length > prefabIndex) {
			kilimleriDondurelimMi = false;
			kilimler.transform.position = Vector3.zero;
			kilimler.transform.rotation = new Quaternion (0, 0, 0, 0);
			DeActiveCurrentCar ();
			ControlButtons ();
			CreateCar ();
			kilimleriDondurelimMi = true;
		}

	}

	public void BeforeButton(){
		prefabIndex--;
		if (prefabIndex >= 0) {
			kilimleriDondurelimMi = false;
			kilimler.transform.position = Vector3.zero;
			kilimler.transform.rotation = new Quaternion (0, 0, 0, 0);
			DeActiveCurrentCar ();
			ControlButtons ();
			CreateCar ();
			kilimleriDondurelimMi = true;
		}
	}

	void Lock(){
		lockPanel.SetActive (true);
		hizUpgradeButton.gameObject.SetActive (false);
		brakeUpgradeButton.gameObject.SetActive (false);
		manevraUpgradeButton.gameObject.SetActive (false);
		carPriceText.text = theCar.GetComponent<carAbilities> ().myPrice.ToString ();
		buyButton.gameObject.SetActive(true);
		playButton.gameObject.SetActive(false);
	}

	void UnLock(){
		lockPanel.SetActive (false);
		hizUpgradeButton.gameObject.SetActive (true);
		brakeUpgradeButton.gameObject.SetActive (true);
		manevraUpgradeButton.gameObject.SetActive (true);
		//yedek
		buyButton.gameObject.SetActive(false);
		playButton.gameObject.SetActive(true);
	}

	void LockOrUnlockedCar(string s){
		if (PlayerPrefs.GetString (s, "satinAlinmadi") == "satinAlinmadi") {
			Lock ();
		} else if (PlayerPrefs.GetString (s, "satinAlinmadi") == "satinAlindi") {
			UnLock ();
		}
	}

	void DeActiveCurrentCar(){
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
		moneyText.text = PlayerPrefs.GetFloat ("para", 0).ToString();
		scoreText.text = "En yUksek: " + PlayerPrefs.GetFloat ("skorBest", 0).ToString ();
	}
}
