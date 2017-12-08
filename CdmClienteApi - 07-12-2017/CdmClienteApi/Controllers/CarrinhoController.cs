using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Generico.Aplicacao;
using Generico.Dominio;

namespace CdmClienteApi.Controllers
{
    [RoutePrefix("api")]
    public class CarrinhoController : ApiController
    {


        //http://localhost:49764/api/unidade/carrinho/consultaUnidadeAtendimento
        [HttpPost]
        [Route("unidade/carrinho/ConsultaUnidadeAtendimento")]
        public HttpResponseMessage ConsultaUnidadeAtendimento(TB_DADOS_API consultaAtendimento)
        {
         
            try
            {

                string numeroCarrinho =   consultaAtendimento.NumeroCarrinho.ToString();
                string cep = consultaAtendimento.Cep;
                bool retiraLocal = consultaAtendimento.RetiraNoLocal;

                var tTabela = new ConsultaUnidadeEstoque();
                var listar = tTabela.SelecionaUnidadeAtendimento(cep);

                //vai verficar os itens
                foreach (var item in consultaAtendimento.Itens)
                {
                    var tTabelaItens = new ConsultaUnidadeEstoque();
                    var listarItens = tTabelaItens.ConsultaProdutoNoEstoque( item.Codigo,item.Qtd);

                }

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
                     token =  tabela.ConsultaUsuario("http://hml.ezitus.com/matriz/services/auth/login");
                     pedido = tabela.ConsultaPedido("http://hml.ezitus.com/matriz/services/pedidos/porNumero", idpedido, token);
                }

                if (status == "CANCELADO")
                {
                    //cancelar o pedido
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
