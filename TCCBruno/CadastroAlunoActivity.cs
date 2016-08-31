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
using TCCBruno.DAO;

namespace TCCBruno
{
    [Activity(Label = "Cadastro de Alunos")]
    public class CadastroAlunoActivity : Activity
    {
        private GridLayout _gridLayout;

        private string _nomeAluno;
        public string NomeAluno
        {
            get
            {
                return _nomeAluno
                    ?? (_nomeAluno = FindViewById<EditText>(Resource.Id.EDT_FirstName).Text);
            }
        }

        private string _sobrenomeAluno;
        public string SobrenomeAluno
        {
            get
            {
                return _sobrenomeAluno
                    ?? (_sobrenomeAluno = FindViewById<EditText>(Resource.Id.EDT_LastName).Text);
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email
                    ?? (_email = FindViewById<EditText>(Resource.Id.EDT_Email).Text);
            }
        }



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CadastroAlunoPage);

            // Create your application here
            _gridLayout = FindViewById<GridLayout>(Resource.Id.GRL_CadastroAluno);
        }

        [Java.Interop.Export("BTN_CadastrarAluno_Click")]
        public void BTN_CadastrarAluno_Click(View v)
        {
            if (!Validation.ValidatedFields(_gridLayout))
            {
                Validation.DisplayAlertMessage("Preencha todos os campos antes de cadastrar um Aluno", this);
                return;
            }

            //Campos preenchidos com sucesso:
            Aluno newAluno = new Aluno
            {
                nome_aluno = NomeAluno.ToString() + " " + SobrenomeAluno.ToString(),
                usuario = Email.ToString(),
                senha = "personal2016", //Senha inicial padrão
                status = true //Novo aluno cadastrado => Status = Ativo
            };
            try
            {
                newAluno.InsertAluno(newAluno);
                Validation.DisplayAlertMessage("Aluno cadastrado com sucesso!", this);
            }
            catch
            {
                Validation.DisplayAlertMessage("Erro ao cadastrar Aluno", this);
            }
        }

    }


}