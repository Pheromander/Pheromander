using UnityEngine;
using System.Collections;

/**
 * Class that display the selection markers of the agent when it is selected:
 * - attack range (red circle)
 * - maximum pehromone perception range (green circle)
 * - pheromone perception angles (grey lines)
 */
public class AgentSelectionMarker : MonoBehaviour {
	/**
	 * Game object which holds the attack range and pheromone perception range circles.
	 */
	[SerializeField]
	GameObject _selectionMarkers;

	/**
	 * Line renderer to render the pheromone perception angle to the left.
	 */
	[SerializeField]
	LineRenderer _leftAngleLine;

	/**
	 * Line renderer to render the pheromone perception angle to the right.
	 */
	[SerializeField]
	LineRenderer _rightAngleLine;

	/**
	 * Direct reference to the pheromone perception circle.
	 * 
	 * Used to scale the circle to match the pheromone perception range.
	 */
	[SerializeField]
	GameObject _perceptionCircle;

	/**
	 * Direct reference to the attack range circle.
	 * 
	 * Used to scale the circle to match the attackr ange.
	 */
	[SerializeField]
	GameObject _attackCircle;

	GlobalAgentConfiguration _agentConfiguration;

	void Awake() {
		_agentConfiguration = FindObjectOfType (typeof(GlobalAgentConfiguration)) as GlobalAgentConfiguration;
	}

	/**
	 * Display selection markers.
	 */
	public void Show() {
		// Update attack circle only on show, since the attack range
		// is not configurable.
		float attackRadius = GetComponent<Attack>().GetAttackRadius() * 2;
		_attackCircle.transform.localScale = new Vector3 (attackRadius, attackRadius, 1f);
			
		_selectionMarkers.SetActive(true);
	}

	/**
	 * Hide selection markers.
	 */
	public void Hide() {
		_selectionMarkers.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		float currentPerceptionRange = _agentConfiguration.smellDistance;

		// Recalculate ends of left and right pheromone perception angle line renderers to match the configuration.
		Vector3 angleBaseVector = new Vector3 (0f, 0f, 1f) * currentPerceptionRange;
		_leftAngleLine.SetPosition (1, Quaternion.Euler (0f, _agentConfiguration.smellAngle * Mathf.Rad2Deg, 0f) * angleBaseVector);
		_rightAngleLine.SetPosition (1, Quaternion.Euler (0f, -_agentConfiguration.smellAngle * Mathf.Rad2Deg, 0f) * angleBaseVector);

		// Rescale perception circle to match pheromone perception range.
		_perceptionCircle.transform.localScale = new Vector3(currentPerceptionRange * 2, currentPerceptionRange * 2, 1f);
	}
}
