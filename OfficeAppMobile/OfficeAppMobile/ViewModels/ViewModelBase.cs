using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Services;

namespace OfficeAppMobile.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IDestructible
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

        /// <summary>
        /// Lifecycle event that runs before you leave the page
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        /// <summary>
        /// Lifecycle event that runs after the UI appears
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        /// <summary>
        /// Lifecycle event that runs before the UI appears
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }
    }
}