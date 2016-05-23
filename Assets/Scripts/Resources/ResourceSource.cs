using UnityEngine;
using System.Collections;

/**
 * Behavior of a resource.
 */
public class ResourceSource : MonoBehaviour {
	/**
	 * The type of the resource. NOTE: Currently not used.
	 */
	public EResourceTypes resourceType { get; private set; }

	/**
	 * The maximum capacity of the resource.
	 */
	[SerializeField]
	int _maxCapacity = 100;

	/**
	 * The current capacity of the resource.
	 */
	int _currentCapacity;

	void Start() {
		_currentCapacity = _maxCapacity;
	}

	/**
	 * Remove one resource of this spot.
	 * If the resource depletes, it is destroyed.
	 */
	public void DecrementCapacity() {
		--_currentCapacity;
		if(_currentCapacity <= 0) {
			Destroy(this.gameObject);
		}
	}

	public int currentCapacity {
		get {
			return _currentCapacity;
		}
	}

	public int maxCapacity {
		get {
			return _maxCapacity;
		}
	}
}
