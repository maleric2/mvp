using maleric.MVP.Core.Presenter;
using maleric.MVP.States;
using System;
using UnityEngine;

namespace maleric.MVP.Samples
{
	public class MainMenuPresenter : Presenter
	{
		private MainMenuView _view;

		public MainMenuPresenter(IState state) : base(state)
		{

		}

		protected override void CollectReferences()
		{
			_view = GameObject.FindObjectOfType<MainMenuView>();
			Debug.Log("Found View " + _view.ToString(), _view);
		}

		protected override void DisposeOfReferences()
		{
			_view = null;
		}

		protected override void InternalSetup() { }

		protected override void InternalUnsetup() { }

		protected override void InternalSetInteractable(bool isInteractable)
		{

		}

		protected override void InternalSetVisible(bool isVisible)
		{

		}
	}
}
