using UnityEngine;
using System.Collections;

[RequireComponent (typeof(RadioButton))]
public class EnablePheromonePlacer : MonoBehaviour {
	EnvironmentInteraction _environmentInteraction;

	[SerializeField]
	EPheromoneTypes _pheromoneType;

	RadioButton _radioButton;

	void Awake() {
		_environmentInteraction = FindObjectOfType (typeof(EnvironmentInteraction)) as EnvironmentInteraction;
		_radioButton = GetComponent<RadioButton> ();
	}

	public void OnClick() {
		if (_radioButton.active) {
			_environmentInteraction.pheromoneType = _pheromoneType;
			_environmentInteraction.Activate (EnvironmentInteraction.EInteractionType.EEI_PLACE_PHEROMONES);
		} else  {
			// If the EnvironmentInteraction is still active with matching pheromone type
			// deactivate it as well.
			if (_environmentInteraction.currentInteraction == EnvironmentInteraction.EInteractionType.EEI_PLACE_PHEROMONES 
		        && _environmentInteraction.pheromoneType == _pheromoneType) {
				_environmentInteraction.Deactivate (EnvironmentInteraction.EInteractionType.EEI_PLACE_PHEROMONES);
			}
		}
	}
}
