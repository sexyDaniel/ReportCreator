using ReportCreater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReportCreater.ViewModels
{
    public class ClientInfoViewModel : BaseViewModel
    {
        public delegate void Recalc();
        public Recalc R;
        public ClientInfo ClientInfo { get; set; }
        public int MinuteCount
        {
            get { return ClientInfo.MinuteCount; }
            set
            {
                ClientInfo.MinuteCount = value;
                R();
                OnPropertyChanged("MinuteCount");
            }
        }

        public int StaticWorkPrice
        {
            get { return ClientInfo.StaticWorkPrice; }
            set
            {
                ClientInfo.StaticWorkPrice = value;
                R();
                OnPropertyChanged("StaticWorkPrice");
            }
        }
        public bool StaticWork
        {
            get { return ClientInfo.StaticWork; }
            set
            {
                ClientInfo.StaticWork = value;
                HourCount = 0;
                MinuteCount = 0;
                R();
                OnPropertyChanged("StaticWork");
            }
        }
        public string Date
        {
            get 
            {
                return ClientInfo.Date; 
            }
            set
            {
                ClientInfo.Date = value;
                OnPropertyChanged("Date");
            }
        }
        public int HourCount
        {
            get { return ClientInfo.HourCount; }
            set
            {
                ClientInfo.HourCount = value;
                R();
                OnPropertyChanged("HourCount");
            }
        }
        public string Question
        {
            get { return ClientInfo.Question; }
            set
            {
                ClientInfo.Question = value;
                OnPropertyChanged("Question");
            }
        }
    }
}
