using System;
using Spine.Unity;
using UnityEngine;

namespace Player
{
	public class PlayerVisualController : MonoBehaviour
	{
		[SerializeField] private new SkeletonAnimation animation;
		
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
	}
}