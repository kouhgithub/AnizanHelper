using System.Text.RegularExpressions;

namespace CSharpSamples.Text.Search
{
	/// <summary>
	/// Regex�N���X���g�p���Đ��K�\���������s��
	/// </summary>
	public class RegexSearch : ISearchable
	{
		private readonly Regex regex;
		private readonly string pattern;

		/// <summary>
		/// �����p�^�[�����擾
		/// </summary>
		public string Pattern
		{
			get
			{
				return this.pattern;
			}
		}

		public Regex Regex
		{
			get
			{
				return this.regex;
			}
		}

		/// <summary>
		/// RegexSearch�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="key"></param>
		public RegexSearch(string key, RegexOptions options)
		{
			this.regex = new Regex(key, options);
			this.pattern = key;
		}

		/// <summary>
		/// ������ƍ����s��
		/// </summary>
		/// <param name="text">����������</param>
		/// <returns></returns>
		public int Search(string text)
		{
			return this.Search(text, 0);
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X���當����ƍ����s��
		/// </summary>
		/// <param name="text">����������</param>
		/// <param name="index">�����J�n�C���f�b�N�X</param>
		/// <returns></returns>
		public int Search(string text, int index)
		{
			Match m = this.regex.Match(text, index);
			return (m.Success ? m.Index : -1);
		}
	}
}
