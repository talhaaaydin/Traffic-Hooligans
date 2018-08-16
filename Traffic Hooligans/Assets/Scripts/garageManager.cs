using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class garageManager : MonoBehaviour {

	public GameObject[] playerCarPrefabs;
	GameObject theCar;
	public GameObject lockPanel, paraYokPanel, loadingPanel;
	public Slider loadingSlider;
	AsyncOperation async;
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

	public float hizValueBolunecekDeger = 6000, frenValueBolunecekDeger = 300, manevraValueCarpilacakDeger = 40;

	// Use this for initialization
	void Start () {
		//baslangicArabasiIsim değişkenine bunu ekliyoruz çünkü bir gameobjecti yarattığınızda adının sonuna (Clone) eki geliyor.
		baslangicArabasiIsim += "(Clone)";
		//artık kilim olmasada yeniden değiştirmeye lüzum yok. etiketi kilim olduğu için etiketten bulunuyor.
		kilimler = GameObject.FindGameObjectWithTag ("kilim");
		//hiz, fren ve manevra yetilerini bir üst seviyeye çıkartmak için gereken butonları yine etiket aracılığı ile buluyoruz.
		hizUpgradeButton = GameObject.FindGameObjectWithTag ("hizUpgradeButton").GetComponent<Button>();
		brakeUpgradeButton = GameObject.FindGameObjectWithTag ("brakeUpgradeButton").GetComponent<Button>();
		manevraUpgradeButton = GameObject.FindGameObjectWithTag ("manevraUpgradeButton").GetComponent<Button>();

		CreateCar ();	
		TextSettings ();
		ButonaAta ();
		//bedavaya verdiğimiz araba olan başlangıç arabasını oyunu ilk defa açanlar için satın alındı ibaresi ekleniyor. ilk defa açmayanlar için gerek yok aslında.
		if (GameObject.FindGameObjectWithTag ("Player").name == baslangicArabasiIsim) {
			
			PlayerPrefs.SetString (baslangicArabasiIsim, "satinAlindi");
			UnLock ();
		}
	}

	//eğer izin varsa kilimler objemizi belirlenen hızda döndürüyoruz.
	void kilimleriDondur(bool izin){
		if (izin) {
			kilimler.transform.Rotate (0, rotateSpeed, 0);
		}
	}

	void ButonaAta(){
		//burada butonlara işlevlerini atıyoruz.
		nextButton.onClick.AddListener (NextButton);
		beforeButton.onClick.AddListener (BeforeButton);
		playButton.onClick.AddListener (PlayButton);
		buyButton.onClick.AddListener (BuyButton);
	}

	void PlayButton(){
		//ana sahnede theGod tarafından doğru araba yaratılsın diye playerprefs e hangi arabanın olduğunu kaydediyoruz.
		PlayerPrefs.SetInt ("theCarIndex", prefabIndex);
		//ve yükleme ekranına geçiş yapılacak.
		StartCoroutine (LoadingScreen ());
	}

	IEnumerator LoadingScreen(){
		//yükleme panelini görünür yapıyoruz.
		loadingPanel.SetActive (true);
		//yukarıda belirlediğimiz async operation değişkenini tanımlıyoruz. arka planda ana sahneyi yükleyecek.
		async = SceneManager.LoadSceneAsync (anaSahneIsmi);
		//şu anda arka planda yüklenen sahneye erişim yok
		async.allowSceneActivation = false;

		//arka planda yükleme işlemi tamamlanmadığı sürece bu döngü geçerli olacak
		while (!async.isDone) {
			//sliderımızın değerini arka planda yükleme işleminin durumuna eşitliyoruz
			loadingSlider.value = async.progress;

			//arka planda yükleme işlemi tamamlandığında progress değeri 0.9 olur bu yüzden 0.9a eşit mi diye bakıyoruz
			if (async.progress == 0.9f) {
				//eğer eşitse sliderımızn değerini 1 e eşitliyoruz
				loadingSlider.value = 1;
				//ve arka planda yüklenen sahneye erişimi açıyoruz.
				async.allowSceneActivation = true;
			}
			//null değerini döndürüyoruz yani boş değerini.
			yield return null;
		}

	}


	void BuyButton(){
		//ekranda gözüken araba fiyatını float tipine parse edip bunu price değişkenine atadık.
		float price = float.Parse (carPriceText.text);
		//eğer hafızada kayıtlı olan para - kayıtlı değilse 0 kabul edilecek - price dan büyük veya eşitse bu arabayı satın alabiliriz.
		if (PlayerPrefs.GetFloat ("para", 0) >= price) {
			//satın aldığımızı belirtmek için araba ismine hafızada satinAlindi ibaresini ekliyoruz.
			PlayerPrefs.SetString (theCar.name, "satinAlindi");
			//metodumuzu çağırıyoruz.
			LockOrUnlockedCar (theCar.name);
		} else {
			//eğer yeterli paramız yoksa para yok panelimizi açıyoruz
			paraYokPanel.SetActive (true);
			//ve ne kadar paranın eksik olduğunu gösteren yazımızın içeriğini değiştiriyoruz.
			kacParaLazim.text = (price - PlayerPrefs.GetFloat ("para", 0)).ToString () + " PARA LAZIM!";
		}
	}


	void ValueSets(){
		//arabaların yeteneklerini gösteren yazıları göstermek ve içeriğini kontrol etmek için birden çok metodta kullanılan aşağıdaki üç kod satırını bir metod 
		//haline getirerek tek bir yerden kontrol edilebilir hale getirdim.
		//hiz yeteneği arabanın ulaşabileceği en büyük hız ile sahip olduğu maksimum motor kuvvetinin çarpımlarının 6000 e bölünmesiyle belirlenir. 6000 değişebilir.
		hizValue.SetText (((int)(theCar.GetComponent<CarControllerScript> ().enBuyukHiz * theCar.GetComponent<CarControllerScript>().maxMotorForce / hizValueBolunecekDeger)).ToString());
		//fren yeteneği arabanın fren kuvvetinin 300 e bölünmesiyle bulunur. 300 değişebilir.
		frenValue.SetText (((int)(theCar.GetComponent<CarControllerScript>().brakeForce / frenValueBolunecekDeger)).ToString());
		//manevra yeteneği arabanın sahip olduğu manevra becerisi değişkeninin 40 ile çarpılıp en yakın tam sayıya yuvarlanması ile bulunur.
		manevraValue.SetText (Mathf.RoundToInt(theCar.GetComponent<CarControllerScript> ().manevraBecerisi * manevraValueCarpilacakDeger).ToString());
	}

	void Update(){
		kilimleriDondur (kilimleriDondurelimMi);
		TextSettings ();
		ValueSets ();
	}

	private void CreateCar(){

		//ileri ve geri butonları ile yönetilen prefabIndex değişkeni gösterilecek arabanın prefab listesinde hangi sırada olduğunu tutar. theCar adlı tanımlanmış gameobject
		//burada yaratılıyor.
		theCar = Instantiate (playerCarPrefabs [prefabIndex]) as GameObject;
		//araba çalışmadığı için animasyonların gösterilmesini durduruyoruz.
		theCar.GetComponent<Animator> ().StopPlayback ();
		//ve animator componentini de devre dışı bırakıyoruz.
		theCar.GetComponent<Animator> ().enabled = false;
		//pozisyonu belirlenen başlangıç pozisyonuna ayarlıyoruz. rotasyonu da belirlenen başlangıç rotasyonuna ayarlıyoruz.
		theCar.transform.position = startPosition;
		theCar.transform.rotation = quat;
		//ve arabamızın transform ailesini kilimler transformu haline getiriyoruz.
		theCar.transform.SetParent (kilimler.transform);
		//kilimleri döndürme değerini true yaparak kilimleri döndürüyoruz. yukarıda kilimleri izin varsa döndürelim dediğimiz yerde izin kilimleriDondureliMi değişkeni.
		kilimleriDondurelimMi = true;
		string theName = theCar.name;

		ValueSets ();


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
