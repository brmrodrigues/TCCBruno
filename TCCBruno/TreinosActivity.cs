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

            if (itemType == PackedPositionType.Child)
            {
                //TODO: Se for Child (TreinoTIpo), exibir Fragment que perguntará se o Instrutor deseja:
                //1 - Exibir Exercícios do Subtreino
                //2 - Alterar o Subtreino (por enquanto apenas a duração)
                //3 - Remover o Subtreino

                //var treinoTipoId = (int)(_treinosListView.Adapter.GetItemId(e.Position));
                //Nav.NavigateTo(MainActivity._cadastroExerciciosPageKey, treinoTipoId);
            }
            else if (itemType == PackedPositionType.Group)
            {
                //TODO: Se for do Group (Treino), exibir Fragment que perguntará se o Instrutor deseja:
                //1 - Cadastrar Subtreino
                //2 - Alterar Treino
                //3 - Excluir Treino 
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
            var listAdapter = new TreinosExpandableListAdapter(this, treinosList);
            _treinosListView.SetAdapter(listAdapter);

            //var listAdapter = new TreinosAdapter(this, treinosList);
            //_treinosListView.Adapter = listAdapter;
        }

        private void BTN_NovoTreino_Click(object sender, EventArgs e)
        {
            Nav.NavigateTo(MainActivity._cadastroTreinoPageKey, _instrutorAlunoDict);
            //OpenCalendar("Data de Início");
            //OpenCalendar("Data Final");
        }



        /// <summary>
        /// Ao voltar a esta tela, carrega os treinos novamente do BD
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            LoadTreinos();
        }
    }


}