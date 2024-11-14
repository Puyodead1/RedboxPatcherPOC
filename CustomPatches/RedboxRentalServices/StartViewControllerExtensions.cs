using Redbox.Rental.Services;

namespace CustomPatches.Redbox.Rental.Services
{
    public static class StartViewControllerExtensions
    {
        public static void RedboxLogoCommand()
        {
            OrchestrationService.ExecuteScript("activate_fa");
        }
    }
}
