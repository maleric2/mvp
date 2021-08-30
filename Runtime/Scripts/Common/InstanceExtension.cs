using UnityEngine;

namespace maleric.MVP.Common
{
	public static class InstanceExtension
	{
		public static TObject InstantiateResource<TObject>(string path, GameObject parent, bool worldPositionStays = true)
			where TObject : Object
		{
			var loadedBehaviour = Resources.Load<TObject>(path);
			var behaviour = Object.Instantiate(loadedBehaviour, parent.transform, worldPositionStays);
			behaviour.name = loadedBehaviour.name;
			return behaviour;
		}
	}
}