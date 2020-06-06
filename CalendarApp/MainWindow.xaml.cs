using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace CalendarApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DateTime CalendarDate;

        public static User sessionUser;

        public MainWindow()
        {
            System.Globalization.Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            InitializeComponent();
            CalendarDate = DateTime.Now;
            UpdateCalendar();
        }

        public MainWindow(User user)
        {
            System.Globalization.Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            InitializeComponent();
            CalendarDate = DateTime.Now;
            sessionUser = user;
            UpdateCalendar();
        }

        public MainWindow(DateTime incomingDate)
        {
            System.Globalization.Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            InitializeComponent();
            CalendarDate = incomingDate;
            UpdateCalendar();
        }

        public void UpdateCalendar()
        {
            MonthView.Children.Clear();
            UpdateTitle();
            UpdateRectangle();
            UpdateDayNumbers();
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            int nextMonth = 1;
            CalendarDate = CalendarDate.AddMonths(nextMonth);
            UpdateCalendar();
        }

        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            int previousMonth = -1;
            CalendarDate = CalendarDate.AddMonths(previousMonth);
            UpdateCalendar();
        }

        private void GoToWeek(object sender, RoutedEventArgs e)
        {
            var weekView = new WeekWindow();
            weekView.Show();
            this.Close();
        }

        private void GoToAppointment()
        {
            /*var AppointmentView = new AppointmentWindow();
            AppointmentView.Show();*/
        }

        private void GoToAppointmentForm()
        {
            var AppointmentFormView = new AppointmentForm();
            AppointmentFormView.Show();
        }

        private void GoToSignIn()
        {
            var SignInView = new SignInWindow();
            SignInView.Show();
        }

        private void UpdateRectangle()
        {
            int weekendRowProperty = 0;
            int weekendColumnProperty = 5;
            int weekendRowSpanProperty = 6;
            int weekendColumnSpanProperty = 2;
            Rectangle weekendHighlight = new Rectangle();
            weekendHighlight.SetValue(Grid.RowProperty, weekendRowProperty);
            weekendHighlight.SetValue(Grid.ColumnProperty, weekendColumnProperty);
            weekendHighlight.SetValue(Grid.RowSpanProperty, weekendRowSpanProperty);
            weekendHighlight.SetValue(Grid.ColumnSpanProperty, weekendColumnSpanProperty);
            SolidColorBrush rectangleColourFill = new SolidColorBrush();
            rectangleColourFill.Color = Color.FromArgb(100, 127, 255, 212);
            weekendHighlight.SetValue(Shape.FillProperty, rectangleColourFill);
            MonthView.Children.Add(weekendHighlight);
        }

        private void UpdateDayNumbers()
        {
            int sunday = 7;
            int year = CalendarDate.Year;
            int month = CalendarDate.Month;
            int firstDay = 1;
            DateTime firstDayOfMonth = new DateTime(year, month, firstDay);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            System.Diagnostics.Debug.WriteLine(firstDayOfWeek);
            if (firstDayOfWeek == 0)
            {
                firstDayOfWeek = sunday;
            }
            sunday--;
            int day = 1;
            int week = 0;
            int weekDay = firstDayOfWeek - day;
            for (int i = firstDayOfWeek; i < daysInMonth + firstDayOfWeek; i++)
            {
                TextBlock dayNumber = new TextBlock();
                dayNumber.Text = day.ToString();
                dayNumber.SetValue(Grid.RowProperty, week);
                dayNumber.SetValue(Grid.ColumnProperty, weekDay);
                MonthView.Children.Add(dayNumber);
                if (weekDay == sunday)
                {
                    week++;
                    weekDay = 0;
                }
                else
                {
                    weekDay++;
                }
                day++;
            }
        }

        private void UpdateTitle()
        {
            string title = CalendarDate.ToString("MMMM") + " " + CalendarDate.Year;
            Title.Text = title;
        }
    }
}
