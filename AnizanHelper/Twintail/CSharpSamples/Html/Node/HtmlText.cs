// HtmlText.cs

namespace CSharpSamples.Html
{
	/// <summary>
	/// Html���̃e�L�X�g��\��
	/// </summary>
	public class HtmlText : HtmlNode
	{
		private string text;

		/// <summary>
		/// �e�L�X�g�̓��e���擾�܂��͐ݒ�
		/// </summary>
		public string Content
		{
			set
			{
				this.text = value;
			}
			get
			{
				return this.text;
			}
		}

		/// <summary>
		/// ���̃v���p�e�B��Content�v���p�e�B�Ɠ����l��Ԃ�
		/// </summary>
		public override string Html
		{
			get
			{
				return this.text;
			}
		}

		/// <summary>
		/// ���̃v���p�e�B��Content�v���p�e�B�Ɠ����l��Ԃ�
		/// </summary>
		public override string InnerHtml
		{
			get
			{
				return this.text;
			}
		}

		/// <summary>
		/// ���̃v���p�e�B��Content�v���p�e�B�Ɠ����l��Ԃ�
		/// </summary>
		public override string InnerText
		{
			get
			{
				return this.text;
			}
		}

		/// <summary>
		/// HtmlText�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="text"></param>
		public HtmlText(string text)
		{
			this.text = text;
		}

		/// <summary>
		/// HtmlText�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlText() : this(string.Empty)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
