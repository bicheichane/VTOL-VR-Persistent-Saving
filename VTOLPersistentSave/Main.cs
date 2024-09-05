global using static VTOLPersistentSave.Logger;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using System.IO;
using System.Reflection;

namespace VTOLPersistentSave
{
	[ItemId("jehuty.persistentSave")] // Harmony ID for your mod, make sure this is unique
	public class Main : VtolMod
	{
		private void Awake()
		{
		}


		public override void UnLoad()
		{
			// Destroy any objects
		}
	}
}