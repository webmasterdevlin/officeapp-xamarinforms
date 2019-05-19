using Prism.Commands;
using System;
using System.Threading.Tasks;
using OfficeAppMobile.Models;
using OfficeAppMobile.Services;
using Prism.Navigation;
using Prism.Services;

namespace OfficeAppMobile.ViewModels
{
    public class SignupPageViewModel : ViewModelBase
    {
        private readonly UserService _userService = new UserService();

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


        public SignupPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService) { }

        public DelegateCommand GoToLoginPageCommand => new DelegateCommand(async () => await GoToLogin());

        private async Task GoToLogin()
        {
            await NavigationService.GoBackAsync();
        }

        public DelegateCommand SignupCommand => new DelegateCommand(async () =>
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await PageDialogService.DisplayAlertAsync("Error Signing Up",
                   "Please complete the form", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await PageDialogService.DisplayAlertAsync("Error Signing Up",
                    "Password and confirm password are not matched", "OK");
                return;
            }

            try
            {
                await SignupNewUser();
            }
            catch (Exception ex)
            {
                await UnableToSignup(ex);
            }

        });

        private async Task UnableToSignup(Exception ex)
        {
            await PageDialogService.DisplayAlertAsync("Soemthing happened", ex.Message,
                    "Ok");
        }

        private async Task SignupNewUser()
        {
            await _userService.SignupAsync(new User
            {
                UserName = UserName,
                Email = Email,
                Password = Password
            });
            await GoToLogin();
        }
    }
}
