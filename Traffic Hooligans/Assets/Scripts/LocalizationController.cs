using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationController : MonoBehaviour {

	public static LocalizationController varlik;
	private Dictionary<string, string> yerelText;
	private bool hazirMi = false;
	private string missingTextString = "yerellestirilmis text bulunamadi";

	// Use this for initialization
	void Awake () {
		if (varlik == null) {
			varlik = this;
		} else if (varlik != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
	
	public void LoadLocalizedTextOnEditor(string dosyaIsmi){
		yerelText = new Dictionary<string, string> ();
		string dosyaYolu = Path.Combine (Application.streamingAssetsPath + "/" ,	 dosyaIsmi);
		Debug.Log ("Dosya yolu: " + dosyaYolu);
		if (File.Exists (dosyaYolu)) {
			string jsonVerisi = File.ReadAllText (dosyaYolu);
			LocalizationData veri = JsonUtility.FromJson<LocalizationData> (jsonVerisi);

			for (int i = 0; i < veri.items.Length; i++) {
				yerelText.Add (veri.items [i].key, veri.items [i].value);
			}
			Debug.Log ("Veri yüklendi. Sözlük " + yerelText.Count + " tane anahtar / değer ikilisi içeriyor.");
		} else {
			Debug.LogError (dosyaIsmi + " isimli dosya bulunamadı!");
		}
		hazirMi = true;
	}

	IEnumerator LoadLocalizedTextOnAndroid(string dosyaIsmi){
		yerelText = new Dictionary<string, string> ();
		string dosyaYolu = Path.Combine (Application.streamingAssetsPath + "/", dosyaIsmi);
		string jsonVerisi;
		if (dosyaYolu.Contains ("://") || dosyaYolu.Contains (":///")) {
			UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get (dosyaYolu);
			yield return www.SendWebRequest();
			jsonVerisi = www.downloadHandler.text;
		} else {
			jsonVerisi = File.ReadAllText (dosyaYolu);
		}
		LocalizationData veri = JsonUtility.FromJson<LocalizationData> (jsonVerisi);
		for (int i = 0; i < veri.items.Length; i++) {
			yerelText.Add (veri.items [i].key, veri.items [i].value);
		}
		Debug.Log ("Veri yüklendi. Sözlük " + yerelText.Count + " tane anahtar / değer ikilisi içeriyor.");
		hazirMi = true;
	}

	public void DilBelirle(string dosyaIsmi){
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			LoadLocalizedTextOnEditor (dosyaIsmi);
		} else if (Application.platform == RuntimePlatform.OSXEditor) {
			LoadLocalizedTextOnEditor (dosyaIsmi);
		} else if (Application.platform == RuntimePlatform.Android) {
			StartCoroutine ("LoadLocalizedTextOnAndroid", dosyaIsmi);
		}
	}

	public string GetLocalizedValue(string key){
		string result = missingTextString;
		if (yerelText.ContainsKey (key)) {
			result = yerelText [key];
		}
		return result;
	}

	public bool IsReady(){
		return hazirMi;
	}
}
