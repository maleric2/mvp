using maleric.Core.Common;
using System;

namespace maleric.Core.Service
{
	public interface IService : IServiceDepended
	{
		bool IsServiceReady();

		event Action OnServiceReady;

		IServiceDepended[] GetDependencies();
	}
}
