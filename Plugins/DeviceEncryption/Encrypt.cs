using System;

namespace DeviceEncryption
{
	public class Encrypt
	{
		public object PrivateKey
		{
			get;
			set;
		}

		public object PublicKey
		{
			get;
			set;
		}

		public object Key
		{
			get;
			set;
		}

		public byte[] Value
		{
			get;
			set;
		}

		public EncryptType Type { get; set; }
	}
}
