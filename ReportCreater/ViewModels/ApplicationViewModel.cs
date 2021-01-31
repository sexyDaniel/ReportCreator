using ReportCreater.Models;
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
        private EFClientRepository clientRepository;
        private ClientViewModel selectedClient;
        public ObservableCollection<ClientViewModel> ClientsViewModelsColliction { get; set; }
        public ApplicationViewModel(EFClientRepository repo)
        {
            clientRepository = repo;
            ClientsViewModelsColliction = new ObservableCollection<ClientViewModel>();
            UploadClients();
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
                      if (selectedClientReport!=null)
                        DocxCreater.CreateClientReportAsync(selectedClientReport.Client);
                  }, (obj) => selectedClientReport !=null));
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
                      ClientViewModel client = new ClientViewModel(new Client() { Name = "Новый клиент", ClientInfoCollection = new List<ClientInfo>() });
                      ClientsViewModelsColliction.Insert(0, client);
                      clientRepository.AddClient(client.Client);
                      SelectedClient = client;
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
                      }
                  }, (obj) => ClientsViewModelsColliction.Count > 0));
            }
        }


        private RelayCommand createReport;
        public RelayCommand CreateReport
        {
            get
            {
                return createReport ??
                  (createReport = new RelayCommand(obj =>
                  {
                     // DocxCreater.CreateGeneralMonthReport(ClientsViewModelsColliction[0].Client, "example1.docx");
                  }));
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
                  }));
            }
        }

        private void UploadClients() 
        {
            var clients = clientRepository.GetClientsInView();
            foreach (var c in clients) 
            {
                ClientsViewModelsColliction.Add(new ClientViewModel(c));
            }
        }
    }
}
