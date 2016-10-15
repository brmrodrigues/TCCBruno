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
using TCCBruno.Adapters;
using TCCBruno.Model;
using TCCBruno.Extension;

namespace TCCBruno
{
    [Activity(Label = "Cadastro de Exercício", Icon = "@drawable/logoAcademia")]
    public class CadastroExecucao_ExercicioActivity : ActivityBase
    {
        private int _treinoTipoId;
        private Spinner _spnCategorias;
        private Spinner _spnExercicios;
        private EditText _edtSeries;
        private EditText _edtRepeticoes;
        private EditText _edtCarga;
        private EditText _edtDescanso;

        private Button _btnSalvarExecucaoExercicio;
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
            SetContentView(Resource.Layout.CadastroExecucao_ExercicioPage);

            _edtSeries = FindViewById<EditText>(Resource.Id.EDT_Series);
            _edtRepeticoes = FindViewById<EditText>(Resource.Id.EDT_Repeticoes);
            _edtCarga = FindViewById<EditText>(Resource.Id.EDT_Carga);
            _edtDescanso = FindViewById<EditText>(Resource.Id.EDT_Descanso);
            _spnCategorias = FindViewById<Spinner>(Resource.Id.SPN_CategoriaExercicio);
            _spnExercicios = FindViewById<Spinner>(Resource.Id.SPN_Exercicios);
            _btnSalvarExecucaoExercicio = FindViewById<Button>(Resource.Id.BTN_SalvarExecucaoExercicio);


            //Eventos:
            _spnCategorias.ItemSelected += SPN_CategoriaExercicio_ItemSelected;
            _btnSalvarExecucaoExercicio.Click += BTN_SalvarExecucaoExercicio_Click;

            _treinoTipoId = Nav.GetAndRemoveParameter<int>(Intent);
            LoadCategoriasAndExercicios();
        }

        private void BTN_SalvarExecucaoExercicio_Click(object sender, EventArgs e)
        {
            //Debounce Issue Fix:
            if (SystemClock.ElapsedRealtime() - lastClickTime < 1000) //1000ms
            {
                return;
            }
            lastClickTime = SystemClock.ElapsedRealtime();

            Execucao_Exercicio newExecucaoExercicio = new Execucao_Exercicio
            {
                exercicio_id = (int)_spnExercicios.SelectedItemId,
                treino_tipo_id = _treinoTipoId,
                series = Convert.ToByte(_edtSeries.Text),
                repeticoes = Convert.ToInt16(_edtRepeticoes.Text),
                carga = Convert.ToInt16(_edtCarga.Text),
                duracao_descanso = Convert.ToInt16(_edtDescanso.Text)
            };

            Execucao_ExercicioDAO execucaoDAO = new Execucao_ExercicioDAO();
            if (execucaoDAO.InsertExecucaoExercicio(newExecucaoExercicio))
            {
                //Validation.DisplayAlertMessage("Aluno cadastrado com sucesso!", this);
                GoBack();
            }
            else
            {
                Validation.DisplayAlertMessage("Não foi possível salvar o Exercício", this);
            }
        }

        private void SPN_CategoriaExercicio_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //int categoria_id = (int)_spnCategorias.Adapter.GetItemId(e.Position);
            Categoria_Exercicio categoriaSelected = _spnCategorias.SelectedItem.Cast<Categoria_Exercicio>();
            var exercicioAdapter = new ExerciciosListAdapter(this, categoriaSelected.Exercicios);
            _spnExercicios.Adapter = exercicioAdapter;
        }

        private void LoadCategoriasAndExercicios()
        {
            CategoriaExercicioDAO categoriaDAO = new CategoriaExercicioDAO();
            var categoriasList = categoriaDAO.LoadCategoriasExercicio();
            if (categoriasList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Categorias de Exercícios", this);
                return;
            }

            //Para cada Categoria, carregar os seus exercícios correspondentes
            ExercicioDAO exercicioDAO = new ExercicioDAO();
            foreach (var categoria in categoriasList)
            {
                categoria.Exercicios = exercicioDAO.LoadExerciciosByCategoria(categoria.categoria_exercicio_id);
            }

            //Preence ListView com os Alunos do Instrutor logado
            var categoriaAdapter = new CategoriaExercicioListAdapter(this, categoriasList);
            _spnCategorias.Adapter = categoriaAdapter;
        }
    }
}