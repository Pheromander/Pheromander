using UnityEngine;
using System.Collections;

/**
 * Component controling the placement of pheromones of agents.
 */
public class AntPheromonePlacement : MonoBehaviour {
	/**
	 * The global pheromone manager used to deposit pheromones on the map.
	 */
	PheromoneManager _pheromoneManager;

	/**
	 * The type of the pheromone that should be emitted.
	 */
	EPheromoneTypes _type;

	/**
	 * Timer to measure if a pheromone should be deposited or not.
	 */
	float placementTimer = 0.0f;

	GlobalAgentConfiguration _agentConfiguration;

	void Awake() {
		_pheromoneManager = (PheromoneManager)Object.FindObjectOfType (typeof(PheromoneManager));
		_agentConfiguration = FindObjectOfType (typeof(GlobalAgentConfiguration)) as GlobalAgentConfiguration;

		_type = EPheromoneTypes.Food;
	}

	/**
	 * When enabled, deposit a pheromone of type _type. The rate is defined in the
	 * GlobalAgentconfiguration object.
	 */
	void Update () {
		placementTimer += Time.deltaTime;

		if (placementTimer >= _agentConfiguration.pheromonePlacementRate) {
			Vector3 pheromonePosition = new Vector3 (transform.position.x, 0, transform.position.z);
			pheromonePosition = pheromonePosition - transform.forward;

			_pheromoneManager.PlacePheromone (pheromonePosition, _type);

			placementTimer = 0.0f;
		}
	}

	/**
	 * Set the type of pheromones that should be deposited.
	 * 
	 * This is set by other components according to the current task and state of the agent.
	 */
	public void SetPheromoneType(EPheromoneTypes type) {
		_type = type;
	}
}
