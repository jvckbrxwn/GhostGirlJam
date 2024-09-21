using InteractableObjects.Base;
using Objects.Room.PickUpObjects;

namespace InteractableObjects
{
	public class SafeItemController : BaseInteractableComponent
	{
		private bool finallySolved = false;

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

			finallySolved = true;
			inventoryManager.AddItem(PickUpType.StorageKey, default);
		}
	}
}