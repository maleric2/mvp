namespace maleric.Core.MVP.Presenter
{
	public interface IBackable
	{
		bool IsBackable { get; }
		void Back();
	}
}
