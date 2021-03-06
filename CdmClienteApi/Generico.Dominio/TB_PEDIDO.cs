﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Generico.Dominio
{
    public class TB_PEDIDO
    {
        //classe para recebecer os dados lidos
        public class usuario
        {
            public string success { get; set; }
            public string errorMessage { get; set; }
            public Answer answer { get; set; }
           
       }


        //classe que recebe os dados
        public class Answer
        {
            public string token { get; set; }
           
        }



        //classe para recebecer os dados lidos do pedido
        public class Pedido
        {

            public string success { get; set; }

            public string errorMessage { get; set; }

            public AnswerPedido answer { get; set; }


        }

        public class AnswerPedido
        {
            public string numeroPedido { get; set; }
            public string tipoPedido { get; set; }
            public string dtPedido { get; set; }
            public string dtVencimento { get; set; }
            public string idCdm { get; set; }
            public string status { get; set; }
            public string idEnvioUnidade { get; set; }
            public string idFormaEntrega { get; set; }
            public string nomeEntrega { get; set; }
            public string ordemColetaEntrega { get; set; }
            public string rgEntrega { get; set; }
            public string notaFiscal { get; set; }
            public string situacao { get; set; }
            public decimal valorTotal { get; set; }
            public string faixaDesconto { get; set; }
            public string valorDesconto { get; set; }
            public decimal valorFinalPedido { get; set; }
            public string flgCobrarFrete { get; set; }
            public decimal valorFrete { get; set; }
            public string servicoEntrega { get; set; }
            public string formaEntrega { get; set; }

            public ClientePedido cliente { get; set; }
            public List<Item> itens { get; set; }

        }

        public class ClientePedido
        {
            public string idCliente { get; set; }
            public string nomeCliente { get; set; }
            public string tipoDocumento { get; set; }
            public string numDocumento { get; set; }
            public string email { get; set; }
            public string dtNascimento { get; set; }
            public string genero { get; set; }
            public string ddd { get; set; }
            public string telefone { get; set; }
            public string endereco { get; set; }
            public string numero { get; set; }
            public string bairro { get; set; }
            public string cep { get; set; }
            public Cidade cidade { get; set; }
            public string tipoEnderecoEntrega { get; set; }
            public string orientacaoEntregador { get; set; }
            public Cadastro cadastro { get; set; }
           
        }

        public class Cidade
        {
            public string codigo { get; set; }
            public string nome { get; set; }
            public Estado estado { get; set; }
        }

        public class Estado
        {
            public string sigla { get; set; }
            public string nome { get; set; }
            public Pais pais { get; set; }
        }

        public class Pais
        {
            public string codigo { get; set; }
            public string nome { get; set; }
            public string idioma { get; set; }
            public string ddi { get; set; }
        }

        public class Cadastro
        {
            public string idCadastro { get; set; }
            public string patrocinador { get; set; }
            public string confirmado { get; set; }
            public string primeiroNome { get; set; }
            public string sobrenome { get; set; }
            public string email { get; set; }
            public string login { get; set; }
            public DateTime dtNascimento { get; set; }
            public string tipoDocumento { get; set; }
            public string numDocumento { get; set; }
            public string genero { get; set; }
            public string ddd { get; set; }
            public string telefone { get; set; }
            public string endereco { get; set; }
            public string complemento { get; set; }
            public string numero { get; set; }
            public string bairro { get; set; }
            public string cep { get; set; }
            public CidadeCadastro cidade { get; set; }
            public string tipoPessoa { get; set; }
            public string dtCadastro { get; set; }
            public string cnae { get; set; }
            public string ie { get; set; }
            public string im { get; set; }
            public bool bloqueado { get; set; }


        }

        public class CidadeCadastro
        {
            public string codigo { get; set; }
            public string nome { get; set; }
            public EstadoCadastro estado { get; set; }
        }

        public class EstadoCadastro
        {
            public string sigla { get; set; }
            public string nome { get; set; }
            public PaisCadastro pais { get; set; }
        }

        public class PaisCadastro
        {
            public string codigo { get; set; }
            public string nome { get; set; }
            public string idioma { get; set; }
            public string ddi { get; set; }
        }

        public class CabPedido
        {
            public string tipoPedido { get; set; }
            public string dtPedido { get; set; }
            public string dtVencimento { get; set; }
            public string idCdm { get; set; }
            public string status { get; set; }
            public string idEnvioUnidade { get; set; }
            public string idFormaEntrega { get; set; }
            public string nomeEntrega { get; set; }
            public string ordemColetaEntrega { get; set; }
            public string rgEntrega { get; set; }
            public string notaFiscal { get; set; }
            
        }

        public class Item
        {
            public string idItemPedido { get; set; }
            public int item { get; set; }
            public string plano { get; set; }
            public Produto produto { get; set; }
            public string descricaoItem { get; set; }
            public decimal valorUnitario { get; set; }
            public string qtde { get; set; }
            public decimal valorTotal { get; set; }
            public string pontos { get; set; }
        }

        public class Produto
        {
            public string id { get; set; }
            public string codigo { get; set; }
            public string categoria { get; set; }
            public string nome { get; set; }
            public string descricao { get; set; }
            public bool profissional { get; set; }
            public decimal preco { get; set; }
            public decimal precoSDC { get; set; }
            public decimal precoDistribuidor { get; set; }
            public decimal valorReservaBonus { get; set; }
            public decimal valorReservaBonusSdc { get; set; }
            public decimal peso { get; set; }
            public decimal pontosUnilevel { get; set; }
            public decimal cubagem { get; set; }
        }


    }
}

