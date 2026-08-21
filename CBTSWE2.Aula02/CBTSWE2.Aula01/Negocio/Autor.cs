using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTSWE2.Aula01.Negocio
{
    public class Autor
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public char Genero { get; private set; }

        public Autor(string nome, string email, char genero)
        {
            Nome = nome;
            Email = email;
            Genero = genero;
        }
    }
}