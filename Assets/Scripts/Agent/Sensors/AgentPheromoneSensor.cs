using UnityEngine;
using System.Collections;

/**
 * Sensor class that informs the AgentPheromonePerception component
 * of the agent about pheromones that entered and left their minimum
 * pheromone perception range.
 */
public class AgentPheromoneSensor : MonoBehaviour {
	/**
	 * Refernce to the pheromone perception component.
	 * 
	 * This is notified about pheromones which entered or left the sensor.
	 */
	[SerializeField]
	AgentPheromonePerception _pheromoneBehaviour;

	void OnTriggerEnter(Collider collider) {
		Pheromone pheromone = collider.GetComponentInParent<Pheromone> ();
		if (pheromone) {
			_pheromoneBehaviour.PheromoneEntered (pheromone);
		}
	}

	void OnTriggerExit(Collider collider) {
		Pheromone pheromone = collider.GetComponentInParent<Pheromone> ();
		if(pheromone) {
			_pheromoneBehaviour.PheromoneExited(pheromone);
		}
	}
}
