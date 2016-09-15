using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;


public class Sensors : MonoBehaviour {


	public Text output;

	public Vector3 offset;
	public bool inverse;

	List<AccelerationEvent> evs=new List<AccelerationEvent>();


	void Start() {
//		SystemInfo.supportsGyroscope;
//		SystemInfo.supportsAccelerometer;
		Input.gyro.enabled = true;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}


	void Update () {
		evs.AddRange (Input.accelerationEvents);
		if (evs.Count > 256)
			evs.RemoveRange (0, evs.Count - 256);


		Quaternion q = Quaternion.identity;
		q = Quaternion.Euler(offset) * q;

		if (inverse)
			q = Quaternion.Inverse (Input.gyro.attitude) * q;
		else
			q = Input.gyro.attitude * q;


		transform.rotation = q;

		output.text = Input.gyro.rotationRate.ToString();
	}



	public Material matPositive;
	public Material matFreefall;
	public Material matNegative;
	void OnPostRender() {
		var yscale = 0.25f;
		var xscale = 1f / 256f;
		GL.PushMatrix();
		matPositive.SetPass(0);
		GL.LoadOrtho();
		GL.Begin(GL.LINES);
		GL.Color(Color.red);

		for (int i = 0; i < evs.Count; i++) {
			var mag = evs [i].acceleration.magnitude;
			GL.Vertex3(i * xscale, 0.5f, 1);
			GL.Vertex3(i * xscale, 0.5f + mag * yscale, 1);
		}
		GL.End();
		GL.PopMatrix();
	}
}
