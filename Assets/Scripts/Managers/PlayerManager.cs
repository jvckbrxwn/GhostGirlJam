using Cysharp.Threading.Tasks;
using InteractableObjects;
using InteractableObjects.Base;
using Player;
using Player.Movement;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
	public class PlayerManager : MonoBehaviour, IManager
	{
		[SerializeField] private PlayerData playerData;
		[SerializeField] private PlayerMovement playerMovement;
		[SerializeField] private PlayerStatesController playerStatesController;
		
		private InputAction moveAction;

		private DummyIntroSystem dummyIntroSystem;

		public string PlayerTag => playerData.Tag;
		public PlayerStateType CurrentState => playerStatesController.CurrentState;
		public PlayerStatesController StatesController => playerStatesController;
		public PlayerMovement Movement => playerMovement;
		
		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		private void Start()
		{
			Init();
		}

		private void Update()
		{
			if (!playerStatesController.CooldownIsActive && playerStatesController.CurrentState != PlayerStateType.Ghost && moveAction.IsPressed())
			{
				ChangeState(PlayerStateType.Ghost).Forget();
			}
		}

		private void Init()
		{
			dummyIntroSystem = ServiceManager.Instance.GetManager<DummyIntroSystem>();
			moveAction = InputSystem.actions.FindAction("Ghost");
		}

		public void InteractWith(IInteractable component)
		{
			component.Interact();
		}

		public void SetMovementState(bool state)
		{
			playerMovement.SetMovementState(state);
		}

		public void TranslateTo(DoorComponent door)
		{
			Vector3 newPos = door.transform.position;
			Vector3 p = door.transform.InverseTransformDirection(Vector3.up);
			TranslateTo(newPos - p);
			door.Room.ChangeRoom();
		}

		private async UniTask ChangeState(PlayerStateType type)
		{
			if (!dummyIntroSystem.IntroIsFinished)
			{
				await UniTask.CompletedTask;
			}
			
			await playerStatesController.ChangeState(type);
		}
		
		private void TranslateTo(Vector3 position)
		{
			playerMovement.gameObject.transform.position = position;
		}
	}
}