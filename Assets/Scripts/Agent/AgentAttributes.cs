using UnityEngine;
using System.Collections;

/**
 * Helper class that holds the speed and ID of the agent.
 */
public class AgentAttributes : MonoBehaviour {
	/**
	 * The speed of the agent.
	 */
	public float antSpeed;

	/**
	 * The unique ID of the agent assigned by the AgentManager.
	 */
	public uint ID;

	// Use this for initialization
	void Start () {
		antSpeed = GetComponent<NavAgentHelper> ().GetMovementSpeed();
	}
}
