using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CAROIL.Class;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
namespace CAROIL.View
{
    /// <summary>
    /// Interaction logic for CliNovo.xaml
    /// </summary>
    public partial class CliNovo : MetroWindow
    {
        public static string CpfCnpj;
        public static Clientes MyCliente { get; set; }
        public CliNovo()
        {
            InitializeComponent();
        }

        private void CliNovo_OnLoaded(object sender, RoutedEventArgs e)
        {
            CmdExluir.IsEnabled = false;

            if (CpfCnpj != null)
            {
                TxtCpfCnpj.Text = CpfCnpj;
            }
            if (MyCliente != null)
            {
                // Editar usuario
                ClienteLoadEdit edit = OseMySql.EditarCliente(MyCliente.CpfCnpj);
                if (edit != null)
                {
                    CmdExluir.IsEnabled = true;
                    TxtCpfCnpj.Text = edit.CpfCnpj;
                    TxtNome.Text = edit.Nome;
                    TxtTelefone.Text = edit.Telefone;
                    TxtCelular.Text = edit.Celular;
                    TxtEmail.Text = edit.Email;
                    TxtEndereco.Text = edit.Endereco;
                    TxtBairro.Text = edit.Bairro;
                    TxtObs.Text = edit.Obs;
                    if (edit.DataNasc.Length == 8)
                    {
                        TxtDataNasc.Text = edit.DataNasc.Substring(0, 2) + "/" + edit.DataNasc.Substring(2, 2) + "/" +
                                       edit.DataNasc.Substring(4, 4);
                    }
                    
                }
            }
        }

        private void CliNovo_OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;                   
            }
        }

        private async void CmdConfirma_OnClick(object sender, RoutedEventArgs e)
        {
            if (MyCliente != null)
            {
                // update cliente
                ClienteLoadEdit c = new ClienteLoadEdit()
                {
                    Id = OseMySql.ReturnIdCliente(TxtCpfCnpj.Text.Trim()),
                    CpfCnpj = TxtCpfCnpj.Text,
                    Nome = TxtNome.Text,
                    Telefone = TxtTelefone.Text,
                    Email = TxtEmail.Text,
                    Celular = TxtCelular.Text,
                    Endereco = TxtEndereco.Text,
                    Bairro = TxtBairro.Text,
                    Obs = TxtObs.Text,
                    DataNasc = TxtDataNasc.Text.Replace("/",string.Empty)
                };
                if (OseMySql.UpdateCliente(c) != 0)
                {
                    await this.ShowMessageAsync("Falha", "Falha ao Atualizar Cliente . . .");
                }
                await this.ShowMessageAsync("Sucesso", "Sucesso ao Atualizar Cliente . . .");
                Close();
                return;
            }

            if (TxtCpfCnpj.Text.Trim() != string.Empty && TxtNome.Text.Trim() != string.Empty &&
                TxtTelefone.Text.Trim() != string.Empty)
                //TxtEmail.Text.Trim() != string.Empty && TxtEndereco.Text.Trim() != string.Empty &&
                //TxtBairro.Text.Trim() != string.Empty)
            {
                if (OseMySql.AdcionarNovoCliente(TxtCpfCnpj.Text.Trim(), TxtNome.Text, TxtTelefone.Text.Trim(),
                        TxtEmail.Text.Trim(), TxtCelular.Text.Trim(), TxtEndereco.Text, TxtBairro.Text, TxtObs.Text, TxtDataNasc.Text.Replace("/", string.Empty)) !=0)
                {
                    await this.ShowMessageAsync("Falha", "Cpf/Cnpj ja cadastrado . . .");
                    return;    
                }
                await this.ShowMessageAsync("Sucesso", "Cadastro efetuado com sucesso . . .");
                Close();
            }
        }

        private async void CmdExluir_OnClick(object sender, RoutedEventArgs e)
        {
            if(MyCliente != null)
            OseMySql.DeletaCliente(MyCliente.CpfCnpj);
            await this.ShowMessageAsync("Sucesso", "Cliente Excluido . . .");
            Close();
        }

        private void TxtDataNasc_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                switch (TxtDataNasc.Text.Replace("/",string.Empty).Length)
                {
                    case 2:
                        TxtDataNasc.Text = TxtDataNasc.Text.Substring(0, 2) + "/";
                        TxtDataNasc.SelectionStart = 3;
                        break;

                    case 4:
                        TxtDataNasc.Text = TxtDataNasc.Text.Substring(0, 2) + "/" + TxtDataNasc.Text.Substring(3, 2) + "/";
                        TxtDataNasc.SelectionStart = 6;
                        break;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
