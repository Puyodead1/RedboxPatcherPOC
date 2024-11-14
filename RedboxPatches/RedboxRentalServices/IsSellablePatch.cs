using HarmonyLib;
using Redbox.Rental.Services.KioskProduct;
using System.Reflection;

namespace RedboxPatches.RedboxRentalServices
{
    [HarmonyPatch(typeof(TitleProduct), "get_IsSellable")]
    public class IsSellablePatch
    {
        static bool Prefix(TitleProduct __instance, ref bool __result)
        {
            var priceEngineIsSellableField = typeof(TitleProduct).GetProperty("PriceEngineIsSellable", BindingFlags.NonPublic | BindingFlags.Instance);
            bool priceEngineIsSellable = (bool)priceEngineIsSellableField.GetValue(__instance);

            __result = __instance.IsEmptyCase || priceEngineIsSellable;
            return false;
        }
    }
}
