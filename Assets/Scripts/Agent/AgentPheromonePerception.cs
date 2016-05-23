using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * The pheromone perception behaviour of the agent. It makes the agent
 * move along pheromone trails.
 */
public class AgentPheromonePerception : MonoBehaviour {
	/**
	 * The agent's nav helper.
	 */
	NavAgentHelper _navHelper;

	/**
	 * The agents state machine.
	 */
	AgentStates _states;

	/**
	 * Nearby pheromones (detected by the AgentPheromoneSensor) grouped by
	 * pheromone types.
	 */
	Dictionary<EPheromoneTypes, HashSet<Pheromone>> _nearbyPheromones;

	/**
	 * The sphere collider used to detect nearby pheromones (see AgentPheromoneSensor).
	 */
	[SerializeField]
	SphereCollider _pheromoneSensorCollider;

	/**
	 * The current smell range as defined in the GlobalAgentConfiguration.
	 */
	float _currentSmellDistance;

	/**
	 * The current smell angle (as cosine!) as defined in the GlobalAgentConfiguration.
	 */
	float _currentSmellAngleCos;

	/**
	 * Stores the state of the agent, before the agent entered FollowingPheromone state.
	 */
	public
	EAntStates _previousState;

	GlobalAgentConfiguration _agentConfiguration;

	void Awake() {
		_navHelper = GetComponent<NavAgentHelper> ();
		_states = GetComponent<AgentStates> ();

		// Create dicts and sets for newarby pheromones.
		_nearbyPheromones = new Dictionary<EPheromoneTypes, HashSet<Pheromone>>();
		_nearbyPheromones [EPheromoneTypes.Food] = new HashSet<Pheromone> ();
		_nearbyPheromones [EPheromoneTypes.Attack] = new HashSet<Pheromone> ();
		_nearbyPheromones [EPheromoneTypes.Repellant] = new HashSet<Pheromone> ();

		_agentConfiguration = FindObjectOfType (typeof(GlobalAgentConfiguration)) as GlobalAgentConfiguration;
	}

	void Start() {
		_previousState = _states.currentState;
		_currentSmellDistance = 0f;
		UpdatePheromoneSensor ();

		// Repeadetly prune destroyed pheromones.
		InvokeRepeating ("PruneDeletedPheromones", 20.0f, 10.0f);
	}

	/**
	 * Updates the movement direction (see NavAgentHelper) according to the
	 * pheromones located near the agent.
	 * 
	 * Combat/Harvest pheromones are only taken into account, if the task of the agent
	 * is combat and harvest respectively.
	 * 
	 * Also: Combat/Harvest pheromones are ignored in certain states. Repellant pheromones
	 * are taken into account in every state.
	 */
	void Update() {
		_currentSmellAngleCos = Mathf.Cos (_agentConfiguration.smellAngle);

		UpdatePheromoneSensor ();

		Vector3 attractionDirection = new Vector3();

		if (_states.IsIdling ()) {
			_previousState = EAntStates.Idle;
		}

		// Only take harvest/combat pheromones into account if the agent is idling or following pheromones.
		if(_states.IsIdling() || _states.IsFollowingPheromone()) {
			if (_states.GetTask () == EAntTasks.HarvestFood && _previousState != EAntStates.ReturnToBase) {
				attractionDirection = AggregateMovementDirection (EPheromoneTypes.Food);
			} else if (_states.GetTask () == EAntTasks.Attack) {
				attractionDirection = AggregateMovementDirection (EPheromoneTypes.Attack);
			}
		}

		// Always take repellant pheromones into account.
		Vector3 repulsionDirection = AggregateMovementDirection (EPheromoneTypes.Repellant);

		// If pheromones influence movement direction, set this direction and switch
		// to pheromone following state if necessary.
		if (attractionDirection.sqrMagnitude > 0f || repulsionDirection.sqrMagnitude > 0f) {
			_navHelper.SetDirection (0.01f * attractionDirection + 0.99f * repulsionDirection);

			if (!_states.IsFollowingPheromone ()) {
				// Save the current state so we can restore status quo lateron.
				_previousState = _states.currentState;
				_states.currentState = EAntStates.FollowPheromone;
			}
		}
		// Switch back to previous state, if no longer following pheromones.
		else if(_states.IsFollowingPheromone()) {
			_states.currentState = _previousState;
		}

//		Debug.DrawLine (transform.position, transform.position + Quaternion.Euler(0f, GlobalAntConfiguration.smellAngle * Mathf.Rad2Deg, 0f) * transform.forward * _currentSmellDistance);
//		Debug.DrawLine (transform.position, transform.position + Quaternion.Euler(0f, -GlobalAntConfiguration.smellAngle * Mathf.Rad2Deg, 0f) * transform.forward * _currentSmellDistance);
	}

