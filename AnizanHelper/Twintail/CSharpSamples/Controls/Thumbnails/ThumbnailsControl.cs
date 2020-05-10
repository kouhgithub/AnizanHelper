// ThumbnailsControl.cs

namespace CSharpSamples
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Threading;
	using System.Windows.Forms;
	using CSharpSamples.Winapi;

	/// <summary>
	/// ���X�g�r���[�ɉ摜���k���\���ł���@�\��ǉ�����N���X
	/// </summary>
	public class ThumbnailsControl : Control
	{
		private ListView listView;
		private ImageList imageList;

		private Dictionary<string, int> imageIndices; // key �̓t�@�C�����Avalue �� �쐬�ς݃T���l�C���� ImageIndex�B
		private Queue<ListViewItem> queue; // �T���l�C�����쐬�� ListViewItem ���i�[����L���[�B
		private ImageFilter filter;

		private ManualResetEvent resetEvent = new ManualResetEvent(false);
		private Thread thread = null;

		private bool disposed = false;

		/// <summary>
		/// �T���l�C���̉摜�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public Size ImageSize
		{
			set
			{
				lock (this.imageList)
				{
					this.imageList.ImageSize = value;
				}
			}
			get
			{
				lock (this.imageList)
				{
					return this.imageList.ImageSize;
				}
			}
		}

		/// <summary>
		/// �摜�ɂ�����t�B���^�[���擾�܂��͐ݒ�
		/// </summary>
		public ImageFilter Filter
		{
			set
			{
				if (this.filter != value)
				{
					this.filter = value;
				}
			}
			get { return this.filter; }
		}

		/// <summary>
		/// ���ׂẴT���l�C���̌��t�@�C�������擾
		/// </summary>
		public string[] AllItems
		{
			get
			{
				string[] allItems = new string[this.imageIndices.Count];
				this.imageIndices.Keys.CopyTo(allItems, 0);

				return allItems;
			}
		}

		/// <summary>
		/// �I������Ă���T���l�C���̌��t�@�C���̃p�X���擾
		/// </summary>
		public string[] SelectedItems
		{
			get
			{
				List<string> list = new List<string>();

				foreach (ListViewItem item in this.listView.SelectedItems)
				{
					list.Add(item.Tag as string);
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// �R���e�L�X�g���j���[���擾�܂��͐ݒ�
		/// </summary>
		public override ContextMenu ContextMenu
		{
			set
			{
				this.listView.ContextMenu = value;
			}
			get
			{
				return this.listView.ContextMenu;
			}
		}

		/// <summary>
		/// �T���l�C���摜�f�[�^���擾
		/// </summary>
		public ImageList Thumbnails
		{
			get
			{
				lock (this.imageList)
				{
					return this.imageList;
				}
			}
		}

		/// <summary>
		/// ThumbnailsControl�N���X�̃C���X�^���X��������
		/// </summary>
		public ThumbnailsControl()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.imageList = new ImageList();
			this.imageList.ColorDepth = ColorDepth.Depth32Bit;
			this.imageList.TransparentColor = Color.White;
			this.imageList.ImageSize = new Size(100, 100);

			this.listView = new ListView();
			this.listView.Dock = DockStyle.Fill;
			this.listView.View = View.LargeIcon;
			this.listView.LargeImageList = this.listView.SmallImageList = this.imageList;
			this.Controls.Add(this.listView);

			this.filter = ImageFilter.None;
			this.queue = new Queue<ListViewItem>();
			this.imageIndices = new Dictionary<string, int>();

			WinApi.SetWindowTheme(this.listView.Handle, "explorer", null);
		}

		/// <summary>
		/// �g�p���Ă��郊�\�[�X�����
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}

			if (disposing)
			{
				this.Abort();

				this.listView.Items.Clear();
				this.listView.LargeImageList = null;

				this.imageList.Dispose();
				this.imageList = null;

				this.disposed = true;
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// �w�肵���t�@�C���̃T���l�C�����쐬���\������
		/// </summary>
		/// <param name="fileName">�T���l�C���\������t�@�C����</param>
		public void Add(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}

			this.AddRange(new string[] { fileName });
		}

		/// <summary>
		/// �w�肵���t�@�C���̃T���l�C�����쐬���\������
		/// </summary>
		/// <param name="fileNames">�T���l�C���\������t�@�C�����̔z��</param>
		public void AddRange(string[] fileNames)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}

			List<ListViewItem> list = new List<ListViewItem>();

			// ��[�A�C�e��������ǉ�
			foreach (string filename in fileNames)
			{
				ListViewItem item = new ListViewItem(Path.GetFileName(filename));
				item.ImageIndex = -1;
				item.Tag = filename;

				lock (this.queue)
				{
					this.queue.Enqueue(item);
				}

				list.Add(item);
			}

			lock (this.listView)
			{
				this.listView.Items.AddRange(list.ToArray());
			}

			this.ThreadRun();
		}

		/// <summary>
		/// �w�肵���t�@�C�����̃T���l�C�����폜
		/// </summary>
		/// <param name="filename"></param>
		public void Remove(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}

			for (int i = this.listView.Items.Count - 1; i >= 0; i--)
			{
				lock (this.listView)
				{
					if (filename.Equals((string)this.listView.Items[i].Tag))
					{
						this.listView.Items.RemoveAt(i);
					}
				}
			}
		}

		/// <summary>
		/// �T���l�C�������X���b�h���N��
		/// </summary>
		private void ThreadRun()
		{
			this.resetEvent.Set();
			if (this.thread == null)
			{
				this.thread = new Thread(new ThreadStart(this.__GenerateThread));
				this.thread.Name = "THUMB_CTRL";
				this.thread.IsBackground = true;
				this.thread.Priority = ThreadPriority.Lowest;
				this.thread.Start();
			}
		}

		/// <summary>
		/// �T���l�C���̐����𒆎~����
		/// </summary>
		private void Abort()
		{
			if (this.thread != null && this.thread.IsAlive)
			{
				this.thread.Abort();
			}

			this.thread = null;
		}

		/// <summary>
		/// �T���l�C�����N���A
		/// </summary>
		public void Clear()
		{
			this.resetEvent.Reset();
			this.listView.Items.Clear();
			lock (this.imageIndices)
			{
				this.imageIndices.Clear();
			}

			lock (this.imageList)
			{
				this.imageList.Images.Clear();
			}
		}

		private void __GenerateThread()
		{
			while (true)
			{
				this.resetEvent.WaitOne();

				if (this.queue.Count > 0)
				{
					ListViewItem item;

					lock (this.queue)
					{
						item = (ListViewItem)this.queue.Dequeue();
					}

					string filename = (string)item.Tag;

					int imageIndex;
					lock (this.imageIndices)
					{
						// ���ɃT���l�C�����쐬����Ă��Ȃ����ǂ������`�F�b�N
						if (this.imageIndices.ContainsKey(filename))
						{
							imageIndex = this.imageIndices[filename];
						}
						// �V�K�ɃT���l�C�����쐬���A�쐬���ꂽ�T���l�C���̃C���f�b�N�X���擾�B
						else
						{
							imageIndex = this.CreateThumbnail(filename);
						}
						// �쐬�ς݃T���l�C���̃C���f�b�N�X�ԍ���ۑ�
						this.imageIndices[filename] = imageIndex;
					}

					// ImageIndex ��ݒ�
					MethodInvoker m = delegate { item.ImageIndex = imageIndex; };
					this.Invoke(m);
				}
				else
				{
					this.resetEvent.Reset();
				}
			}
		}

		/// <summary>
		/// �w�肵���t�@�C���̃T���l�C���摜���쐬
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private int CreateThumbnail(string fileName)
		{
			int imageIndex = -1;

			byte[] bytes = this.LoadData(fileName);
			if (bytes == null)
			{
				lock (this.imageList)
				{
					MethodInvoker m = delegate
					{
						imageIndex = this.imageList.Images.Add(this.GetErrorImage(),
							this.imageList.TransparentColor);
					};
					this.Invoke(m);
				}
				return imageIndex;
			}

			MemoryStream mem = new MemoryStream(bytes);

			using (Image source = new Bitmap(mem))
			{
				float width = (float)this.ImageSize.Width / source.Width;
				float height = (float)this.ImageSize.Height / source.Height;
				float percent = Math.Min(width, height);

				width = (source.Width * percent);
				height = (source.Height * percent);

				Rectangle rect = new Rectangle(
					(int)((this.ImageSize.Width - width) / 2),
					(int)((this.ImageSize.Height - height) / 2),
					(int)width, (int)height);

				Image buffer = new Bitmap(this.ImageSize.Width, this.ImageSize.Height);

				using (Graphics g = Graphics.FromImage(buffer))
				{
					using (Image thumb = source.GetThumbnailImage((int)width, (int)height,
							delegate { return false; }, IntPtr.Zero))
					{
						this._SetFilter(thumb as Bitmap);

						lock (this.imageList)
						{
							g.Clear(this.imageList.TransparentColor);
						}

						g.DrawImage(thumb, rect);
					}
				}

				lock (this.imageList)
				{
					MethodInvoker m = delegate
					{
						imageIndex = this.imageList.Images.Add(buffer,
							this.imageList.TransparentColor);
					};
					this.Invoke(m);
				}
			}

			return imageIndex;
		}

		private Image GetErrorImage()
		{
			Image image = new Bitmap(this.ImageSize.Width, this.ImageSize.Height);
			using (Graphics g = Graphics.FromImage(image))
			{
				g.DrawLine(Pens.Red, 0, 0, this.ImageSize.Width, this.ImageSize.Height);
				g.DrawLine(Pens.Red, this.ImageSize.Width, 0, 0, this.ImageSize.Height);
			}
			return image;
		}

		/// <summary>
		/// �w�肵���摜�Ƀt�B���^��������
		/// </summary>
		/// <param name="image"></param>
		private bool _SetFilter(Bitmap image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			switch (this.filter)
			{
				case ImageFilter.Alpha:
					return BitmapFilter.Brightness(image, 100);

				case ImageFilter.Mosaic:
					return BitmapFilter.Pixelate(image, 3, false);

				case ImageFilter.GrayScale:
					return BitmapFilter.GrayScale(image);

				default:
					return false;
			}
		}

		protected virtual byte[] LoadData(string uri)
		{
			return File.ReadAllBytes(uri);
		}
	}

	/// <summary>
	/// �摜�ɂ�����t�B���^�[��\���񋓑�
	/// </summary>
	public enum ImageFilter
	{
		/// <summary>�w��Ȃ�</summary>
		None,
		/// <summary>�A���t�@����</summary>
		Alpha,
		/// <summary>���U�C�N</summary>
		Mosaic,
		/// <summary>�O���[�X�P�[��</summary>
		GrayScale,
	}
}
