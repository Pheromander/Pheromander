using UnityEngine;
using System.Collections;

public class RadioButtonGroup : MonoBehaviour {
	[SerializeField]
	RadioButton[] buttons;

	void Start() {
		// Register this button group to all of it's
		// buttons.
		foreach (RadioButton radioButton in buttons) {
			radioButton.buttonGroup = this;
		}
	}
	
	// Update is called once per frame
	public void OnChildClicked(RadioButton clickedChild) {
		foreach (RadioButton button in buttons) {
			if (button != clickedChild)
				button.Deactivate ();
		}
	}
}
