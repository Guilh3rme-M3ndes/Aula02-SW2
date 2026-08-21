using CBTSWE2.Aula01.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTSWE2.Aula01.Repositorio
{
    public interface ILivroRepositorio
    {
        void Incluir(Livro livro);
        IEnumerable<Livro> Todos();
    }
}
