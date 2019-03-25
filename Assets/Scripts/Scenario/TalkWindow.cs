using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class TalkWindow : ManagerBase
	{
		public void RequestText(string text)
		{
			gameObject.SetActive(true);
			talkText.text = "";
			request = text;
			count = 0;
			nextIcon.SetActive(false);
		}

		private void ShowAllText()
		{
			if (request == null)
			{
				return;
			}
			talkText.text = request;
			request = null;
			count = 0;

			// Nextアイコン表示
			nextIcon.SetActive(true);
		}

		public void OnClick()
		{
			if (request != null)
			{
				ShowAllText();
			}
			else
			{
				Hide();
			}
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public bool IsActive()
		{
			return gameObject.activeSelf;
		}


		public void Update()
		{
			if (request != null)
			{
				time += Time.deltaTime;
				if (time >= textWait)
				{
					time = 0;
					if (request.Length <= count)
					{
						ShowAllText();
					}
					else
					{
						talkText.text += request[count];
						count++;
					}
				}
			}
		}

		private float time = 0;
		private int count = 0;
		private string request = null;

		[SerializeField]
		private float textWait = 0.1f;

		[SerializeField]
		private Text talkText = null;

		[SerializeField]
		private Text nameText = null;

		[SerializeField]
		private GameObject nextIcon = null;
	}
}