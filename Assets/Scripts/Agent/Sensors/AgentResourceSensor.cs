using UnityEngine;
using System.Collections;

/**
 * Sensor that notifies the agent's AgentHarvesting component
 * when the agent entered a resource.
 */
public class AgentResourceSensor : MonoBehaviour {
	AgentHarvesting _harvesting;

	// Use this for initialization
	void Awake () {
		_harvesting = GetComponentInParent<AgentHarvesting> ();
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider collider) {
		// Start harvesting the resource.
		_harvesting.StartHarvesting (collider);
	}

}
