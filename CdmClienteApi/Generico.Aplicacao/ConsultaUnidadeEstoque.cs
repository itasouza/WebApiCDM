using System;
using System.Collections.Generic;
using Generico.Dominio;
using Generico.Repositorio;
using Npgsql;


namespace Generico.Aplicacao
{
    public class ConsultaUnidadeEstoque
    {
        private Contexto contexto;

        public List<TB_UNIDADE> SelecionaUnidadeAtendimento(string cep)
        {
            var retorno = 0;
            var strQuery = "";
            strQuery += " select a.*from tb_unidade a ";
            strQuery += " inner join tb_faixa_atendimento b on a.idcdm = b.unidade_idcdm ";
            strQuery += string.Format(" where '{0}' between b.faixa_cep_inicial and  b.faixa_cep_final ", cep);
            strQuery += " and a.ativo = 't' LIMIT 1 ";


             //verifica se encontrou alguem
            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["idcdm"]);
                }
                reader.Close();
            }

            
            if (retorno > 0)
            {
                //se encontrou, retorna os dados da unidade e vai consultar os produtos
            }
            else
            {
                //se não encontrou, vai retornar a fábrica, não consulta produto
                strQuery = "";
                strQuery += " select  a.* from tb_unidade a  where a.atende_pedido_especial = 't' LIMIT 1 ";
            }


            using (contexto = new Contexto())
            {
                var retornoDataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return TransformaReaderEmListaObjetos(retornoDataReader);
            }

        }



        private List<TB_UNIDADE> TransformaReaderEmListaObjetos(NpgsqlDataReader reader)
        {
            var retornando = new List<TB_UNIDADE>();
            while (reader.Read())
            {

                TB_UNIDADE tabela = new TB_UNIDADE()
                {

                    idcdm = reader["idcdm"] == DBNull.Value ? 0 : Convert.ToInt32(reader["idcdm"]),
                    ativo = reader["ativo"] == DBNull.Value ? string.Empty : reader["ativo"].ToString(),
                    cnpj = reader["cnpj"] == DBNull.Value ? string.Empty : reader["cnpj"].ToString(),
                    nome = reader["nome"] == DBNull.Value ? string.Empty : reader["nome"].ToString(),
                    atende_pedido_especial = reader["atende_pedido_especial"] == DBNull.Value ? string.Empty : reader["atende_pedido_especial"].ToString(),
                    endereco = reader["endereco"] == DBNull.Value ? string.Empty : reader["endereco"].ToString(),
                    numero = reader["numero"] == DBNull.Value ? string.Empty : reader["numero"].ToString(),
                    cidade = reader["cidade"] == DBNull.Value ? string.Empty : reader["cidade"].ToString(),
                    cep = reader["cep"] == DBNull.Value ? string.Empty : reader["cep"].ToString(),
                    contato = reader["contato"] == DBNull.Value ? string.Empty : reader["contato"].ToString(),
                    telefone = reader["telefone"] == DBNull.Value ? string.Empty : reader["telefone"].ToString(),
                    email = reader["email"] == DBNull.Value ? string.Empty : reader["email"].ToString()

                };

                retornando.Add(tabela);
            }
            reader.Close();
            return retornando;
        }



    }
}
