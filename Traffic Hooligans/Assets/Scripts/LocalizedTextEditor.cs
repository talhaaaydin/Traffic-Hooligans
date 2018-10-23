/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LocalizedTextEditor : UnityEditor {

	public LocalizationData locData;

	[MenuItem("Window/LocalizedTextEditor")]
	static void Init(){
		EditorWindow.GetWindow (typeof(LocalizedTextEditor)).Show ();
	}

	private void OnGUI(){
		if (locData != null) {
			SerializedObject serObj = new SerializedObject (this);
			SerializedProperty serProp = serObj.FindProperty ("locData");
			EditorGUILayout.PropertyField (serProp, true);
			serObj.ApplyModifiedProperties ();

			if (GUILayout.Button ("Save Data")) {
				SaveGameData ();
			}
		}

		if (GUILayout.Button ("Load Data")) {
			LoadGameData ();
		}

		if (GUILayout.Button ("Create new data")) {
			CreateNewData ();
		}
	}

	private void LoadGameData(){
		string filePath = EditorUtility.OpenFilePanel ("Yerelleştirme veri dosyasını seçiniz", Application.streamingAssetsPath, "json");

		if (!string.IsNullOrEmpty (filePath)) {
			string dataAsJson = File.ReadAllText (filePath);
			locData = JsonUtility.FromJson<LocalizationData> (dataAsJson);
		}
	}

	private void SaveGameData(){
		string filePath = EditorUtility.SaveFilePanel ("Yerelleştirme veri dosyasını kaydet", Application.streamingAssetsPath, "yerellestirmeVerisi", "json");
		if (!string.IsNullOrEmpty (filePath)) {
			string dataAsJson = JsonUtility.ToJson (locData);
			File.WriteAllText (filePath, dataAsJson);
		}
	}

	private void CreateNewData(){
		locData = new LocalizationData ();
	}
}
*/