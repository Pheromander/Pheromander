using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyStatusDisplay : MonoBehaviour {
	GameObject _enemy;

	Text _HPValue;
	Text _regenerationValue;
	Text _damageValue;

	[SerializeField]
	DeleteGameObjectButton _deleteGameObjectButton;

	// Use this for initialization
	void Awake () {
		_HPValue = transform.Find ("HPValue").GetComponent<Text> ();
		_regenerationValue = transform.Find ("RegenerationValue").GetComponent<Text> ();
		_damageValue = transform.Find ("DamageValue").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!_enemy) {
			BroadcastMessage ("Hide", this, SendMessageOptions.DontRequireReceiver);
			gameObject.SetActive (false);
			return;
		}

		Health health = _enemy.GetComponent<Health> ();
		_HPValue.text = ((int)health.GetCurrentHealth ()) + "/" + health.GetInitialHealth ();
		_regenerationValue.text = health.GetRegenerationRate ().ToString ();

		Attack attack = _enemy.GetComponent<Attack> ();
		_damageValue.text = attack.GetDamage ().ToString ();
	}

	public void Show() {
		_enemy.GetComponent<EnemySelectionMarker> ().Show ();
	}

	public void SetEnemy(GameObject enemy) {
		if(_enemy) {
			_enemy.GetComponent<EnemySelectionMarker> ().Hide ();
		}

		_enemy = enemy;
		_deleteGameObjectButton.objectToDelete = _enemy;
	}

	public void Hide() {
		if (_enemy) {
			_enemy.GetComponent<EnemySelectionMarker> ().Hide ();
		}
	}

}
