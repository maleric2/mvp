using UnityEngine;
using UnityEngine.UI;

namespace maleric.Core.MVP.View
{
	[System.Serializable]
	public struct CanvasControlGroup
	{
		public Canvas Canvas;
		public CanvasGroup Group;
		public GraphicRaycaster Raycaster;

		public void AutoFill(GameObject gameObject)
		{
			Canvas = gameObject.GetComponent<Canvas>();
			Group = gameObject.GetComponent<CanvasGroup>();
			Raycaster = gameObject.GetComponent<GraphicRaycaster>();
		}
	}

	/// <summary>
	/// Simple View for UI Canvas Type of View
	/// As View must be unique, extend this View to use it
	/// Implement IModelView<IModel> or IView<IModel> to connect with Model
	/// </summary>
	public abstract class UIView : MonoBehaviour, IView, Common.IInteractable
	{
		[SerializeField] protected CanvasControlGroup _canvasControlGroup;
		public bool IsVisible => _canvasControlGroup.Canvas == null || _canvasControlGroup.Canvas.enabled;

		public bool IsInteractable => _canvasControlGroup.Raycaster == null || _canvasControlGroup.Raycaster.enabled;

		public void SetInteractable(bool isInteractable)
		{
			if (_canvasControlGroup.Raycaster) _canvasControlGroup.Raycaster.enabled = isInteractable;
			if (_canvasControlGroup.Group) _canvasControlGroup.Group.interactable = isInteractable;
		}

		public void SetVisible(bool isVisible)
		{
			if (_canvasControlGroup.Canvas) _canvasControlGroup.Canvas.enabled = isVisible;
			if (_canvasControlGroup.Group) _canvasControlGroup.Group.alpha = isVisible ? 1f : 0f;
		}
	}
}