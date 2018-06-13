using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace BackgroundWorkerUserControls.Controls
{
    public class ToggleButtonEx:ToggleButton
    {

        private static Style oStyle = null;
        public System.Windows.Controls.Image  MainImage { get; set; }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }



        public ToggleButtonEx() : base()
        {
            Uri resourceLocater = new Uri("/BackgroundWorkerUserControls;component/Themes/ToggleButtonEx.xaml", System.UriKind.Relative);

            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            oStyle = resourceDictionary["ToggleExStyle"] as Style;
            Style = oStyle;

        

        }

        public Uri FrontImageSource
        {
            get { return (Uri)GetValue(FrontImageSourceProperty); }
            set { SetValue(FrontImageSourceProperty, value); }
        }




        private static void OnFrontImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if ((d as ToggleButtonEx).MainImage != null)
            //    (d as ToggleButtonEx).MainImage.Source = new System.Windows.Media.Imaging.BitmapImage((Uri)e.NewValue);

        }

        // the inherits property ensure child elements inherit this property FrameworkPropertyMetadataOptions.Inherits,
        public static readonly DependencyProperty FrontImageSourceProperty =
            DependencyProperty.Register("FrontImageSource", typeof(Uri), typeof(ToggleButtonEx),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnFrontImageSourceChanged)));



        public Uri BackImageSource
        {
            get { return (Uri)GetValue(BackImageSourceProperty); }
            set { SetValue(BackImageSourceProperty, value); }
        }




        private static void OnBackImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            //if ((d as ToggleButtonEx).MainImage != null)
            //    (d as ToggleButtonEx).MainImage.Source = new System.Windows.Media.Imaging.BitmapImage((Uri)e.NewValue);
        }
    


        public static readonly DependencyProperty BackImageSourceProperty =
            DependencyProperty.Register("BackImageSource", typeof(Uri), typeof(ToggleButtonEx),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnBackImageSourceChanged)));

        //"/BackgroundWorkerUserControls;component/Images/Play.png"

    }
}
