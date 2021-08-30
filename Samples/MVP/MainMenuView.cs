using maleric.MVP.Core.View;
using maleric.MVP.States;
using System;
using UnityEngine;

namespace maleric.MVP.Samples
{
	public class MainMenuView : UIView, IModelView<MainMenuModel>
	{
		public void OnModelChange(MainMenuModel data) { }
	}
}
