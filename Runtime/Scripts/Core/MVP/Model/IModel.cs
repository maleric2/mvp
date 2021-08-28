using System;

namespace maleric.Core.MVP.Model
{
	public interface IModel : IDisposable
	{
		event Action OnModelChange;
	}
}