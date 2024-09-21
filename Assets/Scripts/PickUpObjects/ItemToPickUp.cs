using System.Collections.Generic;
using System.Linq;
using System.Text;
using InteractableObjects.Base;
using Managers;
using ServiceLocator;
using UnityEngine;
using PickUpItemData = PickUpObjects.Scriptables.PickUpItemData;

namespace Objects.Room.PickUpObjects
{
	public enum PickUpType
	{
		Chokolade, //шоколад
		Toy, //іграшка
		Sugar, //цукор
		StorageKey, //ключ від складу
		Cookie, //печиво
		Tea, //чай
		Flour, //борошно 
		TeaLeafs, //чайне листя
		Spoon, //ложка
		Pin, //голка
		Thread, //нитка
		Button, //ґудзик
		Eggs, //яйця,
		MeasureCup, //мірний стаканчик
	}

	[RequireComponent(typeof(SpriteRenderer))]
	public class ItemToPickUp : MonoBehaviour, IPickupable, IInteractable
	{
		[SerializeField] private PickUpType type;
		[SerializeField] private PickUpItemData pickUpItemData;

		[SerializeField] private List<PickUpType> requiredItems;

		private TooltipManager tooltipManager;
		private InventoryManager inventoryManager;
		private DummyDatabaseManager dummyDatabaseManager;
		private SpriteRenderer spriteRenderer;

		private string playerTag;
		private bool canInteract;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			tooltipManager = ServiceManager.Instance.GetManager<TooltipManager>();
			inventoryManager = ServiceManager.Instance.GetManager<InventoryManager>();
			dummyDatabaseManager = ServiceManager.Instance.GetManager<DummyDatabaseManager>();
			playerTag = ServiceManager.Instance.GetManager<PlayerManager>().PlayerTag;

			spriteRenderer.sprite = pickUpItemData.Sprite;
		}

		private void Update()
		{
			if (canInteract && Input.GetKeyDown(KeyCode.E))
			{
				PickUp();
			}
		}

		public void PickUp()
		{
			if (!requiredItems.All(item => inventoryManager.HasItem(item)))
			{
				StringBuilder s = new();
				s.Append(dummyDatabaseManager.GetItemData(type).Data.Hint);
				tooltipManager.ShowItemTooltip(transform, s.ToString());
				return;
			}

			inventoryManager.AddItem(type, MemberwiseClone() as IPickupable);
			//for now, it will be destroyable, I think here should be a bug :))
			Destroy(gameObject);
		}

		public void Interact()
		{
			PickUp();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(playerTag))
			{
				canInteract = true;
				tooltipManager.ShowUseTooltip(transform, other.transform, spriteRenderer.bounds.size.magnitude);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(playerTag))
			{
				canInteract = false;
				tooltipManager.HideUseTooltip();
				tooltipManager.HideItemsTooltip();
			}
		}
	}
}