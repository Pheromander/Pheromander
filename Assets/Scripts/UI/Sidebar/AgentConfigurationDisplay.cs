using UnityEngine;
using System.Collections;

public class AgentConfigurationDisplay : MonoBehaviour {
	[SerializeField]
	SliderWidget _smellAngleConfig;

	[SerializeField]
	SliderWidget _smellDistanceConfig;

	[SerializeField]
	SliderWidget _pheromonePlacementRateConfig;

	GlobalAgentConfiguration _agentConfiguration;

    StatisticsManager _statisticsManager;

	void Awake() {
		_agentConfiguration = FindObjectOfType (typeof(GlobalAgentConfiguration)) as GlobalAgentConfiguration;
        _statisticsManager = FindObjectOfType<StatisticsManager>();
	}

	// Use this for initialization
	void Show() {
		_smellAngleConfig.SetValue (_agentConfiguration.smellAngle * Mathf.Rad2Deg);

		_smellDistanceConfig.SetValue (_agentConfiguration.smellDistance);

		_pheromonePlacementRateConfig.SetValue (1.0f / _agentConfiguration.pheromonePlacementRate);
	}
	
	public void ApplyChanges() {
		_agentConfiguration.smellAngle = _smellAngleConfig.GetValue () * Mathf.Deg2Rad;
		_agentConfiguration.smellDistance = _smellDistanceConfig.GetValue ();

		_agentConfiguration.pheromonePlacementRate = 1.0f / _pheromonePlacementRateConfig.GetValue ();

        _statisticsManager.agentConfigChanged += 1;
	}
}
