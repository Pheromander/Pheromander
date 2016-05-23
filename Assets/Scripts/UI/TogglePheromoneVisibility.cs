using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Script that toggles the visibilty of a given pheromone type.
 * 
 * This script should be attached to a button.
 */
[RequireComponent (typeof(Image))]
public class TogglePheromoneVisibility : MonoBehaviour {
	[SerializeField]
	EPheromoneTypes _pheromoneType;

	/**
	 * A sprite to display if pheromones are visible (e.g. an open eye).
	 */
	[SerializeField]
	Sprite _visibleSprite;

	/**
	 * A sprite to display if pheromones are hidden (e.g. a closed eye).
	 */
	[SerializeField]
	Sprite _hiddenSprite;

	/**
	 * Image of the button. This is used to mark the button as pressed, i.e.
	 * make it's background grey.
	 */
	Image _image;

	/**
	 * Image used to display _visibleSprite or _hiddenSprite.
	 */
	Image _iconImage;

	PheromoneConfiguration _pheromoneConfiguration;

	void Awake() {
		_image = GetComponent<Image> ();
		_iconImage = transform.Find ("IconImage").GetComponent<Image> ();

		GlobalPheromoneConfiguration globalPheromoneConfiguration = FindObjectOfType (typeof(GlobalPheromoneConfiguration)) as GlobalPheromoneConfiguration;
		_pheromoneConfiguration = globalPheromoneConfiguration.configs [_pheromoneType];
	}

	/**
	 * Toggle the visibility of all pheromones of type _pheromoneType. Also update
	 * the button to display the current visibility state.
	 */
	public void ToggleVisibility () {
		bool visibility = !_pheromoneConfiguration.particleSystemVisible;
		_pheromoneConfiguration.particleSystemVisible = visibility;

		if (!visibility) {
			_image.color = Color.gray;
			_iconImage.sprite = _hiddenSprite;
		} else {
			_image.color = Color.white;
			_iconImage.sprite = _visibleSprite;
		}
	}
}
