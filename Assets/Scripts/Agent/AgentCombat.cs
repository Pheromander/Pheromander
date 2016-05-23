using UnityEngine;
using System.Collections;

/**
 * Component defining the combat behavior of the agent.
 */
[RequireComponent (typeof(Attack))]
[RequireComponent (typeof(AgentStates))]
[RequireComponent (typeof(AntPheromonePlacement))]
[RequireComponent (typeof(NavAgentHelper))]
public class AgentCombat : MonoBehaviour {
	/**
	 * The Attack component.
	 */
	Attack _attack;

	/**
	 * Reference to the AgentStates component.
	 */
	AgentStates _states;

	/**
	 * Reference to the AgentPheromonePlacement component.
	 */
	AntPheromonePlacement _pheromonePlacement;

	/**
	 * Reference to the NavAgentHelper component.
	 */
	NavAgentHelper _navHelper;

	/**
	 * If an agent fights an enemy, it circles around the opponent.
	 * This variable defines the radius of this circle.
	 */
	float _randomRadius;

	/**
	 * Flag that is set to true if the agent is currently fighting an opponent.
	 */
	bool _inCombat = false;

	// Use this for initialization
	void Awake () {
		_attack = GetComponent<Attack> ();
		_states = GetComponent<AgentStates> ();
		_pheromonePlacement = GetComponent<AntPheromonePlacement> ();
		_navHelper = GetComponent<NavAgentHelper> ();

		_randomRadius = _attack.GetAttackRadius ();
	}
	
	// Update is called once per frame
	void Update () {
		// Check if another task was assigned. In this case,
		// combat mode should be left.
		if(_states.GetTask() != EAntTasks.Attack) {
			_states.currentState = EAntStates.Idle;
			return;
		}

		// If changed to combat mode
		if (_attack.HasTarget () && !_inCombat) {
			_inCombat = true;
			_pheromonePlacement.SetPheromoneType (EPheromoneTypes.Attack);
			return;
		}

		// If target was lost (and possibly was in combat mode)
		if (!_attack.HasTarget ()) {
			_inCombat = false;
			_states.currentState = EAntStates.Idle;
			return;
		}

		// We are in combat so update combat circling.
		if (_inCombat) {
			GameObject target = _attack.GetCurrentTarget ();
			Vector3 distance = target.transform.position - transform.position;

			// Only change circle if we risk to lose the target.
			if (distance.magnitude > (_randomRadius - 0.2)) {
				DoCombatCircling ();
			}
		}
	}

	/**
	 * Combating agents circle around their enemies. This method recalculates
	 * a random point inside this circle to which the agent moves.
	 */
	void DoCombatCircling() {
		GameObject target = _attack.GetCurrentTarget ();

		// Sanity check. THis is acutally not reqiured, since DoCombatCircling() is only
		// invoked when the agent is _inCombat. The integrity of this state is ensured by
		// the Update() method.
		if (!target) {
			return;
		}

		// Orbit around enemy to continue attack and spread pheromones
		Vector2 randomCircle = Random.insideUnitCircle;
		Vector3 randomPoint = new Vector3(randomCircle.x, 0, randomCircle.y);

		randomPoint = target.transform.position + randomPoint.normalized * _randomRadius;

		_navHelper.SetDirection(randomPoint - transform.position);
	}
}
