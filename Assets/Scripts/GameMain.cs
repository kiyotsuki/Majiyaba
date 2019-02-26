using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Majiyaba
{
	public class GameMain : MonoBehaviour
	{
		// インスタンス
		private static GameMain _instance;

		/// <summary>
		/// アプリケーションの開始
		/// GameMainをインスタンス化して開始する
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Setup()
		{
			var obj = new GameObject("GameMain");
			DontDestroyOnLoad(obj);

			// インスタンスを登録
			_instance = obj.AddComponent<GameMain>();
		}

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns></returns>
		public static GameMain GetInstance()
		{
			return _instance;
		}



		/// <summary>
		///	各マネージャの取得
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetManager<T>() where T : ManagerBase
		{
			foreach (var manager in _managerList)
			{
				if (manager is T)
				{
					return (T)manager;
				}
			}
			return null;
		}


		/// <summary>
		/// ゲーム初期化開始
		/// </summary>
		void Start()
		{
			StartCoroutine(initializeGame());
		}

		/// <summary>
		/// ゲームの初期化
		/// アプリケーション開始時に一度だけ行う
		/// </summary>
		/// <returns></returns>
		private IEnumerator initializeGame()
		{
			_fadeManager = createManager<FadeManager>();
			
			yield return new WaitUntil(() =>
			{
				if(_fadeManager.Ready == false)
				{
					return false;
				}
				return true;
			});

			createManager<ActorManager>();
			createManager<AdventureManager>();
			createManager<DebugManager>();

			yield return new WaitUntil(() =>
			{
				foreach (var manager in _managerList)
				{
					if(manager.Ready == false)
					{
						return false;
					}
				}
				return true;
			});

			_activeScene = SceneManager.GetActiveScene();
			if(_activeScene.name == "_main")
			{
				// タイトルへ遷移
				RequestChangeScene("title");
			}
			else
			{
				foreach (var manager in _managerList)
				{
					StartCoroutine(manager.OnBeginScene());
				}
				_sceneState = SceneState.Init;
			}
			
			_initialized = true;
			yield break;
		}

		/// <summary>
		///	マネージャのクラス名でGameObjectを作成
		///	初期化を開始する
		/// </summary>
		/// <typeparam name="T"></typeparam>
		private T createManager<T>() where T : ManagerBase
		{
			var obj = new GameObject(typeof(T).Name);
			obj.transform.SetParent(gameObject.transform);
			var manager = obj.AddComponent<T>();
			_managerList.Add(manager);

			StartCoroutine(manager.OnInitialize());
			return manager;
		}
		

		// Update is called once per frame
		void Update()
		{
			if(_initialized == false)
			{
				// 初期化が終わるまで待機
				return;
			}

			switch (_sceneState)
			{
				case SceneState.Request:
					{
						// フェードアウトするまで待つ
						if (_fadeManager.IsCoverd())
						{
							foreach (var manager in _managerList)
							{
								manager.OnTerminateScene();
							}

							_sceneLoadOperation = SceneManager.LoadSceneAsync(_nextSceneName);
							_sceneState = SceneState.Load;
						}
					}
					break;

				case SceneState.Load:
					{
						if(_sceneLoadOperation.isDone)
						{
							_activeScene = SceneManager.GetActiveScene();
							_nextSceneName = null;
							_sceneLoadOperation = null;
							
							foreach (var manager in _managerList)
							{
								StartCoroutine(manager.OnBeginScene());
							}
							_sceneState = SceneState.Init;
						}
					}
					break;

				case SceneState.Init:
					{
						bool allReady = true;
						foreach (var manager in _managerList)
						{
							if(manager.Ready == false)
							{
								allReady = false;
								break;
							}
						}
						if (allReady)
						{
							_fadeManager.RequestFadeIn(FadeManager.FadeFlag.Scene);
							_sceneState = SceneState.None;
						}
					}
					break;

				case SceneState.None:
					{
						foreach (var manager in _managerList)
						{
							manager.OnUpdateScene();
						}
					}
					break;
			}
		}
		
		/// <summary>
		/// 他シーンへ遷移
		/// フェードの管理もここで行う
		/// </summary>
		/// <param name="sceneName"></param>
		public bool RequestChangeScene(string sceneName)
		{
			if(_sceneState != SceneState.None)
			{
				Debug.LogAssertion("シーン遷移中に別のシーンへ遷移しようとしました " + sceneName);
				return false;
			}
			_nextSceneName = sceneName;
			_sceneState = SceneState.Request;

			// フェード開始
			_fadeManager.RequestFadeOut(FadeManager.FadeFlag.Scene);
			return false;
		}

		public enum SceneState
		{
			None,
			Request,
			Load,
			Init,
		}
		private SceneState _sceneState = SceneState.None;
		private string _nextSceneName = null;
		private AsyncOperation _sceneLoadOperation = null;

		private Scene _activeScene = new Scene();

		private bool _initialized = false;
		private List<ManagerBase> _managerList = new List<ManagerBase>();

		private FadeManager _fadeManager = null;
	}
}