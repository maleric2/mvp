using maleric.Core.MVP.Presenter;
using maleric.Core.States;
using System;

namespace maleric.Example
{
	public class GameState : AState
	{
		public override string SceneName => "Game";

		protected Presenter<GameView> _simpleViewPresenter;

		public override void Setup()
		{
			_simpleViewPresenter = new Presenter<GameView>(this);
		}

		public override void Unsetup()
		{
			_simpleViewPresenter = null;
		}

		protected override void OnStateEnter()
		{
			_simpleViewPresenter.Setup();
			_simpleViewPresenter.SetVisible(true);
		}

		protected override void OnStateExit()
		{
			_simpleViewPresenter.SetVisible(false);
			_simpleViewPresenter.Unsetup();
		}
	}
}
