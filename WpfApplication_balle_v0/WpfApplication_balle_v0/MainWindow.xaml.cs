﻿using System;
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
        Storyboard rotation;
        MyColorPicker colorPicker;
        double speedRadio = 1;
        int currentshap = 0;//0:ball, 1:retangle 2 : Phen
        public MainWindow()
        {
            InitializeComponent();
            board = (Storyboard)mainGrid.FindResource("animations");
            rotation = (Storyboard)mainGrid.FindResource("rotationAnimation");
            myRetangle.Visibility = Visibility.Hidden;
            image.Visibility = Visibility.Hidden;
        }


        public void MenuItemStart_Click(object Sender, EventArgs e)
        {
            rotation.Begin(this, true);
            board.Begin(this, true);
        }
        public void MenuItemStop_Click(object Sender, EventArgs e) {
            board.Stop(this);
            rotation.Stop(this);
        }

        public void MenuItemColorDiagram_click(object Sender, EventArgs e) {
            ColorDialog cd = new ColorDialog();
            if (currentshap != 3){//si c'est pas l'image, on va changer le couleur
                if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.Drawing.Color color = cd.Color;
                    board.Children[currentshap].SetValue(ColorAnimation.ToProperty, System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                }
                board.Begin(this, true);
                board.SetSpeedRatio(this, speedRadio);
            }

        }

        public void MenuItemColorPicker_click(object Sender, EventArgs e) {
            colorPicker = new MyColorPicker((Color)board.Children[0].GetValue(ColorAnimation.ToProperty));
            if (currentshap != 3){//si c'est pas l'image, on va changer le couleur
                if (colorPicker.ShowDialog() == true)
                {
                    board.Children[currentshap].SetValue(ColorAnimation.ToProperty, colorPicker.colorPicker.Color);
                    board.Begin(this, true);
                    board.SetSpeedRatio(this, speedRadio);
                }
            }
        }

        public void MenuItemExit_click(object Sender, EventArgs e) {
            this.Close();
        }

        public void SpeedSlow_click(object Sender, EventArgs e) {
            speedRadio = 0.3;
            changeAnimationSpeed(speedRadio);
        }
        public void SpeedNormal_click(object Sender, EventArgs e)
        {
            speedRadio = 1;
            changeAnimationSpeed(speedRadio);

        }

        public void SpeedFast_click(object Sender, EventArgs e)
        {
            speedRadio = 3;
            changeAnimationSpeed(speedRadio);
            
        }

        private void changeAnimationSpeed(double speed)
        {
            rotation.SetSpeedRatio(this, speed);
            board.SetSpeedRatio(this, speed);
        }

        public void FormBall_click(object Sender, EventArgs e) {
            changeVisibility(0);
        }

        public void FormRetang_click(object Sender, EventArgs e){
            changeVisibility(1);
        }

        public void FormPhen_click(object Sender, EventArgs e)
        {
            changeVisibility(2);
        }

        private void changeVisibility(int shap) {
            if (currentshap != shap) {
                if (shap == 0) {
                    currentshap = 0;
                    myEllipse.Visibility = Visibility.Visible;
                    myRetangle.Visibility = Visibility.Hidden;
                    image.Visibility = Visibility.Hidden;
                    board.RepeatBehavior = RepeatBehavior.Forever;
                    board.AutoReverse = true;
                    board.Begin(this,true);
                } else if (shap == 1) {
                    currentshap = 1;
                    myEllipse.Visibility = Visibility.Hidden;
                    myRetangle.Visibility = Visibility.Visible;
                    image.Visibility = Visibility.Hidden;
                    board.RepeatBehavior = RepeatBehavior.Forever;
                    board.AutoReverse = true;
                    board.Begin(this, true);
                } else{
                    currentshap = 2;
                    myEllipse.Visibility = Visibility.Hidden;
                    myRetangle.Visibility = Visibility.Hidden;
                    image.Visibility = Visibility.Visible;
                    board.RepeatBehavior = new RepeatBehavior(1.0);
                    board.AutoReverse = false;
                    board.Begin(this, true);
                }
            }
        }

        public void sliderspeed_mousedown(object Sender, EventArgs e)
        {
            speedRadio = speedSlider.Value;
            changeAnimationSpeed(speedRadio);
        }
       

    }
}
