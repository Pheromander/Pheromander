using UnityEngine;
using System.Collections;

/**
 * Script defining the random walk behavior of agents.
 */
public class AgentRandomWalk : MonoBehaviour {
	/**
	 * The current direction is modified by a random angle. This variable
	 * defines the upper bound of this random angle.
	 */
	public float idleRandomAngle = 60.0f;

	/**
	 * The interval in which the current movement direction is randomized.
	 */
	public float idleLocationChangeInterval = 0.5f;

	/**
	 * Timer to measure if current movement direction should be influenced by
	 * random factor.
	 */
	float idleLocationChangeTimer = 0.0f;

	NavAgentHelper navHelper;

	void Awake() {
		navHelper = GetComponent<NavAgentHelper> ();
	}

	void Start() {
		idleLocationChangeTimer = idleLocationChangeInterval;
	}

	void Update() {
		idleLocationChangeTimer += Time.deltaTime;

		if(idleLocationChangeTimer >= idleLocationChangeInterval) {
			// Determine a random angle.
			float randomAngle = Random.Range (-idleRandomAngle, idleRandomAngle);

			// Rotate current direction by this angle.
			Vector3 randomDirection = Quaternion.Euler (0, randomAngle, 0) * transform.forward;
			navHelper.SetDirection (randomDirection);

			idleLocationChangeTimer = 0.0f;
		}
	}
}
