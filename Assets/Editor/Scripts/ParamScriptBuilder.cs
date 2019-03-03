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
			WriteBlockEnd(true);

			// Value定義がある場合はパラメータとGetData定義作成
			if (valueDefines.Count != 0)
			{
				// パラメータ定義
				WriteBlock($"public class Data");
				var constructorText = "";
				foreach (var def in valueDefines)
				{
					if (constructorText != "")
					{
						constructorText += ", ";
					}
					constructorText += def.GenerateConstructorDefine();
				}
				WriteBlock($"public Data({constructorText})");
				foreach (var def in valueDefines)
				{
					WriteLine(def.GenerateSetValueDefine());
				}
				WriteBlockEnd(true);
				foreach (var def in valueDefines)
				{
					WriteLine(def.GeneratePropatyDefine());
				}
				WriteBlockEnd(true);

				
				// データ取得関数作成
				WriteLine("private static readonly Data[] data = {");
				foreach (var source in dataSources)
				{
					var setValueDefines = "";
					foreach (var def in valueDefines)
					{
						if(setValueDefines != "")
						{
							setValueDefines += ", ";
						}
						setValueDefines += def.ConvertValue(source[def.GetIndex()]);
					}
					WriteLine($"new Data({setValueDefines}),");
				}
				WriteLine("};");
				WriteLine();

				// データ取得関数
				// IDでもアクセスできるように用意
				WriteBlock("public static Data GetData(ID id)");
				WriteLine("return GetData((int)id);");
				WriteBlockEnd(true);

				WriteBlock("public static Data GetData(int id)");
				WriteLine("if( id < 0 || data.Length <= id ) return null;");
				WriteLine("return data[id];");
				WriteBlockEnd(true);

				// データ数取得関数
				WriteBlock("public static int GetCount()");
				WriteLine("return data.Length;");
				WriteBlockEnd();
			}

			WriteBlockEnd();
			WriteBlockEnd();

			return builder.ToString();
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
		private void WriteBlockEnd(bool addLine = false)
		{
			depth -= 1;
			WriteLine("}");
			if (addLine) WriteLine();
		}

		private string scriptName = null;
		private List<ParamValueDefine> valueDefines = null;
		private List<string[]> dataSources = null;

		private int depth = 0;
		private StringBuilder builder = new StringBuilder();
	}
}