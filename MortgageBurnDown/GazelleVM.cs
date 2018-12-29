using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MortgageBurnDown
{
    public class GazelleVM : INotifyPropertyChanged, ICommand
    {
        private KeyValuePair<DateTime, decimal>? _selectedItem;

        public ObservableCollection<KeyValuePair<DateTime, decimal>> ExtraPayments
        {
            get
            {
                return GazelleSeries.Instance.ExtraPayments;
            }
        }

        public KeyValuePair<DateTime, decimal> SelectedItem
        {
            get
            {
                return _selectedItem.HasValue ? _selectedItem.Value : default(KeyValuePair<DateTime, decimal>);
            }
            set
            {
                _selectedItem = value;
            }
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var tuple = parameter as Tuple<string, DateTime, decimal>;

            switch (tuple.Item1)
            {
                case "Add":
                    ExtraPayments.Add(new KeyValuePair<DateTime, decimal>(tuple.Item2, tuple.Item3));
                    break;
                case "Remove":
                    if (_selectedItem.HasValue)
                    {
                        ExtraPayments.Remove(_selectedItem.Value);
                    }
                    break;
            }
        }
    }
}