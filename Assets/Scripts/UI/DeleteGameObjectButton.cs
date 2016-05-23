using UnityEngine;
using System.Collections;

public class DeleteGameObjectButton : MonoBehaviour {

	public GameObject objectToDelete;

	[SerializeField]
	DeleteGameObjectConfirmationDialog _confirmationDialog;

	[SerializeField]
	[TextArea(5, 5)]
	string text;

	// Use this for initialization
	void  Awake () {
		
	}
	
	// Update is called once per frame
	public void OnClick () {
		_confirmationDialog.Activate (objectToDelete, text);
	}
}
