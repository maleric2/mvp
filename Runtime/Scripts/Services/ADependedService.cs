using System;
using System.Text;
using maleric.MVP.Common;
using UnityEngine;

namespace maleric.MVP.Service
{
	/// <summary>
	/// Service that should cover all Dependencies.
	/// At least States and Services are implementing IServiceDepended
	/// Save System should work in the same way where mutually dependency is logged
	/// </summary>
	public abstract class ADependedService : IService
	{
		// Note: Might require Update for Save Depended Services to avoid mutually dependency
		// Note: SaveData used by each Service can be also IServiceDepended

		protected bool _isReady;

		public event Action OnServiceReady;

		public event Action OnReadyForServiceChange;

		public readonly IServiceDepended[] _dependencies;
		public bool IsReadyForService => IsServiceReady();

		public IServiceDepended[] GetDependencies() { return _dependencies; }

		public bool IsServiceReady() { return _isReady; }

		public ADependedService(params IServiceDepended[] dependencies)
		{
			_dependencies = dependencies;
			if (_dependencies != null)
			{
				for (int i = 0; i < _dependencies.Length; i++)
				{
					_dependencies[i].OnReadyForServiceChange += OnDependencyReadyForServiceChange;
#if UNITY_EDITOR
					if (_dependencies[i] is IService serviceDependency) CheckForMutuallyDependencies(serviceDependency);

#endif
				}
			}
		}

		/// <summary>
		/// Example is where all classes are depended of SaveService, but SaveService might be depended on some of them
		/// </summary>
		/// <param name="serviceDependency"></param>
		private void CheckForMutuallyDependencies(IService serviceDependency)
		{
			IServiceDepended[] dependencyDependencies = serviceDependency.GetDependencies();
			if (dependencyDependencies != null)
			{
				for (int i = 0; i < dependencyDependencies.Length; i++)
				{
					if (dependencyDependencies[i] == this)
					{
						StringBuilder errorMessage = new StringBuilder("ADependedService: ");
						errorMessage.Append(this.GetType().FullName).Append(" have mutual dependency with ");
						errorMessage.Append(dependencyDependencies[i].GetType().FullName).Append(" service");
						Debug.LogError(errorMessage);
					}
				}
			}
		}

		private void OnDependencyReadyForServiceChange() { HandleCheckIsReady(); }

		protected void HandleCheckIsReady()
		{
			if (!_isReady)
			{
				_isReady = CheckIsReady();
				if (_isReady)
				{
					ServiceReady();
					OnServiceReady?.Invoke();
					OnReadyForServiceChange?.Invoke();
				}
			}
		}

		protected virtual bool CheckIsReady()
		{
			bool isReady = true;
			if (_dependencies != null)
			{
				for (int i = 0; i < _dependencies.Length; i++)
				{
					isReady = isReady && _dependencies[i].IsReadyForService;
					if (!isReady) break;
				}
			}
			return isReady;
		}

		protected abstract void ServiceReady();
	}
}
