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
using TCCBruno.Model;

namespace TCCBruno
{
    [Activity(Label = "Cadastro de Avaliação Física", Icon = "@drawable/logoAcademia")]
    public class CadastroAvFisicaActivity : ActivityBase
    {
        private readonly int[,] mBraco = new int[6, 4] { { 18, 23, 29, 39 }, { 16, 22, 29, 36 }, { 11, 17, 22, 30 }, { 9, 13, 17, 22 }, { 6, 10, 13, 21 }, { 4, 8, 11, 18 } };
        private readonly int[,] mAbdominal = new int[6, 4] { { 33, 38, 42, 47 }, { 29, 32, 36, 42 }, { 22, 26, 30, 35 }, { 17, 21, 25, 30 }, { 13, 17, 21, 25 }, { 7, 11, 16, 22 } };
        private readonly int[,] mGeral = new int[6, 4] { { 24, 29, 34, 39 }, { 25, 30, 34, 40 }, { 23, 28, 33, 38 }, { 18, 24, 29, 35 }, { 16, 20, 25, 33 }, { 15, 20, 25, 33 } };

        private Aluno _aluno;
        private bool _calculosOk = false;

        //private Spinner _meusAlunosSpinner;
        private Spinner _protocoloSpinner;

        private RadioButton _masculinoRadioButton;
        private RadioButton _femininoRadioButton;

        //Variáveis de Entrada
        public double Idade { get { return FindViewById<EditText>(Resource.Id.EDT_Idade).Text.ToDouble(); } }
        public double Peso { get { return FindViewById<EditText>(Resource.Id.EDT_Peso).ToDouble(); } }
        public double Estatura { get { return FindViewById<EditText>(Resource.Id.EDT_Estatura).ToDouble(); } }
        public double PressaoArterial { get { return FindViewById<EditText>(Resource.Id.EDT_PressaoArterial).ToDouble(); } }

        //Perímetros
        public double Torax { get { return FindViewById<EditText>(Resource.Id.EDT_Torax).ToDouble(); } }
        public double Cintura { get { return FindViewById<EditText>(Resource.Id.EDT_Cintura).ToDouble(); } }
        public double Abdome { get { return FindViewById<EditText>(Resource.Id.EDT_Abdome).ToDouble(); } }
        public double BracoDir { get { return FindViewById<EditText>(Resource.Id.EDT_BracoDir).ToDouble(); } }
        public double BracoEsq { get { return FindViewById<EditText>(Resource.Id.EDT_BracoEsq).ToDouble(); } }
        public double AntebrDir { get { return FindViewById<EditText>(Resource.Id.EDT_AntebrDir).ToDouble(); } }
        public double AntebrEsq { get { return FindViewById<EditText>(Resource.Id.EDT_AntebrEsq).ToDouble(); } }
        public double Quadril { get { return FindViewById<EditText>(Resource.Id.EDT_Quadril).ToDouble(); } }
        public string CoxaDir { get { return FindViewById<EditText>(Resource.Id.EDT_CoxaDir).Text; } }
        public string CoxaEsq { get { return FindViewById<EditText>(Resource.Id.EDT_CoxaEsq).Text; } }
        public double PernaDir { get { return FindViewById<EditText>(Resource.Id.EDT_PernaDir).ToDouble(); } }
        public double PernaEsq { get { return FindViewById<EditText>(Resource.Id.EDT_PernaEsq).ToDouble(); } }
        public double Ombro { get { return FindViewById<EditText>(Resource.Id.EDT_Ombro).ToDouble(); } }


        //Dobras Cutâneas
        public double Tricipital { get { return FindViewById<EditText>(Resource.Id.EDT_Tricipital).ToDouble(); } }
        public double Subescapular { get { return FindViewById<EditText>(Resource.Id.EDT_Subescapular).ToDouble(); } }
        public double Suprailicia { get { return FindViewById<EditText>(Resource.Id.EDT_Suprailicia).ToDouble(); } }
        public double Abdominal { get { return FindViewById<EditText>(Resource.Id.EDT_Abdominal).ToDouble(); } }
        public double Peitoral { get { return FindViewById<EditText>(Resource.Id.EDT_Peitoral).ToDouble(); } }
        public double AxilarMedia { get { return FindViewById<EditText>(Resource.Id.EDT_AxilarMedia).ToDouble(); } }
        public double CoxaMedial { get { return FindViewById<EditText>(Resource.Id.EDT_CoxaMedial).ToDouble(); } }
        public double RadioU { get { return FindViewById<EditText>(Resource.Id.EDT_RadioU).ToDouble(); } }
        public double Femural { get { return FindViewById<EditText>(Resource.Id.EDT_Femural).ToDouble(); } }

