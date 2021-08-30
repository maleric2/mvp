using System;

namespace maleric.MVP.Common
{
	public interface IServiceDepended
	{
		bool IsReadyForService { get; }
		event Action OnReadyForServiceChange;
	}
}
