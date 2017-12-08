using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Generico.Aplicacao;
using Generico.Dominio;
using Generico.Repositorio;

namespace CdmClienteApi.Controllers
{
    [RoutePrefix("api")]
    public class CarrinhoController : ApiController
    {
        //caminho inicial da api
        string UrlApi = "http://hml.ezitus.com/matriz/";

         //aqui a api recebe um array de dados
          //  {
          //  "numeroCarrinho":122865,
          //  "itens":[
          //      {"codigo":"PA00058","qtd":1},
          //      {"codigo":"MA00068","qtd":1},
          //      {"codigo":"PA00004","qtd":1},
          //      {"codigo":"PA00009","qtd":1}
          //  ],
          //  "cep":"08914444",
          //  "retiradaNoLocal":false
          //}
    
        //http://localhost:49764/api/unidade/carrinho/consultaUnidadeAtendimento
        [HttpPost]
        [Route("unidade/carrinho/ConsultaUnidadeAtendimento")]
        public HttpResponseMessage ConsultaUnidadeAtendimento(TB_DADOS_API consultaAtendimento)
        {
         
            try
            {
                //envia todos os dados para validação
                var tTabela = new ConsultaUnidadeEstoque();
                var listar = tTabela.SelecionaUnidadeAtendimento(consultaAtendimento);
                return Request.CreateResponse(HttpStatusCode.OK, new { dados = listar.ToArray() });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }




        //http://localhost:49764/api/unidade/carrinho/GravaPedido/3/115873/ABERTO
        //http://localhost:49764/api/unidade/carrinho/GravaPedido/3/115873/CANCELADO
        [HttpPost]
        [Route("unidade/carrinho/GravaPedido/{idcdm}/{idpedido}/{status}")]
        public HttpResponseMessage GravaPedido(string idcdm, string idpedido, string status)
        {
            string token = "";
            string pedido = "";
            var tabela = new GravaPedidoAplicacao();

            try
            {
                if (status == "ABERTO")
                {
                    //consulta o usuario e pega o token
                     string teste = UrlApi + "services/auth/login";

                     token =  tabela.ConsultaUsuario(UrlApi+"services/auth/login");
                    //consulta o pedido e grava no banco 
                     pedido = tabela.ConsultaPedido(UrlApi+"services/pedidos/porNumero", idpedido, token);
                    //abre o pedido, deixando disponivel para a unidade
                     var tTabela = new ConsultaUnidadeEstoque();
                         tTabela.AbrePedido(idpedido);
                }

                if (status == "CANCELADO")
                {
                    //cancelar o pedido
                    var tTabela = new ConsultaUnidadeEstoque();
                        tTabela.CancelamentoPedido(idpedido);
                }


                 return Request.CreateResponse(HttpStatusCode.OK, new { dados = pedido.ToArray() });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }




    }


}
