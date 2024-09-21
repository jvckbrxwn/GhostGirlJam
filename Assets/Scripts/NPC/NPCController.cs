using System.Collections.Generic;
using System.Linq;
using InteractableObjects.Base;
using Objects.Room.PickUpObjects;
using Spine.Unity;
using UnityEngine;

namespace Objects.Room.NPC
{
	public class NPCController : BaseInteractableComponent
	{
		[SerializeField] private SkeletonAnimation skeletonAnimation;
		
		[Header("Items that we have to give to the ghost")]
		[SerializeField] private List<PickUpType> pickUpItems;

		[Header("Items that ghost give us after quest compition")]
		[SerializeField] private List<PickUpType> itemsToGive;
		
		[field: SerializeField] public bool FinishedQuest { get; private set; } = false;

		public override void Interact()
		{
			if (FinishedQuest)
			{
				Debug.Log("Show tooltip with love");
				return;
			}

			bool itemsIsPresent = pickUpItems.All(item => inventoryManager.HasItem(item));

			if (!itemsIsPresent)
			{
				Debug.Log("Show tooltip with items that npc need");
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
			}
		}
	}
}