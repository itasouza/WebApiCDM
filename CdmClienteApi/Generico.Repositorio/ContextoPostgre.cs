using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generico.Repositorio
{
    public class ContextoPostgre
    {

        private readonly NpgsqlConnection minhaConexao;

        public ContextoPostgre()
        {
            minhaConexao = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConexaoPostgre"].ConnectionString);
            minhaConexao.Open();
        }

        public void ExecutaComando(string strQuery)
        {
            var cmdComando = new NpgsqlCommand
            {
                CommandText = strQuery,
                CommandType = CommandType.Text,
                Connection = minhaConexao
            };

            cmdComando.ExecuteNonQuery();
        }

        public NpgsqlDataReader ExecutaComandoComRetorno(string strQuery)
        {
            var cmdComando = new NpgsqlCommand(strQuery, minhaConexao);
            return cmdComando.ExecuteReader();
        }

        public void Dispose()
        {
            if (minhaConexao.State == ConnectionState.Open)
                minhaConexao.Close();

        }
    }
}