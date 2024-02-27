using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
        private int playerCount = 0;
        public OsobniPodaci()
        {
            this.InitializeComponent();
        }
        private bool IsText(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
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
            string connectionString = @"DataSource=D:\Skola\Niop 3g\UWP_Kviz\UWP_Kviz\Databaza.db;Version=3";
            string insertQuery = "INSERT INTO OsobniPodaci (OIB, Ime, Prezime) VALUES (?, ?, ?)";
            string selectQuery = "SELECT COUNT(*) FROM OsobniPodaci WHERE OIB = ?";
            int rowCount = 0;
            long value1;
            string value2 = Imeunos.Text;
            string value3 = Prezimeunos.Text;
            if (string.IsNullOrEmpty(OIBunos.Text) || string.IsNullOrEmpty(Imeunos.Text) || string.IsNullOrEmpty(Prezimeunos.Text))
            {
                ErrorTextbox.Text = "Ups! Nedostaju podaci!";
                return;
            }
            else if (!long.TryParse(OIBunos.Text, out value1) && (!IsText(Imeunos.Text) && !IsText(Prezimeunos.Text)))
            {
                ErrorTextbox.Text = "Svi podaci su u pogrešnom formatu!";
                return;
            }
            else if (!long.TryParse(OIBunos.Text, out value1) && (!IsText(Imeunos.Text) && IsText(Prezimeunos.Text)))
            {
                ErrorTextbox.Text = "OIB i ime su u pogrešnom formatu";
                return;
            }
            else if (!long.TryParse(OIBunos.Text, out value1) && (IsText(Imeunos.Text) && !IsText(Prezimeunos.Text)))
            {
                ErrorTextbox.Text = "OIB i prezime su u pogrešnom formatu";
                return;
            }
            else if (!long.TryParse(OIBunos.Text, out value1) && (IsText(Imeunos.Text) && IsText(Prezimeunos.Text)))
            {
                ErrorTextbox.Text = "OIB mora biti u brojčanom obliku!";
                return;
            }
            else if (!IsText(Imeunos.Text) && IsText(Prezimeunos.Text) && (long.TryParse(OIBunos.Text, out value1)))
            {
                ErrorTextbox.Text = "Ime mora biti u tekstualnom obliku!";
                return;
            }
            else if (IsText(Imeunos.Text) && !IsText(Prezimeunos.Text) && (long.TryParse(OIBunos.Text, out value1)))
            {
                ErrorTextbox.Text = "Prezime mora biti u tekstualnom obliku!";
                return;
            }
            else if (!IsText(Imeunos.Text) && !IsText(Prezimeunos.Text) && (long.TryParse(OIBunos.Text, out value1)))
            {
                ErrorTextbox.Text = "Ime i prezime moraju biti u tekstualnom obliku!";
                return;
            }
            else if (!long.TryParse(OIBunos.Text, out value1) && (!IsText(Imeunos.Text) || !IsText(Prezimeunos.Text)))
            {
                ErrorTextbox.Text = "Svi podaci su u pogrešnom formatu!";
                return;
            }
            else if (long.TryParse(OIBunos.Text, out value1) && (IsText(Imeunos.Text) || IsText(Prezimeunos.Text)))
            {

                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                        {
                            command.Parameters.AddWithValue("@OIB", value1);
                            rowCount = Convert.ToInt32(command.ExecuteScalar());
                        }
                    }

                    if (rowCount > 0)
                    {
                        ErrorTextbox.Text = "Osoba s istim OIB-om već postoji!";
                        return;
                    }

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Value1", value1);
                            command.Parameters.AddWithValue("@Value2", value2);
                            command.Parameters.AddWithValue("@Value3", value3);
                            command.ExecuteNonQuery();
                        }
                    }
                    playerCount++;
                    if(playerCount==1)
                    {
                        BrojIgraca.Text = "Drugi igrač:";
                    }
                    OIBunos.Text = "";
                    Imeunos.Text = "";
                    Prezimeunos.Text = "";
                    ErrorTextbox.Text = "";
                    if (playerCount == 2)
                    {
                        Frame.Navigate(typeof(Kviz));
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or display an error message
                    ErrorTextbox.Text = "An error occurred: " + ex.Message;
                }

            }


        }

    }
}
