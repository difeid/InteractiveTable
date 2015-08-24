﻿using System;
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
using System.IO;
using System.Windows.Threading;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : Page
    {
        private int rate = 0;

        private string culture;
        private int currentQuestion;
        private int currentAnswer;
        private int countQuestion;
        private DispatcherTimer timer;

        public Test(int countQuestion)
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            culture = App.Language.Name;
            Init();
            this.countQuestion = countQuestion;
        }

        private void Init()
        {
            testCanvas.Visibility = Visibility.Visible;
            testResultCanvas.Visibility = Visibility.Collapsed;
            
            currentQuestion = 1;
            currentAnswer = OpenQuestion(currentQuestion, culture);
            if (currentAnswer == 0 && culture != "ru-RU")
            {
                culture = "ru-RU";
                currentAnswer = OpenQuestion(currentQuestion, culture);
            }
        }

        public int OpenQuestion(int numberQuestion, string culture)
        {
            string path = String.Format("Contents/Test/question.{0}.{1}.txt",numberQuestion, culture);
            int answer = 0;

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                
                if (lines != null && lines.Length >= 6)
                {
                    ButtonEnabled(true);

                    b1_answer.Background = Brushes.Transparent;
                    b2_answer.Background = Brushes.Transparent;
                    b3_answer.Background = Brushes.Transparent;
                    b4_answer.Background = Brushes.Transparent;

                    numberText.Text = numberQuestion.ToString();
                    questionText.Text = lines[0];

                    b1_answer.Content = lines[1];
                    b2_answer.Content = lines[2];
                    b3_answer.Content = lines[3];
                    b4_answer.Content = lines[4];

                    Int32.TryParse(lines[5], out answer);
                }
                
            }
            return answer;
        }

        public void NextQuestion(Button but, bool right)
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 700);
            timer.Tick += (s, ar) =>
            {
                timer.Stop();
                if (++currentQuestion <= countQuestion)
                {
                    currentAnswer = OpenQuestion(currentQuestion, culture);
                }
                else
                {
                    TestResult(rate);
                }
            };
            timer.Start();
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter converter = new BrushConverter();
            Brush green = converter.ConvertFromString("#FF27AC5E") as Brush;
            Brush red = converter.ConvertFromString("#FFC91329") as Brush;

            Button but = (sender as Button);
            int n = Convert.ToInt32(but.Name.Substring(1, 1));
            ButtonEnabled(false);

            if (n == currentAnswer)
            {
                but.Background = green;
                but.Foreground = Brushes.White;
                rate++;
            }
            else
            {
                but.Background = red;
                but.Foreground = Brushes.White;
            }
            NextQuestion(but, true);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        public void ButtonEnabled(bool enabled)
        {
            b1_answer.IsEnabled = enabled;
            b2_answer.IsEnabled = enabled;
            b3_answer.IsEnabled = enabled;
            b4_answer.IsEnabled = enabled;
        }

        public void TestResult(int result)
        {
            testCanvas.Visibility = Visibility.Collapsed;
            testResultCanvas.Visibility = Visibility.Visible;
            resultText.Text = result.ToString();

            ResourceDictionary dict = (from d in Application.Current.Resources.MergedDictionaries
                                       where d.Source != null && d.Source.OriginalString.StartsWith("LanguageResources/lang.")
                                       select d).First();

            if (result == 0)
            {
                 text.Text = dict["t_Answer2"] as String;
            }
            else if (result == 1)
            {
                text.Text = dict["t_Answer0"] as String;
            }
            else if (result <= 4)
            {
                text.Text = dict["t_Answer1"] as String;
            }
            else
            {
                text.Text = dict["t_Answer2"] as String;
            }
        }

        private void Repeat_Button_Click(object sender, RoutedEventArgs e)
        {
            rate = 0;
            Init();
        }
    }
}
