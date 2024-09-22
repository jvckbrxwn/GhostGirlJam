using System.Collections.Generic;
using InteractableObjects.Base;
using Managers;
using Objects.Room.PickUpObjects;
using ServiceLocator;
using UnityEngine;

namespace InteractableObjects
{
	public class FlourObjectController : BaseInteractableComponent
	{
		private bool finallySolved = false;

		private DummyDatabaseManager dummyDatabaseManager;

		protected override void Start()
		{
			base.Start();
			dummyDatabaseManager = ServiceManager.Instance.GetManager<DummyDatabaseManager>();
		}

		public override void Interact()
		{
			if (finallySolved)
			{
				//програвати звук, який типу забороняє уже дивитись туди
				return;
			}

			if (!inventoryManager.HasItem(PickUpType.StorageKey))
			{
				tooltipManager.ShowItemTooltip(transform,
					new List<Sprite> { dummyDatabaseManager.GetItemData(PickUpType.StorageKey).Data.Sprite });
				return;
			}

			if (!inventoryManager.HasItem(PickUpType.MeasureCup))
			{
				tooltipManager.ShowItemTooltip(transform, "Чим би набрати борошно?");
				return;
			}

			inventoryManager.UseItem(PickUpType.MeasureCup);
			inventoryManager.AddItem(PickUpType.Flour, default);
			finallySolved = true;
		}
	}
}