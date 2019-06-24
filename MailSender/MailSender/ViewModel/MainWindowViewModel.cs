using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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
            //private set => Set(ref _Recipients, value);
            
            private set
            {
                if (!Set(ref _Recipients, value)) return;
                _RecipientsView.Source = value;
                RaisePropertyChanged(nameof(FiltredRecipients));
                //Смысл в следующем: если значение свойства не изменилось(кто - то попытался установить то же значение, что уже было в этом свойстве), то мы просто ничего не делаем. 
                //    Иначе!!! - мы устанавливаем новый источник данных для _RecipientsView
            }
        }

        private Recipient _CurrentRecipient;

        public Recipient CurrentRecipient
        {
            get => _CurrentRecipient;
            set => Set(ref _CurrentRecipient, value);
        }

        #region RecipientsFilterText : string - Текст фильтра получателей

        /// <summary>Текст фильтра получателей</summary>
        private string _RecipientsFilterText;

        /// <summary>Текст фильтра получателей</summary>
        public string RecipientsFilterText
        {
            get => _RecipientsFilterText;
            //set => Set(ref _RecipientsFilterText, value);
            set
            {
                if (!Set(ref _RecipientsFilterText, value)) return;
                _RecipientsView.View.Refresh();
            }

        }

        public ICollectionView FiltredRecipients => _RecipientsView.View;

        private readonly CollectionViewSource _RecipientsView;

        #endregion

        public ICommand UpdateDataCommand { get; }

        public ICommand CreateRecipientCommand { get; }

        public ICommand SaveRecipientCommand { get; }
        public ICommand DelRecipientCommand { get; }

        public ICommand ApplicationExitCommand { get; }

        public MainWindowViewModel(IRecipientsDataService RecipientsDataService)
        {
            _RecipientsDataService = RecipientsDataService;

            UpdateDataCommand = new RelayCommand(OnUpdateDataCommandExecuted, CanUpdateDataCommandExecute);
            CreateRecipientCommand = new RelayCommand(OnCreateRecipientCommandExecuted, CanCreateRecipientCommandExecute);
            SaveRecipientCommand = new RelayCommand<Recipient>(OnSaveRecipientCommandExecuted, CanSaveRecipientCommandExecuted);
            DelRecipientCommand = new RelayCommand<Recipient>(OnDelRecipientCommandExecuted, CanDelRecipientCommandExecuted);

            ApplicationExitCommand = new RelayCommand(OnApplicationExitCommanExecuted, () => true, true);

            _RecipientsView = new CollectionViewSource();
            _RecipientsView.Filter += OnRecipientsFilter;

            //UpdateData();
        }

        private void OnRecipientsFilter(object Sender, FilterEventArgs E)
        {
            //Будем фильтровать фильтровать всех получателей почты, у которых в имени, в адресе, либо в описании будет встречаться текст, который будет указан в _RecipientsFilterText.
            //Причём, будем делать это без учёта регистра.
            //При этом, если _RecipientsFilterText хранит пустую ссылку, либо пустую строку, то фильтр должен быть выключен.
            //Поэтому в методе фильтрации первой строкой будет проверка:
            var filter = _RecipientsFilterText?.ToUpper();
            if (string.IsNullOrEmpty(filter)) return;
            var recipient = (Recipient)E.Item;
            if (!(recipient.Name?.ToUpper().Contains(filter) ?? false)
                && !(recipient.Address?.ToUpper().Contains(filter) ?? false)
                && !(recipient.Description?.ToUpper().Contains(filter) ?? false))
                E.Accepted = false;
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

        private bool CanDelRecipientCommandExecuted(Recipient recipient) => recipient != null;

        private void OnDelRecipientCommandExecuted(Recipient recipient)
        {
            _RecipientsDataService.Delete(recipient);
            UpdateData();
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
