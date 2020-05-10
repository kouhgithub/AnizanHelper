// ColorProgressBar.cs

namespace CSharpSamples
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	/// <summary>
	/// �J���t���ȃv���O���X�o�[
	/// </summary>
	public class ColorProgressBar : ProgressCtrl
	{
		private Color[] colors;
		private Color borderColor;
		private int scaleSize;
		private int borderSize;

		/// <summary>
		/// �ڐ���̃T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public int ScaleSize
		{
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("ScaleSize��1�ȏ�̒l�łȂ���΂Ȃ�܂���");
				}
				this.scaleSize = value;
			}
			get { return this.scaleSize; }
		}

		/// <summary>
		/// ���E�̐F���擾�܂��͐ݒ�
		/// </summary>
		public Color BorderColor
		{
			set { this.borderColor = value; }
			get { return this.borderColor; }
		}

		/// <summary>
		/// �v���O���X�o�[�̐F�z����擾�܂��͐ݒ�
		/// </summary>
		public Color[] Colors
		{
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Colors");
				}
				this.colors = value;
				this.Refresh();
			}
			get { return this.colors; }
		}

		/// <summary>
		/// ColorProgressBar�N���X�̃C���X�^���X��������
		/// </summary>
		public ColorProgressBar() : base()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.scaleSize = 8;
			this.borderSize = 1;
			this.borderColor = Color.Black;
			this.BackColor = Color.DarkGreen;
			this.BorderStyle = Border3DStyle.Flat;
			this.colors = new Color[1] { Color.Green };
		}

		/// <summary>
		/// �ڐ����`��
		/// </summary>
		/// <param name="g"></param>
		private void DrawScale(Graphics g)
		{
			Pen pen = new Pen(this.borderColor, this.borderSize);
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

			// �l�p��`��
			g.DrawRectangle(pen, rect);

			// �c����`��
			Point from = new Point(0, 0);
			Point to = new Point(0, this.Height);

			for (int i = this.Minimum; i <= this.Maximum; i++)
			{
				from.X = (this.scaleSize + this.borderSize) * i;
				to.X = (this.scaleSize + this.borderSize) * i;
				g.DrawLine(pen, from, to);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			// �ڐ����`��
			this.DrawScale(g);

			// �P�̐F�ɑ΂��Ẵ����������v�Z
			int clrCount = this.Maximum / this.colors.Length;

			// ���W
			Rectangle rect = new Rectangle(this.borderSize, this.borderSize,
				this.scaleSize, this.Height - this.borderSize * 2);
			SolidBrush brush = null;

			for (int i = this.Minimum; i < this.Position; i++)
			{
				int clridx = i / clrCount;

				if (clridx >= this.colors.Length)
				{
					clridx = this.colors.Length - 1;
				}

				Color color = this.colors[clridx];

				// �u���V���쐬
				if (brush == null || brush.Color != color)
				{
					brush = new SolidBrush(color);
				}

				g.FillRectangle(brush, rect);
				rect.X += this.scaleSize + this.borderSize;
			}

			// ���E����`��
			Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
			ControlPaint.DrawBorder3D(g, bounds, this.BorderStyle);
		}

		/// <summary>
		/// �ڐ���ƕ��𒚓x�����悤�Ƀ��T�C�Y
		/// </summary>
		public void ResizeBar()
		{
			this.Width = (this.ScaleSize + this.borderSize) * this.Maximum + this.borderSize;
		}
	}

}
