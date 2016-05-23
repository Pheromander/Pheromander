using UnityEngine;
using System.Collections;

/**
 * The enemy sensor of agents which notifies the Attack script of the agent
 * about possible targets that entered the agent's maximum attack range.
 */
public class AgentEnemySensor : MonoBehaviour {
	/**
	 * Reference to the attack component which is notified about enemies
	 * that are in range.
	 */
	[SerializeField]
	Attack _attack;

	/**
	 * Reference to the agent states component.
	 */
	[SerializeField]
	AgentStates _states;

	void OnTriggerEnter(Collider collider) {
		// The entered collider could be a new target.
		PossibleTargetInTrigger (collider);
	}
	
	void OnTriggerStay(Collider collider) {
		// The entered collider could be a new target.
		PossibleTargetInTrigger (collider);
	}

	void OnTriggerExit(Collider collider) {
		// When a collider leaves this sensor, the current
		// engaged target is possibly out of range now.
		_attack.LoseTarget (collider.gameObject);
	}

	void PossibleTargetInTrigger(Collider collider) {
		// If the agent is not in combat task, do nothing.
		if (_states.GetTask () != EAntTasks.Attack) {
			return;
		}

		// Notify attack script about new target.
		_attack.AcquireNewTarget(collider.gameObject);

		// Change to combat state (which may be left immediately, see AgentCombat).
		if (_states.GetTask () == EAntTasks.Attack) {
			_states.currentState = EAntStates.Combat;
		}
	}
}
