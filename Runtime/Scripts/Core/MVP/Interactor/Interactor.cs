using maleric.Core.MVP.View;
using System;
using UnityEngine;

namespace maleric.Core.MVP.Model
{
	public abstract class AInteractor
	{
		public abstract void LinkView(object viewObject, bool updateModel = true);

		public abstract void UnlinkView();

		protected abstract void UpdateModel();

		public abstract Type GetModelType();
	}

	/// <summary>
	/// Connects Model with View.
	/// Listens for Model change, and calls OnModelChange inside the View.
	/// </summary>
	/// <typeparam name="T">IModel</typeparam>
	public class Interactor<T> : AInteractor where T : IModel
	{
		protected IView<T> _view;
		protected T _observableData;
		protected Func<T> _onFetchData;

		public Interactor(Func<T> onFetchData)
		{
			_onFetchData = onFetchData;
		}

		public override Type GetModelType()
		{
			return typeof(T);
		}

		public virtual void LinkView(IView<T> view, bool updateModel = true)
		{
			_view = view;
			_observableData = _onFetchData();
			if (updateModel)
				UpdateModel();
			_observableData.OnModelChange += UpdateModel;
		}

		public override void LinkView(object viewObject, bool updateModel = true)
		{
			if (viewObject is IView<T>)
			{
				LinkView((IView<T>)viewObject, updateModel);
			}
			else
			{
				Debug.LogError("Interactor: Unable To Link View, Wrong Type!");
			}
		}

		public override void UnlinkView()
		{
			_view = null;
		}

		protected override void UpdateModel()
		{
			if (_view != null && _observableData != null)
				_view.OnModelChange(_observableData);
		}
	}

	/// <summary>
	/// Extendable version that is meant for more complex scenarios
	/// Extension contains logic when View should be updated.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ExtendableInteractor<T> : AInteractor
	{
		private IView<T> _view;
		private T _data;

		// Example
		/*public ExtendableInteractor(EconomyService service)
		{
			service.OnEconomyUpdated += UpdateModel;
		}
		*/

		public void LinkView(IView<T> view, bool updateModel = true)
		{
			_view = view;
			if (updateModel)
				UpdateModel();
		}

		public override void LinkView(object viewObject, bool updateModel = true)
		{
			if (viewObject is IView<T>)
			{
				LinkView((IView<T>)viewObject, updateModel);
			}
			else
			{
				Debug.LogError("Interactor: Unable To Link View, Wrong Type!");
			}
		}

		public override void UnlinkView()
		{
			_view = null;
		}

		public abstract T FetchData();

		protected override void UpdateModel()
		{
			_data = FetchData();
			if (_view != null && _data != null)
				_view.OnModelChange(_data);
		}

		public override Type GetModelType()
		{
			return typeof(T);
		}
	}
}