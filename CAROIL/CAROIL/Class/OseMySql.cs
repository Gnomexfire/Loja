using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MySql.Data.MySqlClient;
namespace CAROIL.Class
{
    public static class OseMySql
    {
        public static LoadConfig Config = new LoadConfig();

        #region declare

        public static string ConnectionString => "SERVER=" + Server +
                                                 ";PORT=" + Port + 
                                                 ";DATABASE=" + Database +
                                                 ";UID=" + User + 
                                                 ";PASSWORD=" + Password ;
        public static string Server, User, Password, Database, Port;
        private static MySqlConnection _connection;
        #endregion

        #region methods
        /// <summary>
        /// retorna o ultimo numero da os soma +1 gerar novo id
        /// </summary>
        /// <returns></returns>
        public static int RetornaUltimaOs()
        {
            _connection = new MySqlConnection(ConnectionString);
            int response = new int();
            try
            {
                _connection.Open();
                var command = @"select osnum from osservice top1 order by OsNum desc LIMIT 1";
                using (MySqlCommand c = new MySqlCommand(command,_connection))
                {
                    MySqlDataReader dataReader = c.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response = Convert.ToInt32(dataReader.GetString(0));
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return 0;
            }
            return response + 1;
        }
        /// <summary>
        /// retorna o ultimo IdCliente
        /// </summary>
        /// <returns></returns>
        public static int RetornaUlitmoIdCliente()
        {
            _connection = new MySqlConnection(ConnectionString);
            int response = new int();
            try
            {
                _connection.Open();
                var command = @"select idoscliente from oscliente top1 order by idoscliente desc LIMIT 1";
                using (MySqlCommand c = new MySqlCommand(command, _connection))
                {
                    MySqlDataReader dataReader = c.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response = Convert.ToInt32(dataReader.GetString(0));
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return 0;
            }
            return response + 1;
        }

        /// <summary>
        /// retorna lista de pesquisa de clientes
        /// </summary>
        /// <param name="pesquisaTipo">filtro de pesquisa</param>
        /// <param name="value">chave de pesquisa</param>
        /// <returns></returns>
        public static List<Clientes> RetornaListaClientes(OseMySql.PesquisaTipo pesquisaTipo, string value = "")
        {
            List<Clientes> response = new List<Clientes>();
            // ReSharper disable once NotAccessedVariable
            string sqlcommand = string.Empty;

            switch (pesquisaTipo)
            {
                    case PesquisaTipo.CpfCnpj:
                    sqlcommand = " SELECT clientecpfcnpj,clientenome,clientetelefone FROM osemaster.oscliente WHERE clientecpfcnpj Like Concat('%" + value + "%') ";
                    break;
                    case PesquisaTipo.Nome:
                    sqlcommand = " SELECT clientecpfcnpj,clientenome,clientetelefone FROM osemaster.oscliente WHERE clientenome Like Concat('%" + value + "%') ";
                    break;
                    case PesquisaTipo.Endereco:
                    sqlcommand = " SELECT clientecpfcnpj,clientenome,clientetelefone FROM osemaster.oscliente WHERE clienteendereco Like Concat('%" + value + "%') ";
                    break;
                    case PesquisaTipo.Completa:
                    sqlcommand = " SELECT clientecpfcnpj,clientenome,clientetelefone FROM osemaster.oscliente ";
                    break;
                    case PesquisaTipo.Telefone:
                    sqlcommand = " SELECT clientecpfcnpj,clientenome,clientetelefone FROM osemaster.oscliente WHERE clientetelefone Like Concat('%" + value + "%') ";
                    break;
            }
            sqlcommand = sqlcommand + " ORDER BY clientenome ASC";
            try
            {
                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();

                using (MySqlCommand command = new MySqlCommand(sqlcommand, _connection))
                {
                    MySqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Clientes c = new Clientes()
                        {
                            CpfCnpj = dataReader.GetString(0),
                            Nome = dataReader.GetString(1),
                            Telefone = dataReader.GetString(2)
                        };
                        response.Add(c);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
            

            return response;
        }
        /// <summary>
        /// retorna total de clientes cadastrados
        /// </summary>
        /// <returns></returns>
        public static int RetornaTotalClientes()
        {
            int response=0;
            string command = @"SELECT COUNT(*) FROM oscliente";
            try
            {
                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();
                using (MySqlCommand c = new MySqlCommand(command,_connection))
                {
                    MySqlDataReader dataReader = c.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response =Convert.ToInt32(dataReader.GetString(0));
                    }
                }
            }
            catch (Exception exc)
            {
               Console.WriteLine(exc);
            }
            return response;
        }

        /// <summary>
        /// adciona novo cliente tabela =>oscliente
        /// </summary>
        /// <param name="cpfcnpj"></param>
        /// <param name="nome"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <param name="cel"></param>
        /// <param name="endereco"></param>
        /// <param name="bairro"></param>
        /// <param name="obs"></param>
        /// <param name="datanasc"></param>
        public static int AdcionarNovoCliente(string cpfcnpj, string nome, string telefone, string email, string cel,
            string endereco, string bairro, string obs,string datanasc)
        {
            string command = "INSERT INTO oscliente VALUES (" + RetornaUlitmoIdCliente() + "," + cpfcnpj + ",'" + nome + "','" + telefone + "','" + email + "','" + cel + "','" + endereco + "','" + bairro + "','" + obs + "','" + datanasc + "')";
            _connection = new MySqlConnection(ConnectionString);
            _connection.Open();
            try
            {
                using (MySqlCommand c = new MySqlCommand(command, _connection))
                {
                    c.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                if (exc.HResult == -2147467259)
                {
                    // Chave Duplicada

                    return -1;
                }
                Console.WriteLine(exc);    
            }
            return 0;
        }
        /// <summary>
        /// deleta o cliente 
        /// </summary>
        /// <param name="cpfcnpj"></param>
        /// <returns></returns>
        public static int DeletaCliente(string cpfcnpj)
        {
            string command = "delete from oscliente where clientecpfcnpj = '" + cpfcnpj + "'";
            _connection = new MySqlConnection(ConnectionString);
            _connection.Open();

            try
            {
                using (MySqlCommand c = new MySqlCommand(command,_connection))
                {
                    c.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return -1;
            }
            return 0;
        }
        /// <summary>
        /// carrega o cliente para ser editado
        /// </summary>
        /// <param name="cpfcnpj"></param>
        /// <returns></returns>
        public static ClienteLoadEdit EditarCliente(string cpfcnpj)
        {
            ClienteLoadEdit response = new ClienteLoadEdit();
            _connection = new MySqlConnection(ConnectionString);
            _connection.Open();
            string command = "select * from oscliente where clientecpfcnpj = '" + cpfcnpj.Trim() + "'";
            try
            {
                using (MySqlCommand c = new MySqlCommand(command,_connection))
                {
                    MySqlDataReader dataReader = c.ExecuteReader();
                    while (dataReader.Read())
                    {
                        response.Id = Convert.ToInt32(dataReader.GetString(0));
                        response.CpfCnpj = dataReader.GetString(1);
                        response.Nome = dataReader.GetString(2);
                        response.Telefone = dataReader.GetString(3);
                        response.Email = dataReader.GetString(4);
                        response.Celular = dataReader.GetString(5);
                        response.Endereco = dataReader.GetString(6);
                        response.Bairro = dataReader.GetString(7);
                        response.Obs = dataReader.GetString(8);
                        response.DataNasc = dataReader.GetString(9);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);    
            }
            return response;
        }
        /// <summary>
        /// atualiza cliente
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int UpdateCliente(ClienteLoadEdit c)
        {
            _connection = new MySqlConnection(ConnectionString);
            try
            {
                _connection.Open();
                string command = "UPDATE oscliente " +
	                             " SET" +
                                 " clientecpfcnpj='" + c.CpfCnpj + "'," +
                                 " clientenome='" + c.Nome + "'," +
                                 " clientetelefone='" + c.Telefone + "'," +
                                 " clienteemail='" + c.Email + "'," +
                                 " clientecel='" + c.Celular + "'," +
                                 " clientebairro='" + c.Bairro + "'," +
                                 " clienteobs='" + c.Obs + "'," +
                                 " clienteendereco='" + c.Endereco + "'," +
                                 " clientedatanasc='" + c.DataNasc + "' WHERE " +
                                 " idoscliente ='" + c.Id + "'";
                using (MySqlCommand myc = new MySqlCommand(command, _connection))
                {
                    myc.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                return -1;
            }
            return 0;
        }
        /// <summary>
        /// retorna o id do cliente
        /// </summary>
        /// <param name="cpfcnpj"></param>
        /// <returns></returns>
        public static int ReturnIdCliente(string cpfcnpj)
        {
            _connection = new MySqlConnection(ConnectionString);
            try
            {
                _connection.Open();
                string command = "SELECT idoscliente FROM oscliente WHERE clientecpfcnpj = '" + cpfcnpj + "'";
                using (MySqlCommand c = new MySqlCommand(command, _connection))
                {
                    MySqlDataReader dataReader = c.ExecuteReader();
                    while (dataReader.Read())
                    {
                        return Convert.ToInt32(dataReader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
            return 0;
        }
        #endregion

        #region enumeration
        /// <summary>
        /// enumeration para pesquisa de cliente mysql
        /// </summary>
        public enum PesquisaTipo
        {
            CpfCnpj,
            Nome,
            Endereco,
            Telefone,
            Completa    
        }
        #endregion
    }

    /// <summary>
    /// carregar configuracao Mysql
    /// </summary>
    public class LoadConfig
    {
        public void Load()
        {
            try
            {
                var file = string.Concat(AppDomain.CurrentDomain.BaseDirectory.Replace("\\applications", string.Empty),
                                    "Configuracao.xml");

                var xml = new XmlDocument();
                xml.Load(file);
                var nodes = xml.SelectNodes("/MYSQL");
                if (nodes == null)
                {
                    return;
                }
                foreach (XmlNode index in nodes)
                {
                    var i = index.SelectSingleNode("SERVIDOR");
                    if (i != null) OseMySql.Server = i.InnerText;
                    i = index.SelectSingleNode("PORTA");
                    if (i != null) OseMySql.Port = i.InnerText;
                    i = index.SelectSingleNode("USUARIO");
                    if (i != null) OseMySql.User = i.InnerText;
                    i = index.SelectSingleNode("SENHA"); // Md5 Hash
                    if (i != null && Md5Lib.Md5Hash("osemaster") == i.InnerText)
                    {
                        OseMySql.Password = "osemaster";
                    }
                    i = index.SelectSingleNode("BANCO");
                    if (i != null) OseMySql.Database = i.InnerText;

                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }

        }
    }

    public static class Md5Lib
    {
        public static string Md5Hash(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(Encoding.ASCII.GetBytes(s));

            byte[] result = md5.Hash;

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                str.Append(result[i].ToString("x2"));
            }
            return str.ToString();
        }

        //public static string Md5Encrypt(string s)
        //{
        //    var toEncodeAsBytes = Encoding.ASCII.GetBytes(s);
        //    var returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
        //    return returnValue;
        //}
        //public static string Md5Decrypt(string s)
        //{
        //    var encodedDataAsBytes = System.Convert.FromBase64String(s);
        //    var returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
        //    return returnValue;
        //}
    }
}