// HtmlDocument.cs

namespace CSharpSamples.Html
{
	using System.IO;
	using System.Text;

	/// <summary>
	/// HtmlDocument �̊T�v�̐����ł��B
	/// </summary>
	public class HtmlDocument
	{
		private HtmlNodeCollection root;
		private bool formatted;

		/// <summary>
		/// ���[�g�m�[�h�R���N�V�������擾
		/// </summary>
		public HtmlNodeCollection Root
		{
			get
			{
				return this.root;
			}
		}

		/// <summary>
		/// Html�h�L�������g���擾
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
		/// �t�H�[�}�b�g���ꂽHtml���ǂ������擾�܂��͐ݒ�
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
		/// HtmlDocument�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="html"></param>
		public HtmlDocument(string html)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.root = null;
			this.formatted = true;

			this.LoadHtml(html);
		}

		/// <summary>
		/// HtmlDocument�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlDocument() : this(string.Empty)
		{
		}

		/// <summary>
		/// �w�肵��Html�`���̕������ǂݍ���
		/// </summary>
		/// <param name="html"></param>
		public void LoadHtml(string html)
		{
			HtmlParser p = new HtmlParser();
			this.root = p.Parse(html);
		}

		/// <summary>
		/// �w�肵���t�@�C������Html��ǂݍ���
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
		/// Html���X�g���[���ɏ�������
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
		/// �w�肵�����O�̃^�O�����ׂĎ擾
		/// </summary>
		/// <param name="tagName"></param>
		/// <returns></returns>
		public HtmlElement[] GetElementsByName(string tagName)
		{
			return HtmlElement.Sta_GetElementsByName(this.root, tagName);
		}

		/// <summary>
		/// �w�肵��ID�����G�������g���擾
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public HtmlElement GetElementById(string id)
		{
			return HtmlElement.Sta_GetElementById(this.root, id);
		}

		/// <summary>
		/// ���̃C���X�^���X��Html�`���̕�����ɕϊ�
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Html;
		}
	}
}
