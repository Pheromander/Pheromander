using UnityEngine;
using System.Collections;

// Helper class that activates the PotentialField.
// For example, this script can be added to a button to enable
// the potential field when the button is clicked.
public class TogglePotentialField : MonoBehaviour {
	PotentialField _potentialField;

	[SerializeField]
	EPheromoneTypes _pheromoneType;

	// Use this for initialization
	void Awake () {
		_potentialField = FindObjectOfType (typeof(PotentialField)) as PotentialField;
	}
	
	public void Toggle() {
		if(!_potentialField.IsActiveWith(_pheromoneType)) {
			_potentialField.Activate(_pheromoneType);
		} else {
			_potentialField.Deactivate();
		}
	}


}
