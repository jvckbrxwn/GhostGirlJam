using System.Threading.Tasks;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ServiceLocator;
using ServiceLocator.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
	public class CameraManager : MonoBehaviour, IManager
	{
		[SerializeField] private CinemachineConfiner cinemachineConfiner;
		
		[Space, SerializeField] private Image fadeImage;
		[SerializeField] private float fadeDuration;

		private void Awake()
		{
			ServiceManager.Instance.AddManager(this);
		}

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			Debug.Log($"Hello, I'm {GetType()}");
		}

		public async UniTask Fade(float to)
		{
			await fadeImage.DOFade(to, fadeDuration).ToUniTask();
		}

		public void SetCameraConfiner(PolygonCollider2D cameraConfiner)
		{
			cinemachineConfiner.m_BoundingShape2D = cameraConfiner;
		}
	}
}