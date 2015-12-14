package md510b4ee4d897ae283fddefe9c3e1ad44c;


public class TextHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_toString:()Ljava/lang/String;:GetToStringHandler\n" +
			"";
		mono.android.Runtime.register ("MaskedEditAndroid.Mask.Control.TextHolder, MaskedEditAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TextHolder.class, __md_methods);
	}


	public TextHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TextHolder.class)
			mono.android.TypeManager.Activate ("MaskedEditAndroid.Mask.Control.TextHolder, MaskedEditAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public TextHolder (java.lang.String p0, int p1, int p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == TextHolder.class)
			mono.android.TypeManager.Activate ("MaskedEditAndroid.Mask.Control.TextHolder, MaskedEditAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public java.lang.String toString ()
	{
		return n_toString ();
	}

	private native java.lang.String n_toString ();

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
