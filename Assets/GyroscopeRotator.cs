using UnityEngine;

/// <summary>
/// Gyroscope rotator. Attach to object 
/// based on:
/// http://forum.unity3d.com/threads/sharing-gyroscope-camera-script-ios-tested.241825/#post-1601382
/// https://github.com/MattRix/UnityDecompiled/blob/master/UnityEngine/UnityEngine/Transform.cs
/// </summary>
public class GyroscopeRotator : MonoBehaviour {


	public bool isACamera;


	void Start() {
		Input.gyro.enabled = true;
	}


	void Update() {
		Quaternion qGyro = Input.gyro.attitude;

		// Swap "handedness" of quaternion from gyro.
		qGyro *= Quaternion.Euler (0, 0, 180f);

		// Rotate to make sense as a camera pointing out the back of your device.
		qGyro *= Quaternion.Inverse (qGyro) * Quaternion.Euler (90f, 180f, 0f) * qGyro;

		// If this is not a camera - invert it
		if(!isACamera)
			qGyro = Quaternion.Inverse (qGyro);

		transform.localRotation = qGyro;
	}




}
