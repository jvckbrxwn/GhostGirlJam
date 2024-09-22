using System;
using System.Threading;
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

		private CancellationTokenSource token;
		
		private void Awake()
		{
			token = new CancellationTokenSource();
			ServiceManager.Instance.AddManager(this);
		}

		private void Start()
		{
			WaitForSpeech(token.Token).Forget();
		}

		private void Update()
		{
			if (!IntroIsFinished && Input.GetKeyDown(KeyCode.Space))
			{
				StopSpeech();
			}
		}

		private async UniTask WaitForSpeech(CancellationToken cancellationToken)
		{
			int delay = Mathf.RoundToInt(speech.clip.length * 1000);
			await UniTask.Delay(delay, cancellationToken: cancellationToken);
			imageWithText.GetComponentInChildren<TMP_Text>().DOFade(0, 0.3f);
			imageWithText.GetComponent<Image>().DOFade(0, 0.3f).onComplete += () => imageWithText.SetActive(false);
			IntroIsFinished = true;
			IntroWasFinished?.Invoke();
		}

		private void StopSpeech()
		{
			token.Cancel();
			speech.Stop();
			imageWithText.GetComponentInChildren<TMP_Text>().DOFade(0, 0.3f);
			imageWithText.GetComponent<Image>().DOFade(0, 0.3f).onComplete += () => imageWithText.SetActive(false);
			IntroIsFinished = true;
			IntroWasFinished?.Invoke();
			token = new CancellationTokenSource();
		}
	}
}