using HarmonyLib;
using Redbox.Rental.Services.KioskProduct;
using System;

namespace RedboxPatches.RedboxRentalServices
{
    [HarmonyPatch(typeof(TitleProduct), "get_DoNotRentDate")]
    public class DoNotRentDatePatch
    {
        static bool Prefix(TitleProduct __instance, ref DateTime? __result)
        {
            __result = null;
            return false;
        }
    }
}
