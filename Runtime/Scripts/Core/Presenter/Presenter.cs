using maleric.MVP.Common;
using maleric.MVP.Core.Model;
using maleric.MVP.States;
using System;

namespace maleric.MVP.Core.Presenter
{
	public abstract class Presenter : IBackable, IVisible, IInteractable
	{
		public virtual bool IsBackable => false;

		public bool IsVisible => _isVisible;

		public bool IsInteractable => _isInteractable;

		protected bool _isVisible;
		protected bool _isInteractable;

		/// <summary>
		/// This presenter is inside the state
		/// </summary>
		protected IState _state;

		public virtual void Back() { }

		/// <summary>
		/// Constructor. Use State to parent State Views together
		/// </summary>
		/// <param name="state">This state is owner of this presenter</param>
		public Presenter(IState state) { _state = state; }

		/// <summary>
		/// Call to setup Presenter and Collect References
		/// NOTE: Setup is usually called from OnStateEnter
		/// </summary>
		public void Setup()
		{
			CollectReferences();
			InternalSetup();
		}

		/// <summary>
		/// Call to unsetup Presenter and Dispose References
		/// NOTE: Unsetup is usually called from OnStateEnter
		/// </summary>
		public void Unsetup()
		{
			InternalUnsetup();
			DisposeOfReferences();
		}

		/// <summary>
		/// Sets Presenter Views To Visible and Interactable
		/// NOTE: Usually called from OnStateEnter or OnStateExit
		/// </summary>
		/// <param name="isVisible"></param>
		public void SetVisible(bool isVisible)
		{
			InternalSetVisible(_isVisible = isVisible);
			SetInteractable(isVisible);
		}

		/// <summary>
		/// Set only Interactable
		/// </summary>
		/// <param name="isInteractable"></param>
		public void SetInteractable(bool isInteractable)
		{
			InternalSetInteractable(_isInteractable = isInteractable);
		}

		protected abstract void InternalSetup();
		protected abstract void InternalUnsetup();

		protected abstract void CollectReferences();
		protected abstract void DisposeOfReferences();

		protected abstract void InternalSetVisible(bool isVisible);
		protected abstract void InternalSetInteractable(bool isInteractable);
	}

	/// <summary>
	/// Simple Presenter that connects to one View and handles visibility
	/// </summary>
	/// <typeparam name="TView">View</typeparam>
	public class Presenter<TView> : Presenter where TView : UnityEngine.Component, View.IView
	{
		protected TView _view;

		public Presenter(IState state) : base(state) { }

		protected override void CollectReferences()
		{
			_view = UnityEngine.GameObject.FindObjectOfType<TView>();
			if (_view) _view.gameObject.transform.SetParent(_state.GetStateGameObject().transform);
		}

		protected override void DisposeOfReferences()
		{
			_view = null;
		}

		protected override void InternalSetup() { }

		protected override void InternalUnsetup() { }

		protected override void InternalSetVisible(bool isVisible)
		{
			if (_view is IVisible view) view.SetVisible(isVisible);
		}

		protected override void InternalSetInteractable(bool isInteractable)
		{
			if (_view is IInteractable view) view.SetInteractable(isInteractable);
		}
	}

	/// <summary>
	/// Simple presenter that connects with one View and sets up Model for it
	/// Created Model Data is default or null by default
	/// </summary>
	/// <typeparam name="TView">View</typeparam>
	/// <typeparam name="TModel">Model</typeparam>
	public class Presenter<TView, TModel> : Presenter<TView> where TModel : IModel where TView : UnityEngine.Component, View.IView
	{
		private Interactor<TModel> _interactor;

		public Presenter(IState state) : base(state)
		{
			Interactor<TModel> interactor = new Interactor<TModel>(GetModelData);
		}

		protected virtual TModel GetModelData()
		{
			return default(TModel);
		}

		protected override void InternalSetup()
		{
			if (_view) _interactor.LinkView(_view);
		}

		protected override void InternalUnsetup()
		{
			_interactor.UnlinkView();
		}
	}

	/// <summary>
	/// Simple Presenter that have option to spawn a prefab for a View
	/// To spawn a prefab, either we use custom way or we spawn it by path to Resource
	/// </summary>
	public abstract class PrefabPresenter : Presenter
	{
		public PrefabPresenter(IState state) : base(state) { }

		protected TObject CreateView<TObject>(string path) where TObject : UnityEngine.Object, View.IView
		{
			return InstanceExtension.InstantiateResource<TObject>(path, _state.GetStateGameObject());
		}
	}
}