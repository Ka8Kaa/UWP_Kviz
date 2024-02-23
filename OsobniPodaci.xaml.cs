﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Kviz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OsobniPodaci : Page
    {
        public OsobniPodaci()
        {
            this.InitializeComponent();
        }
        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Zaigraj_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(OIBunos.Text) || string.IsNullOrEmpty(Imeunos.Text) || string.IsNullOrEmpty(Prezimeunos.Text))
            {
                ErrorTextbox.Text = "Ups! Nedostaju podaci!";
            }
            else
            {
                Frame.Navigate(typeof(Kviz));
            }
        }
    }
}