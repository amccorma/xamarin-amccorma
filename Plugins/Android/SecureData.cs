using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Javax.Crypto.Spec;
using Java.Security;
using Javax.Crypto;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceEncryption.Android.SecureData))]
namespace DeviceEncryption.Android
{
	public class SecureData : ISecure
	{
		private static Lazy<KeyStoreAccess> KeyStore = new Lazy<KeyStoreAccess>();
		private static String algorithm = "AES";
		private static byte[] keyValue = new byte[] {20,21,33,44,55,66,77,88,19,5,4,9,14,23,3,1,8,2,32,41,6,4,0,23,98,12,24,6,6,10,7,3};


//		public List<SecurityTypes> ListSupportedAlgorithms() {
//			List<SecurityTypes> results = new List<object> ();;
//
//			// get all the providers
//			Provider[] providers = Security.GetProviders ();
//
//			for (int p = 0; p < providers.Length; p++) {
//				// get all service types for a specific provider
//				var servicetypes = new List<String>();
//				foreach (var key in providers[p].Services) {
//					results.Add (new SecurityTypes 
//					{
//						ProviderName =  key.Provider.Name, 
//						Algorithm = key.Algorithm
//					});
//				}
//			}
//
//			return results;
//		}

		public SecureData()
		{

		}

		private SecretKeySpec AES
		{
			get
			{
				try {
					return new SecretKeySpec(keyValue, algorithm);
				} catch (Exception) {
					return null;
				}
			}
		}

		public byte[] KeyGenerator(string AndroidKey)
		{
			SecureRandom sr = SecureRandom.GetInstance("SHA1PRNG");
			var encoder = new System.Text.UnicodeEncoding();

			var b = encoder.GetBytes (AndroidKey);

			// use key
			sr.SetSeed(encoder.GetBytes(AndroidKey));

			return encoder.GetBytes (AndroidKey);


//			Cipher c = Cipher.GetInstance("AES");
//			c.Init(CipherMode.EncryptMode, 
//			KeyGenerator kg = KeyGenerator.GetInstance("AES");
//			kg.Init(128, sr);
//			return new SecretKeySpec (kg.GenerateKey ().GetEncoded (), "AES").GetEncoded ();
		}

		private object[] RSA
		{
			get
			{
				IPublicKey publicKey = null;
				IPrivateKey privateKey = null;
				try {
					KeyPairGenerator kpg = KeyPairGenerator.GetInstance("RSA");
					kpg.Initialize(1024);
					KeyPair kp = kpg.GenKeyPair();
					publicKey = kp.Public;
					privateKey = kp.Private;
					return new object[] { privateKey, publicKey };
				} catch (Exception) {
					return null;
				}
			}
		}

		public Encrypt Encode(EncryptType type, string value)
		{
			var result = new Encrypt ();
			result.Value = null;
			result.Type = type;
			if (type == EncryptType.OK) {
				try {
					var key = AES;
					if (key != null)
					{
						result.Key = key;
						Cipher c = Cipher.GetInstance("AES");
						c.Init(CipherMode.EncryptMode, key as IKey);
						var encoder = new System.Text.UnicodeEncoding();
						result.Value = c.DoFinal(encoder.GetBytes(value));
						return result;
					}
				} 
				catch (Exception) {
					return null;
				}
			} else if (type == EncryptType.STRONG) {
				try 
				{
					var keys = RSA;
					if (keys != null)
					{
						result.PublicKey = keys[1];
						result.PrivateKey = keys[0];
						Cipher c = Cipher.GetInstance ("RSA");
						c.Init(Javax.Crypto.CipherMode.EncryptMode, result.PublicKey as IKey);
						var encoder = new System.Text.UnicodeEncoding();
						result.Value = c.DoFinal (encoder.GetBytes(value));
						return result;
					}
				} 
				catch (Exception) {
					return null;
				}
			}

			return null;
		}

		public string Decode(Encrypt obj)
		{
			if (obj.Type == EncryptType.OK) {
				try {
					Cipher c = Cipher.GetInstance("AES");
					//c.Init(Javax.Crypto.CipherMode.DecryptMode, obj.Key as IKey);
					c.Init(Javax.Crypto.CipherMode.DecryptMode, AES as IKey);
					var	decodedBytes = c.DoFinal(obj.Value);
					return System.Text.Encoding.Unicode.GetString(decodedBytes);
				} 
				catch (Exception) {
					return null;
				}
			} 
			else if (obj.Type == EncryptType.STRONG) {
				try {
					Cipher c = Cipher.GetInstance ("RSA");
					var k = obj.PublicKey as IKey;
					c.Init(Javax.Crypto.CipherMode.DecryptMode, obj.PrivateKey as IKey);
					var decodedBytes = c.DoFinal (obj.Value);
					return System.Text.Encoding.Unicode.GetString(decodedBytes);
				} catch (Exception) {
					return null;
				}
			}

			return null;
		}


		public bool Exists(string key)
		{
			return KeyStore.Value.Contains (key);
		}

		public string Get(string key)
		{
			try
			{
				var value = KeyStore.Value.Get (key) as KeyStore.SecretKeyEntry;
				return System.Text.Encoding.Default.GetString (value.SecretKey.GetEncoded());
			}
			catch (Exception) {
				return String.Empty;
			}
		}

		public bool Delete(string key)
		{
			try
			{
				KeyStore.Value.Delete(key);
				return true;
			}
			catch(Exception) {
				return false;
			}
		}

		public bool Save(string key, string value)
		{
			try
			{
				KeyStore.Value.Add(key, value);
				return true;
			}
			catch(Exception) {
				return false;
			}
		}

		public bool Clear()
		{
			try
			{
				KeyStore.Value.Clear ();
				return true;
			}
			catch {
				return false;
			}
		}
	}
}