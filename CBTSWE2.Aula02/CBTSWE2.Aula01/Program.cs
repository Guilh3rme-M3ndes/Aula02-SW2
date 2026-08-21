// Stiven Richardy Silva Rodrigues
// Guilherme Mendes de Sousa

using CBTSWE2.Aula01;
using CBTSWE2.Aula01.Negocio;
using CBTSWE2.Aula01.Repositorio;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;

var _repo = new LivroRepositorioSQLite();

if (!_repo.Todos().Any())
{
    var autor1 = new Autor("Stiven Richardy Silva Rodrigues", "stiven.rodrigues@aluno.ifsp.edu.br", 'M');
    var autor2 = new Autor("Guilherme Mendes de Sousa", "mendes.sousa@aluno.ifsp.edu.br", 'M');
    var livro = new Livro("Sistemas Web II", new[] { autor1, autor2 }, 150.50, 10);
    _repo.Incluir(livro);
    Console.WriteLine("[SEED] Banco populado com sucesso.");
}

IWebHost host = new WebHostBuilder()
    .UseKestrel()
    .UseStartup<Startup>()
    .Build();

host.Run();
