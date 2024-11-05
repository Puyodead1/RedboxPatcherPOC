using HarmonyLib;
using System.IO;
using System;
using System.Reflection;

namespace Launcher
{
    public class HarmonyHelper
    {
        public static void Patch()
        {
            var harmony = new Harmony("me.puyodead1.redbox");

            // Load the patch library
            string patchDllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RedboxPatches.dll");

            // Check if the patch DLL exists
            if (!File.Exists(patchDllPath))
            {
                Console.WriteLine("Patch DLL not found.");
                return;
            }
            harmony.PatchAll(Assembly.LoadFrom(patchDllPath));
        }
    }
}
