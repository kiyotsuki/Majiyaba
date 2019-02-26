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
			_itemSource.SetActive(false);
			_menuPage.SetActive(false);

			// オープンボタンにコールバック登録
			_openButton.GetComponent<Button>().onClick.AddListener(Open);

			// クローズボタンにコールバック登録
			_closeButton.GetComponent<Button>().onClick.AddListener(Close);

			// 各ページ登録
			_pageList.Add(Page.Top, new DebugPageTop());
			_pageList.Add(Page.Scene, new DebugPageScene());
		}

		/// <summary>
		/// デバッグメニューを開く
		/// </summary>
		public void Open()
		{
			_menuPage.SetActive(true);
			setupPage(_currentPage);
		}

		/// <summary>
		/// デバッグメニューを閉じる
		/// </summary>
		public void Close()
		{
			_menuPage.SetActive(false);
		}

		/// <summary>
		/// デバッグメニューにボタン追加
		/// </summary>
		public void AddButton(string label, UnityAction onClick)
		{
			var buttonObj = GameObject.Instantiate(_itemSource);
			buttonObj.SetActive(true);
			buttonObj.transform.SetParent(_menuPage.transform);

			var text = buttonObj.GetComponentInChildren<Text>();
			text.text = label;

			var button = buttonObj.GetComponent<Button>();
			button.onClick.AddListener(onClick);

			_buttonList.Add(buttonObj);
		}

		/// <summary>
		/// 指定ページに遷移
		/// </summary>
		/// <param name="page"></param>
		public void ToPage(Page page)
		{
			_pageHistory.Add(_currentPage);
			setupPage(page);
		}

		/// <summary>
		/// 前のページに戻る
		/// </summary>
		public void BackPage()
		{
			if(_pageHistory.Count <= 0)
			{
				Close();
				return;
			}
			var index = _pageHistory.Count - 1;
			var page = _pageHistory[index];
			_pageHistory.RemoveAt(index);

			setupPage(page);
		}

		/// <summary>
		/// 現在表示されてるボタンを消し、指定されたページを構築
		/// 戻るボタンは最後に自動配置
		/// </summary>
		/// <param name="page"></param>
		private void setupPage(Page page)
		{
			_currentPage = page;

			foreach (var button in _buttonList)
			{
				GameObject.Destroy(button);
			}
			_buttonList.Clear();

			if(_pageList.ContainsKey(page))
			{
				var pageGenerator = _pageList[page];
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
		private Page _currentPage = Page.Top;
		private Dictionary<Page, PageBase> _pageList = new Dictionary<Page, PageBase>();
		private List<Page> _pageHistory = new List<Page>();
		private List<GameObject> _buttonList = new List<GameObject>();

		[SerializeField]
		private GameObject _menuPage = null;

		[SerializeField]
		private GameObject _openButton = null;

		[SerializeField]
		private GameObject _closeButton = null;

		[SerializeField]
		private GameObject _itemSource = null;
	}
}