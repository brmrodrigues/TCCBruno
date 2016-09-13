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
using TCCBruno.Model;
using TCCBruno.DAO;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using TCCBruno.Adapters;

namespace TCCBruno
{
    [Activity(Label = "Treinos")]
    public class TreinosActivity : ActivityBase
    {
        //private GridLayout _gridLayout;
        //private int _instrutorId;
        private ListView _treinosListView;
        Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();

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
            SetContentView(Resource.Layout.TreinosPage);

            //_gridLayout = FindViewById<GridLayout>(Resource.Id.GRL_CadastroAluno);
            _treinosListView = FindViewById<ListView>(Resource.Id.LV_Treinos);
            FindViewById<Button>(Resource.Id.BTN_NovoTreino).Click += BTN_NovoTreino_Click; ;

            //Recebe o Id do usuário (instrutor) logado no sistema por passagem de parâmetro da tela anterior
            _instrutorAlunoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);
            LoadTreinos();
        }

        private void LoadTreinos()
        {
            TreinoDAO treinoDAO = new TreinoDAO();
            var treinosList = treinoDAO.LoadTreinos(_instrutorAlunoDict["aluno_id"]);
            if (treinosList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Treinos", this);
                return;
            }
            //Preence ListView com os Treinos do Aluno
            var listAdapter = new TreinosAdapter(this, treinosList);
            _treinosListView.Adapter = listAdapter;
        }

        private void BTN_NovoTreino_Click(object sender, EventArgs e)
        {
            Nav.NavigateTo("CadastroTreinoPage", _instrutorAlunoDict);
        }
    }


}