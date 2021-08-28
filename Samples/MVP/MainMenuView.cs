using maleric.Core.MVP.View;
using maleric.Core.States;
using System;
using UnityEngine;

namespace maleric.Example
{
	public class MainMenuView : UIView, IModelView<MainMenuModel>
	{
		public void OnModelChange(MainMenuModel data) { }
	}
}
