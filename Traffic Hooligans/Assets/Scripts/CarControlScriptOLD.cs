using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarControlScript : MonoBehaviour {

	private float horizontalAxis;

	public WheelCollider onSolW, onSagW, arkaSolW, arkaSagW;
	public Transform onSolT, onSagT, arkaSolT, arkaSagT;

	//public float motorForce;
	public float maxSteeringAngle = 5f;

	public float hiz;
	public float motorForce;
	public float brakeForce;

	public float enKucukHiz = 10;
	public float enBuyukHiz = 25;

	private Rigidbody rb;

	public bool frenYapabilme = true;
	public bool otonomHizlanabilme = false;
	public bool hizlanabilme = true;

	public bool GazButton = false;
	public bool FrenButton = false;



	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	private void GetInputs(){
		horizontalAxis = Input.acceleration.x;
	}

	private void SteeringWheels(){
		//ön tekerlekleri saga sola verilen açı kadar döndürecek.
		float nowSteeringAngle = maxSteeringAngle * horizontalAxis;
		onSagW.steerAngle = nowSteeringAngle;
		onSolW.steerAngle = nowSteeringAngle;
	}

	public void Gaz(){
		//eğer hızlanabilme true ise belirlenen motor kuvveti motorun torku olacak.
		if (hizlanabilme) {
			onSolW.motorTorque = motorForce * -1;
			onSagW.motorTorque = motorForce * -1;
			onSagW.brakeTorque = 0;
			onSolW.brakeTorque = 0;
			GazButton = true;
			FrenButton = false;
		} 
	}

	public void Brake(){
		//eğer fren yapabilme boolu true ise belirlenen fren kuvveti ile yavaşlanılacak.
		if (frenYapabilme) {
			onSagW.motorTorque = 0;
			onSolW.motorTorque = 0;
			onSagW.brakeTorque = brakeForce;
			onSolW.brakeTorque = brakeForce;
			FrenButton = true;
			GazButton = false;
		}
	}

	public void BrakeUp(){
		FrenButton = false;
		onSagW.brakeTorque = 0;
		onSolW.brakeTorque = 0;
	}

	public void GazUp(){
		GazButton = false;
		onSagW.motorTorque = 0;
		onSolW.motorTorque = 0;
	}

	public void kontrolBilgisayarda(){
		if (!frenYapabilme) {
			onSagW.brakeTorque = 0;
			onSolW.brakeTorque = 0;
			FrenButton = false;
		}

		if (!hizlanabilme) {
			onSagW.motorTorque = 0;
			onSolW.motorTorque = 0;
			FrenButton = false;
		}
	}


	private void otonomHizlanma(){
		if (otonomHizlanabilme) {
			onSolW.motorTorque = motorForce * -1;
			onSagW.motorTorque = motorForce * -1;
		}
	}

	private void hizLimitleme(){
		if (!hizlanabilme) {
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, enBuyukHiz);
		}
	}

	private void BoolManager(){
		if (hiz <= enKucukHiz + 1) {
			frenYapabilme = false;
		} else {
			frenYapabilme = true;
		}

		if (hiz < enKucukHiz) {
			otonomHizlanabilme = true;
		} else {
			otonomHizlanabilme = false;
		}

		if (hiz >= enBuyukHiz) {
			hizlanabilme = false;
		} else {
			hizlanabilme = true;
		}

	}

	private void UpdateWheelPose(WheelCollider collider, Transform transform){
		Vector3 pos;
		Quaternion quat;

		collider.GetWorldPose (out pos, out quat);

		transform.position = pos;
		transform.rotation = quat;
	}

	private void UpdateWheelPoses(){
		UpdateWheelPose (onSagW, onSagT);
		UpdateWheelPose (onSolW, onSolT);
		UpdateWheelPose (arkaSagW, arkaSagT);
		UpdateWheelPose (arkaSolW, arkaSolT);

	}



	void Update(){
		hiz = rb.velocity.magnitude;
		GetInputs ();
		SteeringWheels ();
		UpdateWheelPoses ();
		BoolManager ();
		otonomHizlanma ();
		hizLimitleme ();
		kontrolBilgisayarda ();

	}




}
