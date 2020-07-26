namespace OpenSynScanWifi.Helpers
{
	public interface IObjectPool<T>
	{
		T Get();
		void Return(T item);
	}
}