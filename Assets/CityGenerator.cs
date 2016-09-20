using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CityGenerator : MonoBehaviour {
	public GameObject building;

	[Range(0, 1000)]
	public int max = 200;

	#if UNITY_EDITOR
	[ContextMenu("generate")]
	void Generate() {
		foreach (Transform child in transform)
			DestroyImmediate (child.gameObject);

		for (int i = 0; i < max; i++) {
			Vector3 pos = new Vector3 (Random.Range (-1000, 1000), -100, Random.Range (-1000, 1000));

			Quaternion rot = Quaternion.Euler (0, Random.Range (0, 360), 0);
			var go = GameObject.Instantiate (building, pos, rot) as GameObject;

			var scale = go.transform.localScale;
			scale *= Random.Range (1f, 4f);
			if (pos.magnitude < 800)
				scale.y /= 4;
			go.transform.localScale = scale;

			pos.y = go.transform.localScale.y/2;
			go.transform.localPosition = pos;
			go.transform.parent = transform;
			go.isStatic = true;
		}
	}
	#endif
}
