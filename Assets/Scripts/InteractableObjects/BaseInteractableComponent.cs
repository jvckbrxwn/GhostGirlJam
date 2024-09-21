using Managers;
using ServiceLocator;
using UnityEngine;

namespace InteractableObjects.Base
{
	public abstract class BaseInteractableComponent : MonoBehaviour, IInteractable
	{
		protected PlayerManager playerManager;
		protected TooltipManager tooltipManager;
		protected InventoryManager inventoryManager;

		protected bool canInteract = false;

		protected void Start()
		{
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
			tooltipManager = ServiceManager.Instance.GetManager<TooltipManager>();
			inventoryManager = ServiceManager.Instance.GetManager<InventoryManager>();
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

		public abstract void Interact();

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(playerManager.PlayerTag))
			{
				canInteract = true;
				tooltipManager.ShowUseTooltip(transform, other.transform, 1);
			}
		}
		
		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(playerManager.PlayerTag))
			{
				canInteract = false;
				tooltipManager.HideUseTooltip();
				tooltipManager.HideItemsTooltip();
			}
		}
	}
}