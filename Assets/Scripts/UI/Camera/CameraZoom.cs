using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	public float zoomSpeed = 100.0f;
	
	public float minDistance = 3.0f;
	public float maxDistance = 25.0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// "zoom"
		if (Input.GetAxis("Mouse ScrollWheel")!= 0 ){
			float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
			
			Vector3 newPosition = transform.position + transform.forward * zoomDelta;
			if(newPosition.y > minDistance && newPosition.y < maxDistance) {
				transform.position = newPosition;
			}
		}
	}
}
