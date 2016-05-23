using UnityEngine;
using System.Collections;

/**
 * Script defining the harvest behavior of agents.
 */
public class AgentHarvesting : MonoBehaviour {
	/**
	 * The time the agent needs to collect 1 unit of resources
	 * when harvesting.
	 */
	[SerializeField]
	float _harvestSpeed = 0.5f;

	/**
	 * Timer used to measure if the agent can collect another
	 * unit of resources (see _harvestSpeed).
	 */
	float _harvestTimer;

	AgentStates _states;

	AgentInventory _inventory;

	ResourceSource _currentSource;

	AntPheromonePlacement _pheromonePlacement;

	NavAgentHelper _navHelper;

	// Use this for initialization
	void Awake () {
		_states = GetComponent<AgentStates> ();
		_inventory = GetComponent<AgentInventory> ();
		_pheromonePlacement = GetComponent<AntPheromonePlacement> ();
		_navHelper = GetComponent<NavAgentHelper> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Increase harvest timer.
		_harvestTimer += Time.deltaTime;

		// Return to base if resource source depleted or inventory is full.
		if (!_currentSource || _inventory.IsFull ()) {
			_pheromonePlacement.SetPheromoneType (EPheromoneTypes.Food);
			_states.currentState = EAntStates.ReturnToBase;
			return;
		}

		// Harvest some resources if possible (timer).
		if (_harvestTimer >= _harvestSpeed) {
			_navHelper.Stop ();

			_currentSource.DecrementCapacity();
			_inventory.IncrementCapacity();
			_harvestTimer = 0.0f;
		}
	}

	/**
	 * Begin to harvest the resource.
	 * 
	 * This method is called by the AgentResourceSensor when it detects a
	 * resource.
	 */
	public void StartHarvesting(Collider other) {
		if (_states.GetTask () != EAntTasks.HarvestFood) {
			return;
		}

		ResourceSource resourceSource = other.GetComponent<ResourceSource> ();

		if (resourceSource) {
			_navHelper.SetDirection (other.transform.position - transform.position);
			_currentSource = resourceSource;
			_inventory.resourceType = resourceSource.resourceType;
			_harvestTimer = 0.0f;
			_states.currentState = EAntStates.Harvesting;
		}
	}
}
