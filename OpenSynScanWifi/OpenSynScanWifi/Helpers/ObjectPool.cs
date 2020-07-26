using System;
using System.Collections.Concurrent;
using OpenSynScanWifi.Annotations;

namespace OpenSynScanWifi.Helpers
{
	public class ObjectPool<T> : IObjectPool<T>
	{
		[NotNull] private readonly ConcurrentBag<T> _objects;
		[NotNull] private readonly Func<T> _objectGenerator;

		public ObjectPool([NotNull] Func<T> objectGenerator)
		{
			_objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));

			_objects = new ConcurrentBag<T>();
		}

		public ObjectPool([NotNull] Func<T> objectGenerator, int initialCapacity) : this(objectGenerator) 
		{
			for (int i = 0; i <= initialCapacity; i++)
			{
				this._objects.Add(objectGenerator());
			}
		}

		[NotNull]
		public T Get() => _objects.TryTake(out T item) ? item : _objectGenerator();

		public void Return(T item) => _objects.Add(item);
	}
}
