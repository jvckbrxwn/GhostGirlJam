using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Room.PickUpObjects;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;

namespace Managers
{
	[Serializable]
	public struct PickUpItemData
	{
		[SerializeField] private PickUpType type;
		[SerializeField] private PickUpObjects.Scriptables.PickUpItemData data;

		public PickUpType Type => type;
		public PickUpObjects.Scriptables.PickUpItemData Data => data;
	}

	public class DummyDatabaseManager : MonoBehaviour, IManager
	{
		[SerializeField] private List<PickUpItemData> datas;

		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		public PickUpItemData GetItemData(PickUpType type)
		{
			return datas.FirstOrDefault(item => item.Type == type);
		}
	}
}