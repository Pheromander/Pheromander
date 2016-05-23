using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatisticsDisplay : MonoBehaviour {
	[SerializeField]
	Text _timeValue;

	[SerializeField]
	Text _totalResourceValue;

	[SerializeField]
	Text _resourcesPerMinuteValue;

	[SerializeField]
	Text _highestPopulationValue;

	[SerializeField]
	Text _agentsCreatedValue;

	[SerializeField]
	Text _agentsLost;

	[SerializeField]
	Text _agentsTotalDamage;

	[SerializeField]
	Text _enemyTotalDamage;

	[SerializeField]
	Text _enemiesKilled;

	/**
	 * Reference to the StatisticsManager of the scene.
	 */
	StatisticsManager _statisticsManager;

	void Awake() {
		_statisticsManager = FindObjectOfType(typeof(StatisticsManager)) as StatisticsManager;
	}

	// Update is called once per frame
	void Update () {
		_timeValue.text = (_statisticsManager.ingameTime/ 60.0f).ToString("F2");

		_totalResourceValue.text = _statisticsManager.totalResources.ToString();
		_resourcesPerMinuteValue.text = (_statisticsManager.totalResources / (Time.timeSinceLevelLoad / 60.0f)).ToString("F2");

		_highestPopulationValue.text = _statisticsManager.highestPopulation.ToString ();

		_agentsCreatedValue.text = _statisticsManager.agentsCreated.ToString ();
		_agentsLost.text = _statisticsManager.agentsLost.ToString ();
		_agentsTotalDamage.text = _statisticsManager.agentTotalDamage.ToString ();

		_enemiesKilled.text = _statisticsManager.enemiesKilled.ToString ();
		_enemyTotalDamage.text = _statisticsManager.enemyTotalDamage.ToString ();
	}
}
