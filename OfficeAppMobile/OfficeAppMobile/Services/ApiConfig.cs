using Xamarin.Forms;

namespace OfficeAppMobile.Services
{
    public static class BaseUrl
    {
        const string Android = "http://10.0.2.2:5000";
        const string iOSandUWP = "http://localhost:5000";

        const string Departments = "departments";
        const string Signup = "users";
        const string Login = "authentication";

        public static string SetDepartmentUrl()
        {
            return (Device.RuntimePlatform == Device.Android) ? $"{Android}/{Departments}" : $"{iOSandUWP}/{Departments}";
        }

        public static string SetLoginUrl()
        {
            return (Device.RuntimePlatform == Device.Android) ? $"{Android}/{Login}" : $"{iOSandUWP}/{Login}";
        }

        public static string SetSignupUrl()
        {
            return (Device.RuntimePlatform == Device.Android) ? $"{Android}/{Signup}" : $"{iOSandUWP}/{Signup}";
        }
    }
}