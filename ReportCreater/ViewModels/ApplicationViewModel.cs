using ReportCreater.Models;
using ReportCreater.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreater.ViewModels
{
    class ApplicationViewModel : BaseViewModel
    {
        public string[] Months { get; set; } = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        private int selectedMonth;
        public int SelectedMonth{get { return selectedMonth; } set{selectedMonth = value; OnPropertyChanged("SelectedMonth");}}
        private EFClientRepository clientRepository;
        private ClientViewModel selectedClient;
        private DateTime fromDate;
        private DateTime byDate;
        private bool isFilter;
        private IDialogService dialogService;
        public bool IsFilter { get { return isFilter; } set { isFilter = value; OnPropertyChanged("IsFilter"); } }
        public ObservableCollection<ClientViewModel> ClientsViewModelsColliction { get; set; }
        public ApplicationViewModel(IDialogService dialogService,EFClientRepository repo)
        {
            fromDate = DateTime.Now;
            byDate = DateTime.Now;
            clientRepository = repo;
            ClientsViewModelsColliction = new ObservableCollection<ClientViewModel>();
            UploadClients();
            this.dialogService = dialogService;
            DocxCreater.Message = dialogService.ShowMessage;
        }

        public ClientViewModel SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                if (selectedClient != null)
                    selectedClient.OneMinutePrice = Math.Round(selectedClient.OneHourePrice / 60, 3);
                OnPropertyChanged("SelectedClient");
            }
        }

        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }
        public DateTime ByDate
        {
            get { return byDate; }
            set
            {
                byDate = value;
                OnPropertyChanged("ByDate");
            }
        }

        private ClientViewModel selectedClientReport;
        public ClientViewModel SelectedClientReport
        {
            get { return selectedClientReport; }
            set
            {
                selectedClientReport = value;
                OnPropertyChanged("SelectedClientReport");
            }
        }

        private RelayCommand createClientReportCommand;
        public RelayCommand CreateClientReportCommand
        {
            get
            {
                return createClientReportCommand ??
                  (createClientReportCommand = new RelayCommand(obj =>
                  {
                      var selectedClientForReport = clientRepository.GetClient(selectedClientReport.Client.Id); 
                      if (isFilter) 
                      {
                          selectedClientForReport.ClientInfoCollection = selectedClientForReport.ClientInfoCollection
                          .Where(c =>DateTime.Parse(c.Date).Date>=FromDate.Date&& DateTime.Parse(c.Date).Date <= ByDate.Date)
                          .ToList();
                          selectedClientForReport.TotalPrice = selectedClientForReport.CalcTotalPrice();
                      }
                      selectedClientForReport.ClientInfoCollection = selectedClientForReport.ClientInfoCollection
                      .OrderBy(c=>DateTime.Parse(c.Date))
                      .ToList();

                      DocxCreater.CreateClientReportAsync(selectedClientForReport);
                  }, (obj) => selectedClientReport !=null));
            }
        }

        private RelayCommand createReport;
        public RelayCommand CreateMonthReport
        {
            get
            {
                return createReport ??
                  (createReport = new RelayCommand(obj =>
                  {
                      var clients = clientRepository.GetClientsInView();
                      foreach (var c in clients) 
                      {
                          c.ClientInfoCollection = c.ClientInfoCollection
                          .Where(cI=>DateTime.Parse(cI.Date).Date.Month==(SelectedMonth+1))
                          .OrderBy(cI=>DateTime.Parse(cI.Date))
                          .ToList();
                          c.TotalPrice = c.CalcTotalPrice();
                      }
                      DocxCreater.CreateGeneralMonthReportAsync(clients, Months[SelectedMonth]);
                  }));
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      ClientViewModel client = new ClientViewModel(new Client() { Name = "Новый клиент"});
                      ClientsViewModelsColliction.Insert(0, client);
                      clientRepository.AddClient(client.Client);
                      SelectedClient = client;
                      dialogService.ShowMessage("Добавлен новый клиент");
                  }));
            }
        }


        private RelayCommand deleteClientCommand;
        public RelayCommand DeleteClientCommand
        {
            get
            {
                return deleteClientCommand ??
                  (deleteClientCommand = new RelayCommand(obj =>
                  {
                      if (obj is ClientViewModel clientViewModel) 
                      {
                          clientRepository.DeleteClient(clientViewModel.Client.Id);
                          ClientsViewModelsColliction.Remove(clientViewModel);
                          dialogService.ShowMessage($"Клиент {clientViewModel.Name} удален");
                      }
                  }, (obj) => ClientsViewModelsColliction.Count > 0));
            }
        }

        private RelayCommand saveClientRepository;
        public RelayCommand SaveClientRepository
        {
            get
            {
                return saveClientRepository ??
                  (saveClientRepository = new RelayCommand(obj =>
                  {
                      foreach (var c in ClientsViewModelsColliction) 
                      {
                          clientRepository.UpdateClient(c.Client);
                      }
                      dialogService.ShowMessage("Данные сохранены");
                  }));
            }
        }

        private void UploadClients() 
        {
            var clients = clientRepository.GetClientsInView().ToList();
            foreach (var c in clients) 
            {
                ClientsViewModelsColliction.Add(new ClientViewModel(c));
            }
        }
    }
}
