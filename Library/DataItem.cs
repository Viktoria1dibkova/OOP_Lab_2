using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class DataItem : ViewModelBase
    {
        private string name;
        private string valuee;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Value
        {
            get => valuee;
            set => SetProperty(ref valuee, value);
        }
    }
}
