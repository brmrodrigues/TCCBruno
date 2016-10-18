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
using System.Globalization;
using Android.Content.PM;

namespace TCCBruno
{
    [Activity(Label = "Cadastro de Novo Aluno", Icon = "@drawable/logoAcademia", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CadastroAlunoActivity : ActivityBase
    {
        private GridLayout _gridLayout;
        private int _instrutorId;
        private long lastClickTime = 0;


        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
        }

        public string NomeAluno
        {
            get
            {
                return FindViewById<EditText>(Resource.Id.EDT_FirstName).Text;
            }
        }

        public string SobrenomeAluno
        {
            get
            {
                return FindViewById<EditText>(Resource.Id.EDT_LastName).Text;
            }
        }

        public string DataNascimento
        {
            get
            {
                return FindViewById<EditText>(Resource.Id.EDT_DataNascimento).Text;
            }
        }

        public string Email
        {
            get
            {
                return FindViewById<EditText>(Resource.Id.EDT_Email).Text;
            }
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CadastroAlunoPage);

            _gridLayout = FindViewById<GridLayout>(Resource.Id.GRL_CadastroAluno);

            //Recebe o Id do usuário (instrutor) logado no sistema por passagem de parâmetro da tela anterior
            _instrutorId = Nav.GetAndRemoveParameter<int>(Intent);

        }

        [Java.Interop.Export("BTN_CadastrarAluno_Click")]
        public void BTN_CadastrarAluno_Click(View v)
        {
            //Debounce Issue Fix:
            if (SystemClock.ElapsedRealtime() - lastClickTime < 1000) //1000ms
            {
                return;
            }
            lastClickTime = SystemClock.ElapsedRealtime();

            if (!Validation.ValidatedFields(_gridLayout))
            {
                Validation.DisplayAlertMessage("Preencha todos os campos antes de cadastrar um Aluno", this);
                return;
            }

            //Campos preenchidos com sucesso:
            Pessoa newPessoa = new Pessoa
            {
                nome_pessoa = NomeAluno.ToString() + " " + SobrenomeAluno.ToString(),
                usuario = Email.ToString(),
                senha = "personal2016", //Senha inicial padrão
                status = true //Nova pessoa cadastrado => Status = Ativo
            };

            AlunoDAO alunoDAO = new AlunoDAO();
            if (alunoDAO.InsertPessoa(_instrutorId, newPessoa, (DateTime.ParseExact(DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToShortDateString()))
            {
                Validation.DisplayAlertMessage("Aluno cadastrado com sucesso!", this);
            }
            else
            {
                Validation.DisplayAlertMessage("Não foi possível cadastrar o Aluno", this);
            }

            //Volta para a tela MeusAlunos
            GoBack();
        }

    }


}