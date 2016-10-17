using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Data;
using System.Data.SqlClient;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using TCCBruno.Model;

namespace TCCBruno
{
    [Activity(Label = "Personal Academia", MainLauncher = true, Icon = "@drawable/logoAcademia")]
    public class LoginActivity : ActivityBase
    {
        private int _usuarioId; //Id do usuário logado no sistema
        //Navegação de Pages:
        public const string _cadastroAvFisicaPageKey = "CadastroAvFisicaPage";
        public const string _cadastroAlunoPageKey = "CadastroAlunoPage";
        public const string _meusAlunosPageKey = "MeusAlunosPage";
        public const string _treinosPageKey = "TreinosPage";
        public const string _cadastroTreinoPageKey = "CadastroTreinoPage";
        public const string _execucaoExerciciosPageKey = "ExerciciosExecucaoPage";
        public const string _cadastroExecucao_ExercicioPageKey = "CadastroExecucao_ExercicioPage";
        public const string _cadastroTreinoTipoPageKey = "CadastroTreinoTipoPage";
        public const string _checkInPageKey = "CheckInPage";
        public const string _homePageKey = "HomePage";
        public const string _loginPageKey = "LoginPage";
        public const string _estatisticasFaltaPageKey = "EstatisticasFaltaPage";

        private static bool _initialized; //flag utilizada na inicialização do ServiceLocator

        private LinearLayout _linearLayoutLoginPage;
        public static SqlConnection _connection;
        public static string connectionString =
            "Data Source=tcp:tccbruno.database.windows.net,1433;" +
            "Initial Catalog=Tcc;User ID=tccbruno@tccbruno;Password=dmXrsYc2";
        //public static MobileServiceClient mobileService =
        //           new MobileServiceClient("https://appquickstart.azurewebsites.net");

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LoginPage);
            FindViewById<Button>(Resource.Id.BTN_Entrar).Click += ButtonClick_Entrar;

            //Inicialização do ServiceLocator para registrar as Views que serão utilizadas neste Projeto
            if (!_initialized)
            {
                _initialized = true;
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                var nav = new NavigationService();
                //Registro das Views com suas respectivas ViewModels
                nav.Configure(_cadastroAvFisicaPageKey, typeof(CadastroAvFisicaActivity));
                nav.Configure(_cadastroAlunoPageKey, typeof(CadastroAlunoActivity));
                nav.Configure(_meusAlunosPageKey, typeof(MeusAlunosActivity));
                nav.Configure(_treinosPageKey, typeof(TreinosActivity));
                nav.Configure(_cadastroTreinoPageKey, typeof(CadastroTreinoActivity));
                nav.Configure(_cadastroExecucao_ExercicioPageKey, typeof(CadastroExecucao_ExercicioActivity));
                nav.Configure(_execucaoExerciciosPageKey, typeof(Execucao_ExerciciosActivity));
                nav.Configure(_cadastroTreinoTipoPageKey, typeof(CadastroTreinoTipoActivity));
                nav.Configure(_checkInPageKey, typeof(CheckInActivity));
                nav.Configure(_homePageKey, typeof(HomeActivity));
                nav.Configure(_loginPageKey, typeof(LoginActivity));
                nav.Configure(_estatisticasFaltaPageKey, typeof(EstatisticasFaltaActivity));


                SimpleIoc.Default.Register<INavigationService>(() => nav);
            }

            //A página de Login é um LinearLayoutPage, adquirimos sua instância para realizar
            //a validação dos campos 'usuário' e 'senha' no evento 'ButtonClick_Entrar()'
            _linearLayoutLoginPage = (LinearLayout)FindViewById(Resource.Id.linearLayoutloginPage);

