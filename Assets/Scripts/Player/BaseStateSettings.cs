using System;

namespace Player
{
	[Serializable]
	public abstract class BaseStateSettings
	{
		public float speed;
		public bool canPickup = true;
	}
}