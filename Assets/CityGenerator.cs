using UnityEngine;
using System.Collections;

public class CityGenerator : MonoBehaviour {
	public GameObject building;

	[Range(0, 1000)]
	public int max = 200;

	void Start() {
		for (int i = 0; i < max; i++) {
			Vector3 pos = new Vector3 (Random.Range (-1000, 1000), -100, Random.Range (-1000, 1000));
			if (pos.magnitude < 800)
				continue;
			Quaternion rot = Quaternion.Euler (0, Random.Range (0, 360), 0);
			var go = GameObject.Instantiate (building, pos, rot) as GameObject;
			go.transform.localScale *= Random.Range (1f, 4f);
		}
	}
}
