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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CAROIL.Class;
using CAROIL.View;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

// ReSharper disable once CheckNamespace
namespace CAROIL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region declare

        public static string InUser;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // load config
            OseMySql.Config.Load();
            LabelClientNome.Content = string.Empty;
            LblUsuario.Content = InUser;
            CanvasNovaOs.Visibility =Visibility.Hidden;
        }

        private void CmdNova_OnClick(object sender, RoutedEventArgs e)
        {
            CanvasNovaOs.Visibility = Visibility.Visible;
            CanvasMain.IsEnabled = false;

            TxtBoxNumOs.Text = OseMySql.RetornaUltimaOs().ToString();
        }

        private void CmdRetorna_OnClick(object sender, RoutedEventArgs e)
        {
            CanvasNovaOs.Visibility = Visibility.Hidden;
            CanvasMain.IsEnabled = true;
            TxtNumOs.Text = string.Empty;
            TxtCpfCnpj.Text = string.Empty;
            LabelClientNome.Content = string.Empty;
        }

        public Clientes MyClient;
        private async void CmdConsultaCli_OnClick(object sender, RoutedEventArgs e)
        {
            MyClient = null;
            if (TxtCpfCnpj.Text.Length == 14 || TxtCpfCnpj.Text.Length == 11)
            {
                foreach (Clientes c in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.CpfCnpj, TxtCpfCnpj.Text.Trim()))
                {
                    if (c != null)
                    {
                        MyClient = new Clientes()
                        {
                            Id = c.Id,
                            Nome = c.Nome,
                            CpfCnpj = c.CpfCnpj,
                            Telefone = c.Telefone
                        };
                        TxtCpfCnpj.Text = MyClient.CpfCnpj;
                        LabelClientNome.Content = MyClient.Nome;
                    }
                    //else
                    //{
                    //    var result = await this.ShowMessageAsync("Cadastro Cliente", "Deseja cadastrar novo cliente ?", MessageDialogStyle.AffirmativeAndNegative);
                    //    if (result == MessageDialogResult.Affirmative)
                    //    {
                    //        // novo usuario
                    //        CliNovo window = new CliNovo()
                    //        {
                    //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    //            ShowTitleBar = false,
                    //            IsWindowDraggable = true,
                    //            TitleCaps = false,
                    //            GlowBrush = new SolidColorBrush(Colors.Black),
                    //            ShowMaxRestoreButton = false,
                    //            ResizeMode = ResizeMode.CanMinimize
                    //        };
                    //        window.ShowDialog();
                    //    }
                    //}
                }
                if (MyClient == null)
                {
                    var result = await this.ShowMessageAsync("Cadastro Cliente", "Deseja cadastrar novo cliente ?", MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Affirmative)
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
                        CliNovo.CpfCnpj = TxtCpfCnpj.Text;
                        window.ShowDialog();
                        CliNovo.CpfCnpj = string.Empty;

                        foreach (Clientes c in OseMySql.RetornaListaClientes(OseMySql.PesquisaTipo.CpfCnpj, TxtCpfCnpj.Text.Trim()))
                        {
                            MyClient = new Clientes()
                            {
                                Id = c.Id,
                                Nome = c.Nome,
                                CpfCnpj = c.CpfCnpj,
                                Telefone = c.Telefone
                            };
                        }

                    }
                }
            }
            else if(TxtCpfCnpj.Text.Trim() == string.Empty)
            {
                CliConsulta window = new CliConsulta()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ShowTitleBar = false,
                    IsWindowDraggable = true,
                    TitleCaps = false,
                    GlowBrush = new SolidColorBrush(Colors.Black),
                    ShowMaxRestoreButton = false,
                    ResizeMode = ResizeMode.CanMinimize
                };
                MyClient = new Clientes();
                window.ShowDialog();
                TxtCpfCnpj.Text = MyClient.CpfCnpj;

                LabelClientNome.Content = MyClient.Nome;
            }
            //else
            //{
            //    var result = await this.ShowMessageAsync("Cadastro Cliente","Deseja cadastrar novo cliente ?",MessageDialogStyle.AffirmativeAndNegative);
            //    if (result == MessageDialogResult.Affirmative)
            //    {
            //        // novo usuario
            //        CliNovo window = new CliNovo()
            //        {
            //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //            ShowTitleBar = false,
            //            IsWindowDraggable = true,
            //            TitleCaps = false,
            //            GlowBrush = new SolidColorBrush(Colors.Black),
            //            ShowMaxRestoreButton = false,
            //            ResizeMode = ResizeMode.CanMinimize
            //        };
            //        window.ShowDialog();
            //    }
            //}
            if (MyClient != null)
            {
                TxtCpfCnpj.Text = MyClient.CpfCnpj;
                LabelClientNome.Content = MyClient.Nome;
            }
            else
            {
                TxtCpfCnpj.Text = string.Empty;
                LabelClientNome.Content = string.Empty;
            }
        }

        private void TxtCpfCnpj_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtCpfCnpj.Text.Trim() == string.Empty || TxtCpfCnpj.Text.Length != 11 || TxtCpfCnpj.Text.Length != 14 )
            {
                //MyClient = null;
                LabelClientNome.Content = string.Empty;
            }
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    if (CanvasNovaOs.Visibility == Visibility.Visible)
                    {
                        CmdRetorna_OnClick(null, null);
                    }
                    else
                    {
                        Close();
                    }
                    break;
            }
        }

        private void CmdConsultaPlaca_OnClick(object sender, RoutedEventArgs e)
        {
            if (TxtVeiculoPlaca.Text != string.Empty)
            {
                // consulta se existe 
                // cria novo
            }
            else
            {
               // abre tela de consulta
               var window = new VeiculoConsulta()
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
            }
        }
    }
}
