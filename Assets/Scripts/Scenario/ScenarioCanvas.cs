using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class ScenarioCanvas : ManagerBase
	{
		public void Awake()
		{
			// 最初は非表示
			talkWindow.Hide();
		}

		public void Update()
		{

		}

		public TalkWindow GetTalkWindow()
		{
			return talkWindow;
		}
		
		[SerializeField]
		private TalkWindow talkWindow = null;
	}
}