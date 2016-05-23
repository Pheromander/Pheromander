using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
[RequireComponent (typeof(Slider))]
public class SliderWidget : MonoBehaviour {
	[SerializeField]
	float minValue;

	[SerializeField]
	float maxValue;

	[SerializeField]
	string rounding = "F2";

	[SerializeField]
	Slider slider;

	[SerializeField]
	InputField input;

	void Awake() {
		slider.minValue = minValue;
		slider.maxValue = maxValue;
	}

	public void SliderChanged(float value) {
		input.text = value.ToString (rounding);
	}

	public void InputChanged() {
		float value = 0.0f;

		try {
			value = float.Parse (input.text);
			slider.value = value;
			input.text = slider.value.ToString(rounding);
		} catch {
			input.text = slider.value.ToString(rounding);
		}
	}

	public void SetValue(float value) {
		slider.value = value;
		SliderChanged (value);
	}

	public float GetValue() {
		return slider.value;
	}
}
