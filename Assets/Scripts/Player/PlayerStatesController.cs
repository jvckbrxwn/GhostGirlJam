using System;
using Cysharp.Threading.Tasks;
using Player.Movement;
using UnityEngine;

namespace Player
{
	public enum PlayerStateType
	{
		Girl, Ghost
	}

	public class PlayerStatesController : MonoBehaviour
	{
		[SerializeField] private PlayerMovement playerMovement;
		[SerializeField] private PlayerVisualController playerVisualController;
		[SerializeField] private int ghostStateDurationInSeconds = 3;
		[SerializeField] private int ghostStateCooldownInSeconds = 10;

		public PlayerStateType CurrentState { get; private set; }
		public bool CooldownIsActive { get; private set; } = false;

		public async UniTask ChangeState(PlayerStateType type)
		{
			CurrentState = type;
			playerVisualController.ChangeState(type);
			switch (type)
			{
				case PlayerStateType.Girl:
					await ExecuteGirlState();
					break;
				case PlayerStateType.Ghost:
					await ExecuteGhostState();
					break;
			}
		}

		private async UniTask ExecuteGhostState()
		{
			CooldownIsActive = true;
			await UniTask.Delay(TimeSpan.FromSeconds(ghostStateDurationInSeconds));
			ChangeState(PlayerStateType.Girl).Forget();
		}
		
		private async UniTask ExecuteGirlState()
		{
			await UniTask.Delay(TimeSpan.FromSeconds(ghostStateCooldownInSeconds));
			CooldownIsActive = false;
		}
	}
}