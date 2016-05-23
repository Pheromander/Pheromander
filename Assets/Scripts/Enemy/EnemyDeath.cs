using UnityEngine;
using System.Collections;

// Stuff to execute when the enemy has died.
public class EnemyDeath : MonoBehaviour {

	// This method does stuff that is necessray to be done when
	// the enemy dies.
	//
	// This method is invoked by the Health script which sends a "OnDeath" message
	// to the game object, when healt drops to zero.
	void OnDeath() {
		// Disable the collider so no agents try to attack the dead.
		GetComponent<BoxCollider> ().enabled = false;
	}
}
