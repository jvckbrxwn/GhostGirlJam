using System;
using System.Collections.Generic;
using ServiceLocator.Base;

namespace ServiceLocator
{
	//first iteration version
	public class ServiceManager
	{
		private static readonly Dictionary<Type, IManager> managers = new();

		private static ServiceManager locator;

		public static ServiceManager Instance { get { return locator ??= new ServiceManager(); } }

		public void AddManager(IManager manager)
		{
			managers.Add(manager.GetType(), manager);
		}

		public T GetManager<T>() where T : class, IManager
		{
			if (managers.TryGetValue(typeof(T), out IManager manager))
			{
				return manager as T;
			}

			return default;
		}
	}
}