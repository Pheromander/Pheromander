using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EnvironmentInteraction : MonoBehaviour {
	public enum EInteractionType {
		EEI_NONE,
		EEI_PLACE_PHEROMONES,
		EEI_PLACE_ENEMIES,
		EEI_PLACE_RESOURCES
	}

	public Vector3 position;

	public float pheromonePlacementSpeed = 0.1f;

	PheromoneManager _pheromoneManager;

    StatisticsManager _statisticsManager;

	public EPheromoneTypes pheromoneType { get; set; }

	public EInteractionType currentInteraction { get; private set; }

	float placementTimer = 0.0f;

	int groundLayerMask;
	int agentLayerMask;

	[SerializeField]
	GameObject _cursor;

	[SerializeField]
	GameObject _enemyPrefab;

	[SerializeField]
	GameObject _resourcePrefab;

	void Awake() {
		_pheromoneManager = (PheromoneManager) Object.FindObjectOfType(typeof(PheromoneManager));
        _statisticsManager = (StatisticsManager) Object.FindObjectOfType(typeof(StatisticsManager));
	}

	void Start() {
		groundLayerMask = LayerMask.GetMask ("Ground");
		agentLayerMask = LayerMask.GetMask ("Ant");
	}

	// Update is called once per frame
	void LateUpdate () {
		placementTimer += Time.deltaTime;

		// If right mouse button was pressed, disable itself
		if (Input.GetMouseButtonUp (1)) {
			Deactivate(currentInteraction);
			return;
		}

		// Only do stuff if no UI element is targeted
		if(EventSystem.current.IsPointerOverGameObject())
			return;

		// Get cursor position on map.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit groundHit;
		if (!Physics.Raycast (camRay, out groundHit, 1000.0f, groundLayerMask)) {
			return;
		}
			
		// Update cursor position
		_cursor.transform.position = groundHit.point;

		// Perform actual interaction
		switch (currentInteraction) {
		case EInteractionType.EEI_PLACE_PHEROMONES:
			DoPlacePheromoneInteraction (groundHit.point);
			break;
		case EInteractionType.EEI_PLACE_ENEMIES:
			DoPlaceEnemiesInteraction (groundHit.point);
			break;
		case EInteractionType.EEI_PLACE_RESOURCES:
			DoPlaceResourcesInteraction (groundHit.point);
			break;
		default:
			break;
		}
	}

	public void Activate(EInteractionType interactionType) {
		currentInteraction = interactionType;
		_cursor.SetActive (true);

		// Enable script
		this.enabled = true;
	}

	/**
	 * Deactivate EnvironmentInteraction. This only happens if interactionType and
	 * the state that the EnvironmentInteraction is currently in, are the same.
	 */
	public void Deactivate(EInteractionType interactionType) {
		if (interactionType == currentInteraction) {
			currentInteraction = EInteractionType.EEI_NONE;
			_cursor.SetActive (false);

			// Disable script
			this.enabled = false;
		}
	}

	void DoPlacePheromoneInteraction(Vector3 cursorPosition) {
		// If the shift key is not hold, perform standard pheromone placement.
		if(!Input.GetButton("Fire3")) {
			// If possible, place / erase a pheromone
			if (placementTimer >= pheromonePlacementSpeed && Input.GetButton ("Fire1")) {
				if(pheromoneType != EPheromoneTypes.ErasePheromone) { 
					_pheromoneManager.PlacePheromone(cursorPosition, pheromoneType);

                    // Log placed pheromone in statistics manager.
                    _statisticsManager.pheromonesPlaced[pheromoneType] += 1;
				} else {
					_pheromoneManager.ErasePheromone(cursorPosition);
				}
				placementTimer = 0.0f;
			}
		}
		// Else: the shift key is hold, so perform task assignment.
		else {
			if (Input.GetButton("Fire1")) {
				AssignTask(cursorPosition);
			}
		}
	}

	void DoPlaceEnemiesInteraction(Vector3 cursorPosition) {
		if(Input.GetButtonDown("Fire1")) {
			Instantiate(_enemyPrefab, cursorPosition, new Quaternion());
		}
	}

	void DoPlaceResourcesInteraction(Vector3 cursorPosition) {
		if(Input.GetButtonDown("Fire1")) {
			Instantiate(_resourcePrefab, cursorPosition, new Quaternion());
		}
	}

	private void AssignTask(Vector3 position) {
		EAntTasks task;

		// Choose task to assing
		if(pheromoneType == EPheromoneTypes.Food) {
			task = EAntTasks.HarvestFood;
		} else if(pheromoneType == EPheromoneTypes.Attack) {
			task = EAntTasks.Attack;
		} else {
			return;
		}
		
		Collider[] targetedColliders = Physics.OverlapSphere(position, 0.5f, agentLayerMask);
		
		foreach(Collider collider in targetedColliders) {
			AgentStates antStates = collider.GetComponent<AgentStates>();
			
			if(antStates)
				antStates.SetTask (task);
		}
	}
}
