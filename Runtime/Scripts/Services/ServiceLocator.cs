using maleric.MVP.Common;
using System;
using System.Collections.Generic;

namespace maleric.MVP.Service
{
	public interface IServiceLocator
	{
		T Provide<T>() where T : class, IService;
	}
	public class ServiceLocator : IServiceLocator
	{
		public Dictionary<Type, object> Map => _map;
		private readonly Dictionary<Type, object> _map = new Dictionary<Type, object>();

		public void Register<T>(T service) where T : class
		{
			_map.Add(typeof(T), service);
		}

		public void AddOrReplace<T>(T service) where T : class
		{
			if (_map.TryGetValue(typeof(T), out var existing))
			{
				_map[typeof(T)] = service;
			}
			else
			{
				_map.Add(typeof(T), service);
			}
		}

		public T Provide<T>() where T : class, IService
		{
			object inst = null;
			_map.TryGetValue(typeof(T), out inst);

			return (T)inst;
		}

		public List<T> ProvideAllOfType<T>()
		{
			List<T> providedInstances = new List<T>();
			foreach (var kvp in _map)
			{
				if (kvp.Value is T)
				{
					providedInstances.Add((T)kvp.Value);
				}
			}
			return providedInstances;
		}

	}
}