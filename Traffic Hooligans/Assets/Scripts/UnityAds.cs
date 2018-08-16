using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UnityAds : MonoBehaviour {
	//---------- ONLY NECESSARY FOR ASSET PACKAGE INTEGRATION: ----------//
	#if UNITY_IOS
	private string gameId = "2740220";
	#elif UNITY_ANDROID
	private string gameId = "2740221";
	#endif
	//-------------------------------------------------------------------//
	public Button m_Button;
	public string GarajSahneIsmi = "Garage";
	public string placementId = "rewardedVideo";
	public float odulPara = 50f;
	public GameObject MoneyIsIncreasing;
	public GameObject odulluReklamHata, reklamiIzlemediniz, reklamGosterilemiyor;
	// Use this for initialization
	void Start () {
		if (SceneManager.GetActiveScene ().name == GarajSahneIsmi) {
			if (m_Button)
				m_Button.onClick.AddListener (ShowAd);
		}
		if (Advertisement.isSupported) {
			Advertisement.Initialize (gameId, true);
		}
	}

	public void ShowInterstitialAd(){
		Advertisement.Show ();
	}

	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene ().name == GarajSahneIsmi) {
			/*if (m_Button)
				m_Button.interactable = Advertisement.IsReady (placementId);
				*/
		}
	}

	void ShowAd ()
	{
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		Advertisement.Show(placementId, options);
	}

	void HandleShowResult (ShowResult result)
	{
		if(result == ShowResult.Finished) {
			Debug.Log("Video completed - Offer a reward to the player");
			PlayerPrefs.SetFloat ("para", odulPara + PlayerPrefs.GetFloat ("para", 0));

			MoneyIsIncreasing.SetActive (true);
			Invoke ("MoneyIsIncreasingOff", MoneyIsIncreasing.transform.GetChild (0).transform.gameObject.GetComponent<Animation> ().clip.length);

		}else if(result == ShowResult.Skipped) {
			odulluReklamHata.SetActive (true);
			reklamiIzlemediniz.SetActive (true);
			reklamGosterilemiyor.SetActive (false);
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}else if(result == ShowResult.Failed) {
			odulluReklamHata.SetActive (true);
			reklamiIzlemediniz.SetActive (false);
			reklamGosterilemiyor.SetActive (true);
			Debug.LogError("Video failed to show");
		}
	}

	void MoneyIsIncreasingOff(){
		MoneyIsIncreasing.SetActive (false);
	}

}
