using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	/**
	 * The initial number of hit points.
	 * This value can never be exceeded by _currentHealth.
	 */
	[SerializeField]
	float _initialHealth = 100.0f;

	/**
	 * The current number of hit points.
	 * If _currentHealth drops to or below zero, the
	 * related game object is destroyed.
	 */
	float _currentHealth;

	/**
	 * The rate in which health is regenerated in hit points per second.
	 */
	[SerializeField]
	float _regenerationRate = 0f;

	/**
	 * Reference to an animator. When set, the death animation is triggered when
	 * health drops to zero.
	 */
	[SerializeField]
	Animator _animator;

	/**
	 * A GameObject that is notified if this Health instance
	 * destroys the game object it's attached to.
	 * I.e.: When health drops to zero or below, SendMessage("OnDeath", this)
	 * is executed on notifyOnDeath.
	 */
	public GameObject notifyOnDeath;

	void Start () {
		_currentHealth = _initialHealth;
	}
	
	/**
	 * Applies health regeneration and handles the event of death.
	 * 
	 * It sends two messages to its GameObject: "OnDeath" when the object was killed
	 * and "OnDamage" when damage was inflicted on the GameObject.
	 */
	void Update () {
		if (_currentHealth < _initialHealth) {
			_currentHealth += Time.deltaTime * _regenerationRate;
		}

		if (_currentHealth <= 0) {
			// If an animator is attached, play the Death animation.
			if (_animator) {
				_animator.SetTrigger ("Die");
			}

			// Notify registered game object about my death.
			if (notifyOnDeath) {
				notifyOnDeath.SendMessage ("OnDeath", this, SendMessageOptions.DontRequireReceiver);
			}

			// Notify componets of myself about my death.
			this.gameObject.SendMessage("OnDeath", this, SendMessageOptions.DontRequireReceiver);

			// Destroy me later.
			Invoke("Destroy", 1.0f);

			// Disable this script to prevent health regenration and multiple
			// calls to the death animation and death notifications.
			this.enabled = false;
		}
	}

	/**
	 * Take "damage" amount of damage (*cough*).
	 */
	public void TakeDamage(float damage) {
		_currentHealth -= damage;

		gameObject.SendMessage ("OnDamaged", damage);
	}

	/**
	 * Destroy the game object.
	 * 
	 * Used to trigger a delayed destruction of the object on death.
	 */
	void Destroy() {
		Destroy(this.gameObject);
	}

	/**
	 * Get the initial (maximum) number of HPs.
	 */
	public float GetInitialHealth() {
		return _initialHealth;
	}

	/**
	 * Returns the current HPs.
	 */
	public float GetCurrentHealth() {
		return _currentHealth;
	}

	/**
	 * Get the regeneration rate.
	 */
	public float GetRegenerationRate() {
		return _regenerationRate;
	}
}
