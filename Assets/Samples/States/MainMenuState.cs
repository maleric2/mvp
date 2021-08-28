using maleric.Core.States;
using System;

namespace maleric.Example
{
	public class MainMenuState : AState
	{
		public override string SceneName => "Menu";
		MainMenuPresenter mainMenuPresenter;

		public override void Setup()
		{
			mainMenuPresenter = new MainMenuPresenter(this);
		}

		public override void Unsetup()
		{
			mainMenuPresenter = null;
		}

		protected override void OnStateEnter()
		{
			mainMenuPresenter.Setup();
			mainMenuPresenter.SetVisible(true);
		}

		protected override void OnStateExit()
		{
			mainMenuPresenter.SetVisible(false);
			mainMenuPresenter.Unsetup();
		}
	}
}
