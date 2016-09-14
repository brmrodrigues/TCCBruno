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
using TCCBruno.DAO;
using TCCBruno.Model;

namespace TCCBruno
{
    [Activity(Label = "Cadastro de Treino")]
    public class CadastroTreinoActivity : Activity
    {
        Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();
        private DateTime _dateFrom, _dateTo;

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
            SetContentView(Resource.Layout.CadastroTreinoPage);

            FindViewById<CalendarView>(Resource.Id.CV_From).DateChange += CV_From_OnDateChange;
            FindViewById<CalendarView>(Resource.Id.CV_To).DateChange += CV_To_OnDateChange;
            FindViewById<Button>(Resource.Id.BTN_CadastrarTreino).Click += BTN_CadastrarTreino_Click;

            // Create your application here
            _instrutorAlunoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);

        }

        private void BTN_CadastrarTreino_Click(object sender, EventArgs e)
        {
            if (_dateFrom.Year < 2000 || _dateTo.Year <= 2000) //Verificação se o usuário selecionou alguma data
            {
                Validation.DisplayAlertMessage("Selecione o período do treino antes de fazer o cadastro!", this);
                return;
            }
            //Datas válidas, cadastrar Treino:
            TreinoDAO treinoDAO = new TreinoDAO();
            Treino newTreino = new Treino
            {
                data_inicio = _dateFrom,
                data_fim = _dateTo,
                aluno_id = _instrutorAlunoDict["aluno_id"]
            };
            if (treinoDAO.InsertTreino(newTreino))
            {
                //Validation.DisplayAlertMessage("Treino cadastrado com sucesso!", this);
                //Nav.GoBack();
            }
            else
            {
                Validation.DisplayAlertMessage("Não foi possível cadastrar o Treino", this);
            }
            base.OnBackPressed();
        }

        private void CV_From_OnDateChange(object sender, CalendarView.DateChangeEventArgs e)
        {
            _dateFrom = new DateTime(e.Year, e.Month + 1, e.DayOfMonth);
        }

        private void CV_To_OnDateChange(object sender, CalendarView.DateChangeEventArgs e)
        {
            _dateTo = new DateTime(e.Year, e.Month + 1, e.DayOfMonth);
        }


    }
}