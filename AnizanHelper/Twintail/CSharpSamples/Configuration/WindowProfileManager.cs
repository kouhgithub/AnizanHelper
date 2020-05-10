// WindowProfileManager.cs

using System;
using System.Drawing;
using System.Windows.Forms;

namespace CSharpSamples
{
	/// <summary>
	/// Form �E�C���h�E�̏�Ԃ� .ini �`���ŕۑ�/��������N���X�B
	/// </summary>
	public class WindowProfileManager
	{
		private Form form = null;
		private Rectangle normalWindowRect = Rectangle.Empty;

		/// <summary>
		/// WindowProfileManager �N���X�̃C���X�^���X���������B
		/// </summary>	
		public WindowProfileManager(Form f)
		{
			if (f == null)
			{
				throw new ArgumentNullException("f");
			}
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//

			this.normalWindowRect =
				new Rectangle(f.Location, f.ClientSize);

			this.form = f;
			this.form.Move += new EventHandler(this.OnResize);
			this.form.Resize += new EventHandler(this.OnResize);
		}

		private void OnResize(object sender, EventArgs e)
		{
			if (this.form.WindowState == FormWindowState.Normal)
			{
				this.normalWindowRect = new Rectangle(
					this.form.Location, this.form.ClientSize);
			}
		}

		public void Serialize(string fileName)
		{
			CSPrivateProfile prof = new CSPrivateProfile();
			this.Save(prof);

			prof.Write(fileName);
		}

		public void Deserialize(string fileName)
		{
			CSPrivateProfile prof = new CSPrivateProfile();
			prof.Read(fileName);

			this.Load(prof);
		}

		public virtual void Save(CSPrivateProfile prof)
		{
			prof.SetValue("Window", "Bounds", this.normalWindowRect);
			prof.SetValue("Window", "State", this.form.WindowState);
		}

		public virtual void Load(CSPrivateProfile prof)
		{
			this.form.WindowState = (FormWindowState)
				prof.GetEnum("Window", "State", this.form.WindowState);

			Rectangle rc = prof.GetRect("Window", "Bounds", this.normalWindowRect);
			this.form.Location = rc.Location;
			this.form.ClientSize = rc.Size;
		}
	}
}
