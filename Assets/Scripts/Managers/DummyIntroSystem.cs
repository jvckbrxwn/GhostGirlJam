using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ServiceLocator;
using ServiceLocator.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
	public class DummyIntroSystem : MonoBehaviour, IManager
	{
		[SerializeField] private GameObject imageWithText;
		[SerializeField] private AudioSource speech;
		[Range(0, 1), SerializeField] private float volume;

		public event Action IntroWasFinished;

		public bool IntroIsFinished { get; private set; } = false;

		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		private void Start()
		{
			imageWithText.SetActive(true);
			speech.volume = volume;
			speech.Play();

			WaitForSpeech().Forget();
		}

		private async UniTask WaitForSpeech()
		{
			Debug.Log(speech.clip.length);
			int delay = Mathf.RoundToInt(speech.clip.length * 1000);
			await UniTask.Delay(delay);
			imageWithText.GetComponentInChildren<TMP_Text>().DOFade(0, 0.3f);
			imageWithText.GetComponent<Image>().DOFade(0, 0.3f).onComplete += () => imageWithText.SetActive(false);
			IntroIsFinished = true;
			IntroWasFinished?.Invoke();
		}
	}
}