using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControllerScript : MonoBehaviour {

	public WheelCollider onSolW, onSagW, arkaSolW, arkaSagW;
	public Transform onSolT, onSagT, arkaSolT, arkaSagT;
	public GameObject arkaFarlar;
	public bool carptim = false;

	private GameObject theGod;

	private Rigidbody rb;
	private AudioSource audioS;
	public AnimationClip ShiftingGear;
	//diğer araçlar tarafından kontrol edildiği için public olması gerekir.
	public float hiz, manevraBecerisi;

	public float horizontalInput;
	private float verticalInput;

	public float acceleration = 20;
	public float maxMotorForce = 300f;
	public float brakeForce = 300f;

	public float enKucukHiz = 10f;
	public float enBuyukHiz = 25f;

	private bool frenYapabilme = false;
	public bool gazBasili = false;
	public bool frenBasili = false;
	public bool ShiftingGearNow = false;
	public bool hizlaniyor =false;

	public float brakeTorque, motorTorque;

	public float VerticalInputDecreaseKatSayi = 2;

	public float[] GearRatio;
	private int CurrentGear = 0;
	public float MaxRPM = 3000f;
	public float MinRPM = 1000f;
	private float nowRPM;
	public float EngineRPM = 0f;

	public float KontrolMesafesi = 3.2f;
	public float SollamaMesafesi = 2.07f;

	private void GearOperations(){
		//motor rpm i bir tekerlegin rpm i ile geçerli vitesin vites oranını çarparak buluruz.
		nowRPM = -onSagW.rpm;
		EngineRPM = nowRPM * GearRatio [CurrentGear];


		//SHIFT GEAR
		//Eğer motor rpm max rpm den büyükse vites arttırılması gerekir.
		if (EngineRPM >= MaxRPM) {
			
			int GeciciVites = CurrentGear;
			//for döngüsü ile tüm vites oranlarını deniyoruz.
			for (int i = 0; i < GearRatio.Length; i++) {
				//vites oranlarını en küçükten başlatarak deniyoruz ve max rpm den düşük bir motor rpm inin bulunması için if kullanıyoruz.
				if (nowRPM * GearRatio [i] < MaxRPM) {
					//eğer motor rpm imiz geçerli vites oranında max rpm den düşükse o viteste kalabiliriz.
					GeciciVites = i;
					break;
				}
			}
			//shifting gear now animasyon koşuludur. vites değiştirdiğimiz için küçük bir animasyon oynatılır.
			ShiftingGearNow = true;
			//ve bulduğumuz vites geçerli vitesimiz olur.
			CurrentGear = GeciciVites;
		} 

		if (EngineRPM <= MinRPM) {
			int GeciciVites = CurrentGear;
			for (int a = GearRatio.Length - 1; a > -1; a--) {
				if (nowRPM * GearRatio [a] > MinRPM) {
					GeciciVites = a;
					break;
				}
			}
			ShiftingGearNow = true;
			CurrentGear = GeciciVites;
		} 

	}

	private void Sollama(){
		RaycastHit hit, hit2;
		int a = 0;
		theGodScript theGodS = theGod.GetComponent<theGodScript>();
		float unitToKmH = theGodS.unitToKmH;
		float skorHiz = theGodS.skorHiz;
		bool overtakeLeft = Physics.Raycast (transform.position, transform.TransformDirection (Vector3.left), out hit, SollamaMesafesi);
		bool overtakeRight = Physics.Raycast (transform.position, transform.TransformDirection (Vector3.right), out hit2, SollamaMesafesi);
		if (overtakeRight && hit2.transform.gameObject.CompareTag ("BotCar") && hiz * unitToKmH >= skorHiz) {
			Debug.Log (hit2.distance + " right");
			a = 1;
			theGodS.sollamaGosterge.SetActive (true);
		} else {
			a = 0;
			theGodS.sollamaGosterge.SetActive (false);
		}
		if (overtakeLeft && hit.transform.gameObject.CompareTag("BotCar") && hiz * unitToKmH >= skorHiz) {
			Debug.Log (hit.distance + " left");
			a = 1;
			theGodS.sollamaGosterge.SetActive (true);
		} else {
			a = 0;
			theGodS.sollamaGosterge.SetActive (false);
		}

		theGodS.yakinMakas += a;
		theGodS.sollamaValueText.text = theGodS.yakinMakas.ToString ();
	}

	private void CarSound(){
		audioS.pitch = float.Parse(((float)(Mathf.Abs (EngineRPM / MaxRPM) + 0.6)).ToString("F2"));
		if (audioS.pitch > 2f) {
			audioS.pitch = 2f;
		}
	}

	private void AnimationController(){
		// belirlenen en küçük hıza ulaşıncaya kadar vites değiştirilse de hissettirilmesini istemiyoruz o yüzden hizimiz sadece belirlenen 
		// en küçük hızdan daha büyük olduğu zamanlar animator daki animasyon koşulunu burada viteslerle değiştirdiğimiz koşul değişkenine eşitliyoruz.
		if (hiz > enKucukHiz) {
			GetComponent<Animator> ().SetBool ("ShiftingGearNow", ShiftingGearNow);
		}
	}

	private void GetInput(){
		//Accelerometerden X yönündeki alınan değer.
		horizontalInput = Input.acceleration.x;
		//Vertical input değerini en az -1 en fazla 1 arasında sıkıştır.
		//verticalInput = Mathf.Clamp (verticalInput, -1, 1);
	}

	private void Steering(){
		//aslında donme açısını direkt olarak yatay girişe eşitlediğimizden dolayı bu değişkene gerek yok fakat kullanırken hassasiyet beğenilmezse
		//diye böyle bir değişken kullanmayı uygun gördüm.
		float steeringAngle = horizontalInput * manevraBecerisi;

		//geçici bir vector3 oluşturuyoruz ve bunu arabamızın şu an ki pozisyon değerlerine eşitliyoruz.
		Vector3 pos = transform.position;
		//daha sonra x değerini yani yolda sağ sol yapmamıza yarayan değeri yukarıdaki donme açısına bağımlı hale getiriyoruz.
		pos.x += steeringAngle; 
		//fakat arabamızın bariyerleri aşıp gitmemesi için x değerini -7 ile 7 arasına sıkıştırıyoruz ki bunlar yolda ulaşılabilecek en güvenli
		//değerler oluyor.
		pos.x = Mathf.Clamp (pos.x, -7f, 7f);
		//ve sonra arabımızın konumunu geçici vector 3 ümüze eşitliyoruz.
		transform.position = pos;

	
	}

	private void CarRotationFix(){
		//arabamızın rotasyon değerlerini tek değiştiren vites değiştirme animasyonumuz olduğundan animasyon koşulu 1 değerindeyken
		//rotasyon değerlerini rahat bırakıyoruz.
		if (!ShiftingGearNow) {
			//geçici bir quaternion oluşturarak bunu rotasyon değerlerimize eşitliyoruz.
			Quaternion quat = transform.rotation;
			//z ve y değerlerini 0 ve 180 e eşitliyoruz. x değerine karışmıyoruz çünkü onu Steering metodumuzda -7 ve 7 arasına
			//sıkıştırıp, accelerometerin köpeği haline getirdik.
			quat.z = 0;
			quat.y = 180;
			//ve rotasyon değerlerimizi geçici quaterniona eşitledik.
			transform.rotation = quat;
		}
	}

	//wheelcolliderlerımız motoru ve freni kontrol eder.
	private void UpdateWheelPose(WheelCollider collider, Transform transform1){
		//geçici bir vector3 ve bir tane de quaternion oluşturduk
		Vector3 pos;
		Quaternion quat;

		//wheelcollider ımızın pozisyonunu ve rotasyon değerlerini geçici vector3 ve quat a atadık.
		collider.GetWorldPose (out pos, out quat);
		//daha sonra tekerlerimizin pozisyon ve rotasyon değerlerini wheel colliderlerınkine eşitledik.
		transform1.position = pos;
		transform1.rotation = quat;
	}

	private void UpdateWheelPoses(){
		UpdateWheelPose (onSolW, onSolT);
		UpdateWheelPose (arkaSolW, arkaSolT);
		UpdateWheelPose (onSagW, onSagT);
		UpdateWheelPose (arkaSagW, arkaSagT);
	}

	public void VerticalInputIncrease(){
		
		verticalInput += Time.deltaTime * acceleration;
		if (verticalInput > 1) {
			verticalInput = 1;
		}


	}

	private void VerticalInputDecrease(){
		verticalInput -= Time.deltaTime / VerticalInputDecreaseKatSayi ;
	}
	/*
	public void Brake(){
		if (frenYapabilme) {
			VerticalInputDecrease();
		}
	}*/

	public void Brake(){
		if (frenYapabilme) {
			if (frenBasili) {
				arkaFarlar.SetActive (true);
				onSagW.brakeTorque = brakeForce;
				onSolW.brakeTorque = brakeForce;
				arkaSagW.brakeTorque = brakeForce;
				arkaSolW.brakeTorque = brakeForce;
				verticalInput = 0;
			} else {
				arkaFarlar.SetActive (false);
				onSagW.brakeTorque = 0;
				onSolW.brakeTorque = 0;
				arkaSagW.brakeTorque = 0;
				arkaSolW.brakeTorque = 0;
			}
		} else {
			arkaFarlar.SetActive (false);
			onSagW.brakeTorque = 0;
			onSolW.brakeTorque = 0;
			arkaSagW.brakeTorque = 0;
			arkaSolW.brakeTorque = 0;
		}
	}


	private void Accelerate(){
		float motorForce = maxMotorForce / GearRatio[CurrentGear] * -verticalInput;
		onSagW.motorTorque = motorForce;
		onSolW.motorTorque = motorForce;
	}

	private void verticalInputManager(){
		if (hiz > enBuyukHiz) {
			verticalInput = 0;
			onSagW.motorTorque = 0;
			onSolW.motorTorque = 0;
		}

		if (hiz < enKucukHiz && CurrentGear > 1) {
			onSagW.motorTorque = 0;
			onSolW.motorTorque = 0;
			CurrentGear = 0;
		}

		if (hiz < enKucukHiz && verticalInput < 0) {
			verticalInput = 0;
		}

		if (verticalInput > -0.3f && hiz > enKucukHiz + 0.3) {
			VerticalInputDecrease ();
		}

		if (!gazBasili) {
			if (hiz < enKucukHiz) {
				VerticalInputIncrease ();
			}
			hizlaniyor = false;
		}

		if (gazBasili) {
			if (hiz <= enBuyukHiz) {
				VerticalInputIncrease ();
				if (hiz > enKucukHiz) {
					hizlaniyor = true;
				}
			}

		
		}
	}

	private void BoolManager(){
		if (hiz <= enKucukHiz) {
			frenYapabilme = false;
		} else {
			frenYapabilme = true;
		}

		if (!gazBasili && hiz < enKucukHiz) {
			hizlaniyor = false;
		}

		if (ShiftingGearNow) {
			Invoke ("ShiftingGearNowOff", ShiftingGear.length);
		}
	}
	
	private void ShiftingGearNowOff(){
		ShiftingGearNow = false;
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.CompareTag ("BotCar")) {
			carptim = true;
		}
	}

	void Start(){
		rb = GetComponent<Rigidbody> ();
		verticalInput = 0;
		audioS = GetComponent<AudioSource> ();
		carptim = false;
		theGod = GameObject.FindGameObjectWithTag ("theGod");
	}

	void Update(){
		//get input metodu x ekseninde hareket için gerekli olan yatay input girişini accelerometer yardımıyla belirler.
		GetInput ();
		//accelerate metodu on sol ve sag tekerleklere uygulanan motor torkunu ayarlar.
		Accelerate ();
		//bool manager metodu ile fren yapabilme boolunu hizin belirlenen en küçük hızdan büyük veya küçük olması koşulu ile bire veya sıfıra ayarlar.
		BoolManager ();
		// 1 - hiz belirlenen en büyük hızdan büyük olduğu zamanlar vertical inputu, on sol ve on sag tekerleklerinin motor torkunu sıfıra eşitler.
		// 2 - hiz belirlenen en küçük hızdan düşük olduğu zamanlar geçerli vites 1 den büyükse on sol ve sag tekerleklere uygulanan motor torkunu sıfırlar ve geçerli vitesi 0 indirir.
		// 3 - hiz belirlenen en küçük hızdan düşük olduğu zamanlar vertical input sıfırdan küçükse vertical inputu 0 a eşitler.
		// 4 - hiz belirlenen en küçük hızın 0.3 fazlasından büyük olduğu zamanlar ve vertical inputun -0.3 den büyük olduğu zamanlar vertical inputu yavaşça küçültür.
		// 5 - eğer gaz butonuna basılı değil ve hiz belirlenen en küçük hızdan düşükse vertical inputu yavaşça büyültür.
		verticalInputManager ();
		Brake ();
		CarSound ();
		AnimationController ();
		hiz = rb.velocity.magnitude;
		Sollama ();
	}

	void FixedUpdate(){
		Steering ();
		CarRotationFix ();
		UpdateWheelPoses ();
		GearOperations ();

		brakeTorque = onSagW.brakeTorque;
		motorTorque = onSagW.motorTorque;
	}

}
