//using HarmonyLib;
//using Redbox.Core;
//using Redbox.KioskEngine.ComponentModel.KioskServices;
//using Redbox.KioskEngine.ComponentModel;
//using Redbox.KioskEngine.Environment;
//using System.Windows;
//using Redbox.Rental.Services.Authentication;
//using System.Threading.Tasks;
//using Redbox.Rental.Model.KioskClientService.Authentication;
//using Redbox.Rental.Model;

//namespace RedboxPatches.RedboxRentalServices
//{
//    /**
//     * This patch allows logging in to the FMA
//     */
//    [HarmonyPatch(typeof(AuthenticationServices))]
//    [HarmonyPatch("CallKCSAuthenticate")]
//    public static class AuthenticatePatch
//    {
//        public static void Prefix(string storeNumber, string userName, string password, bool useLdapAuthentication, RemoteServiceCallback completeCallback)
//        {
//            MessageBox.Show("AuthenticatePatch", "Prefix");

//            //Task.Run(delegate
//            //{
//            //    RemoteServiceResult remoteServiceResult = new RemoteServiceResult();
//            //    remoteServiceResult.Properties["is_online"] = true;
//            //    remoteServiceResult.Properties["response"] = AuthenticationResponse.Valid.ToString();
//            //    remoteServiceResult.Success = true;
//            //    remoteServiceResult.ExecutionTime = new System.TimeSpan(0, 0, 0);
//            //    remoteServiceResult.Properties["stand_alone"] = ServiceLocator.Instance.GetService<IStoreServices>().IsStandAlone;
//            //    LogHelper.Instance.Log("IRemoteServiceResult: {0}", new object[] { (remoteServiceResult != null) ? remoteServiceResult.ToObfuscatedString() : null });
//            //    ICallbackService service2 = ServiceLocator.Instance.GetService<ICallbackService>();
//            //    if (service2 != null)
//            //    {
//            //        service2.EnqueueCallback(new RemoteServiceCallbackEntry
//            //        {
//            //            Callback = completeCallback,
//            //            Result = remoteServiceResult,
//            //            Message = "Invoking completeCallback from AuthenticationServicesProxy.Authenticate."
//            //        });
//            //    }
//            //});

//            return;
//        }
//    }
//}
