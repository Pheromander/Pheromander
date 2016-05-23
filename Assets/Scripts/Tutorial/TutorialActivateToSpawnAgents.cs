using UnityEngine;
using System.Collections;

public class TutorialActivateToSpawnAgents : MonoBehaviour {

	AgentManager agntMngr;
	void Start () {
		agntMngr = FindObjectOfType (typeof(AgentManager)) as AgentManager;
		agntMngr.IncreasePopulation (30);
	}

}
