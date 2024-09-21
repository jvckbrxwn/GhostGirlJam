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
	public class NPCController : MonoBehaviour, IInteractable
	{
		[SerializeField] private SkeletonAnimation skeletonAnimation;
		[SerializeField] private List<PickUpType> pickUpItems;
		[field: SerializeField] public bool FinishedQuest { get; private set; } = false;

		private PlayerManager playerManager;
		private TooltipManager tooltipManager;
		private InventoryManager inventoryManager;
		private bool canInteract = false;

		private void Start()
		{
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
			tooltipManager = ServiceManager.Instance.GetManager<TooltipManager>();
			inventoryManager = ServiceManager.Instance.GetManager<InventoryManager>();
			//skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
		}

		private void Update()
		{
			if (canInteract && Input.GetKeyDown(KeyCode.E))
			{
				playerManager.InteractWith(this);
				tooltipManager.HideUseTooltip();
				canInteract = false;
			}
		}

		public void Interact()
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

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(playerManager.PlayerTag))
			{
				canInteract = true;
				tooltipManager.ShowUseTooltip(transform, other.transform, 0.5f);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(playerManager.PlayerTag))
			{
				canInteract = false;
				tooltipManager.HideUseTooltip();
			}
		}
	}
}