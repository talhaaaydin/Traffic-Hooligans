using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botCarController : MonoBehaviour {

	public WheelCollider onSolW, onSagW, arkaSolW, arkaSagW;
	public Transform onSolT, onSagT, arkaSolT, arkaSagT;
	Rigidbody rb;

	private float EngineRPM, nowRPM;
	private int CurrentGear = 0;
	public float[] GearRatio;
	public float MaxRPM = 1200, MinRPM = 500;

	public float hiz;
	public float enBuyukHiz = 16, enKucukHiz = 9; 
	public float onumdekininHizi;

	public float maxMotorForce = 3000;
	public float brakeForce = 3000f;
	public float acceleration = 20f, VerticalInputDecreaseKatSayi = 1;
	public float guvenliUzaklık = 20f;
	public float mevcutUzaklık;
	public float riskliUzaklık = 10f;
	public float gozlemlemeUzaklıgı = 60f;
	private float verticalInput;

	private bool hizlanalımMı = true;
	private bool yavaslayalımMı = false;

	public float motorTorque, brakeTorque;

	void Start(){
		rb = GetComponent<Rigidbody> ();
	}

	void Update(){
		GearOperations ();
		botCarIntelligence ();
		Accelerate ();
		Brake (yavaslayalımMı);
	}

	void FixedUpdate(){
		TransformFix ();
		UpdateWheelPoses ();
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

	private void Accelerate(){
		if (hizlanalımMı) {
			VerticalInputIncrease ();
		} else {
			verticalInput = 0;
		}

		if (hiz > enKucukHiz && verticalInput > -0.2) {
			VerticalInputDecrease ();
		}
		float motorForce = maxMotorForce / GearRatio [CurrentGear] * -verticalInput;
		motorTorque = onSagW.motorTorque = motorForce;
		onSolW.motorTorque = motorForce;
	}

	private void Brake(bool izin){
		if (izin) {
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
		brakeTorque = onSagW.brakeTorque;
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

	private bool onumdeBirisiYok(){
		hiz = rb.velocity.magnitude;
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, gozlemlemeUzaklıgı)){
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.black);
			if (hit.collider.gameObject.CompareTag ("BotCar") || hit.collider.gameObject.CompareTag("Player")) {
				onumdekininHizi = hit.rigidbody.velocity.magnitude;
			}
			mevcutUzaklık = hit.distance;
			return false;
		} else {
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * 1000, Color.blue);
			return true;
		}
	}

	private void TransformFix(){
		Quaternion quat = transform.rotation;
		quat.x = quat.z = 0; quat.y = 180;
		transform.rotation = quat;

		if (transform.position.y > 3 || transform.position.y < -3) {
			Destroy (this.gameObject);
		}
	}

	private void botCarIntelligence(){
		if (onumdeBirisiYok ()) {
			if (hiz < enBuyukHiz) {
				hizlanalımMı = true;
				yavaslayalımMı = false;
			} else if (hiz == enBuyukHiz) {
				hizlanalımMı = false;
				yavaslayalımMı = false;
			} else {
				hizlanalımMı = false;
				yavaslayalımMı = true;
			}
		} else {
			if (mevcutUzaklık > guvenliUzaklık) {
				if (hiz < enBuyukHiz) {
					hizlanalımMı = true;
				} else {
					hizlanalımMı = false;
				}
				yavaslayalımMı = false;
			} else if(mevcutUzaklık == guvenliUzaklık){
				if (hiz < onumdekininHizi) {
					hizlanalımMı = true;
					yavaslayalımMı = false;
				} else if (hiz >= onumdekininHizi) { 
					hizlanalımMı = false;
					yavaslayalımMı = true;
				} 
			} else if (mevcutUzaklık < guvenliUzaklık && mevcutUzaklık > riskliUzaklık) {
				if (hiz >= onumdekininHizi) {
					hizlanalımMı = false;
					yavaslayalımMı = true;
				} else {
					hizlanalımMı = false;
					yavaslayalımMı = false;
				}
			} else if(mevcutUzaklık <= riskliUzaklık){
				hizlanalımMı = false;
				yavaslayalımMı = true;
			}
		}
	}
		
}
