using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class AdMobUnity : MonoBehaviour {

	InterstitialAd interstitial;
	RewardBasedVideoAd rewardBasedVideo;
	public string GarajSahneIsmı = "Garage";
	public GameObject MoneyIsIncreasing;

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		string appId = "ca-app-pub-5726198782816496~6039228110";
		#elif UNITY_IPHONE
		string appId = "ca-app-pub-3940256099942544~1458002511";
		#else
		string appId = "unexpected_platform";
		#endif

		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appId);

		// Get singleton reward based video ad reference.
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		// Called when an ad request has successfully loaded.
		rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
		// Called when an ad request failed to load.
		rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
		// Called when an ad is shown.
		rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
		// Called when the ad starts to play.
		rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
		// Called when the user should be rewarded for watching a video.
		rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
		// Called when the ad is closed.
		rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
		// Called when the ad click caused the user to leave the application.
		rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;


	}

		public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		}

		public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
		MonoBehaviour.print(
		"HandleRewardBasedVideoFailedToLoad event received with message: "
		+ args.Message);
		}

		public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
		}

		public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
		}

		public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		}

		public void HandleRewardBasedVideoRewarded(object sender, Reward args)
		{
		string type = args.Type;
		double amount = args.Amount;

		TextMeshProUGUI reklamOduluText = new TextMeshProUGUI();
			reklamOduluText = MoneyIsIncreasing.transform.GetChild (1).transform.gameObject.GetComponent<TextMeshProUGUI> ();
			PlayerPrefs.SetFloat ("para", (float)amount + PlayerPrefs.GetFloat ("para", 0));

			MoneyIsIncreasing.SetActive (true);
			Invoke ("MoneyIsIncreasingOff", MoneyIsIncreasing.transform.GetChild (0).transform.gameObject.GetComponent<Animation> ().clip.length);
			

		MonoBehaviour.print(
		"HandleRewardBasedVideoRewarded event received for "
		+ amount.ToString() + " " + type);
		reklamOduluText.text = "+" + amount.ToString ();
		}

		void MoneyIsIncreasingOff(){
			MoneyIsIncreasing.SetActive (false);
			

		}

		public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
		}

		private void RequestRewardBasedVideo()
		{
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5726198782816496/8998203991";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/1712485313";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded video ad with the request.
		rewardBasedVideo.LoadAd(request, adUnitId);
		}

		public void ShowRewardAd(){
		RequestRewardBasedVideo ();
		if (rewardBasedVideo.IsLoaded()) {
		rewardBasedVideo.Show();
		}
		}



		//##Oyun Bitiş Reklamı ##

		private void RequestInterstitial(){
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5726198782816496/6166783258";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/4411468910";
		#else
		string adUnitId = "unexpected_platform";
		#endif


		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);

		// Called when an ad request has successfully loaded.
		interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		interstitial.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
		}



		public void HandleOnAdLoaded(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdLoaded event received");
		}

		public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
		+ args.Message);
		}

		public void HandleOnAdOpened(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdOpened event received");
		}

		public void HandleOnAdClosed(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdClosed event received");
		}

		public void HandleOnAdLeavingApplication(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
		}

		public void ShowInterstitialAd(){
		RequestInterstitial ();
		if (interstitial.IsLoaded ()) {
		interstitial.Show ();
		}
		}
	

}
