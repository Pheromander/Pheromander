using UnityEngine;
using System.Collections;

/**
 * Display selection markers of the enemy, i.e. a red circle indicating it's attack range.
 */
public class EnemySelectionMarker : MonoBehaviour {
	/**
	 * Reference to the game object storing the selection markers.
	 */
	[SerializeField]
	GameObject _selectionMarkers;

	/**
	 * Direct reference to the attack range circle (red) which is scaled to
	 * match the attack range of the enemy.
	 */
	[SerializeField]
	GameObject _attackCircle;

	/**
	 * Scale _attackCircle to match the enemie's attack range.
	 */
	void Start() {
		float attackRadius = GetComponent<Attack> ().GetAttackRadius () * 2;
		_attackCircle.transform.localScale = new Vector3 (attackRadius, attackRadius, 1f);
	}

	/**
	 * Display selection markers.
	 */
	public void Show() {
		_selectionMarkers.SetActive (true);
	}

	/**
	 * Hide selection markers.
	 */
	public void Hide() {
		_selectionMarkers.SetActive (false);
	}
}
