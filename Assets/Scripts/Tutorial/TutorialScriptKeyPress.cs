using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class TutorialScriptKeyPress : MonoBehaviour
{
	public GameObject panel;
	public Text showText, keyPressText;
	public GameObject[] arrows; // arrows to show
	public Text[] textToShow; // holds texts to show for each step
	public GameObject enemy;
	public GameObject[] focus;
	public AgentManager agentManager;
	int counter = 1;
	private Vector3 moveCamera;

	// Use tis for initialization
	void Start ()
	{
		enemy.SetActive (false);
		agentManager.GetComponent<AgentManager> ().enabled = false;
		showText.text = textToShow [0].text;
		moveCamera = enemy.gameObject.transform.position;
		DeactivateAllArrows ();
		moveCamera = new Vector3 ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		//moveCamera.y += 50;
		if (Input.GetKeyDown (KeyCode.Space) && counter < 14) {

			showText.text = textToShow [counter].text;
			if(arrows[counter]!=null)
			   arrows[counter].SetActive(true);
			if(arrows[counter-1]!=null)
			   arrows[counter-1].SetActive(false);

			if (counter == 4)
				agentManager.GetComponent<AgentManager> ().enabled = true; //spawn agents
			
			if (counter == 13){
				focus [counter].SetActive (true); // "spawn" enemy
				moveCamera = focus [counter].transform.position;
				moveCamera.y += 55;
				moveCamera.z -= 25;
				Camera.main.transform.position = moveCamera;
			}
			++counter;

		}
		if (counter > 0 && counter <= 13) {
			if (focus [counter - 1] != null) {
				moveCamera = focus [counter - 1].transform.position;
				moveCamera.y += 55;
				moveCamera.z -= 25;
				Camera.main.transform.position = moveCamera;

			}

		}
		if (counter > 13 && Input.GetKeyDown (KeyCode.Space)) {
			if(counter > 14){
				this.gameObject.SetActive(false);
			}
			   ++counter;

		}

	}

	void DeactivateAllArrows ()
	{

		for (int i=0; i<arrows.Length; ++i) {
			if (arrows [i] != null)
				arrows [i].SetActive (false);
		}
	}
}