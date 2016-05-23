using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Sidebar : MonoBehaviour {
	[SerializeField]
	GameObject[] _panels;

	public void ShowPanel(GameObject panelToActivate) {
		foreach (GameObject panel in _panels) {
			if (panel == panelToActivate) {
				panel.SetActive (true);
				panel.SendMessage ("Show", SendMessageOptions.DontRequireReceiver);
			} else {
				panel.SendMessage ("Hide", SendMessageOptions.DontRequireReceiver);
				panel.SetActive (false);
			}
		}
	}
}
