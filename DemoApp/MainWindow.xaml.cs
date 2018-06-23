using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Custom.Generic;
using System.Globalization;
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

namespace DemoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        private MainWindowViewModel windowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            windowViewModel = new MainWindowViewModel();
            windowViewModel.Progress += ProgressChanged ;
            windowViewModel.Clear += Clear;
            this.DataContext = windowViewModel;

        }

        private void Clear(object sender, EventArgs e)
        {

            MainCanvas.Children.Clear();

        }


        private void ProgressChanged(object sender, ProgressChangedEventArgs<WorkerArgument> e)
        {
            FireflyHelper.Add(MainCanvas, e.UserState.Rand);
        }


    }





}

