using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeleteGameObjectConfirmationDialog : MonoBehaviour {

	GameObject _objectToDelete;

	[SerializeField]
	Text _text;

	// Update is called once per frame
	public void OnDeletionConfirmed () {
		if (_objectToDelete) {
			Destroy (_objectToDelete);
			_objectToDelete = null;
		}
	}

	public void Activate(GameObject gameObject, string text) {
		_objectToDelete = gameObject;
		_text.text = text;
		this.gameObject.SetActive (true);
	}
}
