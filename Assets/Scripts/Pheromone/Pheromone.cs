using UnityEngine;
using System.Collections;
//using UnityEditor;

public class Pheromone : MonoBehaviour {
	// Serialization only for debug purpose!
	[SerializeField]
	float _diffusionRate = 0.1f;

	// Serialization only for debug purpose!
	[SerializeField]
	float _minimumConcentration = 0.01f;

	// Serialization only for debug purpose!
	[SerializeField]
	float _startIntensity = 1.17809f;

	// Serialization only for debug purpose!
	[SerializeField]
	float _currentConcentration;

	[SerializeField]
	float _updateRate = 0.0f;
	float _updateTimer = 0.0f;

	[SerializeField]
	ParticleSystem _particleSystem;

	[SerializeField]
	GameObject _diffusionSphere;

	// Serialization only for debug purpos!
	[SerializeField]
	public EPheromoneTypes type { get; private set; }

	bool _currentVisibility;

	/**
	 * Reference to the GlobalPheromoneConfiguration of the PheromoneManager of the scene.
	 */
	GlobalPheromoneConfiguration _pheromoneConfiguration;

	void Awake() {
		_pheromoneConfiguration = FindObjectOfType (typeof(GlobalPheromoneConfiguration)) as GlobalPheromoneConfiguration;
	}

	void Update () {
		_updateTimer += Time.deltaTime;

		// Toggle visibility
		UpdateVisibility();		

		if(_updateTimer >= _updateRate) {
			_diffusionSphere.transform.localScale += new Vector3 (1f, 1f, 1f) * _diffusionRate * _updateTimer;

			RecalculateCurrentConcentration ();

			_updateTimer = 0.0f;

			if (_currentConcentration <= _minimumConcentration)
				Destroy (gameObject);
		}
	}

	void UpdateVisibility() {
		bool newVisibility = _pheromoneConfiguration.configs [type].particleSystemVisible;

		if (newVisibility == false && _currentVisibility == true) {
			_particleSystem.GetComponent<Renderer>().enabled = false;
		} else if(newVisibility == true && _currentVisibility == false) {
			_particleSystem.GetComponent<Renderer> ().enabled = true;
		}

		_currentVisibility = newVisibility;
	}

	public void Initialize(EPheromoneTypes type, float intensity, float diameter, float diffusionRate, float minimumConcentration, Color color) {
		this.type = type;
		_startIntensity = intensity;
		_diffusionRate = diffusionRate;
		_minimumConcentration = minimumConcentration;
		_diffusionSphere.transform.localScale = new Vector3 (diameter, diameter, diameter);

		// max radius = (n / (3/4 * PI * c))^(1/3)
		float maxRadius = Mathf.Pow (_startIntensity / (0.75f * Mathf.PI * _minimumConcentration), 1f / 3f);
		float ttl = maxRadius / _diffusionRate;

		// Initialize particle system
		_particleSystem.startLifetime = ttl;//Mathf.Pow ((0.75f * _startIntensity) / (_minimumConcentration * Mathf.PI), 1f / 3f) / diffusionRate + 3.0f;
		_particleSystem.startSpeed = diffusionRate * 0.5f;
		_particleSystem.startColor = color;

		ParticleSystem.EmissionModule emission = _particleSystem.emission;
		emission.rate = new ParticleSystem.MinMaxCurve(0f);

		ParticleSystem.ShapeModule shape = _particleSystem.shape;
		shape.radius = diameter * 0.5f;
		
		_particleSystem.Emit ((int)(50.0f * _startIntensity));

		_currentVisibility = true;

		RecalculateCurrentConcentration ();
		UpdateVisibility ();
	}

	public float currentConcentration {
		get {
			return _currentConcentration;
		}
	}

	public float startIntensity {
		get {
			return _startIntensity;
		}
		set {
			_startIntensity = value;
		}
	}

	void RecalculateCurrentConcentration() {
		// Calculate new concentration of pheromone. This depends on the volume and thus on the 
		// radius of the pheromone sphere. The volume of a sphere is V = 0.75 * rˆ3 * pi.
		//
		// Since we are using half spheres (pheromones can not travel through the ground) we need half
		// of the volumen 0.5 * V = 0.375 * rˆ3 * pi.
		//
		// Concentration is calculated by number of molecules / volume with number of molecules being
		// the startIntensity.
		float radius = _diffusionSphere.transform.localScale.x;
		_currentConcentration = _startIntensity / (0.375f * radius * radius * radius * Mathf.PI);
	}
}
