using System;

namespace DeviceEncryption
{
	public interface ISecure
	{
		Encrypt Encode(EncryptType type, string value);
		string Decode(Encrypt obj);

		string Get(string key);
		bool Delete(string key);
		bool Save(string key, string value);
		bool Exists(string key);
		bool Clear();
	}
}
