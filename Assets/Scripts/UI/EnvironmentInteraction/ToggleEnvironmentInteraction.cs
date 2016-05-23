using UnityEngine;
using System.Collections;

[RequireComponent (typeof(RadioButton))]
public class ToggleEnvironmentInteraction : MonoBehaviour {
	[SerializeField]
	EnvironmentInteraction.EInteractionType _interactionType;

	RadioButton _radioButton;

	EnvironmentInteraction _environmentInteraction;

	void Awake() {
		_radioButton = GetComponent<RadioButton> ();

		_environmentInteraction = FindObjectOfType (typeof (EnvironmentInteraction)) as EnvironmentInteraction;
	}

	public void Toggle() {
		if (_radioButton.active) {
			_environmentInteraction.Activate (_interactionType);
		} else {
			_environmentInteraction.Deactivate (_interactionType);
		}
	}
}