            //LoadAlunos();
        }

        private void ButtonClick_Entrar(object sender, EventArgs e)
        {

            ValidateFields();
            var usuario = FindViewById<EditText>(Resource.Id.editText_Email).Text;
            var senha = FindViewById<EditText>(Resource.Id.ediText_Senha).Text;
            bool tipoPessoa = false;
            int resultLogin = LoginExecute(usuario, senha, ref tipoPessoa);
            switch (resultLogin)
            {
                case 0:
                    DisplayAlertMessage("O sistema está indisponível no momento");
                    break;
                case -1:
                    DisplayAlertMessage("Usuário não cadastrado no sistema");
                    break;
                case -2:
                    DisplayAlertMessage("Senha incorreta");
                    break;
                default:
                    DisplayAlertMessage("Login efetuado com sucesso!");
                    _usuarioId = resultLogin;
                    var nav = ServiceLocator.Current.GetInstance<INavigationService>();
                    //TODO: Fazer verificação se usuário logado é Aluno ou Instrutor: 
                    //direcioná -lo para sua tela correspondente.


                    //if (!tipoPessoa) //Usuário é um ALUNO
                    //{
                    //    Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();
                    //    _instrutorAlunoDict.Add("aluno_id", _usuarioId);
                    //    //nav.NavigateTo(_treinosPageKey, _instrutorAlunoDict);

                    //    //Apenas para testar GPS:
                    //    //nav.NavigateTo(_checkInPageKey, _usuarioId);
                    //    nav.NavigateTo(_homePageKey, _instrutorAlunoDict);
                    //}
                    //else //Usuário é um INSTRUTOR
                    //{
                    //    nav.NavigateTo(_meusAlunosPageKey, _usuarioId);
                    //}

                    Dictionary<string, int> _instrutorAlunoDict = new Dictionary<string, int>();

                    if (!tipoPessoa)
                    {
                        _instrutorAlunoDict.Add("aluno_id", _usuarioId);
                    }
                    else
                    {
                        _instrutorAlunoDict.Add("instrutor_id", _usuarioId);
                    }

                    nav.NavigateTo(_homePageKey, _instrutorAlunoDict);

                    //nav.NavigateTo(_cadastroAlunoPageKey, _usuarioId); //Instrutor
                    //nav.NavigateTo(_meusAlunosPageKey, _usuarioId); //Instrutor
                    break;
            }

            if (usuario.Equals("av"))
            {
                var nav = ServiceLocator.Current.GetInstance<INavigationService>();
                Aluno aluno = new Aluno
                {
                    aluno_id = 6,
                    data_nascimento = "12/03/1992"
                };
                nav.NavigateTo(_cadastroAvFisicaPageKey, aluno); //Para testes
            }
            else if (usuario.Equals("aluno"))
            {
                var nav = ServiceLocator.Current.GetInstance<INavigationService>();
                nav.NavigateTo(_cadastroAlunoPageKey);
            }
        }

        private int LoginExecute(string usuario, string senha, ref bool tipoPessoa)
        {
            int resultId;

            using (_connection = new SqlConnection(connectionString))
            {
                try
                {
                    _connection.Open();
                    SqlCommand command = new SqlCommand("LoginAutenticacaoNew", _connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@usuario", usuario);
                    command.Parameters.AddWithValue("@senha", senha);
                    //Configuração do parâmetro de retorno da StoredProcedure
                    //SqlParameter retParameter = new SqlParameter("ret", SqlDbType.Int);
                    //retParameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@tipoPessoa", SqlDbType.Bit).Direction = ParameterDirection.Output; //tipoPessoa => 0 - Aluno, 1 - Instrutor
                    command.ExecuteNonQuery();

                    resultId = Convert.ToInt32(command.Parameters["@id"].Value);
                    tipoPessoa = Convert.ToBoolean(command.Parameters["@tipoPessoa"].Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    resultId = 0;
                }
                finally
                {
                    _connection.Close();
                }
            }
            return resultId;
        }

        /// <summary>
        /// Vericia se os campos Login e Senha estão vazios
        /// </summary>
        private void ValidateFields()
        {
            for (int i = 0; i < _linearLayoutLoginPage.ChildCount; i++)
            {
                if (_linearLayoutLoginPage.GetChildAt(i) is EditText)
                {
                    if (string.IsNullOrWhiteSpace(((EditText)_linearLayoutLoginPage.GetChildAt(i)).Text))
                    {
                        DisplayAlertMessage("Preencha os campos Login e Senha");

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Pop up de um alerta ao usuário
        /// </summary>
        /// <param name="message"></param>
        private void DisplayAlertMessage(string message)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetMessage(message);
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public static void LoadAlunos()
        {
            using (_connection = new SqlConnection(connectionString))
            {
                string queryString = "SELECT [nome_aluno], [usuario], [senha] FROM [ALUNO]";
                SqlCommand sqlCommand = new SqlCommand(queryString, _connection);
                //sqlCommand.Parameters.AddWithValue("@parameter", paramValue);
                try
                {
                    _connection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}",
                        sqlDataReader[0], sqlDataReader[1], sqlDataReader[2]);
                    }
                    sqlDataReader.Close();

                    //Usando DataTable
                    //DataTable dataTable = new DataTable();
                    //SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                    //dataAdapter.Fill(dataTable);
                    //foreach (DataRow row in dataTable.Rows)
                    //{
                    //    foreach (var item in row.ItemArray)
                    //    {
                    //        Console.WriteLine(item);
                    //    }
                    //}
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected override void OnResume()
        {
            //Limpar a Activity anterior
            //this.Intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);

            base.OnResume();
        }
    }
}