        //Exercícios
        public int FlexGeral1 { get { return FindViewById<EditText>(Resource.Id.EDT_FlexGeral1).Text.ToInt32(); } }
        public int FlexGeral2 { get { return FindViewById<EditText>(Resource.Id.EDT_FlexGeral2).Text.ToInt32(); } }
        public int FlexGeral3 { get { return FindViewById<EditText>(Resource.Id.EDT_FlexGeral3).Text.ToInt32(); } }
        public int FlexBraco { get { return FindViewById<EditText>(Resource.Id.EDT_FlexBraco).Text.ToInt32(); } }
        public int AbdominaisRep { get { return FindViewById<EditText>(Resource.Id.EDT_AbdominaisRep).Text.ToInt32(); } }


        //Variáveis de Saída

        //Fracionamento
        TextView TV_PesoMuscular;
        TextView TV_PesoGordo;
        TextView TV_PesoMagro;
        TextView TV_PorcPesoMuscular;
        TextView TV_PorcGorduraAtual;
        TextView TV_PorcPesoMagro;
        TextView TV_PesoIdeal;
        TextView TV_Excesso;
        TextView TV_PesoOsseo;
        TextView TV_PesoResidual;

        //Classificações
        TextView TV_IMC;
        TextView TV_RCQ;
        TextView TV_IAC;
        TextView TV_PA;

        //Classificações - Exercícios
        TextView TV_FlexibilidadeGeral;
        TextView TV_FlexoesBraco;
        TextView TV_Abdominais;



        //FRACIONAMENTO
        public double PesoMuscular
        {
            get { return TV_PesoMuscular.Text.ToDouble(); }
            set { TV_PesoMuscular.Text = value.ToString("0.00"); }
        }

        public double PesoGordo
        {
            get { return TV_PesoGordo.Text.ToDouble(); }
            set { TV_PesoGordo.Text = value.ToString("0.00"); }
        }

        public double PesoMagro
        {
            get { return TV_PesoMagro.Text.ToDouble(); }
            set { TV_PesoMagro.Text = value.ToString("0.00"); }
        }

        public double PorcPesoMuscular
        {
            get { return TV_PorcPesoMuscular.Text.ToDouble(); }
            set { TV_PorcPesoMuscular.Text = value.ToString("0.00"); }
        }
        public double PorcGorduraAtual
        {
            get { return TV_PorcGorduraAtual.Text.ToDouble(); }
            set { TV_PorcGorduraAtual.Text = value.ToString("0.00"); }
        }
        public double PorcPesoMagro
        {
            get { return TV_PorcPesoMagro.Text.ToDouble(); }
            set { TV_PorcPesoMagro.Text = value.ToString("0.00"); }
        }
        public double PesoIdeal
        {
            get { return TV_PesoIdeal.Text.ToDouble(); }
            set { TV_PesoIdeal.Text = value.ToString("0.00"); }
        }
        public double Excesso
        {
            get { return TV_Excesso.Text.ToDouble(); }
            set { TV_Excesso.Text = value.ToString("0.00"); }
        }
        public double PesoOsseo
        {
            get { return TV_PesoOsseo.Text.ToDouble(); }
            set { TV_PesoOsseo.Text = value.ToString("0.00"); }
        }
        public double PesoResidual
        {
            get { return TV_PesoResidual.Text.ToDouble(); }
            set { TV_PesoResidual.Text = value.ToString("0.00"); }
        }

        //Classificações
        public double IMC
        {
            get { return TV_IMC.Text.ToDouble(); }
            set { TV_IMC.Text = value.ToString("0.00"); }
        }

        public double RCQ
        {
            get { return TV_RCQ.Text.ToDouble(); }
            set { TV_RCQ.Text = value.ToString("0.00"); }
        }

        public double IAC
        {
            get { return TV_IAC.Text.ToDouble(); }
            set { TV_IAC.Text = value.ToString("0.00"); }
        }

        public double PA
        {
            get { return TV_PA.Text.ToDouble(); }
            set { TV_PA.Text = value.ToString("0.00"); }
        }

