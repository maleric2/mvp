namespace maleric.MVP.Common
{
	public interface IInteractable
	{
		public bool IsInteractable { get; }

		public void SetInteractable(bool isInteractable);
	}
}
