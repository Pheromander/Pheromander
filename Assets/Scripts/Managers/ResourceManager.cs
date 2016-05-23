using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour {

	/**
	 * The current number of resources.
	 */
	uint resourceCount = 0;

	/**
	 * Reference to the StatisticManager.
	 */
	StatisticsManager _statisticsManager;

	void Awake() {
		_statisticsManager = FindObjectOfType (typeof (StatisticsManager)) as StatisticsManager;
	}

	/**
	 * Add a certain amount of resources.
	 */
	public void AddResources(uint amount) {
		resourceCount += amount;
		_statisticsManager.totalResources += amount;
	}

	/**
	 * Withdraw a certain amount of resources.
	 */
	public void RemoveResources(uint amount) {
		resourceCount -= amount;
	}

	/**
	 * Get the number of Resources.
	 */
	public uint GetResourceCount() {
		return resourceCount;
	}
}
