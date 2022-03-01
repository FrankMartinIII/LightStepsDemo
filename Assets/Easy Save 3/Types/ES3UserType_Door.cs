using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("closedDoor", "sourcesNeededToOpen", "isLocked", "sourcesPowering", "curColor", "isVisible", "onSprite", "offSprite", "thisCollider")]
	public class ES3UserType_Door : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Door() : base(typeof(Door)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Door)obj;
			
			writer.WritePrivateFieldByRef("closedDoor", instance);
			writer.WriteProperty("sourcesNeededToOpen", instance.sourcesNeededToOpen, ES3Type_int.Instance);
			writer.WriteProperty("isLocked", instance.isLocked, ES3Type_bool.Instance);
			writer.WritePrivateField("sourcesPowering", instance);
			writer.WritePrivateField("curColor", instance);
			writer.WritePrivateField("isVisible", instance);
			writer.WritePrivateFieldByRef("onSprite", instance);
			writer.WritePrivateFieldByRef("offSprite", instance);
			writer.WritePrivateFieldByRef("thisCollider", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Door)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "closedDoor":
					reader.SetPrivateField("closedDoor", reader.Read<UnityEngine.GameObject>(), instance);
					break;
					case "sourcesNeededToOpen":
						instance.sourcesNeededToOpen = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "isLocked":
						instance.isLocked = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "sourcesPowering":
					reader.SetPrivateField("sourcesPowering", reader.Read<System.Int32>(), instance);
					break;
					case "curColor":
					reader.SetPrivateField("curColor", reader.Read<ColorSystem.Colors>(), instance);
					break;
					case "isVisible":
					reader.SetPrivateField("isVisible", reader.Read<System.Boolean>(), instance);
					break;
					case "onSprite":
					reader.SetPrivateField("onSprite", reader.Read<UnityEngine.Sprite>(), instance);
					break;
					case "offSprite":
					reader.SetPrivateField("offSprite", reader.Read<UnityEngine.Sprite>(), instance);
					break;
					case "thisCollider":
					reader.SetPrivateField("thisCollider", reader.Read<UnityEngine.Collider2D>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_DoorArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_DoorArray() : base(typeof(Door[]), ES3UserType_Door.Instance)
		{
			Instance = this;
		}
	}
}