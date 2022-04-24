using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("hasGrapple", "launchDistance", "grappleSpeed", "pullSpeed", "abortPullTimer", "lr")]
	public class ES3UserType_PlayerGrapple : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PlayerGrapple() : base(typeof(PlayerGrapple)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (PlayerGrapple)obj;
			
			writer.WriteProperty("hasGrapple", instance.hasGrapple, ES3Type_bool.Instance);
			writer.WritePrivateField("launchDistance", instance);
			writer.WritePrivateField("grappleSpeed", instance);
			writer.WritePrivateField("pullSpeed", instance);
			writer.WritePrivateField("abortPullTimer", instance);
			writer.WritePrivateFieldByRef("lr", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (PlayerGrapple)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "hasGrapple":
						instance.hasGrapple = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "launchDistance":
					reader.SetPrivateField("launchDistance", reader.Read<System.Int32>(), instance);
					break;
					case "grappleSpeed":
					reader.SetPrivateField("grappleSpeed", reader.Read<System.Single>(), instance);
					break;
					case "pullSpeed":
					reader.SetPrivateField("pullSpeed", reader.Read<System.Single>(), instance);
					break;
					case "abortPullTimer":
					reader.SetPrivateField("abortPullTimer", reader.Read<System.Single>(), instance);
					break;
					case "lr":
					reader.SetPrivateField("lr", reader.Read<UnityEngine.LineRenderer>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_PlayerGrappleArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PlayerGrappleArray() : base(typeof(PlayerGrapple[]), ES3UserType_PlayerGrapple.Instance)
		{
			Instance = this;
		}
	}
}