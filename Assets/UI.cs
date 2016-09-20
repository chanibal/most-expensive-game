using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour {


	public Text text;

	void Update() {
		text.text = string.Format (
			"Longest throw: {0:0.00}sec\nMost rotations: {1:0} deg\nLongest stable throw: {2:0.00}sec",
			longestFreefall,
			mostRotations,
			longestStableThrow
		);
	}


	public float longestFreefall;
	public float longestStableThrow;
	public float mostRotations;

}
