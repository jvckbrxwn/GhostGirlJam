using System;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerVisualController : MonoBehaviour
	{
		[SerializeField] private new SkeletonAnimation animation;

		[Space, SerializeField] private float speedIdle = 1;
		[SerializeField] private float speedWalk = 1;
		
		public void ChangeState(PlayerStateType type)
		{
			Debug.Log($"Try to switch to {type}");
			switch (type)
			{
				case PlayerStateType.Girl:
					//animation.AnimationState.SetAnimation(0, "girl", false);
					break;
				case PlayerStateType.Ghost:
					//animation.AnimationState.SetAnimation(0, "ghost", true);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public void Flip(bool x)
		{
			animation.Skeleton.FlipX = x;
		}

		public bool GetFlip()
		{
			return animation.Skeleton.FlipX;
		}

		public void MoveAnimation()
		{
			animation.AnimationState.SetAnimation(0, "walk", true);
			animation.AnimationState.TimeScale = speedWalk;
		}
		
		public void IdleAnimation()
		{
			animation.AnimationState.SetAnimation(0, "idle", true);
			animation.AnimationState.TimeScale = speedIdle;
		}
	}
}