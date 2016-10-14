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

namespace TCCBruno
{
    [Activity(Label = "Meus Alunos")]
    public class MeusAlunosActivity : Android.Support.V4.App.Fragment
    {

        ListView _listViewAlunos;
        int _instrutorId;

        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
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
            var listAdapter = new MeusAlunosPageAdapter(this.Activity, pessoasList);
            _listViewAlunos.Adapter = listAdapter;
        }

        private void LV_MeusAlunos_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int aluno_Id = (int)_listViewAlunos.Adapter.GetItemId(e.Position);
            Dictionary<string, int> instrutorAlunoDict = new Dictionary<string, int>();
            instrutorAlunoDict.Add("instrutor_id", _instrutorId);
            instrutorAlunoDict.Add("aluno_id", aluno_Id);
            Nav.NavigateTo("TreinosPage", instrutorAlunoDict);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MeusAlunosPage, container, false);

            _listViewAlunos = view.FindViewById<ListView>(Resource.Id.LV_MeusAlunos);
            view.FindViewById<Button>(Resource.Id.BTN_NovoAluno).Click += BTN_NovoAluno_Click;
            //Instancia evento de Click da List View
            _listViewAlunos.ItemClick += LV_MeusAlunos_ItemClick;

            //Recebe o Id do usuário (instrutor) logado no sistema por passagem de parâmetro da tela anterior
            //_instrutorId = Nav.GetAndRemoveParameter<int>(this.Activity.Intent);
            LoadAlunos();

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            LoadAlunos();
        }


    }
}