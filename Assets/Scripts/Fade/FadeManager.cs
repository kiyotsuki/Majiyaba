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
			_fadeCanvas = GameObject.Instantiate(canvasPref);

			_fadeCanvas.transform.SetParent(gameObject.transform);
			_fadeImage = _fadeCanvas.GetComponentInChildren<Image>();
			_raycaster = _fadeCanvas.GetComponentInChildren<GraphicRaycaster>();

			ForceFadeOut();
			Ready = true;
			yield break;
		}

		public void Update()
		{
			if(_fadeCanvas == null)
			{
				return;
			}

			switch (_fadeState)
			{
				case FadeState.FadeIn:
				case FadeState.FadeOut:
					{
						_timeCounter += Time.deltaTime;
						float rate = Mathf.Min(1, (_timeCounter / _fadeTime));

						if (_fadeState == FadeState.FadeOut)
						{
							_color.a = rate;
						}
						else
						{
							_color.a = 1.0f - rate;
						}

						_fadeImage.color = _color;
						if (rate == 1)
						{
							if (_fadeState == FadeState.FadeOut)
							{
								_fadeState = FadeState.Coverd;
							}
							else
							{
								_fadeState = FadeState.None;

								// レイキャストを無効化
								_raycaster.enabled = false;
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
			Debug.Log("強制フェードアウト " + _fadeFlags.ToString());

			_fadeImage.color = new Color(0, 0, 0, 1);

			// レイキャスト遮断開始
			_raycaster.enabled = true;

			_fadeState = FadeState.Coverd;
		}


		/// <summary>
		/// フェードアウト開始
		/// 最初のフェードアウトのみ実行され、以降はフラグを加算する
		/// </summary>
		/// <param name="flag"></param>
		/// <param name="time"></param>
		public void RequestFadeOut(FadeFlag flag, float time = 0.3f)
		{
			_fadeFlags |= flag;
			Debug.Log("フェードアウト " + flag.ToString() + " : " + _fadeFlags.ToString());

			if (_fadeState == FadeState.FadeOut)
			{
				return;
			}
			if(_fadeState == FadeState.Coverd)
			{
				return;
			}
			startFade(FadeState.FadeOut, time);
		}

		/// <summary>
		/// フェードイン開始
		/// すべてのフラグが下りた最後のフェードインが実行される
		/// </summary>
		/// <param name="flag"></param>
		/// <param name="time"></param>
		public void RequestFadeIn(FadeFlag flag, float time = 0.3f)
		{
			_fadeFlags &= ~flag;
			Debug.Log("フェードイン " + flag.ToString() + " : " + _fadeFlags.ToString());
			if (_fadeFlags != FadeFlag.None)
			{
				return;
			}
			if(_fadeState == FadeState.None)
			{
				return;
			}
			startFade(FadeState.FadeIn, time);
		}

		/// <summary>
		/// フェードの実態
		/// </summary>
		/// <param name="state"></param>
		/// <param name="time"></param>
		private void startFade(FadeState state, float time)
		{
			if (state == FadeState.FadeOut)
			{
				_color = new Color(0, 0, 0, 1);
			}
			else
			{
				_color = new Color(0, 0, 0, 0);
			}
			_fadeTime = time;
			_timeCounter = 0;
			_fadeState = state;

			// レイキャスト遮断開始
			_raycaster.enabled = true;
		}

		public bool IsCoverd()
		{
			return _fadeState == FadeState.Coverd;
		}

		public bool IsNone()
		{
			return _fadeState == FadeState.None;
		}

		private enum FadeState
		{
			None,
			FadeIn,
			FadeOut,
			Coverd,
		}
		private FadeState _fadeState = FadeState.None;
		private Color _color;
		private float _fadeTime = 0;
		private float _timeCounter = 0;

		[System.Flags]
		public enum FadeFlag
		{
			None	= 0,
			System	= 1,
			Scene	= 2,
			Event	= 4,
		}
		private FadeFlag _fadeFlags = FadeFlag.None;
		
		private GameObject _fadeCanvas = null;
		private GraphicRaycaster _raycaster = null;
		private Image _fadeImage = null;
	}
}