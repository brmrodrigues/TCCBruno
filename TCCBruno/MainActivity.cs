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

namespace TCCBruno
{
    [Activity(Label = "Personal Academia", MainLauncher = true, Icon = "@drawable/logoAcademia")]
    public class MainActivity : ActivityBase
    {
        private int _usuarioId; //Id do usuário logado no sistema
        //Navegação de Pages:
        public const string Page2Key = "Page2";
        public const string _cadastroAlunoPageKey = "CadastroAlunoPage";
        public const string _meusAlunosPageKey = "MeusAlunosPage";
        public const string _treinosPageKey = "TreinosPage";
        private static bool _initialized; //flag utilizada na inicialização do ServiceLocator

        private LinearLayout _linearLayoutLoginPage;
        public static SqlConnection _connection;
        public static string connectionString =
            "Data Source=tcp:tccbruno.database.windows.net,1433;" +
            "Initial Catalog=Tcc;User ID=tccbruno@tccbruno;Password=dmXrsYc2";
        public static MobileServiceClient mobileService =
                   new MobileServiceClient("https://appquickstart.azurewebsites.net");

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            FindViewById<Button>(Resource.Id.BTN_Entrar).Click += ButtonClick_Entrar;

            //Inicialização do ServiceLocator para registrar as Views que serão utilizadas neste Projeto
            if (!_initialized)
            {
                _initialized = true;
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                var nav = new NavigationService();
                //Registro das Views com suas respectivas ViewModels
                nav.Configure(Page2Key, typeof(CadastrarAvFisicaActivity));
                nav.Configure(_cadastroAlunoPageKey, typeof(CadastroAlunoActivity));
                nav.Configure(_meusAlunosPageKey, typeof(MeusAlunosActivity));
                nav.Configure(_treinosPageKey, typeof(TreinoActivity));

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
            int resultLogin = LoginExecute(usuario, senha);
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
                    //...
                    //nav.NavigateTo(_cadastroAlunoPageKey, _usuarioId); //Instrutor
                    nav.NavigateTo(_meusAlunosPageKey, _usuarioId); //Instrutor
                    break;
            }

            if (usuario.Equals("page2"))
            {
                var nav = ServiceLocator.Current.GetInstance<INavigationService>();
                nav.NavigateTo(Page2Key);
            }
            else if (usuario.Equals("aluno"))
            {
                var nav = ServiceLocator.Current.GetInstance<INavigationService>();
                nav.NavigateTo(_cadastroAlunoPageKey);
            }

            //Console.WriteLine("Button Entrar clicado");
        }

        /// <summary>
        /// Executa o Login, faz consulta no BD para validar o login/senha informados
        /// </summary>
        private int LoginExecute(string usuario, string senha)
        {
            int result;

            using (_connection = new SqlConnection(connectionString))
            {
                try
                {
                    _connection.Open();
                    SqlCommand command = new SqlCommand("LoginAutenticacao", _connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@usuario", usuario);
                    command.Parameters.AddWithValue("@senha", senha);
                    //Configuração do parâmetro de retorno da StoredProcedure
                    SqlParameter retParameter = new SqlParameter("ret", SqlDbType.Int);
                    retParameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(retParameter);
                    command.ExecuteNonQuery();

                    result = Convert.ToInt32(command.Parameters["ret"].Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    result = 0;
                }
                finally
                {
                    _connection.Close();
                }
            }
            return result;
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
    }
}


