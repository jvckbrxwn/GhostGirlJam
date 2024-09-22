using System.Collections.Generic;
using System.Linq;
using InteractableObjects.Base;
using Managers;
using Objects.Room.PickUpObjects;
using Player;
using ServiceLocator;
using Spine.Unity;
using UnityEngine;

namespace Objects.Room.NPC
{
	public enum NPCType
	{
		Maid, Cook, LittleGirl, Butler
	}

	public class NPCController : BaseInteractableComponent
	{
		[SerializeField] private NPCType type;
		[SerializeField] private SkeletonAnimation skeletonAnimation;

		[Header("Items that we have to give to the ghost")]
		[SerializeField] private List<PickUpType> pickUpItems;

		[Header("Items that ghost give us after quest completion")]
		[SerializeField] private List<PickUpType> itemsToGive;

		private DummyDatabaseManager dummyDatabaseManager;

		[field: SerializeField] public bool FinishedQuest { get; private set; } = false;
		public NPCType Type => type;

		protected override void Start()
		{
			base.Start();
			dummyDatabaseManager = ServiceManager.Instance.GetManager<DummyDatabaseManager>();
			skeletonAnimation?.AnimationState.SetAnimation(0, "idle", true);
		}

		protected override void Update()
		{
			base.Update();
			FlipToPlayerPosition();
		}

		private void FlipToPlayerPosition()
		{
			if (skeletonAnimation is null)
			{
				return;
			}

			Vector3 forward = transform.TransformDirection(Vector3.right);
			Vector3 toOther = Vector3.Normalize(playerManager.Movement.transform.position - transform.position);
			skeletonAnimation.Skeleton.FlipX = Vector3.Dot(forward, toOther) < 0;
		}

		public override void Interact()
		{
			if (FinishedQuest)
			{
				Debug.Log("Show tooltip with love");
				tooltipManager.ShowHeartTooltip(transform);
				return;
			}

			if (playerManager.StatesController.CurrentState == PlayerStateType.Girl)
			{
				tooltipManager.ShowItemTooltip(transform, "Привид відмовляється говорити з вами у людській подобі");
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
				tooltipManager.ShowHeartTooltip(transform);
				if (type != NPCType.Cook && type != NPCType.LittleGirl)
				{
					skeletonAnimation?.AnimationState.SetAnimation(0, "idle_2", true);
				}

				foreach (PickUpType type in itemsToGive)
				{
					inventoryManager.AddItem(type, default);
				}
			}
		}
	}
}