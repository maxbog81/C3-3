using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MailSender.lib.Data.Linq2SQL;
using MailSender.lib.Services;

namespace MailSender.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IRecipientsDataService _RecipientsDataService;

        private string _Title = "Рассыльщик почты";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private string _Status = "Готов!";

        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        private ObservableCollection<Recipient> _Recipients;

        public ObservableCollection<Recipient> Recipients
        {
            get => _Recipients;
            private set => Set(ref _Recipients, value);
        }

        private Recipient _CurrentRecipient;

        public Recipient CurrentRecipient
        {
            get => _CurrentRecipient;
            set => Set(ref _CurrentRecipient, value);
        }

        public ICommand UpdateDataCommand { get; }

        public ICommand CreateRecipientCommand { get; }

        public ICommand SaveRecipientCommand { get; }

        public ICommand ApplicationExitCommand { get; }

        public MainWindowViewModel(IRecipientsDataService RecipientsDataService)
        {
            _RecipientsDataService = RecipientsDataService;

            UpdateDataCommand = new RelayCommand(OnUpdateDataCommandExecuted, CanUpdateDataCommandExecute);
            CreateRecipientCommand = new RelayCommand(OnCreateRecipientCommandExecuted, CanCreateRecipientCommandExecute);
            SaveRecipientCommand = new RelayCommand<Recipient>(OnSaveRecipientCommandExecuted, CanSaveRecipientCommandExecuted);

            ApplicationExitCommand = new RelayCommand(OnApplicationExitCommanExecuted, () => true, true);

            //UpdateData();
        }

        private static void OnApplicationExitCommanExecuted()
        {
            Application.Current.Shutdown();
        }

        private bool CanSaveRecipientCommandExecuted(Recipient recipient) => recipient != null;

        private void OnSaveRecipientCommandExecuted(Recipient recipient)
        {
            _RecipientsDataService.Update(recipient);
        }

        private bool CanCreateRecipientCommandExecute() => true;

        private void OnCreateRecipientCommandExecuted()
        {
            var new_recipient = new Recipient
            {
                Name = "New recpient",
                Address = "recipient@server.com"
            };
            _RecipientsDataService.Create(new_recipient);
            _Recipients.Add(new_recipient);
            CurrentRecipient = new_recipient;
        }

        private bool CanUpdateDataCommandExecute() => true;

        private void OnUpdateDataCommandExecuted()
        {
            UpdateData();
        }

        public void UpdateData()
        {
            Recipients = new ObservableCollection<Recipient>(_RecipientsDataService.GetAll());
        }
    }

}
