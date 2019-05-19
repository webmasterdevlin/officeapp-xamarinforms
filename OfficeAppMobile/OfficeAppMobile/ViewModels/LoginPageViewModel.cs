using Prism.Commands;
using System;
using OfficeAppMobile.Models;
using OfficeAppMobile.Services;
using Prism.Navigation;
using Prism.Services;

namespace OfficeAppMobile.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly UserService _userService = new UserService();

        public string Email { get; set; }
        public string Password { get; set; }

        private bool _isLogging;

        public bool IsLogging
        {
            get => _isLogging;
            set
            {
                _isLogging = value;
                RaisePropertyChanged();
            }
        }

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
        }

        public DelegateCommand LoginCommand => new DelegateCommand(async () =>
        {
            IsLogging = !IsLogging;
            var login = new User { Email = Email, Password = Password };

            try
            {
                if (await _userService.LoginAsync(login))
                {
                    await NavigationService.NavigateAsync("/NavigationPage/MainPage"); // Resets the Navigation Stack to prevent user from going back to LoginPage
                    return;
                }

                await PageDialogService.DisplayAlertAsync("Authentication failed", "Please try again",
                  "OK");
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Something happened", ex.Message,
                    "OK");
            }
            IsLogging = !IsLogging;
        });

        public DelegateCommand ToSignupPageCommand => new DelegateCommand(async () =>
            await NavigationService.NavigateAsync("SignupPage")
        );
    }
}