using System.Collections;
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
	public GameObject pausePanel, settingsPanel, hizGosterge, yolGosterge, skorGosterge, yuksekHizGosterge, buttons, skorPanel;
	public AudioMixer aMixer;
	[SerializeField]
	public bool carptim;
	public Slider VolumeSlider;
	private AudioSource[] audioSource;
	public TextMeshProUGUI skorValueText;
	public string anaSahneIsmi = "mainScene";

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

	public void DeleteAllPlayerPrefs(){
		PlayerPrefs.DeleteAll ();
	}

	public void UpgradeButtons(string ability){
		GameObject.FindGameObjectWithTag ("Player").GetComponent<carAbilities> ().Upgrade (ability);
	}

	public void pause(){
		if (SahneIsmi == anaSahneIsmi) {
			Animation animation = pauseButton.gameObject.GetComponent<Animation> ();
			animation.Play (animation.clip.name);

			hizGosterge.SetActive (false);
			yolGosterge.SetActive (false);
			skorGosterge.SetActive (false);
			yuksekHizGosterge.SetActive (false);
			buttons.SetActive (false);
			pausePanel.SetActive (true);
			PauseAllAudio ();
			Time.timeScale = 0;

		}
	}

	void skorMenu(){
		if (SahneIsmi == anaSahneIsmi) {
			hizGosterge.SetActive (false);
			yolGosterge.SetActive (false);
			skorGosterge.SetActive (false);
			yuksekHizGosterge.SetActive (false);
			buttons.SetActive (false);
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
			hizGosterge.SetActive (true);
			yolGosterge.SetActive (true);
			skorGosterge.SetActive (true);
			yuksekHizGosterge.SetActive (true);
			buttons.SetActive (true);
			pausePanel.SetActive (false);
			ContinueAllAudio ();
			Time.timeScale = 1;
		}
	}

	public void AgainTheScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void GoToGarage(){
		SceneManager.LoadScene ("Garage");
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

	void Start(){
		HangiSahnedeyiz ();
		if (SahneIsmi == anaSahneIsmi) {
			float currentValue = PlayerPrefs.GetFloat ("volume",0);;
			VolumeSlider.value = currentValue;
			aMixer.SetFloat ("volume", currentValue);
			carptim = GetComponent<theGodScript> ().theCar.GetComponent<CarControllerScript>().carptim;
		}
	}

}
