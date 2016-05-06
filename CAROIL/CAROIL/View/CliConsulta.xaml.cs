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
using System.Windows.Threading;
using CAROIL.Class;
using MahApps.Metro.Controls;

namespace CAROIL.View
{
    /// <summary>
    /// Interaction logic for CliConsulta.xaml
    /// </summary>
    public partial class CliConsulta : MetroWindow
    {
        #region declare

        #endregion
        public CliConsulta()
        {
            InitializeComponent();
        }

        private void MenuItemDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if(ListViewCliente.SelectedItem == null) { return;}
            Clientes c = (Clientes) ListViewCliente.SelectedItem;
            OseMySql.DeletaCliente(c.CpfCnpj);

            // Lista todos os clientes
            ListViewCliente.Items.Clear();
            foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.Completa))
            {
                ListViewCliente.Items.Add(new Clientes()
                {
                    CpfCnpj = clientes.CpfCnpj,
                    Nome = clientes.Nome,
                    Telefone = clientes.Telefone
                });
            }
            // Conta total de clientes
            LabelTotalCliente.Content = "Total de clientes :" + OseMySql.RetornaTotalClientes().ToString();
        }
        private void MenuItemEditar_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListViewCliente.SelectedItem == null) { return; }
            Clientes c = (Clientes)ListViewCliente.SelectedItem;

            // novo usuario
            CliNovo window = new CliNovo()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowTitleBar = false,
                IsWindowDraggable = true,
                TitleCaps = false,
                GlowBrush = new SolidColorBrush(Colors.Black),
                ShowMaxRestoreButton = false,
                ResizeMode = ResizeMode.CanMinimize
            };
            CliNovo.MyCliente = new Clientes();
            CliNovo.MyCliente = c;
            window.ShowDialog();
            CliNovo.MyCliente = null;

            // Lista todos os clientes
            ListViewCliente.Items.Clear();
            foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.Completa))
            {
                ListViewCliente.Items.Add(new Clientes()
                {
                    CpfCnpj = clientes.CpfCnpj,
                    Nome = clientes.Nome,
                    Telefone = clientes.Telefone
                });
            }
            // Conta total de clientes
            LabelTotalCliente.Content = "Total de clientes :" + OseMySql.RetornaTotalClientes().ToString();
        }

        private void CliConsulta_OnLoaded(object sender, RoutedEventArgs e)
        {
            ListViewCliente.Items.Clear();
            // Lista todos os clientes
            foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.Completa))
            {
                ListViewCliente.Items.Add(new Clientes()
                {
                    CpfCnpj = clientes.CpfCnpj,
                    Nome = clientes.Nome,
                    Telefone = clientes.Telefone
                });
            }

            // Conta total de clientes
            LabelTotalCliente.Content = "Total de clientes :" + OseMySql.RetornaTotalClientes().ToString();
        }

        private void TxtChavePesquisa_OnKeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void TxtChavePesquisa_OnKeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void TxtChavePesquisa_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtChavePesquisa.Text.Trim() == string.Empty)
            {
                // lista todos os cliente
                ListViewCliente.Items.Clear();
                // Lista todos os clientes
                foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.Completa))
                {
                    ListViewCliente.Items.Add(new Clientes()
                    {
                        CpfCnpj = clientes.CpfCnpj,
                        Nome = clientes.Nome,
                        Telefone = clientes.Telefone
                    });
                }
            }
            else
            {
                ListViewCliente.Items.Clear();
                if (RadioNome.IsChecked != null && RadioNome.IsChecked.Value)
                {
                    // pesquisa por nome
                    foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.Nome, TxtChavePesquisa.Text.Trim()))
                    {
                        ListViewCliente.Items.Add(new Clientes()
                        {
                            CpfCnpj = clientes.CpfCnpj,
                            Nome = clientes.Nome,
                            Telefone = clientes.Telefone
                        });
                    }
                }
                else if (RadioCpfCnpj.IsChecked != null && RadioCpfCnpj.IsChecked.Value)
                {
                    // pesquisa por cpf ou cnpj
                    foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.CpfCnpj, TxtChavePesquisa.Text.Trim()))
                    {
                        ListViewCliente.Items.Add(new Clientes()
                        {
                            CpfCnpj = clientes.CpfCnpj,
                            Nome = clientes.Nome,
                            Telefone = clientes.Telefone
                        });
                    }
                }
                else if (RadioEndereco.IsChecked != null && RadioEndereco.IsChecked.Value)
                {
                    // pesquisa por endero
                    foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.Endereco, TxtChavePesquisa.Text))
                    {
                        ListViewCliente.Items.Add(new Clientes()
                        {
                            CpfCnpj = clientes.CpfCnpj,
                            Nome = clientes.Nome,
                            Telefone = clientes.Telefone
                        });
                    }
                }
                else if (RadioTelefone.IsChecked != null && RadioTelefone.IsChecked.Value)
                {
                    // pesquisa por telefone
                    foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.Telefone, TxtChavePesquisa.Text))
                    {
                        ListViewCliente.Items.Add(new Clientes()
                        {
                            CpfCnpj = clientes.CpfCnpj,
                            Nome = clientes.Nome,
                            Telefone = clientes.Telefone
                        });
                    }
                }
            }
        }

        private void MenuItemSelecionar_OnClick(object sender, RoutedEventArgs e)
        {
            if(ListViewCliente.SelectedItem != null)
            ((MainWindow) Application.Current.MainWindow).MyClient = (Clientes) ListViewCliente.SelectedItem;
            Close();
        }

        private void ListViewCliente_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewCliente.SelectedItem != null)
                ((MainWindow)Application.Current.MainWindow).MyClient = (Clientes)ListViewCliente.SelectedItem;
            Close();
        }

        private void CmdNewCliente_OnClick(object sender, RoutedEventArgs e)
        {
            // novo usuario
            CliNovo window = new CliNovo()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowTitleBar = false,
                IsWindowDraggable = true,
                TitleCaps = false,
                GlowBrush = new SolidColorBrush(Colors.Black),
                ShowMaxRestoreButton = false,
                ResizeMode = ResizeMode.CanMinimize
            };
            window.ShowDialog();

            ListViewCliente.Items.Clear();
            // Lista todos os clientes
            foreach (Clientes clientes in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.Completa))
            {
                ListViewCliente.Items.Add(new Clientes()
                {
                    CpfCnpj = clientes.CpfCnpj,
                    Nome = clientes.Nome,
                    Telefone = clientes.Telefone
                });
            }

            // Conta total de clientes
            LabelTotalCliente.Content = "Total de clientes :" + OseMySql.RetornaTotalClientes().ToString();

        }

        private void CliConsulta_OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();

                    break;
            }
        }
    }
}
