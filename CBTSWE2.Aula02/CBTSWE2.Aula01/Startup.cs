using CBTSWE2.Aula01.Negocio;
using CBTSWE2.Aula01.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBTSWE2.Aula01
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(Roteamento);
        }

        private Livro ObterUltimoLivroDoBanco()
        {
            ILivroRepositorio repo = new LivroRepositorioSQLite();
            return repo.Todos().LastOrDefault();
        }

        public Task RotaNomeLivro(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
            var livro = ObterUltimoLivroDoBanco();
            return context.Response.WriteAsync(livro != null ? livro.Titulo : "Nenhum livro no banco.");
        }

        public Task RotaToString(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
            var livro = ObterUltimoLivroDoBanco();
            return context.Response.WriteAsync(livro != null ? livro.ToString() : "Nenhum livro no banco.");
        }

        public Task RotaNomesAutores(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
            var livro = ObterUltimoLivroDoBanco();
            return context.Response.WriteAsync(livro != null ? livro.ObterNomesAutores() : "Nenhum livro no banco.");
        }

        public Task RotaApresentarLivroHTML(HttpContext context)
        {
            var livro = ObterUltimoLivroDoBanco();

            if (livro == null)
            {
                context.Response.StatusCode = 404;
                return context.Response.WriteAsync("Nenhum livro cadastrado.");
            }

            context.Response.ContentType = "text/html; charset=utf-8";

            var html = $@"
            <!DOCTYPE html>
            <html lang='pt-BR'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Detalhes da Obra</title>
                <style>
                    :root {{ --bg: #f4f4f5; --surface: #ffffff; --text: #18181b; --text-muted: #71717a; --primary: #09090b; --border: #e4e4e7; }}
                    body {{ font-family: -apple-system, BlinkMacSystemFont, 'Inter', 'Segoe UI', Roboto, sans-serif; background-color: var(--bg); color: var(--text); margin: 0; padding: 40px 20px; display: flex; justify-content: center; }}
                    .container {{ background: var(--surface); padding: 40px; border-radius: 12px; border: 1px solid var(--border); box-shadow: 0 4px 24px rgba(0,0,0,0.04); width: 100%; max-width: 650px; }}
                    .header {{ border-bottom: 1px solid var(--border); padding-bottom: 20px; margin-bottom: 24px; }}
                    h1 {{ font-size: 1.75rem; margin: 0 0 8px 0; font-weight: 600; letter-spacing: -0.02em; }}
                    .stats-grid {{ display: flex; gap: 16px; margin-bottom: 32px; }}
                    .stat-box {{ background: var(--bg); padding: 16px; border-radius: 8px; flex: 1; border: 1px solid var(--border); }}
                    .stat-label {{ font-size: 0.75rem; color: var(--text-muted); text-transform: uppercase; letter-spacing: 0.05em; margin-bottom: 4px; display: block; }}
                    .stat-value {{ font-size: 1.25rem; font-weight: 600; }}
                    h2 {{ font-size: 1.1rem; margin-top: 0; margin-bottom: 16px; font-weight: 500; }}
                    table {{ width: 100%; border-collapse: collapse; }}
                    th, td {{ padding: 14px 16px; text-align: left; border-bottom: 1px solid var(--border); font-size: 0.95rem; }}
                    th {{ color: var(--text-muted); font-weight: 500; background: var(--bg); }}
                    tr:last-child td {{ border-bottom: none; }}
                    .btn {{ display: inline-block; width: 100%; text-align: center; margin-top: 32px; padding: 14px; background-color: var(--primary); color: white; text-decoration: none; border-radius: 6px; font-weight: 500; font-size: 0.95rem; transition: opacity 0.2s; box-sizing: border-box; }}
                    .btn:hover {{ opacity: 0.9; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>{livro.Titulo}</h1>
                    </div>
                    
                    <div class='stats-grid'>
                        <div class='stat-box'>
                            <span class='stat-label'>Preço de Venda</span>
                            <span class='stat-value'>{(livro.Preco).ToString("C")}</span>
                        </div>
                        <div class='stat-box'>
                            <span class='stat-label'>Estoque Disponível</span>
                            <span class='stat-value'>{livro.Quantidade} un.</span>
                        </div>
                    </div>

                    <h2>Autoria</h2>
                    <table>
                        <thead>
                            <tr>
                                <th>Nome Completo</th>
                                <th>Contato (E-mail)</th>
                                <th>Gênero</th>
                            </tr>
                        </thead>
                        <tbody>";

            foreach (var autor in livro.Autores)
            {
                html += $@"
                            <tr>
                                <td>{autor.Nome}</td>
                                <td>{autor.Email}</td>
                                <td>{autor.Genero}</td>
                            </tr>";
            }

            html += @"
                        </tbody>
                    </table>
                    <a href='/livro' class='btn'>Cadastrar Nova Obra</a>
                </div>
            </body>
            </html>";

            return context.Response.WriteAsync(html);
        }

        public async Task RotaCadastroLivro(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get)
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                var html = @"
                <!DOCTYPE html>
                <html lang='pt-BR'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Registro de Livro</title>
                    <style>
                        :root { --bg: #f4f4f5; --surface: #ffffff; --text: #18181b; --primary: #09090b; --border: #e4e4e7; --focus: #a1a1aa; --danger: #ef4444; }
                        body { font-family: -apple-system, BlinkMacSystemFont, 'Inter', 'Segoe UI', Roboto, sans-serif; background-color: var(--bg); color: var(--text); margin: 0; padding: 40px 20px; display: flex; justify-content: center; }
                        .form-container { background: var(--surface); padding: 40px; border-radius: 12px; border: 1px solid var(--border); box-shadow: 0 4px 24px rgba(0,0,0,0.04); width: 100%; max-width: 550px; }
                        h2 { font-size: 1.5rem; margin-top: 0; margin-bottom: 24px; font-weight: 600; letter-spacing: -0.02em; }
                        .form-group { margin-bottom: 16px; }
                        .row { display: flex; gap: 16px; margin-bottom: 16px; }
                        .row .form-group { margin-bottom: 0; flex: 1; }
                        label { font-size: 0.85rem; font-weight: 500; display: block; margin-bottom: 6px; }
                        input, select { width: 100%; padding: 12px; border: 1px solid var(--border); border-radius: 6px; font-size: 0.95rem; font-family: inherit; box-sizing: border-box; transition: border-color 0.2s; background: #fff; }
                        input:focus, select:focus { outline: none; border-color: var(--focus); }
                        .section-title { font-size: 0.85rem; text-transform: uppercase; letter-spacing: 0.05em; color: #71717a; margin: 32px 0 16px 0; border-bottom: 1px solid var(--border); padding-bottom: 8px; display: flex; justify-content: space-between; align-items: center; }
                        
                        /* Author Cards */
                        #authors-container { display: flex; flex-direction: column; gap: 16px; }
                        .author-card { background: var(--bg); border: 1px solid var(--border); padding: 20px; border-radius: 8px; position: relative; }
                        .btn-remove { position: absolute; top: 16px; right: 20px; background: none; border: none; color: var(--danger); font-size: 0.8rem; font-weight: 600; cursor: pointer; padding: 0; }
                        .btn-remove:hover { text-decoration: underline; }
                        
                        /* Buttons */
                        .btn-add { background: none; border: 1px dashed var(--border); color: var(--text); padding: 12px; border-radius: 6px; cursor: pointer; font-size: 0.9rem; font-weight: 500; width: 100%; margin-top: 12px; transition: background 0.2s; }
                        .btn-add:hover { background: #e4e4e7; }
                        .btn-submit { width: 100%; background: var(--primary); color: white; padding: 14px; border: none; border-radius: 6px; cursor: pointer; font-size: 0.95rem; font-weight: 500; margin-top: 32px; transition: opacity 0.2s; }
                        .btn-submit:hover { opacity: 0.9; }
                    </style>
                </head>
                <body>
                    <div class='form-container'>
                        <h2>Registro de Obra</h2>
                        <form method='POST' action='/livro'>
                            
                            <div class='section-title'>Dados do Livro</div>
                            <div class='form-group'>
                                <label>Título da Obra</label>
                                <input type='text' name='titulo' required autocomplete='off' />
                            </div>
                            <div class='row'>
                                <div class='form-group'>
                                    <label>Preço</label>
                                    <input type='text' name='preco' id='precoInput' required placeholder='R$ 0,00' oninput='maskCurrency(this)' />
                                </div>
                                <div class='form-group'>
                                    <label>Quantidade</label>
                                    <input type='number' name='quantidade' min='0' step='1' required oninput=""this.value = this.value.replace(/[^0-9]/g, '')"" />
                                </div>
                            </div>
                            
                            <div class='section-title'>Autoria</div>
                            <div id='authors-container'>
                                <!-- Autor 1 Inicial (Fixo) -->
                                <div class='author-card' id='author-1'>
                                    <div class='form-group'>
                                        <label>Nome Completo</label>
                                        <input type='text' name='autorNome' required autocomplete='name' />
                                    </div>
                                    <div class='row'>
                                        <div class='form-group'>
                                            <label>E-mail Institucional</label>
                                            <input type='email' name='autorEmail' required autocomplete='email' />
                                        </div>
                                        <div class='form-group' style='max-width: 120px;'>
                                            <label>Gênero</label>
                                            <select name='autorGenero' required>
                                                <option value='M'>Masc (M)</option>
                                                <option value='F'>Fem (F)</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <button type='button' class='btn-add' onclick='addAuthor()'>+ Adicionar Coautor</button>
                            <button type='submit' class='btn-submit'>Processar Registro Completo</button>
                        </form>
                    </div>

                    <script>
                        // Máscara de Moeda Brasileira
                        function maskCurrency(input) {
                            let value = input.value.replace(/\D/g, '');
                            if (value === '') { input.value = ''; return; }
                            value = (parseInt(value) / 100).toFixed(2) + '';
                            value = value.replace('.', ',');
                            value = value.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
                            input.value = 'R$ ' + value;
                        }

                        // Dinâmica de Adição de Múltiplos Autores
                        let authorCount = 1;
                        function addAuthor() {
                            authorCount++;
                            const container = document.getElementById('authors-container');
                            const html = `
                                <div class='author-card' id='author-${authorCount}'>
                                    <button type='button' class='btn-remove' onclick='removeAuthor(${authorCount})'>Remover</button>
                                    <div class='form-group'>
                                        <label>Nome Completo</label>
                                        <input type='text' name='autorNome' required />
                                    </div>
                                    <div class='row'>
                                        <div class='form-group'>
                                            <label>E-mail Institucional</label>
                                            <input type='email' name='autorEmail' required />
                                        </div>
                                        <div class='form-group' style='max-width: 120px;'>
                                            <label>Gênero</label>
                                            <select name='autorGenero' required>
                                                <option value='M'>Masc (M)</option>
                                                <option value='F'>Fem (F)</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>`;
                            container.insertAdjacentHTML('beforeend', html);
                        }

                        function removeAuthor(id) {
                            const element = document.getElementById(`author-${id}`);
                            if (element) { element.remove(); }
                        }
                    </script>
                </body>
                </html>";
                await context.Response.WriteAsync(html);
            }
            else if (context.Request.Method == HttpMethods.Post)
            {
                var form = await context.Request.ReadFormAsync();
                var titulo = form["titulo"].ToString();
                var quantidade = Convert.ToInt32(form["quantidade"]);
                var precoStr = form["preco"].ToString().Replace("R$", "").Replace(".", "").Replace(",", ".").Trim();
                double.TryParse(precoStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double preco);
                var nomes = form["autorNome"];
                var emails = form["autorEmail"];
                var generos = form["autorGenero"];
                var listaAutores = new List<Autor>();

                for (int i = 0; i < nomes.Count; i++)
                {
                    char genero = string.IsNullOrEmpty(generos[i]) ? 'N' : generos[i].ToString().ToUpper()[0];
                    listaAutores.Add(new Autor(nomes[i], emails[i], genero));
                }

                var novoLivro = new Livro(titulo, listaAutores.ToArray(), preco, quantidade);
                var repo = new LivroRepositorioSQLite();
                repo.Incluir(novoLivro);
                context.Response.Redirect("/livro/ApresentarLivro");
            }
        }

        public Task Roteamento(HttpContext context)
        {
            var caminhosAtendidos = new Dictionary<string, RequestDelegate>
            {
                {"/livro", RotaCadastroLivro},
                {"/livro/nome", RotaNomeLivro},
                {"/livro/detalhes", RotaToString},
                {"/livro/autores", RotaNomesAutores},
                {"/livro/ApresentarLivro", RotaApresentarLivroHTML}
            };

            if (caminhosAtendidos.ContainsKey(context.Request.Path))
            {
                var metodo = caminhosAtendidos[context.Request.Path];
                return metodo.Invoke(context);
            }

            context.Response.StatusCode = 404;
            return context.Response.WriteAsync("Caminho Inexistente - TP01");
        }
    }
}