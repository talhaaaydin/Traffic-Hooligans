using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qualityDropdownChangerByLanguage : MonoBehaviour {
	private string turkce = "GERI", english = "BACK", deutsch = "ZURUCK";

	public GameObject turkceQualityDropdown, englishQualityDropdown, deutschQualityDropdown;
	// Use this for initialization
	void Start () {
		string kontrolText = LocalizationController.varlik.GetLocalizedValue ("geri_btn");
		if (kontrolText == turkce) {
			turkceQualityDropdown.SetActive (true);
			englishQualityDropdown.SetActive (false);
			deutschQualityDropdown.SetActive (false);
		} else if (kontrolText == english) {
			turkceQualityDropdown.SetActive (false);
			englishQualityDropdown.SetActive (true);
			deutschQualityDropdown.SetActive (false);
		} else if (kontrolText == deutsch) {
			turkceQualityDropdown.SetActive (false);
			englishQualityDropdown.SetActive (false);
			deutschQualityDropdown.SetActive (true);
		}
	}
}
