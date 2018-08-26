using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimationController : MonoBehaviour {

	Animator animator;
	CarControllerScript ccs;
	bool frenBasili;
	float horizontalInput;

	public string FrenAnimasyon = "BreakAnim";
	public string DonmeSolAnimasyon = "DonmeAnimSol";
	public string DonmeSagAnimasyon = "DonmeAnimSag";

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		ccs = GetComponent<CarControllerScript> ();
		frenBasili = ccs.frenBasili;
		horizontalInput = ccs.horizontalInput;
	}
	
	// Update is called once per frame
	void Update () {
		boolFloatUpdate ();
		AnimationManager ();
	}

	void AnimationManager(){
		if (frenBasili) {
			animator.SetBool ("BreakAnim", true);
		} else {
			animator.SetBool ("BreakAnim", false);
		}

		if (horizontalInput > 0.14f) {
			animator.SetBool ("DonmeAnimSag", true);
		} else {
			animator.SetBool ("DonmeAnimSag", false);
		}

		if (horizontalInput < -0.14f) {
			animator.SetBool ("DonmeAnimSol", true);
		} else {
			animator.SetBool ("DonmeAnimSol", false);
		}

	}

	void boolFloatUpdate(){
		frenBasili = ccs.frenBasili;
		horizontalInput = ccs.horizontalInput;


	}
}
