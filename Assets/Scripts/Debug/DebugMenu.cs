using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Majiyaba
{
	public class DebugMenu : MonoBehaviour
	{
		public void Start()
		{
			// 初期は非表示
			itemSource.SetActive(false);
			menuPage.SetActive(false);

			// オープンボタンにコールバック登録
			openButton.GetComponent<Button>().onClick.AddListener(Open);

			// クローズボタンにコールバック登録
			closeButton.GetComponent<Button>().onClick.AddListener(Close);

			// 各ページ登録
			pageList.Add(Page.Top, new DebugPageTop());
			pageList.Add(Page.Scene, new DebugPageScene());
		}

		/// <summary>
		/// デバッグメニューを開く
		/// </summary>
		public void Open()
		{
			menuPage.SetActive(true);
			SetupPage(currentPage);
		}

		/// <summary>
		/// デバッグメニューを閉じる
		/// </summary>
		public void Close()
		{
			menuPage.SetActive(false);
		}

		/// <summary>
		/// デバッグメニューにボタン追加
		/// </summary>
		public void AddButton(string label, UnityAction onClick)
		{
			var buttonObj = GameObject.Instantiate(itemSource);
			buttonObj.SetActive(true);
			buttonObj.transform.SetParent(menuPage.transform);

			var text = buttonObj.GetComponentInChildren<Text>();
			text.text = label;

			var button = buttonObj.GetComponent<Button>();
			button.onClick.AddListener(onClick);

			buttonList.Add(buttonObj);
		}

		/// <summary>
		/// 指定ページに遷移
		/// </summary>
		/// <param name="page"></param>
		public void ToPage(Page page)
		{
			pageHistory.Add(currentPage);
			SetupPage(page);
		}

		/// <summary>
		/// 前のページに戻る
		/// </summary>
		public void BackPage()
		{
			if(pageHistory.Count <= 0)
			{
				Close();
				return;
			}
			var index = pageHistory.Count - 1;
			var page = pageHistory[index];
			pageHistory.RemoveAt(index);

			SetupPage(page);
		}

		/// <summary>
		/// 現在表示されてるボタンを消し、指定されたページを構築
		/// 戻るボタンは最後に自動配置
		/// </summary>
		/// <param name="page"></param>
		private void SetupPage(Page page)
		{
			currentPage = page;

			foreach (var button in buttonList)
			{
				GameObject.Destroy(button);
			}
			buttonList.Clear();

			if(pageList.ContainsKey(page))
			{
				var pageGenerator = pageList[page];
				pageGenerator.Setup(this);
			}
			AddButton("戻る", () => BackPage());
		}


		/// <summary>
		/// デバッグメニュー用ページ基底
		/// ボタンの追加などをこれを継承して行う
		/// </summary>
		public abstract class PageBase
		{
			public abstract void Setup(DebugMenu menu);
		}
		
		// ページ定義
		public enum Page
		{
			Top,
			Scene,
			Quest,
			Scenario,
		}
		private Page currentPage = Page.Top;
		private Dictionary<Page, PageBase> pageList = new Dictionary<Page, PageBase>();
		private List<Page> pageHistory = new List<Page>();
		private List<GameObject> buttonList = new List<GameObject>();

		[SerializeField]
		private GameObject menuPage = null;

		[SerializeField]
		private GameObject openButton = null;

		[SerializeField]
		private GameObject closeButton = null;

		[SerializeField]
		private GameObject itemSource = null;
	}
}