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
    public class ClientViewModel : BaseViewModel
    {
        public Client Client { get; set; }
        private EFClientInfoRepository repository;
        private double oneMinPrice;
        private ClientInfoViewModel selectedClientInfo;
        private int questionsCount;
        public ObservableCollection<ClientInfoViewModel> ClientInfoCollection { get; set; }
        public ClientViewModel(Client c)
        {
            repository = new EFClientInfoRepository();
            Client = c;
            UploadClientInfo();
            questionsCount = ClientInfoCollection.Count;
        }

        public ClientInfoViewModel SelectedClientInfo
        {
            get { return selectedClientInfo; }
            set
            {
                if (value != null)
                {
                    selectedClientInfo = value;
                    selectedClientInfo.R = RecalcTotalPrice;
                }
                OnPropertyChanged("SelectedClientInfo");
            }
        }

        public int QuestionsCount
        {
            get { return questionsCount; }
            set
            {
                questionsCount = value;
                OnPropertyChanged("QuestionsCount");
            }
        }

        public double OneHourePrice
        {
            get { return Client.OneHourePrice; }
            set
            {
                Client.OneHourePrice = value;
                OneMinutePrice = Math.Round(Client.OneHourePrice / 60, 3);
                // client.TotalPrice = client.ClientInfoCollection.Sum(i => i.HourCount * client.OneHourePrice);
                OnPropertyChanged("OneHourePrice");
            }
        }

        public double OneMinutePrice
        {
            get { return oneMinPrice; }
            set
            {
                oneMinPrice = value;
                OnPropertyChanged("OneMinutePrice");
            }
        }

        public decimal TotalPrice
        {
            get { return Client.TotalPrice; }
            set
            {
                // Client.TotalPrice = Client.CalcTotalPrice();
                Client.TotalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }
        public string Name
        {
            get { return Client.Name; }
            set
            {
                Client.Name = value;
                OnPropertyChanged("Name");
            }
        }

        private RelayCommand add;
        public RelayCommand Add
        {
            get
            {
                return add ??
                  (add = new RelayCommand(obj =>
                  {
                      ClientInfoViewModel clientInfo = new ClientInfoViewModel() { ClientInfo = new ClientInfo() { Question = "Новый вопрос" } };
                      if (ClientInfoCollection == null)
                          ClientInfoCollection = new ObservableCollection<ClientInfoViewModel>();
                      ClientInfoCollection.Insert(0, clientInfo);
                      repository.AddClientInfo(Client.Id, clientInfo.ClientInfo);
                      Client.ClientInfoCollection.Add(clientInfo.ClientInfo);
                      QuestionsCount++;
                  }));
            }
        }

        private RelayCommand deleteClientInfoCommand;
        public RelayCommand DeleteClientInfoCommand
        {
            get
            {
                return deleteClientInfoCommand ??
                  (deleteClientInfoCommand = new RelayCommand(obj =>
                  {
                      QuestionsCount--;
                      ClientInfoCollection.Remove(SelectedClientInfo);
                      Client.ClientInfoCollection.Remove(SelectedClientInfo.ClientInfo);
                      RecalcTotalPrice();
                      repository.DeleteClientInfo(SelectedClientInfo.ClientInfo);
                  }, (obj) => ClientInfoCollection.Count > 0));
            }
        }

        private void UploadClientInfo() 
        {
            var clientInfos = repository.GetClientInfo(Client.Id);
            ClientInfoCollection = new ObservableCollection<ClientInfoViewModel>();
            foreach (var c in clientInfos) 
            {
                ClientInfoCollection.Add(new ClientInfoViewModel { ClientInfo = c });
                Client.ClientInfoCollection.Add(c);
            }
        }
        public void RecalcTotalPrice()
        {
            TotalPrice = Client.CalcTotalPrice();
        }
    }
}
