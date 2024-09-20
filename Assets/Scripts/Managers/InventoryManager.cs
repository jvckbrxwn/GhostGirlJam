using System;
using System.Collections.Generic;
using InteractableObjects.Base;
using ServiceLocator.Base;
using UnityEngine;

namespace Managers
{
	//Dummy version of inventory manager
	public class InventoryManager : MonoBehaviour, IManager
	{
		private Dictionary<Type, IPickupable> pickupables = new();

		public void AddItem(IPickupable item)
		{
			
		}

		public void UseItem<T>() where T : class, IPickupable
		{
			
		}
	}
}