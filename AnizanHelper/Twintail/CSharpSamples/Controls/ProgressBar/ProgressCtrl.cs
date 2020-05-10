// ProgressCtrl.cs

namespace CSharpSamples
{
	using System;
	using System.Windows.Forms;

	/// <summary>
	/// �v���O���X�o�[�̊�{
	/// </summary>
	public abstract class ProgressCtrl : Control
	{
		private int minimum;
		private int maximum;
		private int position;
		private int step;
		private Border3DStyle border;

		/// <summary>
		/// �v���O���X�o�[�̍ŏ��l���擾�܂��͐ݒ�
		/// </summary>
		public int Minimum
		{
			set
			{
				if (value > this.maximum)
				{
					throw new ArgumentOutOfRangeException("Minimum");
				}

				if (this.minimum != value)
				{
					this.minimum = value;
					this.Refresh();
				}
			}
			get { return this.minimum; }
		}

		/// <summary>
		/// �v���O���X�o�[�̍ő�l���擾�܂��͐ݒ�
		/// </summary>
		public int Maximum
		{
			set
			{
				if (value < this.minimum)
				{
					throw new ArgumentOutOfRangeException("Maximum");
				}

				if (this.maximum != value)
				{
					this.maximum = value;
					this.Refresh();
				}
			}
			get { return this.maximum; }
		}

		/// <summary>
		/// �v���O���X�o�[�̌��ݒl���擾�܂��͐ݒ�
		/// </summary>
		public int Position
		{
			set
			{
				if (value < 0 || value > this.maximum)
				{
					throw new ArgumentOutOfRangeException("Position");
				}

				if (this.position != value)
				{
					this.position = value;
					this.Refresh();
				}
			}
			get { return this.position; }
		}

		/// <summary>
		/// PerformStep���\�b�h���g�p�������̑��ʕ����擾�܂��͐ݒ�
		/// </summary>
		public int Step
		{
			set
			{
				if (value > this.maximum)
				{
					throw new ArgumentOutOfRangeException("Step");
				}

				if (this.step != value)
				{
					this.step = value;
				}
			}
			get { return this.step; }
		}

		/// <summary>
		/// �v���O���X�o�[�̋��E�����擾�܂��͐ݒ�
		/// </summary>
		public Border3DStyle BorderStyle
		{
			set
			{
				if (value != this.border)
				{
					this.border = value;
					this.Refresh();
				}
			}
			get { return this.border; }
		}

		/// <summary>
		/// �S�������擾
		/// </summary>
		protected int Percent
		{
			get
			{
				float range = (float)(Math.Abs(this.minimum) + Math.Abs(this.maximum));

				if (range == 0)
				{
					return 0;
				}

				float result = (float)this.position / range * 100.0f;
				return (int)result;
			}
		}

		/// <summary>
		/// ProgressCtrl�N���X�̃C���X�^���X��������
		/// </summary>
		public ProgressCtrl()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			this.border = Border3DStyle.SunkenOuter;
			this.minimum = 0;
			this.position = 0;
			this.maximum = 100;
			this.step = 1;
		}

		/// <summary>
		/// ���݂̈ʒu����Step�̕������i�߂�
		/// </summary>
		public virtual void PerformStep()
		{
			this.Increment(this.Step);
		}

		/// <summary>
		/// �w�肵���ʂ������݈ʒu��i�߂�
		/// </summary>
		/// <param name="value">���݈ʒu���C���N�������g�����</param>
		public virtual void Increment(int value)
		{
			if (this.Position + value >= this.Maximum)
			{
				this.Position = this.Maximum;
			}
			else
			{
				this.Position += value;
			}
		}

		/// <summary>
		/// ���݈ʒu���ŏ��l�Ƀ��Z�b�g
		/// </summary>
		public virtual void Reset()
		{
			this.Position = 0;
		}
	}
}
