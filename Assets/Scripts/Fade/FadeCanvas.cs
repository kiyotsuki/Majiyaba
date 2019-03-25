using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class FadeCanvas : ManagerBase
	{
		public void Awake()
		{
			// 最初は黒幕非表示
			image.gameObject.SetActive(false);
		}

		public void Update()
		{
			switch (fadeState)
			{
				case State.FadeIn:
				case State.FadeOut:
					{
						timeCounter += Time.deltaTime;
						float rate = Mathf.Min(1, (timeCounter / fadeTime));

						if (fadeState == State.FadeOut)
						{
							color.a = rate;
						}
						else
						{
							color.a = 1.0f - rate;
						}

						image.color = color;
						if (rate == 1)
						{
							if (fadeState == State.FadeOut)
							{
								fadeState = State.Coverd;
							}
							else
							{
								fadeState = State.None;
								image.gameObject.SetActive(false);
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
			// 真っ黒に
			image.gameObject.SetActive(true);
			image.color = new Color(0, 0, 0, 1);

			fadeState = State.Coverd;
		}

		
		/// <summary>
		/// フェードの開始
		/// </summary>
		/// <param name="state"></param>
		/// <param name="time"></param>
		public void StartFade(State state, float time)
		{
			if (state == State.FadeOut)
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
			image.gameObject.SetActive(true);
		}

		public State GetState()
		{
			return fadeState;
		}
		
		public enum State
		{
			None,
			FadeIn,
			FadeOut,
			Coverd,
		}
		private State fadeState = State.None;
		private Color color;
		private float fadeTime = 0;
		private float timeCounter = 0;
		
		[SerializeField]
		private Image image = null;
	}
}