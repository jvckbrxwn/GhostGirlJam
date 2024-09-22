using System;
using Managers;
using ServiceLocator;
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
					ghostAnimation.AnimationState.SetAnimation(0, "out", false).Complete += _ =>
					{
						ghostAnimation.gameObject.SetActive(false);
					};
					animation.gameObject.SetActive(true);
					var anim = playerManager.Movement.IsMoving ? "walk" : "idle";
					animation.AnimationState.SetAnimation(0, anim, false);
					StateChanged?.Invoke(animation);
					break;
				case PlayerStateType.Ghost:
					animation.gameObject.SetActive(false);
					ghostAnimation.gameObject.SetActive(true);
					ghostAnimation.AnimationState.SetAnimation(0, "in", false);
					anim = playerManager.Movement.IsMoving ? "fly" : "idle";
					ghostAnimation.AnimationState.AddAnimation(0, anim, false, 0);
					StateChanged?.Invoke(ghostAnimation);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
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

		public void IdleAnimation()
		{
			if (animation.gameObject.activeSelf)
			{
				animation.AnimationState.SetAnimation(0, "idle", true);
				animation.AnimationState.TimeScale = speedIdle;
			}

			if (ghostAnimation.gameObject.activeSelf)
			{
				ghostAnimation.AnimationState.SetAnimation(0, "idle", true);
				ghostAnimation.AnimationState.TimeScale = speedIdle;
			}
		}
	}
}