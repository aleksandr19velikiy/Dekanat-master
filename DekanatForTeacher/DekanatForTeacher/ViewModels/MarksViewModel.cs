using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DekanatForTeacher.ViewModels
{
    public class MarksViewModel : INotifyPropertyChanged
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullName
        {
            get { return string.Format("{0} {1} {2}", LastName, FirstName, MiddleName); }
        }

        public readonly int?[] MarksArray = new int?[5];

        public int? MarkType1
        {
            get { return MarksArray[0]; }
            set { MarksArray[0] = value; }
        }

        public int? MarkType2
        {
            get { return MarksArray[1]; }
            set { MarksArray[1] = value; }
        }

        public int? MarkType3
        {
            get { return MarksArray[2]; }
            set { MarksArray[2] = value; }
        }

        public int? MarkType4
        {
            get { return MarksArray[3]; }
            set { MarksArray[3] = value; }
        }

        public int? MarkType5
        {
            get { return MarksArray[4]; }
            set
            {
                MarksArray[4] = value;
                OnPropertyChanged("MarkType5");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
