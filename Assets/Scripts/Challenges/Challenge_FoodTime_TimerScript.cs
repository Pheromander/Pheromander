using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Challenge_FoodTime_TimerScript : MonoBehaviour {
	[SerializeField]
	int timerStartValue = 60;

	[SerializeField]
	Text ingameScore;

	[SerializeField]
	Text timerText;

	[SerializeField]
	GameObject gameOverPanel;

	StatisticsManager _statisticsManager;


	/// <summary>
	/// This is invoked when the challenge is started.
	/// </summary>
	void Start () {
		_statisticsManager = FindObjectOfType (typeof (StatisticsManager)) as StatisticsManager;

		// Start the statistics manager. Required to start ingame time measurement only after the
		// "Start Challenge" button was pressed.
		_statisticsManager.enabled = true;

		InvokeRepeating ("Refresh", 0f, 1.0f);
	}
	

	void Refresh () {
		UpdateTime ();
		UpdateIngameScore ();
		CheckGameStatus ();
	}

	public void UpdateTime(){
		timerStartValue -= 1;

		string timeString = (timerStartValue / 60).ToString ();
		timeString += ":";

		int seconds = timerStartValue % 60;

		timeString += seconds < 10 ? "0" + seconds : seconds.ToString();

		timerText.text = timeString;
	}

	void UpdateIngameScore() {
		ingameScore.text = "Score: " + _statisticsManager.totalResources;
	}

	void CheckGameStatus(){
		if (timerStartValue < 0) {
			CancelInvoke ("Refresh");
			Time.timeScale = 0f;
			gameOverPanel.SetActive (true);
		}
	}
}
