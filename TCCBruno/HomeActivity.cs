using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Views;
using TCCBruno.Adapters;
using Microsoft.Practices.ServiceLocation;
using TCCBruno.DAO;

namespace TCCBruno
{
    [Activity(Label = "Personal Academia", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class HomeActivity : ActionBarActivity
    {
        private SupportToolbar mToolbar;
        private MyActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private LeftMenuListAdapter mLeftAdapter;
        Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();
        private SupportFragment _currentFragment;
        private MeusAlunosActivity _meusAlunosFragment;
        private CheckInActivity _checkInFragment;
        private LogOutFragment _logOutFragment;
        private Stack<SupportFragment> _stackFragment;

        private string[] _instrutorMenuItems =
        {
            "Nome",
            "Meus Alunos",
            "Sair"
        };

        private int[] _instrutorMenuImageIds =
        {
            Resource.Drawable.ic_user,
            Resource.Drawable.ic_meusAlunos,
            Resource.Drawable.ic_logOut
        };

        private string[] _alunoMenuItems =
        {
            "Nome",
            "Treinos",
            "Check-in",
            "Sair"
        };

        private int[] _alunoMenuImageIds =
        {
            Resource.Drawable.ic_user,
            Resource.Drawable.ic_treinos,
            Resource.Drawable.ic_checkIn,
            Resource.Drawable.ic_logOut
        };

        public NavigationService Nav
        {
            get
            {
                return (NavigationService)ServiceLocator.Current
                    .GetInstance<INavigationService>();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.HomePage);

            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            //mRightDrawer = FindViewById<ListView>(Resource.Id.right_drawer);

            mLeftDrawer.Tag = 0;
            //mRightDrawer.Tag = 1;

            mLeftDrawer.ItemClick += LeftDrawer_ItemClick;

            SetSupportActionBar(mToolbar);

            //Retira o id do usuario logado
            _instrutorAlunoDict = Nav.GetAndRemoveParameter<Dictionary<string, int>>(Intent);


            //mLeftAdapter = _instrutorAlunoDict.ContainsKey("aluno_id") ? new LeftMenuListAdapter(this, _alunoMenuItems) :
            //                                                                new LeftMenuListAdapter(this, _instrutorMenuItems);

            _stackFragment = new Stack<SupportFragment>();

            if (_instrutorAlunoDict.ContainsKey("aluno_id"))
            {
                _alunoMenuItems[0] = GetNomeAluno(_instrutorAlunoDict["aluno_id"]);
                mLeftAdapter = new LeftMenuListAdapter(this, _alunoMenuItems, _alunoMenuImageIds);

                _logOutFragment = new LogOutFragment();
                _checkInFragment = new CheckInActivity(_instrutorAlunoDict["aluno_id"]);

                //Uso de FragmentLayout dentro do FragContainer
                var transaction = SupportFragmentManager.BeginTransaction();
                //O último Fragment adicionado será o primeiro a ser exibido (Stack)
                transaction.Add(Resource.Id.fragmentContainer, _logOutFragment, "LogOutFragment"); //Container, Content Activity, Tag
                transaction.Hide(_logOutFragment);
                transaction.Add(Resource.Id.fragmentContainer, _checkInFragment, "CheckInFragment"); //Container, Content Activity, Tag
                //transaction.Hide(_checkInFragment);

                transaction.Commit();
                _currentFragment = _checkInFragment;

            }

            else
            {
                _instrutorMenuItems[0] = GetNomeInstrutor(_instrutorAlunoDict["instrutor_id"]);
                mLeftAdapter = new LeftMenuListAdapter(this, _instrutorMenuItems, _instrutorMenuImageIds);

                _meusAlunosFragment = new MeusAlunosActivity(_instrutorAlunoDict["instrutor_id"]);
                _logOutFragment = new LogOutFragment();

                //Uso de FragmentLayout dentro do FragContainer
                var transaction = SupportFragmentManager.BeginTransaction();
                //O último Fragment adicionado será o primeiro a ser exibido (Stack)
                transaction.Add(Resource.Id.fragmentContainer, _logOutFragment, "LogOutFragment"); //Container, Content Activity, Tag
                transaction.Hide(_logOutFragment);
                transaction.Add(Resource.Id.fragmentContainer, _meusAlunosFragment, "MeusAlunosFragment"); //Container, Content Activity, Tag
                transaction.Commit();
                _currentFragment = _meusAlunosFragment;
            }

            mLeftDrawer.Adapter = mLeftAdapter;


            mDrawerToggle = new MyActionBarDrawerToggle(
                this,                           //Host Activity
                mDrawerLayout,                  //DrawerLayout
                Resource.String.openDrawer,     //Opened Message
                Resource.String.closeDrawer     //Closed Message
            );

            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();

            if (bundle != null)
            {
                if (bundle.GetString("DrawerState") == "Opened")
                {
                    SupportActionBar.SetTitle(Resource.String.openDrawer);
                }

                else
                {
                    SupportActionBar.SetTitle(Resource.String.closeDrawer);
                }
            }

            else
            {
                //This is the first the time the activity is ran
                SupportActionBar.SetTitle(Resource.String.closeDrawer);
            }

        }

        private string GetNomeAluno(int alunoId)
        {
            AlunoDAO alunoDAO = new AlunoDAO();

            return alunoDAO.GetNomeAluno(alunoId);
        }

        private string GetNomeInstrutor(int instrutorId)
        {
            InstrutorDAO instrutorDAO = new InstrutorDAO();

            return instrutorDAO.GetNomeInstrutor(instrutorId);
        }

        private void LeftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (_instrutorAlunoDict.ContainsKey("aluno_id"))
                switch (e.Position)
                {
                    case 0:
                        break;
                    case 1:
                        mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
                        Nav.NavigateTo(LoginActivity._treinosPageKey, _instrutorAlunoDict);
                        break;
                    case 2:
                        ShowFragment(_checkInFragment);
                        mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
                        //Nav.NavigateTo(LoginActivity._checkInPageKey, _instrutorAlunoDict["aluno_id"]);
                        break;
                    case 3:
                        ShowFragment(_logOutFragment);
                        mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
                        break;
                    default:
                        break;
                }
            else
            {
                switch (e.Position)
                {
                    case 0:
                        break;
                    case 1:
                        ShowFragment(_meusAlunosFragment);
                        mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
                        break;
                    case 2:
                        ShowFragment(_logOutFragment);
                        mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
                        break;
                    default:
                        break;
                }
            }

        }

        private void ShowFragment(SupportFragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();

            trans.Hide(_currentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null); // Allow to get back to the previous fragment on GoBack()
            trans.Commit();

            _stackFragment.Push(_currentFragment);
            _currentFragment = fragment;
        }

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportFragmentManager.PopBackStack();
                _currentFragment = _stackFragment.Pop();
            }
            else
            {
                base.OnBackPressed();

            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {

                case Android.Resource.Id.Home:
                    //The hamburger icon was clicked which means the drawer toggle will handle the event
                    //all we need to do is ensure the right drawer is closed so the don't overlap
                    //mDrawerLayout.CloseDrawer(mRightDrawer);
                    mDrawerToggle.OnOptionsItemSelected(item);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
            {
                outState.PutString("DrawerState", "Opened");
            }

            else
            {
                outState.PutString("DrawerState", "Closed");
            }

            base.OnSaveInstanceState(outState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }

    }
}


