using System;
using System.Collections.Generic;
using InteractableObjects.Base;
using Objects.Room.HiddenObjects;
using Objects.Room.PickUpObjects;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;

namespace Managers
{
	//Dummy version of inventory manager
	public class InventoryManager : MonoBehaviour, IManager
	{
		public event Action<IPickupable> ItemAdded;
		public event Action<IPickupable> ItemUsed;

		private Dictionary<PickUpType, IPickupable> pickupables = new();
		private Dictionary<int, string> password = new();

		public Dictionary<PickUpType, IPickupable> Pickupables => pickupables;

		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		public void AddItem(PickUpType type, IPickupable item)
		{
			pickupables.Add(type, item);
			ItemAdded?.Invoke(item);
		}

		public void UseItem(PickUpType type)
		{
			if (pickupables.ContainsKey(type))
			{
				ItemUsed?.Invoke(pickupables[type]);
				pickupables.Remove(type);
			}
		}

		public bool HasItem(PickUpType type)
		{
			return pickupables.ContainsKey(type);
		}

		public void AddPasswordPart(int key, string value)
		{
			password.TryAdd(key, value);
		}

		public bool IsPasswordComplete()
		{
			HiddenObjectController[] count = FindObjectsByType<HiddenObjectController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			return password.Count == count.Length;
		}
	}
}