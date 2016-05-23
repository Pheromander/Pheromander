using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopBar : MonoBehaviour {
	/**
	 * The label on which the number of resources is displayed.
	 */
	Text _resourceValue;

	/**
	 * The label on which the number of agents is displayed.
	 */
	Text _agentsValue;

	/**
	 * The AgentManager of the scene.
	 */
	AgentManager _agentManager;

	/**
	 * The ResourceManager of the scene.
	 */
	ResourceManager _resourceManager;

	void Awake() {
		_resourceValue = transform.Find ("FoodValue").GetComponent<Text> ();
		_agentsValue = transform.Find ("AgentsValue").GetComponent <Text>();

		_agentManager = FindObjectOfType (typeof(AgentManager)) as AgentManager;
		_resourceManager = FindObjectOfType (typeof(ResourceManager)) as ResourceManager;
	}

	// Update is called once per frame
	void Update () {
		_resourceValue.text = _resourceManager.GetResourceCount().ToString ();
		_agentsValue.text = _agentManager.currentAgentCount.ToString();
	}
}
