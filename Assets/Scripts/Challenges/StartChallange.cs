using UnityEngine;
using System.Collections;

public class StartChallange : MonoBehaviour {
	// AgentManager which is disabled at first and enabled
	// when the challenge starts.
	AgentManager _agentManager;

	// The ingame gui that is hidden at first and displayed
	// when the challenge starts.
	[SerializeField]
	GameObject _ingameGui;

	// Use this for initialization
	void Awake () {
		_agentManager = FindObjectOfType (typeof(AgentManager)) as AgentManager;
		_agentManager.enabled = false;

		_ingameGui.SetActive (false);
	}
	
	// Update is called once per frame
	public void StartChallenge() {
		_agentManager.enabled = true;

		_ingameGui.SetActive (true);
	}
}
