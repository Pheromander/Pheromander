using UnityEngine;
using System.Collections;

/**
 * Component that collects statistics about the agent which it is attached to.
 */
public class AgentStatisticsCollector : MonoBehaviour {
	/**
	 * Reference to the statistics manager.
	 */
	StatisticsManager _statisticsManager;

	void Awake() {
		_statisticsManager = FindObjectOfType (typeof(StatisticsManager)) as StatisticsManager;
	}

	// When created, increase number of created agents.
	void Start() {
		++_statisticsManager.agentsCreated;
	}

	// On death, increase number of lost agents.
	void OnDeath() {
		++_statisticsManager.agentsLost;
	}

	// On damage, increase amount of total damage.
	void OnDamaged(float damage) {
		_statisticsManager.agentTotalDamage += damage;
	}
}
