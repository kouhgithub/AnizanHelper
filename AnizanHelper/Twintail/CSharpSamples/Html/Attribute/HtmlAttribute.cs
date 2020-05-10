// HtmlAttribute.cs

namespace CSharpSamples.Html
{
	/// <summary>
	/// ������\���N���X
	/// </summary>
	public class HtmlAttribute
	{
		private string name;
		private string _value;

		/// <summary>
		/// �����̖��O���擾�܂��͐ݒ�
		/// </summary>
		public string Name
		{
			set
			{
				this.name = value;
			}
			get
			{
				return this.name;
			}
		}

		/// <summary>
		/// �����̒l���擾�܂��͐ݒ�
		/// </summary>
		public string Value
		{
			set
			{
				this._value = value;
			}
			get
			{
				return this._value;
			}
		}

		/// <summary>
		/// ������Html�`���Ŏ擾
		/// </summary>
		public string Html
		{
			get
			{
				return string.Format("{0}=\"{1}\"", this.name, this._value);
			}
		}

		/// <summary>
		/// HtmlAttribute�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="name">������</param>
		/// <param name="val">�����l</param>
		public HtmlAttribute(string name, string val)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.name = name;
			this._value = val;
		}

		/// <summary>
		/// HtmlAttribute�N���X�̃C���X�^���X��������
		/// </summary>
		public HtmlAttribute() : this(string.Empty, string.Empty)
		{
		}

		/// <summary>
		/// ���̃C���X�^���X�𕶎���`���ɕϊ�
		/// </summary>
		/// <returns>Html�v���p�e�B�̒l��Ԃ�</returns>
		public override string ToString()
		{
			return this.Html;
		}
	}
}
