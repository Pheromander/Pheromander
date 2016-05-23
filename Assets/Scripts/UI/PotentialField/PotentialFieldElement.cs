using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PotentialFieldElement : MonoBehaviour {
	public EPheromoneTypes pheromoneType { get; set; }

	HashSet<Pheromone> _nearbyPheromones;

	GameObject _display;

	float _pruningTimer = 0f;

	// Use this for initialization
	void Awake () {
		_nearbyPheromones = new HashSet<Pheromone> ();
		_display = transform.FindChild ("Display").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateDisplay ();

		// Prune deleted pheromones from time to time
		_pruningTimer += Time.deltaTime;
		if (_pruningTimer > 3.0f) {
			_nearbyPheromones.RemoveWhere (ShouldBePruned);
		}
	}

	void UpdateDisplay() {
		bool displayActive = false;

		Vector3 newDirection = new Vector3 ();

		float sign = 1f;
		if (pheromoneType == EPheromoneTypes.Repellant) {
			sign = -1f;
		}

		// Aggregate pheromones to calculate direction
		foreach (Pheromone pheromone in _nearbyPheromones) {
			if (!pheromone)
				continue;

			if (pheromone.type != pheromoneType)
				continue;

			displayActive = true;
			newDirection += (pheromone.transform.position - transform.position) * pheromone.currentConcentration;	
		}

		// Deactivate display, if no pheromones of type pheromoneType are nearby.
		if (!displayActive) {
			_display.SetActive (false);
			return;
		}
		_display.SetActive (true);

		// Fix newDirection vector
		newDirection.y = 0f;
		newDirection.z *= -1;

		newDirection *= sign;

		// Set new rotation of display
		float rotation = MathHelper.AngleSigned (newDirection, new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f)) *  Mathf.Rad2Deg;
		_display.transform.rotation = Quaternion.Euler (90f,  rotation + 90f, 0f);
	}

	void OnTriggerEnter(Collider collider) {
		Pheromone pheromone = collider.transform.parent.GetComponent<Pheromone> ();

		if(pheromone) {
			_nearbyPheromones.Add (pheromone);
		}
	}

	void OnTriggerExit(Collider collider) {
		Pheromone pheromone = collider.transform.parent.GetComponent<Pheromone> ();

		if (pheromone) {
			_nearbyPheromones.Remove (pheromone);
		}
	}

	bool ShouldBePruned(Pheromone pheromone) {
		if (pheromone) {
			return false;
		}
		return true;
	}

	public void Clear() {
		_nearbyPheromones.Clear ();
	}
}
