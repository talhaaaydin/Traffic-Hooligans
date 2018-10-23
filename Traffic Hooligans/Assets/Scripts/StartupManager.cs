using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartupManager : MonoBehaviour {

	public GameObject LoadingPanel, LanguagePanel;
	public string garajSahneIsmi = "Garage";
	public Image LoadingCircleProgress;
	AsyncOperation async;

	// Use this for initialization
	private IEnumerator Start () {
		while (!LocalizationController.varlik.IsReady ()) {
			yield return true;
		}

		EkranaDokunun ();

	}

	public void EkranaDokunun(){
		LanguagePanel.SetActive (false);
		//yükleme panelini görünür yapıyoruz.
		LoadingPanel.SetActive (true);
		StartCoroutine (LoadingScreen ());
	}

	IEnumerator LoadingScreen(){
		//yukarıda belirlediğimiz async operation değişkenini tanımlıyoruz. arka planda ana sahneyi yükleyecek.
		async = SceneManager.LoadSceneAsync (garajSahneIsmi);
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
