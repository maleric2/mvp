using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace maleric.Core.States
{
	public interface IStateMachine
	{
		IBaseState CurrentState { get; }
		Task GoToState(IState state, ILoadState loadingSceneState);
		IState GetState(Type type);
		Action<IState> OnStateChange { get; set; }
	}

	public class StateMachine : IStateMachine, IDisposable
	{
		private readonly Dictionary<Type, IState> _statesMapPerType = new Dictionary<Type, IState>();

		private IBaseState _currentState = null;
		private IState _stateBeingLoaded = null;
		private AState[] _states;

		public IBaseState CurrentState => _currentState;

		public Action<IBaseState> OnStateChange { get; set; }

		Action<IState> IStateMachine.OnStateChange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public StateMachine(AState[] states)
		{
			_states = states;

			for (int i = 0; i < _states.Length; i++)
				_statesMapPerType.Add(_states[i].GetType(), _states[i]);
		}

		public void Setup()
		{
			for (int i = 0; i < _states.Length; i++)
				_states[i].Setup();
		}

		public void Dispose() { }

		public IState GetState(Type type)
		{
			return _statesMapPerType[type];
		}

		public async Task GoToState<T>(ILoadState loadingSceneState = null) where T : IState
		{
			await GoToState(GetState(typeof(T)), loadingSceneState);
		}

		/// <summary>
		/// Go to the State
		/// </summary>
		/// <param name="state">State to load</param>
		/// <param name="loadingSceneState">State that handles Loading progress between this and target State</param>
		/// <returns></returns>
		public async Task GoToState(IState state, ILoadState loadingSceneState)
		{
			string targetSceneName = state.SceneName;
			if (SceneManager.GetActiveScene().name != targetSceneName)
			{
				_stateBeingLoaded = state;

				await GoToStateInstantly(loadingSceneState);
				await loadingSceneState.LoadScene(targetSceneName);
				await GoToStateInstantly(state);
			}
			else
			{
				await GoToStateInstantly(state);
			}
		}

		private async Task GoToStateInstantly(IBaseState state)
		{
			if (_currentState != state)
			{
				Debug.LogWarningFormat("Changing From state '{0}' to state '{1}'", (_currentState != null ? _currentState.ToString() : ""), state.ToString());
				if (_currentState != null) _currentState.Exit();

				_currentState = state;

				await Task.Yield();
				_currentState.Enter();
				await Task.Yield();
				OnStateChange?.Invoke(state);
			}
		}
	}
}
