using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Android.Locations;
using Android.Util;
using System.Threading.Tasks;
using TCCBruno.Extension;
using TCCBruno.Adapters;
using TCCBruno.DAO;
using TCCBruno.Model;

namespace TCCBruno
{
    [Activity(Label = "Realizar Check-in")]
    public class CheckInActivity : Activity, ILocationListener
    {
        private int _alunoId;
        Location _currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;
        private Button _checkInButton;
        private Spinner _treinoTipoSpinner;
        private const double ACADEMIA_LATITUDE = -21.99476054;
        private const double ACADEMIA_LONGITUDE = -47.91893633;
        private const double ACADEMIA_RAIO_MAX = 0.3; //Kilometros
        //private const double _latitudeAcademia = -5.814137; //Coordenadas Reais da Personal Academia
        //private const double _longitudeAcademia = -35.217261;

        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
        }
        #region Implementação de ILocationListener #######################################################
        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            var distance = HaversineDistance();

            //Permitir realizar o CheckIn se a localização do aluno estiver no raio de cobertura definido da Academia
            _checkInButton.Visibility = distance < ACADEMIA_RAIO_MAX ? ViewStates.Visible : ViewStates.Invisible;
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
        #endregion ########################################################################################

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CheckInPage);

            //Get View Controls
            _checkInButton = FindViewById<Button>(Resource.Id.BTN_CheckIn);
            _treinoTipoSpinner = FindViewById<Spinner>(Resource.Id.SPN_TreinoTipo);

            //Controls Event Handlers
            _checkInButton.Click += BTN_CheckIn_Click;

            _alunoId = Nav.GetAndRemoveParameter<int>(Intent);

            LoadTreinoTiposAtual();

            InitializeLocationManager();
        }

        private void LoadTreinoTiposAtual()
        {
            Treino_TipoDAO treinoTipoDAO = new Treino_TipoDAO();

            var treinosTipoList = treinoTipoDAO.LoadTreino_TiposAtual(_alunoId);
            _treinoTipoSpinner.Adapter = new TreinoTipoListAdapter(this, treinosTipoList);

        }

        /// <summary>
        /// Cálculo da distância entre dois pontos (coordendas em latitude, longitude)
        /// </summary>
        /// <returns></returns>
        private double HaversineDistance()
        {
            double R = 6371; //Kilometers
            var lat = (ACADEMIA_LATITUDE - _currentLocation.Latitude).ToRadians();
            var lng = (ACADEMIA_LONGITUDE - _currentLocation.Longitude).ToRadians();

            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                        Math.Cos(_currentLocation.Latitude.ToRadians()) * Math.Cos(ACADEMIA_LATITUDE.ToRadians()) *
                        Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));

            return R * h2;
        }

        private void BTN_CheckIn_Click(object sender, EventArgs e)
        {
            if (_currentLocation == null)
            {
                Validation.DisplayAlertMessage("Não foi possível determinar a sua localização. Ative o GPS ou tente novamente em alguns minutos", this);
            }
            else //Verificar se o Aluno está em uma região próxima das coordenadas da academia
            {
                if (DoCheckIn())
                    Validation.DisplayAlertMessage("Check-in realizado com sucesso!", this);
                else
                    Validation.DisplayAlertMessage("Falha ao realizar o checkin", this);
            }

            //Address address = await ReverseGeocodeCurrentLocation();
            //DisplayAddress(address);
        }

        private bool DoCheckIn()
        {
            CheckInDAO checkinDAO = new CheckInDAO();

            CheckIn newcheckin = new CheckIn
            {
                aluno_id = _alunoId,
                treino_tipo_id = Convert.ToInt32(_treinoTipoSpinner.Adapter.GetItemId(_treinoTipoSpinner.SelectedItemPosition)),
                data_checkin = DateTime.Now

            };
            return checkinDAO.InsertCheckIn(newcheckin);
        }


        //private void DisplayAddress(Address address)
        //{
        //    if (address != null)
        //    {
        //        StringBuilder deviceAddress = new StringBuilder();
        //        for (int i = 0; i < address.MaxAddressLineIndex; i++)
        //        {
        //            deviceAddress.AppendLine(address.GetAddressLine(i));
        //        }
        //        // Remove the last comma from the end of the address.
        //        Validation.DisplayAlertMessage(deviceAddress.ToString(), this);
        //    }
        //    else
        //    {
        //        Validation.DisplayAlertMessage("Unable to determine the address. Try again in a few minutes.", this);
        //    }
        //}

        //private async Task<Address> ReverseGeocodeCurrentLocation()
        //{
        //    Geocoder geocoder = new Geocoder(this);
        //    IList<Address> addressList =
        //        await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

        //    Address address = addressList.FirstOrDefault();
        //    return address;
        //}

        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);

            Criteria criteriaLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };

            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaLocationService, true);

            _locationProvider = acceptableLocationProviders.Any() ? acceptableLocationProviders.First() : string.Empty;

            //Console.WriteLine();
            Log.Debug("CheckInActivity", "Usando locationProvider: " + _locationProvider);
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (_locationProvider != string.Empty)
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        /// <summary>
        /// Economizar a bateria desligando as atualizações de localização quando a Activity for para background
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

    }
}