        //Classificações - Exercícios
        public double FlexibilidadeGeral
        {
            get { return TV_FlexibilidadeGeral.Text.ToDouble(); }
            set { TV_FlexibilidadeGeral.Text = value.ToString("0.00"); }
        }

        public double FlexoesBraco
        {
            get { return TV_FlexoesBraco.Text.ToDouble(); }
            set { TV_FlexoesBraco.Text = value.ToString("0.00"); }
        }

        public double Abdominais
        {
            get { return TV_Abdominais.Text.ToDouble(); }
            set { TV_Abdominais.Text = value.ToString("0.00"); }
        }


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
            SetContentView(Resource.Layout.CadastroAvFisicaPage);


            //Get View Controls
            //_meusAlunosSpinner = FindViewById<Spinner>(Resource.Id.SPN_MeusAlunosAvFisica);
            _protocoloSpinner = FindViewById<Spinner>(Resource.Id.SPN_ProtocoloAvFis);
            _masculinoRadioButton = FindViewById<RadioButton>(Resource.Id.RB_Masculino);
            _femininoRadioButton = FindViewById<RadioButton>(Resource.Id.RB_Feminino);


            //Resultados (TextViews)

            //Fracionamento
            TV_PesoMuscular = FindViewById<TextView>(Resource.Id.TV_PesoMuscular); ;
            TV_PesoGordo = FindViewById<TextView>(Resource.Id.TV_PesoGordo); ;
            TV_PesoMagro = FindViewById<TextView>(Resource.Id.TV_PesoMagro); ;
            TV_PorcPesoMuscular = FindViewById<TextView>(Resource.Id.TV_PorcPesoMuscular); ;
            TV_PorcGorduraAtual = FindViewById<TextView>(Resource.Id.TV_PorcGorduraAtual); ;
            TV_PorcPesoMagro = FindViewById<TextView>(Resource.Id.TV_PorcPesoMagro); ;
            TV_PesoIdeal = FindViewById<TextView>(Resource.Id.TV_PesoIdeal); ;
            TV_Excesso = FindViewById<TextView>(Resource.Id.TV_Excesso); ;
            TV_PesoOsseo = FindViewById<TextView>(Resource.Id.TV_PesoOsseo); ;
            TV_PesoResidual = FindViewById<TextView>(Resource.Id.TV_PesoResidual); ;

            //Classificações
            TV_IMC = FindViewById<TextView>(Resource.Id.TV_IMC);
            TV_RCQ = FindViewById<TextView>(Resource.Id.TV_RCQ);
            TV_IAC = FindViewById<TextView>(Resource.Id.TV_IAC);
            TV_PA = FindViewById<TextView>(Resource.Id.TV_PA);

            //Classificações-Exercícios
            TV_FlexibilidadeGeral = FindViewById<TextView>(Resource.Id.TV_FlexibilidadeGeral);
            TV_FlexoesBraco = FindViewById<TextView>(Resource.Id.TV_FlexoesBraco);
            TV_Abdominais = FindViewById<TextView>(Resource.Id.TV_Abdominais);


            //View Event Handlers
            FindViewById<Button>(Resource.Id.BTN_Calcular).Click += BTN_Calcular_Click;
            FindViewById<Button>(Resource.Id.BTN_CadastrarAvFisica).Click += BTN_CadastrarAvFisica_Click;

            _protocoloSpinner.Adapter = new ProtocoloAvFisicaListAdapter(this);

            _aluno = Nav.GetAndRemoveParameter<Aluno>(Intent);
            //LoadAlunos();
        }

        private void BTN_Calcular_Click(object sender, EventArgs e)
        {
            try
            {
                Calculate();
                _calculosOk = true;
            }
            catch
            {
                Validation.DisplayAlertMessage("Preenche todos os campos corretamente antes de realizar os cálculos.", this);
                _calculosOk = false;
            }
        }

