using System;

namespace maleric.Core.Common
{
	public interface IServiceDepended
	{
		bool IsReadyForService { get; }
		event Action OnReadyForServiceChange;
	}
}
