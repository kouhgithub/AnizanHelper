
using System;
using System.Text;

namespace CSharpSamples.Text.Search
{
	/// <summary>
	/// BM�@�ɂ�镶����ƍ��A���S���Y���B��ʂ̃��������g�p����̂ł��߂ہc�B
	/// (�Q�lURL: http://www2.starcat.ne.jp/~fussy/algo/algo7-4.htm)
	/// </summary>
	[Obsolete]
	public class BmSearch : ISearchable
	{
		private readonly int[] skip = new int[char.MaxValue + 1];
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
		/// BmSearch�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="key"></param>
		public BmSearch(string key)
		{
			this.pattern = key;
			this.makeTable(key);
		}

		/// <summary>
		/// �ړ��ʃe�[�u�����쐬
		/// </summary>
		/// <param name="patt"></param>
		private void makeTable(string key)
		{
			int len = key.Length;

			for (int i = 0; i < this.skip.Length; i++)
			{
				this.skip[i] = len;
			}

			int idx = 0;
			while (len > 0)
			{
				this.skip[key[idx++]] = --len;
			}
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
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (index < 0 || index >= input.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}

			int patlen = this.pattern.Length - 1;
			int endPos = (input.Length - index) - patlen;

			while (index < endPos)
			{
				int i;
				for (i = patlen; i >= 0; i--)
				{
					if (input[index + i] != this.pattern[i])
					{
						break;
					}
				}

				// ���ׂĈ�v
				if (i < 0)
				{
					return index;
				}

				// �e�[�u������ړ��ʂ����߂�(�����Ȃ�ړ��ʂ�2)	
				int move = this.skip[input[index + i]] - (patlen - i);
				index += (move > 0) ? move : 2;
			}

			return index;
		}
	}

	/// <summary>
	/// BM�@�ɂ�镶����ƍ��A���S���Y���BBmSearch�N���X�Ƃ͈Ⴂ�A�o�C�g�P�ʂŔ�r���s���B
	/// (�Q�lURL: http://www2.starcat.ne.jp/~fussy/algo/algo7-4.htm)
	/// </summary>
	public class BmSearch2 : ISearchable
	{
		private readonly int[] skip = new int[byte.MaxValue + 1];
		private readonly byte[] pattern;
		private readonly string patternS;

		/// <summary>
		/// �����p�^�[�����擾
		/// </summary>
		public string Pattern
		{
			get
			{
				return this.patternS;
			}
		}

		/// <summary>
		/// BmSearch2�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="key"></param>
		public BmSearch2(string key)
		{
			this.patternS = key;
			this.pattern = Encoding.Unicode.GetBytes(key);
			this.makeTable(this.pattern);
		}

		/// <summary>
		/// �ړ��ʃe�[�u�����쐬
		/// </summary>
		/// <param name="key"></param>
		private void makeTable(byte[] key)
		{
			int len = key.Length;

			for (int i = 0; i < this.skip.Length; i++)
			{
				this.skip[i] = len;
			}

			int idx = 0;
			while (len > 0)
			{
				this.skip[key[idx++]] = --len;
			}
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
		/// <param name="index">�����J�n�C���f�b�N�X</param>
		/// <returns></returns>
		public unsafe int Search(string input, int index)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (input.Length == 0)
			{
				return -1;
			}
			if (index < 0 || index >= input.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}

			int length = Encoding.Unicode.GetByteCount(input);
			int keylen = this.pattern.Length - 1;

			fixed (char* lpsz = input)
			{
				byte* st = (byte*)lpsz;
				byte* p = (byte*)(lpsz + index);
				byte* ed = st + (length - keylen);

				while (p < ed)
				{
					int i;
					for (i = keylen; i >= 0; i--)
					{
						if (p[i] != this.pattern[i])
						{
							break;
						}
					}

					// ���ׂĈ�v
					if (i < 0)
					{
						return (int)(p - st) / 2;
					}

					// �e�[�u������ړ��ʂ����߂�(�����Ȃ�2�������ړ�)	
					int move = this.skip[p[i]] - (keylen - i);
					p += (move > 0) ? move : 4;
				}
			}

			return -1;
		}
	}
}
