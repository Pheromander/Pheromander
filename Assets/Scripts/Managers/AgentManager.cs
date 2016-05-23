using UnityEngine;
using System.Collections;

// The AgentManager takes care of spawning agents and
// counting the size of the current agent population.
public class AgentManager : MonoBehaviour {
	// Reference to the agent prefab used to insatantiate new agents.
	[SerializeField]
	Object _agentPrefab;

	// Transform object at wich new agents are spawned.
	[SerializeField]
	Transform _spawnPoint;

	// The position at which agents are spawned.
	// This is the position of _spawnPoint with an adjusted y-value.
	Vector3 _spawnPosition;

	// The current maximum number of agents.
	//
	// At the beginning of the simulation, this value defines the
	// initial size of the population.
	//
	// To spawn new agenents lateron, use the IncreasePopulation() method.
	[SerializeField]
	uint _maxAgentCount = 100;

	// Get the current number of agents in the scene.
	public uint currentAgentCount { get; private set; }

	// The fraction (in percent) of the total population of agents that
	// get the combat task assigned when created.
	[SerializeField]
	int _fractionOfCombatAgents = 30;

	// Integer used to assign each agent an unique ID.
	uint _nextId = 1;

	// Flag to show if the SpawnAgentsUntilPopulationIsReached method
	// is invoked repeadetly to establish a population.
	bool isSpawningAgents = false;

	/**
	 * Instance of a StatisticManager.
	 */
	StatisticsManager _statisticsManager;

	// Initializes the AgentManager. Also takese care that the
	// initial agent population is spawned.
	void Start() {
		_spawnPosition = _spawnPoint.position;
		_spawnPosition.y = 1f;

		_statisticsManager = FindObjectOfType (typeof(StatisticsManager)) as StatisticsManager;

		InvokeRepeating ("SpawnAgentsUntilPopulationIsReached", 0f, 0.1f);
	}

	// Repeatedly spawns agents until the _maxAgentCount is reached.
	void SpawnAgentsUntilPopulationIsReached () {
		isSpawningAgents = true;

		if (_maxAgentCount == currentAgentCount) {
			isSpawningAgents = false;
			CancelInvoke ("SpawnAgentsUntilPopulationIsReached");
			return;
		}

		SpawnAgent ();
	}

	// Spawns a new agent with a random task determined by _fractionOfCombatAgents (see above).
	void SpawnAgent() {
		GameObject agent = Instantiate (_agentPrefab, _spawnPosition, new Quaternion()) as GameObject;
		agent.transform.eulerAngles = new Vector3 (0f, Random.Range(0f, 360f), 0f);
		agent.GetComponent<AgentAttributes> ().ID = _nextId++;
		agent.GetComponent<Health> ().notifyOnDeath = this.gameObject;

		// Assign random task
		int rnd = Random.Range(0, 100);
		if (rnd < _fractionOfCombatAgents) {
			agent.GetComponent<AgentStates> ().SetTask (EAntTasks.Attack);
		} else {
			agent.GetComponent<AgentStates> ().SetTask (EAntTasks.HarvestFood);
		}

		// Increase current agent count
		++currentAgentCount;
        _statisticsManager.currentPopulation = currentAgentCount;

		if (currentAgentCount > _statisticsManager.highestPopulation) {
			_statisticsManager.highestPopulation = currentAgentCount;
		}
	}

	// Gets invoked from other game objects when they die.
	// See Health script.
	void OnDeath(Health health) {
		// Decrease the current agent count and max population,
		// when an agent died.
		if (health.GetComponent<AgentStates> ()) {
			--_maxAgentCount;
			--currentAgentCount;
            _statisticsManager.currentPopulation = currentAgentCount;
		}
	}

	// Increases the population by delta, thus
	// spawning delta new agents.
	public void IncreasePopulation(uint delta) {
		_maxAgentCount += delta;

		if (!isSpawningAgents) {
			InvokeRepeating ("SpawnAgentsUntilPopulationIsReached", 0f, 0.1f);
		}
	}
}
