using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreater.Models
{
    public class Client
    {
        public int Id { get; set; }
        public double OneHourePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Name { get; set; }

        public List<ClientInfo> ClientInfoCollection { get; set; }

        public decimal CalcTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (var c in ClientInfoCollection)
            {
                if (c.StaticWork)
                    totalPrice += c.StaticWorkPrice;
                else
                    totalPrice += (decimal)Math.Round((c.MinuteCount + c.HourCount * 60) * OneHourePrice / 60, 3);
            }
            return totalPrice;
        }
    }
}
