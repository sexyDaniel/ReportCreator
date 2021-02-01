using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreater.Models
{
    class EFClientRepository
    {
        public void AddClient(Client client) 
        {
            using (ApplicationDbContext context = new ApplicationDbContext()) 
            {
                context.Clients.Add(client);
                context.SaveChanges();
            }
        }

        public List<Client> GetClientsInView()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var clients = context.Clients.ToList();
                foreach (var client in clients) 
                {
                    client.ClientInfoCollection = context.ClientInfos.Where(ci=>ci.ClientId==client.Id).ToList();
                }
                return clients;
            }
        }

        public Client GetClient(int clientId) 
        {
            using (ApplicationDbContext context = new ApplicationDbContext()) 
            {
                var client = context.Clients.FirstOrDefault(c=>c.Id==clientId);
                if(client!=null)
                    client.ClientInfoCollection = context.ClientInfos.Where(c => c.ClientId == clientId).ToList();
                return client;
            }
        }

        public void UpdateClient(Client client) 
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var searchClient = context.Clients.FirstOrDefault(c=>client.Id==c.Id);
                searchClient.ClientInfoCollection = context.ClientInfos.Where(c => c.ClientId == client.Id).ToList();
                if (searchClient != null) 
                {
                    searchClient.Name = client.Name;
                    searchClient.OneHourePrice = client.OneHourePrice;
                    searchClient.TotalPrice = client.TotalPrice;
                    UpdateClientInfo(client,searchClient);
                }
                context.SaveChanges();
            }
        }

        public void DeleteClient(int clientId) 
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var searchClient = context.Clients.FirstOrDefault(c => clientId == c.Id);
                searchClient.ClientInfoCollection = context.ClientInfos.Where(c => c.ClientId == clientId).ToList();
                if (searchClient != null)
                {
                    context.Clients.Remove(searchClient);
                    DeleteClientInfo(context, searchClient);
                }
                context.SaveChanges();
            }
        }

        private void DeleteClientInfo(ApplicationDbContext context, Client searchClient) 
        {
            foreach (var clientInfo in searchClient.ClientInfoCollection)
                context.ClientInfos.Remove(clientInfo);
        }

        private void UpdateClientInfo(Client client, Client searchClient) 
        {
            foreach (var clientInfo in searchClient.ClientInfoCollection)
            {
                var searchClientInfo = client.ClientInfoCollection.FirstOrDefault(cI => cI.Id == clientInfo.Id);
                if (searchClientInfo != null)
                {
                    clientInfo.Date = searchClientInfo.Date;
                    clientInfo.HourCount = searchClientInfo.HourCount;
                    clientInfo.MinuteCount = searchClientInfo.MinuteCount;
                    clientInfo.Question = searchClientInfo.Question;
                    clientInfo.StaticWork = searchClientInfo.StaticWork;
                    clientInfo.StaticWorkPrice = searchClientInfo.StaticWorkPrice;
                }
            }
        }
    }
}
