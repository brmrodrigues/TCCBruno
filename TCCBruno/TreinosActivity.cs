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
    [Activity(Label = "Treinos", Icon = "@drawable/logoAcademia")]
    public class TreinosActivity : ActivityBase
    {
        private ExpandableListView _treinosListView;
        private Button _novoTreinoButton;
        Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();
        private int _alunoId = -1;
        private const int DIALOG_TREINO = 0;
        private const int DIALOG_TREINO_TIPO = 1;
        private const int DIALOG_CADASTRO_TREINO_TIPO = 2;
        private int _treinoTipo_SingleChoiceItemSelected = 0; //Primeira opção pré-selecionada
        private int _treino_SingleChoiceItemSelected = 0; //Primeira opção pré-selecionada
        private int _treinoSelectedId;
        private int _subTreinosCount;
        TreinosExpandableListAdapter _treinosAdapter;
        private int _treinoTipoSelectedId = -1;

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

            //Get View Controls
            _treinosListView = FindViewById<ExpandableListView>(Resource.Id.LV_Treinos);
            _novoTreinoButton = FindViewById<Button>(Resource.Id.BTN_NovoTreino);

            //Declare View Event Handlers
            _treinosListView.ItemLongClick += LV_Treinos_ItemLongClick;
            _treinosListView.ChildClick += LV_Treinos_ChildClick;
            _novoTreinoButton.Click += BTN_NovoTreino_Click;

            //Por passagem de parâmetro, recebe um Dict com chaves: "instrutorId" e "alunoId" OU apenas "alunoId"
            _instrutorAlunoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);
            if (!_instrutorAlunoDict.ContainsKey("instrutor_id")) // Aluno logado, ajustar a GUI da Page para o mesmo
            {
                _novoTreinoButton.Visibility = ViewStates.Invisible;
            }
        }

        private void LV_Treinos_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            Console.WriteLine();
        }

        private void LV_Treinos_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            ExpandableListView listView = (ExpandableListView)e.Parent;
            long pos = listView.GetExpandableListPosition(e.Position);
            var itemType = ExpandableListView.GetPackedPositionType(pos);
            var treinoPos = ExpandableListView.GetPackedPositionGroup(pos);
            var treinoTipoPos = ExpandableListView.GetPackedPositionChild(pos);

            Dialog dialog = null;
            if (itemType == PackedPositionType.Child) //Long Click em SubTreino
            {
                //Retirar o Id do SubTreino selecionado
                _treinoTipoSelectedId = (int)_treinosAdapter.GetChildId(treinoPos, treinoTipoPos);
                //Fragment: Exibir Exercicios, Alterar Subtreino, Remover Subtreino
                if (_instrutorAlunoDict.ContainsKey("instrutor_id")) //Acesso para Instrutor
                {
                    //Acesso restrito para Instrutor
                    var treinoTipoNome = _treinosAdapter.GetNomeTreinoTipo(treinoPos, treinoTipoPos);
                    var args = new Bundle();
                    args.PutString("0", treinoTipoNome);
                    dialog = OnCreateDialog(DIALOG_TREINO_TIPO, args);
                    dialog.Show();
                }
                else //Aluno logado, redirecioná-lo para os Exercícios do Subtreino escolhido
                {
                    Dictionary<string, int> alunoTreinoTipoDict = new Dictionary<string, int>();
                    alunoTreinoTipoDict.Add("aluno_id", _instrutorAlunoDict["aluno_id"]);
                    alunoTreinoTipoDict.Add("treinoTipo_id", _treinoTipoSelectedId);
                    Nav.NavigateTo(LoginActivity._execucaoExerciciosPageKey, alunoTreinoTipoDict); //Aluno logado, logo Execucao_ExercicioActivity receberá treinoTipoId inválido
                }
            }
            else if (itemType == PackedPositionType.Group && _instrutorAlunoDict.ContainsKey("instrutor_id")) //Long Click em Treino. Acesso para Instrutor
            {
                //Fragment: Add Subtreino, Alterar Treino, Remover Treino
                _treinoSelectedId = (int)_treinosAdapter.GetGroupId(treinoPos); //armazena o id do treino selecionado
                _subTreinosCount = (int)_treinosAdapter.GetChildrenCount(treinoPos);
                var args = new Bundle();
                args.PutString("0", (treinoPos + 1).ToString());
                dialog = OnCreateDialog(DIALOG_TREINO, args);
                dialog.Show();
            }
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

            //Para cada Treino, carregar o seu treino Tipo:
            Treino_TipoDAO treinoTipoDAO = new Treino_TipoDAO();
            foreach (var treino in treinosList)
            {
                treino.Treino_Tipo = treinoTipoDAO.LoadTreino_Tipos(treino.treino_id);
            }

            //Preence ListView com os Treinos do Aluno
            _treinosAdapter = new TreinosExpandableListAdapter(this, treinosList);
            _treinosListView.SetAdapter(_treinosAdapter);

            //var listAdapter = new TreinosAdapter(this, treinosList);
            //_treinosListView.Adapter = listAdapter;
        }

        private void RemoveSelectedTreino()
        {
            if (_treinoSelectedId < 0)
                Validation.DisplayAlertMessage("Falha ao selecionar Treino. Tente novamente", this);

            TreinoDAO treinoDAO = new TreinoDAO();
            if (!treinoDAO.RemoveTreino(_treinoSelectedId))
                Validation.DisplayAlertMessage("Não foi possível remover o Treino selecionado", this);
        }

        private void RemoveSelectedSubTreino()
        {
            if (_treinoTipoSelectedId < 0)
                Validation.DisplayAlertMessage("Falha ao selecionar SubTreino. Tente novamente", this);

            Treino_TipoDAO treinoTipoDAO = new Treino_TipoDAO();
            if (!treinoTipoDAO.RemoveTreinoTipo(_treinoTipoSelectedId))
                Validation.DisplayAlertMessage("Não foi possível remover o SubTreino selecionado", this);
        }

        private void BTN_NovoTreino_Click(object sender, EventArgs e)
        {
            Nav.NavigateTo(LoginActivity._cadastroTreinoPageKey, _instrutorAlunoDict);
        }

        /// <summary>
        /// Ao voltar a esta tela, carrega os treinos novamente do BD
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            LoadTreinos();
        }

        protected override Dialog OnCreateDialog(int dialogType, Bundle args)
        {
            var builder = new AlertDialog.Builder(this);
            //builder.SetIconAttribute(A)
            //args.
            switch (dialogType)
            {
                default:
                    break;
                case DIALOG_TREINO:
                    builder.SetTitle("Treino " + args.GetString("0"));
                    builder.SetSingleChoiceItems(Resource.Array.treinoItemLongClickList, 0, (s, e) => { _treino_SingleChoiceItemSelected = e.Which; });
                    builder.SetPositiveButton("OK", Treino_SingleChoiceOKClick);
                    builder.SetNegativeButton("Cancelar", (s, e) => { });
                    break;
                case DIALOG_TREINO_TIPO:
                    builder.SetTitle("Sub-Treino " + args.GetString("0"));
                    builder.SetSingleChoiceItems(Resource.Array.subTreinoItemLongClickList, 0, (s, e) => { _treinoTipo_SingleChoiceItemSelected = e.Which; });
                    builder.SetPositiveButton("OK", TreinoTipo_SingleChoiceOKClick);
                    builder.SetNegativeButton("Cancelar", (s, e) => { });
                    break;
            }

            return builder.Create();
        }

        private void Treino_SingleChoiceOKClick(object sender, DialogClickEventArgs e)
        {
            switch (_treino_SingleChoiceItemSelected)
            {
                default:
                    break;

                case 0: //Cadastrar Subtreino
                    //var dialog = OnCreateDialog(DIALOG_CADASTRO_TREINO_TIPO, null);
                    //dialog.Show();
                    Dictionary<string, int> treinoId_subTreinosCount = new Dictionary<string, int>();
                    treinoId_subTreinosCount.Add("treinoId", _treinoSelectedId);
                    treinoId_subTreinosCount.Add("subTreinosCount", _subTreinosCount);
                    Nav.NavigateTo(LoginActivity._cadastroTreinoTipoPageKey, treinoId_subTreinosCount);
                    break;

                case 1: //Alterar Treino

                    break;

                case 2: //Excluir Treino
                    RemoveSelectedTreino();
                    LoadTreinos();
                    break;
            }
        }

        private void TreinoTipo_SingleChoiceOKClick(object sender, DialogClickEventArgs e)
        {
            switch (_treinoTipo_SingleChoiceItemSelected)
            {
                default:
                    break;
                case 0: //Exibir Exercícios
                    Dictionary<string, int> alunoTreinoTipoDict = new Dictionary<string, int>();
                    alunoTreinoTipoDict.Add("treinoTipo_id", _treinoTipoSelectedId);
                    Nav.NavigateTo(LoginActivity._execucaoExerciciosPageKey, alunoTreinoTipoDict);
                    break;
                case 1: //Alterar o SubTreino

                    break; //Remover SubTreino
                case 2:
                    RemoveSelectedSubTreino();
                    LoadTreinos();
                    break;

            }
            _treinoTipo_SingleChoiceItemSelected = 0; //Reinicia a pré-seleção do primeiro item
        }

        //private void TreinoTipo_SingleChoiceItemClick(object sender, DialogClickEventArgs args)
        //{
        //    _treinoTipo_SingleChoiceItemSelected = args.Which;
        //}


    }


}