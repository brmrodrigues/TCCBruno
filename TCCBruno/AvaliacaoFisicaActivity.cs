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
using TCCBruno.Model;
using Android.Content.PM;

namespace TCCBruno
{
    [Activity(Label = "Avaliações Físicas", Icon = "@drawable/logoAcademia", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AvaliacaoFisicaActivity : Activity
    {
        //private GridLayout _gridLayout;
        //private int _instrutorId;
        private ListView _avaliacoesFisicaListView;
        private Button _novaAvalicaoFisicaButton;
        private AvaliacaoFisicaListAdapter _avFisicaListAdapter;
        //private ListView _treinosListView;
        Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();
        private int _avFisica_SingleChoiceItemSelected = 0; //Primeira opção pré-selecionada
        private int _avaliacaoFisicaSelectedId = -1;
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
            SetContentView(Resource.Layout.AvaliacaoFisicaPage);

            //Get View Controls
            _avaliacoesFisicaListView = FindViewById<ListView>(Resource.Id.LV_AvaliacoesFisica);
            _novaAvalicaoFisicaButton = FindViewById<Button>(Resource.Id.BTN_NovaAvaliacaoFisica);

            //Declare View Event Handlers
            _novaAvalicaoFisicaButton.Click += BTN_NovaAvFisica_Click;
            _avaliacoesFisicaListView.ItemLongClick += LV_AvaliacaoFisica_ItemLongClick;
            _avaliacoesFisicaListView.ItemClick += LV_AvaliacaoFisica_ItemClick;


            //Get parameter from MVVMLight Navigator
            _instrutorAlunoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);
            if (!_instrutorAlunoDict.ContainsKey("instrutor_id")) // Acesso permitido apenas para Instrutor
            {
                _novaAvalicaoFisicaButton.Visibility = ViewStates.Invisible;
            }
            LoadAvaliacoesFisica();
        }

        private void LV_AvaliacaoFisica_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (_instrutorAlunoDict.ContainsKey("instrutor_id")) // Acesso apenas para Aluno. O Instrutor já tem acesso em ItemLongClick
                return;
            //_instrutorAlunoDict.Add("avaliacao_fisica_id", (int)_avFisicaListAdapter.GetItemId(e.Position));
            //Nav.NavigateTo(LoginActivity._cadastroAvFisicaPageKey, _instrutorAlunoDict);
        }

        private void LV_AvaliacaoFisica_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            if (!_instrutorAlunoDict.ContainsKey("instrutor_id")) //Operação permitida apenas para Instrutor
                return;

            _avaliacaoFisicaSelectedId = (int)_avFisicaListAdapter.GetItemId(e.Position);

            Dialog dialog = null;
            //_instrutorAlunoDict.Add("avaliacao_fisica_id", (int)_avFisicaListAdapter.GetItemId(e.Position));
            var args = new Bundle();
            args.PutString("0", "Avaliação " + (e.Position + 1).ToString());
            dialog = OnCreateDialog(0, args);
            dialog.Show();
        }

        protected override Dialog OnCreateDialog(int dialogType, Bundle args)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle(args.GetString("0"));
            builder.SetSingleChoiceItems(Resource.Array.avaliacoesFisicaItemLongClickList, 0, (s, e) => { _avFisica_SingleChoiceItemSelected = e.Which; });
            builder.SetPositiveButton("OK", ExecucaoExercicios_SingleChoiceOKClick);
            builder.SetNegativeButton("Cancelar", (s, e) => { });

            return builder.Create();
        }

        private void ExecucaoExercicios_SingleChoiceOKClick(object sender, DialogClickEventArgs e)
        {
            switch (_avFisica_SingleChoiceItemSelected)
            {
                default:
                    break;

                case 0: //Exibir Detalhes
                    //Nav.NavigateTo(LoginActivity._cadastroAvFisicaPageKey, _instrutorAlunoDict);
                    break;

                case 1: //Remover AvaliacaoFisica
                    RemoveSelectedAvaliacaoFisica();
                    LoadAvaliacoesFisica();
                    break;
            }
        }

        private void BTN_NovaAvFisica_Click(object sender, EventArgs e)
        {
            AlunoDAO alunoDAO = new AlunoDAO();
            var aluno = alunoDAO.GetDataNascimentoAluno(_instrutorAlunoDict["aluno_id"]);
            Nav.NavigateTo(LoginActivity._cadastroAvFisicaPageKey, aluno);
        }

        private void LoadAvaliacoesFisica()
        {
            Avaliacao_FisicaDAO avFisicaDAO = new Avaliacao_FisicaDAO();
            var avFisicaList = avFisicaDAO.LoadAvaliacoesResumo(_instrutorAlunoDict["aluno_id"]);
            if (avFisicaList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Avaliações Físicas", this);
                return;
            }

            //Preenche ListView
            _avFisicaListAdapter = new AvaliacaoFisicaListAdapter(this, avFisicaList);
            _avaliacoesFisicaListView.Adapter = _avFisicaListAdapter;

            //var listAdapter = new TreinosAdapter(this, treinosList);
            //_treinosListView.Adapter = listAdapter;
        }

        private void RemoveSelectedAvaliacaoFisica()
        {
            if (_avaliacaoFisicaSelectedId < 0)
            {
                Validation.DisplayAlertMessage("Falha ao selecionar Avaliação. Tente novamente", this);
                return;
            }
            Avaliacao_FisicaDAO execExercicioDAO = new Avaliacao_FisicaDAO();
            if (!execExercicioDAO.RemoveAvaliacaoFisica(_avaliacaoFisicaSelectedId))
                Validation.DisplayAlertMessage("Não foi possível remover a Avaliação selecionada", this);

        }

        /// <summary>
        /// Ao voltar a esta tela, carrega os treinos novamente do BD
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            LoadAvaliacoesFisica();
        }
    }
}