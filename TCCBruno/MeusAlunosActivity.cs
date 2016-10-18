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
    [Activity(Label = "Meus Alunos", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MeusAlunosActivity : Android.Support.V4.App.Fragment
    {
        MeusAlunosPageAdapter _meusAlunosListAdapter;
        ListView _listViewAlunos;
        private int _instrutorId;
        private int _pessoaSelectedId = -1;
        private int _alunoSelectedId = -1;
        private bool _alunoStatusSelected;
        private int _meusAlunos_SingleChoiceItemSelected = 0; //Primeira opção pré-selecionada


        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
        }

        public MeusAlunosActivity()
        {

        }

        public MeusAlunosActivity(int instrutorId)
        {
            _instrutorId = instrutorId;
        }

        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //    SetContentView(Resource.Layout.MeusAlunosPage);

        //    _listViewAlunos = FindViewById<ListView>(Resource.Id.LV_MeusAlunos);
        //    FindViewById<Button>(Resource.Id.BTN_NovoAluno).Click += BTN_NovoAluno_Click;
        //    //Instancia evento de Click da List View
        //    _listViewAlunos.ItemClick += LV_MeusAlunos_ItemClick;
        //    //Recebe o Id do usuário (instrutor) logado no sistema por passagem de parâmetro da tela anterior
        //    _instrutorId = Nav.GetAndRemoveParameter<int>(Intent);
        //    LoadAlunos();
        //}

        private void BTN_NovoAluno_Click(object sender, EventArgs e)
        {
            Nav.NavigateTo(LoginActivity._cadastroAlunoPageKey, _instrutorId);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MeusAlunosPage, container, false);

            _listViewAlunos = view.FindViewById<ListView>(Resource.Id.LV_MeusAlunos);
            view.FindViewById<Button>(Resource.Id.BTN_NovoAluno).Click += BTN_NovoAluno_Click;
            //Instancia evento de Click da List View
            _listViewAlunos.ItemClick += LV_MeusAlunos_ItemClick;
            _listViewAlunos.ItemLongClick += LV_MeusAlunos_ItemLongClick;

            //Recebe o Id do usuário (instrutor) logado no sistema por passagem de parâmetro da tela anterior
            //_instrutorId = Nav.GetAndRemoveParameter<int>(this.Activity.Intent);
            LoadAlunos();

            return view;
        }

        private void LV_MeusAlunos_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int aluno_Id = (int)_listViewAlunos.Adapter.GetItemId(e.Position);
            Dictionary<string, int> instrutorAlunoDict = new Dictionary<string, int>();
            instrutorAlunoDict.Add("instrutor_id", _instrutorId);
            instrutorAlunoDict.Add("aluno_id", aluno_Id);
            Nav.NavigateTo("TreinosPage", instrutorAlunoDict);
        }

        private void LV_MeusAlunos_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Dialog dialog = null;

            _alunoStatusSelected = _meusAlunosListAdapter.GetAlunoStatus(e.Position);
            _pessoaSelectedId = _meusAlunosListAdapter.GetPessoaId(e.Position);
            _alunoSelectedId = (int)_meusAlunosListAdapter.GetItemId(e.Position);

            if (_pessoaSelectedId < 0 || _alunoSelectedId < 0)
            {
                Validation.DisplayAlertMessage("Falha ao selecionar Aluno. Tente novamente", this.Activity);
                return;
            }

            var nomeAluno = _meusAlunosListAdapter.GetNomeAluno(e.Position);
            var args = new Bundle();
            args.PutString("0", nomeAluno);
            dialog = OnCreateDialog(0, args);
            dialog.Show();
        }

        private Dialog OnCreateDialog(int dialogType, Bundle args)
        {
            var builder = new AlertDialog.Builder(this.Activity);
            builder.SetTitle(args.GetString("0"));
            builder.SetSingleChoiceItems(Resource.Array.meusAlunosItemLongClickList, 0, (s, e) => { _meusAlunos_SingleChoiceItemSelected = e.Which; });
            builder.SetPositiveButton("OK", MeusAlunos_SingleChoiceOKClick);
            builder.SetNegativeButton("Cancelar", (s, e) => { });

            return builder.Create();
        }

        private void MeusAlunos_SingleChoiceOKClick(object sender, DialogClickEventArgs e)
        {
            switch (_meusAlunos_SingleChoiceItemSelected)
            {
                default:
                    break;

                case 0: //Ativar/Desativar Aluno
                    ActivateDeactivateSelectedAluno();
                    LoadAlunos();
                    break;
                case 1: //Avaliações Física do Aluno
                    var instrutorAlunoDict = new Dictionary<string, int>();
                    instrutorAlunoDict.Add("instrutor_id", _instrutorId);
                    instrutorAlunoDict.Add("aluno_id", _alunoSelectedId);
                    Nav.NavigateTo(LoginActivity._avaliacaoFisicaPageKey, instrutorAlunoDict);
                    break;
                case 2: //Estatísticas de Falta
                    Nav.NavigateTo(LoginActivity._estatisticasFaltaPageKey, _alunoSelectedId);
                    break;
                case 3: //Remover Aluno do Sistema
                    RemoveSelectedAluno();
                    LoadAlunos();
                    break;
            }
        }

        private void LoadAlunos()
        {
            AlunoDAO alunoDAO = new AlunoDAO();
            var pessoasList = alunoDAO.LoadAlunos(_instrutorId);
            if (pessoasList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Alunos", this.Activity);
                return;
            }
            //Preence ListView com os Alunos do Instrutor logado
            _meusAlunosListAdapter = new MeusAlunosPageAdapter(this.Activity, pessoasList);
            _listViewAlunos.Adapter = _meusAlunosListAdapter;
        }

        private void ActivateDeactivateSelectedAluno()
        {
            AlunoDAO alunoDAO = new AlunoDAO();
            if (!alunoDAO.ActivateDeactivateAluno(_pessoaSelectedId, _alunoStatusSelected))
                Validation.DisplayAlertMessage("Não foi possível ativar/desativar o Aluno selecionado", this.Activity);
        }

        private void RemoveSelectedAluno()
        {
            AlunoDAO alunoDAO = new AlunoDAO();
            if (!alunoDAO.RemoveAluno(_pessoaSelectedId))
                Validation.DisplayAlertMessage("Não foi possível remover o Aluno selecionado", this.Activity);
        }

        public override void OnResume()
        {
            base.OnResume();
            LoadAlunos();
        }


    }
}