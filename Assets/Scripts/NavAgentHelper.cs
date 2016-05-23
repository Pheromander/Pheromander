using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class NavAgentHelper : MonoBehaviour {
	// The Rigidbody of the game object.
	//
	// This is used to control the movement by setting
	// velocities to the rigid body.
	Rigidbody _rigidBody;

	// The direction, in which to move.
	public Vector3 direction;

	// Flag to control if the NavAgentHelper stopped or is moving.
	bool _stopped;

	/**
	 * The (maximum) movement speed of the agent.
	 */
	[SerializeField]
	float _movementSpeed = 3.5f;

	/**
	 * The turn speed of the agent, i.e. it's maximum angular velocity.
	 */
	[SerializeField]
	float _turnSpeed = 1.0f;

	/**
	 * Enum for the three states of obstacle avoidance.
	 */
	enum EObstacleAvoidance {
		// No obstacle was touched, so do not avoid anything.
		None,
		// An obstacle was touched on the agent's left-hand side.
		Left,
		// An obstacle was touched on the agent's rigth-hand side.
		Right
	}

	/**
	 * Timer to control obstacle avoidance.
	 */
	float _obstacleAvoidanceTimer = 0.0f;

	/**
	 * The current state of obstacle avoidance.
	 */
	EObstacleAvoidance _obstacleAvoidance;

	/**
	 * The current velocity of the agent.
	 * 
	 * This is calculated per frame and applied to the rigid body
	 * during FixedUpdate().
	 */
	Vector3 _currentVelocity;

	/**
	 * The current angular velocity of the agent.
	 * 
	 * This is calculated per frame and applied to the rigid body during
	 * FixedUpdate().
	 */
	Vector3 _currentAngularVelocity;

	void Awake() {
		_rigidBody = GetComponent<Rigidbody> ();
	}

	/**
	 * Sets the velocities calculated per frame.
	 */
	void FixedUpdate() {
		_rigidBody.velocity = _currentVelocity;
		_rigidBody.angularVelocity = _currentAngularVelocity;

		Vector3 rotation = transform.rotation.eulerAngles;
		rotation.x = rotation.z = 0f;
		transform.rotation = Quaternion.Euler(rotation);
	}

	/**
	 * Update velocities, avoid obstacles or do nothing
	 * if the agent stopped.
	 */
	void Update() {
		if (_stopped) {
			return;
		}

		if (_obstacleAvoidance != EObstacleAvoidance.None) {
			AvoidObstacle ();
		} else {
			StandardMovement ();
		}
	}

	/**
	 * The standard movement, i.e. the movement applied when no
	 * object avoidance is active.
	 */
	void StandardMovement() {
		// Check if target is on the left (negative) or on the right (positive)
		float angleDifference = Vector3.Dot (direction, transform.right);
		angleDifference = angleDifference >  1f ?  1f : angleDifference;
		angleDifference = angleDifference < -1f ? -1f : angleDifference;

		_currentAngularVelocity = angleDifference * _turnSpeed * new Vector3(0f, 1f, 0f);

		_currentVelocity = direction * _movementSpeed;//transform.forward * _movementSpeed;
	}

	/**
	 * Simple obstacle avoidance method.
	 * 
	 * While turning away from the obstacle, move 0.1 seconds backward to resolve
	 * the collision. To avoid further collisions, move foreward for 0.15 seconds
	 * and return back to standard movement.
	 * 
	 * The whole obstacle avoidance needs 0.25 seconds.
	 * 
	 * Note: The magic numbers of 0.1 and 0.25 in this function are determined
	 * manually with the standard speed parameters (see above).
	 */
	void AvoidObstacle() {
		if (_obstacleAvoidanceTimer < 0.1f) {
			_currentVelocity = -transform.forward;
		} else {
			_currentVelocity = transform.forward;
		}
			
		_currentAngularVelocity = transform.up * _turnSpeed;
		if (_obstacleAvoidance == EObstacleAvoidance.Right) {
			_currentAngularVelocity = -_currentAngularVelocity;
		}
	
		if (_obstacleAvoidanceTimer > 0.25f) {
			_obstacleAvoidance = EObstacleAvoidance.None;
		}

		_obstacleAvoidanceTimer += Time.deltaTime;
	}

	// Sets the direction in which to move.
	// The direction is normalized inplicitely.
	public void SetDirection(Vector3 direction) {
		this.direction = new Vector3 (direction.x, 0f, direction.z).normalized;
		_stopped = false;
	}

	/**
	 * Returns the direction to move to.
	 */
	public Vector3 GetDirection() {
		return direction;
	}

	/**
	 * Stop any movement immediately.
	 */
	public void Stop() {
		_currentVelocity = new Vector3 (0f, 0f, 0f);
		_currentAngularVelocity = new Vector3 (0f, 0f, 0f);
		_stopped = true;
	}

	/**
	 * Get the movement speed.
	 */
	public float GetMovementSpeed() {
		return _movementSpeed;
	}

	/**
	 * Handle a collision event.
	 * 
	 * Determine on which side the collision occured to move away accordingly.
	 * Activates object avoidance that is executed in AvoidObstacle().
	 */
	void OnCollisionEnter(Collision collision) {
		Vector3 collisionVector = collision.contacts [0].point - transform.position;
		if (Vector3.Dot (collisionVector, transform.right) > 0) {
			_obstacleAvoidance = EObstacleAvoidance.Right;
		} else {
			_obstacleAvoidance = EObstacleAvoidance.Left;
		}

		_obstacleAvoidanceTimer = 0.0f;
	}
}
