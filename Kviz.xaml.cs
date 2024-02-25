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

        public Kviz()
        {
            this.InitializeComponent();
        }

        private void Započni_Click(object sender, RoutedEventArgs e)
        {
            // Fetch a random question
            Question randomQuestion = GetRandomQuestion();

            // Ensure that a question was fetched
            if (randomQuestion != null)
            {
                // Combine correct and wrong answers into a list
                List<string> options = new List<string>();
                options.Add(randomQuestion.CorrectAnswer);
                options.AddRange(randomQuestion.WrongAnswers);

                // Shuffle the options
                Random rnd = new Random();
                options = options.OrderBy(x => rnd.Next()).ToList();

                // Update UI with the shuffled options
                questionTextBlock.Text = randomQuestion.QuestionText;

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
    }
}




