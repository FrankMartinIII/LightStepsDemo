using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("currentSegment", "loadedNeighbors", "curColor")]
	public class ES3UserType_GameManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameManager() : base(typeof(GameManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (GameManager)obj;
			
			writer.WritePropertyByRef("currentSegment", instance.currentSegment);
			writer.WriteProperty("loadedNeighbors", instance.loadedNeighbors, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<LevelSegment>)));
			writer.WriteProperty("curColor", instance.curColor, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(ColorSystem.Colors)));
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (GameManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "currentSegment":
						instance.currentSegment = reader.Read<LevelSegment>();
						break;
					case "loadedNeighbors":
						instance.loadedNeighbors = reader.Read<System.Collections.Generic.List<LevelSegment>>();
						break;
					case "curColor":
						instance.curColor = reader.Read<ColorSystem.Colors>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_GameManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameManagerArray() : base(typeof(GameManager[]), ES3UserType_GameManager.Instance)
		{
			Instance = this;
		}
	}
}