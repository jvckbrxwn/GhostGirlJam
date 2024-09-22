using System;
using System.Collections;
using Managers;
using ServiceLocator;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerVisualController : MonoBehaviour
	{
		[SerializeField] private new SkeletonAnimation animation;
		[SerializeField] private SkeletonAnimation ghostAnimation;

		[Space, SerializeField] private float speedIdle = 1;
		[SerializeField] private float speedWalk = 1;

		private PlayerManager playerManager;
		private Coroutine coroutine;

		public event Action<SkeletonAnimation> StateChanged;

		private void Start()
		{
			playerManager = ServiceManager.Instance.GetManager<PlayerManager>();
			animation.gameObject.SetActive(true);
			ghostAnimation.gameObject.SetActive(false);
		}

		public void ChangeState(PlayerStateType type)
		{
			Debug.Log($"Try to switch to {type}");
			switch (type)
			{
				case PlayerStateType.Girl:
					ghostAnimation.AnimationState.SetAnimation(0, "out", false).Complete += track =>
					{
						if (coroutine != null)
						{
							StopCoroutine(coroutine);
						}

						coroutine = StartCoroutine(DisableGhost(track));
					};
					animation.gameObject.SetActive(true);
					var anim = playerManager.Movement.IsMoving ? "walk" : "idle";
					animation.AnimationState.SetAnimation(0, anim, true);
					animation.AnimationState.TimeScale = playerManager.Movement.IsMoving ? speedWalk : speedIdle;
					StateChanged?.Invoke(animation);
					break;
				case PlayerStateType.Ghost:
					animation.gameObject.SetActive(false);
					ghostAnimation.gameObject.SetActive(true);
					ghostAnimation.AnimationState.SetAnimation(0, "in", false);
					anim = playerManager.Movement.IsMoving ? "fly" : "idle";
					ghostAnimation.AnimationState.AddAnimation(0, anim, true, 0);
					ghostAnimation.AnimationState.TimeScale = playerManager.Movement.IsMoving ? speedWalk : speedIdle;
					StateChanged?.Invoke(ghostAnimation);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		private IEnumerator DisableGhost(TrackEntry trackEntry)
		{
			yield return new WaitUntil(() => trackEntry.IsComplete);
			ghostAnimation.gameObject.SetActive(false);
		}

		public void Flip(bool x)
		{
			animation.Skeleton.FlipX = x;
			ghostAnimation.Skeleton.FlipX = x;
		}

		public bool GetFlip()
		{
			return animation.Skeleton.FlipX;
		}

		public void MoveAnimation()
		{
			if (animation.gameObject.activeSelf)
			{
				animation.AnimationState.SetAnimation(0, "walk", true);
				animation.AnimationState.TimeScale = speedWalk;
			}

			if (ghostAnimation.gameObject.activeSelf)
			{
				ghostAnimation.AnimationState.SetAnimation(0, "fly", true);
				ghostAnimation.AnimationState.TimeScale = speedWalk;
			}
		}

		public void IdleAnimation(bool loop = true)
		{
			if (animation.gameObject.activeSelf)
			{
				animation.AnimationState.SetAnimation(0, "idle", loop);
				animation.AnimationState.TimeScale = speedIdle;
			}

			if (ghostAnimation.gameObject.activeSelf)
			{
				ghostAnimation.AnimationState.SetAnimation(0, "idle", loop);
				ghostAnimation.AnimationState.TimeScale = speedIdle;
			}
		}
	}
}