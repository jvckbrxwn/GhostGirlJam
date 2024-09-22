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
		[SerializeField] private GameObject rulesGameObject;
		[SerializeField] private AudioSource speech;
		[SerializeField] private AudioSource mainAudio;
		[Range(0, 1), SerializeField] private float volume;
		[Range(0, 1), SerializeField] private float themeVolume;

		public event Action IntroWasFinished;

		public bool IntroIsFinished { get; private set; } = false;
		public bool RulesIsFinished { get; private set; } = false;

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
			CompleteIntro();
			await UniTask.Delay(300, cancellationToken: cancellationToken);
			OpenRules();
			await UniTask.Delay(3000, cancellationToken: cancellationToken);
			CloseRules();
			IntroWasFinished?.Invoke();
		}

		private async UniTask WaitForRules(CancellationToken cancellationToken)
		{
			CompleteIntro();
			await UniTask.Delay(300, cancellationToken: cancellationToken);
			OpenRules();
			await UniTask.Delay(3000, cancellationToken: cancellationToken);
			CloseRules();
			IntroWasFinished?.Invoke();
		}

		private void CloseRules()
		{
			rulesGameObject.GetComponentInChildren<TMP_Text>().DOFade(0, 0.3f);
			imageWithText.GetComponent<Image>().DOFade(0, 0.3f).onComplete+= () =>
			{
				imageWithText.gameObject.SetActive(false);
			};
			
			rulesGameObject.GetComponent<Image>().DOFade(0, 0.3f).onComplete+= () =>
			{
				rulesGameObject.gameObject.SetActive(false);
			};
		}

		private void OpenRules()
		{
			rulesGameObject.SetActive(true);
			rulesGameObject.GetComponentInChildren<TMP_Text>().DOFade(1, 0.3f);
		}

		private void StopSpeech()
		{
			token.Cancel();
			speech.Stop();
			CompleteIntro();
			token = new CancellationTokenSource();

			WaitForRules(token.Token).Forget();
		}

		private void CompleteIntro()
		{
			speech.Stop();
			mainAudio.Play();
			mainAudio.volume = themeVolume;
			imageWithText.GetComponentInChildren<TMP_Text>().DOFade(0, 0.3f);
			IntroIsFinished = true;
		}
	}
}