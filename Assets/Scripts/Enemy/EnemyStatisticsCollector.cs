using UnityEngine;
using System.Collections;

/**
 * Collects statistics about enemy objects.
 */
public class EnemyStatisticsCollector : MonoBehaviour {
	/**
	 * Reference to the StatisticsManager of the scene.
	 */
	StatisticsManager _statisticsManager;

	/**
	 * Initialize the component.
	 */
	void Awake() {
		_statisticsManager = FindObjectOfType (typeof(StatisticsManager)) as StatisticsManager;
	}

	/**
	 * Collect statistics about the damage done to agents.
	 * 
	 * This is triggered via message from the Health component.
	 */
	void OnDamaged(float damage) {
		_statisticsManager.enemyTotalDamage += damage;
	}

	/**
	 * Collect statistics about the death of enemies.
	 *
	 * This is triggered via message from the Health component.
	 */
	void OnDeath() {
		++_statisticsManager.enemiesKilled;
	}
}
