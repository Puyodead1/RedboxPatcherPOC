using HarmonyLib;
using Redbox.Rental.Model.KioskProduct;
using Redbox.Rental.Services.KioskProduct;
using System.Windows;

namespace RedboxPatches.RedboxRentalServices
{
    [HarmonyPatch(typeof(TitleProduct), "get_IsRentable")]
    public class IsRentablePatch
    {
        static bool Prefix(TitleProduct __instance, ref bool __result)
        {
            __result = __instance.IsEmptyCase || (!__instance.DoNotRent && __instance.IsValidProduct && __instance.TitleType != TitleType.DigitalCode && __instance.IsAvailable);
            return false;
        }
    }
}
