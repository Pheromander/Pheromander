using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadSceneScript : MonoBehaviour {

	public GameObject main;
	public GameObject settings;


	public void startNewGame(){
		SceneManager.LoadSceneAsync (1);
	}

	public void startChallenge() {
		SceneManager.LoadSceneAsync (2);
	}

	public void startTutorial(){
		SceneManager.LoadSceneAsync (3);
	}

	public void backToMainMenu() {
		SceneManager.LoadScene (0);
	}

	public void exitGame(){
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
	}
}
