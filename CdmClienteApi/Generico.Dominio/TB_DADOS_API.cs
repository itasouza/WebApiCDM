using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Generico.Dominio
{
   public class TB_DADOS_API
    {
        [JsonProperty("numeroCarrinho")]
        public long NumeroCarrinho { get; set; }

        [JsonProperty("itens")]
        public List<TB_CARRINHO> Itens { get; set; }

        [JsonProperty("cep")]
        public string Cep { get; set; }

        [JsonProperty("retiraNoLocal")]
        public bool RetiraNoLocal { get; set; }
    }
}
