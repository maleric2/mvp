using maleric.MVP.Core.Model;
using maleric.MVP.States;
using System;

namespace maleric.MVP.Samples
{
	public class MainMenuModel : IModel
	{
		public event Action OnModelChange;

		public void Dispose() { }
	}
}
