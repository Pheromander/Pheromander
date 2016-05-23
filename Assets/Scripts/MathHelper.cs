using UnityEngine;
using System.Collections;

public class MathHelper : MonoBehaviour {
	//
	// Return the relative angle between v1 and v2 along the
	// rotation axis n.
	// Returns the (signed) angle in radians.
	public static float AngleSigned(Vector3 v1, Vector2 v2, Vector3 n) {
		return Mathf.Atan2 (
			Vector3.Dot (n, Vector3.Cross (v1, v2)),
			Vector3.Dot (v1, v2)
		);
	}
}
