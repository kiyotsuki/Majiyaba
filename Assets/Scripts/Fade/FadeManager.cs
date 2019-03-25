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
			var canvas = GameObject.Instantiate(canvasPref);

			canvas.transform.SetParent(gameObject.transform);
			fadeCanvas = canvas.GetComponent<FadeCanvas>();
			ForceFadeOut();

			Ready = true;
			yield break;
		}
		

		/// <summary>
		///	強制フェードアウト
		///	即座に真っ暗な状態にする（フェードフラグは操作しない）
		/// </summary>
		public void ForceFadeOut()
		{
			Debug.Log("強制フェードアウト " + fadeFlags.ToString());
			fadeCanvas.ForceFadeOut();
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

			if (fadeCanvas.GetState() == FadeCanvas.State.FadeOut)
			{
				return;
			}
			if (IsCoverd())
			{
				return;
			}
			fadeCanvas.StartFade(FadeCanvas.State.FadeOut, time);
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
			if (IsNone())
			{
				return;
			}
			fadeCanvas.StartFade(FadeCanvas.State.FadeIn, time);
		}
		

		public bool IsCoverd()
		{
			return fadeCanvas.GetState() == FadeCanvas.State.Coverd;
		}

		public bool IsNone()
		{
			return fadeCanvas.GetState() == FadeCanvas.State.None;
		}
		
		[System.Flags]
		public enum FadeFlag
		{
			None	= 0,
			System	= 1,
			Scene	= 2,
			Event	= 4,
		}
		private FadeFlag fadeFlags = FadeFlag.None;		
		private FadeCanvas fadeCanvas = null;
	}
}