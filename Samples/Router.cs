using maleric.MVP.Common;
using maleric.MVP.States;
using System;
using UnityEngine;

namespace maleric.MVP.Samples
{

	// Example:
	// Router: Manages how to load stuff - can be used in one scene but also in many others
	// Kickstarter? Start something and destroy itself later on -> We can use this to initialize stuff at first (call Router and similar)
	// Signleton -> We can also use Singleton to call otherstuff from it, Singleton could have ServiceProvider, Router
	// Note: MVP is used generally to avoid using singleton



	// TODO:
	// - Complete this example, currently its good, it works
	// - Check if we can use Prefabs for View instead
	// - Note if we're going to spawn prefabs, its best to create Game.Resources with all paths inside it to avoid a lot of ScriptableObjects
	// - Create Universal Load State that can be always used even if its state without UI or its created on go
	// - Manage Routers Better and DontDestroyOnLoad and If another scene have other Router/Camera and similar -> We should ignore it
	// - Check if Router should be Singleton or should we use something like that to access it for easy scene change
	// - Services?
	// - Maybe Kickstarter + Router => Kickstarter gets the router and handles it and its Singleton (Kickstarter.Router)

	// Note: Having Scriptable Object for a reference creates a problem where we need to check
	//		 a lot of SO to see what is valid, and to slot everything, that takes more time

	// Cons:
	// - It does load new scene, its not inside 1 scene as in HOP
	// - MVP is good but, a bit slow, Model + View + Presenter each time but GenericPresenter helps

	// Pros:
	// - Clear arhitecture
	// - Many developers can work at same time at ease
	public class Router : ARouter
	{
		private LoadState loadState = null;

		public override ALoadState GetLoadState(Type targetStateType)
		{
			if (loadState == null) loadState = new LoadState();
			return loadState;
		}

		public override AState[] GetStates()
		{
			return new AState[] { new MainMenuState(), new GameState() };
		}

		[ContextMenu("Go To Game")]
		public void GoToGame()
		{
			GoToState<GameState>();
		}
	}
}
