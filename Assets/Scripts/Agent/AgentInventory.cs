using UnityEngine;
using System.Collections;

/**
 * Simple inventory class to store how many resources
 * an agent is currently carrying.
 */
public class AgentInventory : MonoBehaviour {
	/**
	 * The maximum capacity of resources the agent can carry.
	 */
	public int capacity = 5;

	/**
	 * The number of resources that are currently carried by the agent
	 * 
	 * This number is inside of [0, capacity].
	 */
	public uint carryCount {
		get {
			return _carryCount;
		}
		set {
			_carryCount = value;
		}
	}

	/**
	 * The type of resources carried at the moment.
	 * 
	 * NOTE: This is noto used at the moment!
	 */
	public EResourceTypes resourceType {
		get {
			return _resourceType;
		}
		set {
			_resourceType = value;
		}
	}

	/**
	 * The number of resources that are currently carried by the agent.
	 * See carryCount.
	 */
	uint _carryCount = 0;

	/**
	 * The type of resource carried at the moment.
	 * NOTE: At the moment, this is not used!
	 */
	EResourceTypes _resourceType;

	/**
	 * Returns true if the inventory is full.
	 */
	public bool IsFull() {
		return carryCount == capacity;
	}

	/**
	 * Add one unit of resoruces to the inventory.
	 */
	public void IncrementCapacity() {
		++carryCount;
	}

	/**
	 * Remove all resources from the inventory.
	 */
	public void Clear() {
		carryCount = 0;
	}
}
