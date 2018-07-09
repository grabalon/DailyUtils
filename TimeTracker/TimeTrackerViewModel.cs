using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TimeTracker
{
    internal class TimeTrackerViewModel : INotifyPropertyChanged, ICommand
    {
        private bool _standUpState;
        private bool _waterState;

        public bool StandUpState
        {
            get
            {
                return _standUpState;
            }
            set
            {
                if (_standUpState != value)
                {
                    _standUpState = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool WaterState
        {
            get
            {
                return _waterState;
            }
            set
            {
                if (_waterState != value)
                {
                    _waterState = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object _)
        {
            return StandUpState && WaterState;
        }

        public void Execute(object parameter)
        {
            string productivity = parameter.ToString();

            using (var writer = new StreamWriter("productivity.csv", append: true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy,hh:mm:ss tt")},{productivity}");
            }

            CloseApp();
        }

        private void CloseApp()
        {
            Application.Current.MainWindow.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CanExecuteChanged;
    }
}
