using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generico.Dominio
{
   public class TB_UNIDADE
    {
        public int idcdm { get; set; }
        public string ativo { get; set; }
        public string cnpj { get; set; }
        public string nome { get; set; }
        public string atende_pedido_especial { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string cep { get; set; }
        public string contato { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
    }
}
