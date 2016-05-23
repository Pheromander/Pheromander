using UnityEngine;
using System.Collections;

public class CameraFocus : MonoBehaviour {
	[SerializeField]
	Transform _movementTransform;

	[SerializeField]
	Transform _zoomTransform;

	float _initialZoomDistance;

	void Awake() {
		_initialZoomDistance = _zoomTransform.position.y;
	}

	public void Focus(Vector3 focusPoint, bool lockFocus = false) {
		Vector3 targetPosition = focusPoint - _initialZoomDistance * _zoomTransform.forward;
		_movementTransform.position = new Vector3 (targetPosition.x, _movementTransform.position.y, targetPosition.z);

		//_zoomTransform.position = new Vector3(_movementTransform.position.x, _zoomTransform.position.y, _movementTransform.position.z);
		_zoomTransform.position = new Vector3 (_movementTransform.position.x, _initialZoomDistance, _movementTransform.position.z);
	}
}
