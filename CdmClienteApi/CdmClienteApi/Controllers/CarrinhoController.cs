using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CdmClienteApi.Controllers
{
    [RoutePrefix("api")]
    public class CarrinhoController : ApiController
    {


        //http://localhost:7630/api/unidade/consulta/carrinho/122865/PA00058:1,MA00068:1,PA00004:1/41706670/false
        [HttpGet]
        [Route("unidade/consulta/carrinho/{numerocarrinho}/{itens}/{cep}/{retiralocal}")]
        public HttpResponseMessage ConsultaUnidadeAtendimento(string numerocarrinho, string[] itens, string cep, string retiralocal)
        {
         
            try
            {
               var tTabela = "";
               var listar = "";
               return Request.CreateResponse(HttpStatusCode.OK, new { usuario = listar.ToArray() });
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }



        public class Carrinho
        {
            public string numerocarrinho { get; set; }
            public Itens itens { get; set; }
            public string cep { get; set; }
            public string retiralocal { get; set; }

            public Carrinho(string NumeroCarrinho, string Cep, string RetiraLocal)
            {
                this.numerocarrinho = NumeroCarrinho;
                this.cep = Cep;
                this.retiralocal = RetiraLocal;
            }

        }

        public class Itens
        {
            public string itens { get; set; }

            public Itens(string Itens)
            {
                this.itens = Itens;
            }
        }


    }


}
