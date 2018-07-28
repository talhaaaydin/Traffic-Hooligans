using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VerySimpleColorManager : MonoBehaviour {
	public TextMeshProUGUI skorValueText;
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextMeshProUGUI> ().colorGradientPreset = skorValueText.colorGradientPreset;
	}
}
