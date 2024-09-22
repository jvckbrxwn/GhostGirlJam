using Managers;
using ServiceLocator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private PlayerData data;
		[SerializeField] private new Rigidbody2D rigidbody2D;
		[SerializeField] private float speed = 10f;
		[SerializeField] private PlayerVisualController playerVisualController;

		private InputAction moveAction;
		private DummyIntroSystem dummyIntroSystem;

		public bool IsMoving => moveAction.IsInProgress();

		// Start is called before the first frame update
		void Start()
		{
			moveAction = InputSystem.actions.FindAction("Move");
			
			moveAction.performed += MoveActionOnstarted;
			moveAction.canceled += MoveActionOncanceled;

			dummyIntroSystem = ServiceManager.Instance.GetManager<DummyIntroSystem>();
			dummyIntroSystem.IntroWasFinished += OnIntroWasFinished;
			data.MovementState = false;
		}

		private void OnIntroWasFinished()
		{
			data.MovementState = true;
		}

		private void MoveActionOncanceled(InputAction.CallbackContext obj)
		{
			playerVisualController.IdleAnimation();
		}

		private void MoveActionOnstarted(InputAction.CallbackContext obj)
		{
			playerVisualController.MoveAnimation();
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			if (!data.MovementState)
			{
				rigidbody2D.velocity = Vector2.zero;
				return;
			}

			Vector3 moveValue = moveAction.ReadValue<Vector2>();
			rigidbody2D.MovePosition(transform.position + moveValue * (speed * Time.deltaTime));
			if (moveValue.x < 0 && !playerVisualController.GetFlip())
			{
				playerVisualController.Flip(true);
			}

			if (moveValue.x > 0 && playerVisualController.GetFlip())
			{
				playerVisualController.Flip(false);
			}
		}

		public void SetMovementState(bool state)
		{
			data.MovementState = state;
		}
	}
}