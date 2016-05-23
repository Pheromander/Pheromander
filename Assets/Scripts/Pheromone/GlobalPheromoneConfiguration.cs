using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Contains configuration for all available pheromone types.
public class GlobalPheromoneConfiguration : MonoBehaviour {
	// Dictionary that holds one pheromone configuration per pheromone type.
	public Dictionary<EPheromoneTypes, PheromoneConfiguration> configs = new Dictionary<EPheromoneTypes, PheromoneConfiguration>()
	{
		{ EPheromoneTypes.Food, new PheromoneConfiguration(Color.green) },
		{ EPheromoneTypes.Attack, new PheromoneConfiguration(Color.red, 12f, 0.2f, 0.02f) },
		{ EPheromoneTypes.Repellant, new PheromoneConfiguration(Color.blue, 5.0f, 0.02f) }
	};
}

// Configuration class for a single pheromone type
public class PheromoneConfiguration {
	public PheromoneConfiguration(Color color, float initialIntensity = 1.7809f, float diffusionRate = 0.2f, float minimumConcentration = 0.02f) {
		this.color = color;
		this.initialIntensity = initialIntensity;
		this.diffusionRate = diffusionRate;
		this.minimumConcentration = minimumConcentration;
	}

	public float initialIntensity;
	public float initialRadius = 1.0f;
	public float diffusionRate;
	public float minimumConcentration;
	public Color color;
	public bool particleSystemVisible = true;
}