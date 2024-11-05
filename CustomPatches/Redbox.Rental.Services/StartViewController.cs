using HarmonyLib;
using Redbox.Rental.Services.Controllers;
using System;
using System.Reflection;
using System.Windows;
using Redbox.Rental.UI.Models;

namespace CustomPatches.Redbox.Rental.Services
{
    /**
     * This patch adds the action for clicking the Redbox Logo on the start screen
     * Requires a modified Redbox.Rental.UI.exe
     */
    [HarmonyPatch(typeof(StartViewController))]
    [HarmonyPatch("ConfigureStartView")]
    public static class RedboxLogoCommandAdditionPatch
    {
        public static void Postfix()
        {

            // Get a reference to the private GetStartViewUserControl method
            MethodInfo getStartViewUserControlMethod = AccessTools.Method(typeof(StartViewController), "GetStartViewUserControl");

            if (getStartViewUserControlMethod == null)
            {
                Console.WriteLine("Failed to retrieve GetStartViewUserControl method.");
                return;
            }

            // Invoke the private method
            var startViewUserControl = getStartViewUserControlMethod.Invoke(null, null);
            if (startViewUserControl is FrameworkElement element && element.DataContext is StartViewModel startViewModel)
            {
                // Add the new command by calling the wrapper method in StartViewControllerExtensions
                startViewModel.OnRedboxLogoCommand += new Action(StartViewControllerExtensions.RedboxLogoCommand);
            }
        }
    }
}
