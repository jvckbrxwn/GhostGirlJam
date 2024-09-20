using System;
using Cysharp.Threading.Tasks;
using InteractableObjects;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;

namespace Managers
{
	public class TransitionManager : MonoBehaviour, IManager
	{
		public event Action BeforeInteract;
		public event Action AfterInteract;
		
		private CameraManager cameraManager;
		private PlayerManager playerManager;
		
		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			Debug.Log($"Hello, I'm {GetType()}");
			cameraManager = ServiceManager.Instance.GetManager<CameraManager>();
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
		}

		public void MoveTo(Vector3 position)
		{ }

		public void MoveTo(Vector2 position)
		{ }

		public async UniTask MoveTo(DoorComponent door)
		{
			BeforeInteract?.Invoke();
			await MoveToInternal(door);
			AfterInteract?.Invoke();
		}

		private async UniTask MoveToInternal(DoorComponent door)
		{
			await cameraManager.Fade(to: 1);
			playerManager.TranslateTo(door);
			await cameraManager.Fade(to: 0);
		}
	}
}