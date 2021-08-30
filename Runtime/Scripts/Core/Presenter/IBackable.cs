namespace maleric.MVP.Core.Presenter
{
	public interface IBackable
	{
		bool IsBackable { get; }
		void Back();
	}
}
