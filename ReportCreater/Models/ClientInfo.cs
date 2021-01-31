using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreater.Models
{
    public class ClientInfo
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public bool StaticWork { get; set; } = false;
        public int StaticWorkPrice { get; set; }
        public string Date { get; set; }
        public string Question { get; set; }
        public int HourCount { get; set; }
        public int MinuteCount { get; set; }
        public Client Client { get; set; }
    }
}
