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

namespace TCCBruno
{
    [Activity(Label = "Cadastro de Exercícios")]
    public class CadastroExercicioActivity : ActivityBase
    {
        private int _treinoTipoId;
        private Spinner _spnCategorias;
        private Spinner _spnExercicios;
        private EditText _edtSeries;
        private EditText _edtRepeticoes;

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
            SetContentView(Resource.Layout.CadastroExerciciosPage);

            _edtSeries = FindViewById<EditText>(Resource.Id.EDT_Series);
            _edtRepeticoes = FindViewById<EditText>(Resource.Id.EDT_Repeticoes);
            _spnCategorias = FindViewById<Spinner>(Resource.Id.SPN_CategoriaExercicio);
            _spnExercicios = FindViewById<Spinner>(Resource.Id.SPN_Exercicios);


            //Eventos:
            _spnCategorias.ItemSelected += SPN_CategoriaExercicio_ItemSelected;

            _treinoTipoId = Nav.GetAndRemoveParameter<int>(Intent);
            LoadCategoriasAndExercicios();
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