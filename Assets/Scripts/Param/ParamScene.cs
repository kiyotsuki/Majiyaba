using UnityEngine;

namespace Majiyaba
{
	/// <summary>
	/// ゲーム内パラメータ
	/// ParamGeneratorによって自動出力
	/// </summary>
	public class ParamScene
	{
		public enum ID
		{
			Invalid = -1,
			Title = 0,
			Test = 1,
			Stage01 = 2,
			Stage02 = 3,
		}
		
		public class Data
		{
			public Data(string SceneName, bool Playable)
			{
				this.SceneName = SceneName;
				this.Playable = Playable;
			}
			
			public string SceneName { get; }
			public bool Playable { get; }
		}
		
		private static readonly Data[] data = 
		{
			new Data("title", false),
			new Data("test", true),
			new Data("stage01", true),
			new Data("stage02", true),
		};
		
		public static Data Get(int id)
		{
			if( id < 0 || data.Length <= id ) return null;
			return data[id];
		}
		
		public static Data Get(ID id)
		{
			return Get((int)id);
		}
		
		public static int Count
		{
			get { return data.Length; }
		}
	}
}
