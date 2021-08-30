using System;

namespace maleric.MVP.Core.Model
{
	public interface IModel : IDisposable
	{
		event Action OnModelChange;
	}
}