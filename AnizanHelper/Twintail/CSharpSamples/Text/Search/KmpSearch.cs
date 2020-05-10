namespace CSharpSamples.Text.Search
{
	/// <summary>
	/// Knuth-Morris-Pratt�ɂ�镶����ƍ��A���S���Y���B
	/// (�Q�lURL: http://alfin.mine.utsunomiya-u.ac.jp/~niy/algo/k/kmpMatch.html)
	/// </summary>
	public class KmpSearch : ISearchable
	{
		private readonly int[] shift;
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

		/// <summary>
		/// KmpSearch�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="key"></param>
		public KmpSearch(string key)
		{
			this.shift = this.makeTable(key);
			this.pattern = key;
		}

		/// <summary>
		/// �ړ��ʃe�[�u�����쐬
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private int[] makeTable(string key)
		{
			int[] table = new int[key.Length];
			int p = 0, t = 0;

			while (++t != key.Length)
			{
				table[t] = p;
				if (key[t] == key[p])
				{
					p++;
				}
			}
			return table;
		}

		/// <summary>
		/// ������ƍ����s��
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Search(string input)
		{
			return this.Search(input, 0);
		}

		/// <summary>
		/// ������ƍ����s��
		/// </summary>
		/// <param name="input"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public int Search(string input, int index)
		{
			if (input.Length < this.pattern.Length)
			{
				return -1;
			}

			int endPos = input.Length;
			int patEnd = this.pattern.Length;
			int patIdx = 0;

			while (index != endPos)
			{
				if (input[index++] == this.pattern[patIdx])
				{
					patIdx++;
				}
				else if (patIdx != 0)
				{
					patIdx = this.shift[patIdx];
					index--;
				}
				if (patIdx == patEnd)
				{
					return (index - this.pattern.Length);
				}
			}
			return -1;
		}
	}
}
