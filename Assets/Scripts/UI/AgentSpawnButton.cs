using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Image))]
[RequireComponent (typeof(Button))]
[RequireComponent (typeof(ToolTip))]
public class AgentSpawnButton : MonoBehaviour {

	/**
	 * The number of agents that are spawned when clicked.
	 */
	[SerializeField]
	uint _agentCount = 1;

	/**
	 * The number of resources spawning _one_ agent costs.
	 */
	[SerializeField]
	uint _costsPerAgent = 5;

	/**
	 * The total amount of resources needed to spawn _agentCount number of agents.
	 */
	uint _totalCosts;

	/**
	 * The image of the button.
	 */
	Image _image;

	/**
	 * The button.
	 */
	Button _button;

	/**
	 * The AgentManager of the scene.
	 */
	AgentManager _agentManager;

	/**
	 * The ResourceManager of the scene.
	 */
	ResourceManager _resourceManager;

	void Awake() {
		_image = GetComponent<Image> ();
		_button = GetComponent<Button> ();

		_agentManager = FindObjectOfType (typeof(AgentManager)) as AgentManager;
		_resourceManager = FindObjectOfType (typeof(ResourceManager)) as ResourceManager;
	}

	void Start() {
		_totalCosts = _agentCount * _costsPerAgent;

		if (_totalCosts > 0) {
			GetComponent<ToolTip> ().toolTipText = "Total resource costs: " + _totalCosts;
		} else {
			GetComponent<ToolTip> ().toolTipText = "Spawn agents for free.";
		}
	}

	/**
	 * Enable/disable button according to available resources and _totalCosts.
	 */
	void Update () {
		if (_resourceManager.GetResourceCount() >= _totalCosts) {
			_image.color = Color.white;
			_button.enabled = true;
		} else {
			_image.color = Color.gray;
			_button.enabled = false;
		}
	}

	public void SpawnAgents() {
		if (_resourceManager.GetResourceCount() >= _totalCosts) {
			_resourceManager.RemoveResources (_totalCosts);
			_agentManager.IncreasePopulation (_agentCount);
		}
	}
}
