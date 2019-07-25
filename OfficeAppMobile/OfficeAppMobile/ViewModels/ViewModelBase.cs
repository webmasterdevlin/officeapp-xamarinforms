using Prism.Mvvm;
using Prism.Navigation;
using System;
using Prism.Services;
using Prism.AppModel;

namespace OfficeAppMobile.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IAutoInitialize, IDestructible
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected INavigationService NavigationService { get; }
        protected IPageDialogService PageDialogService { get; }

        protected ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            PageDialogService = pageDialogService ?? throw new ArgumentNullException(nameof(pageDialogService));
        }


        public virtual void Destroy()
        {
        }
    }
}