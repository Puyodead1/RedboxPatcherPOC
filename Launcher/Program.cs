using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace Launcher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Harmony.DEBUG = true;
            

            string AssemblyFile = Assembly.GetExecutingAssembly().Location;
            string AssemblyFolder = Path.GetFullPath(Path.GetDirectoryName(AssemblyFile) + Path.DirectorySeparatorChar);
            int ModCount = 0;

            string modFolder = Path.Combine(AssemblyFolder, "Patches");
            string modDepsFolder = Path.Combine(modFolder, "Libs");

            if (!Directory.Exists(modFolder))
                Directory.CreateDirectory(modFolder);
            if (!Directory.Exists(modDepsFolder))
                Directory.CreateDirectory(modDepsFolder);


            string targetPath = Path.Combine(AssemblyFolder, "kioskengine_o.exe");
            if (string.IsNullOrEmpty(targetPath) || !File.Exists(targetPath))
                throw new Exception($"Target assembly not found! {targetPath}");

            Console.WriteLine("Loading mods dependencies:");
            string[] modDependencies = Directory.GetFiles(modDepsFolder, "*.dll");
            foreach (var file in modDependencies)
            {
                Console.WriteLine("Loading: " + file);
                Assembly asm = Assembly.LoadFile(file);
                Console.WriteLine("Found assembly: " + asm.ToString());
                Console.WriteLine("AssemblyName: " + asm.GetName());
                Console.WriteLine("Found assembly: " + asm.ToString());
                AppDomain.CurrentDomain.Load(asm.GetName());
            }

            var modsAssemblies = new List<Assembly>();
            Console.WriteLine("Loading mods:");
            foreach (var file in Directory.GetFiles(modFolder, "*.dll"))
            {
                Console.WriteLine("Loading: " + file);
                Assembly mod = Assembly.LoadFile(file);
                modsAssemblies.Add(mod);
                ModCount++;
            }

            //Console.WriteLine($"Loading main assembly: {targetPath}");
            //Assembly main = Assembly.LoadFile(targetPath);


            //Console.WriteLine("Loading main dependencies ...");
            //foreach (var file in main.GetManifestResourceNames())
            //    if (file.Contains(".dll"))
            //    {
            //        Console.WriteLine("Loading: " + file);
            //        Stream input = main.GetManifestResourceStream(file);
            //        Assembly.Load(ReadStreamAssembly(input));
            //    }

            Harmony harmony = new Harmony("me.puyodead1.redbox");
            foreach (var mod in modsAssemblies)
            {
                Console.WriteLine("Harmony.PatchAll() mod: " + mod.GetName().Name);
                try
                {
                    harmony.PatchAll(mod);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Harmony.PatchAll() failed on {mod.GetName().Name}!", ex);
                }
            }

            Console.WriteLine("Assemblies:");
            Array.ForEach(AppDomain.CurrentDomain.GetAssemblies(), entry =>
            {
                Console.WriteLine($"Loaded: {entry.FullName}");
            });

            foreach (var method in harmony.GetPatchedMethods())
                Console.WriteLine($"Patched method: \"{method.Name}\"");

            Console.WriteLine("Starting KioskEngine");
            Thread.Sleep(1000);

            //// Open the Target on an STA thread
            //var thread = new Thread(() =>
            //{
            //    main.EntryPoint.Invoke(null, null);
            //});

            //// Set the new thread to STA
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();

            // Start the target process
            var startInfo = new ProcessStartInfo(targetPath)
            {
                UseShellExecute = true,
                CreateNoWindow = false
            };

            Process.Start(startInfo);
        }

        private static byte[] ReadStreamAssembly(Stream assemblyStream)
        {
            byte[] array = new byte[assemblyStream.Length];
            using (Stream a = assemblyStream)
            {
                a.Read(array, 0, array.Length);
            }
            return array;
        }
    }
}