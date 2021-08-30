using UnityEngine;

namespace maleric.MVP.Core.View
{
	/// <summary>
	/// Simple View that is only GameObject
	/// As View must be unique, extend this View to use it
	/// Implement IModelView<IModel> or IView<IModel> to connect with Model
	/// </summary>
	public abstract class GameObjectView : MonoBehaviour, IView
	{
		public bool IsVisible => _go.activeSelf;

		private GameObject _go;

		private void Awake()
		{
			_go = gameObject;
		}

		public void SetVisible(bool isVisible)
		{
			_go.SetActive(isVisible);
		}
	}
}