using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generico.Dominio
{
   public class TB_FAIXA_ATENDIMENTO
    {
        public string idfaixaatendimento { get; set; }
        public string nome_bairro { get; set; }
        public string faixa_cep_final { get; set; }
        public string faixa_cep_inicial { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string unidade_idcdm { get; set; }

    }
}
