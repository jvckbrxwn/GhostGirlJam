using InteractableObjects.Base;
using Managers;
using Objects.Room.NPC;
using Objects.Room.PickUpObjects;
using ServiceLocator;

namespace InteractableObjects
{
	public class SafeItemController : BaseInteractableComponent
	{
		private bool finallySolved = false;

		private DummyQuestSystem dummyQuestSystem;

		protected override void Start()
		{
			base.Start();
			dummyQuestSystem = ServiceManager.Instance.GetManager<DummyQuestSystem>();
		}

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