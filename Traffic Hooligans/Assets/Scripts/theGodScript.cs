using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class theGodScript : MonoBehaviour {

	private AudioSource audioS;
	public GameObject[] carPrefabs;
	public GameObject roadManagerP, BotCarCreatorP, potCarLocP;
	public GameObject theCar, BotCarCreator, MainCamera;
	public GameObject Gazbutton, Frenbutton, EngineStartbutton, yuksekHizGosterge, sollamaGosterge;
	private GameObject PotansiyelArabaKonumları;
	public TextMeshProUGUI speedText, distanceText, skorText, skorPanelText, katedilenMesafeValueText, katedilenMesafeMoneyText, yakinMakasValueText, yakinMakasMoneyText;
	public TextMeshProUGUI seksen5kmhustuValueText, seksen5kmhUstuMoneyText, yuksekHizValueText, sollamaValueText, toplamMoneyText;

	public TMP_ColorGradient seksen5alti, seksen5ustu;

	private Vector3 startPositionPlayer = new Vector3 (0, 0.85f, 0);
	public float posPOTOffsetZ = 20.08f;
	private float distance, hiz, yuksekHizTime;
	public float yakinMakas = 0;
	public float skor;
	public float skorHiz = 85f;
	public float katedilenMesafeParaKatSayisi, yakinMakasMesafeParaKatSayisi, seksen5kmhUstuParaKatSayisi;
	private bool hizlaniyor;
	public float unitToKmH = 10 / 3;

	public float gidilenYolParasi = 0, yuksekHizParasi = 0, yakinMakasParasi = 0, toplamPara = 0;

	// Use this for initialization
	void Start () {
		skor = distance = hiz = yuksekHizTime = 0;
		int prefabIndex = PlayerPrefs.GetInt ("theCarIndex", 0);
		createCar (prefabIndex);
		createManagers ();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		butonaAta ();
		audioS = GetComponent<AudioSource> ();
		distance = 0;
		Time.timeScale = 1;
		yakinMakas = 0;
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

		MainCamera.GetComponent<camerafollow> ().objectToFollow = theCar.transform;

	}

	void Update(){
		ArabaKonumlarıTransformFix ();
		TextSettings ();
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

		//hiz göstergesi
		hiz = theCar.GetComponent<Rigidbody> ().velocity.magnitude;
		speedText.SetText (((int)(hiz*unitToKmH)).ToString () );

		//gidilen toplam yol gösterge
		float gidilenYolValue2 = hiz * unitToKmH / 200 * Time.deltaTime;
		distance += gidilenYolValue2;
		distanceText.SetText ((distance).ToString ("F1") );
		//skor panelindeki gidilen toplam yol
		katedilenMesafeValueText.SetText ((distance).ToString ("F1"));
		if (skor > 0) {
			gidilenYolParasi = (Mathf.RoundToInt (distance * katedilenMesafeParaKatSayisi));
			katedilenMesafeMoneyText.text = (gidilenYolParasi).ToString ();
		} else {
			katedilenMesafeMoneyText.text = "0";
		}

		yakinMakasValueText.SetText (((int)(yakinMakas)).ToString());
		if (skor > 0) {
			yakinMakasParasi = Mathf.RoundToInt (yakinMakas * yakinMakasMesafeParaKatSayisi);
			yakinMakasMoneyText.text = yakinMakasParasi.ToString ();
		} else {
			yakinMakasMoneyText.text = "0";
		}


		//arabamız hızlandığı zamanlar puan artışı olacak.
		hizlaniyor = theCar.GetComponent<CarControllerScript> ().hizlaniyor;
		/*eğer arabamız hizlaniyor ve belirlenen yüksek hiz değerinden daha düşük bir hızdayken ve hizimiz en küçük ulaşılacak hızdan daha çok
		olduğu zamanlar skorumuz time.delta time ın 10 katı şeklinde artıyor.
		hizimiz en küçük ulaşılacak hızdan daha çok olduğu zamanlar koşulunu ortaya atmamamızın sebebi arabamizin hiz belirlenen
		en küçük hizdan düşük olduğu zamanlar zaten kendi kendine hizlaniyor zaten.*/
		if (hizlaniyor && hiz * unitToKmH < skorHiz && hiz > theCar.GetComponent<CarControllerScript>().enKucukHiz) {
			skor += Time.deltaTime * 10f;
		}
		/*eğer arabamiz hizlaniyor ve belirlenen yüksek hiz değerinden daha büyük veya ona eşitse ve hizimiz en küçük ulaşılacak hızdan daha çok
		olduğu zamanlar skorumuz time.delta time ın 30 katı şeklinde artıyor.*/
		else if (hizlaniyor && hiz * unitToKmH >= skorHiz  && hiz > theCar.GetComponent<CarControllerScript>().enKucukHiz) {
			skor += Time.deltaTime * 30f;
		}

		//hizimizi km/h cinsine çevirmek için 10 / 3 ile çarpıyoruz.
		//eğer hizimiz km/h cinsinden belirlenen yüksek hizdan düşükse skor yazdığımız yazıyı seksen5altı
		//rengi olarak belirlediğimiz renk setini ayarlıyoruz.
		if (hiz * unitToKmH < skorHiz) {
			skorText.colorGradientPreset = seksen5alti;
		}//eğer hizimiz km/h cinsinden belirlenen yüksek hizdan büyük ve eşitse ve hızlanıyorsam skor yazdığımız yazıyı seksen5ustu
		//rengi olarak belirlediğimiz renk setini ayarlıyoruz.
		else if (hiz * unitToKmH >= skorHiz && hizlaniyor) {
			skorText.colorGradientPreset = seksen5ustu;
		} 
		skorText.text = skorPanelText.text = (Mathf.RoundToInt(skor)).ToString();
		if (PlayerPrefs.GetFloat ("skorBest", 0) < Mathf.RoundToInt (skor)) {
			PlayerPrefs.SetFloat ("skorBest", Mathf.RoundToInt (skor));
		}

		if (hiz * unitToKmH >= skorHiz) {
			yuksekHizGosterge.SetActive (true);
			yuksekHizTime += Time.smoothDeltaTime;
		} else {
			yuksekHizGosterge.SetActive (false);
		}

		yuksekHizValueText.text = seksen5kmhustuValueText.text = (Mathf.RoundToInt(yuksekHizTime)).ToString ();
		if (skor > 0) {
			yuksekHizParasi = Mathf.RoundToInt (yuksekHizTime * seksen5kmhUstuParaKatSayisi);
			seksen5kmhUstuMoneyText.text = (yuksekHizParasi).ToString ();
		} else {
			seksen5kmhUstuMoneyText.text = "0";
		}

		sollamaValueText.colorGradientPreset = seksen5ustu;

		if (PlayerPrefs.GetFloat ("yuksekHizTimeBest", 0) < Mathf.RoundToInt (yuksekHizTime)) {
			PlayerPrefs.SetFloat ("yuksekHizTimeBest", Mathf.RoundToInt (yuksekHizTime));
		}


	}

	void EngineStart(){
		EngineStartbutton.GetComponent<Button> ().onClick.RemoveListener (EngineStart);
		audioS.Play ();
		Invoke ("butonGizle", audioS.clip.length);
	}

	public void butonGizle(){
		EngineStartbutton.SetActive (false);
		Gazbutton.SetActive (true);
		Frenbutton.SetActive (true);
		theCar.GetComponent<CarControllerScript> ().enabled = true;
	}

	public void Gaz(bool basili){
		theCar.GetComponent<CarControllerScript> ().gazBasili = basili;
	
	}

	public void Brake(bool basili){
		theCar.GetComponent<CarControllerScript> ().frenBasili = basili;
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
