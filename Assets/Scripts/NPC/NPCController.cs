using System.Collections.Generic;
using System.Linq;
using InteractableObjects.Base;
using Managers;
using Objects.Room.PickUpObjects;
using ServiceLocator;
using Spine.Unity;
using UnityEngine;

namespace Objects.Room.NPC
{
	public class NPCController : BaseInteractableComponent
	{
		[SerializeField] private SkeletonAnimation skeletonAnimation;

		[Header("Items that we have to give to the ghost")]
		[SerializeField] private List<PickUpType> pickUpItems;

		[Header("Items that ghost give us after quest completion")]
		[SerializeField] private List<PickUpType> itemsToGive;

		[field: SerializeField] public bool FinishedQuest { get; private set; } = false;

		private DummyDatabaseManager dummyDatabaseManager;

		protected override void Start()
		{
			base.Start();
			dummyDatabaseManager = ServiceManager.Instance.GetManager<DummyDatabaseManager>();
		}

		public override void Interact()
		{
			if (FinishedQuest)
			{
				Debug.Log("Show tooltip with love");
				tooltipManager.ShowHeartTooltip(transform);
				return;
			}

			bool itemsIsPresent = pickUpItems.All(item => inventoryManager.HasItem(item));

			if (!itemsIsPresent)
			{
				Debug.Log("Show tooltip with items that npc need");
				List<Sprite> sprites = pickUpItems.Select(item => dummyDatabaseManager.GetItemData(item).Data.Sprite).ToList();
				tooltipManager.ShowItemTooltip(transform, sprites);
			}
			else
			{
				foreach (PickUpType item in pickUpItems)
				{
					inventoryManager.UseItem(item);
				}

				FinishedQuest = true;
				Debug.Log("Quest was finished");
				Debug.Log("Show tooltip with love");
				
				tooltipManager.ShowHeartTooltip(transform);

				foreach (PickUpType type in itemsToGive)
				{
					inventoryManager.AddItem(type, default);
				}
			}
		}
	}
}