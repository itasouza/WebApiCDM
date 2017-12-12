using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;
using static Generico.Dominio.TB_PEDIDO;
using Generico.Repositorio;

namespace Generico.Aplicacao
{
    public class GravaPedidoAplicacao
    {

        private Contexto contexto;



        public string ConsultaUsuario(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    username = "sistemacdm",
                    password = "qZrm4Rqk"
                });

                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string json = streamReader.ReadToEnd();

                usuario m = JsonConvert.DeserializeObject<usuario>(json);
                string token = m.answer.token;
                return token;
            }

        }




        public string ConsultaPedido(string urlpedido, string NumeroPedido, string token)
        {

            var request = (HttpWebRequest)WebRequest.Create(urlpedido + "/" + NumeroPedido + "/");
            request.Method = "GET";

            request.Headers["AuthToken"] = token;//Adicionando o AuthToken  no Header da requisição

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string json = streamReader.ReadToEnd();

                Pedido p = JsonConvert.DeserializeObject<Pedido>(json);
                string pedido = p.answer.numeroPedido;

               string CodigoCadastro =  GravaCadastro(p.answer.cliente.cadastro);
               string CodigoCliente  =  GravaCliente(p.answer.cliente, CodigoCadastro);
             //  string CodigoCabecahoPedido = GravaCabecalhoPedido(p, CodigoCliente);

                return json;
            }
        }





        public string ConsultaPais(string codigo)
        {
            int retorno = 0;
            var strQuery = "";

            //apensas vamos retornar o pais, não vamos gravar ou atualizar
            strQuery = string.Format("select * from pais WHERE codigo = '{0}' ", codigo);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id da cidade
                    retorno = Convert.ToInt32(reader["codigo"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }

        public string ConsultaEstado(string sigla)
        {
            string retorno = "";
            var strQuery = "";

            //apensas vamos retornar o estado, não vamos gravar ou atualizar
            strQuery = string.Format("select * from estado WHERE sigla = '{0}' ", sigla);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id da cidade
                    retorno = reader["sigla"].ToString();
                }
                reader.Close();
            }

            return retorno.ToString();
        }

        public string ConsultaCidade(string codigo)
        {
            int retorno = 0;
            var strQuery = "";

            //apensas vamos retornar a cidade, não vamos gravar ou atualizar
            strQuery = string.Format("select * from cidade WHERE codigo = '{0}' ", codigo);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id da cidade
                    retorno = Convert.ToInt32(reader["codigo"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }

        public string ConsultaCadastro(string codigo)
        {
            int retorno = 0;
            var strQuery = "";

            //verificar se o cadastro existe
            strQuery = string.Format("select * from cadastro WHERE id_cadastro = '{0}' ", codigo);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id 
                    retorno = Convert.ToInt32(reader["id_cadastro"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }

        public string ConsultaPatrocinador(string codigo)
        {
            int retorno = 0;
            var strQuery = "";

            //verificar se o cadastro existe
            strQuery = string.Format("select * from cadastro WHERE login = '{0}' ", codigo);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id 
                    retorno = Convert.ToInt32(reader["id_patrocinador"]);
                }
                reader.Close();
            }

            return retorno.ToString();

        }

        public string ConsultaProduto(string codigo)
        {
            int retorno = 0;
            var strQuery = "";

            //verificar se o cadastro existe
            strQuery = string.Format("select * from produto WHERE codigo = '{0}' ", codigo);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id 
                    retorno = Convert.ToInt32(reader["id"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }

      

        public string GravaCadastro(Cadastro cadastro)
        {
            int retorno = 0;
            var strQuery = new StringBuilder();
            string cidade = ConsultaCidade(cadastro.cidade.codigo);
            string patrocinador = ConsultaPatrocinador(cadastro.patrocinador);

            //verificar se o cadastro existe
            strQuery.Append("select * from cadastro WHERE id_cadastro = ").Append(cadastro.idCadastro);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery.ToString());
                while (reader.Read())
                {
                    //pega o Id do cliente
                    retorno = Convert.ToInt32(reader["id_cadastro"]);
                }
                reader.Close();
            }

            if (retorno == 0)
            {
                //inserir um novo cadastro
                strQuery.Clear();
                strQuery.Append("INSERT INTO cadastro (id_patrocinador,confirmado,primeiro_nome,sobrenome,")
                        .Append("email,login,senha,dt_nascimento,tipo_documento,num_documento, genero,ddd,telefone,endereco,complemento,numero,bairro,cep,")
                        .Append("cidade, tipo_pessoa,dt_cadastro,cnae,ie,im,docs_verificados,apto_para_saque, docs_submetidos) ")
                        .Append("VALUES (")
                        .Append(cadastro.patrocinador).Append(",")
                        .Append(cadastro.confirmado).Append(",")
                        .Append("'").Append(cadastro.primeiroNome).Append("',")
                        .Append("'").Append(cadastro.sobrenome).Append("',")
                        .Append("'").Append(cadastro.email).Append("',")
                        .Append("'").Append(cadastro.login).Append("',")
                        .Append("'123',")
                        .Append("'").Append(cadastro.dtNascimento.ToString("yyyy-dd-mm hh:mm:ss")).Append("',")
                        .Append("'").Append(cadastro.tipoDocumento).Append("',")
                        .Append("'").Append(cadastro.numDocumento).Append("',")
                        .Append("'").Append(cadastro.genero).Append("',")
                        .Append("'").Append(cadastro.ddd).Append("',")
                        .Append("'").Append(cadastro.telefone).Append("',")
                        .Append("'").Append(cadastro.endereco).Append("',")
                        .Append("'").Append(cadastro.complemento).Append("',")
                        .Append("'").Append(cadastro.numero).Append("',")
                        .Append("'").Append(cadastro.bairro).Append("',")
                        .Append("'").Append(cadastro.cep).Append("',")
                        .Append("'").Append(cidade).Append("',") //cidade   //preciso gravar a cidade antes
                        .Append("'").Append(cadastro.tipoPessoa).Append("',") //tipo de pessoa
                        .Append("'").Append(DateTime.Parse(cadastro.dtCadastro.Replace("-", "/")).ToString("yyyy-dd-mm hh:mm:ss")).Append("',")
                        .Append("'").Append(cadastro.cnae).Append("',")
                        .Append("'").Append(cadastro.ie).Append("',")
                        .Append("'").Append(cadastro.im).Append("',")
                        .Append("true,")
                        .Append("true,")
                        .Append("true)")
                        .Append("; select * from cadastro order by id_cadastro desc LIMIT 1  ");

            }
            else
            {
                //OK
                strQuery.Clear();
                strQuery.Append("UPDATE cadastro SET ")
                        .Append("id_patrocinador = '").Append(patrocinador).Append("',")
                        .Append("confirmado = ").Append(cadastro.confirmado).Append(",")
                        .Append("primeiro_nome = '").Append(cadastro.primeiroNome).Append("',")
                        .Append("sobrenome = '").Append(cadastro.sobrenome).Append("',")
                        .Append("email = '").Append(cadastro.email).Append("',")
                        .Append("login = '").Append(cadastro.login).Append("',")
                        .Append("dt_nascimento = '").Append(cadastro.dtNascimento).Append("',")
                        // .Append("tipo_documento = '").Append(cadastro.tipoDocumento).Append("',") //nao deixou atualizar o cpf 
                        // .Append("num_documento = '").Append(cadastro.numDocumento).Append("',") //nao deixou atualizar o cpf 
                        .Append("genero = '").Append(cadastro.genero).Append("',")
                        .Append("ddd = '").Append(cadastro.ddd).Append("',")
                        .Append("telefone = '").Append(cadastro.telefone).Append("',")
                        .Append("endereco = '").Append(cadastro.endereco).Append("',")
                        .Append("complemento = '").Append(cadastro.complemento).Append("',")
                        .Append("numero = '").Append(cadastro.numero).Append("',")
                        .Append("bairro = '").Append(cadastro.bairro).Append("',")
                        .Append("cep = '").Append(cadastro.cep).Append("',")
                        .Append("cidade = '").Append(cidade).Append("',") //cidade   //preciso gravar a cidade antes 
                        .Append("tipo_pessoa = '").Append(cadastro.tipoPessoa).Append("',")//tipo de pessoa                                        
                        .Append("agencia = '").Append("").Append("',") //agencia                
                        .Append("cnae = '").Append(cadastro.cnae).Append("',")
                        .Append("ie = '").Append(cadastro.ie).Append("',")
                        .Append("im = '").Append(cadastro.im).Append("'")
                        .Append(" WHERE id_cadastro = ").Append(cadastro.idCadastro);

            }

            //retornar o ID do cadastro
            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery.ToString());
                while (reader.Read())
                {
                    //retornar o ID do cliente inserido
                    retorno = Convert.ToInt32(reader["id_cadastro"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }


        public string GravaCliente(ClientePedido cliente,string CodigoCadastro)
        {
            int retorno = 0;
            var strQuery = new StringBuilder();
            string cadastro = CodigoCadastro;

            //verificar se o cliente existe
            strQuery.Append("select * from cliente WHERE id_cliente = ").Append(cliente.idCliente);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery.ToString());
                while (reader.Read())
                {
                    //pega o Id do cliente
                    retorno = Convert.ToInt32(reader["id_cliente"]);
                }
                reader.Close();
            }

            if (retorno == 0)
            {
                //inserir um novo cliente
                strQuery.Clear();
                strQuery.Append("INSERT INTO cliente (id_cadastro,nome_cliente,tipo_documento,num_documento,")
                        .Append("email,dt_nascimento,genero,ddd,telefone,endereco, numero,bairro,cep,cidade,tipo_endereco_entrega,orientacao_entregador, estado)")
                        .Append(" VALUES (")
                        .Append("'").Append(cliente.nomeCliente).Append("',")
                        .Append("'").Append(cliente.tipoDocumento).Append("',")
                        .Append("'").Append(cliente.numDocumento).Append("',")
                        .Append("'").Append(cliente.email).Append("',")
                        .Append("'").Append(DateTime.Parse(cliente.dtNascimento.Replace("-", "/")).ToString("yyyy-MM-dd")).Append("',")
                        .Append("'").Append(cliente.genero).Append("',")
                        .Append("'").Append(cliente.ddd).Append("',")
                        .Append("'").Append(cliente.telefone).Append("',")
                        .Append("'").Append(cliente.endereco).Append("',")
                        .Append("'").Append(cliente.numero).Append("',")
                        .Append("'").Append(cliente.bairro).Append("',")
                        .Append("'").Append(cliente.cep).Append("',")
                        .Append("'").Append(cliente.cidade.codigo).Append("',") //precisa gravar a cidade antes
                        .Append("'").Append(cliente.tipoEnderecoEntrega).Append("',")
                        .Append("'").Append(cliente.orientacaoEntregador).Append("',")
                        .Append("'").Append(cliente.cidade.estado).Append("'")
                        .Append(" ) ")
                        .Append("; select  * from cliente order by id_cliente desc LIMIT 1  ");
            }
            else
            {
                //atualiza o cliente
                strQuery.Clear();
                strQuery.Append("UPDATE cliente SET ")
                        .Append("nome_cliente = '").Append(cliente.nomeCliente).Append("',")
                        .Append("tipo_documento = '").Append(cliente.tipoDocumento).Append("',")
                        .Append("num_documento = '").Append(cliente.numDocumento).Append("',")
                        .Append("email = '").Append(cliente.email).Append("',")
                        .Append("dt_nascimento = '").Append(DateTime.Parse(cliente.dtNascimento.Replace("-", "/")).ToString("yyyy-MM-dd")).Append("',")
                        .Append("genero = '").Append(cliente.genero).Append("',")
                        .Append("ddd = '").Append(cliente.ddd).Append("',")
                        .Append("telefone = '").Append(cliente.telefone).Append("',")
                        .Append("endereco = '").Append(cliente.endereco).Append("',")
                        .Append("numero = '").Append(cliente.numero).Append("',")
                        .Append("bairro = '").Append(cliente.bairro).Append("',")
                        .Append("cep = '").Append(cliente.cep).Append("',")
                        .Append("cidade = '").Append(cliente.cidade.codigo).Append("',") //precisa gravar a cidade antes                        
                        .Append("tipo_endereco_entrega = '").Append(cliente.tipoEnderecoEntrega).Append("',")
                        .Append("orientacao_entregador = '").Append(cliente.orientacaoEntregador).Append("',")
                        .Append("estado = '").Append(cliente.cidade.estado.sigla).Append("' ")
                        .Append("WHERE id_cliente = ").Append(cliente.idCliente);
            }

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery.ToString());
                while (reader.Read())
                {
                    //retornar o ID do cliente inserido
                    retorno = Convert.ToInt32(reader["id_cliente"]);
                }
                reader.Close();
            }


            //retorna o id do cliente
            return retorno.ToString();

        }





        public string GravaCabecalhoPedido(CabPedido pedido, string CodigoCliente)
        {
            int retorno = 0;
            var strQuery = new StringBuilder();

            //inserir um novo cliente            
            strQuery.Append("INSERT INTO pedidos (id_cliente,id_tipo_pedido,dt_pedido,dt_vencimento,id_cdm, status,idformaentrega,nome_entrega,ordem_coleta_entrega,")
                    .Append("rg_Entrega)")
                    .Append(" VALUES(")
                    .Append("'").Append(CodigoCliente).Append("',") //id do cliente
                    .Append(pedido.tipoPedido).Append(",")
                    .Append("'").Append(DateTime.Parse(pedido.dtPedido.Replace("-", "/")).ToString("yyyy-MM-dd")).Append("',")
                    .Append("'").Append(DateTime.Parse(pedido.dtVencimento.Replace("-", "/")).ToString("yyyy-MM-dd")).Append("',")
                    .Append(pedido.idCdm).Append(",")
                    .Append(pedido.status).Append(",")
                    //.Append(pedido.idEnvioUnidade).Append(",") Nao tinha este campo no banco
                    .Append(pedido.idFormaEntrega).Append(",")
                    .Append("'").Append(pedido.nomeEntrega).Append("',")
                    .Append("'").Append(pedido.ordemColetaEntrega).Append("',")
                    .Append("'").Append(pedido.rgEntrega).Append("') ")
                    .Append("; select numero_pedido from pedidos order by numero_pedido desc LIMIT 1  ");

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery.ToString());
                while (reader.Read())
                {
                    //retornar o ID do cliente inserido
                    retorno = Convert.ToInt32(reader["numero_pedido"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }


        public string GravaItensPedido(List<Item> itens)
        {
            int retorno = 0;
            var strQuery = new StringBuilder();


            for (int i = 0; i < itens.Count; i++)
            {

                //consulta se o produto existe no cadastro
                int IdProduto = Convert.ToInt32(ConsultaProduto(itens[i].produto.codigo));

                if(IdProduto > 0)
                {
                    strQuery.Clear();
                    strQuery.Append("INSERT INTO itens_pedido (numero_pedido,item,id_plano,descricao_item,valor_unitario,qtde, valor_total,pontos,id_produto)  ")
                            .Append("VALUES (")
                            .Append(itens[i].idItemPedido).Append(",")
                            .Append(itens[i].item).Append(",")
                            .Append(itens[i].plano).Append(",")
                            .Append("'").Append(itens[i].descricaoItem).Append("',")
                            .Append(itens[i].valorUnitario).Append(",")
                            .Append(itens[i].qtde).Append(",")
                            .Append(itens[i].valorTotal).Append(",")
                            .Append(itens[i].pontos).Append(",")
                            .Append(itens[i].produto.id).Append(")")
                            .Append("; select id_item_pedido from itens_pedido order by id_item_pedido desc LIMIT 1");
                }



                using (contexto = new Contexto())
                {
                    var reader = contexto.ExecutaComandoComRetorno(strQuery.ToString());
                    while (reader.Read())
                    {
                        //retornar o ID do cliente inserido
                        retorno = Convert.ToInt32(reader["id_item_pedido"]);
                    }
                    reader.Close();
                }
            }

            return retorno.ToString();
        }
    }
}
