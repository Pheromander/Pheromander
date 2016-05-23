using UnityEngine;
using System.Collections;

/**
 * A sensor component that notifies an Attack component about
 * possible targets that entered, or exited a collider.
 */
public class EnemySensor : MonoBehaviour {
	/**
	 * Reference to the Attack component which is notified
	 * about enemy enter/exit events
	 */
	[SerializeField]
	Attack _attack;

	/**
	 * When a collider entered, notify the Attack component.
	 */
	void OnTriggerEnter(Collider collider) {
		_attack.AcquireNewTarget(collider.gameObject);
	}

	/**
	 * When a collider stays, notify the Attack component.
	 * 
	 * This is necessary in the event of a previous target being killed.
	 * When only OnTriggerEnter() is handled, the Attack component could not
	 * acquire a target that still resides inside the collider.
	 */
	void OnTriggerStay(Collider collider) {
		_attack.AcquireNewTarget (collider.gameObject);
	}

	/**
	 * When a collider exits, notify the Attack component.
	 */
	void OnTriggerExit(Collider collider) {
		_attack.LoseTarget (collider.gameObject);
	}
}
