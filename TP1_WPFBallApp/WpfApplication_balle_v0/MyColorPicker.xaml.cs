using CustomControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfApplication_balle_v0
{
    /// <summary>
    /// Logique d'interaction pour MyColorPicker.xaml
    /// </summary>
    public partial class MyColorPicker : Window
    {
        public MyColorPicker(Color currentColor)
        {
            InitializeComponent();
            colorPicker.Color = currentColor;
        }

        private void cmdGet_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void cmdSet_Click(object sender, RoutedEventArgs e)
        {
            colorPicker.Color = Colors.Beige;
        }

        private void colorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (lblColor != null) lblColor.Text = "The new color is " + e.NewValue.ToString();
        }

        
    }
}
