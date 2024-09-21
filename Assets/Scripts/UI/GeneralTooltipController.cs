using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.Room.UI
{
	public class GeneralTooltipController : MonoBehaviour
	{
		[SerializeField] private GameObject itemsHolder;
		[SerializeField] private List<Image> itemImages;
		[SerializeField] private TMP_Text messageText;
		[SerializeField] private RectTransform rectTransform;

		public RectTransform RectTransform => rectTransform;

		public void SetSprites(List<Sprite> sprites)
		{
			messageText.gameObject.SetActive(false);
			itemImages.ForEach(x=>x.gameObject.SetActive(false));
			for (int i = 0; i < sprites.Count; i++)
			{
				itemImages[i].sprite = sprites[i];
				itemImages[i].gameObject.SetActive(true);
			}
			itemsHolder.SetActive(true);
		}

		public void SetText(string text)
		{
			itemsHolder.SetActive(false);
			messageText.text = text;
			messageText.gameObject.SetActive(true);
		}
	}
}