using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialStepwise : MonoBehaviour
{

	//One step needs to have an Arrow pointing at the function or object you want to show, one Object that the camera will follow(no standard movement during this time) and two Texts, one that will actually be shown in UI and one that contains the text. An alternative constructor for MainCamera y and z position offset is available. If you give nothing/null the step wont cause an exception but simply ignore it
	public class TutorialStep
	{
		
		public GameObject Arrow;
		public GameObject CameraFocus;
		public GameObject ObjectToActivate;
		public Text TextToShow;
		public Text ShowText;
		private float CAMERAOFFSET_Y = 75;
		private float CAMERAOFFSET_Z = -35;
		private bool initiated = false;


		//procedures that only need to care of once
		public void ShowOnce ()
		{
			if (Arrow != null) {	//activate pointing arrow if there is any
				Arrow.SetActive (true);
			}
			if (TextToShow != null) {
				ShowText.text = TextToShow.text; //updates text if there is any
			}
			if (ObjectToActivate != null) {	//activate object if there is any
				ObjectToActivate.SetActive (true);
			}
			if (CameraFocus != null) {
				Vector3 moveCamera = CameraFocus.transform.position;
				moveCamera.y += CAMERAOFFSET_Y;
				moveCamera.z += CAMERAOFFSET_Z;
				Camera.main.transform.position = moveCamera;
			}
			
		}
		//procedures that need to be updated every frame
		public void ShowAlways ()
		{
			


		}

	

		//call this method to "run" the step. initialisation and updating is all done here
		public void runStep ()
		{
			if (!initiated) {
				ShowOnce ();
				initiated = true;
			}
			ShowAlways ();
		}

		public void stopStep ()
		{
			if (Arrow != null) {	//deactivate pointing arrow if there is any
				Arrow.SetActive (false);
			}
			/*if (ObjectToActivate != null) {	//deactivate object if there is any
				ObjectToActivate.SetActive (false);
			}
			*/
			initiated = false;


		}

		public TutorialStep (GameObject Arrow, GameObject CameraFocus, GameObject ObjectToActivate, Text TextToShow, Text ShowText)
		{
			this.Arrow = Arrow;
			this.CameraFocus = CameraFocus;
			this.ObjectToActivate = ObjectToActivate;
			this.TextToShow = TextToShow;
			this.ShowText = ShowText;
		}

		public TutorialStep (GameObject Arrow, GameObject CameraFocus, GameObject ObjectToActivate, Text TextToShow, Text ShowText, float camY, float camZ)
		{
			if(this.Arrow !=null)
				this.Arrow = Arrow;
			if(this.CameraFocus !=null)
				this.CameraFocus = CameraFocus;
			if(this.ObjectToActivate!=null)
				this.ObjectToActivate = ObjectToActivate;
			if(this.TextToShow!=null)
				this.TextToShow = TextToShow;
			if(this.ShowText!=null)
				this.ShowText = ShowText;
			
			this.CAMERAOFFSET_Y = camY;
			this.CAMERAOFFSET_Z = camZ;
		}



	}

	//wenn neue steps hinzukommen oder rausfallen, hier die länge des arrays einstellen
	public TutorialStep[] tutorialSteps = new TutorialStep[18];

	private int currentTutorialStep = 0;
	private bool increase =false,decrease=false;
	public GameObject[] Arrows;
	public GameObject[] CameraFocus;
	public GameObject[] ObjectToActivate;
	public Text[] Trigger;
	public float[] TriggerValue;
	public Text[] TextToShow;
	public Text ShowText;
	public Text nextStepText;
	public string pressKeyText;
	public string notReadyText;
	public GameObject nextStepButton, prevStepButton;
	public Text TutorialTitle;
	private bool once=true;
	private float counterGameEnded = 3.0f;

	public Text AmountAgents,AmountResources;
	public GameObject gameLostScreen;

	//return first found agent (gameobject on ant layer)
	private GameObject FindFirstAgent()
	{

		GameObject[] gos = GameObject.FindObjectsOfType (typeof(GameObject)) as GameObject[];

		foreach (GameObject go in gos) {
			if (go.layer == 9) { //ant layer
				return go;
			}

		}
		return null;

	}

	//check if there is any trigger. if so, check if it is already fulfilled.
	bool checkTrigger (int i)
	{
		if (i >= 0 && i < Trigger.Length) {
			if (Trigger [i] == null)
				return true;
			else {
			
				float temp = 0;
				float.TryParse (Trigger [i].text, out temp);
				//Debug.Log ("currentStep: " + i + " ist : " + temp + "| soll : " + TriggerValue [i]);
				if (temp >= TriggerValue [i])
					return true;
				
			}
		} 
		return false;
	}

	void Start ()
	{
					
		for (int i = 0; i < tutorialSteps.Length; ++i) {
			tutorialSteps [i] = new TutorialStep (Arrows [i], CameraFocus [i], ObjectToActivate [i], TextToShow [i], ShowText);
			tutorialSteps [i].stopStep ();
		}

		// manually give step 6 focus to next found agent


	
	


		//add initial 5 resources to tutorial
		FindObjectOfType<ResourceManager> ().AddResources (5);
	}

	//if you occasionally make the tutorial "unbeatable", spawn 30 more agents.
	void checkGameEnded(){
		float agents = 1, resources = 1;
		float.TryParse (AmountAgents.text, out agents);
		float.TryParse (AmountResources.text,out resources);
		if (agents <=0 && resources <=0)
		{
			counterGameEnded = counterGameEnded - Time.deltaTime;
		}
		if (agents <=0 && resources <=0 & counterGameEnded <=0)
		{

			gameLostScreen.SetActive (true);
			AgentManager agntMngr = FindObjectOfType (typeof(AgentManager)) as AgentManager;
			agntMngr.IncreasePopulation (30);
			counterGameEnded = 2.0f;
		}

	}

	/*
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && currentTutorialStep < tutorialSteps.Length) {
			if (checkTrigger (currentTutorialStep)) {
				if (currentTutorialStep > 0) {
					tutorialSteps [currentTutorialStep].stopStep (); //shut down current text
				}
				++currentTutorialStep; //increment to next step
				nextStepText.text = pressKeyText;
			} else {
				nextStepText.text = notReadyText;
			}
		}
		if (currentTutorialStep < tutorialSteps.Length) {
			tutorialSteps [currentTutorialStep].runStep (); //run commands of current text
		}
	}
*/
	void Update ()
	{
		checkGameEnded ();

		if (currentTutorialStep <= tutorialSteps.Length)
		{
			if (increase) // initiate next step
			{
				if (checkTrigger (currentTutorialStep)) 
				{
					if (currentTutorialStep >= 0)
					{
						tutorialSteps [currentTutorialStep].stopStep (); //shut down current text
					}
					++currentTutorialStep; //increment to next step

					nextStepText.text = pressKeyText;
					TutorialTitle.text = "Tutorial : "+currentTutorialStep + "/" + (tutorialSteps.Length-1);

				}
				else
				{
					nextStepText.text =  "complete step " + currentTutorialStep + "!";
				}

				if (currentTutorialStep < tutorialSteps.Length) 
				{
					tutorialSteps [currentTutorialStep].runStep (); //run commands of current text
				}
				increase = false;
			}

			if (decrease)
			{
				
					if (currentTutorialStep > 0)
					{
						tutorialSteps [currentTutorialStep].stopStep (); //shut down current text
						--currentTutorialStep; //decrement to previous step
					TutorialTitle.text = "Tutorial : "+currentTutorialStep + "/" + (tutorialSteps.Length-1);

						nextStepText.text = pressKeyText;
						tutorialSteps [currentTutorialStep].runStep ();
					Debug.Log ("Backspace " + currentTutorialStep);
					}
					
				decrease = false;
			}
		}
		if (currentTutorialStep < tutorialSteps.Length) {
			tutorialSteps [currentTutorialStep].runStep (); //run commands of current text
		}

		//release trigger text if necessary

		if (Trigger [currentTutorialStep] != null && checkTrigger (currentTutorialStep)) {
			nextStepText.text = "step " + currentTutorialStep + " completed";
			nextStepButton.GetComponent<Image> ().color = Color.white;
		}

		// gray out a button if it is not pressable
		if (Trigger [currentTutorialStep] != null && !checkTrigger (currentTutorialStep)) {
			nextStepButton.GetComponent<Image> ().color = Color.gray;
		}

		//release a gray button if there is no trigger

		if(Trigger[currentTutorialStep] == null)
			nextStepButton.GetComponent<Image> ().color = Color.white;
		
		//deactivate buttons for next and previous step if it is not possible to press them
		switch (currentTutorialStep) {
		case 0:
			prevStepButton.SetActive (false);
			nextStepButton.SetActive (true);
			TutorialTitle.text = "";
			break;

		case 5: //focus on the next agent. necessary to do this here because of possible null pointer during runtime(agents die)
			
			if (once) {
				GameObject firstAgent = FindFirstAgent ();
				Vector3 moveCamera = firstAgent.transform.position;
				moveCamera.y += 75;
				moveCamera.z += -35;
				Camera.main.transform.position = moveCamera;
				once = false;
			}
			break;
			
		
		case (17) :
			prevStepButton.SetActive (true);
			nextStepButton.SetActive (false);
			TutorialTitle.text = "Tutorial finished";
			break;
		default:
			prevStepButton.SetActive (true);
			nextStepButton.SetActive (true);
			break;
		}
			
	}

	public void increaseStep(){
		increase = true;
		once = true;
	}

	public void decreaseStep(){
		decrease = true;
		once = true;
	}

	public void OnDeath(){
		Debug.Log ("Enemy totgemacht!");
		Trigger[16].text = "2";
		// manual trigger for enemy dead in step 15. lookup the script attached to the enemy for more information
	}
	
}