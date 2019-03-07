using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace Majiyaba
{
	/// <summary>
	/// パラメータスクリプトビルダー
	/// CSVファイルから読み取った情報をCSコードに変換する
	/// </summary>
	public class ParamScriptBuilder
	{
		public ParamScriptBuilder(string scriptName, List<ParamValueDefine> valueDefines, List<string[]> dataSources)
		{
			this.scriptName = scriptName;
			this.valueDefines = valueDefines;
			this.dataSources = dataSources;
		}

		/// <summary>
		/// 変数定義とデータリストを使ってクラスを作成
		/// Enum定義しかない場合それのみ作成
		/// 完成したスクリプトコードを返す
		/// </summary>
		/// <returns></returns>
		public string Build()
		{
			WriteLine("using UnityEngine;");
			WriteLine("");
			WriteBlock("namespace Majiyaba");
			WriteLine("/// <summary>");
			WriteLine("/// ゲーム内パラメータ");
			WriteLine("/// ParamGeneratorによって自動出力");
			WriteLine("/// </summary>");
			WriteBlock("public class " + scriptName);

			// データ定義から名前を集めてEnumを作成
			WriteBlock("public enum ID");
			WriteLine("Invalid = -1,");
			for (int i = 0; i < dataSources.Count; i++)
			{
				var id = dataSources[i][0];
				if (id == "")
				{
					// 名前がついてない場合はEnumからは除外
					continue;
				}
				WriteLine($"{id} = {i},");
			}
			WriteBlockEnd();

			// Value定義がある場合はパラメータとGetData定義作成
			if (valueDefines.Count != 0)
			{
				WriteLine();

				// データクラス定義
				WriteDataClassDefine();
				WriteLine();

				// データ配列
				WriteDataArray();
				WriteLine();

				// データ取得関数
				WriteBlock("public static Data Get(int id)");
				WriteLine("if( id < 0 || data.Length <= id ) return null;");
				WriteLine("return data[id];");
				WriteBlockEnd();
				WriteLine();

				// IDでもアクセスできる版
				WriteBlock("public static Data Get(ID id)");
				WriteLine("return Get((int)id);");
				WriteBlockEnd();
				WriteLine();

				// データ数取得パラメータ
				WriteBlock("public static int Count");
				WriteLine("get { return data.Length; }");
				WriteBlockEnd();
			}

			WriteBlockEnd();
			WriteBlockEnd();

			return builder.ToString();
		}

		/// <summary>
		/// データクラスの定義
		/// コンストラクタとパラメータを用意
		/// </summary>
		private void WriteDataClassDefine()
		{
			WriteBlock($"public class Data");

			// コンストラクタの宣言部分作成
			var text = "";
			foreach (var def in valueDefines)
			{
				if (text != "") text += ", ";
				text += $"{ def.Type } { def.Name }";
			}

			// コンストラクタ作成
			WriteBlock($"public Data({ text })");
			foreach (var def in valueDefines)
			{
				WriteLine($"this.{ def.Name } = { def.Name };");
			}
			WriteBlockEnd();
			WriteLine();

			// パラメータ定義
			foreach (var def in valueDefines)
			{
				WriteLine($"public { def.Type } { def.Name } {{ get; }}");
			}
			WriteBlockEnd();
		}		

		/// <summary>
		/// データ配列作成
		/// 静的変数の配列として定義する
		/// </summary>
		private void WriteDataArray()
		{
			// データを下から検索し末尾の空白を除外
			int count = dataSources.Count;
			while(count > 0)
			{
				var values = dataSources[count - 1];
				bool hasValue = false;
				foreach(var def in valueDefines )
				{
					if(values[def.Index] != "")
					{
						hasValue = true;
						break;
					}
				}
				if(hasValue)
				{
					// 一つでも値が入っていたら終了
					break;
				}
				// 空っぽならカウントを下げて再開
				count -= 1;
			}

			// データ配列作成
			WriteBlock("private static readonly Data[] data = ");
			for (var i = 0; i < count; i++)
			{
				var values = dataSources[i];
				var text = "";
				foreach (var def in valueDefines)
				{
					if (text != "") text += ", ";
					text += def.ConvertValue(values[def.Index]);
				}
				WriteLine($"new Data({ text }),");
			}
			WriteBlockEnd(true);
		}


		/// <summary>
		/// コードに一列を追加
		/// 現在のブロックの状態を見てインデントも入れる
		/// </summary>
		/// <param name="text"></param>
		private void WriteLine(string text = "")
		{
			for (int i = 0; i < depth; i++)
			{
				builder.Append("	");
			}
			builder.Append(text);
			builder.Append("\n");
		}

		/// <summary>
		/// 入力されたテキストの次の行に { を入れて改行する
		/// 以降のテキストにインデントを追加で加える
		/// </summary>
		/// <param name="text"></param>
		private void WriteBlock(string text)
		{
			WriteLine(text);
			WriteLine("{");
			depth += 1;
		}

		/// <summary>
		/// ブロック終了 } を加えて改行する
		/// 開業後に1ライン空白行を加えることもできる
		/// </summary>
		/// <param name="addLine"></param>
		private void WriteBlockEnd(bool withSemicolon = false)
		{
			depth -= 1;
			WriteLine(withSemicolon ? "};" : "}");
		}

		private string scriptName = null;
		private List<ParamValueDefine> valueDefines = null;
		private List<string[]> dataSources = null;

		private int depth = 0;
		private StringBuilder builder = new StringBuilder();
	}
}