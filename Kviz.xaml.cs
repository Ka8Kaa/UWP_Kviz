using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWP_Kviz
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] WrongAnswers { get; set; }
    }

    public sealed partial class Kviz : Page
    {
        private List<int> displayedQuestionIds = new List<int>();
        private int totalNumberOfQuestions = 2;
        private Question currentQuestion; // Field to store the current question

        public Kviz()
        {
            this.InitializeComponent();
            FetchLastTwoRows();
        }

        private void Započni_Click(object sender, RoutedEventArgs e)
        {
            // Fetch a random question
            currentQuestion = GetRandomQuestion();

            // Ensure that a question was fetched
            if (currentQuestion != null)
            {
                // Combine correct and wrong answers into a list
                List<string> options = new List<string>();
                options.Add(currentQuestion.CorrectAnswer);
                options.AddRange(currentQuestion.WrongAnswers);

                // Shuffle the options
                Random rnd = new Random();
                options = options.OrderBy(x => rnd.Next()).ToList();

                // Update UI with the shuffled options
                questionTextBlock.Text = currentQuestion.QuestionText;

                // Check if we have at least four options
                if (options.Count >= 4)
                {
                    optionRadioButton1.Content = options[0];
                    optionRadioButton2.Content = options[1];
                    optionRadioButton3.Content = options[2];
                    optionRadioButton4.Content = options[3];

                    // Make the TextBlock and RadioButtons visible
                    questionTextBlock.Visibility = Visibility.Visible;
                    optionRadioButton1.Visibility = Visibility.Visible;
                    optionRadioButton2.Visibility = Visibility.Visible;
                    optionRadioButton3.Visibility = Visibility.Visible;
                    optionRadioButton4.Visibility = Visibility.Visible;

                    // Clear error message
                    ErrorText.Text = "";
                }
                else
                {
                    // Handle the case when there are not enough options
                    ErrorText.Text = "Not enough options for the question.";
                }
            }
        }

        // Method to fetch a random question from SQLite database
        private Question GetRandomQuestion()
        {
            string connectionString = @"Data Source=D:\Skola\Niop 3g\UWP_Kviz\UWP_Kviz\Databaza.db;Version=3";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // SQLite query to get a random question that hasn't been displayed yet
                    string query = "SELECT * FROM Pitanja WHERE ID NOT IN (" + string.Join(",", displayedQuestionIds) + ") ORDER BY RANDOM() LIMIT 1";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {
                                    // Construct the question object
                                    Question question = new Question
                                    {
                                        Id = reader.GetInt32(0),
                                        QuestionText = reader.GetString(1),
                                        CorrectAnswer = reader.GetString(2),
                                        WrongAnswers = new string[]
                                        {
                                            reader.GetString(3),
                                            reader.GetString(4),
                                            reader.GetString(5)
                                        }
                                    };

                                    // Add the displayed question ID to the list
                                    displayedQuestionIds.Add(question.Id);

                                    // If all questions have been displayed, reset the list
                                    if (displayedQuestionIds.Count == totalNumberOfQuestions)
                                    {
                                        displayedQuestionIds.Clear();
                                    }

                                    return question;
                                }
                            }
                            else
                            {
                                // Handle case when the query did not return any rows
                                ErrorText.Text = "No rows returned by the query.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., database connection error)
                ErrorText.Text = "An error occurred: " + ex.Message;
            }

            return null; // Handle case when no new question is found
        }

        // Method to get the current question
        private Question GetCurrentQuestion()
        {
            return currentQuestion;
        }

        private void Provjeri_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected answer
            string selectedAnswer = GetSelectedAnswer();

            // Check if an answer is selected
            if (selectedAnswer != null)
            {
                // Check if the selected answer is correct
                if (selectedAnswer == currentQuestion.CorrectAnswer)
                {
                    // If the answer is correct, generate a new question
                    DisplayNewQuestion();
                }
                else
                {
                    // If the answer is wrong, display a message
                    PogresniOdgovori.Text = "Pogrešan odgovor! Pokušaj ponovno!";
                }
            }
            else
            {
                // If no answer is selected, display a message
                PogresniOdgovori.Text = "Molimo izaberite odgovor.";
            }
        }

        // Method to get the selected answer
        private string GetSelectedAnswer()
        {
            if (optionRadioButton1.IsChecked == true)
            {
                return optionRadioButton1.Content as string;
            }
            else if (optionRadioButton2.IsChecked == true)
            {
                return optionRadioButton2.Content as string;
            }
            else if (optionRadioButton3.IsChecked == true)
            {
                return optionRadioButton3.Content as string;
            }
            else if (optionRadioButton4.IsChecked == true)
            {
                return optionRadioButton4.Content as string;
            }
            else
            {
                return null;
            }
        }

        // Method to display a new question
        private void DisplayNewQuestion()
        {
            // Clear the error message
            ErrorText.Text = "";

            // Fetch and display a new question
            Question randomQuestion = GetRandomQuestion();
            if (randomQuestion != null)
            {
                // Display the new question
                questionTextBlock.Text = randomQuestion.QuestionText;

                // Shuffle the options
                List<string> options = new List<string>();
                options.Add(randomQuestion.CorrectAnswer);
                options.AddRange(randomQuestion.WrongAnswers);
                Random rnd = new Random();
                options = options.OrderBy(x => rnd.Next()).ToList();

                // Update UI with the shuffled options
                optionRadioButton1.Content = options[0];
                optionRadioButton2.Content = options[1];
                optionRadioButton3.Content = options[2];
                optionRadioButton4.Content = options[3];
            }
        }
        private void FetchLastTwoRows()
        {
            string connectionString = @"Data Source=D:\Skola\Niop 3g\UWP_Kviz\UWP_Kviz\Databaza.db;Version=3";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // SQLite query to fetch the last two rows from the table
                    string query = "SELECT * FROM OsobniPodaci ORDER BY ID_Unos DESC LIMIT 2";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Ensure there are rows returned
                            if (reader.HasRows)
                            {
                                // Counter to keep track of which row we are on
                                int rowCounter = 0;

                                while (reader.Read())
                                {
                                    // Retrieve values from each column
                                    int ID_Unos = reader.GetInt32(0);
                                    long OIB = reader.GetInt64(1);
                                    string ime = reader.GetString(2);
                                    string prezime = reader.GetString(3);

                                    // Assuming you have text blocks named accordingly: 
                                    // idTextBlock1, questionTextBlock1, answerTextBlock1 for the second to last row
                                    // idTextBlock2, questionTextBlock2, answerTextBlock2 for the last row

                                    // Update text blocks with data from the current row
                                    if (rowCounter == 0)
                                    {
                                        DrugiIgracOIB.Text = $"{OIB}";
                                        DrugiIgracIme.Text = $"{ime}";
                                        DrugiIgracPrezime.Text = $"{prezime}";
                                    }
                                    else if (rowCounter == 1)
                                    {
                                        PrviIgracOIB.Text = $"{OIB}";
                                        PrviIgracIme.Text = $"{ime}";
                                        PrviIgracPrezime.Text = $"{prezime}";
                                    }

                                    // Increment the row counter
                                    rowCounter++;
                                }
                            }
                            else
                            {
                                // Handle case when no rows are returned
                                ErrorText.Text = "No rows returned by the query.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., database connection error)
                ErrorText.Text = "An error occurred: " + ex.Message;
            }
        }
    }
}





