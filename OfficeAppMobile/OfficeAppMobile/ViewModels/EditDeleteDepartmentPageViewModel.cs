using Prism.Commands;
using System;
using Newtonsoft.Json;
using OfficeAppMobile.Models;
using OfficeAppMobile.Services;
using Prism.Navigation;
using Prism.Services;
using OfficeApp.Helpers;
using System.Threading.Tasks;

namespace OfficeAppMobile.ViewModels
{
    public class EditDeleteDepartmentPageViewModel : ViewModelBase
    {
        private readonly DepartmentService _departmentService = new DepartmentService();

        private Department _currentDepartment;

        public Department CurrentDepartment
        {
            get => _currentDepartment;
            set
            {
                _currentDepartment = value;
                RaisePropertyChanged();
            }
        }

        public EditDeleteDepartmentPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            GetDepartmentFromPreviousPage(parameters);

            base.OnNavigatingTo(parameters);
        }

        public DelegateCommand UpdateCommand => new DelegateCommand(async () =>
        {
            var content = JsonConvert.SerializeObject(CurrentDepartment);

            try
            {
                await Update(content);
            }
            catch (Exception ex)
            {
                await UnableToSendRequest(ex);
            }
        });

        public DelegateCommand DeleteCommand => new DelegateCommand(async () =>
        {
            var userResponse = await PageDialogService.DisplayAlertAsync("Deleting an entry",
                "You sure you want to delete this?", "Yes", "Cancel");
            if (!userResponse) return;

            try
            {
                await Remove();
            }
            catch (Exception ex)
            {
                await UnableToSendRequest(ex);
            }
        });

        public DelegateCommand LogoutCommand => new DelegateCommand(async () =>
            {
                Settings.Jwt = "";
                await NavigationService.NavigateAsync("/LoginPage");
            }
        );

        private void GetDepartmentFromPreviousPage(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("(^_^)ImTheKey"))
                CurrentDepartment = (Department)parameters["(^_^)ImTheKey"];
        }

        private async Task Update(string content)
        {
            await _departmentService.SendPutAsync(CurrentDepartment, content);
            await NavigationService.GoBackAsync();
        }

        private async Task Remove()
        {
            await _departmentService.SendDeleteAsync(CurrentDepartment.Id);
            await NavigationService.GoBackAsync();
        }

        private async Task UnableToSendRequest(Exception ex)
        {
            await PageDialogService.DisplayAlertAsync("Something happened", ex.Message, "Ok");
        }
    }
}