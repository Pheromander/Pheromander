using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadioButton : MonoBehaviour {
	public bool active { get; private set; }

	// The RadioButtonGroup which this RadioButton belongs to.
	// This is set by the according RadioButtonGroup. There is
	// no need to register the ButtonGroup manually.
	public RadioButtonGroup buttonGroup { private get; set; }

	Image image;
	RadioButtonGroup group;

	// Use this for initialization
	void Awake () {
		active = false;
		image = GetComponent<Image> ();
	}

	void Update() {
		if (active && Input.GetMouseButtonUp (1)) {
			Deactivate ();
		}
	}

	public void Toggle() {
		if (!active) {
			buttonGroup.OnChildClicked (this);
			active = true;
			UpdateColor ();
		} else {
			Deactivate ();
		}
	}

	public void Deactivate() {
		active = false;
		UpdateColor ();
	}

	void UpdateColor() {
		if (active)
			image.color = Color.gray;
		else
			image.color = Color.white;
	}
}
