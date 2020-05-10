// Property.cs

namespace CSharpSamples
{
	using System;
	using System.Collections;

	/// <summary>
	/// �P�̃v���p�e�B��\��
	/// </summary>
	public class Property
	{
		private string caption;
		private object data;

		/// <summary>
		/// �L���v�V�������擾�܂��͐ݒ�
		/// </summary>
		public string Caption
		{
			set { this.caption = value; }
			get { return this.caption; }
		}

		/// <summary>
		/// �f�[�^���擾�܂��͐ݒ�
		/// </summary>
		public object Data
		{
			set { this.data = value; }
			get { return this.data; }
		}


		/// <summary>
		/// Property�N���X���R���N�V�����Ǘ�
		/// </summary>
		public class PropertyCollection : CollectionBase
		{
			private PropertyDialog owner;

			/// <summary>
			/// indexor
			/// </summary>
			public Property this[int index]
			{
				get
				{
					return (Property)this.List[index];
				}
			}

			internal PropertyCollection(PropertyDialog owner)
			{
				if (owner == null)
				{
					throw new ArgumentNullException("owner");
				}

				this.owner = owner;
			}

			public int Add(Property property)
			{
				this.owner.CreatePage(property);
				return this.List.Add(property);
			}

			public void AddRange(PropertyCollection properties)
			{
				foreach (Property property in properties)
				{
					this.Add(property);
				}
			}

			public void AddRange(Property[] properties)
			{
				foreach (Property property in properties)
				{
					this.Add(property);
				}
			}

			public void Remove(Property property)
			{
				this.owner.RemovePage(property);
				this.List.Remove(property);
			}
		}
	}
}
