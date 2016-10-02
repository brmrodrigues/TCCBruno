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
using static AvFisica.Enumeration;
using TCCBruno.DAO;
using Microsoft.Practices.ServiceLocation;
using TCCBruno.Adapters;
using Android.Text;
using TCCBruno.Extension;

namespace TCCBruno
{
    [Activity(Label = "CadastrarAvFisica")]
    public class CadastroAvFisicaActivity : ActivityBase
    {
        private readonly int[,] mBraco = new int[6, 4] { { 18, 23, 29, 39 }, { 16, 22, 29, 36 }, { 11, 17, 22, 30 }, { 9, 13, 17, 22 }, { 6, 10, 13, 21 }, { 4, 8, 11, 18 } };
        private readonly int[,] mAbdominal = new int[6, 4] { { 33, 38, 42, 47 }, { 29, 32, 36, 42 }, { 22, 26, 30, 35 }, { 17, 21, 25, 30 }, { 13, 17, 21, 25 }, { 7, 11, 16, 22 } };
        private readonly int[,] mGeral = new int[6, 4] { { 24, 29, 34, 39 }, { 25, 30, 34, 40 }, { 23, 28, 33, 38 }, { 18, 24, 29, 35 }, { 16, 20, 25, 33 }, { 15, 20, 25, 33 } };

        private int _instrutorId;

        private Spinner _meusAlunosSpinner;
        private Spinner _protocoloSpinner;

        //Variáveis de Entrada
        //private EditText _pesoEditText;
        private EditText _tricipitalEditText;
        private EditText _subescapularEditText;
        private EditText _suprailiciaEditText;
        private EditText _abdominalEditText;
        private EditText _peitoralEditText;
        private EditText _axilarMediaEditText;
        private EditText _coxaMedialEditText;
        private EditText _idadeEditText;
        private EditText _radioUEditText;
        private EditText _femuralEditText;

        //Variáveis de Entrada
        public double Peso { get { return FindViewById<EditText>(Resource.Id.EDT_Peso).ToDouble(); } }
        public double Tricipital { get { return FindViewById<EditText>(Resource.Id.EDT_Tricipital).ToDouble(); } }
        public double Subescapular { get { return FindViewById<EditText>(Resource.Id.EDT_Subescapular).ToDouble(); } }
        public double Suprailicia { get { return FindViewById<EditText>(Resource.Id.EDT_Suprailicia).ToDouble(); } }
        public double Abdominal { get { return FindViewById<EditText>(Resource.Id.EDT_Abdominal).ToDouble(); } }
        public double Peitoral { get { return FindViewById<EditText>(Resource.Id.EDT_Peitoral).ToDouble(); } }
        public double AxilarMedia { get { return FindViewById<EditText>(Resource.Id.EDT_AxilarMedia).ToDouble(); } }
        public double CoxaMedial { get { return FindViewById<EditText>(Resource.Id.EDT_CoxaMedial).ToDouble(); } }
        public int Idade { get { return FindViewById<EditText>(Resource.Id.EDT_Idade).Text.ToInt32(); } }
        public double RadioU { get { return FindViewById<EditText>(Resource.Id.EDT_RadioU).ToDouble(); } }
        public double Femural { get { return FindViewById<EditText>(Resource.Id.EDT_Femural).ToDouble(); } }

        //Variáveis de Saída
        public double PesoGordo
        {
            get { return FindViewById<EditText>(Resource.Id.TV_PesoGordo).ToDouble(); }
            set { FindViewById<TextView>(Resource.Id.TV_PesoGordo).Text = value.ToString(); }
        }

        public double PesoMagro
        {
            get { return FindViewById<EditText>(Resource.Id.TV_PesoMagro).ToDouble(); }
            set { PesoMagro = value; FindViewById<TextView>(Resource.Id.TV_PesoMagro).Text = value.ToString(); }
        }

        //public double PesoMagroPorc
        //{
        //    get { return FindViewById<EditText>(Resource.Id.TV_PesoGordo).ToDouble(); }
        //    set { PesoMagroPorc = value; }
        //}

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
            SetContentView(Resource.Layout.CadastroAvFIsicaPage);


            //Get View Controls
            _meusAlunosSpinner = FindViewById<Spinner>(Resource.Id.SPN_MeusAlunosAvFisica);
            _protocoloSpinner = FindViewById<Spinner>(Resource.Id.SPN_ProtocoloAvFis);


            //View Event Handlers
            //_pesoEditText.TextChanged += (s, e) => { Calculate(); };
            FindViewById<EditText>(Resource.Id.EDT_Peso).FocusChange += (s, e) => { Calculate(); };
            FindViewById<EditText>(Resource.Id.EDT_Idade).FocusChange += (s, e) => { Calculate(); };

