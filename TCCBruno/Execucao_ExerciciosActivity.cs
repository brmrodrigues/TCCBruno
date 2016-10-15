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
    [Activity(Label = "Exercícios", Icon = "@drawable/logoAcademia")]
    public class Execucao_ExerciciosActivity : ActivityBase
    {
        //private GridLayout _gridLayout;
        //private int _instrutorId;
        private ListView _execucaoExerciciosListView;
        private Button _novoExercicioButton;
        //private ListView _treinosListView;
        Dictionary<string, int> _alunoTreinoTipoDict = new Dictionary<string, int>();
        //private int _treinoTipoId;

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

            //Get View Controls
            _execucaoExerciciosListView = FindViewById<ListView>(Resource.Id.LV_ExerciciosExecucao);
            _novoExercicioButton = FindViewById<Button>(Resource.Id.BTN_NovoExercicio);

            //Declare View Event Handlers
            _novoExercicioButton.Click += BTN_NovoExercicio_Click;

            //Get parameter from MVVMLight Navigator
            _alunoTreinoTipoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);
            if (_alunoTreinoTipoDict.ContainsKey("aluno_id")) // Aluno logado, ajustar a GUI da Page para o mesmo
            {
                _novoExercicioButton.Visibility = ViewStates.Invisible;
            }
            LoadExecucaoExercicios();
        }

        private void BTN_NovoExercicio_Click(object sender, EventArgs e)
        {
            Nav.NavigateTo(LoginActivity._cadastroExecucao_ExercicioPageKey, _alunoTreinoTipoDict["treinoTipo_id"]);
        }

        private void LoadExecucaoExercicios()
        {
            Execucao_ExercicioDAO execucaoExercicioDAO = new Execucao_ExercicioDAO();
            var execucaoExerciciosList = execucaoExercicioDAO.LoadExecucaoExercicios(_alunoTreinoTipoDict["treinoTipo_id"]);
            if (execucaoExerciciosList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Exercícios de Execução", this);
                return;
            }

            ExercicioDAO exercicioDAO = new ExercicioDAO();
            foreach (var execucaoExercicio in execucaoExerciciosList)
            {
                execucaoExercicio.Exercicio = exercicioDAO.GetExercicio(execucaoExercicio.exercicio_id); ;
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