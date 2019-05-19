using System;
using OfficeApp.Helpers;
using Prism.Navigation;

namespace OfficeAppMobile.Utils
{
    public static class Logout
    {
        public static Action Out()
        {
            return async () =>
             {
                 Settings.Jwt = "";
                 await INavigationService.NavigateAsync("/LoginPage");
             };
        }
    }
}
