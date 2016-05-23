using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AntStatusDisplay : MonoBehaviour {
	[SerializeField]
	Text gameIDVal;

	[SerializeField]
	Text taskVal;

	[SerializeField]
	Text statusVal;

	[SerializeField]
	Text hpVal;

	[SerializeField]
	Text capacityVal;

	[SerializeField]
	Text attackRangeVal;

	[SerializeField]
	Text damageVal;

	[SerializeField]
	SetCameraFocus _setCameraFocus;

	private GameObject _agent;

	void Update() {
		// Hide if the agent died.
		if (!_agent) {
			BroadcastMessage ("Hide", this, SendMessageOptions.DontRequireReceiver);
			gameObject.SetActive (false);
			return;
		}

		AgentAttributes attributes = _agent.GetComponent<AgentAttributes> ();
		gameIDVal.text = attributes.ID.ToString ();

		AgentInventory inventory = _agent.GetComponent<AgentInventory> ();
		capacityVal.text = inventory.carryCount.ToString () + " / " + inventory.capacity.ToString ();

		Health health = _agent.GetComponent<Health> ();
		hpVal.text = health.GetCurrentHealth () + " / " + health.GetInitialHealth ();

		statusVal.text = _agent.GetComponent<AgentStates> ().GetStatusString ();
		taskVal.text = _agent.GetComponent<AgentStates> ().GetTaskString ();

		Attack attack = _agent.GetComponent<Attack> ();
		attackRangeVal.text = attack.GetAttackRadius ().ToString ();
		damageVal.text = attack.GetDamage ().ToString ();
	}

	public void Show() {
		_agent.GetComponent<AgentSelectionMarker> ().Show ();
	}

	public void Hide() {
		if (_agent) {
			_agent.GetComponent<AgentSelectionMarker> ().Hide ();
		}
		_setCameraFocus.UnlockFocus ();
	}

	public void SetAgent(GameObject agent) {
		if (_agent) {
			_agent.GetComponent<AgentSelectionMarker> ().Hide ();
		}

		_agent = agent;

		_setCameraFocus.focusObject = _agent.transform;
	}

	public GameObject GetAgent() {
		return _agent;
	}
}

