using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAROIL.Class
{
    public class UListViewModel
    {
        /// <summary>
        /// modelo listview cliente
        /// </summary>
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }

    public class Clientes
    {
        public int Id { get; set; }
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }

    public class ClienteLoadEdit
    {
        public int Id { get; set; }
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Obs { get; set; }
        public string DataNasc { get; set; }
    }
}
