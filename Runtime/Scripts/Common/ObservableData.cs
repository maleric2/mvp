using maleric.MVP.Core.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace maleric.MVP.Common
{
	/// <summary>
	/// Interface for the core Observable Data
	/// Invokes OnDataChange Event when we set another (reference) data 
	/// Note: It doesnt know which data is inside and if something inside the class is changed.
	/// </summary>
	/// <typeparam name="T">Type of data</typeparam>
	public interface IObservableData<T>
	{
		T Data { get; }
		event Action<T> OnDataChange;
	}

	/// <summary>
	/// Interface for the Observable List Data. A list with multiple IObservableData
	/// Invokes OnDataChange Event when list or observable inside it was changed 
	/// </summary>
	/// <typeparam name="T">Type of list element</typeparam>
	public interface IObservableListData<T> : IObservableData<List<IObservableData<T>>>
	{
		void Add(ObservableData<T> data);
		void Remove(IObservableData<T> data);
		void RemoveAt(int index);
		T[] GetArray();
		ObservableData<T> GetObservable(int index);
		List<ObservableData<T>> GetObservables();

		event Action<T> OnObservableChange;
	}

	/// <summary>
	/// Concrete Solution with settable Data for Observable Data.
	/// Invokes OnDataChange Event when we set another (reference) data 
	/// Note: It doesnt know which data is inside and if something inside the class is changed.
	/// </summary>
	/// <typeparam name="T">Type of data</typeparam>
	public class ObservableData<T> : IObservableData<T>
	{
		public T Data
		{
			get { return _data; }
			set
			{
				T previousValue = _data;
				_data = value;

				SetupIModelEvents(_data, previousValue);

				DataChange();
			}
		}

		public event Action<T> OnDataChange;

		protected T _data;
		
		/// <summary>
		/// Called on each Data Change. Including IModel change if Data is also an extension of IModel
		/// </summary>
		protected virtual void DataChange()
		{
			OnDataChange?.Invoke(Data);
		}

		/// <summary>
		/// If this is IModel, we also listen for changes inside it
		/// </summary>
		/// <param name="data">Current Data</param>
		/// <param name="previousData">Previous Data</param>
		protected virtual void SetupIModelEvents(T data, T previousData)
		{
			if (data is IModel && !data.Equals(previousData))
			{
				// Unsubscribe
				if (previousData != null) ((IModel)data).OnModelChange -= DataChange;

				// Subscribe
				if (data != null) ((IModel)data).OnModelChange += DataChange;
			}
		}
	}

	/// <summary>
	/// Concrete solution with Settable Data for the Observable List Data. A list with multiple IObservableData
	/// Invokes OnDataChange Event when list or observable inside it was changed 
	/// </summary>
	/// <typeparam name="T">Type of list element</typeparam>
	public class ObservableListData<T> : IObservableListData<T>
	{
		private List<IObservableData<T>> _protectedObservables = new List<IObservableData<T>>();
		private List<ObservableData<T>> _observables = new List<ObservableData<T>>();
		public List<IObservableData<T>> Data => _protectedObservables;

		public virtual void Add(ObservableData<T> data)
		{
			_protectedObservables.Add(data);
			_observables.Add(data);

			DataChange();
			data.OnDataChange += Data_OnDataChange;
		}

		private void Data_OnDataChange(T obj)
		{
			OnObservableChange?.Invoke(obj);
			DataChange();
		}

		public virtual void Remove(IObservableData<T> data)
		{
			DataChange();
			data.OnDataChange -= Data_OnDataChange;

			_protectedObservables.Remove(data);
			_observables.Remove(data as ObservableData<T>);

		}

		public virtual void RemoveAt(int index)
		{
			DataChange();
			_protectedObservables[index].OnDataChange -= Data_OnDataChange;

			_protectedObservables.RemoveAt(index);
			_observables.RemoveAt(index);
		}

		public T[] GetArray()
		{
			T[] dataArray = new T[_protectedObservables.Count];
			for (int i = 0; i < dataArray.Length; i++)
			{
				dataArray[i] = _protectedObservables[i].Data;
			}
			return dataArray;
		}

		public ObservableData<T> GetObservable(int index)
		{
			return _observables[index];
		}

		public List<ObservableData<T>> GetObservables()
		{
			return _observables;
		}

		protected virtual void DataChange()
		{
			OnDataChange?.Invoke(Data);
		}

		public event Action<List<IObservableData<T>>> OnDataChange;
		public event Action<T> OnObservableChange;

		public void Clear()
		{
			Data.Clear();
			DataChange();
		}
	}
}