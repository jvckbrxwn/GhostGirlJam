using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;

namespace Managers
{
	public class TooltipManager : MonoBehaviour, IManager
	{
		[SerializeField] private GameObject useTooltip;

		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		public void ShowUseTooltip(Transform position, Transform otherPosition, float offset)
		{
			Vector3 forward = position.TransformDirection(Vector3.right);
			Vector3 toOther = Vector3.Normalize(otherPosition.position - position.position);
			Vector3 direction = Vector3.Dot(forward, toOther) < 0 ? Vector3.right : Vector3.left;

			useTooltip.transform.position = position.position + direction * offset;
			useTooltip.SetActive(true);
		}

		public void HideUseTooltip()
		{
			useTooltip.SetActive(false);
		}
	}
}