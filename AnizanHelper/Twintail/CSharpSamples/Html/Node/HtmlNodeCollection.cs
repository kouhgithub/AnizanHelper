// HtmlNodeCollection.cs

namespace CSharpSamples.Html
{
	using System.Collections;

	/// <summary>
	/// HtmlNodeCollection �̊T�v�̐����ł��B
	/// </summary>
	public class HtmlNodeCollection
	{
		private ArrayList nodes;
		private HtmlElement parent;

		/// <summary>
		/// �R���N�V�������̃m�[�h�����擾
		/// </summary>
		public int Count
		{
			get
			{
				return this.nodes.Count;
			}
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�ɂ���m�[�h���擾�܂��͐ݒ�
		/// </summary>
		public HtmlNode this[int index]
		{
			set
			{
				this.RemoveAt(index);
				this.Insert(index, value);
			}
			get
			{
				return (HtmlNode)this.nodes[index];
			}
		}

		/// <summary>
		/// HtmlNodeCollection�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="parent"></param>
		public HtmlNodeCollection(HtmlElement parent)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.nodes = new ArrayList();
			this.parent = parent;
		}

		/// <summary>
		/// newNode���R���N�V�����̖����ɒǉ�
		/// </summary>
		/// <param name="newNode"></param>
		public void Add(HtmlNode newNode)
		{
			if (newNode.Parent != null)
			{
				throw new HtmlException();  // ����C���X�^���X�𕡐��o�^���邱�Ƃ͏o���Ȃ�
			}

			this.nodes.Add(newNode);
			newNode.SetParent(this.parent);
		}

		/// <summary>
		/// newNode���w�肵���ʒu�ɑ}��
		/// </summary>
		/// <param name="index"></param>
		/// <param name="newNode"></param>
		public void Insert(int index, HtmlNode newNode)
		{
			if (newNode.Parent != null)
			{
				throw new HtmlException();  // ����C���X�^���X�𕡐��o�^���邱�Ƃ͏o���Ȃ�
			}

			this.nodes.Insert(index, newNode);
			newNode.SetParent(this.parent);
		}

		/// <summary>
		/// �w�肵���m�[�h���R���N�V��������폜
		/// </summary>
		/// <param name="node"></param>
		public void Remove(HtmlNode node)
		{
			this.nodes.Remove(node);
			node.SetParent(null);
		}

		/// <summary>
		/// �w�肵���ʒu�ɂ���m�[�h���R���N�V��������폜
		/// </summary>
		/// <param name="index"></param>
		/// <param name="node"></param>
		public void RemoveAt(int index)
		{
			HtmlNode node = (HtmlNode)this.nodes[index];

			this.nodes.RemoveAt(index);
			node.SetParent(null);
		}

		/// <summary>
		/// ���ׂẴm�[�h���R���N�V��������폜
		/// </summary>
		public void RemoveAll()
		{
			while (this.Count > 0)
			{
				this.RemoveAt(0);
			}
		}

		/// <summary>
		/// node�̃R���N�V�������C���f�b�N�X��Ԃ�
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public int IndexOf(HtmlNode node)
		{
			return this.nodes.IndexOf(node);
		}

		/// <summary>
		/// node���R���N�V�������ɑ��݂��Ă��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool Contains(HtmlNode node)
		{
			return this.nodes.Contains(node);
		}

		/// <summary>
		/// HtmlNodeCollection�𔽕���������񋓎q��Ԃ�
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return this.nodes.GetEnumerator();
		}
	}
}
