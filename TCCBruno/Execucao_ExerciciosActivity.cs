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
using Android.Content.PM;

namespace TCCBruno
{
    [Activity(Label = "Exercícios", Icon = "@drawable/logoAcademia", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Execucao_ExerciciosActivity : ActivityBase
    {
        //private GridLayout _gridLayout;
        //private int _instrutorId;
        private ListView _execucaoExerciciosListView;
        private Button _novoExercicioButton;
        private Execucao_ExerciciosListAdapter _execucaoExercicioListAdapter;
        //private ListView _treinosListView;
        Dictionary<string, int> _alunoTreinoTipoDict = new Dictionary<string, int>();
        private int _execucaoExercicio_SingleChoiceItemSelected = 0; //Primeira opção pré-selecionada
        private int _execucaoExercicioSelectedId = -1;
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
            _execucaoExerciciosListView.ItemLongClick += LV_ExecucaoExercicios_ItemLongClick;

            //Get parameter from MVVMLight Navigator
            _alunoTreinoTipoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);
            if (_alunoTreinoTipoDict.ContainsKey("aluno_id")) // Aluno logado, ajustar a GUI da Page para o mesmo
            {
                _novoExercicioButton.Visibility = ViewStates.Invisible;
            }
            LoadExecucaoExercicios();
        }

        private void LV_ExecucaoExercicios_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            if (_alunoTreinoTipoDict.ContainsKey("aluno_id")) //Operação permitida apenas para Instrutor
                return;

            Dialog dialog = null;

            _execucaoExercicioSelectedId = (int)_execucaoExercicioListAdapter.GetItemId(e.Position);
            var execucaoExercicioNome = _execucaoExercicioListAdapter.GetNomeExercicio(e.Position);
            var args = new Bundle();
            args.PutString("0", execucaoExercicioNome);
            dialog = OnCreateDialog(0, args);
            dialog.Show();
        }

        protected override Dialog OnCreateDialog(int dialogType, Bundle args)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle(args.GetString("0"));
            builder.SetSingleChoiceItems(Resource.Array.execucaoExercicioItemLongClickList, 0, (s, e) => { _execucaoExercicio_SingleChoiceItemSelected = e.Which; });
            builder.SetPositiveButton("OK", ExecucaoExercicios_SingleChoiceOKClick);
            builder.SetNegativeButton("Cancelar", (s, e) => { });

            return builder.Create();
        }

        private void ExecucaoExercicios_SingleChoiceOKClick(object sender, DialogClickEventArgs e)
        {
            switch (_execucaoExercicio_SingleChoiceItemSelected)
            {
                default:
                    break;

                case 0: //Alterar ExecucaoExercicio

                    break;

                case 1: //Remover ExecucaoExercicio
                    RemoveSelectedExecucaoExercicio();
                    LoadExecucaoExercicios();
                    break;
            }
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
            _execucaoExercicioListAdapter = new Execucao_ExerciciosListAdapter(this, execucaoExerciciosList);
            _execucaoExerciciosListView.Adapter = _execucaoExercicioListAdapter;

            //var listAdapter = new TreinosAdapter(this, treinosList);
            //_treinosListView.Adapter = listAdapter;
        }

        private void RemoveSelectedExecucaoExercicio()
        {
            if (_execucaoExercicioSelectedId < 0)
            {
                Validation.DisplayAlertMessage("Falha ao selecionar Exercício. Tente novamente", this);
                return;
            }
            Execucao_ExercicioDAO execExercicioDAO = new Execucao_ExercicioDAO();
            if (!execExercicioDAO.RemoveSelectedExercicio(_execucaoExercicioSelectedId))
                Validation.DisplayAlertMessage("Não foi possível remover o Exercício selecionado", this);

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