        private void BTN_CadastrarAvFisica_Click(object sender, EventArgs e)
        {
            if (!_calculosOk)
            {
                Validation.DisplayAlertMessage("Falha ao cadastrar Avaliação. Realize os cálculos novamente.", this);
                return;
            }

            Avaliacao_FisicaDAO avFisicaDAO = new Avaliacao_FisicaDAO();
            Avaliacao_Fisica newAvFisica = new Avaliacao_Fisica()
            {
                aluno_id = _aluno.aluno_id,
                peso = Peso.ToDecimal(),
                estatura = Estatura.ToDecimal(),
                pressao_arterial = PressaoArterial.ToDecimal(),
                torax = Torax.ToDecimal(),
                cintura = Cintura.ToDecimal(),
                abdome = Abdome.ToDecimal(),
                braco_dir = BracoDir.ToDecimal(),
                braco_esq = BracoEsq.ToDecimal(),
                antebr_dir = AntebrDir.ToDecimal(),
                antebr_esq = AntebrEsq.ToDecimal(),
                quadril = Quadril.ToDecimal(),
                coxa_dir = CoxaDir,
                coxa_esq = CoxaEsq,
                perna_dir = PernaDir.ToDecimal(),
                perna_esq = PernaEsq.ToDecimal(),
                ombro = Ombro.ToDecimal(),
                tricipital = Tricipital.ToDecimal(),
                subescapular = Subescapular.ToDecimal(),
                suprailicia = Suprailicia.ToDecimal(),
                abdominal = Abdominal.ToDecimal(),
                peitoral = Peitoral.ToDecimal(),
                axilar_media = AxilarMedia.ToDecimal(),
                coxa_medial = CoxaMedial.ToDecimal(),
                radio_u = RadioU.ToDecimal(),
                femural = Femural.ToDecimal(),
                flex_geral1 = FlexGeral1,
                flex_geral2 = FlexGeral2,
                flex_geral3 = FlexGeral3,
                flex_braco = FlexBraco,
                abdominais_rep = AbdominaisRep

            };
            if (!avFisicaDAO.InsertAvaliacao(newAvFisica))
                Validation.DisplayAlertMessage("Não foi possível cadastrar a Avalicação Física. Tente novamente.", this);
            else
            {
                Validation.DisplayAlertMessage("Avaliação Física cadastrada com sucesso!", this);
            }


        }

