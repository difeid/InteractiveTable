using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace InteractiveTable
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<CultureInfo> m_Languages = new List<CultureInfo>();

        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        public static bool PopupOpen { get; set; }

        public static bool BookOpen { get; set; }

        private DispatcherTimer timer;

        public App()
        {
            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US")); //Английский
            m_Languages.Add(new CultureInfo("ru-RU")); //Русский
            m_Languages.Add(new CultureInfo("tt-RU")); //Татарский

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1, 0);
            timer.Tick += IdleTick;
            timer.Start();
        }

        //Евент для оповещения всех окон приложения
        public static event EventHandler LanguageChanged;

        public static event EventHandler IdleTimeOut;

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                //1. Меняем язык приложения:
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                //2. Создаём ResourceDictionary для новой культуры
                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("LanguageResources/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    case "tt-RU":
                        dict.Source = new Uri(String.Format("LanguageResources/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("LanguageResources/lang.en-US.xaml", UriKind.Relative);
                        break;
                }

                //3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("LanguageResources/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }

                //4. Вызываем евент для оповещения всех окон.
                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        private void IdleTick(object sender, EventArgs e)
        {
            var idle = GetIdle();
            if (idle.Minutes >= 10)
            {
                IdleTimeOut(Application.Current, new EventArgs());
            }
        }

        public static TimeSpan GetIdle()
        {
            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

            GetLastInputInfo(ref lastInputInfo);

            var lastInput = DateTime.Now.AddMilliseconds(-(Environment.TickCount - lastInputInfo.dwTime));
            return DateTime.Now - lastInput;
        }

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
    }
}
