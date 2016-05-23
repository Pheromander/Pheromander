using UnityEngine;
using System.Collections;

public class PotentialField : MonoBehaviour {
	int _groundLayer;

	EPheromoneTypes _pheromoneType;

	// Use this for initialization
	void Start () {
		_groundLayer = LayerMask.GetMask ("Ground");
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		// If right mouse button was pressed, disable itself
		if (Input.GetMouseButtonUp (1)) {
			Deactivate();
			return;
		}

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit raycastHit;

		if (!Physics.Raycast (camRay, out raycastHit, 1000.0f, _groundLayer)) {
			return;
		}

		transform.position = raycastHit.point;
	}

	public void Activate(EPheromoneTypes pheromoneType) {
		gameObject.SetActive (true);

		_pheromoneType = pheromoneType;

		foreach (Transform row in transform) {
			foreach (Transform child in row) {
				PotentialFieldElement element = child.GetComponent<PotentialFieldElement> ();

				if (element) {
					element.pheromoneType = _pheromoneType;
				}
			}
		}
	}

	public void Deactivate() {
		foreach (Transform row in transform) {
			foreach (Transform child in row) {
				PotentialFieldElement element = child.GetComponent<PotentialFieldElement> ();

				if (element) {
					element.Clear ();
				}
			}
		}

		gameObject.SetActive (false);
	}

	public bool IsActiveWith(EPheromoneTypes pheromoneType) {
		return gameObject.activeInHierarchy && _pheromoneType == pheromoneType;
	}
}
