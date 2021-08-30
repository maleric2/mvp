using maleric.MVP.States;
using System;

namespace maleric.MVP.Samples
{
	public class MainMenuState : AState
	{
		public override string SceneName => "Menu";
		MainMenuPresenter mainMenuPresenter;

		public override void Setup(Service.IServiceLocator serviceLocator)
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
