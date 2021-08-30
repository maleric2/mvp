using maleric.MVP.Common;
using System;

namespace maleric.MVP.Service
{
	public interface IService : IServiceDepended
	{
		bool IsServiceReady();

		event Action OnServiceReady;

		IServiceDepended[] GetDependencies();
	}
}
