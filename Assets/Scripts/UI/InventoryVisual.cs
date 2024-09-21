using System.Collections.Generic;
using System.Linq;
using InteractableObjects.Base;
using Managers;
using ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.Room.UI
{
	public class InventoryVisual : MonoBehaviour
	{
		[SerializeField] private List<Image> items;

		private InventoryManager inventoryManager;
		private DummyDatabaseManager dummyDatabaseManager;

		private void Start()
		{
			inventoryManager = ServiceManager.Instance.GetManager<InventoryManager>();
			dummyDatabaseManager = ServiceManager.Instance.GetManager<DummyDatabaseManager>();
			
			inventoryManager.ItemAdded += SetInventoryVisuals;
			inventoryManager.ItemUsed += SetInventoryVisuals;
			
			items.ForEach(i => i.gameObject.SetActive(false));
		}

		private void SetInventoryVisuals(IPickupable pickupable1)
		{
			items.ForEach(i => i.gameObject.SetActive(false));
			int i = 0;
			foreach (PickUpItemData data in inventoryManager.Pickupables.Select(pickupable =>
				         dummyDatabaseManager.GetItemData(pickupable.Key)))
			{
				items[i].sprite = data.Data.Sprite;
				items[i].gameObject.SetActive(true);
				i++;
			}
		}
	}
}