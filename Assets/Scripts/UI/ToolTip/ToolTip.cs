using UnityEngine;
using System.Collections;

public class ToolTip : MonoBehaviour {
	[SerializeField]
	string _toolTipTitle = "Tool Tip Title";

	[SerializeField]
	[TextArea(5, 5)]
	string _toolTipText = "Some tool tip explaination.";

	ToolTipDisplay _toolTipDisplay;

	public string toolTipText {
		get {
			return _toolTipText;
		}
		set {
			_toolTipText = value;
		}
	}

	void Awake() {
		_toolTipDisplay = FindObjectOfType (typeof(ToolTipDisplay)) as ToolTipDisplay;
	}

	public void OnMouseEnter() {
		_toolTipDisplay.SetText (this, _toolTipTitle, _toolTipText);
	}

	public void OnMouseExit() {
		_toolTipDisplay.Hide (this);
	}

	void Hide() {
		OnMouseExit ();
	}
}
