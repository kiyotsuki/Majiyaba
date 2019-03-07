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
			Stage01 = 1,
			Stage02 = 2,
			Stage03 = 3,
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
			new Data("stage01", true),
			new Data("stage02", true),
			new Data("stage03", true),
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
