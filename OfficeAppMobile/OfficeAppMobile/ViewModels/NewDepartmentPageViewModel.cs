using Prism.Commands;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OfficeApp.Helpers;
using OfficeAppMobile.Services;
using Prism.Navigation;
using Prism.Services;

namespace OfficeAppMobile.ViewModels
{
    public class NewDepartmentPageViewModel : ViewModelBase
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly DepartmentService _departmentService = new DepartmentService();

        public string NewName { get; set; }
        public string NewDescription { get; set; }
        public string NewHead { get; set; }
        public string NewCode { get; set; }

        public NewDepartmentPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
        }

        public DelegateCommand SaveCommand => new DelegateCommand(async () =>
        {
            var toCamelCaseProperties = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Settings.Jwt}");

            /*  Another option for using Content-Type
            *  HttpContent httpContent = new StringContent(content);
            *  httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            */

            var content = JsonConvert.SerializeObject(
                new
                {
                    Name = $"{NewName}",
                    Description = $"{NewDescription}",
                    Head = $"{NewHead}",
                    Code = $"{NewCode}"
                }, toCamelCaseProperties
            );

            try
            {
                await _departmentService.SendPostAsync(content);
                await NavigationService.NavigateAsync("/NavigationPage/MainPage"); // reset the Navigation Stack
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Something happened", ex.Message, "Ok");
            }
        });

        public DelegateCommand LogoutCommand => new DelegateCommand(() =>
        {
            Settings.Jwt = "";
            NavigationService.NavigateAsync("/LoginPage");
        }
            );
    }
}