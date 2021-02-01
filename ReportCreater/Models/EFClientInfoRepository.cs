using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreater.Models
{
    class EFClientInfoRepository
    {
        public void AddClientInfo(ClientInfo clientInfo)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                    context.ClientInfos.Add(clientInfo);
                    context.SaveChanges();
            }
        }

        public void DeleteClientInfo(ClientInfo clientInfo) 
        {
            using (ApplicationDbContext context = new ApplicationDbContext()) 
            {
                var clientInfoSeach = context.ClientInfos.FirstOrDefault(c => c.Id == clientInfo.Id);
                if (clientInfoSeach != null) 
                {
                    context.ClientInfos.Remove(clientInfoSeach);
                }
                context.SaveChanges();
            }
        }

        public List<ClientInfo> GetClientInfo(int clientId) 
        {
            using (ApplicationDbContext context = new ApplicationDbContext()) 
            {
                return context.ClientInfos.Where(c=>c.ClientId == clientId).ToList();
            }
        }
    }
}
