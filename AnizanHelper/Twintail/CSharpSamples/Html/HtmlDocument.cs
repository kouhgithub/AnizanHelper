// HtmlDocument.cs

namespace CSharpSamples.Html
{
	using System.IO;
	using System.Text;

	/// <summary>
	/// HtmlDocument の概要の説明です。
	/// </summary>
	public class HtmlDocument
	{
		private HtmlNodeCollection root;
		private bool formatted;

		/// <summary>
		/// ルートノードコレクションを取得
		/// </summary>
		public HtmlNodeCollection Root
		{
			get
			{
				return this.root;
			}
		}

		/// <summary>
		/// Htmlドキュメントを取得
		/// </summary>
		public string Html
		{
			get
			{
				if (this.formatted)
				{
					HtmlFormatter f = new HtmlFormatter();
					return f.Format(this.root);
				}
				else
				{
					StringBuilder sb = new StringBuilder();

					foreach (HtmlNode node in this.root)
					{
						sb.Append(node.Html);
					}

					return sb.ToString();
				}
			}
		}

		/// <summary>
		/// フォーマットされたHtmlかどうかを取得または設定
		/// </summary>
		public bool Formatted
		{
			set
			{
				if (this.formatted != value)
				{
					this.formatted = value;
				}
			}
			get
			{
				return this.formatted;
			}
		}

		/// <summary>
		/// HtmlDocumentクラスのインスタンスを初期化
		/// </summary>
		/// <param name="html"></param>
		public HtmlDocument(string html)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.root = null;
			this.formatted = true;

			this.LoadHtml(html);
		}

		/// <summary>
		/// HtmlDocumentクラスのインスタンスを初期化
		/// </summary>
		public HtmlDocument() : this(string.Empty)
		{
		}

		/// <summary>
		/// 指定したHtml形式の文字列を読み込む
		/// </summary>
		/// <param name="html"></param>
		public void LoadHtml(string html)
		{
			HtmlParser p = new HtmlParser();
			this.root = p.Parse(html);
		}

		/// <summary>
		/// 指定したファイルからHtmlを読み込む
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="encoding"></param>
		public void Load(string filePath, Encoding encoding)
		{
			using (StreamReader sr = new StreamReader(filePath, encoding))
			{
				this.LoadHtml(sr.ReadToEnd());
			}
		}

		/// <summary>
		/// Htmlをストリームに書き込む
		/// </summary>
		/// <param name="writer"></param>
		public void Save(string filePath)
		{
			using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.Default))
			{
				sw.Write(this.Html);
			}
		}

		/// <summary>
		/// 指定した名前のタグをすべて取得
		/// </summary>
		/// <param name="tagName"></param>
		/// <returns></returns>
		public HtmlElement[] GetElementsByName(string tagName)
		{
			return HtmlElement.Sta_GetElementsByName(this.root, tagName);
		}

		/// <summary>
		/// 指定したIDを持つエレメントを取得
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public HtmlElement GetElementById(string id)
		{
			return HtmlElement.Sta_GetElementById(this.root, id);
		}

		/// <summary>
		/// このインスタンスをHtml形式の文字列に変換
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Html;
		}
	}
}
