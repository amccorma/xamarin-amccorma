using System;
using Java.Security;
using System.IO;
using Javax.Crypto.Spec;
using Javax.Crypto;

namespace DeviceEncryption.Android
{
	public class KeyStoreAccess
	{
		private KeyStore keyStore;

		private string filename;

		private Java.Security.KeyStore.PasswordProtection Password
		{
			get
			{
				return new Java.Security.KeyStore.PasswordProtection(filename.ToCharArray());
			}
		}

		public KeyStoreAccess()
		{
			filename = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "pseadata");
			keyStore = KeyStore.GetInstance(KeyStore.DefaultType);

			Java.IO.File fileTest = new Java.IO.File(filename);

			// if an existing keystore is there then use it. Otherwise, create a new,
			// empty keystore
			if (fileTest.Exists() && fileTest.IsFile && fileTest.CanRead() && fileTest.CanWrite())
			{
				//FileInputStream file = new FileInputStream(filename);
				using (System.IO.FileStream file = new FileStream(filename, FileMode.Open))
				{
					keyStore.Load(file, Password.GetPassword());
					file.Close();
				}
			}
			else
			{
				keyStore.Load(null, Password.GetPassword());
			}
		}


		public void Add(string key, string value)
		{
			PBEKeySpec keyspec = new PBEKeySpec(value.ToCharArray());
			SecretKeyFactory fk = SecretKeyFactory.GetInstance("PBEWithMD5andDES");
			ISecretKey mysec = fk.GenerateSecret(keyspec);
			KeyStore.SecretKeyEntry entry = new KeyStore.SecretKeyEntry(mysec);

			keyStore.SetEntry(key, entry, Password);
			Save();
		}

		public void Clear()
		{
			if (keyStore != null)
			{
				keyStore.Dispose();

				// remove the file
				System.IO.File.Delete(filename);
			}
		}

		public bool Contains(string key)
		{
			return keyStore.ContainsAlias(key);
		}

		public Java.Security.KeyStore.IEntry Get(string key)
		{
			if (Contains(key))
			{
				return keyStore.GetEntry(key, Password);
			}
			return null;
		}

		public void Delete(string key)
		{
			if (keyStore.ContainsAlias(key))
			{
				keyStore.DeleteEntry(key);
				Save();
			}
		}

		private void Save()
		{
			using (System.IO.FileStream file = new FileStream(filename, FileMode.OpenOrCreate))
			{
				keyStore.Store(file, Password.GetPassword());
				file.Close();
			};
		}

	}
}