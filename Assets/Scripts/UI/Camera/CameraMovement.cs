using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float scrollSpeed = 3.0f;
	public float rotationSpeed = 100.0f;
	
	// Update is called once per frame
	void Update () {
		Vector3 translation = new Vector3 ();
		translation.x = Input.GetAxis ("Horizontal") * scrollSpeed * Time.deltaTime;
		translation.z = Input.GetAxis ("Vertical") * scrollSpeed * Time.deltaTime;
		
		transform.Translate (translation);
		
		
		// Rotation
		if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) {
			Vector3 rotation = new Vector3 ();
			rotation.y = Input.GetAxis ("Mouse X") * rotationSpeed * Time.deltaTime;
			transform.Rotate (rotation);
		}
		
	}
}
