using System.Collections.Generic;
using DG.Tweening;
using Objects.Room.UI;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;

namespace Managers
{
	public class TooltipManager : MonoBehaviour, IManager
	{
		[SerializeField] private GameObject useTooltip;
		[SerializeField] private GeneralTooltipController itemsTooltip;
		[SerializeField] private RectTransform heartTooltip;
		[SerializeField] private Canvas actualCanvas;
		
		[Space, SerializeField] private float characterTooltipOffset;
		[SerializeField] private float duration = 0.5f;

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

		public void ShowItemTooltip(Transform transform, List<Sprite> sprites)
		{
			var p = actualCanvas.WorldToCanvasPosition(transform.position);
			p.y -= p.y / 2;
			itemsTooltip.SetSprites(sprites);
			itemsTooltip.gameObject.SetActive(true);
			itemsTooltip.RectTransform.DOAnchorPos(p, duration);
		}
		
		public void ShowItemTooltip(Transform transform, string text)
		{
			var p = actualCanvas.WorldToCanvasPosition(transform.position);
			p.y -= p.y / 2;
			itemsTooltip.SetText(text);
			itemsTooltip.gameObject.SetActive(true);
			itemsTooltip.RectTransform.DOAnchorPos(p, duration);
		}

		public void ShowHeartTooltip(Transform transform)
		{
			var p = actualCanvas.WorldToCanvasPosition(transform.position);
			p.y -= p.y / 2;
			heartTooltip.gameObject.SetActive(true);
			heartTooltip.DOAnchorPos(p, duration);
		}
		
		public void HideUseTooltip()
		{
			useTooltip.SetActive(false);
		}

		public void HideItemsTooltip()
		{
			itemsTooltip.gameObject.SetActive(false);
		}
		
		public void HideHeartTooltip()
		{
			heartTooltip.gameObject.SetActive(false);
		}
	}

	public static class CanvasPositioningExtensions
	{
		public static Vector3 WorldToCanvasPosition(this Canvas canvas, Vector3 worldPosition, Camera camera = null)
		{
			if (camera == null)
			{
				camera = Camera.main;
			}

			var viewportPosition = camera.WorldToViewportPoint(worldPosition);
			return canvas.ViewportToCanvasPosition(viewportPosition);
		}

		public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
		{
			var viewportPosition = new Vector3(screenPosition.x / Screen.width,
				screenPosition.y / Screen.height,
				0);
			return canvas.ViewportToCanvasPosition(viewportPosition);
		}

		public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
		{
			var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
			var canvasRect = canvas.GetComponent<RectTransform>();
			var scale = canvasRect.sizeDelta;
			return Vector3.Scale(centerBasedViewPortPosition, scale);
		}
	}
}