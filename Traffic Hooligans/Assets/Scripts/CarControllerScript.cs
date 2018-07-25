using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerScript : MonoBehaviour {

	public WheelCollider onSolW, onSagW, arkaSolW, arkaSagW;
	public Transform onSolT, onSagT, arkaSolT, arkaSagT;

	private Rigidbody rb;
	public float hiz;

	private float horizontalInput;
	private float verticalInput;

	public float acceleration = 20;
	public float maxMotorForce = 300f;
	public float brakeForce = 300f;

	public float enKucukHiz = 10f;
	public float enBuyukHiz = 25f;

	private bool frenYapabilme = false;
	private bool gazBasili = false;

	public float brakeTorque, motorTorque;

	public float VerticalInputDecreaseKatSayi = 2;

	public float[] GearRatio;
	public int CurrentGear = 0;
	public float MaxRPM = 3000f;
	public float MinRPM = 1000f;
	public float nowRPM;
	public float EngineRPM = 0f;

	public float KontrolMesafesi = 3.2f;

	private AudioSource audioS;


	private void GearOperations(){
		EngineRPM = -(onSagW.rpm + onSolW.rpm) / 2 * GearRatio [CurrentGear];

		nowRPM = -onSagW.rpm;
		//SHIFT GEAR
		if (EngineRPM >= MaxRPM) {
			int GeciciVites = CurrentGear;
			for (int i = 0; i < GearRatio.Length; i++) {
				if (nowRPM * GearRatio [i] < MaxRPM) {
					GeciciVites = i;
					break;
				}
			}

			CurrentGear = GeciciVites;
		}

		if (EngineRPM <= MinRPM) {
			int GeciciVites = CurrentGear;
			for (int a = GearRatio.Length - 1; a >= 0; a--) {
				if (nowRPM * GearRatio [a] > MinRPM) {
					GeciciVites = a;
					break;
				}
			}

			CurrentGear = GeciciVites;
		}

	}

	private void CarSound(){
		audioS.pitch = float.Parse(((float)(Mathf.Abs (EngineRPM / MaxRPM) + 0.6)).ToString("F2"));
		if (audioS.pitch > 2f) {
			audioS.pitch = 2f;
		}
	}

	private void GetInput(){
		//Accelerometerden X yönündeki alınan değer.
		horizontalInput = Input.acceleration.x;
		//Vertical input değerini en az -1 en fazla 1 arasında sıkıştır.
		//verticalInput = Mathf.Clamp (verticalInput, -1, 1);
	}

	private void Steering(){
		//donme açısı = yatay input değeri ile en büyük donme açısının çarpımı kadardır.
		float steeringAngle = horizontalInput;

		Vector3 pos = transform.position;
		pos.x += steeringAngle; 
		pos.x = Mathf.Clamp (pos.x, -7f, 7f);
		transform.position = pos;

	
	}

	private void CarRotationFix(){
		Quaternion quat = transform.rotation;
		quat.x = 0;
		quat.z = 0;
		quat.y = 180;
		transform.rotation = quat;
	}

	private void UpdateWheelPose(WheelCollider collider, Transform transform){
		Vector3 pos;
		Quaternion quat;

		collider.GetWorldPose (out pos, out quat);
		transform.position = pos;
		transform.rotation = quat;
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

	public void Brake(bool basili){
		if (basili && frenYapabilme) {
			onSagW.brakeTorque = brakeForce;
			onSolW.brakeTorque = brakeForce;
			arkaSagW.brakeTorque = brakeForce;
			arkaSolW.brakeTorque = brakeForce;
			verticalInput = 0;
		} else {
			onSagW.brakeTorque = 0;
			onSolW.brakeTorque = 0;
			arkaSagW.brakeTorque = 0;
			arkaSolW.brakeTorque = 0;
		}
	}

	public void Gaz(bool basili){
		if (basili) {
			gazBasili = true;
		} else {
			gazBasili = false;
		}
	}

	private void Accelerate(){
		float motorForce = maxMotorForce / GearRatio[CurrentGear] * -verticalInput;
		onSagW.motorTorque = motorForce;
		onSolW.motorTorque = motorForce;
	}

	private void otonomHizlanma(){
		if (hiz < enKucukHiz) {
			VerticalInputIncrease ();
			onSagW.brakeTorque = 0;
			onSolW.brakeTorque = 0;
			arkaSagW.brakeTorque = 0;
			arkaSolW.brakeTorque = 0;
		}

		if (hiz < enKucukHiz && CurrentGear > 1) {
			CurrentGear = 0;
			onSagW.motorTorque = 0;
			onSolW.motorTorque = 0;
		}

		if (hiz < enKucukHiz && verticalInput < 0) {
			verticalInput = 0;
		}

		if (verticalInput >-0.1 && hiz > enKucukHiz + 0.4) {
			VerticalInputDecrease ();
		}
	}

	private void HizLimitleme(){
		if (hiz > enBuyukHiz) {
			verticalInput = 0;
			onSagW.motorTorque = 0;
			onSolW.motorTorque = 0;
		}
	}

	private void BoolManager(){
		if (hiz < enKucukHiz) {
			frenYapabilme = false;
		} else {
			frenYapabilme = true;
		}
	}

	private void BaskaBirArabayaMıCarptın(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.back), out hit, Mathf.Infinity)) {
			if (hit.distance <= KontrolMesafesi) {
				Debug.Log ("Çarptın!");
			}
		}
	}

	void Start(){
		rb = GetComponent<Rigidbody> ();
		verticalInput = 0;
		audioS = GetComponent<AudioSource> ();
	}

	void Update(){
		GetInput ();
		Accelerate ();
		BoolManager ();
		HizLimitleme ();
		if (gazBasili) {
			VerticalInputIncrease ();
		}
		BaskaBirArabayaMıCarptın ();
		CarSound ();
		hiz = rb.velocity.magnitude;

	}

	void FixedUpdate(){
		Steering ();
		CarRotationFix ();
		UpdateWheelPoses ();
		otonomHizlanma();
		GearOperations ();

		brakeTorque = onSagW.brakeTorque;
		motorTorque = onSagW.motorTorque;
	}

}
