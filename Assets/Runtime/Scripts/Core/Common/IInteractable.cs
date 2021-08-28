namespace maleric.Core.Common
{
	public interface IInteractable
	{
		public bool IsInteractable { get; }

		public void SetInteractable(bool isInteractable);
	}
}
