using maleric.Core.Common;
using maleric.Core.States;
using System;
using UnityEngine;

namespace maleric.Example
{
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
