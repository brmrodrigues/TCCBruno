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
    public class MeusAlunosActivity : ActivityBase
    {

        ListView _listViewAlunos;

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
            SetContentView(Resource.Layout.MeusAlunosPage);

            _listViewAlunos = FindViewById<ListView>(Resource.Id.LV_MeusAlunos);
            //Recebe o Id do usuário (instrutor) logado no sistema por passagem de parâmetro da tela anterior
            int instrutorId = Nav.GetAndRemoveParameter<int>(Intent);
            LoadAlunos(instrutorId);
        }

        private void LoadAlunos(int instrutorId)
        {
            AlunoDAO alunoDAO = new AlunoDAO();
            var pessoasArray = alunoDAO.LoadAlunos(instrutorId);
            if (pessoasArray == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Alunos", this);
                return;
            }
            var listAdapter = new MeusAlunosPageAdapter(this, pessoasArray);
            _listViewAlunos.Adapter = listAdapter;
        }
    }
}