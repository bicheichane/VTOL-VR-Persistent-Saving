using System.IO;
using HarmonyLib;

//Based off of https://gist.github.com/ThatGenericName/11ecf30ef45b5fb93eef97d02366f8e7#file-quickloadfromfile-cs

namespace VTOLPersistentSave
{

	[HarmonyPatch(typeof(MFDQuicksavePage), nameof(MFDQuicksavePage.OnEnable))]
	public class QuickLoadFromFile
	{

		public static bool QuicksaveOverride = true;

		/// <summary>
		/// despite VTOL:VR saving the quicksave to a file, it never attempts to actually load from this file, instead
		/// only attempting to load data from ram if it exists there.
		/// This forces the quicksave manager to reload from file if there is no quicksave in ram,
		/// whenever the MFD Quicksave page is opened.
		/// </summary>
		/// <returns>Always returns true as we always want the base quickload logic to run</returns>
		[HarmonyPrefix]
		public static bool QuicksaveOverridePrefix()
		{

			if (!QuicksaveOverride)
			{
				Log("Quickload override disabled");
				return true;
			}

			Log("In-Memory quicksave override enabled");

			ConfigNode qsn = (ConfigNode)Traverse.Create(typeof(QuicksaveManager)).Field("quicksaveNode").GetValue();
			
			if (qsn != null)
			{
				Log("Quicksave file already exists in memory, disabling override functionality");
				QuicksaveOverride = false;
				return true;
			}

			Log("Reading quicksave file...");
			ConfigNode newQsn = ConfigNode.LoadFromFile(Path.Combine(PilotSaveManager.saveDataPath, "quicksave.cfg"), true);

			Log("Overriding quicksave config node");
			Traverse.Create(typeof(QuicksaveManager)).Field("quicksaveNode").SetValue(newQsn);

			Log("Override attempt successful. Disabling further attempts");

			QuicksaveOverride = false;

			return true;
		}

	}
}