using UnityEngine;
using System.Collections;

public class AgentReturnToBase : MonoBehaviour {
	/**
	 * Reference to the agent's state machine.
	 */
	AgentStates state;

	/**
	 * Reference to the NavAgentHelper.
	 */
	NavAgentHelper navHelper;

	/**
	 * Reference to the inventory of the agent.
	 */
	AgentInventory inventory;

	/**
	 * The base of the agent to which it returns.
	 */
	GameObject agentBase;

	/**
	 * The ResourceManager of the scene.
	 */
	ResourceManager _resourceManager;

	// Use this for initialization
	void Awake () {
		navHelper = GetComponent<NavAgentHelper> ();
		state = GetComponent<AgentStates> ();
		inventory = GetComponent<AgentInventory> ();
		agentBase = GameObject.FindWithTag ("AntHill");

		_resourceManager = FindObjectOfType (typeof(ResourceManager)) as ResourceManager;
	}

	/**
	 * Sets the movement direction of the agent to the center of the base.
	 */
	void FixedUpdate () {
		Vector3 homeDirection = agentBase.transform.position - transform.position;
		navHelper.SetDirection (0.9f * navHelper.GetDirection() + 0.01f * homeDirection);
	}

	/**
	 * If the base is hit (collided with), drop resources and change back to
	 * random walk state.
	 */
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "AntHill") {
			_resourceManager.AddResources(inventory.carryCount);
			inventory.Clear();

			// Make agent rotate 180 degrees.
			if (state.IsReturningToBase ()) {
				navHelper.SetDirection (-navHelper.GetDirection ());
				transform.rotation = transform.rotation * Quaternion.AngleAxis (180f, transform.up);
			}

			state.currentState = EAntStates.Idle;
		}
	}
}
