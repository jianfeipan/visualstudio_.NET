using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

using System.Windows;
using CustomControls;
using System.ComponentModel;
using System.Diagnostics;

namespace WpfApplication_balle_v0
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Storyboard board;
        MyColorPicker colorPicker;
        double speedRadio = 1;
        public MainWindow()
        {
            InitializeComponent();
            board = (Storyboard)mainGrid.FindResource("animations");
        }


        public void MenuItemStart_Click(object Sender, EventArgs e)
        {
            board.Begin(this, true);
        }
        public void MenuItemStop_Click(object Sender, EventArgs e) {
            board.Stop(this);
        }

        public void MenuItemColorDiagram_click(object Sender, EventArgs e) {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                System.Drawing.Color color = cd.Color;             
                board.Children[0].SetValue(ColorAnimation.ToProperty, System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            board.Begin(this, true);
            board.SetSpeedRatio(this, speedRadio);
        }

        public void MenuItemColorPicker_click(object Sender, EventArgs e) {
            colorPicker = new MyColorPicker((Color)board.Children[0].GetValue(ColorAnimation.ToProperty));
            if (colorPicker.ShowDialog() == true){
                board.Children[0].SetValue(ColorAnimation.ToProperty, colorPicker.colorPicker.Color);
                board.Begin(this, true);
                board.SetSpeedRatio(this, speedRadio);
            }           
        }

        public void MenuItemExit_click(object Sender, EventArgs e) {
            this.Close();
        }

        public void SpeedSlow_click(object Sender, EventArgs e) {
            board.SetSpeedRatio(this, 0.3);
            speedRadio = 0.3;
        }
        public void SpeedNormal_click(object Sender, EventArgs e)
        {
            board.SetSpeedRatio(this, 1);
            speedRadio = 1;
        }

        public void SpeedFast_click(object Sender, EventArgs e)
        {
            board.SetSpeedRatio(this, 3);
            speedRadio = 3;
        }

    }
}
