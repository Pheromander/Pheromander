using UnityEngine;
using System.Collections;

/**
 * State machine of the agent.
 */
public class AgentStates : MonoBehaviour {
	/**
	 * Reference to a set of mesh renderers which can be used
	 * to display the current task (color encoded by ColorEncode(EAntTasks))
	 * on the mesh of the agent.
	 */
	[SerializeField]
	MeshRenderer[] _taskDisplays;

	/**
	 * Reference to a mesh rederer which can be used to
	 * display the current state (color encoded by ColorEncode(EAntStates)
	 * on the mehs of the agent.
	 */
	[SerializeField]
	MeshRenderer _statusDisplay;

	/**
	 * Translate a given state to a human readable name.
	 */
	public static string StateToString(EAntStates state) {
		switch (state) {
		case EAntStates.Idle: return "Random walk";
		case EAntStates.Harvesting: return "Harvesting";
		case EAntStates.ReturnToBase: return "Back to Base";
		case EAntStates.FollowPheromone: return "Reacting to Pheromones";
		case EAntStates.Combat: return "In combat";
		default: return "n/a";
		}
	}

	/**
	 * Translate a given task to a human readable name.
	 */
	public static string TaskToString(EAntTasks task) {
		switch (task) {
		case EAntTasks.HarvestFood:	return "Harvest";
		case EAntTasks.Attack: return "Combat";
		default: return "n/a";
		}
	}

	/**
	 * Translate given state into a color code.
	 */
	public static Color ColorEncode(EAntStates state) {
		switch (state) {
		case EAntStates.Idle: return Color.white;
		case EAntStates.Harvesting: return Color.blue;
		case EAntStates.ReturnToBase: return new Color(184, 110, 0);
		case EAntStates.FollowPheromone: return Color.green;
		case EAntStates.Combat: return Color.red;
		default: return Color.magenta;
		}
	}

	/**
	 * Translate a task into a color code.
	 */
	public static Color ColorEncode(EAntTasks task) {
		switch (task) {
		case EAntTasks.HarvestFood: return Color.green;
		case EAntTasks.Attack: return Color.red;
		default: return Color.magenta;
		}
	}

	/**
	 * Short cut to check if agent is idling (random walk).
	 */
	public bool IsIdling() {
		return currentState == EAntStates.Idle;
	}

	/**
	 * Short cut to check if agent is reacting to pheromones.
	 */
	public bool IsFollowingPheromone() {
		return currentState == EAntStates.FollowPheromone;
	}

	/**
	 * Short cut to check if agent is returnign to base.
	 */
	public bool IsReturningToBase() {
		return currentState == EAntStates.ReturnToBase;
	}

	/**
	 * Short cut to check if agent is  arvesting.
	 */
	public bool IsHarvesting() {
		return currentState == EAntStates.Harvesting;
	}

	/**
	 * Reassign the task of the agent.
	 */
	public void SetTask(EAntTasks task) {
		_task = task;

		foreach (MeshRenderer taskDisplay in _taskDisplays) {
			taskDisplay.material.color = ColorEncode (_task);
		}
	}

	/**
	 * Get the current task of the agent.
	 */
	public EAntTasks GetTask() {
		return _task;
	}

	EAntStates _currentState;

	/**
	 * Property to get and set the current state of the agent.
	 */
	public EAntStates currentState {
		get { return _currentState; }
		set { _currentState = value;
			ChangeComponentStates ();
		}
	}

	private EAntTasks _task;

	AgentRandomWalk randomWalk;
	AgentHarvesting harvesting;
	AntPheromonePlacement pheromonePlacement;
	AgentPheromonePerception pheromonePerception;
	AgentReturnToBase returnToBase;
	AgentCombat combat;

	void Awake() {
		randomWalk = GetComponent<AgentRandomWalk> ();
		harvesting = GetComponent<AgentHarvesting> ();
		pheromonePlacement = GetComponent<AntPheromonePlacement> ();
		pheromonePerception = GetComponent<AgentPheromonePerception> ();
		returnToBase = GetComponent<AgentReturnToBase> ();
		combat = GetComponent<AgentCombat>();
	}

	// Use this for initialization
	void Start () {
		currentState = EAntStates.Idle;

		// Enable /  disable components as necessary
		randomWalk.enabled = true;
		harvesting.enabled = false;
		pheromonePlacement.enabled = false;
		pheromonePerception.enabled = true;
		returnToBase.enabled = false;
		combat.enabled = false;
	}
	
	/**
	 * Enable/disable agent components according
	 * to the current state of the agent.
	 */
	void ChangeComponentStates () {
		// Activate stuff of current state
		switch(currentState) {
		case EAntStates.Idle:
			randomWalk.enabled = true;
			pheromonePerception.enabled = true;
			pheromonePlacement.enabled = false;
			returnToBase.enabled = false;
			combat.enabled = false;
			break;
		case EAntStates.FollowPheromone:
			randomWalk.enabled = false;
			break;
		case EAntStates.Harvesting:
			randomWalk.enabled = false;
			harvesting.enabled = true;
			pheromonePerception.enabled = false;
			break;
		case EAntStates.ReturnToBase:
			randomWalk.enabled = false;
			pheromonePlacement.enabled = true;
			pheromonePerception.enabled = true;
			harvesting.enabled = false;
			returnToBase.enabled = true;
			break;
		case EAntStates.Combat:
			randomWalk.enabled = false;
			pheromonePerception.enabled = true;
			pheromonePlacement.enabled = true;
			harvesting.enabled = false;
			returnToBase.enabled = false;
			combat.enabled = true;
			break;
		}

		// Change color to display current state
		if (_statusDisplay != null) {
			_statusDisplay.material.color = ColorEncode (currentState);
		}
	}

	public string GetStatusString() {
		return AgentStates.StateToString (currentState);
	}

	public string GetTaskString() {
		return AgentStates.TaskToString(_task);
	}
}

/**
 * EAntStates describe the current action of the ant, e.g. if it traces a pheromone trail.
 */
public enum EAntStates {
	/**
	 * Random walking
	 */
	Idle,
	/**
	 * Reacting to pheromones
	 */
	FollowPheromone,
	/**
	 * Returning to the base
	 */
	ReturnToBase,
	/**
	 * Collecting resources
	 */
	Harvesting,
	/**
	 * Fighting an enemy.
	 */
	Combat
}

/**
 * EAntTasks describe the overall task of an ant, for example if it is searching for food.
 */
public enum EAntTasks {
	/**
	 * search and collect resources.
	 */
	HarvestFood,
	/**
	 * Search and attack enemies.
	 */
	Attack
}