using maleric.Core.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace maleric.Core.Common
{

	/// <summary>
	/// ARouter is used for navigation between States and scenes
	/// This is the only MonoBehaviour required in the scene
	/// Concrete version must implement states and Load State
	/// </summary>
	public abstract class ARouter : MonoBehaviour
	{
		protected StateMachine _stateMachine;
		protected AState[] _states;
		protected bool _isInitialized;

		protected virtual void Awake()
		{
			_states = GetStates();
			_stateMachine = new StateMachine(_states);
			DontDestroyOnLoad(this);
		}

		protected virtual void Start()
		{
			// Initial Scene Setup
			var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			List<AState> statesForThisScene = new List<AState>();
			for (int i = 0; i < _states.Length; i++)
			{
				if (_states[i].SceneName.Equals(activeScene.name)) statesForThisScene.Add(_states[i]);
			}

			_stateMachine.Setup();

			if (statesForThisScene.Count > 0)
			{
				GoToState(statesForThisScene[0]);
			}
			else
			{
				Debug.LogError("No State Found for this scene");
			}
		}

		protected void OnDestroy()
		{
			_stateMachine.Dispose();
		}

		public async void GoToState(IState targetState)
		{
			await _stateMachine.GoToState(targetState, GetLoadState(targetState.GetType()));
		}

		public async void GoToState<T>() where T : IState
		{
			await _stateMachine.GoToState<T>(GetLoadState(typeof(T)));
		}

		public abstract States.AState[] GetStates();

		public abstract States.ALoadState GetLoadState(Type targetStateType);
	}
}