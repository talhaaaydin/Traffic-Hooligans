﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class ButtonScripts : MonoBehaviour {
	[SerializeField]
	private string SahneIsmi;
	public Button pauseButton; 
	public GameObject pausePanel, settingsPanel, hizGosterge, yolGosterge, skorGosterge, yuksekHizGosterge, buttons, skorPanel, loadingPanel;
	public GameObject sagMiniMap, solMiniMap, solMiniMapCam, sagMiniMapCam;
	public Slider loadingSlider;
	public AudioMixer aMixer;
	public TMP_Dropdown qualityDropdown;
	AsyncOperation async;
	[SerializeField]
	public bool carptim;
	public Slider VolumeSlider;
	private AudioSource[] audioSource;
	public TextMeshProUGUI skorValueText;
	public string anaSahneIsmi = "mainScene", garajSahneIsmi = "Garage";

	private void HangiSahnedeyiz(){
		SahneIsmi = SceneManager.GetActiveScene ().name;
	}

	void Update(){
		if (SahneIsmi == anaSahneIsmi) {
			carptim = GetComponent<theGodScript> ().theCar.GetComponent<CarControllerScript> ().carptim;
			if (carptim) {
				skorMenu ();
			}
		}
	}

	public void CheatOnPlayerPrefs(float para){
		
			PlayerPrefs.SetFloat ("para", para);

	}

	public void ResetPlayerPrefs(){

		PlayerPrefs.DeleteAll ();

	}

	public void UpgradeButtons(string ability){
		GameObject.FindGameObjectWithTag ("Player").GetComponent<carAbilities> ().Upgrade (ability);
	}

	void GostergeButtonPanelActiveOrDeactive(bool t){
		hizGosterge.SetActive (t);
		yolGosterge.SetActive (t);
		skorGosterge.SetActive (t);
		yuksekHizGosterge.SetActive (t);
		buttons.SetActive (t);
		sagMiniMap.SetActive (t);
		sagMiniMapCam.SetActive (t);
		solMiniMap.SetActive (t);
		solMiniMapCam.SetActive (t);
	}

	public void pause(){
		if (SahneIsmi == anaSahneIsmi) {
			Animation animation = pauseButton.gameObject.GetComponent<Animation> ();
			animation.Play (animation.clip.name);

			GostergeButtonPanelActiveOrDeactive (false);

			pausePanel.SetActive (true);
			PauseAllAudio ();
			Time.timeScale = 0;

		}
	}

	void skorMenu(){
		if (SahneIsmi == anaSahneIsmi) {
			GostergeButtonPanelActiveOrDeactive (false);
			PauseAllAudio ();
			skorValueText.SetText(GetComponent<theGodScript>().skor.ToString("F1"));
			skorPanel.SetActive (true);
			Time.timeScale = 0;

		}
	}

	void PauseAllAudio(){
		audioSource = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource aSource in audioSource) {
			aSource.Pause ();
		}
	}

	void ContinueAllAudio(){
		audioSource = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource aSource in audioSource) {
			aSource.UnPause ();
		}
	}

	public void Continue(){
		if (SahneIsmi == anaSahneIsmi) {
			GostergeButtonPanelActiveOrDeactive (true);
			pausePanel.SetActive (false);
			ContinueAllAudio ();
			Time.timeScale = 1;
		}
	}

	public void AgainTheScene(){
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void GoToGarage(){
		pausePanel.SetActive (false);
		StartCoroutine (LoadingScreen ());

	}

	IEnumerator LoadingScreen(){
		loadingPanel.SetActive (true);
		async = SceneManager.LoadSceneAsync (garajSahneIsmi);
		async.allowSceneActivation = false;

		while (!async.isDone) {
			loadingSlider.value = async.progress;

			if (async.progress == 0.9f) {
				loadingSlider.value = 1;
				async.allowSceneActivation = true;
			}
			yield return null;
		}

	}


	public void Settings(){
		if (SahneIsmi == anaSahneIsmi) {
			pausePanel.SetActive (false);
			settingsPanel.SetActive (true);
		}
	}

	public void SettingsBack(){
		if (SahneIsmi == anaSahneIsmi) {
			pausePanel.SetActive (true);
			settingsPanel.SetActive (false);
		}
	}

	public void SetVolume(float volume){
		aMixer.SetFloat ("volume", volume);
		PlayerPrefs.SetFloat ("volume", volume);
	}

	public void SetQuality(int qualityIndex){
		QualitySettings.SetQualityLevel (qualityIndex);
		PlayerPrefs.SetInt ("quality", qualityIndex);
		Time.timeScale = 1;
	}

	void Start(){
		HangiSahnedeyiz ();
		if (SahneIsmi == anaSahneIsmi) {
			float currentValue = PlayerPrefs.GetFloat ("volume", 0);
			VolumeSlider.value = currentValue;
			aMixer.SetFloat ("volume", currentValue);
			int currentQuality = PlayerPrefs.GetInt ("quality", 1);
			QualitySettings.SetQualityLevel (currentQuality);
			Time.timeScale = 1;
			carptim = GetComponent<theGodScript> ().theCar.GetComponent<CarControllerScript> ().carptim;
		} else if (SahneIsmi == garajSahneIsmi) {
			float currentValue = PlayerPrefs.GetFloat ("volume",0);
			VolumeSlider.value = currentValue;
			aMixer.SetFloat ("volume", currentValue);
			int currentQuality = PlayerPrefs.GetInt ("quality", 1);
			qualityDropdown.value = currentQuality;
			QualitySettings.SetQualityLevel (currentQuality);
			Time.timeScale = 1;
		}
	}

}
