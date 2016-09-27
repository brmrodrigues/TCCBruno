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
using TCCBruno.Adapters;
using Microsoft.Practices.ServiceLocation;
using TCCBruno.Model;
using TCCBruno.DAO;

namespace TCCBruno
{
    [Activity(Label = "Cadastro de SubTreino")]
    public class CadastroTreinoTipoActivity : ActivityBase
    {
        private Spinner _spnDuracaoTreinoTipo;
        private EditText _edtDescricaoTreinoTipo;
        DuracaoTreinosTipoListAdapter _duraoListAdapter;
        private readonly double[] _duracoesTreinoTipo = { 0.5, 1.0, 1.5, 2.0, 2.5, 3.0 };
        private readonly string[] _nomesTreinoTipo = { "A", "B", "C", "D", "E" };
        //private int _treinoId;
        private Dictionary<string, int> _treinoId_subTreinosCount;
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
            SetContentView(Resource.Layout.CadastroTreinoTipoPage);
            // Create your application here
            _spnDuracaoTreinoTipo = FindViewById<Spinner>(Resource.Id.SPN_DuracaoTreinoTipo);
            FindViewById<Button>(Resource.Id.BTN_SalvarTreinoTipo).Click += BTN_SalvarTreinoTipo_Click;
            _edtDescricaoTreinoTipo = FindViewById<EditText>(Resource.Id.EDT_DescricaoTreinoTipo);

            _duraoListAdapter = new DuracaoTreinosTipoListAdapter(this);
            _spnDuracaoTreinoTipo.Adapter = _duraoListAdapter;

            _treinoId_subTreinosCount = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);

        }

        private void BTN_SalvarTreinoTipo_Click(object sender, EventArgs e)
        {
            //Debounce Issue Fix:
            if (SystemClock.ElapsedRealtime() - lastClickTime < 1000) //1000ms
            {
                return;
            }
            lastClickTime = SystemClock.ElapsedRealtime();

            var selectedItemPos = _spnDuracaoTreinoTipo.SelectedItemPosition;
            //Console.WriteLine(duracoes[selectedItemPos].ToString());
            var subTreinosCount = _treinoId_subTreinosCount["subTreinosCount"];
            if ((subTreinosCount >= _nomesTreinoTipo.Length))
            {
                Validation.DisplayAlertMessage("Não é possível cadastrar mais do que cinco SubTreinos", this);
            }
            else
            {


                Treino_Tipo newTreinoTipo = new Treino_Tipo
                {
                    treino_id = _treinoId_subTreinosCount["treinoId"],
                    treino_tipo_nome = _nomesTreinoTipo[subTreinosCount],
                    duracao = _duracoesTreinoTipo[selectedItemPos],
                    descricao = _edtDescricaoTreinoTipo.Text
                };

                Treino_TipoDAO treinoTipoDAO = new Treino_TipoDAO();
                if (treinoTipoDAO.InsertTreino_Tipo(newTreinoTipo))
                {
                    GoBack();
                }
                else
                {
                    Validation.DisplayAlertMessage("Não foi possível cadastrar o SubTreino", this);
                }
            }
        }
    }
}