using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Room.NPC;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
	public class DummyQuestSystem : MonoBehaviour, IManager
	{
		[SerializeField] private List<NPCController> npcs;
		[Space, SerializeField] private Image finishImage;

		public event Action GameIsDone;

		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
			InvokeRepeating(nameof(IsAllQuestsReady), 0f, 5f);
		}

		public bool IsQuestReady(NPCType type)
		{
			var npc = npcs.FirstOrDefault(x => x.Type == type);
			return npc != null && npc.FinishedQuest;
		}

		public void IsAllQuestsReady()
		{
			if (npcs.All(x => x.FinishedQuest))
			{
				CancelInvoke(nameof(IsAllQuestsReady));
				GameIsDone?.Invoke();
			}
		}
	}
}