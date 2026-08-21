using CBTSWE2.Aula01.Negocio;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTSWE2.Aula01.Repositorio
{
    public class LivroRepositorioSQLite : ILivroRepositorio
    {
        private readonly string _connectionString = "Data Source=banco.db";

        public LivroRepositorioSQLite()
        {
            CriarBancoDeDados();
        }

        private void CriarBancoDeDados()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Livros (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo TEXT NOT NULL,
                    Preco REAL NOT NULL,
                    Quantidade INTEGER NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS Autores (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Genero TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS LivroAutores (
                    LivroId INTEGER,
                    AutorId INTEGER,
                    FOREIGN KEY(LivroId) REFERENCES Livros(Id),
                    FOREIGN KEY(AutorId) REFERENCES Autores(Id)
                );
            ";
            command.ExecuteNonQuery();
        }

        public void Incluir(Livro livro)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var cmdLivro = connection.CreateCommand();
                cmdLivro.CommandText = "INSERT INTO Livros (Titulo, Preco, Quantidade) VALUES ($titulo, $preco, $qtd); SELECT last_insert_rowid();";
                cmdLivro.Parameters.AddWithValue("$titulo", livro.Titulo);
                cmdLivro.Parameters.AddWithValue("$preco", livro.Preco);
                cmdLivro.Parameters.AddWithValue("$qtd", livro.Quantidade);
                var livroId = (long)cmdLivro.ExecuteScalar();

                foreach (var autor in livro.Autores)
                {
                    var cmdAutor = connection.CreateCommand();
                    cmdAutor.CommandText = "INSERT INTO Autores (Nome, Email, Genero) VALUES ($nome, $email, $genero); SELECT last_insert_rowid();";
                    cmdAutor.Parameters.AddWithValue("$nome", autor.Nome);
                    cmdAutor.Parameters.AddWithValue("$email", autor.Email);
                    cmdAutor.Parameters.AddWithValue("$genero", autor.Genero.ToString());
                    var autorId = (long)cmdAutor.ExecuteScalar();

                    var cmdAssoc = connection.CreateCommand();
                    cmdAssoc.CommandText = "INSERT INTO LivroAutores (LivroId, AutorId) VALUES ($livroId, $autorId)";
                    cmdAssoc.Parameters.AddWithValue("$livroId", livroId);
                    cmdAssoc.Parameters.AddWithValue("$autorId", autorId);
                    cmdAssoc.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public IEnumerable<Livro> Todos()
        {
            var livros = new List<Livro>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT l.Id, l.Titulo, l.Preco, l.Quantidade, a.Nome, a.Email, a.Genero 
                FROM Livros l
                LEFT JOIN LivroAutores la ON l.Id = la.LivroId
                LEFT JOIN Autores a ON la.AutorId = a.Id
            ";

            using var reader = cmd.ExecuteReader();
            var dicionarioLivros = new Dictionary<long, (string Titulo, double Preco, int Qtd, List<Autor> Autores)>();

            while (reader.Read())
            {
                var id = reader.GetInt64(0);
                if (!dicionarioLivros.ContainsKey(id))
                {
                    dicionarioLivros[id] = (reader.GetString(1), reader.GetDouble(2), reader.GetInt32(3), new List<Autor>());
                }

                if (!reader.IsDBNull(4))
                {
                    var autor = new Autor(reader.GetString(4), reader.GetString(5), reader.GetString(6)[0]);
                    dicionarioLivros[id].Autores.Add(autor);
                }
            }

            foreach (var item in dicionarioLivros.Values)
            {
                livros.Add(new Livro(item.Titulo, item.Autores.ToArray(), item.Preco, item.Qtd));
            }

            return livros;
        }
    }
}