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
using TCCBruno.DAO;
using TCCBruno.Model;
using Android.Content.PM;

namespace TCCBruno
{
    [Activity(Label = "Cadastro de Treino", Icon = "@drawable/logoAcademia", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CadastroTreinoActivity : Activity
    {
        Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();
        private Button _btnDataInicio;
        private Button _btnDataFim;
        private string _dataInicio = DateTime.Now.ToShortDateString();
        private string _dataFim = DateTime.Now.AddMonths(3).ToShortDateString();
        private long lastClickTime = 0;


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
            SetContentView(Resource.Layout.CadastroTreinoPage);

            //Get View Controls
            _btnDataInicio = FindViewById<Button>(Resource.Id.BTN_DataInicio);
            _btnDataFim = FindViewById<Button>(Resource.Id.BTN_DataFinal);

            //Eventos
            _btnDataInicio.Click += BTN_DataInicio_Click;
            _btnDataFim.Click += BTN_DataFim_Click;
            FindViewById<Button>(Resource.Id.BTN_CadastrarTreino).Click += BTN_CadastrarTreino_Click;
            // Create your application here
            _btnDataInicio.Text += DateTime.Parse(_dataInicio).ToString("dd/MM/yyyy");
            _btnDataFim.Text += DateTime.Parse(_dataFim).ToString("dd/MM/yyyy");
            _instrutorAlunoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);
        }

        private void BTN_DataInicio_Click(object sender, EventArgs e)
        {
            //OpenCalendar("Data de Início:", ref _dataInicio);
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dataInicio = time.ToShortDateString();
                _btnDataInicio.Text = "Data de Início: " + DateTime.Parse(_dataInicio).ToString("dd/MM/yyyy");
            });
            DatePickerFragment.TITLE = "Data de Início:";
            frag.Show(FragmentManager, DatePickerFragment.TAG);

            //_btnDataInicio.Text = "Data de Início: " + _dataInicio;
        }

        private void BTN_DataFim_Click(object sender, EventArgs e)
        {
            //OpenCalendar("Data Final:", ref _dataFim);
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dataFim = time.ToShortDateString();
                _btnDataFim.Text = "Data Final: " + DateTime.Parse(_dataFim).ToString("dd/MM/yyyy");
            });
            DatePickerFragment.TITLE = "Data Final:";
            DatePickerFragment.CURRENT = DateTime.Parse(_dataInicio).AddMonths(3);
            frag.Show(FragmentManager, DatePickerFragment.TAG);

            //_btnDataFim.Text = "Data Final: " + _dataFim;
        }

        private void BTN_CadastrarTreino_Click(object sender, EventArgs e)
        {
            //Debounce Issue Fix:
            if (SystemClock.ElapsedRealtime() - lastClickTime < 1000) //1000ms
            {
                return;
            }
            lastClickTime = SystemClock.ElapsedRealtime();

            if (_dataInicio.Equals("") || _dataFim.Equals("")) //Verificação se o usuário selecionou alguma data
            {
                Validation.DisplayAlertMessage("Selecione o período do treino antes de fazer o cadastro!", this);
                return;
            }
            //Datas válidas, cadastrar Treino:
            TreinoDAO treinoDAO = new TreinoDAO();
            Treino newTreino = new Treino
            {
                data_inicio = _dataInicio,
                data_fim = _dataFim,
                aluno_id = _instrutorAlunoDict["aluno_id"]
            };
            if (!treinoDAO.InsertTreino(newTreino))
                Validation.DisplayAlertMessage("Não foi possível cadastrar o Treino", this);

            base.OnBackPressed();
        }
    }
}