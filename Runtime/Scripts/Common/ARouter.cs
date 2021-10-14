using maleric.MVP.Service;
using maleric.MVP.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace maleric.MVP.Common
{
	/// <summary>
	/// ARouter is managing state. Its used for navigation between States and Scenes. In this MVP arhitecture, this is <strong>the only MonoBehaviour required</strong> in the scene.
	/// <para>Initially it finds the first state that use currently opened scene and sets it for initial state. It is used by other states and presenters/controlers to load different state. Uses LoadState to handle loading visuals.</para>
	/// <para><strong>How to create it</strong></para>
	/// <list type="number">
	/// <item>Create Router that extends ARouter</item>
	///		<item>
	///			<term>Implement <see cref="AState">AState[] GetStates()</see> method and return all states you use</term>
	///			<description>For best results, put initial states (states used inside scenes as initial state) first</description>
	///		</item>
	///		<item>
	///			<term>Implement <see cref="ALoadState">ALoadState GetLoadState(Type targetStateType)</see></term> 
	///			<description>Return LoadingState that will be visible when we load state from different scene</description>
	///		</item>
	///		<item>Override <strong>InitServices()</strong> and call _serviceLocator.Register(Service) to register all used services</item>
	/// </list>
	/// <para><strong>How to use it later</strong></para>
	/// <list type="bullet">
	/// <item>Easiest way is to create <strong>Router Prefab</strong> and use it in in starting scene, and also in all working scenes that needs to be played. If New Scene is loaded with another Router, that gameObject will be destroyed </item>
	/// <item>Router can also be created using Kickstarter if we need another arhitectural layer</item>
	/// </list>
	/// </summary>
	public abstract class ARouter : MonoBehaviour, IService
	{
		protected static ARouter _instance;

		protected StateMachine _stateMachine;
		protected AState[] _states;
		protected bool _isInitialized;

		protected ServiceLocator _serviceLocator;

		#region Service
		public bool IsReadyForService => true;

		public event Action OnServiceReady, OnReadyForServiceChange;
		#endregion

		protected virtual void Awake()
		{
			if (_instance != null) Destroy(this.gameObject);
			else _instance = this;

			_serviceLocator = new ServiceLocator();

			_states = GetStates();
			_stateMachine = new StateMachine(_states);

			DontDestroyOnLoad(this);
		}

		protected virtual void Start()
		{
			InitServices();
			InitStates();

			OnServiceReady?.Invoke();
			OnReadyForServiceChange?.Invoke();
		}

		/// <summary>
		/// Override, call base.InitServices() and register other services
		/// <example>
		///		<code>
		///			base.InitServices();
		///			var playerService = new PlayerService();
		///			_serviceLocator.Register(playerService);
		///			_serviceLocator.Register(new EconomyService(playerService));
		///		</code>
		/// </example>
		/// </summary>
		protected virtual void InitServices()
		{
			_serviceLocator.Register(this);
		}

		protected virtual void InitStates()
		{
			// Initial Scene Setup
			var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			List<AState> statesForThisScene = new List<AState>();
			for (int i = 0; i < _states.Length; i++)
			{
				if (_states[i].SceneName.Equals(activeScene.name)) statesForThisScene.Add(_states[i]);
			}

			_stateMachine.Setup(_serviceLocator);

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

		public async void GoToState(IState targetState) => await _stateMachine.GoToState(targetState, GetLoadState(targetState.GetType()));

		public async void GoToState<T>() where T : IState
		{
			var targetState = GetLoadState(typeof(T));
			if (targetState == null) Debug.LogError("State Not Found " + typeof(T).Name);
			await _stateMachine.GoToState<T>(targetState);
		}
		/// <summary>
		/// Return all states that we need inside this game/app.
		/// <example>
		///		<code>
		///			return new AState[] { new MainMenuState(), new GameState() };
		///		</code>
		/// </example>
		/// </summary>
		/// <returns></returns>
		public abstract States.AState[] GetStates();

		/// <summary>
		/// Used for loading between two states, visibile mostly only on scene change.
		/// <example>
		///		<code>
		///			if (loadState == null) loadState = new LoadState();
		///			return loadState;
		///		</code>
		/// </example>
		/// </summary>
		/// <param name="targetStateType">If we have multiple loading states, we can decide which one to return by using this param</param>
		/// <returns></returns>
		public abstract States.ALoadState GetLoadState(Type targetStateType);

		#region Service
		public bool IsServiceReady() => true;

		public IServiceDepended[] GetDependencies() => null;
		#endregion
	}
}