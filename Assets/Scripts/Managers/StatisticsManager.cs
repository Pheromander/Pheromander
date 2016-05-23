using UnityEngine;
using System.Collections.Generic;
using System.IO;

/**
 * StatisticsManager collects statistics about the current game.
 */
public class StatisticsManager : MonoBehaviour {
    public bool dumpStatistics = true;

	public uint totalResources = 0;

    public uint currentPopulation = 0;
	public uint highestPopulation = 0;

	public uint agentsCreated = 0;
	public uint agentsLost = 0;
	public float agentTotalDamage = 0;

	public float enemyTotalDamage = 0;
	public uint enemiesKilled = 0;

    /// <summary>
    /// The number of pheromones placed by the player grouped by pheromone type.
    /// </summary>
    public Dictionary<EPheromoneTypes, uint> pheromonesPlaced;

    /// <summary>
    /// The number of configuration changes applied to pheromones grouped by their type.
    /// </summary>
    public Dictionary<EPheromoneTypes, uint> pheromoneConfigChanged;

    /// <summary>
    /// The number of configuration changes applied to agents.
    /// </summary>
    public uint agentConfigChanged;

	/// <summary>
	/// The time when the simulation was startet.
	/// </summary>
	float _simulationStartTime = 0f;

    /// <summary>
    /// Path to which the statistics are logged.
    /// </summary>
    string statisticsLogFilePath;

    /// <summary>
    /// A number to numerate the log output.
    /// </summary>
    uint logNumber = 0;

	/// <summary>
	/// Gets the ingame time i.e. how long the simulation was running.
	/// </summary>
	/// <example>
	/// Example: In challenge mode, the level is loaded but the simulation is not started immediately
	/// but only after the "Start Challenge" button was clicked. The ingameTime has to be used in this
	/// case to measure the accurate time since the beginning of the simulation.
	/// </example>
	/// <value>The ingame time.</value>
	public float ingameTime {
		get {
			return Time.timeSinceLevelLoad - _simulationStartTime;
		}
		private set { }
	}

    void Awake()
    {
        pheromonesPlaced = new Dictionary<EPheromoneTypes, uint>();
        pheromonesPlaced[EPheromoneTypes.Food] = 0;
        pheromonesPlaced[EPheromoneTypes.Attack] = 0;
        pheromonesPlaced[EPheromoneTypes.Repellant] = 0;

        pheromoneConfigChanged = new Dictionary<EPheromoneTypes, uint>();
        pheromoneConfigChanged[EPheromoneTypes.Food] = 0;
        pheromoneConfigChanged[EPheromoneTypes.Attack] = 0;
        pheromoneConfigChanged[EPheromoneTypes.Repellant] = 0;
    }
				

	void Start() {
        _simulationStartTime = Time.timeSinceLevelLoad;

        statisticsLogFilePath = Application.dataPath + "/../ChallengeLog.txt";

        if (dumpStatistics)
        {
            DumpStatisticsHeaderToFile();
            InvokeRepeating("DumpStatisticsToFile", 0, 10);
        }
	}

    void DumpStatisticsHeaderToFile()
    {
        string header = string.Format(
            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}\r\n",
            "#",
            "Ingame Time",
            "Total Resources",
            "Current Population",
            "Agents Lost",
            "Enemies Killed",
            "Harvest Pheromones Placed",
            "Combat Pheromones Placed",
            "Repellent Pheromones Placed",
            "Harvest Pheromone Re-Configurations",
            "Combat Pheromone Re-Configurations",
            "Repellent Pheromone Re-Configurations",
            "Agent Re-configurations"
        );

        File.AppendAllText(statisticsLogFilePath, "\r\nNew challenge run started.\r\n");
        File.AppendAllText(statisticsLogFilePath, header);
    }

    void DumpStatisticsToFile()
    {
        string logString = string.Format(
            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}\r\n",
            logNumber,
            ingameTime,
            totalResources,
            currentPopulation,
            agentsLost,
            enemiesKilled,
            pheromonesPlaced[EPheromoneTypes.Food],
            pheromonesPlaced[EPheromoneTypes.Attack],
            pheromonesPlaced[EPheromoneTypes.Repellant],
            pheromoneConfigChanged[EPheromoneTypes.Food],
            pheromoneConfigChanged[EPheromoneTypes.Attack],
            pheromoneConfigChanged[EPheromoneTypes.Repellant],
            agentConfigChanged
        );

        File.AppendAllText(statisticsLogFilePath, logString);

        ++logNumber;
    }
}
