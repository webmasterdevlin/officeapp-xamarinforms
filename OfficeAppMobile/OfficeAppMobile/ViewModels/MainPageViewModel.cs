using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using OfficeApp.Helpers;
using OfficeAppMobile.Models;
using OfficeAppMobile.Services;
using Prism.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using Prism.Ioc;

namespace OfficeAppMobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IInitialize
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly DepartmentService _departmentService = new DepartmentService();
        private ObservableCollection<Department> _observableDepartments;

        public ObservableCollection<Department> ObservableDepartments
        {
            get => _observableDepartments;
            set
            {
                _observableDepartments = value;
                RaisePropertyChanged();
            }
        }


        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
             : base(navigationService, pageDialogService)
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Settings.Jwt}");

        }

        public DelegateCommand ToNewDepPageCommand => new DelegateCommand(() =>
            NavigationService.NavigateAsync("NewDepartmentPage"));

        public async void Initialize(INavigationParameters parameters)
        {
            await IsAuthenticated();
        }

        public DelegateCommand<Department> EditDeleteCommand => new DelegateCommand<Department>(department =>
        {
            NavigationService.NavigateAsync("EditDeleteDepartmentPage", ("CurrentDepartment", department));
        });

        public DelegateCommand LogoutCommand => new DelegateCommand(() =>
          {
              Settings.Jwt = "";
              NavigationService.NavigateAsync("/LoginPage");
          }
        );

        private async Task IsAuthenticated()
        {
            if (CheckIfJwtIsEmpty() || CheckIfJwtIsExpired())
            {
                await NavigationService.NavigateAsync("LoginPage");
                return;
            }

            try
            {
                await LoadDepartments();
            }
            catch (Exception ex)
            {
                await UnableToLoadDepartments(ex);
            }
        }

        private async Task LoadDepartments()
        {
            var contents = await _departmentService.SendGetAsync();
            var departments = JsonConvert.DeserializeObject<List<Department>>(contents);
            ObservableDepartments = new ObservableCollection<Department>(departments);
        }

        private bool CheckIfJwtIsEmpty()
        {
            return Settings.Jwt == "";
        }

        private bool CheckIfJwtIsExpired()
        {
            return DateTime.Now > Settings.JwtExpirationDate;
        }

        private async Task UnableToLoadDepartments(Exception ex)
        {
            await PageDialogService.DisplayAlertAsync("Something happened", ex.Message, "Ok");
            await NavigationService.NavigateAsync("LoginPage");
        }
    }
}