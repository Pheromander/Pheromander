using UnityEngine;
using System.Collections;

// Helper script that can be attached to e.g. a button.
// When the button is clicked, the sidebar is
// notified to open up a panel defined in the Unity editor.
public class RevealSidebarPanel : MonoBehaviour {

	// Reference to the sidebar.
	[SerializeField]
	Sidebar _sidebar;

	// Reference to the panel of the sidebar that should
	// be opened.
	[SerializeField]
	GameObject _panel;

	// Requests the sidebar to open the panel.
	public void OnClick() {
		_sidebar.ShowPanel (_panel);
	}
}
