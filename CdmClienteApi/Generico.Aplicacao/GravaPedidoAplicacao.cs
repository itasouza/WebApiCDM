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
using System.Globalization;
using Generico.Dominio;

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
    
                string npedido            = p.answer.numeroPedido;
                string tipoPedido         = p.answer.tipoPedido;
                string dtPedido           = p.answer.dtPedido;
                string dtVencimento       = p.answer.dtVencimento;
                string idCdm              = p.answer.idCdm;
                string status             = p.answer.status;
                string idEnvioUnidade     = p.answer.idEnvioUnidade;
                string idFormaEntrega     = p.answer.idFormaEntrega;
                string nomeEntrega        = p.answer.nomeEntrega;
                string ordemColetaEntrega = p.answer.ordemColetaEntrega;
                string rgEntrega          = p.answer.rgEntrega;
                string notaFiscal         = p.answer.notaFiscal;
                string situacao           = p.answer.situacao;
                decimal valorTotal        = p.answer.valorTotal;
                string faixaDesconto      = p.answer.faixaDesconto;
                string valorDesconto      = p.answer.valorDesconto;
                decimal valorFinalPedido  = p.answer.valorFinalPedido;
                string flgCobrarFrete     = p.answer.flgCobrarFrete;
                decimal valorFrete        = p.answer.valorFrete;
                string servicoEntrega     = p.answer.servicoEntrega;
                string formaEntrega       = p.answer.formaEntrega;


                string CodigoCadastro = GravaCadastro(p.answer.cliente.cadastro);
                string CodigoCliente = GravaCliente(p.answer.cliente, CodigoCadastro);


                // string CodigoCabecahoPedido = GravaCabecalhoPedido(
                //             p.answer.tipoPedido,
                //             p.answer.dtPedido,
                //             p.answer.dtVencimento,
                //             p.answer.idCdm,
                //             p.answer.status,
                //             p.answer.idEnvioUnidade,
                //             p.answer.idFormaEntrega,
                //             p.answer.nomeEntrega,
                //             p.answer.ordemColetaEntrega,
                //             p.answer.rgEntrega,
                //             p.answer.notaFiscal,
                //             p.answer.situacao,
                //             p.answer.valorTotal,
                //             p.answer.faixaDesconto,
                //             p.answer.valorDesconto,
                //             p.answer.valorFinalPedido,
                //             p.answer.flgCobrarFrete,
                //             p.answer.valorFrete,
                //             p.answer.servicoEntrega,
                //             p.answer.formaEntrega,
                //             CodigoCliente,
                //             p.answer.numeroPedido

                //      );

                //dúvida
                string teste = "115873";
                GravaItensPedido(teste);

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
            var strQuery        = "";
            string cidade       = ConsultaCidade(cadastro.cidade.codigo);
            string patrocinador = ConsultaPatrocinador(cadastro.patrocinador);

            //verificar se o cadastro existe
            strQuery = string.Format("select * from cadastro WHERE num_documento = '{0}' LIMIT 1", cadastro.numDocumento.ToString());

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
                strQuery = "";
                strQuery += " INSERT INTO cadastro (id_patrocinador,confirmado,primeiro_nome,sobrenome, ";
                strQuery += " email,login,senha,dt_nascimento,tipo_documento,num_documento, genero,ddd,telefone,endereco,complemento,numero,bairro,cep, ";
                strQuery += " cidade, tipo_pessoa,dt_cadastro,cnae,ie,im,docs_verificados,apto_para_saque, docs_submetidos,categoria_cadastro)";
                strQuery += " VALUES ( ";
                strQuery += string.Format(" '{0}', ", patrocinador);
                strQuery += string.Format(" '{0}', ",  cadastro.confirmado);
                strQuery += string.Format(" '{0}',  ", cadastro.primeiroNome);
                strQuery += string.Format(" '{0}',  ", cadastro.sobrenome);
                strQuery += string.Format(" '{0}',  ", cadastro.email);
                strQuery += string.Format(" '{0}',  ", cadastro.login);
                strQuery += string.Format(" '{0}',  ", "123");
                strQuery += string.Format(" '{0}',  ", cadastro.dtNascimento.ToString("MM-dd-yyyy"));
                strQuery += string.Format(" '{0}',  ", cadastro.tipoDocumento);
                strQuery += string.Format(" '{0}',  ", cadastro.numDocumento);
                strQuery += string.Format(" '{0}',  ", cadastro.ddd);
                strQuery += string.Format(" '{0}',  ", cadastro.genero);
                strQuery += string.Format(" '{0}',  ", cadastro.telefone);
                strQuery += string.Format(" '{0}',  ", cadastro.bairro);
                strQuery += string.Format(" '{0}',  ", cadastro.cep);
                strQuery += string.Format(" '{0}',  ", cadastro.endereco);
                strQuery += string.Format(" '{0}',  ", cadastro.complemento);
                strQuery += string.Format(" '{0}',  ", cadastro.numero);
                strQuery += string.Format(" '{0}',  ", cidade);
                strQuery += string.Format(" '{0}',  ", cadastro.tipoPessoa);
                strQuery += string.Format(" '{0}',  ", DateTime.Now.ToString("MM-dd-yyy"));
                strQuery += string.Format(" '{0}',  ", cadastro.cnae);
                strQuery += string.Format(" '{0}',  ", cadastro.ie);
                strQuery += string.Format(" '{0}',  ", cadastro.im);
                strQuery += string.Format(" '{0}',  ", "true");
                strQuery += string.Format(" '{0}',  ", "true");
                strQuery += string.Format(" '{0}',  ", "true");
                strQuery += string.Format(" '{0}'  ", "NA");
                strQuery += string.Format(" ) ");
                strQuery += string.Format("; select * from cadastro where num_documento = '{0}' LIMIT 1", cadastro.numDocumento);


            }
            else
            {

                strQuery = "";
                strQuery += " UPDATE cadastro SET ";
                strQuery += string.Format(" id_patrocinador  = '{0}',  ", patrocinador);
                strQuery += string.Format(" confirmado       = '{0}',  ", cadastro.confirmado);
                strQuery += string.Format(" primeiro_nome    = '{0}',  ", cadastro.primeiroNome);
                strQuery += string.Format(" sobrenome        = '{0}',  ", cadastro.sobrenome);
                strQuery += string.Format(" email            = '{0}',  ", cadastro.email);
                //strQuery += string.Format(" login            = '{0}',  ", cadastro.login);
                strQuery += string.Format(" senha            = '{0}',  ", "123");
                strQuery += string.Format(" dt_nascimento    = '{0}',  ", cadastro.dtNascimento.ToString("MM-dd-yyyy"));
               // strQuery += string.Format(" tipo_documento   = '{0}',  ", cadastro.tipoDocumento);
               // strQuery += string.Format(" num_documento    = '{0}',  ", cadastro.numDocumento);
                strQuery += string.Format(" genero           = '{0}',  ", cadastro.genero);
                strQuery += string.Format(" ddd              = '{0}',  ", cadastro.ddd);
                strQuery += string.Format(" telefone         = '{0}',  ", cadastro.telefone);
                strQuery += string.Format(" endereco         = '{0}',  ", cadastro.endereco);
                strQuery += string.Format(" complemento      = '{0}',  ", cadastro.complemento);
                strQuery += string.Format(" numero           = '{0}',  ", cadastro.numero);
                strQuery += string.Format(" bairro           = '{0}',  ", cadastro.bairro);
                strQuery += string.Format(" cep              = '{0}',  ", cadastro.cep);
                strQuery += string.Format(" cidade           = '{0}',  ", cidade);
                strQuery += string.Format(" tipo_pessoa      = '{0}',  ", cadastro.tipoPessoa);
                strQuery += string.Format(" dt_cadastro      = '{0}',  ", DateTime.Now.ToString("MM-dd-yyy"));
                strQuery += string.Format(" cnae             = '{0}',  ", cadastro.cnae);
                strQuery += string.Format(" ie               = '{0}',  ", cadastro.ie);
                strQuery += string.Format(" im               = '{0}',  ", cadastro.im);
                strQuery += string.Format(" docs_verificados = '{0}',  ", "true");
                strQuery += string.Format(" apto_para_saque  = '{0}',  ", "true");
                strQuery += string.Format(" docs_submetidos    = '{0}',  ", "true");
                strQuery += string.Format(" categoria_cadastro = '{0}'  ", "NA");
                strQuery += string.Format(" where  num_documento = '{0}'  ", cadastro.numDocumento);
                strQuery += string.Format("; select * from cadastro where num_documento = '{0}' LIMIT 1", cadastro.numDocumento);

     
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
            var strQuery = "";
            string cadastro = CodigoCadastro;
   
            //verificar se o cadastro existe
            strQuery = string.Format("select * from cliente WHERE num_documento = '{0}' LIMIT 1", cliente.numDocumento.ToString());

           
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
                strQuery = "";
                strQuery += " INSERT INTO cliente (id_cadastro,nome_cliente,tipo_documento,num_documento,";
                strQuery += " email,dt_nascimento,genero,ddd,telefone,endereco, numero,bairro,cep,cidade,tipo_endereco_entrega,orientacao_entregador, estado)";
                strQuery += " VALUES ( ";
                strQuery += string.Format(" '{0}', ", cadastro);
                strQuery += string.Format(" '{0}', ", cliente.nomeCliente);
                strQuery += string.Format(" '{0}',  ", cliente.tipoDocumento);
                strQuery += string.Format(" '{0}',  ", cliente.numDocumento);
                strQuery += string.Format(" '{0}',  ", cliente.email);
                strQuery += string.Format(" '{0}',  ", DateTime.Now.ToString("MM-dd-yyyy"));
                strQuery += string.Format(" '{0}',  ", cliente.genero);
                strQuery += string.Format(" '{0}',  ", cliente.ddd);
                strQuery += string.Format(" '{0}',  ", cliente.telefone );
                strQuery += string.Format(" '{0}',  ", cliente.endereco);
                strQuery += string.Format(" '{0}',  ", cliente.numero);
                strQuery += string.Format(" '{0}',  ", cliente.bairro);
                strQuery += string.Format(" '{0}',  ", cliente.cep);
                strQuery += string.Format(" '{0}',  ", cliente.cidade.codigo);
                strQuery += string.Format(" '{0}',  ", cliente.tipoEnderecoEntrega);
                strQuery += string.Format(" '{0}',  ", cliente.orientacaoEntregador);
                strQuery += string.Format(" '{0}'   ", cliente.cidade.estado.sigla);
                strQuery += string.Format(" ) ");
                strQuery += string.Format("; select * from cliente where num_documento = '{0}' order by id_cliente desc LIMIT 1", cliente.numDocumento);
         }
            else
            {

                strQuery = "";
                strQuery += " UPDATE cliente SET ";
                strQuery += string.Format(" id_cadastro    = '{0}', ", cadastro);
                strQuery += string.Format(" nome_cliente   = '{0}', ", cliente.nomeCliente);
                strQuery += string.Format(" tipo_documento = '{0}',  ", cliente.tipoDocumento);
                strQuery += string.Format(" num_documento  = '{0}',  ", cliente.numDocumento);
                strQuery += string.Format(" email          = '{0}',  ", cliente.email);
                strQuery += string.Format(" dt_nascimento  = '{0}',  ", DateTime.Now.ToString("MM-dd-yyyy"));
                strQuery += string.Format(" genero         = '{0}',  ", cliente.genero);
                strQuery += string.Format(" ddd            = '{0}',  ", cliente.ddd);
                strQuery += string.Format(" telefone       = '{0}',  ", cliente.telefone);
                strQuery += string.Format(" endereco       = '{0}',  ", cliente.endereco);
                strQuery += string.Format(" numero         = '{0}',  ", cliente.numero);
                strQuery += string.Format(" bairro         = '{0}',  ", cliente.bairro);
                strQuery += string.Format(" cep            = '{0}',  ", cliente.cep);
                strQuery += string.Format(" cidade         = '{0}',  ", cliente.cidade.codigo);
                strQuery += string.Format(" tipo_endereco_entrega =  '{0}',  ", cliente.tipoEnderecoEntrega);
                strQuery += string.Format(" orientacao_entregador =  '{0}',  ", cliente.orientacaoEntregador);
                strQuery += string.Format(" estado                =  '{0}'   ", cliente.cidade.estado.sigla);
                strQuery += string.Format(" where  num_documento = '{0}'  ", cliente.numDocumento);
                strQuery += string.Format("; select * from cliente where num_documento = '{0}' order by id_cliente desc LIMIT 1", cliente.numDocumento);
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




        public string GravaCabecalhoPedido(
                string tipoPedido,
                string dt_pedido,
                string dt_vencimento,
                string id_cdm,
                string status,
                string idEnvioUnidade,
                string idFormaEntrega,
                string nomeEntrega,
                string ordemColetaEntrega,
                string rgEntrega,
                string notaFiscal,
                string situacao,
                decimal valor_total,
                string faixa_desconto,
                string valor_desconto,
                decimal valor_final_pedido,
                string flg_cobrar_frete,
                decimal valor_frete,
                string servico_entrega,
                string forma_entrega,
                string id_cliente,
                string numero_pedido
            )
        {

            if(string.IsNullOrEmpty(faixa_desconto))
            {
                faixa_desconto = "0";
            }

            if (string.IsNullOrEmpty(valor_desconto))
            {
                valor_desconto = "0";
            }

            int retorno = 0;
            var strQuery = "";

            strQuery = "";
            strQuery += " INSERT INTO pedidos (id_tipo_pedido,dt_pedido,situacao,valor_total,faixa_desconto,valor_desconto,valor_final_pedido,flg_cobrar_frete,valor_frete,servico_entrega,forma_entrega,id_cliente,numero_pedido_ezitus)";                    
            strQuery += " VALUES ( ";
            strQuery += string.Format(" '{0}', ", tipoPedido);
            strQuery += string.Format(" '{0}', ", DateTime.Now.ToString("yyyy-MM-dd 23:59:00.000"));
            strQuery += string.Format(" '{0}', ", situacao);
            strQuery += string.Format(CultureInfo.InvariantCulture, " '{0:0.00}', ", valor_total);
            strQuery += string.Format(CultureInfo.InvariantCulture, " '{0:0.00}', ", faixa_desconto);
            strQuery += string.Format(CultureInfo.InvariantCulture, " '{0:0.00}', ", valor_desconto);
            strQuery += string.Format(CultureInfo.InvariantCulture, " '{0:0.00}', ", valor_final_pedido);
            strQuery += string.Format(" '{0}', ", flg_cobrar_frete);
            strQuery += string.Format(CultureInfo.InvariantCulture, " '{0:0.00}', ", valor_frete);
            strQuery += string.Format(" '{0}', ", servico_entrega);
            strQuery += string.Format(" '{0}', ", forma_entrega);
            strQuery += string.Format(" '{0}', ", id_cliente);
            strQuery += string.Format(" '{0}' ", numero_pedido);
            strQuery += string.Format(" ) ");
            strQuery += string.Format("; select  * from pedidos where numero_pedido_ezitus = '{0}' LIMIT 1", numero_pedido);


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


        public void GravaItensPedido(string CodigoCabecahoPedido)
        {

            var strQuery = "";
            List<Item> listaItens = new List<Item>();
            
            foreach (var item in listaItens)
            {
                strQuery = "";
            }



            //var strQuery = "";
            // foreach (Item itens in (List<Item>))
            //{

            //}



            //for (int i = 0; i < itens.Count; i++)
            //{

            //    // consulta se o produto existe no cadastro
            //    int IdProduto = Convert.ToInt32(ConsultaProduto(itens[i].produto.codigo));

            //    if (IdProduto > 0)
            //    {
            //        strQuery.Clear();
            //        strQuery.Append("INSERT INTO itens_pedido (numero_pedido,item,id_plano,descricao_item,valor_unitario,qtde, valor_total,pontos,id_produto)  ")
            //                .Append("VALUES (")
            //                .Append(itens[i].idItemPedido).Append(",")
            //                .Append(itens[i].item).Append(",")
            //                .Append(itens[i].plano).Append(",")
            //                .Append("'").Append(itens[i].descricaoItem).Append("',")
            //                .Append(itens[i].valorUnitario).Append(",")
            //                .Append(itens[i].qtde).Append(",")
            //                .Append(itens[i].valorTotal).Append(",")
            //                .Append(itens[i].pontos).Append(",")
            //                .Append(itens[i].produto.id).Append(")")
            //                .Append("; select id_item_pedido from itens_pedido order by id_item_pedido desc LIMIT 1");
            //    }



            //    using (contexto = new Contexto())
            //    {
            //        var reader = contexto.ExecutaComandoComRetorno(strQuery.ToString());
            //        while (reader.Read())
            //        {
            //            //retornar o ID do cliente inserido
            //            retorno = Convert.ToInt32(reader["id_item_pedido"]);
            //        }
            //        reader.Close();
            //    }
            //}
        }

    }



}
