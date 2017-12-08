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

        public List<TB_UNIDADE> SelecionaUnidadeAtendimento(TB_DADOS_API dados)
        {
            bool TemProdutoParaAtendimento = false;
            var retorno = 0;
            var strQuery = "";
            strQuery += " select a.*from tb_unidade a ";
            strQuery += " inner join tb_faixa_atendimento b on a.idcdm = b.unidade_idcdm ";
            strQuery += string.Format(" where '{0}' between b.faixa_cep_inicial and  b.faixa_cep_final ", dados.Cep);
            strQuery += " and a.ativo = 't'  ";
            strQuery += " and a.atende_pedido_especial = 'f' LIMIT 1 ";

             //verifica se encontrou alguem
            using (contexto = new Contexto())
            {
                //encontra um cdm para atendimento
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["idcdm"]);
                }
                reader.Close();
            }

            //se encontrou, então vai ser maior que zero, vai consultar o estoque
            if (retorno > 0)
            {
               

                foreach (var item in dados.Itens)
                {
                    var retornoProduto = 0;
                    var strQueryProduto = "";
                    strQueryProduto += " select a.* , b.* ";
                    strQueryProduto += " from tb_estoque_cdm a ";
                    strQueryProduto += " inner join produto b on a.produto_id = b.id ";
                    strQueryProduto += string.Format(" where a.qtdestoquefisico >= '{0}' ", item.Qtd);
                    strQueryProduto += string.Format(" and a.unidade_idcdm = '{0}' ", retorno);
                    strQueryProduto += string.Format(" and b.codigo = '{0}'  ", item.Codigo);

                    using (contexto = new Contexto())
                    {
                        //verifica cada produto
                        var reader = contexto.ExecutaComandoComRetorno(strQueryProduto);
                        while (reader.Read())
                        {
                            retornoProduto = Convert.ToInt32(reader["idestoque"]);
                        }
                        reader.Close();
                    }

                    //valida se tem estoque
                    if(retornoProduto > 0)
                    {
                        TemProdutoParaAtendimento = true;
                    }else
                    {
                        TemProdutoParaAtendimento = false;
                        //se não encontrou, vai retornar a fábrica, não consulta produto
                        strQuery = "";
                        strQuery += " select  a.* from tb_unidade a  where a.atende_pedido_especial = 't' LIMIT 1 ";
                        break;
                    }

                }
 
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


        public void CancelamentoPedido(string NumeroPedido)
        {
            var strQuery = string.Format("update pedidos set status = 'CANCELADO' where numero_pedido = '{0}' ", NumeroPedido);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }


        public void AbrePedido(string NumeroPedido)
        {
            var strQuery = string.Format("update pedidos set status = 'ABERTO' where numero_pedido = '{0}' ", NumeroPedido);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
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
