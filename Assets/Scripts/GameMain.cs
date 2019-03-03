using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Majiyaba
{
	public class GameMain : MonoBehaviour
	{
		// インスタンス
		public static GameMain Instance { get; protected set; }

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
			Instance = obj.AddComponent<GameMain>();
		}

		/// <summary>
		///	各マネージャの取得
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetManager<T>() where T : ManagerBase
		{
			foreach (var manager in managerList)
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
			fadeManager = CreateManager<FadeManager>();
			
			yield return new WaitUntil(() =>
			{
				if(fadeManager.Ready == false)
				{
					return false;
				}
				return true;
			});

			CreateManager<ActorManager>();
			CreateManager<AdventureManager>();
			CreateManager<DebugManager>();

			yield return new WaitUntil(() =>
			{
				foreach (var manager in managerList)
				{
					if(manager.Ready == false)
					{
						return false;
					}
				}
				return true;
			});

			activeScene = SceneManager.GetActiveScene();
			if(activeScene.name == "_main")
			{
				// タイトルへ遷移
				RequestChangeScene("title");
			}
			else
			{
				foreach (var manager in managerList)
				{
					StartCoroutine(manager.OnBeginScene());
				}
				sceneState = SceneState.Init;
			}
			
			initialized = true;
			yield break;
		}

		/// <summary>
		///	マネージャのクラス名でGameObjectを作成
		///	初期化を開始する
		/// </summary>
		/// <typeparam name="T"></typeparam>
		private T CreateManager<T>() where T : ManagerBase
		{
			var obj = new GameObject(typeof(T).Name);
			obj.transform.SetParent(gameObject.transform);
			var manager = obj.AddComponent<T>();
			managerList.Add(manager);

			StartCoroutine(manager.OnInitialize());
			return manager;
		}
		

		// Update is called once per frame
		void Update()
		{
			if(initialized == false)
			{
				// 初期化が終わるまで待機
				return;
			}

			switch (sceneState)
			{
				case SceneState.Request:
					{
						// フェードアウトするまで待つ
						if (fadeManager.IsCoverd())
						{
							foreach (var manager in managerList)
							{
								manager.OnTerminateScene();
							}

							sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneName);
							sceneState = SceneState.Load;
						}
					}
					break;

				case SceneState.Load:
					{
						if(sceneLoadOperation.isDone)
						{
							activeScene = SceneManager.GetActiveScene();
							nextSceneName = null;
							sceneLoadOperation = null;
							
							foreach (var manager in managerList)
							{
								StartCoroutine(manager.OnBeginScene());
							}
							sceneState = SceneState.Init;
						}
					}
					break;

				case SceneState.Init:
					{
						bool allReady = true;
						foreach (var manager in managerList)
						{
							if(manager.Ready == false)
							{
								allReady = false;
								break;
							}
						}
						if (allReady)
						{
							fadeManager.RequestFadeIn(FadeManager.FadeFlag.Scene);
							sceneState = SceneState.None;
						}
					}
					break;

				case SceneState.None:
					{
						foreach (var manager in managerList)
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
			if(sceneState != SceneState.None)
			{
				Debug.LogAssertion("シーン遷移中に別のシーンへ遷移しようとしました " + sceneName);
				return false;
			}
			nextSceneName = sceneName;
			sceneState = SceneState.Request;

			// フェード開始
			fadeManager.RequestFadeOut(FadeManager.FadeFlag.Scene);
			return false;
		}

		public enum SceneState
		{
			None,
			Request,
			Load,
			Init,
		}
		private SceneState sceneState = SceneState.None;
		private string nextSceneName = null;
		private AsyncOperation sceneLoadOperation = null;

		private Scene activeScene = new Scene();

		private bool initialized = false;
		private List<ManagerBase> managerList = new List<ManagerBase>();

		private FadeManager fadeManager = null;
	}
}