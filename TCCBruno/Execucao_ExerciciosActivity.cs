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
using TCCBruno.Adapters;

namespace TCCBruno
{
    [Activity(Label = "Exercícios")]
    public class Execucao_ExerciciosActivity : ActivityBase
    {
        //private GridLayout _gridLayout;
        //private int _instrutorId;
        private ListView _execucaoExerciciosListView;
        //private ListView _treinosListView;
        Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();
        private int _treinoTipoId;

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
            SetContentView(Resource.Layout.Execucao_ExerciciosPage);

            _execucaoExerciciosListView = FindViewById<ListView>(Resource.Id.LV_ExerciciosExecucao);
            FindViewById<Button>(Resource.Id.BTN_NovoExercicio).Click += BTN_NovoExercicio_Click;
            //_treinosListView = FindViewById<ListView>(Resource.Id.LV_Treinos);

            //_exerciciosList.ItemClick += LV_Treinos_ItemClick;
            //FindViewById<Button>(Resource.Id.BTN_NovoTreino).Click += BTN_NovoTreino_Click; ;

            //Recebe o Id do usuário (instrutor) logado no sistema por passagem de parâmetro da tela anterior
            //_instrutorAlunoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);
            _treinoTipoId = Nav.GetAndRemoveParameter<int>(Intent);
            LoadExecucaoExercicios();
        }

        private void BTN_NovoExercicio_Click(object sender, EventArgs e)
        {
            Nav.NavigateTo(MainActivity._cadastroExecucao_ExercicioPageKey, _treinoTipoId);
        }

        private void LoadExecucaoExercicios()
        {
            Execucao_ExercicioDAO execucaoExercicioDAO = new Execucao_ExercicioDAO();
            var execucaoExerciciosList = execucaoExercicioDAO.LoadExecucaoExercicios(_treinoTipoId);
            if (execucaoExerciciosList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Exercícios de Execução", this);
                return;
            }

            //Preenche ListView
            var listAdapter = new Execucao_ExerciciosListAdapter(this, execucaoExerciciosList);
            _execucaoExerciciosListView.Adapter = listAdapter;

            //var listAdapter = new TreinosAdapter(this, treinosList);
            //_treinosListView.Adapter = listAdapter;
        }


        /// <summary>
        /// Ao voltar a esta tela, carrega os treinos novamente do BD
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            LoadExecucaoExercicios();
        }
    }
}