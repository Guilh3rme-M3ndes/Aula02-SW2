# 📚 TP01 - Sistemas Web II: Gestão de Livros e Autores

Uma aplicação web minimalista desenvolvida em ASP.NET Core (Kestrel) para o gerenciamento de Livros e seus respectivos Autores. O projeto demonstra os fundamentos de roteamento HTTP, injeção de pipeline, persistência de dados e renderização manual de páginas HTML diretamente pelo backend, sem o uso de frameworks de alto nível como MVC ou Razor.

## 🎯 Objetivos
- Compreender o ciclo de vida de uma requisição HTTP via pipeline (`IApplicationBuilder` e `HttpContext`).
- Desenvolver um sistema de roteamento manual de URIs utilizando `Dictionary<string, RequestDelegate>`.
- Transcrever um modelo arquitetural estático (Diagrama UML) para código em C# moderno (utilizando *Properties* e construtores padronizados).
- Implementar persistência de dados relacional com SQLite via ADO.NET (consultas transacionais e comandos parametrizados).
- Construir interfaces web amigáveis e responsivas com HTML5 e CSS3 nativos.

## 🧠 Arquitetura e Lógica Aplicada (UML)
O projeto baseou-se em um diagrama UML específico que modela a relação 1:N (Agregação) entre **Livro** (*Book*) e **Autor** (*Author*).

- **Domínio:** A classe `Livro` possui uma coleção de objetos `Autor`. Os métodos exigidos pelo diagrama (`getAuthorNames()` e `toString()`) foram adaptados utilizando LINQ para máxima eficiência e legibilidade no C#.
- **Normalização (Banco de Dados):** Embora o domínio trabalhe com arrays/listas, a camada de Repositório (`LivroRepositorioSQLite`) converte essa estrutura para um modelo relacional normalizado (3FN) composto por 3 tabelas:
  - `Livros`: Armazena os dados primários da obra (Título, Preço, Quantidade).
  - `Autores`: Armazena informações dos criadores (Nome, E-mail, Gênero).
  - `LivroAutores`: Tabela associativa que resolve a relação e previne orfandade de dados.

## 🛠️ Ferramentas e Tecnologias
- **Linguagem:** C# 12
- **Framework:** .NET 8 (SDK Web)
- **Servidor Web:** Kestrel
- **Banco de Dados:** SQLite (via pacote `Microsoft.Data.Sqlite`)
- **Frontend:** HTML5, CSS3, Vanilla JavaScript (Validações e Máscaras)
- **IDE Recomendada:** Visual Studio 2022

## 🛣️ Rotas da Aplicação (Endpoints)
A aplicação atende aos seguintes caminhos configurados no `Startup.cs`:

| Verbo | Rota | Descrição | Content-Type |
|-------|------|-----------|--------------|
| **GET** | `/livro` | Interface HTML (Formulário) para cadastro de um Livro e múltiplos Autores simultaneamente. | `text/html` |
| **POST** | `/livro` | Endpoint que recebe os dados do form, realiza a sanitização e persiste no banco SQLite. | `-` |
| **GET** | `/livro/ApresentarLivro` | Interface HTML moderna em formato Dashboard exibindo os detalhes do último livro cadastrado e a lista de seus autores. | `text/html` |
| **GET** | `/livro/nome` | Retorna o título da obra mais recente cadastrada. *(Exigência B1)* | `text/plain` |
| **GET** | `/livro/detalhes` | Retorna a formatação exata exigida do método `ToString()`. *(Exigência B2)* | `text/plain` |
| **GET** | `/livro/autores` | Retorna a lista de autores em formato string separada por vírgula. *(Exigência B3)* | `text/plain` |

## 📁 Estrutura de Diretórios
```text
CBTSWE2.Aula01/
│
├── Negocio/                      # Camada de Domínio
│   ├── Autor.cs                  # Entidade de Autor
│   └── Livro.cs                  # Entidade de Livro contendo as regras de negócio UML
│
├── Repositorio/                  # Camada de Acesso a Dados (Data Access Layer)
│   ├── ILivroRepositorio.cs      # Contrato/Interface do repositório
│   └── LivroRepositorioSQLite.cs # Implementação ADO.NET (Criação de tabelas e transações)
│
├── Program.cs                    # Ponto de entrada, rotina de Seed (popular banco) e Host Kestrel
├── Startup.cs                    # Roteamento HTTP, Views HTML embutidas e processamento de requests
├── CBTSWE2.Aula01.csproj         # Configurações do SDK do Projeto e dependências NuGet
└── banco.db                      # Banco de dados gerado automaticamente na primeira execução

```

## 🚀 Como executar o projeto localmente

Siga as etapas abaixo para compilar e executar o projeto utilizando o Git e o Visual Studio 2022:

**1. Clone o repositório:**

```bash
git clone <url-do-seu-repositorio>

```

**2. Abra o projeto:**

* Inicie o Visual Studio 2022.
* Selecione `Arquivo > Abrir > Projeto/Solução...` e escolha o arquivo `CBTSWE2.Aula01.csproj` (ou `.sln`).

**3. Restaure as Dependências:**

* O projeto requer o pacote SQLite.
* No Visual Studio, abra o terminal (`Exibir > Terminal`) e execute:

```bash
dotnet add package Microsoft.Data.Sqlite

```

*(Alternativamente: Clique com o botão direito no projeto > Gerenciar Pacotes NuGet > Procure e instale `Microsoft.Data.Sqlite`).*

**4. Execute a aplicação:**

* Pressione `F5` (ou clique no botão "Iniciar" / `dotnet run`).
* O console do Kestrel será aberto exibindo a porta local escolhida (ex: `http://localhost:5000`).
* Abra seu navegador de preferência e acesse o link acompanhado de `/livro` para iniciar o fluxo.

## 👨‍💻 Autores

**Stiven Richardy Silva Rodrigues**
*Estudante de Análise e Desenvolvimento de Sistemas | IFSP — Campus Cubatão*
GitHub: [@Stiven-Richardy](https://www.google.com/search?q=https://github.com/Stiven-Richardy)

**Guilherme Mendes de Sousa**
*Estudante de Análise e Desenvolvimento de Sistemas | IFSP — Campus Cubatão*
GitHub: [@Guilh3rme-M3ndes](https://github.com/Guilh3rme-M3ndes)
