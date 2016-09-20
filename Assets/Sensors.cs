using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class Sensors : MonoBehaviour {

	public class History:List<float> {

		public History() {
			Capacity = 256;
		}

		public new void Add(float val) {
			base.Add (val);
			if (Count > 256)
				RemoveRange (0, Count - 256);
		}
	}


	public enum Accelerometer { freefall, stationary, high, very_high }
	public Accelerometer accelerometer;

	public enum Gyroscope { none, low, high }
	public Gyroscope gyroscope;
	public float rotationSum;

	public float lerpFactor = 1.2f;

	public Text output;

	public History accelHistory = new History();
	public History gyroHistory = new History();
	public History accelHistorySoft = new History();
	public History gyroHistorySoft = new History();



	void Start() {
//		SystemInfo.supportsGyroscope;
//		SystemInfo.supportsAccelerometer;
		Input.gyro.enabled = true;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		accelSoft = CurrentAccel;
		gyroSoft = CurrentGyro;
	}


	public float CurrentAccel {
		get {
			return Input.acceleration.magnitude;
		}
	}


	Quaternion currentGyroSoft;
	public float CurrentGyro {
		get {
			return Quaternion.Angle (
				currentGyroSoft,
				Quaternion.identity
			);
		}
	}


	float accelSoft;
	float gyroSoft;
	void Update () {
		// accel
		accelSoft = Mathf.Lerp (accelSoft, CurrentAccel, Time.deltaTime * lerpFactor);

		if (accelSoft < 0.8)
			accelerometer = Accelerometer.freefall;
		else if (accelSoft < 1.2)
			accelerometer = Accelerometer.stationary;
		else if (accelSoft < 5)
			accelerometer = Accelerometer.high;
		else
			accelerometer = Accelerometer.very_high;

		if (CurrentAccel > 8)
			accelerometer = Accelerometer.very_high;
		
		accelHistory.Add (CurrentAccel);
		accelHistorySoft.Add (accelSoft);

		// gyro
		currentGyroSoft = Quaternion.SlerpUnclamped(currentGyroSoft, Quaternion.Euler (Input.gyro.rotationRate * Mathf.Rad2Deg), Time.deltaTime * lerpFactor);


		if (CurrentGyro < 15)
			gyroscope = Gyroscope.none;
		else if (CurrentGyro < 100)
			gyroscope = Gyroscope.low;
		else
			gyroscope = Gyroscope.high;

		if (CurrentGyro > 150)
			gyroscope = Gyroscope.high;

		gyroHistory.Add (Quaternion.Angle (Quaternion.Euler (Input.gyro.rotationRate * Mathf.Rad2Deg), Quaternion.identity));
		gyroHistorySoft.Add (CurrentGyro);


		if (gyroscope > Gyroscope.none)
			rotationSum += CurrentGyro * Time.deltaTime;

		// ui
		output.text = string.Format ("{0}\n{2:00.000}g\n{4:00.000}...{5:00.000}\n\n{1}\n{3:00.000}deg/sec\n{6:00.000}...{7:00.000}\n{8:00000}", 
			accelerometer, gyroscope, CurrentAccel, CurrentGyro, accelHistory.Min(), accelHistory.Max(), gyroHistory.Min(), gyroHistory.Max(), rotationSum);

//		if (Input.touchCount > 0) {
//			gyroHistory.Clear ();
//			accelHistory.Clear ();
//			gyroHistorySoft.Clear ();
//			accelHistorySoft.Clear ();
//			rotationSum = 0;
//		}
			
	}



	public Material matAccel;
	public Material matGyro;
	void OnPostRender() {
		var xscale = 1f / 256f;
		GL.PushMatrix();
		matAccel.SetPass(0);
		GL.LoadOrtho();
	
		GL.Begin(GL.LINES);
		for (int i = 0; i < accelHistory.Count; i++) {
			var mag = accelHistory [i];
			GL.Vertex3(i * xscale, 0.5f, 1);
			GL.Vertex3(i * xscale, 0.5f + mag/10, 1);
		}
		for (int i = 1; i < accelHistorySoft.Count; i++) {
			var mag1 = accelHistorySoft [i-1];
			var mag2 = accelHistorySoft [i];
			GL.Vertex3((i-1) * xscale, 0.5f + mag1/10, 1);
			GL.Vertex3(i * xscale, 0.5f + mag2/10, 1);
		}
		GL.End();

		GL.Begin(GL.LINES);
		matGyro.SetPass(0);
		for (int i = 0; i < gyroHistory.Count; i++) {
			var mag = gyroHistory [i];
			GL.Vertex3(i * xscale, 0.5f, 1);
			GL.Vertex3(i * xscale, 0.5f - mag/400, 1);
		}
		for (int i = 1; i < gyroHistorySoft.Count; i++) {
			var mag1 = gyroHistorySoft [i-1];
			var mag2 = gyroHistorySoft [i];
			GL.Vertex3((i-1) * xscale, 0.5f - mag1/400, 1);
			GL.Vertex3(i * xscale, 0.5f - mag2/400, 1);
		}
		GL.End();

		GL.PopMatrix();
	}
}