	/**
	 * Calculate attraction (or repulsion) for a given pheromone type. The resulting
	 * vector is the (negative) gradient and thus the direction the agent moves to.
	 * 
	 * @param pheromoneType The type of the pheromone for which the direction / gradient should be calculated.
	 * @return The direction in which the agent should move.
	 */
	Vector3 AggregateMovementDirection(EPheromoneTypes pheromonType) {
		HashSet<Pheromone> pheromones = _nearbyPheromones [pheromonType];

		Vector3 movementVector = new Vector3();

		Vector3 position = transform.position;
		position.y = 0.0f;

		foreach (Pheromone pheromone in pheromones) {
			// Ensure that no destroyed pheromones are accessed.
			if(!pheromone) {
				continue;
			}

			Vector3 pheromonePosition = pheromone.transform.position;
			pheromonePosition.y = 0.0f;

			Vector3 pheromoneVector = (pheromonePosition - position).normalized;
			float pheromoneAngleCosine = Vector3.Dot (transform.forward, pheromoneVector);

			// Check if pheromone is "in front of" the agent.
			if (pheromoneAngleCosine > _currentSmellAngleCos) {
				movementVector += pheromoneVector * pheromone.currentConcentration;
			}
		}

		// If the pheromone is repellant, return the negative movement vector.
		if (pheromonType == EPheromoneTypes.Repellant) {
			return -movementVector;
		}

		return movementVector;
	}

	/**
	 * When the configuration for the smell distance changed, the collider of the
	 * AgentPheromoneSensor has to be scaled to fit the new perception range.
	 */
	void UpdatePheromoneSensor() {
		if (_currentSmellDistance != _agentConfiguration.smellDistance) {
			_currentSmellDistance = _agentConfiguration.smellDistance;

			_pheromoneSensorCollider.radius = _currentSmellDistance;
		}
	}

	/**
	 * Called by the AgentPheromoneSensor when a new pheromone is percieved.
	 */
	public void PheromoneEntered(Pheromone pheromone) {
		_nearbyPheromones[pheromone.type].Add (pheromone);
	}

	/**
	 * Called by the AgentPheromoneSensor when an already percieved pheromone is out of range.
	 */
	public void PheromoneExited(Pheromone pheromone) {
		_nearbyPheromones[pheromone.type].Remove (pheromone);
	}

	/**
	 * Since deleted pheromones do not call the OnTriggerExit method on the AgentPheromoneSensor,
	 * this method is required. It prunes all destroyed pheromones from the _nearbyPheromones sets.
	 */
	private void PruneDeletedPheromones() {
		if (_states.GetTask () == EAntTasks.HarvestFood) {
			_nearbyPheromones [EPheromoneTypes.Food].RemoveWhere (ShouldBePruned);
		} else {
			_nearbyPheromones [EPheromoneTypes.Attack].RemoveWhere (ShouldBePruned);
		}

		_nearbyPheromones [EPheromoneTypes.Repellant].RemoveWhere (ShouldBePruned);
	}

	/**
	 * Method that checks if a given pheromone should be removed from the _nearbyPheromones sets.
	 */
	private bool ShouldBePruned(Pheromone pheromone) {
		if (pheromone) {
			return false;
		}
		return true;
	}
}