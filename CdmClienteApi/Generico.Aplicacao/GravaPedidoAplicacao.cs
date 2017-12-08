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
   public  class GravaPedidoAplicacao
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
                
                return streamReader.ReadToEnd();
            }
        }



        public void GravaPedido()
        {

            var strQuery = string.Format("");

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }


    }
}
