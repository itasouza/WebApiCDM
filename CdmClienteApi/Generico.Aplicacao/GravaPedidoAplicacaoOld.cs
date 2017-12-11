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
    public class GravaPedidoAplicacao
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




        public string ConsultaPedido(string urlpedido, string token)
        {

            var request = (HttpWebRequest)WebRequest.Create(urlpedido);
            request.Method = "GET";

            request.Headers["AuthToken"] = token;//Adicionando o AuthToken  no Header da requisição

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string json = streamReader.ReadToEnd();

                Pedido p = JsonConvert.DeserializeObject<Pedido>(json);




                string pedido = p.answer.numeroPedido;
                //vai gravar o pedido
                GravaPedido(p);

                return streamReader.ReadToEnd();
            }
        }



        public void GravaPedido(Pedido pedidoCompleto)
        {

            ////verifica se encontrou um pedido com este número
            //int retorno = 0;
            //var strQuery = "";
            //strQuery += " select  ";
            //using (contexto = new Contexto())
            //{
            //    //encontra um cdm para atendimento
            //    var reader = contexto.ExecutaComandoComRetorno(strQuery);
            //    while (reader.Read())
            //    {
            //        retorno = Convert.ToInt32(reader["numeropedido"]);
            //    }
            //    reader.Close();
            //}

            //se não encontrou, vai gravar o pedido
            //if(retorno <= 0)
            //{
            string numeroPedido = pedidoCompleto.answer.numeroPedido;
            //  string idCliente = cliente.idCliente;
            //string tipoDocumento = pedidoCompleto.cliente.tipoDocumento;
            //string numDocumento = pedidoCompleto.cliente.numDocumento;
            //string email = pedidoCompleto.cliente.email;
            //string dtNascimento = pedidoCompleto.cliente.dtNascimento;
            //string genero = pedidoCompleto.cliente.genero;
            //string ddd = pedidoCompleto.cliente.ddd;
            //string telefone = pedidoCompleto.cliente.telefone;
            //string endereco = pedidoCompleto.cliente.endereco;
            //string numero = pedidoCompleto.cliente.numero;
            //string bairro = pedidoCompleto.cliente.bairro;
            //string cep = pedidoCompleto.cliente.cep;
            //string tipoEnderecoEntrega = pedidoCompleto.cliente.tipoEnderecoEntrega;
            //string orientacaoEntregador = pedidoCompleto.cliente.orientacaoEntregador;

            //}




            //using (contexto = new Contexto())
            //{
            //    contexto.ExecutaComando(strQuery);
            //}
        }



        public string GravaCliente(Cliente cliente)
        {
            int retorno = 0;
            var strQuery = "";
            string cadastro = ConsultaCadastro(cliente.cadastro.ToString());

            if(cadastro == "0")
            {
               //temos que gravar o cadastro porque ele não existe
            }

            //verificar se o cliente existe
            strQuery = string.Format("select * from cliente WHERE id_cliente = '{0}' ", cliente.idCliente);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id do cliente
                    retorno = Convert.ToInt32(reader["id_cliente"]);
                }
                reader.Close();
            }

            if (retorno == 0)
            {
                //inserir um novo cliente
                strQuery = "";
                strQuery += "INSERT INTO cliente (id_cadastro,nome_cliente,tipo_documento,num_documento,";
                strQuery += "email,dt_nascimento,genero,ddd,telefone,endereco, numero,bairro,cep,cidade,tipo_endereco_entrega,orientacao_entregador,estado  )";

                strQuery += string.Format(" VALUES (");
                strQuery += string.Format(" '{0}', ", cadastro); //id_cadastro - precisa gravar antes 
                strQuery += string.Format(" '{0}', ", cliente.nomeCliente);
                strQuery += string.Format(" '{0}', ", cliente.tipoDocumento);
                strQuery += string.Format(" '{0}', ", cliente.numDocumento);
                strQuery += string.Format(" '{0}', ", cliente.email);
                strQuery += string.Format(" '{0}', ", cliente.dtNascimento);
                strQuery += string.Format(" '{0}', ", cliente.genero);
                strQuery += string.Format(" '{0}', ", cliente.ddd);
                strQuery += string.Format(" '{0}', ", cliente.telefone);
                strQuery += string.Format(" '{0}', ", cliente.endereco);
                strQuery += string.Format(" '{0}', ", cliente.numero);
                strQuery += string.Format(" '{0}', ", cliente.bairro);
                strQuery += string.Format(" '{0}', ", cliente.cep);
                strQuery += string.Format(" '{0}', ", cliente.cidade); //precisa gravar a cidade antes
                strQuery += string.Format(" '{0}', ", cliente.tipoEnderecoEntrega);
                strQuery += string.Format(" '{0}', ", cliente.orientacaoEntregador);
                strQuery += string.Format(" '{0}' ", "");
                strQuery += string.Format(" ) ");
                strQuery += string.Format("; select  * from cliente order by id_cliente desc LIMIT 1  ");

                using (contexto = new Contexto())
                {
                    var reader = contexto.ExecutaComandoComRetorno(strQuery);
                    while (reader.Read())
                    {
                        //retornar o ID do cliente inserido
                        retorno = Convert.ToInt32(reader["id_cliente"]);
                    }
                    reader.Close();
                }

            }
            else
            {
                //atualiza o cliente
                strQuery = "";
                strQuery += "UPDATE cliente SET";
                strQuery += string.Format(" id_cadastro = '{0}', ", cadastro); //id_cadastro - precisa gravar antes 
                strQuery += string.Format(" nomeCliente = '{0}', ", cliente.nomeCliente);
                strQuery += string.Format(" tipoDocumento = '{0}', ", cliente.tipoDocumento);
                strQuery += string.Format(" numDocumento = '{0}', ", cliente.numDocumento);
                strQuery += string.Format(" email = '{0}', ", cliente.email);
                strQuery += string.Format(" dtNascimento = '{0}', ", cliente.dtNascimento);
                strQuery += string.Format(" genero = '{0}', ", cliente.genero);
                strQuery += string.Format(" ddd = '{0}', ", cliente.ddd);
                strQuery += string.Format(" telefone = '{0}', ", cliente.telefone);
                strQuery += string.Format(" endereco = '{0}', ", cliente.endereco);
                strQuery += string.Format(" numero = '{0}', ", cliente.numero);
                strQuery += string.Format(" bairro = '{0}', ", cliente.bairro);
                strQuery += string.Format(" cep = '{0}', ", cliente.cep);
                strQuery += string.Format(" cidade = '{0}', ", cliente.cidade); //precisa gravar a cidade antes
                strQuery += string.Format(" tipoEnderecoEntrega = '{0}', ", cliente.tipoEnderecoEntrega);
                strQuery += string.Format(" orientacaoEntregador = '{0}', ", cliente.orientacaoEntregador);
                strQuery += string.Format(" estado = '{0}' ", "");
                strQuery += string.Format(" WHERE id_cliente = '{0}' ", cliente.idCliente);

                //retornar o ID do cliente
                using (contexto = new Contexto())
                {
                    var reader = contexto.ExecutaComandoComRetorno(strQuery);
                    while (reader.Read())
                    {
                        //retornar o ID do cliente inserido
                        retorno = Convert.ToInt32(reader["id_cliente"]);
                    }
                    reader.Close();
                }

            }

            //retorna o id do cliente
            return retorno.ToString();

        }


        public string GravaProduto(Produto produto)
        {
            int retorno = 0;
            var strQuery = "";

            //verificar se o cadastro existe
            strQuery = string.Format("select * from produto WHERE id = '{0}' ", produto.id);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id do cliente
                    retorno = Convert.ToInt32(reader["id"]);
                }
                reader.Close();
            }

            if (retorno == 0)
            {

                //inserir um novo cadastro

                strQuery = "";
                strQuery += "INSERT INTO produto (codigo,categoria,nome,descricao,";
                strQuery += "profissional,preco, peso,cubagem,pontos_unilevel,publicar,imagem,alterado_por,alterado_em,limite_desconto,preco_sdc)";

                strQuery += string.Format(" VALUES (");
                strQuery += string.Format(" '{0}', ", produto.codigo);
                strQuery += string.Format(" '{0}', ", "1"); //produto.categoria
                strQuery += string.Format(" '{0}', ", produto.nome);
                strQuery += string.Format(" '{0}', ", produto.descricao);
                strQuery += string.Format(" '{0}', ", produto.profissional);
                strQuery += string.Format(" '{0}', ", produto.preco);
                strQuery += string.Format(" '{0}', ", produto.peso);
                strQuery += string.Format(" '{0}', ", produto.cubagem);
                strQuery += string.Format(" '{0}', ", produto.pontosUnilevel);
                strQuery += string.Format(" '{0}', ", true); //publicar
                strQuery += string.Format(" '{0}', ", "0"); //imagem
                strQuery += string.Format(" '{0}', ", "0"); //alterado_por
                strQuery += string.Format(" '{0}', ", "2017-08-16 12:28:03.352"); //alterado_em
                strQuery += string.Format(" '{0}', ", "0"); //limite_desconto   
                strQuery += string.Format(" '{0}' ", produto.precoSDC); 
                strQuery += string.Format(" ) ");
                strQuery += string.Format("; select  * from produto order by id desc LIMIT 1  ");

                using (contexto = new Contexto())
                {
                    var reader = contexto.ExecutaComandoComRetorno(strQuery);
                    while (reader.Read())
                    {
                        //retornar o ID do cliente inserido
                        retorno = Convert.ToInt32(reader["id"]);
                    }
                    reader.Close();
                }

            }
            else
            {
                strQuery = "";
                strQuery += "UPDATE produto SET";
                strQuery += string.Format(" VALUES (");
                strQuery += string.Format(" codigo = '{0}', ", produto.codigo);
                strQuery += string.Format(" categoria = '{0}', ", produto.categoria);
                strQuery += string.Format(" nome = '{0}', ", produto.nome);
                strQuery += string.Format(" descricao = '{0}', ", produto.descricao);
                strQuery += string.Format(" profissional = '{0}', ", produto.profissional);
                strQuery += string.Format(" preco = '{0}', ", produto.preco);
                strQuery += string.Format(" peso = '{0}', ", produto.peso);    
                strQuery += string.Format(" cubagem = '{0}', ", produto.cubagem);
                strQuery += string.Format(" pontos_unilevel = '{0}', ", produto.pontosUnilevel);
                strQuery += string.Format(" publicar = '{0}', ", true);
                strQuery += string.Format(" imagem = '{0}', ", "0");
                strQuery += string.Format(" alterado_por = '{0}', ", "0");
                strQuery += string.Format(" alterado_em = '{0}', ", "2017-08-16 12:28:03.352");
                strQuery += string.Format(" limite_desconto  = '{0}', ", "0"); //limite_desconto   
                strQuery += string.Format(" preco_sdc = '{0}' ", produto.precoSDC);
                strQuery += string.Format(" ) ");
                strQuery += string.Format(" WHERE id = '{0}' ", produto.id);
            }

            return retorno.ToString();

        }



        public string GravaCabecalhoPedido(CabPedido pedido)
        {
            int retorno = 0;
            var strQuery = "";

            //inserir um novo cliente
            strQuery = "";
            strQuery += "INSERT INTO pedidos (id_cliente,id_tipo_pedido,dt_pedido,dt_vencimento,idCdm, status,idEnvioUnidade,idFormaEntrega,nomeEntrega,ordemColetaEntrega,";
            strQuery += "rg_Entrega)";

            strQuery += string.Format(" VALUES (");
            strQuery += string.Format(" '{0}', ", "1"); //id do cliente
            strQuery += string.Format(" '{0}', ", pedido.tipoPedido);
            strQuery += string.Format(" '{0}', ", pedido.dtPedido);
            strQuery += string.Format(" '{0}', ", pedido.dtVencimento);
            strQuery += string.Format(" '{0}', ", pedido.idCdm);
            strQuery += string.Format(" '{0}', ", pedido.status);
            strQuery += string.Format(" '{0}', ", pedido.idEnvioUnidade);
            strQuery += string.Format(" '{0}', ", pedido.idFormaEntrega);
            strQuery += string.Format(" '{0}', ", pedido.nomeEntrega);
            strQuery += string.Format(" '{0}', ", pedido.ordemColetaEntrega);
            strQuery += string.Format(" '{0}' ", pedido.rgEntrega);
            strQuery += string.Format(" ) ");
            strQuery += string.Format("; select  * from pedidos order by numero_pedido desc LIMIT 1  ");

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //retornar o ID do cliente inserido
                    retorno = Convert.ToInt32(reader["numero_pedido"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }




        public string GravaCadastro(Cadastro cadastro)
        {
            int retorno = 0;
            var strQuery = "";
            string cidade = ConsultaCidade(cadastro.cidade.ToString());

            //verificar se o cadastro existe
            strQuery = string.Format("select * from cadastro WHERE id_cadastro = '{0}' ", cadastro.idCadastro);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id do cliente
                    retorno = Convert.ToInt32(reader["id_cadastro"]);
                }
                reader.Close();
            }

            if (retorno == 0)
            {
                //inserir um novo cadastro
                strQuery = "";
                strQuery += "INSERT INTO cadastro (id_patrocinador,confirmado,primeiro_nome,sobrenome,";
                strQuery += "email,login,senha,dt_nascimento,tipo_documento,num_documento, genero,ddd,telefone,endereco,complemento,numero,bairro,cep, )";
                strQuery += "cidade,banco,numero_conta,tipo_conta,titular_conta,carteira_digital,categoria_cadastro, tipo_distribuidor,)";
                strQuery += "foto, id_multiplick, tipo_pessoa,id_perfil_acesso,dt_cadastro,agencia,ultimo_pos_automatico,cnae,ie,im,docs_verificados,  )";
                strQuery += "apto_para_saque, docs_submetidos )";

                strQuery += string.Format(" VALUES (");
                strQuery += string.Format(" '{0}', ", cadastro.patrocinador);
                strQuery += string.Format(" '{0}', ", cadastro.confirmado);
                strQuery += string.Format(" '{0}', ", cadastro.primeiroNome);
                strQuery += string.Format(" '{0}', ", cadastro.sobrenome);
                strQuery += string.Format(" '{0}', ", cadastro.email);
                strQuery += string.Format(" '{0}', ", cadastro.login);
                strQuery += string.Format(" '{0}', ", "");//senha
                strQuery += string.Format(" '{0}', ", cadastro.dtNascimento);
                strQuery += string.Format(" '{0}', ", cadastro.tipoDocumento);
                strQuery += string.Format(" '{0}', ", cadastro.numDocumento);
                strQuery += string.Format(" '{0}', ", cadastro.genero);
                strQuery += string.Format(" '{0}', ", cadastro.ddd);
                strQuery += string.Format(" '{0}', ", cadastro.telefone);
                strQuery += string.Format(" '{0}', ", cadastro.endereco);
                strQuery += string.Format(" '{0}', ", cadastro.complemento);
                strQuery += string.Format(" '{0}', ", cadastro.numero);
                strQuery += string.Format(" '{0}', ", cadastro.bairro);
                strQuery += string.Format(" '{0}', ", cadastro.cep);
                strQuery += string.Format(" '{0}', ", cidade); //cidade   //preciso gravar a cidade antes
                strQuery += string.Format(" '{0}', ", ""); //banco  
                strQuery += string.Format(" '{0}', ", ""); //numero de conta    
                strQuery += string.Format(" '{0}', ", ""); //tipo de conta  
                strQuery += string.Format(" '{0}', ", ""); //titular conta 
                strQuery += string.Format(" '{0}', ", ""); //carteira digital
                strQuery += string.Format(" '{0}', ", ""); //categoria cadastro
                strQuery += string.Format(" '{0}', ", ""); //tipo distribuido
                strQuery += string.Format(" '{0}', ", ""); //foto
                strQuery += string.Format(" '{0}', ", ""); //id multiplick
                strQuery += string.Format(" '{0}', ", cadastro.tipoPessoa); //tipo de pessoa
                strQuery += string.Format(" '{0}', ", ""); //id do perfil
                strQuery += string.Format(" '{0}', ", cadastro.dtCadastro);
                strQuery += string.Format(" '{0}', ", ""); //agencia
                strQuery += string.Format(" '{0}', ", ""); //último pos automático 
                strQuery += string.Format(" '{0}', ", cadastro.cnae);
                strQuery += string.Format(" '{0}', ", cadastro.ie);
                strQuery += string.Format(" '{0}', ", cadastro.im);
                strQuery += string.Format(" '{0}', ", "true");
                strQuery += string.Format(" '{0}', ", "true");
                strQuery += string.Format(" '{0}' ", "true");
                strQuery += string.Format(" ) ");
                strQuery += string.Format("; select  * from cadastro order by id_cadastro desc LIMIT 1  ");

                using (contexto = new Contexto())
                {
                    var reader = contexto.ExecutaComandoComRetorno(strQuery);
                    while (reader.Read())
                    {
                        //retornar o ID do cliente inserido
                        retorno = Convert.ToInt32(reader["id_cadastro"]);
                    }
                    reader.Close();
                }

            }
            else
            {

                strQuery = "";
                strQuery += "UPDATE cadastro SET";
                strQuery += string.Format(" VALUES (");
                strQuery += string.Format(" id_patrocinador = '{0}', ", cadastro.patrocinador);
                strQuery += string.Format(" confirmado = '{0}', ", cadastro.confirmado);
                strQuery += string.Format(" primeiro_nome = '{0}', ", cadastro.primeiroNome);
                strQuery += string.Format(" sobrenome = '{0}', ", cadastro.sobrenome);
                strQuery += string.Format(" email = '{0}', ", cadastro.email);
                strQuery += string.Format(" login = '{0}', ", cadastro.login);
                strQuery += string.Format(" senha = '{0}', ", "123"); //senha
                strQuery += string.Format(" dt_nascimento = '{0}', ", cadastro.dtNascimento);
                strQuery += string.Format(" tipo_documento = '{0}', ", cadastro.tipoDocumento);
                strQuery += string.Format(" num_documento = '{0}', ", cadastro.numDocumento);
                strQuery += string.Format(" genero = '{0}', ", cadastro.genero);
                strQuery += string.Format(" ddd = '{0}', ", cadastro.ddd);
                strQuery += string.Format(" telefone = '{0}', ", cadastro.telefone);
                strQuery += string.Format(" endereco = '{0}', ", cadastro.endereco);
                strQuery += string.Format(" complemento = '{0}', ", cadastro.complemento);
                strQuery += string.Format(" numero = '{0}', ", cadastro.numero);
                strQuery += string.Format(" bairro = '{0}', ", cadastro.bairro);
                strQuery += string.Format(" cep = '{0}', ", cadastro.cep);
                strQuery += string.Format(" cidade = '{0}', ", cidade); //cidade   //preciso gravar a cidade antes 
                strQuery += string.Format(" banco = '{0}', ", ""); //banco  
                strQuery += string.Format(" numero_conta = '{0}', ", ""); //numero de conta    
                strQuery += string.Format(" tipo_conta = '{0}', ", ""); //tipo de conta  
                strQuery += string.Format(" titular_conta = '{0}', ", ""); //titular conta 
                strQuery += string.Format(" carteira_digital = '{0}', ", ""); //carteira digital
                strQuery += string.Format(" categoria_cadastro = '{0}', ", ""); //categoria cadastro
                strQuery += string.Format(" tipo_distribuidor = '{0}', ", ""); //tipo distribuido
                strQuery += string.Format(" foto = '{0}', ", ""); //foto
                strQuery += string.Format(" id_multiplick = '{0}', ", ""); //id multiplick
                strQuery += string.Format(" tipo_pessoa = '{0}', ", cadastro.tipoPessoa);//tipo de pessoa
                strQuery += string.Format(" id_perfil_acesso = '{0}', ", ""); //id do perfil
                strQuery += string.Format(" dt_cadastro = '{0}', ", cadastro.dtCadastro);
                strQuery += string.Format(" agencia = '{0}', ", ""); //agencia
                strQuery += string.Format(" ultimo_pos_automatico = '{0}', ", ""); //último pos automático 
                strQuery += string.Format(" cnae = '{0}', ", cadastro.cnae);
                strQuery += string.Format(" ie = '{0}', ", cadastro.ie);
                strQuery += string.Format(" im = '{0}', ", cadastro.im);
                strQuery += string.Format(" docs_verificados = '{0}', ", "true");
                strQuery += string.Format(" apto_para_saque = '{0}', ", "true");
                strQuery += string.Format(" docs_submetidos = '{0}' ", "true");
                strQuery += string.Format(" ) ");
                strQuery += string.Format(" WHERE id_cadastro = '{0}' ", cadastro.idCadastro);

                //retornar o ID do cadastro
                using (contexto = new Contexto())
                {
                    var reader = contexto.ExecutaComandoComRetorno(strQuery);
                    while (reader.Read())
                    {
                        //retornar o ID do cliente inserido
                        retorno = Convert.ToInt32(reader["id_cadastro"]);
                    }
                    reader.Close();
                }

            }

            return retorno.ToString();
        }






        public string ConsultaCadastro(string codigo)
        {
            int retorno = 0;
            var strQuery = "";

            //verificar se o cadastro existe
            strQuery = string.Format("select * from cadastro WHERE id_cadastro = '{0}' ", codigo);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id 
                    retorno = Convert.ToInt32(reader["id_cadastro"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }



        public string ConsultaCidade(string codigo)
        {
            int retorno = 0;
            var strQuery = "";

            //apensas vamos retornar a cidade, não vamos gravar ou atualizar
            strQuery = string.Format("select * from cidade WHERE codigo = '{0}' ", codigo);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id da cidade
                    retorno = Convert.ToInt32(reader["codigo"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }


        public string ConsultaPais(string codigo)
        {
            int retorno = 0;
            var strQuery = "";

            //apensas vamos retornar o pais, não vamos gravar ou atualizar
            strQuery = string.Format("select * from pais WHERE codigo = '{0}' ", codigo);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id da cidade
                    retorno = Convert.ToInt32(reader["codigo"]);
                }
                reader.Close();
            }

            return retorno.ToString();
        }


        public string ConsultaEstado(string sigla)
        {
            string retorno = "";
            var strQuery = "";

            //apensas vamos retornar o estado, não vamos gravar ou atualizar
            strQuery = string.Format("select * from estado WHERE sigla = '{0}' ", sigla);

            using (contexto = new Contexto())
            {
                var reader = contexto.ExecutaComandoComRetorno(strQuery);
                while (reader.Read())
                {
                    //pega o Id da cidade
                    retorno = reader["sigla"].ToString();
                }
                reader.Close();
            }

            return retorno.ToString();
        }




    }


}
