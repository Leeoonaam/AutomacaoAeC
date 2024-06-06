using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using automacao.Infrastructure.Drivers;
using automacao.Domain.Services;

namespace automacao
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Boolean VerificaRetorno = false;

                // Configurar o serviço de injeção de dependência
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<WebDriverFactory>()
                    .AddSingleton(provider => provider.GetService<WebDriverFactory>().CreateDriver())
                    .AddSingleton<BrowserService>()
                    .BuildServiceProvider();

                // Usar o serviço BrowserService
                var browserService = serviceProvider.GetService<BrowserService>();
                Console.WriteLine("Iniciando o site...");
                VerificaRetorno = browserService.OpenSite("https://www.alura.com.br/");

                //valida retorno para o proximo servico
                if (VerificaRetorno == true)
                {
                    Console.WriteLine("Site ok.");
                    VerificaRetorno = false;
                    Console.WriteLine("Iniciando pesquisa...");
                    // pesquisa
                    VerificaRetorno = browserService.Search("rpa");

                    //verifica retorno da pesquisa
                    if (VerificaRetorno == true)
                    {
                        Console.WriteLine("Pesquisa ok.");
                        VerificaRetorno = false;
                        Console.WriteLine("Iniciando filtragem...");
                        //aplica filtro
                        VerificaRetorno = browserService.filtros();

                        //verifica retorno filtros
                        if (VerificaRetorno == true)
                        {
                            Console.WriteLine("Filtro ok.");
                            VerificaRetorno = false;
                            Console.WriteLine("Iniciando o check...");
                            //aplica check
                            VerificaRetorno = browserService.Check();

                            if (VerificaRetorno == true)
                            {
                                Console.WriteLine("Check ok.");
                                VerificaRetorno = false;
                                Console.WriteLine("Iniciando a captura dos dados e salvar no banco...");
                                //captura dados e abre o curso
                                VerificaRetorno = browserService.CapturaDados();

                                //verifica retorno captura dos dados
                                if (VerificaRetorno == true)
                                {
                                    Console.WriteLine("Processo finalizado!");
                                }
                                else
                                {
                                    Console.WriteLine("Erro captura dados.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Erro check.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Erro filtros.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Erro Search.");
                    }
                }
                else
                {
                    Console.WriteLine("Erro OpenSite.");
                }
                
                // Fechar o WebDriver
                serviceProvider.GetService<WebDriverFactory>().QuitDriver();
            }
            catch (Exception err)
            {
                Console.WriteLine("Erro: " + err.Message);
            }
        }
    }
}
