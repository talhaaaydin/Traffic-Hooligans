using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class toplamPara : MonoBehaviour {

	private theGodScript theGodS;
	public TextMeshProUGUI toplamParaValue;
	private UnityAds unityAdS;

	// Use this for initialization
	void Start () {
		theGodS = GameObject.FindGameObjectWithTag ("theGod").GetComponent<theGodScript> ();
		float toplamPara = Mathf.RoundToInt (theGodS.gidilenYolParasi + theGodS.yuksekHizParasi + theGodS.yakinMakasParasi);
		toplamParaValue.text = toplamPara.ToString();
		PlayerPrefs.SetFloat ("para", PlayerPrefs.GetFloat ("para", 0) + toplamPara);
		Debug.Log (PlayerPrefs.GetFloat ("para", 0));
		int kacKereOynandi = 0;
		kacKereOynandi = PlayerPrefs.GetInt ("kacKereOynandi", 0);
		kacKereOynandi++;
		PlayerPrefs.SetInt ("kacKereOynandi", kacKereOynandi);
		unityAdS = GetComponent<UnityAds> ();
		Reklam ();
	}


	void Reklam(){
		int kacKereOynandi = PlayerPrefs.GetInt ("kacKereOynandi", 0);
		if (kacKereOynandi != 0 && kacKereOynandi % 3 == 0) {
			unityAdS.ShowInterstitialAd ();
		}
	}


}
