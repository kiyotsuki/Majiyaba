using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Majiyaba
{
	public class GameResourceManager : ManagerBase
	{
		public override IEnumerator OnInitialize()
		{
			//LoadAssetAsync("test", "debug_canvas");

			//AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "images"));
			//testBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "test"));
			/*
			var test = testBundle.LoadAsset("testimage.prefab") as GameObject;
			var go = GameObject.Instantiate(test);
			go.transform.SetParent(this.gameObject.transform);
			*/
			Ready = true;
			yield break;
		}

		public int LoadAssetAsync(string bundle, string asset, System.Action<Object> callback)
		{
			if (useAssetBundle)
			{
				var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + bundle);
				var o = ab.LoadAsset(asset);
			}
			else
			{
				/*
				var path = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundle, asset);
				var o = AssetDatabase.LoadMainAssetAtPath(path[0]);
				Debug.Log("どうだ" + o);
				callback(o);
				*/
			}



			return 0;
		}


		private bool useAssetBundle = false;

		private class LoadedAssetBundle
		{
			public int refCount = 0;
			public AssetBundle assetBundle = null;
		}


		List<AssetBundle> assetBundles = new List<AssetBundle>();
		Dictionary<int, AssetBundle> loadedBundles = new Dictionary<int, AssetBundle>();



		AssetBundle testBundle = null;
	}
}