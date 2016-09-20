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

namespace TCCBruno
{
    [Activity(Label = "Cadastro de Exercícios")]
    public class CadastroExercicioActivity : ActivityBase
    {
        int _treinoId;
        Spinner _spinnerCategorias;


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


            _spinnerCategorias = FindViewById<Spinner>(Resource.Id.SPN_CategoriaExercicio);
            //Spinner SPN_SubTreino = FindViewById<Spinner>(Resource.Id.SPN_SubTreino);
            //var spinnerAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.subTreino_array,
            //                                                    Android.Resource.Layout.SimpleSpinnerItem);
            //spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);
            //SPN_SubTreino.Adapter = spinnerAdapter;
            //_listViewCategorias = FindViewById<ListView>(Resource.Id.LV_CategoriaExercicio);

            _treinoId = Nav.GetAndRemoveParameter<int>(Intent);
            LoadCategoriasExercicios();
        }


        private void LoadCategoriasExercicios()
        {
            CategoriaExercicioDAO categoriaDAO = new CategoriaExercicioDAO();
            var categoriasList = categoriaDAO.LoadCategoriasExercicio();
            if (categoriasList == null)
            {
                Validation.DisplayAlertMessage("Falha ao carregar Categorias de Exercícios", this);
                return;
            }
            //Preence ListView com os Alunos do Instrutor logado
            var categoriaAdapter = new CategoriaExercicioListAdapter(this, categoriasList);
            //_listViewCategorias.Adapter = listAdapter;
            _spinnerCategorias.Adapter = categoriaAdapter;
        }
    }
}