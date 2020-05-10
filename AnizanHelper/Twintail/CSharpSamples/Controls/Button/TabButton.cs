// TabButton.cs

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace CSharpSamples
{
	/// <summary>
	/// TabButtonControl �̃^�u�{�^����\���N���X�ł��B
	/// </summary>
	[DesignTimeVisible(false)]
	public class TabButton : Component, ICloneable
	{
		#region inner class
		/// <summary>
		/// TabButton ���i�[����R���N�V�����ł��B
		/// </summary>
		public class TabButtonCollection : ICollection, IList, IEnumerable
		{
			private TabButtonControl parent;
			private ArrayList innerList;

			/// <summary>
			/// �R���N�V�����Ɋi�[����Ă���^�u�����擾���܂��B
			/// </summary>
			public int Count
			{
				get
				{
					return this.innerList.Count;
				}
			}

			/// <summary>
			/// �w�肵���C���f�b�N�X�̃^�u���擾�܂��͐ݒ肵�܂��B
			/// </summary>
			public TabButton this[int index]
			{
				get
				{
					return (TabButton)this.innerList[index];
				}
			}

			/// <summary>
			/// TabButtonCollection�N���X�̃C���X�^���X���������B
			/// </summary>
			internal TabButtonCollection(TabButtonControl parent)
			{
				this.parent = parent;
				this.innerList = new ArrayList();
			}

			/// <summary>
			/// �R���N�V�����̖����� button ��ǉ����܂��B
			/// </summary>
			/// <param name="button"></param>
			/// <returns></returns>
			public int Add(TabButton button)
			{
				if (button == null)
				{
					throw new ArgumentNullException("button");
				}

				if (button.parent != null)
				{
					throw new ArgumentException("���̃{�^���͊��ɑ��̃^�u�R���g���[���ɓo�^����Ă��܂��B");
				}

				int index = this.innerList.Add(button);

				button.parent = this.parent;
				button.imageList = this.parent.ImageList;
				this.parent.UpdateButtons();

				return index;
			}

			/// <summary>
			/// �R���N�V�����̖����� array ��ǉ����܂��B
			/// </summary>
			/// <param name="array"></param>
			public void AddRange(TabButton[] array)
			{
				foreach (TabButton button in array)
				{
					this.Add(button);
				}
			}

			/// <summary>
			/// �R���N�V�������� index �Ԗڂ� button ��}�����܂��B
			/// </summary>
			/// <param name="index">0 ����n�܂�R���N�V�������C���f�b�N�X�B</param>
			/// <param name="button">index �Ԗڂɑ}�������{�^���B</param>
			public void Insert(int index, TabButton button)
			{
				if (index < 0 || index > this.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}

				if (button.parent != null)
				{
					throw new ArgumentException("���̃{�^���͊��ɑ��̃^�u�R���g���[���ɓo�^����Ă��܂��B");
				}

				this.innerList.Insert(index, button);

				button.parent = this.parent;
				button.imageList = this.parent.ImageList;

				this.parent.UpdateButtons();
			}

			/// <summary>
			/// button ���R���N�V��������폜���܂��B
			/// </summary>
			/// <param name="button">�R���N�V��������폜����{�^���B</param>
			public void Remove(TabButton button)
			{
				if (this.innerList.Contains(button))
				{
					button.parent = null;
					button.imageList = null;

					this.innerList.Remove(button);
					this.parent.UpdateButtons();
				}
			}

			/// <summary>
			/// �R���N�V�������� index �Ԗڂ̃{�^�����폜���܂��B
			/// </summary>
			/// <param name="index">�폜����{�^���̃C���f�b�N�X�B</param>
			public void RemoveAt(int index)
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}

				TabButton button = this[index];
				button.parent = null;
				button.imageList = null;

				this.innerList.RemoveAt(index);
				this.parent.UpdateButtons();
			}

			public bool Contains(TabButton button)
			{
				return this.innerList.Contains(button);
			}

			public int IndexOf(TabButton button)
			{
				return this.innerList.IndexOf(button);
			}

			/// <summary>
			/// �o�^����Ă���{�^�������ׂč폜���܂��B
			/// </summary>
			public void Clear()
			{
				foreach (TabButton button in this.innerList)
				{
					button.parent = null;
					button.imageList = null;
				}
				this.innerList.Clear();
				this.parent.UpdateButtons();
			}

			/// <summary>
			/// button �̈ʒu�� target �̑O�Ɉړ����܂��B
			/// </summary>
			/// <param name="target"></param>
			/// <param name="button"></param>
			public void InsertBefore(TabButton target, TabButton button)
			{
				if (target == button)
				{
					return;
				}

				if (target.parent == null)
				{
					throw new ArgumentException("target �ɐe�����݂��܂���B");
				}

				if (button.parent == null)
				{
					throw new ArgumentException("button �ɐe�����݂��܂���");
				}

				if (target.parent != button.parent)
				{
					throw new ArgumentException("target �� button �̐e���Ⴂ�܂��B");
				}

				int newIndex;

				if (target.Index < button.Index)
				{
					newIndex = target.Index;
				}
				else
				{
					newIndex = target.Index - 1;
				}

				this.innerList.Remove(button);
				this.innerList.Insert(newIndex, button);

				this.parent.UpdateButtons();
			}

			/// <summary>
			/// TabButtonCollection �̃Z�N�V�����̗񋓎q��Ԃ��܂��B
			/// </summary>
			/// <returns>IEnumerator</returns>
			public IEnumerator GetEnumerator()
			{
				return this.innerList.GetEnumerator();
			}

			#region ICollection
			/// <summary>
			/// �q�̃R���N�V�����ւ̃A�N�Z�X����������Ă��邩�ǂ����𔻒f���܂��B
			/// </summary>
			bool ICollection.IsSynchronized
			{
				get
				{
					return this.innerList.IsSynchronized;
				}
			}

			/// <summary>
			/// �q�̃R���N�V�����ւ̃A�N�Z�X�𓯊����邽�߂Ɏg�p����I�u�W�F�N�g���擾���܂��B
			/// </summary>
			object ICollection.SyncRoot
			{
				get
				{
					return this.innerList.SyncRoot;
				}
			}

			/// <summary>
			/// ���̃C���X�^���X�� array �ɃR�s�[���܂��B
			/// </summary>
			/// <param name="array"></param>
			/// <param name="index"></param>
			void ICollection.CopyTo(Array array, int index)
			{
				this.innerList.CopyTo(array, index);
			}
			#endregion

			#region IList
			bool IList.IsReadOnly
			{
				get
				{
					return this.innerList.IsReadOnly;
				}
			}
			bool IList.IsFixedSize
			{
				get
				{
					return this.innerList.IsFixedSize;
				}
			}
			object IList.this[int index]
			{
				set
				{
					throw new NotSupportedException();
				}
				get
				{
					return this[index];
				}
			}
			int IList.Add(object obj)
			{
				return this.Add((TabButton)obj);
			}
			bool IList.Contains(object obj)
			{
				return this.innerList.Contains(obj);
			}
			int IList.IndexOf(object obj)
			{
				return this.innerList.IndexOf(obj);
			}
			void IList.Insert(int index, object obj)
			{
				this.Insert(index, (TabButton)obj);
			}
			void IList.Remove(object obj)
			{
				this.Remove((TabButton)obj);
			}
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}
			#endregion
		}
		#endregion

		internal ImageList imageList;
		private TabButtonControl parent = null;
		private string text = string.Empty;
		private int imageIndex = -1;
		private object tag;

		private Color activeForeColor = SystemColors.ControlText;
		private Color activeBackColor = SystemColors.ControlLightLight;
		private FontStyle activeFontStyle = FontStyle.Regular;
		private FontFamily activeFontFamily = Control.DefaultFont.FontFamily;

		private Color inactiveForeColor = SystemColors.ControlText;
		private Color inactiveBackColor = SystemColors.Control;
		private FontStyle inactiveFontStyle = FontStyle.Regular;
		private FontFamily inactiveFontFamily = Control.DefaultFont.FontFamily;

		internal Rectangle bounds = Rectangle.Empty;

		/// <summary>
		/// �^�u�̕\���e�L�X�g���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue("")]
		public string Text
		{
			set
			{
				this.text = value;
				this.Update(true);
			}
			get
			{
				return this.text;
			}
		}

		/// <summary>
		/// ���̃{�^���̃C���f�b�N�X���擾���܂��B
		/// </summary>
		[Browsable(false)]
		public int Index
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.Buttons.IndexOf(this);
				}

				return -1;
			}
		}

		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
		}

		/// <summary>
		/// ImageList �̃C���f�b�N�X���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(-1)]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor", typeof(UITypeEditor))]
		public int ImageIndex
		{
			set
			{
				if (this.imageIndex != value)
				{
					this.imageIndex = value;
					this.Update(false);
				}
			}
			get
			{
				return this.imageIndex;
			}
		}

		/// <summary>
		/// ���̃{�^�����I������Ă���� true�A�����łȂ���� false ��Ԃ��܂��B
		/// </summary>
		[Browsable(false)]
		public bool IsSelected
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.Selected.Equals(this);
				}

				return false;
			}
		}

		/// <summary>
		/// ���̃{�^���� Rectangle ���W���擾���܂��B
		/// </summary>
		[Browsable(false)]
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȃ^�u�̕����F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "ControlText")]
		public Color ActiveForeColor
		{
			set
			{
				this.activeForeColor = value;
				this.Update(false);
			}
			get
			{
				return this.activeForeColor;
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȃ^�u�̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "ControlLightLight")]
		public Color ActiveBackColor
		{
			set
			{
				this.activeBackColor = value;
				this.Update(false);
			}
			get
			{
				return this.activeBackColor;
			}
		}

		/// <summary>
		/// ��A�N�e�B�u�ȃ^�u�̕����F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "ControlText")]
		public Color InactiveForeColor
		{
			set
			{
				this.inactiveForeColor = value;
				this.Update(false);
			}
			get
			{
				return this.inactiveForeColor;
			}
		}

		/// <summary>
		/// ��A�N�e�B�u�ȃ^�u�̔w�i�F���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(Color), "Control")]
		public Color InactiveBackColor
		{
			set
			{
				this.inactiveBackColor = value;
				this.Update(false);
			}
			get
			{
				return this.inactiveBackColor;
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȕ\���e�L�X�g�̃t�H���g�t�@�~���[���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public FontFamily ActiveFontFamily
		{
			set
			{
				this.activeFontFamily = value;
				this.Update(true);
			}
			get
			{
				return this.activeFontFamily;
			}
		}

		/// <summary>
		/// �A�N�e�B�u�ȕ\���e�L�X�g�̃t�H���g�X�^�C�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(FontStyle), "Regular")]
		public FontStyle ActiveFontStyle
		{
			set
			{
				this.activeFontStyle = value;
				this.Update(true);
			}
			get
			{
				return this.activeFontStyle;
			}
		}

		/// <summary>
		/// ��A�N�e�B�u�ȕ\���e�L�X�g�̃t�H���g�t�@�~���[���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public FontFamily InactiveFontFamily
		{
			set
			{
				this.inactiveFontFamily = value;
				this.Update(true);
			}
			get
			{
				return this.inactiveFontFamily;
			}
		}

		/// <summary>
		/// ��A�N�e�B�u�ȕ\���e�L�X�g�̃t�H���g�X�^�C�����擾�܂��͐ݒ肵�܂��B
		/// </summary>
		[DefaultValue(typeof(FontStyle), "Regular")]
		public FontStyle InactiveFontStyle
		{
			set
			{
				this.inactiveFontStyle = value;
				this.Update(true);
			}
			get
			{
				return this.inactiveFontStyle;
			}
		}

		/// <summary>
		/// �^�O���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public object Tag
		{
			set
			{
				this.tag = value;
			}
			get
			{
				return this.tag;
			}
		}

		public TabButton()
		{
		}

		public TabButton(string text)
		{
			this.Text = text;
		}

		public TabButton(string text, int imageIndex)
		{
			this.Text = text;
			this.ImageIndex = imageIndex;
		}

		public TabButton(TabButton button)
		{
			this.Text = button.Text;
			this.ImageIndex = button.ImageIndex;

			this.ActiveForeColor = button.ActiveForeColor;
			this.ActiveBackColor = button.ActiveBackColor;
			this.ActiveFontFamily = button.ActiveFontFamily;
			this.ActiveFontStyle = button.ActiveFontStyle;

			this.InactiveForeColor = button.InactiveForeColor;
			this.InactiveBackColor = button.InactiveBackColor;
			this.InactiveFontFamily = button.InactiveFontFamily;
			this.InactiveFontStyle = button.InactiveFontStyle;
		}

		/// <summary>
		/// �{�^���̏�Ԃ��X�V���ꂽ���Ƃ�e�R���g���[���ɒʒm���܂��B
		/// </summary>
		/// <param name="all"></param>
		private void Update(bool all)
		{
			if (this.parent == null)
			{
				return;
			}

			if (all)
			{
				this.parent.UpdateButtons();
			}
			else
			{
				this.parent.UpdateButton(this);
			}
		}

		/// <summary>
		/// �C���X�^���X�̊ȈՃR�s�[���쐬���܂��B
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return new TabButton(this);
		}
	}
}
