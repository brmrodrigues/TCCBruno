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
using TCCBruno.Adapters;
using TCCBruno.DAO;
using Android.Content.PM;

namespace TCCBruno
{
    [Activity(Label = "Estatísticas de Falta", Icon = "@drawable/logoAcademia", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EstatisticasFaltaActivity : Activity
    {
        private int _alunoId = -1;
        private EstatisticaFaltaListAdapter _estatFaltaAdapter;
        private ListView _nroPresencaPorSubTreinoListView;

        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EstatisticasFaltaPage);
            _nroPresencaPorSubTreinoListView = FindViewById<ListView>(Resource.Id.LV_NroPresencaPorSubTreino);

            // Create your application here
            _alunoId = Nav.GetAndRemoveParameter<int>(Intent);

            LoadPresencaPorSubTreino();
        }

        private void LoadPresencaPorSubTreino()
        {
            CheckInDAO checkInDAO = new CheckInDAO();
            var presencaList = checkInDAO.LoadPresencaPorSubTreino(_alunoId);
            if (presencaList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Presença", this);
                return;
            }
            _estatFaltaAdapter = new EstatisticaFaltaListAdapter(this, presencaList);
            _nroPresencaPorSubTreinoListView.Adapter = _estatFaltaAdapter;
        }
    }
}