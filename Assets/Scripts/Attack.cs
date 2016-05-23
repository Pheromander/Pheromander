using UnityEngine;
using System.Collections;

/**
 * Component that handles attacking an enemy.
 * 
 * It is able to obtain and lose targets as well as
 * attacking the target.
 * 
 * It collaborates tightly with the Health compoonent.
 */
public class Attack : MonoBehaviour {
	/**
	 * The tag of the enemy game object.
	 * 
	 * Only objects with this tag are considered hostile.
	 */
	[SerializeField]
	string _enemyTag;

	/**
	 * The number of hit points of damage one attack
	 * inflicts on the enemy.
	 */
	[SerializeField]
	float _damage = 5.0f;

	/**
	 * The attack rate as the time that has to pass
	 * between to shots.
	 */
	[SerializeField]
	float _attackRate = 1.0f;

	/**
	 * The maximum attack range. Everything beyond this range
	 * cannot be attacked.
	 */
	[SerializeField]
	float _attackRadius = 3.5f;

	/**
	 * Reference to the collider used to detect enemies.
	 * 
	 * This is scaled automatically to match the _attackRadius.
	 */
	[SerializeField]
	SphereCollider _rangeCollider;

	/**
	 * Line renderer used to render laser beams.
	 */
	[SerializeField]
	LineRenderer _laserBeam;

	/**
	 * A light source used as graphical effect when the
	 * laser beam is fired.
	 */
	[SerializeField]
	Light _laserLight;

	/**
	 * The offset in y-direction of the beginning of the laser beam.
	 */
	[SerializeField]
	float _laserYOffset = 0.5f;

	/**
	 * A reference to the Health component of the current target.
	 */
	Health _currentTarget;

	/**
	 * Attack timer to measure if enough time has passed between
	 * two shots.
	 */
	float _attackTimer = 0f;

	// Use this for initialization
	void Start () {
		_rangeCollider.radius = _attackRadius;
	}
	
	/**
	 * When a target is set, attack it and wait for the attack cooldown.
	 * 
	 * When attacking, the graphical effects (laser beam and light) is handled.
	 */
	void Update () {
		// Always disable laser beam and light.
		_laserBeam.enabled = false;
		_laserLight.enabled = false;

		// If no valid target, skip the rest of the function.
		if (_currentTarget && _currentTarget.GetCurrentHealth () <= 0f) {
			_currentTarget = null;
			return;
		}

		_attackTimer += Time.deltaTime;

		if (_attackTimer > _attackRate && HasTarget ()) {
			_currentTarget.TakeDamage(_damage);
			DrawLaserBeam();
			_laserLight.enabled = true;
			_attackTimer = 0f;
		}
	}

	/**
	 * Draws the laser beam when fired.
	 */
	void DrawLaserBeam() {
		_laserBeam.SetPosition (0, transform.position + new Vector3(0f, _laserYOffset, 0f));
		_laserBeam.SetPosition (1, _currentTarget.transform.position);
		_laserBeam.enabled = true;
	}

	/**
	 * Called by a sensor script (EnemySensor / AgentEnemySensor) to
	 * notify the Attack component about a possible target.
	 */
	public void AcquireNewTarget(GameObject target) {
		if (!HasTarget() && target.CompareTag(_enemyTag)) {
			_currentTarget = target.GetComponentInParent<Health>();
		}
	}

	/**
	 * Called by a sensor script (EnemySensor / AgentEnemySensor) to notify
	 * the Attack component about a target that moved out of _attackRadius.
	 */
	public void LoseTarget(GameObject target) {
		if(target.CompareTag(_enemyTag)) {
			Health _exitedHealth = target.GetComponent<Health>();
			
			if(_currentTarget == _exitedHealth) {
				_currentTarget = null;
			}
		}
	}

	/**
	 * Method that checks whether the Attack component has a target or not.
	 */
	public bool HasTarget() {
		if (_currentTarget)
			return true;
		return false;
	}

	/**
	 * Get the maximum attack range.
	 */
	public float GetAttackRadius() {
		return _attackRadius;
	}

	/**
	 * Get the damage inflicted per hit.
	 */
	public float GetDamage() {
		return _damage;
	}

	/**
	 * Returns the current target GameObject.
	 */
	public GameObject GetCurrentTarget() {
		if (_currentTarget) {
			return _currentTarget.gameObject;
		}
		return null;
	}

	/**
	 * Cleanup on death to avoid glitches.
	 * 
	 * This is called by the Health component that Broadcasts a "OnDeath"
	 * message when health drops to zero.
	 */
	void OnDeath() {
		this.enabled = false;
		_currentTarget = null;
	}
}
