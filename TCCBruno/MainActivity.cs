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

namespace TCCBruno
{
    [Activity(Label = "Personal Academia", MainLauncher = true, Icon = "@drawable/logoAcademia")]
    public class MainActivity : Activity
    {
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

            _linearLayoutLoginPage = (LinearLayout)FindViewById(Resource.Id.linearLayoutloginPage);

            //LoadAlunos();
        }

        [Java.Interop.Export("ButtonClick_Entrar")]
        public void ButtonClick_Entrar(View v)
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
                case 1:
                    DisplayAlertMessage("Login efetuado com sucesso!");
                    break;
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


