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
using System.Windows.Media.Animation;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : Page
    {
        private int rate;

        private string culture;
        private int currentQuestion;
        private int currentAnswer;
        private int countQuestion;

        private DispatcherTimer timer;
        private BrushConverter converter;
        private Brush green;
        private Brush red;

        private DoubleAnimation da;
        private double progressBarPartWidth;

        public Test(int countQuestion)
        {
            InitializeComponent();

            timer = new DispatcherTimer();

            converter = new BrushConverter();
            green = converter.ConvertFromString("#FF27AC5E") as Brush;
            red = converter.ConvertFromString("#FFC91329") as Brush;

            da = new DoubleAnimation();

            culture = App.Language.Name;
            this.countQuestion = countQuestion;
            progressBarPartWidth = 340 / countQuestion;
            Init();
        }

        private void Init()
        {
            da.To = 0;
            da.Duration = TimeSpan.FromMilliseconds(400);
            testResultCanvas.BeginAnimation(Canvas.OpacityProperty, da);
            testResultCanvas.Visibility = Visibility.Collapsed;

            da.To = 0;
            da.Duration = TimeSpan.FromMilliseconds(1);
            progressBar.BeginAnimation(Rectangle.WidthProperty, da);

            testCanvas.Visibility = Visibility.Visible;

            currentQuestion = 1;
            rate = 0;

            currentAnswer = OpenQuestion(currentQuestion, culture);
            if (currentAnswer == 0 && culture != "ru-RU")
            {
                culture = "ru-RU";
                currentAnswer = OpenQuestion(currentQuestion, culture);
            }
            da.To = 1;
            da.Duration = TimeSpan.FromMilliseconds(400);
            testCanvas.BeginAnimation(Canvas.OpacityProperty, da);
        }

        private void Progress(double to)
        {
            da.To = progressBar.Width + to;
            da.Duration = TimeSpan.FromMilliseconds(100);
            progressBar.BeginAnimation(Rectangle.WidthProperty, da);
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

                    b1_answer.Foreground = Brushes.Black;
                    b2_answer.Foreground = Brushes.Black;
                    b3_answer.Foreground = Brushes.Black;
                    b4_answer.Foreground = Brushes.Black;

                    textCount.Text = String.Format("{0}/{1}", numberQuestion, countQuestion);

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
  
        private void Timer_Tick_NextQuestion(object sender, EventArgs e)
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
            timer.Tick -= Timer_Tick_NextQuestion;
        }
        private void Timer_Tick_RightAnswer(object sender, EventArgs e)
        {
            timer.Stop();

            switch (currentAnswer)
            {
                case 1:
                    b1_answer.Background = green;
                    b1_answer.Foreground = Brushes.White;
                    break;
                case 2:
                    b2_answer.Background = green;
                    b2_answer.Foreground = Brushes.White;
                    break;
                case 3:
                    b3_answer.Background = green;
                    b3_answer.Foreground = Brushes.White;
                    break;
                case 4:
                    b4_answer.Background = green;
                    b4_answer.Foreground = Brushes.White;
                    break;
            }
            timer.Tick -= Timer_Tick_RightAnswer;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick_NextQuestion;
            timer.Start();
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Button but = (sender as Button);
            int n = Convert.ToInt32(but.Name.Substring(1, 1));
            ButtonEnabled(false);
            Progress(progressBarPartWidth);

            if (n == currentAnswer)
            {
                but.Background = green;
                but.Foreground = Brushes.White;
                rate++;
                timer.Interval = new TimeSpan(0, 0, 0, 0, 700);
                timer.Tick += Timer_Tick_NextQuestion;
                timer.Start();
            }
            else
            {
                but.Background = red;
                but.Foreground = Brushes.White;

                timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                timer.Tick += Timer_Tick_RightAnswer;
                timer.Start();
            }
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
            da.To = 0;
            da.Duration = TimeSpan.FromMilliseconds(600);
            testCanvas.BeginAnimation(Canvas.OpacityProperty, da);
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
            da.To = 1;
            da.Duration = TimeSpan.FromMilliseconds(600);
            testResultCanvas.BeginAnimation(Canvas.OpacityProperty, da);
        }

        private void Repeat_Button_Click(object sender, RoutedEventArgs e)
        {
            Init();
        }
    }
}
