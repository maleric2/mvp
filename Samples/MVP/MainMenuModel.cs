using maleric.Core.MVP.Model;
using maleric.Core.States;
using System;

namespace maleric.Example
{
	public class MainMenuModel : IModel
	{
		public event Action OnModelChange;

		public void Dispose() { }
	}
}
