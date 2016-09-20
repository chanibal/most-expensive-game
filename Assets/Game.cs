using UnityEngine;
using ChanibaL;

public class Game : StateMachineMonoBehaviour {


	public Sensors sensors;
	public GameObject menu;
	public UI ui;
	public AudioEffectPlayer audioPlayer;

	RandomGenerator rng = new RandomGenerator();


	void SubmitScore(float t, float rotation) {
		Debug.Log ("Score submitted: t=" + t + " rot=" + rotation);

		if (t < 0.1f)	
			return;

		bool record = false;
		if (t > ui.longestFreefall) {
			record = true;
			ui.longestFreefall = t;
		}

		if (rotation < 100 && t > ui.longestStableThrow) {
			record = true;
			ui.longestStableThrow = t;
		}

		if (rotation > ui.mostRotations) {
			record = true;
			ui.mostRotations = rotation;
		}

		if (record)
			audioPlayer.PlayRecord ();
	}


//	bool QueryAccelerometer(float min, float max, float frames = 20) {
//		
//		for (int i = sensors.accelHistory.Count - frames; i < sensors.accelHistory.Count; i++)
//			if (i > 0) {
//				if(senso
//			}
//	}

	#region state machine

	protected override void StateInit ()
	{
		sm.SwitchState (StateMenu);
		sm.logThisStateMachine = true;
	}


	void StateMenu()
	{
		if (sm.EnterState) {
			menu.SetActive (true);
			ui.gameObject.SetActive (false);
		}

		if (sensors.accelerometer >= Sensors.Accelerometer.high)
			sm.SwitchState (StateInHand);

		if (sm.ExitState) {
			menu.SetActive (false);
			ui.gameObject.SetActive (true);
		}
	}



	float startRotation;
	float t;
	void StateFalling() {
		if (sm.EnterState) {
			audioPlayer.PlayThrown ();
			startRotation = sensors.rotationSum;
			t = 0;
		}

		t += Time.deltaTime;

		if (sensors.accelerometer == Sensors.Accelerometer.stationary && sm.TimeInState > 0.25)
			sm.SwitchState (StateInHand);

		if (sensors.accelerometer >= Sensors.Accelerometer.high && sm.TimeInState > 0.1)
			sm.SwitchState (StateHit);

		if (sm.ExitState) {
			SubmitScore (t, sensors.rotationSum - startRotation);
		}
			
	}


	void StateInHand() {
		if (rng.HalfChanceInTime(3))
			audioPlayer.PlayTaunt ();

		if (sensors.accelerometer == Sensors.Accelerometer.freefall && sm.TimeInState > 0.2)
			sm.SwitchState (StateFalling);

		if(sensors.accelerometer >= Sensors.Accelerometer.high && sm.TimeInState > 0.2) {
			sm.SwitchState (StateFalling);
		}

		if (sm.TimeInState > 8)
			sm.SwitchState (StateMenu);
	}


	void StateHit() {
		if (sm.EnterState)
			audioPlayer.PlayHurt ();

		if (sensors.accelerometer == Sensors.Accelerometer.stationary && sm.TimeInState > 0.3)
			sm.SwitchState (StateInHand);

		if (sensors.accelerometer == Sensors.Accelerometer.freefall && sm.TimeInState > 0.3) 
			sm.SwitchState (StateFalling);

		if (sm.ExitState && rng.GetBool ())
			audioPlayer.PlaySurvived ();
			
	}

	#endregion

}
