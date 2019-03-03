using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class FadeManager : ManagerBase
	{
		public override IEnumerator OnInitialize()
		{
			
			var req = Resources.LoadAsync<GameObject>("UI/fade_canvas");
			yield return new WaitUntil(() =>
			{
				return req.isDone;
			});
			var canvasPref = req.asset as GameObject;
			fadeCanvas = GameObject.Instantiate(canvasPref);

			fadeCanvas.transform.SetParent(gameObject.transform);
			fadeImage = fadeCanvas.GetComponentInChildren<Image>();
			raycaster = fadeCanvas.GetComponentInChildren<GraphicRaycaster>();

			ForceFadeOut();
			Ready = true;
			yield break;
		}

		public void Update()
		{
			if(fadeCanvas == null)
			{
				return;
			}

			switch (fadeState)
			{
				case FadeState.FadeIn:
				case FadeState.FadeOut:
					{
						timeCounter += Time.deltaTime;
						float rate = Mathf.Min(1, (timeCounter / fadeTime));

						if (fadeState == FadeState.FadeOut)
						{
							color.a = rate;
						}
						else
						{
							color.a = 1.0f - rate;
						}

						fadeImage.color = color;
						if (rate == 1)
						{
							if (fadeState == FadeState.FadeOut)
							{
								fadeState = FadeState.Coverd;
							}
							else
							{
								fadeState = FadeState.None;

								// レイキャストを無効化
								raycaster.enabled = false;
							}
						}
					}
					break;
			}
		}
		
		/// <summary>
		///	強制フェードアウト
		///	即座に真っ暗な状態にする（フェードフラグは操作しない）
		/// </summary>
		public void ForceFadeOut()
		{
			Debug.Log("強制フェードアウト " + fadeFlags.ToString());

			fadeImage.color = new Color(0, 0, 0, 1);

			// レイキャスト遮断開始
			raycaster.enabled = true;

			fadeState = FadeState.Coverd;
		}


		/// <summary>
		/// フェードアウト開始
		/// 最初のフェードアウトのみ実行され、以降はフラグを加算する
		/// </summary>
		/// <param name="flag"></param>
		/// <param name="time"></param>
		public void RequestFadeOut(FadeFlag flag, float time = 0.3f)
		{
			fadeFlags |= flag;
			Debug.Log("フェードアウト " + flag.ToString() + " : " + fadeFlags.ToString());

			if (fadeState == FadeState.FadeOut)
			{
				return;
			}
			if(fadeState == FadeState.Coverd)
			{
				return;
			}
			StartFade(FadeState.FadeOut, time);
		}

		/// <summary>
		/// フェードイン開始
		/// すべてのフラグが下りた最後のフェードインが実行される
		/// </summary>
		/// <param name="flag"></param>
		/// <param name="time"></param>
		public void RequestFadeIn(FadeFlag flag, float time = 0.3f)
		{
			fadeFlags &= ~flag;
			Debug.Log("フェードイン " + flag.ToString() + " : " + fadeFlags.ToString());
			if (fadeFlags != FadeFlag.None)
			{
				return;
			}
			if(fadeState == FadeState.None)
			{
				return;
			}
			StartFade(FadeState.FadeIn, time);
		}

		/// <summary>
		/// フェードの実態
		/// </summary>
		/// <param name="state"></param>
		/// <param name="time"></param>
		private void StartFade(FadeState state, float time)
		{
			if (state == FadeState.FadeOut)
			{
				color = new Color(0, 0, 0, 1);
			}
			else
			{
				color = new Color(0, 0, 0, 0);
			}
			fadeTime = time;
			timeCounter = 0;
			fadeState = state;

			// レイキャスト遮断開始
			raycaster.enabled = true;
		}

		public bool IsCoverd()
		{
			return fadeState == FadeState.Coverd;
		}

		public bool IsNone()
		{
			return fadeState == FadeState.None;
		}

		private enum FadeState
		{
			None,
			FadeIn,
			FadeOut,
			Coverd,
		}
		private FadeState fadeState = FadeState.None;
		private Color color;
		private float fadeTime = 0;
		private float timeCounter = 0;

		[System.Flags]
		public enum FadeFlag
		{
			None	= 0,
			System	= 1,
			Scene	= 2,
			Event	= 4,
		}
		private FadeFlag fadeFlags = FadeFlag.None;
		
		private GameObject fadeCanvas = null;
		private GraphicRaycaster raycaster = null;
		private Image fadeImage = null;
	}
}