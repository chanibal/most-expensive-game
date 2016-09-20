using UnityEngine;
using System;


namespace ChanibaL {
	
	
	internal class StateMachineWithExceptionCatching:StateMachine {
		
		protected Func<StateMachine.State, Exception, StateMachine.State> onException;
		
		
		public StateMachineWithExceptionCatching(StateMachine.State init, Func<StateMachine.State, Exception, StateMachine.State> onException, bool log=false):base(init, log) {
			this.onException=onException;
		}
		
		
		protected override void RunState() {
			try {
				base.RunState();
			}
			catch(Exception e) {
				Debug.LogWarning(this + "state() exception at frame=" + Time.frameCount + ":");
				Debug.LogException(e);
				var exceptionState=onException(CurrentState, e);
				if(exceptionState != null)
					SwitchStateWithoutExitStateDoNotUseUnlessDesperate(exceptionState, false);	// false => delayed, so it does not hang, but waits for next Update()
				else 
					throw;
			}
		}
		
	}
	
	
	/// <summary>
	/// A MonoBehaviour with one StateMachine and configured events
	/// </summary>
	abstract public class StateMachineMonoBehaviour:MonoBehaviour {

		
		protected StateMachine sm;


		protected string originalGameObjectName;


		protected virtual void Start() {
			originalGameObjectName=gameObject.name;
			sm=new StateMachineWithExceptionCatching(StateInit, StateException);
		}


		protected virtual void Update() {
			if(Application.isPlaying) {
				sm.Update();
				#if UNITY_EDITOR
				gameObject.name=originalGameObjectName+"@"+sm.StateName+" +"+sm.SubStateMachineDepth;
				#endif
			}
			else
				DestroyImmediate(gameObject);
		}

		
		protected virtual void OnDestroy() {
			if(sm != null) {
				if(sm.CurrentState != StateEnd) {
					if(sm.CurrentlyExecuting)
						sm.SwitchState(StateEnd);
					else
						sm.SwitchStateExternalImmidiate(StateEnd);
				}
			}
		}

		
		protected abstract void StateInit();

		
		protected virtual void StateEnd() {
			if(sm.EnterState)
				sm.End();
		}


		protected void OnApplicationQuit() {
			OnDestroy();
		}

		
		/// <summary>
		/// Override this state factory to set up an exception catching state
		/// </summary>
		protected virtual StateMachine.State StateException(StateMachine.State lastState, Exception e) {
			return null;	// override to interrcept
		}
		
	}
	
}
