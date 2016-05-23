using UnityEngine;
using System.Collections;

/**
 * Display a simple selection marker for resources.
 */
public class ResourceSelectionMarker : MonoBehaviour {

	[SerializeField]
	GameObject _selectionMarkers;

	public void Show() {
		_selectionMarkers.SetActive(true);
	}

	public void Hide() {
		_selectionMarkers.SetActive(false);
	}
}
