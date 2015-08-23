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
        private int currentQuestion = 1;
        private int currentAnswer;
        private int countQuestion;

        public Test(int countQuestion)
        {
            InitializeComponent();
            culture = App.Language.Name;
            this.countQuestion = countQuestion;
            currentAnswer = OpenQuestion(currentQuestion, culture);
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

                    b1_answer.BorderBrush = Brushes.Transparent;
                    b2_answer.BorderBrush = Brushes.Transparent;
                    b3_answer.BorderBrush = Brushes.Transparent;
                    b4_answer.BorderBrush = Brushes.Transparent;

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

        public void NextQuestion()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 2, 0);
            timer.Tick += (s, ar) =>
            {
                timer.Stop();
                if (++currentQuestion <= countQuestion)
                {
                    currentAnswer = OpenQuestion(currentQuestion, culture);
                }
                else
                {
                    MessageBox.Show(rate.ToString());
                }
            };
            timer.Start();
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter converter = new BrushConverter();
            Brush green = converter.ConvertFromString("#FF27AC59") as Brush;
            Brush red = converter.ConvertFromString("#FFCF142B") as Brush;

            Button but = (sender as Button);
            int n = Convert.ToInt32(but.Name.Substring(1, 1));
            ButtonEnabled(false);

            if (n == currentAnswer)
            {
                but.BorderBrush = green;
                rate++;
            }
            else
            {
                but.BorderBrush = red;
            }
            NextQuestion();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            rate = 0;
            this.NavigationService.GoBack();
        }

        public void ButtonEnabled(bool enabled)
        {
            b1_answer.IsEnabled = enabled;
            b2_answer.IsEnabled = enabled;
            b3_answer.IsEnabled = enabled;
            b4_answer.IsEnabled = enabled;
        }
    }
}
