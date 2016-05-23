using UnityEngine;
using System.Collections;

public class PheromoneConfigurationDisplay : MonoBehaviour {
	[SerializeField]
	SliderWidget _initialIntensityConfig;

	[SerializeField]
	SliderWidget _diffusionConfig;

	[SerializeField]
	SliderWidget _minConcentrationConfig;

	[SerializeField]
	EPheromoneTypes _pheromoneType;

	PheromoneConfiguration _pheromoneConfiguration;

    StatisticsManager _statisticsManager;

	void Awake() {
		GlobalPheromoneConfiguration globalPheromoneConfiguration = FindObjectOfType (typeof(GlobalPheromoneConfiguration)) as GlobalPheromoneConfiguration;
		_pheromoneConfiguration = globalPheromoneConfiguration.configs [_pheromoneType];

        _statisticsManager = (StatisticsManager)Object.FindObjectOfType(typeof(StatisticsManager));
	}

	// Use this for initialization
	void Show() {
		_initialIntensityConfig.SetValue (_pheromoneConfiguration.initialIntensity);
		_diffusionConfig.SetValue (_pheromoneConfiguration.diffusionRate);
		_minConcentrationConfig.SetValue (_pheromoneConfiguration.minimumConcentration);
	}

	public void ApplyChanges() {
		_pheromoneConfiguration.initialIntensity = _initialIntensityConfig.GetValue ();
		_pheromoneConfiguration.diffusionRate = _diffusionConfig.GetValue ();
		_pheromoneConfiguration.minimumConcentration = _minConcentrationConfig.GetValue ();

        _statisticsManager.pheromoneConfigChanged[_pheromoneType] += 1;
	}
}
