using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizationText : MonoBehaviour {

	public string key;
	public bool TextMeshProMu = true;

	// Use this for initialization
	void Start () {
		if (TextMeshProMu) {
			TextMeshProUGUI text = GetComponent<TextMeshProUGUI> ();
			text.text = LocalizationController.varlik.GetLocalizedValue (key);
		} else {
			Text text = GetComponent<Text> ();
			text.text = LocalizationController.varlik.GetLocalizedValue (key);
		}

	}
}
