using UnityEngine;
using System.Collections;

/**
 * Simple key-value store that holds the agent configuration.
 * 
 * This configuration is shared among all agents so we
 * have a homogeneous set of agents.
 */
public class GlobalAgentConfiguration : MonoBehaviour {
	public float smellDistance = 2.0f;

	// Attention: this is stored as radians!
	public float smellAngle = 70.0f * Mathf.Deg2Rad;

	// Placement rate of pheromones.
	public float pheromonePlacementRate = 0.3f;
}