        private void Calculate()
        {
            var peso = Peso;
            var porcGorduraAtual = GorduraAtual();
            var pesoGordo = Convert.ToDouble(((porcGorduraAtual / Peso) * 100 * peso / 100.0));
            var pesoMagro = (peso - pesoGordo);
            var porcPesoMagro = 100.0 - porcGorduraAtual;
            var pesoIdeal = pesoMagro / 0.85;
            var radioU = RadioU;
            var femural = Femural;
            var estatura = Estatura;
            var pow1 = Math.Pow(estatura / 100, 2.0);
            var pow2 = Math.Pow(((radioU / 100) * (femural / 100) * 400), 0.712);
            var pesoOsseo = 3.02 * (Math.Pow(estatura / 100, 2.0) * Math.Pow(((radioU / 100) * (femural / 100) * 400), 0.712));
            var pesoResidual = (peso * 24.1) / 100.0;
            var pesoMuscular = peso - (pesoGordo + pesoOsseo + pesoResidual);
            var porcPesoMuscular = (pesoMuscular / peso) * 100.0;
            //PesoMuscular = peso - (pesoGordo + pesoOsseo + pesoResidual);
            //var porcPesoMuscular = (PesoMuscular / peso) * 100.0;
            var excesso = peso - pesoIdeal;

            //Classificações IMC, RCQ e P.A.
            //IMC
            var imc = peso / Math.Pow((estatura / 100), 2.0);
            string imcClass = imc <= 12 ? " Desnutrição crônica" :
                                imc > 12 && imc < 18.5 ? " Baixo Peso" :
                                imc >= 18.5 && imc < 25 ? " Recomendável" :
                                imc >= 25 && imc < 30 ? " Sobrepeso" :
                                imc >= 30 && imc < 35 ? " Obeso Grau 1" :
                                imc >= 35 && imc < 40 ? " Obeso Grau 2" :
                                /*imc >= 40*/ " Obeso Grau 3";
            //RCQ
            var cintura = Cintura;
            var quadril = Quadril;
            var rcq = cintura / quadril;
            string rcqClass = (_masculinoRadioButton.Checked && rcq > 0.95) ? " Risco" :
                                (_femininoRadioButton.Checked && rcq > 0.8) ? " Risco" :
                                " Normal";
            //IAC
            var iac = quadril / (((estatura / 100) * Math.Sqrt(estatura / 100.0))) - 18.0;
            string iacClass = iac < 11 ? " Muito Baixa" :
                                iac < 15 ? " Baixa" :
                                iac < 19 ? " Ideal" :
                                iac <= 25 ? " Moderada" :
                                /*iac > 25*/ " Excesso";
            //PA
            var pa = PressaoArterial;
            string paClass = pa < 120 ? " Normal" :
                                pa < 140 ? " Pré-Hipertensão" :
                                pa < 160 ? " Hipertensão-1" :
                                /*pa >= 160 */" Hipertensão-2";

            //Exercícios

            //Flexibilidade Geral
            var flexGeralMean = (FlexGeral1 + FlexGeral2 + FlexGeral3) / 3.0;
            var flexGeralClass = ClassificacaoExercicio(TipoClassExercicio.GERAL, Convert.ToInt32(flexGeralMean), Idade).ToStringNum();
            //Flexões Braço
            var flexBracoClass = ClassificacaoExercicio(TipoClassExercicio.BRACO, Convert.ToInt32(FlexBraco), Idade).ToStringNum();
            //Abdominais
            var abdominaisClass = ClassificacaoExercicio(TipoClassExercicio.ABDOMINAL, Convert.ToInt32(AbdominaisRep), Idade).ToStringNum();


            //Apresentar os resultados na Page:
            //Fracionamento
            PesoMuscular = pesoMuscular;
            PesoGordo = pesoGordo;
            PesoMagro = pesoMagro;
            PorcPesoMuscular = porcPesoMuscular;
            PorcGorduraAtual = porcGorduraAtual;
            PorcPesoMagro = porcPesoMagro;
            PesoIdeal = pesoIdeal;
            Excesso = excesso;
            PesoOsseo = pesoOsseo;
            PesoResidual = pesoResidual;

            //Valores das Classificações
            IMC = imc;
            RCQ = rcq;
            IAC = iac;
            PA = pa;
            //Classificações
            TV_IMC.Text += " - " + imcClass;
            TV_RCQ.Text += " - " + rcqClass;
            TV_IAC.Text += " - " + iacClass;
            TV_PA.Text += " - " + paClass;
            //Classificações - Exercícios
            TV_FlexibilidadeGeral.Text = flexGeralMean.ToString("0.00") + " - " + flexGeralClass;
            TV_FlexoesBraco.Text = FlexBraco.ToString("0.00") + " - " + flexBracoClass;
            TV_Abdominais.Text = AbdominaisRep.ToString("0.00") + " - " + abdominaisClass;
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
                    var auxTerms = (Peitoral + Abdominal + CoxaMedial + Suprailicia + AxilarMedia + Tricipital + Subescapular);
                    var aux2 = 1.112 - 0.00043499 * auxTerms;
                    var den7DC = (1.112 - 0.00043499 * auxTerms
                        + 0.00000055 * (Math.Pow(auxTerms, 2.0)
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

        //private void LoadAlunos()
        //{
        //    AlunoDAO alunoDAO = new AlunoDAO();
        //    var pessoasList = alunoDAO.LoadAlunos(_instrutorId);
        //    if (pessoasList == null)
        //    {
        //        Validation.DisplayAlertMessage("Falha ao carregar Alunos", this);
        //        return;
        //    }
        //    //Preence ListView com os Alunos do Instrutor logado
        //    var listAdapter = new MeusAlunosPageAdapter(this, pessoasList);
        //    _meusAlunosSpinner.Adapter = listAdapter;
        //}

        private ResultadoClassExercicio ClassificacaoExercicio(TipoClassExercicio tipo, int valor, double idade)
        {
            int[,] values = new int[6, 4];
            int faixaEtaria;
            switch (tipo)
            {
                case TipoClassExercicio.ABDOMINAL:
                    values = mAbdominal;
                    break;

                case TipoClassExercicio.BRACO:
                    values = mBraco;
                    break;

                case TipoClassExercicio.GERAL:
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

        private static ResultadoClassExercicio GetClass(int[,] values, int faixaEtaria, int valor)
        {
            if (valor < values[faixaEtaria, 0])
            {
                return ResultadoClassExercicio.RUIM;
            }
            else if (valor < values[faixaEtaria, 1])
            {
                return ResultadoClassExercicio.ABAIXO_DA_MEDIA;
            }
            else if (valor < values[faixaEtaria, 2])
            {
                return ResultadoClassExercicio.NA_MEDIA;
            }
            else if (valor < values[faixaEtaria, 3])
            {
                return ResultadoClassExercicio.ACIMA_DA_MEDIA;
            }
            else
            {
                return ResultadoClassExercicio.EXCELENTE;
            }
        }
    }
}