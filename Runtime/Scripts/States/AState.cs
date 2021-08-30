using maleric.MVP.Common;
using System;
using UnityEngine;

namespace maleric.MVP.States
{
	// Arhitecture Preview:
	// 1. Scenes -> We can have multiple scenes, mostly used for different chunks of game
	//	2. States -> Multiple States that Groups Behavior with multiple presenters and views inside it
	//		3. Presenter -> 

	public interface IBaseState
	{
		bool IsActive { get; }
		void Enter();
		void Exit();
	}

	/// <summary>
	/// State Interface where State is positioned higher in arhitecture.
	/// State is container of mutual behaviour like MenuState, GameplayState, MapState, SettingState
	/// Its tightly connected with Scene. If we're not in correct State, it will be loaded.
	/// State defines how View is organized / instantiated in the scene
	/// </summary>
	public interface IState : IBaseState, IServiceDepended
	{
		string SceneName { get; }
		void Setup(Service.IServiceLocator serviceLocator);
		void Unsetup();

		GameObject GetStateGameObject();
	}

	public abstract class AState : IState
	{
		public bool IsActive { get; private set; }

		public bool IsReadyForService => IsActive;

		public abstract string SceneName { get; }

		public event Action OnReadyForServiceChange;

		protected GameObject _stateGO;

		/// <summary>
		/// Setting up state only once at initialization
		/// </summary>
		public abstract void Setup(Service.IServiceLocator serviceLocator);

		/// <summary>
		/// State unsetup usually on game exit
		/// </summary>
		public abstract void Unsetup();

		public void Enter()
		{
			_stateGO = _stateGO ?? new GameObject(this.GetType().Name);

			IsActive = true;
			OnStateEnter();
			OnReadyForServiceChange?.Invoke();
		}

		public void Exit()
		{
			OnStateExit();
			IsActive = false;
			OnReadyForServiceChange?.Invoke();
		}

		public GameObject GetStateGameObject()
		{
			return _stateGO;
		}

		protected abstract void OnStateEnter();
		protected abstract void OnStateExit();
	}
}
