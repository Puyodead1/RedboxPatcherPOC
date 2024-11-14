//using HarmonyLib;
//using Redbox.Rental.Services.KioskProduct;
//using System;

//namespace RedboxPatches.RedboxRentalServices
//{
//    [HarmonyPatch(typeof(TitleProduct), "get_MerchandiseDate")]
//    public class MerchandiseDateDatePatch
//    {
//        static bool Prefix(TitleProduct __instance, ref DateTime __result)
//        {
//            __result = DateTime.Now.AddDays(-30);
//            return false;
//        }
//    }
//}
