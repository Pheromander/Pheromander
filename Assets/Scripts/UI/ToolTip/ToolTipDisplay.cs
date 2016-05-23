using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolTipDisplay : MonoBehaviour {
	[SerializeField]
	Text _title;

	[SerializeField]
	Text _text;

	[SerializeField]
	GameObject _panel;

	ToolTip _latestModifier;

	public void SetText(ToolTip modifier, string title, string text) {
		_latestModifier = modifier;
		_title.text = title;
		_text.text = text;
		_panel.SetActive (true);
	}

	public void Hide(ToolTip requester) {
		if (_latestModifier == requester) {
			_panel.SetActive(false);
		}
	}
}
