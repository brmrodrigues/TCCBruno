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
        private ExpandableListView _treinosListView;
        //private ListView _treinosListView;
        Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();
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

            _treinosListView = FindViewById<ExpandableListView>(Resource.Id.LV_Treinos);
            //_treinosListView = FindViewById<ListView>(Resource.Id.LV_Treinos);

            _treinosListView.ItemLongClick += LV_Treinos_ItemLongClick;
            _treinosListView.ChildClick += LV_Treinos_ChildClick;

            FindViewById<Button>(Resource.Id.BTN_NovoTreino).Click += BTN_NovoTreino_Click;

            //Recebe o Id do usuário (instrutor) logado no sistema por passagem de parâmetro da tela anterior
            _instrutorAlunoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);
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
            if (itemType == PackedPositionType.Child)
            {
                //TODO: Se for Child (TreinoTIpo), exibir Fragment que perguntará se o Instrutor deseja:
                //1 - Exibir Exercícios do Subtreino
                //2 - Alterar o Subtreino (por enquanto apenas a duração)
                //3 - Remover o Subtreino

                //var treinoTipoId = (int)_treinosListView.Adapter.GetItemId(e.Position);
                //int treinoPosition = e.Parent.SelectedItemPosition;
                //Treino_Tipo treinoTipoSelected = _treinosAdapter.GetChild(treinoPos, treinoTipoPos).Cast<Treino_Tipo>();
                var treinoTipoNome = _treinosAdapter.GetNomeTreinoTipo(treinoPos, treinoTipoPos);
                _treinoTipoSelectedId = (int)_treinosAdapter.GetChildId(treinoPos, treinoTipoPos);
                //_treinosListView.Adapter.
                //Treino_Tipo treinoTipoSelected = itemSelected.Cast<Treino_Tipo>();
                var args = new Bundle();
                //args.PutString("0", treinoTipoSelected.treino_tipo_nome);
                args.PutString("0", treinoTipoNome);
                dialog = OnCreateDialog(DIALOG_TREINO_TIPO, args);
                //var treinoTipoId = (int)(_treinosListView.Adapter.GetItemId(e.Position));
                //Nav.NavigateTo(MainActivity._cadastroExerciciosPageKey, treinoTipoId);
            }
            else if (itemType == PackedPositionType.Group)
            {
                //TODO: Se for do Group (Treino), exibir Fragment que perguntará se o Instrutor deseja:
                //1 - Cadastrar Subtreino
                //2 - Alterar Treino
                //3 - Excluir Treino 
                _treinoSelectedId = (int)_treinosAdapter.GetGroupId(treinoPos); //armazena o id do treino selecionado
                _subTreinosCount = (int)_treinosAdapter.GetChildrenCount(treinoPos);
                var args = new Bundle();
                args.PutString("0", (treinoPos + 1).ToString());
                dialog = OnCreateDialog(DIALOG_TREINO, args);
            }
            dialog.Show();
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

        private void BTN_NovoTreino_Click(object sender, EventArgs e)
        {
            Nav.NavigateTo(MainActivity._cadastroTreinoPageKey, _instrutorAlunoDict);
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
                    builder.SetSingleChoiceItems(Resource.Array.subTreinoItemLongClickList, 0, (s, e) => { _treinoTipoSelectedId = e.Which; });
                    builder.SetPositiveButton("OK", TreinoTipo_SingleChoiceOKClick);
                    builder.SetNegativeButton("Cancelar", (s, e) => { });
                    break;
                    //case DIALOG_CADASTRO_TREINO_TIPO:
                    //    builder.SetTitle("Novo SubTreino");
                    //    //builder.SetSingleChoiceItems(Resource.Array.subTreinoItemLongClickList, 0, (s, e) => { _treinoTipoSelectedId = e.Which; });
                    //    var adapter = new DuracaoTreinosTipoListAdapter(this);
                    //    builder.SetAdapter(adapter, LV_DuracaoSubTreino_ItemClick);
                    //    builder.SetPositiveButton("OK", TreinoTipo_SingleChoiceOKClick);
                    //    builder.SetNegativeButton("Cancelar", (s, e) => { });
                    //    break;
            }



            return builder.Create();
        }

        private void LV_DuracaoSubTreino_ItemClick(object sender, DialogClickEventArgs e)
        {
            //Treino_Tipo newTreino_Tipo = new Treino_Tipo
            //{
            //    treino_id = _treinoSelectedId,
            //    treino_tipo_nome
            //};

            //Treino_TipoDAO treinoTipoDAO = new Treino_TipoDAO();
            //if (treinoTipoDAO.InsertTreino_Tipo())
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
                    Nav.NavigateTo(MainActivity._cadastroTreinoTipoPageKey, treinoId_subTreinosCount);
                    break;

                case 1: //Alterar Treino

                    break;

                case 2: //Excluir Treino

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
                    Nav.NavigateTo(MainActivity._execucaoExerciciosPageKey, _treinoTipoSelectedId);
                    break;
                case 1: //Alterar a duração do SubTreino

                    break; //Remover SubTreino
                case 2:

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