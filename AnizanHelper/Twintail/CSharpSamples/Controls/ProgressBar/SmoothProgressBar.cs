// SmoothProgressBar.cs

namespace CSharpSamples
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	/// <summary>
	/// �X���[�X�ȕ\�������̃v���O���X�o�[�B
	/// .NET 2.0 �͕W���ł���̂ŕK�v�Ȃ��Ȃ����B
	/// </summary>
	public class SmoothProgressBar : ProgressCtrl
	{
		private ProgressTextStyle style;
		private Color valueColor;

		/// <summary>
		/// �l�����̐F���擾�܂��͐ݒ�
		/// </summary>
		public Color ValueColor
		{
			set { this.valueColor = value; }
			get { return this.valueColor; }
		}

		/// <summary>
		/// �e�L�X�g�̕\���X�^�C�����擾�܂��͐ݒ�
		/// </summary>
		public ProgressTextStyle TextStyle
		{
			set
			{
				if (value != this.style)
				{
					this.style = value;
					this.Refresh();
				}
			}
			get { return this.style; }
		}

		/// <summary>
		/// SmoothProgressBar�N���X�̃C���X�^���X��������
		/// </summary>
		public SmoothProgressBar() : base()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.ValueColor = SystemColors.Highlight;
			this.ForeColor = Color.Black;
			this.style = ProgressTextStyle.Percent;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics g = e.Graphics;
			Rectangle rect = e.ClipRectangle;

			// �u���V���쐬
			Brush brush = new SolidBrush(this.ValueColor);
			Brush blank = new SolidBrush(SystemColors.Control);

			// position(���݈ʒu)����`��͈͂��v�Z
			float range = (float)(Math.Abs(this.Minimum) + Math.Abs(this.Maximum));
			float pos = range != 0 ? ((float)this.Position / range) : 0;
			float right = rect.Width * pos;

			g.FillRectangle(brush, 0, 0, right, rect.Height);
			g.FillRectangle(blank, right, 0, rect.Width - right, rect.Height);

			// ������`��
			StringFormat format = StringFormat.GenericDefault;
			Brush textbrush = new SolidBrush(this.ForeColor);
			string text = null;

			switch (this.style)
			{
				case ProgressTextStyle.Percent:
					text = this.Percent + "%";
					break;

				case ProgressTextStyle.Length:
					text = string.Format("{0}/{1}", this.Position, this.Maximum);
					break;

				case ProgressTextStyle.None:
					text = string.Empty;
					break;
			}

			// �S�̂̒����ɔz�u
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			// �������`��
			g.DrawString(text, this.Font, textbrush, rect, format);

			// ���E����`��
			Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
			ControlPaint.DrawBorder3D(g, bounds, this.BorderStyle);
		}
	}

	/// <summary>
	/// �v���O���X�ɕ\������e�L�X�g�̕\���X�^�C����\��
	/// </summary>
	public enum ProgressTextStyle
	{
		/// <summary>
		/// �e�L�X�g��\�����Ȃ�
		/// </summary>
		None = 0,
		/// <summary>
		/// �S�����\��
		/// </summary>
		Percent,
		/// <summary>
		/// �S�̂̒�����\��
		/// </summary>
		Length,
	}
}
