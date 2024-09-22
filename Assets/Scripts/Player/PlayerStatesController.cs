using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player.Movement;
using UnityEngine;
using UnityEngine.UI;

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

		[Space, SerializeField] private Image ghostUIImage;
		[SerializeField] private Image vignette;

		public event Action<PlayerStateType> PlayerStateChanged;

		public PlayerStateType CurrentState { get; private set; }
		public bool CooldownIsActive { get; private set; } = false;

		public PlayerVisualController VisualController => playerVisualController;

		public async UniTask ChangeState(PlayerStateType type)
		{
			CurrentState = type;
			VisualController.ChangeState(type);
			PlayerStateChanged?.Invoke(type);
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
			ghostUIImage.fillAmount = 0;
			vignette.DOFade(1, 0.3f);
			await UniTask.Delay(TimeSpan.FromSeconds(ghostStateDurationInSeconds));
			ChangeState(PlayerStateType.Girl).Forget();
		}

		private IEnumerator UpdateGhostUIImage(float time)
		{
			float t = 0;
			while (time >= t)
			{
				ghostUIImage.fillAmount = t / time;
				t += Time.deltaTime;
				yield return 0;
			}
		}

		private async UniTask ExecuteGirlState()
		{
			vignette.DOFade(0, 0.3f);
			StartCoroutine(UpdateGhostUIImage(ghostStateCooldownInSeconds));
			await UniTask.Delay(TimeSpan.FromSeconds(ghostStateCooldownInSeconds));
			CooldownIsActive = false;
		}
	}
}