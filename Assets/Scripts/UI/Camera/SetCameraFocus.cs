using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Helper script that allows buttons to set the focus of the camera.
 * 
 * Please forgive me for overusing the word "focus".
 */
public class SetCameraFocus : MonoBehaviour {
	/**
	 * Reference to the CameraFocus component of the camera.
	 */
	[SerializeField]
	CameraFocus _camera;

	/**
	 * The transform of the object on which the camera should be centered.
	 */
	public Transform focusObject;

	/// <summary>
	/// Flag that indicates if the camera focus is locked in the _focusObject and
	/// thus cannot be moved.
	/// </summary>
	bool _isLocked = false;

	/// <summary>
	/// The image of e.g. a button to which this script is attached.
	/// </summary>
	Image _image;

	void Awake() {
		_image = GetComponent<Image> ();	
	}

	/// <summary>
	/// Focus the _focusObject without locking the camera.
	/// </summary>
	public void Focus() {
		_camera.Focus (focusObject.position);
	}

	/// <summary>
	/// If the focus is not locked, focus the _focusObject and lock the camera.
	/// If the camera is locked, unlock it.
	/// </summary>
	public void ToggleFocusLock() {
		if (!_isLocked) {
			_isLocked = true;
			_camera.Focus (focusObject.position);

			if (_image) {
				_image.color = Color.grey;
			}

		} else {
			UnlockFocus ();
		}
	}

	/// <summary>
	/// Unlocks the camera focus lock.
	/// </summary>
	public void UnlockFocus() {
		_isLocked = false;

		if (_image) {
			_image.color = Color.white;
		}
	}

	/// <summary>
	/// If the camera focus is locked, reset the focus to
	/// keep _focusObject in the focus.
	/// </summary>
	void Update() {
		if (_isLocked) {
			_camera.Focus (focusObject.position);
		}
	}
}
