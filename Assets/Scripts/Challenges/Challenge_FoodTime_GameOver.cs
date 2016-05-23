using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * Script for the challange mode that is triggered if the
 * challange is over.
 */
public class Challenge_FoodTime_GameOver : MonoBehaviour {
	/**
	 * Reference to a text field to display the final score.
	 */
	[SerializeField]
	Text scoreText;

	/**
	 * Reference to the ingame gui which is disabled on game over.
	 */
	[SerializeField]
	GameObject _inGameGui;

	StatisticsManager statman;

	/**
	 * This is invoked when the challange is over (i.e. time run out).
	 */
	void Start () {
		statman = FindObjectOfType (typeof(StatisticsManager)) as StatisticsManager;

		scoreText.text = statman.totalResources.ToString();

		// Pause the game
		Time.timeScale = 0;

		// Hide ingame gui to prevent interaction
		_inGameGui.SetActive(false);

		// Disable player -> environment interaction (sry for the hacky solution :((( ).
		EnvironmentInteraction environmentInteraction = FindObjectOfType(typeof(EnvironmentInteraction)) as EnvironmentInteraction;
		environmentInteraction.Activate (EnvironmentInteraction.EInteractionType.EEI_NONE);
		environmentInteraction.Deactivate (EnvironmentInteraction.EInteractionType.EEI_NONE);
	}

	public void Close() {
		// Unpause the game.
		Time.timeScale = 1;
	}
}