            _protocoloSpinner.Adapter = new ProtocoloAvFisicaListAdapter(this);

            _instrutorId = Nav.GetAndRemoveParameter<int>(Intent);
            LoadAlunos();
        }

        private void Calculate()
        {
            var peso = Peso;
            var gorduraAtual = GorduraAtual();
            PesoGordo = Convert.ToDouble((gorduraAtual * peso / 100.0));
            //PesoMagro = (Peso - PesoGordo);

        }

        private double GorduraAtual()
        {
            var protocoloSelected = _protocoloSpinner.SelectedItemPosition;
            bool errorCalculation = false;
            double result = 0.0;
            switch (protocoloSelected)
            {
                case 0: //GUEDES 3DC
                    var logInput = Tricipital + Suprailicia + CoxaMedial;
                    var denGuedes = (1.1714 - 0.0671 * Math.Log(logInput <= 0 ? 1 : logInput));
                    if (denGuedes == 0.0)
                        errorCalculation = true;
                    else
                        result = (((4.95 / denGuedes) - 4.5) * 100.0);
                    break;
                case 1: //JACKSON & POLLOCK 3DC
                    var den3DC = (1.10938 - 0.0008267 * (Peitoral + Abdominal + CoxaMedial));
                    if (den3DC == 0.0)
                        errorCalculation = true;
                    else
                        result = (((4.95 / den3DC) + 0.0000016
                                    * Math.Pow(((Peitoral + Abdominal + CoxaMedial)), 2.0) - 0.0002574
                                    * Idade) - 4.5) * 100.0;
                    break;
                case 2: //JACKSON & POLLOCK 7DC
                    var den7DC = (1.112 - 0.00043499 * (Peitoral + Abdominal + CoxaMedial + Suprailicia + AxilarMedia + Tricipital + Subescapular)
                        + 0.00000055 * (Math.Pow((Peitoral + Abdominal + CoxaMedial + Suprailicia + AxilarMedia + Tricipital + Subescapular), 2.0)
                        - 0.00028826 * Idade));
                    if (den7DC == 0.0)
                        errorCalculation = true;
                    else
                        result = ((4.95 / den7DC - 4.5) * 100.0);
                    break;
                default:
                    return 0.0;
            }

            if (errorCalculation)
                Validation.DisplayAlertMessage("Erro no cálculo de Gordura Atual. Informe os valores corretos.", this);

            return result;
        }

        private void LoadAlunos()
        {
            AlunoDAO alunoDAO = new AlunoDAO();
            var pessoasList = alunoDAO.LoadAlunos(_instrutorId);
            if (pessoasList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Alunos", this);
                return;
            }
            //Preence ListView com os Alunos do Instrutor logado
            var listAdapter = new MeusAlunosPageAdapter(this, pessoasList);
            _meusAlunosSpinner.Adapter = listAdapter;
        }

        private ResultadoClassificacao Classificacao(TipoClassificacao tipo, int valor, int idade)
        {
            int[,] values = new int[6, 4];
            int faixaEtaria;
            switch (tipo)
            {
                case TipoClassificacao.ABDOMINAL:
                    values = mAbdominal;
                    break;

                case TipoClassificacao.BRACO:
                    values = mBraco;
                    break;

                case TipoClassificacao.GERAL:
                    values = mGeral;
                    break;
            }

            if (idade < 20)
            {
                faixaEtaria = 0;
            }
            else if (idade < 30)
            {
                faixaEtaria = 1;
            }
            else if (idade < 40)
            {
                faixaEtaria = 2;
            }
            else if (idade < 50)
            {
                faixaEtaria = 3;
            }
            else if (idade < 60)
            {
                faixaEtaria = 4;
            }
            else
            {
                faixaEtaria = 5;
            }

            return GetClass(values, faixaEtaria, valor);
        }

        private static ResultadoClassificacao GetClass(int[,] values, int faixaEtaria, int valor)
        {
            if (valor < values[faixaEtaria, 0])
            {
                return ResultadoClassificacao.RUIM;
            }
            else if (valor < values[faixaEtaria, 1])
            {
                return ResultadoClassificacao.ABAIXO_DA_MEDIA;
            }
            else if (valor < values[faixaEtaria, 2])
            {
                return ResultadoClassificacao.NA_MEDIA;
            }
            else if (valor < values[faixaEtaria, 3])
            {
                return ResultadoClassificacao.ACIMA_DA_MEDIA;
            }
            else
            {
                return ResultadoClassificacao.EXCELENTE;
            }
        }
    }
}