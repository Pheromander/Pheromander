using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceStatusDisplay : MonoBehaviour {
	GameObject _resource;

	[SerializeField]
	DeleteGameObjectButton _deleteGameObjectButton;

	Text _currentCapacityValue;
	Text _maxCapacityValue;

	void Awake() {
		_currentCapacityValue = transform.Find ("CurrentCapacityValue").GetComponent<Text> ();
		_maxCapacityValue = transform.Find ("MaxCapacityValue").GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		if (!_resource) {
			BroadcastMessage ("Hide", this, SendMessageOptions.DontRequireReceiver);
			gameObject.SetActive (false);
			return;
		}

		ResourceSource resourceSource = _resource.GetComponent<ResourceSource> ();
		_currentCapacityValue.text = resourceSource.currentCapacity.ToString ();
		_maxCapacityValue.text = resourceSource.maxCapacity.ToString ();
	}

	public void Show() {
		_resource.GetComponent<ResourceSelectionMarker> ().Show ();
	}

	public void SetResource(GameObject resource) {
		if (_resource) {
			_resource.GetComponent<ResourceSelectionMarker> ().Hide ();
		}
		_resource = resource;
		_deleteGameObjectButton.objectToDelete = _resource;
	}

	public void Hide() {
		if (_resource) {
			_resource.GetComponent<ResourceSelectionMarker> ().Hide ();
		}
	}
}
