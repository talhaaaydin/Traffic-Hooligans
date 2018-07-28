using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMetodlar : MonoBehaviour {

	public float ReMap(float value, float from1, float to1, float from2, float to2){
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

}
