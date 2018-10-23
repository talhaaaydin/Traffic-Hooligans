using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ekranaDokununScript : MonoBehaviour {
	TextMeshProUGUI ekranaDokununText;
	public GameObject LoadingPanel;
	public Image LoadingCircleProgress;
	public string GarajSahneIsmi = "Garage";
	AsyncOperation async;

	// Use this for initialization
	void Start () {
		ekranaDokununText = transform.GetChild (1).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI> ();	
	}

	public void EkranaDokunun(){
		//yükleme panelini görünür yapıyoruz.
		LoadingPanel.SetActive (true);
		ekranaDokununText.gameObject.SetActive (false);
		StartCoroutine (LoadingScreen ());
	}

	IEnumerator LoadingScreen(){
		//yukarıda belirlediğimiz async operation değişkenini tanımlıyoruz. arka planda ana sahneyi yükleyecek.
		async = SceneManager.LoadSceneAsync (GarajSahneIsmi);
		//şu anda arka planda yüklenen sahneye erişim yok
		async.allowSceneActivation = false;

		//arka planda yükleme işlemi tamamlanmadığı sürece bu döngü geçerli olacak
		while (!async.isDone) {
			LoadingCircleProgress.fillAmount = async.progress;

			//arka planda yükleme işlemi tamamlandığında progress değeri 0.9 olur bu yüzden 0.9a eşit mi diye bakıyoruz
			if (async.progress == 0.9f) {
				LoadingCircleProgress.fillAmount = 1;
				//ve arka planda yüklenen sahneye erişimi açıyoruz.
				async.allowSceneActivation = true;
			}
			//null değerini döndürüyoruz yani boş değerini.
			yield return null;
		}

	}

}
