using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Generico.Aplicacao;



namespace CdmClienteApi.Controllers
{
    [RoutePrefix("api")]
    public class CarrinhoController : ApiController
    {


        //http://localhost:49764/api/unidade/carrinho/consultaUnidadeAtendimento
        [HttpPost]
        [Route("unidade/carrinho/ConsultaUnidadeAtendimento")]
        public HttpResponseMessage ConsultaUnidadeAtendimento(ConsultaUnidadeAtendimentoModel consultaAtendimento)
        {
         
            try
            {
                string numeroCarrinho = consultaAtendimento.NumeroCarrinho.ToString();
                string cep = consultaAtendimento.Cep;
                bool retiraLocal = consultaAtendimento.RetiraNoLocal;

                var tTabela = new ConsultaUnidadeEstoque();
                var listar = tTabela.SelecionaUnidadeAtendimento(cep);
                return Request.CreateResponse(HttpStatusCode.OK, new { dados = listar.ToArray() });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }



        public  class ConsultaUnidadeAtendimentoModel
        {
            [JsonProperty("numeroCarrinho")]
            public long NumeroCarrinho { get; set; }

            [JsonProperty("itens")]
            public List<Carrinho> Itens { get; set; }

            [JsonProperty("cep")]
            public string Cep { get; set; }

            [JsonProperty("retiraNoLocal")]
            public bool RetiraNoLocal { get; set; }
        }

        public class Carrinho
        {
            [JsonProperty("codigo")]
            public string Codigo { get; set; }
            [JsonProperty("qtd")]
            public int Qtd { get; set; }
        }



    }


}
