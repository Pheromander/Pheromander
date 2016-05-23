using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Pheromone manager controls the placement of pheromones on the map.
 * 
 * This is only required since we merge pheromones placed on the same spot. 
 * If merging is disabled or handeled by pheromones themselves, this class will
 * no longer be required. (Future work)
 */
[RequireComponent (typeof(GlobalPheromoneConfiguration))]
public class PheromoneManager : MonoBehaviour {
	/**
	 * Referencew to the pheromone prefab.
	 */
	[SerializeField]
	GameObject _pheromonePrefab;

	/**
	 * Used to mask the origins of pheromones.
	 */
	int _pheromoneOriginLayerMask;

	/**
	 * Used to mask pheromones.
	 */
	int _pheromoneLayerMask;

	/**
	 * Reference to the pheromone configuration.
	 */
	GlobalPheromoneConfiguration _pheromoneConfiguration;

	void Awake() {
		_pheromoneOriginLayerMask = LayerMask.GetMask ("PheromoneOrigin");
		_pheromoneLayerMask = LayerMask.GetMask ("Pheromone");

		_pheromoneConfiguration = GetComponent<GlobalPheromoneConfiguration> ();
	}

	/**
	 * Place a pheromone of given type at given position.
	 * 
	 * If necessary, merge it with existing pheromones. A merge is triggered if more than
	 * 4 pheromones are located at the same position, i.e. their centers lie within a circle of 0.5 units.
	 */
	public void PlacePheromone(Vector3 position, EPheromoneTypes type) {
		position.y = 0;

		PheromoneConfiguration pheromoneConfiguration = _pheromoneConfiguration.configs [type];

		// Get current parameters from configuration.
		float initialIntensity = pheromoneConfiguration.initialIntensity;
		float initialScale = pheromoneConfiguration.initialRadius;
		float diffusionRate = pheromoneConfiguration.diffusionRate;
		float minimumConcentration = pheromoneConfiguration.minimumConcentration;
		Color color = pheromoneConfiguration.color;

		// If there are enough pheromones nearby, merge them into one
		Collider[] nearbyPheromoneCenters = Physics.OverlapSphere (position, 0.5f, _pheromoneOriginLayerMask);
		List<GameObject> nearbyPheromones = new List<GameObject> ();

		// Filter nearby pheromones by type.
		foreach(Collider pheromoneCenter in nearbyPheromoneCenters)  {
			GameObject pheromone = pheromoneCenter.transform.parent.gameObject;

			if (pheromone.GetComponent<Pheromone> ().type == type) {
				nearbyPheromones.Add (pheromone);
			}
		}

		// Merge if enough pheromones of the same type are located on position.
		if (nearbyPheromones.Count > 4) {
			foreach(GameObject pheromone in nearbyPheromones) {
				initialIntensity += pheromone.GetComponent<Pheromone>().startIntensity;

				initialScale = Mathf.Max(initialScale, pheromone.transform.Find ("DiffusionSphere").transform.localScale.x);
				//startScale += pheromone.transform.Find ("DiffusionSphere").transform.localScale.x;

				position += pheromone.transform.position;

				Destroy (pheromone);
			}

			// Make start scale the average of all scales
			//startScale = startScale / (nearbyPheromoneCenters.GetLength(0) + 1);
			position = position / (nearbyPheromones.Count + 1);
		}

		// Create and initialize new pheromone
		GameObject newPheromone = Instantiate (_pheromonePrefab, position, new Quaternion ()) as GameObject;
		newPheromone.GetComponent<Pheromone>().Initialize (type, initialIntensity, initialScale, diffusionRate, minimumConcentration, color);
	}

	/**
	 * Erase pheromones that overlap with given position.
	 */
	public void ErasePheromone(Vector3 position) {
		position.y = 0;
		Collider[] nearbyPheromoneCenters = Physics.OverlapSphere (position, 1.0f, _pheromoneLayerMask);

		Pheromone pheromone;
		foreach(Collider collider in nearbyPheromoneCenters) {
			pheromone = collider.GetComponentInParent<Pheromone>();
			Destroy (pheromone.gameObject);
		}
	}
}
		