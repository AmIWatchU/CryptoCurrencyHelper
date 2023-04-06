using CryptoCurrencyHelper.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CryptoCurrencyHelper.ViewModel
{

    // INotifyPropertyChanged notifies the View of property changes, so that Bindings are updated.
    public class MyViewModel : INotifyPropertyChanged
    {
        public class CryptoDataService
        {
            private const string baseUrl = "https://api.coingecko.com/api/v3/";

            public async Task<List<Crypto>> GetTopCryptos(int limit = 10)
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{baseUrl}coins/markets?vs_currency=usd&order=market_cap_desc&per_page={limit}&page=1&sparkline=false";
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Crypto> cryptos = JsonConvert.DeserializeObject<List<Crypto>>(json);
                        return cryptos;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve cryptocurrencies from Coingecko API.");
                    }
                }
            }
        }

        private readonly CryptoDataService _cryptoDataService = new CryptoDataService();

        private List<Crypto> _topCryptos;
        public List<Crypto> TopCryptos
        {
            get => _topCryptos;
            set
            {
                _topCryptos = value;
                OnPropertyChanged(nameof(TopCryptos));
            }
        }

        private Crypto _selectedCrypto;
        public Crypto SelectedCrypto
        {
            get => _selectedCrypto;
            set
            {
                _selectedCrypto = value;
                OnPropertyChanged(nameof(SelectedCrypto));
            }
        }

        public ICommand ShowDetailCommand { get; set; }
        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Func<object, bool> _canExecute;

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }


        private async void LoadData()
        {
            try
            {
                TopCryptos = await _cryptoDataService.GetTopCryptos();
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
