using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTSWE2.Aula01.Negocio
{
    public class Livro
    {
        public string Titulo { get; private set; }
        public Autor[] Autores { get; private set; }
        public double Preco { get; set; }
        public int Quantidade { get; set; }

        public Livro(string titulo, Autor[] autores, double preco, int quantidade = 0)
        {
            Titulo = titulo;
            Autores = autores;
            Preco = preco;
            Quantidade = quantidade;
        }

        public string ObterNomesAutores()
        {
            return string.Join(", ", Autores.Select(a => a.Nome));
        }

        public override string ToString()
        {
            var autoresFormatados = string.Join(", ", Autores.Select(a => $"Autor[nome={a.Nome},email={a.Email},genero={a.Genero}]"));
            return $"Livro[nome={Titulo},autores={{{autoresFormatados}}},preco={Preco},qtd={Quantidade}]";
        }
    }
}
