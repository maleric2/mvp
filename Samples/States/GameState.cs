using maleric.MVP.Core.Presenter;
using maleric.MVP.States;
using System;

namespace maleric.MVP.Samples
{
	public class GameState : AState
	{
		public override string SceneName => "Game";

		protected Presenter<GameView> _simpleViewPresenter;

		public override void Setup(Service.IServiceLocator serviceLocator)
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
