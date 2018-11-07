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

namespace DemoApp
{
   
    /// <summary>
    /// Interaction logic for MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        private MainViewModel windowViewModel;

        public MainUserControl()
        {
            InitializeComponent();
            windowViewModel = new MainViewModel();
            //windowViewModel.Progress += ProgressChanged;
            //windowViewModel.Clear += Clear;
            this.DataContext = windowViewModel;

        }

        //private void Clear(object sender, EventArgs e)
        //{

        //    MainCanvas.Children.Clear();

        //}


        //private void ProgressChanged(object sender, ProgressChangedEventArgs<WorkerArgument> e)
        //{
        //    FireflyHelper.Add(MainCanvas, e.UserState.Rand);
        //}



    }
}



