
namespace maleric.Core.MVP.View
{
	/// <summary>
	/// Interface for View
	/// </summary>
	public interface IView : Common.IVisible
	{

	}

	/// <summary>
	/// Interface for View that use Model
	/// Model Example: PlayerDataModel, float, Vector3
	/// </summary>
	/// <typeparam name="T">Data Model</typeparam>
	public interface IView<T> : IView
	{
		void OnModelChange(T data);
	}

	/// <summary>
	/// Interface for View that use Model
	/// </summary>
	/// <typeparam name="T">IModel</typeparam>
	public interface IModelView<T> : IView<T> where T : Model.IModel
	{
		new void OnModelChange(T data);
	}
}