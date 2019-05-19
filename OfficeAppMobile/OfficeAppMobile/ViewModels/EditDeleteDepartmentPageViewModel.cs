using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using OfficeAppMobile.Models;
using OfficeAppMobile.Services;
using Prism.Navigation;
using Prism.Services;

namespace OfficeAppMobile.ViewModels
{
    public class EditDeleteDepartmentPageViewModel : ViewModelBase
    {
        private readonly HttpClient _client = new HttpClient();
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
            if (parameters.ContainsKey("(^_^)ImTheKey"))
                CurrentDepartment = (Department)parameters["(^_^)ImTheKey"];

            base.OnNavigatingTo(parameters);
        }

        public DelegateCommand UpdateCommand => new DelegateCommand(async () =>
        {
            var content = JsonConvert.SerializeObject(CurrentDepartment);

            try
            {
                await _departmentService.SendPutAsync(CurrentDepartment, content);
                await NavigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Something happened", ex.Message, "Ok");
            }
        });

        public DelegateCommand DeleteCommand => new DelegateCommand(async () =>
        {
            var userResponse = await PageDialogService.DisplayAlertAsync("Deleting an entry",
                "You sure you want to delete this?", "Yes", "Cancel");
            if (!userResponse) return;

            try
            {
                await _departmentService.SendDeleteAsync(CurrentDepartment.Id);
                await NavigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Something happened", ex.Message, "Ok");
            }
        });

        public DelegateCommand LogoutCommand => new DelegateCommand(async () =>
            await NavigationService.NavigateAsync("/NavigationPage/LoginPage")
        );
    }
}