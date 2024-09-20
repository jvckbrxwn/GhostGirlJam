using System.Collections.Generic;
using System.Linq;
using InteractableObjects;
using Managers;
using ServiceLocator;
using UnityEngine;

namespace Objects.Room
{
	public class RoomComponent : MonoBehaviour
	{
		[SerializeField] private PolygonCollider2D cameraConfiner;
		[SerializeField] private List<DoorComponent> doors;

		private CameraManager cameraManager;

#if UNITY_EDITOR
		private void OnValidate()
		{
			DoorComponent[] ds = GetComponentsInChildren<DoorComponent>();
			if (ds.Length > doors.Count || ds.Length < doors.Count)
			{
				doors = ds.ToList();
			}

			if (cameraConfiner is null)
			{
				cameraConfiner = GetComponent<PolygonCollider2D>();
			}
		}
#endif

		private void Start()
		{
			cameraManager = ServiceManager.Instance.GetManager<CameraManager>();
			foreach (DoorComponent door in doors)
			{
				door.SetRoomComponent(this);
			}
		}

		public void ChangeRoom()
		{
			cameraManager.SetCameraConfiner(cameraConfiner);
		}
	}
}