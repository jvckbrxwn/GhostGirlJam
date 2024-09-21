using System.Collections.Generic;
using System.Linq;
using Objects.Room.NPC;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;

namespace Managers
{
	public class DummyQuestSystem : MonoBehaviour, IManager
	{
		[SerializeField] private List<NPCController> npcs;

		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		public bool IsQuestReady(NPCType type)
		{
			var npc = npcs.FirstOrDefault(x => x.Type == type);
			return npc != null && npc.FinishedQuest;
		}
	}
}