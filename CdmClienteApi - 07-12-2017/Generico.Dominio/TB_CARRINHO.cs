using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Generico.Dominio
{
    public class TB_CARRINHO
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }
        [JsonProperty("qtd")]
        public int Qtd { get; set; }
    }
}
