using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private PlayerData data;
		[SerializeField] private new Rigidbody2D rigidbody2D;
		[SerializeField] private float speed = 10f;

		private InputAction moveAction;

		// Start is called before the first frame update
		void Start()
		{
			moveAction = InputSystem.actions.FindAction("Move");
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			if (!data.MovementState)
			{
				rigidbody2D.velocity = Vector2.zero;
				return;
			}

			Vector2 moveValue = moveAction.ReadValue<Vector2>();
			rigidbody2D.velocity = moveValue * speed;
		}

		public void SetMovementState(bool state)
		{
			data.MovementState = state;
		}
	}
}