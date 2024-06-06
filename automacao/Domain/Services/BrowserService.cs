using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace automacao.Domain.Services
{
    public class BrowserService
    {
        private readonly IWebDriver _driver;

        public BrowserService(IWebDriver driver)
        {
            _driver = driver;
        }

        public bool OpenSite(string url)
        {
            try
            {
                Boolean Sucess = false;

                _driver.Navigate().GoToUrl(url); //site
                _driver.Manage().Window.Maximize(); //maximiza o navegador

                //loop verifica janela
                for (int i = 0; i < _driver.WindowHandles.Count; i++)
                {
                    _driver.SwitchTo().Window(_driver.WindowHandles[i]);

                    if (_driver.Title == "Alura | Cursos online de Tecnologia")
                    {
                        //verifica se carregou o campo para pesquisa
                        Sucess = WaitObject("header-barraBusca-form-campoBusca", "id");
                        //valida retorno
                        if (Sucess == false)
                        {
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("Ok, site carregado.");
                            break;
                        }
                    }
                }

                return true;

            }
            catch (Exception err)
            {
                Console.WriteLine("Erro ao abrir o site: " + err.Message);
                return false;
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool Search(string valor)
        {
            try
            {
                DateTime dt = DateTime.Now;

                //preenche o campo pesquisa
                List<IWebElement> elementpesquisa = new List<IWebElement>();
                elementpesquisa.AddRange(_driver.FindElements(By.Id("header-barraBusca-form-campoBusca")));

                //verifica quantidade de elemento com o id encontrado
                if (elementpesquisa.Count <= 0)
                {
                    //time
                    while (elementpesquisa.Count <= 0)
                    {
                        elementpesquisa.AddRange(_driver.FindElements(By.Id("header-barraBusca-form-campoBusca")));

                        if ((DateTime.Now - dt).TotalSeconds > 10)
                        {
                            // erro
                            Console.WriteLine("Erro ao encontrar elemento pesquisa.");
                            return false;
                        }
                    }
                    //força
                    elementpesquisa[0].SendKeys(valor);
                    return false;
                }
                else
                {
                    //preenche
                    elementpesquisa[0].SendKeys(valor);

                    //verifica preenchimento e força novamente
                    while (elementpesquisa[0].GetAttribute("value") == "")
                    {
                        elementpesquisa.AddRange(_driver.FindElements(By.Id("header-barraBusca-form-campoBusca")));
                        elementpesquisa[0].SendKeys(valor);
                        return true;
                    }

                    elementpesquisa[0].SendKeys(Keys.Enter);
                    // ok
                    return true;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Erro ao pesquisar: " + err.Message);
                return false;
            }
        }

        /// <summary>
        /// filtros
        /// </summary>
        /// <returns></returns>
        public bool filtros()
        {
            try
            {
                Boolean Sucess = false;
                DateTime dt = DateTime.Now;

                //verifica objeto na tela apos pesquisa
                Sucess = Wait_Text("Opções e filtros de busca avançada");

                //valida retorno
                if (Sucess == true)
                {
                    //link
                    List<IWebElement> elementpesquisa = new List<IWebElement>();
                    elementpesquisa.AddRange(_driver.FindElements(By.CssSelector(".show-filter-options")));

                    //verifica quantidade de elemento com o id encontrado
                    if (elementpesquisa.Count <= 0)
                    {
                        //time
                        while (elementpesquisa.Count <= 0)
                        {
                            elementpesquisa.AddRange(_driver.FindElements(By.CssSelector(".show-filter-options")));

                            if ((DateTime.Now - dt).TotalSeconds > 10)
                            {
                                // erro
                                Console.WriteLine("Erro ao encontrar elemento opção do filtro.");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //clica
                        elementpesquisa[0].Click();
                        Console.WriteLine("Abertura de Filtro ok.");
                        // ok
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Erro ao abrir opções de filtro.");
                    return false;
                }
                return false;           
            }
            catch (Exception err)
            {
                Console.WriteLine("Erro filtros: " + err.Message);
                return false;
            }
            
        }

        /// <summary>
        /// Check
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            try
            {
                DateTime dt = DateTime.Now;
                Boolean Sucess = false;

                Sucess = Wait_Text("Tipos de conteúdo");

                if (Sucess == true)
                {
                    //opção de check
                    List<IWebElement> elementpesquisa = new List<IWebElement>();
                    elementpesquisa.AddRange(_driver.FindElements(By.CssSelector("label[for='type-filter--0']")));

                    //verifica quantidade de elemento com o id encontrado
                    if (elementpesquisa.Count <= 0)
                    {
                        //time
                        while (elementpesquisa.Count <= 0)
                        {
                            elementpesquisa.AddRange(_driver.FindElements(By.CssSelector("label[for='type-filter--0']")));

                            if ((DateTime.Now - dt).TotalSeconds > 10)
                            {
                                // erro
                                Console.WriteLine("Erro ao encontrar check.");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //clica no check cursos
                        elementpesquisa[0].Click();
                        Console.WriteLine("Check de filtro ok.");

                        //opção de check
                        List<IWebElement> pesquisaresultado = new List<IWebElement>();
                        pesquisaresultado.AddRange(_driver.FindElements(By.Id("busca--filtrar-resultados")));

                        //verifica quantidade de elemento com o id encontrado
                        if (pesquisaresultado.Count <= 0)
                        {
                            //time
                            while (pesquisaresultado.Count <= 0)
                            {
                                pesquisaresultado.AddRange(_driver.FindElements(By.Id("busca--filtrar-resultados")));

                                if ((DateTime.Now - dt).TotalSeconds > 10)
                                {
                                    // erro
                                    Console.WriteLine("Erro ao clicar em botão pesquisar resultado.");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            pesquisaresultado[0].Click();
                            // ok
                            return true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Elemento não encontrado para check.");
                    return false;
                }
                return false;
            }
            catch (Exception err)
            {
                Console.WriteLine("Erro ao Checkar: " + err.Message);
                return false;
            }
        }

        /// <summary>
        /// CapturaDados
        /// </summary>
        /// <returns></returns>
        public bool CapturaDados()
        {
            try
            {
                DateTime dt = DateTime.Now;
                string Titulo = "";
                string Professor = "";
                string CargaHoraria = "";
                string Descricao = "";
                conexao cnn = new conexao();

                //captura titulo
                List<IWebElement> elementTitulo = new List<IWebElement>();
                elementTitulo.AddRange(_driver.FindElements(By.CssSelector("h4.busca-resultado-nome")));

                //verifica quantidade de elemento com o id encontrado
                if (elementTitulo.Count <= 0)
                {
                    //time
                    while (elementTitulo.Count <= 0)
                    {
                        elementTitulo.AddRange(_driver.FindElements(By.CssSelector(".show-filter-options")));

                        if ((DateTime.Now - dt).TotalSeconds > 10)
                        {
                            // erro
                            Console.WriteLine("Erro ao encontrar elemento opção do filtro.");
                            return false;
                        }
                    }
                }
                else
                {
                    //captura
                    Titulo = elementTitulo[0].Text;

                    //captura descricao
                    List<IWebElement> elementDesc = new List<IWebElement>();
                    elementDesc.AddRange(_driver.FindElements(By.CssSelector("p.busca-resultado-descricao")));
                    Descricao = elementDesc[0].Text;

                    //clica na primeira opção de curso
                    List<IWebElement> elementListaCursos = new List<IWebElement>();
                    elementListaCursos.AddRange(_driver.FindElements(By.CssSelector("a.busca-resultado-link")));
                    elementListaCursos[0].Click();

                    Thread.Sleep(250);

                    //clica na primeira opção de curso
                    List<IWebElement> elementCarga = new List<IWebElement>();
                    elementCarga.AddRange(_driver.FindElements(By.CssSelector("p.courseInfo-card-wrapper-infos")));

                    //como contem mais de uma elemento, loop para verificação do valor correto
                    for (int i = 0; i < elementCarga.Count; i++)
                    {
                        //verifica retorno
                        if (elementCarga[i].Text != "" || elementCarga[i].Text != null)
                        {
                            //verifica valor
                            if (elementCarga[i].Text.Contains("h"))
                            {
                                //captura
                                CargaHoraria = elementCarga[i].Text;
                                break;
                            }
                        }
                    }

                    //clica na primeira opção de curso
                    List<IWebElement> elementProf = new List<IWebElement>();
                    elementProf.AddRange(_driver.FindElements(By.CssSelector("h3.instructor-title--name")));
                    Professor = elementProf[0].Text;

                    //insere os dados no banco sql
                    using (SqlConnection cn = cnn.abre_cn())
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO Cursos (Titulo,Descricao,CargaHoraria,Professor)
                                                VALUES ('" + Titulo + "','" + Descricao + "','" + CargaHoraria + "','" + Professor + "')";

                            cmd.ExecuteNonQuery();
                        }
                    }
                    // ok
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {
                Console.WriteLine("Erro ao Checkar: " + err.Message);
                return false;
            }
        }


        /// <summary>
        /// WaitObject
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="stipo"></param>
        /// <returns></returns>
        public bool WaitObject(string valor, string stipo)
        {
            try
            {
                DateTime dt;
                bool exec = true;
                List<IWebElement> conta;
                bool bok = false;
                int sec = 0;

                dt = DateTime.Now;

                while (exec == true)
                {
                    conta = new List<IWebElement>();

                    //tipo
                    if (stipo.ToLower() == "id")
                    {
                        conta.AddRange(_driver.FindElements(By.Id(valor)));
                    }

                    if (stipo.ToLower() == "name")
                    {
                        conta.AddRange(_driver.FindElements(By.Name(valor)));
                    }

                    if (stipo.ToLower() == "ClassName")
                    {
                        conta.AddRange(_driver.FindElements(By.ClassName(valor)));
                    }

                    if (stipo.ToLower() == "xpath")
                    {
                        conta.AddRange(_driver.FindElements(By.XPath(valor)));
                    }

                    if (stipo.ToLower() == "CssSelector")
                    {
                        conta.AddRange(_driver.FindElements(By.CssSelector(valor)));
                    }

                    if (conta.Count > 0)
                    {
                        bok = true;
                        exec = false;
                    }
                    else
                    {
                        sec = Convert.ToInt32((DateTime.Now - dt).TotalSeconds);

                        //time out
                        if (sec > 10)
                        {
                            bok = false;
                            exec = false;
                        }
                    }

                    conta = null;
                    Thread.Sleep(100);
                }

                return bok;
                
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Wait_Text
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public Boolean Wait_Text(string valor)
        {
            DateTime dt;
            bool exec = false;
            int sec = 0;
            string sbody = "";

            try
            {
                dt = DateTime.Now;

                _driver.SwitchTo().DefaultContent();

                //TEXTO DO MODAL 
                sbody = _driver.FindElement(By.TagName("body")).GetAttribute("innerText");

                while (exec == false)
                {

                    //VERIFICA TEXTO NA TELA SE CARREGOU
                    if (sbody.IndexOf(valor) > 0)
                    {
                        exec = true;
                    }
                    else
                    {

                        sec = Convert.ToInt32((DateTime.Now - dt).TotalSeconds);

                        //time out
                        if (sec > 25)
                        {
                            _driver.SwitchTo().DefaultContent();
                            exec = true;
                        }

                    }

                }
                return exec;

            }
            catch (Exception)
            {
                return exec;
            }


        }
    }
}
