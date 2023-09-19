using Prism.Commands;
using Prism.Mvvm;
using System;

namespace ModuleA.ViewModels
{
    public class TabViewModel : BindableBase
    {

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _canUpdate = true;
        public bool CanUpdate
        {
            get { return _canUpdate; }
            set { SetProperty(ref _canUpdate, value); }
        }

        private string _updatedText;
        public string UpdateText
        {
            get { return _updatedText; }
            set { SetProperty(ref _updatedText, value); }
        }

        public DelegateCommand UpdateCommand { get; private set; }


        private void Update()
        {
            UpdateText = $"Updated: {DateTime.Now}";
        }       
    }
}
