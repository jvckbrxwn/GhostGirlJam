using InteractableObjects.Base;
using Managers;
using Objects.Room.NPC;
using Objects.Room.PickUpObjects;

namespace InteractableObjects
{
	public class SafeItemController : BaseInteractableComponent
	{
		private bool finallySolved = false;

		private DummyQuestSystem dummyQuestSystem;

		public override void Interact()
		{
			if (finallySolved)
			{
				//програвати звук, який типу забороняє уже дивитись туди
				return;
			}

			if (!inventoryManager.IsPasswordComplete())
			{
				tooltipManager.ShowItemTooltip(transform, "Потрібно знайти комбінацію");
				return;
			}
			
			if (!dummyQuestSystem.IsQuestReady(NPCType.Maid))
			{
				tooltipManager.ShowItemTooltip(transform, "Покоївка не дозволяє відкрити сейф");
				return;
			}

			finallySolved = true;
			inventoryManager.AddItem(PickUpType.StorageKey, default);
		}
	}
}