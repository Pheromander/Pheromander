using UnityEngine;
using System.Collections;

public class AlwaysStickToAgent : MonoBehaviour {
	private GameObject agent;
	public GameObject arrow;

	// Use this for initialization
	void Start () {
		//agent = FindObjectOfType(typeof(EnemySensor)) as GameObject;
		//if (agent == null)
			//Debug.Log ("No agent found!!");
		//else
			//arrow.transform.parent = agent.transform;


		//suche nach dem agent im gameobject haufen


	}

	void Update () {
		GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

		foreach (GameObject go in gos) {

			if (go.layer == 9) { //ant layer
				agent = go;
			//Debug.Log ("Agent found");
				break;
			}

		}
		this.transform.parent = agent.transform;
	}
}
