using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
        public static DateTime calendarDate;

        public static User sessionUser;

        public static List<Appointment> sessionUserAppointments;

        public MainWindow(User user)
        {
            System.Globalization.Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            InitializeComponent();
            calendarDate = DateTime.Now;
            sessionUser = user;
            UpdateCalendar();
        }

        public MainWindow(DateTime incomingDate)
        {
            System.Globalization.Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            InitializeComponent();
            calendarDate = incomingDate;
            UpdateCalendar();
        }

        public void UpdateCalendar()
        {
            MonthView.Children.Clear();
            UpdateTitle();
            UpdateWeekendRectangle();
            GetAppointments();
            UpdateDayNumbers();
            UpdateDayButtons();
        }

        public void GetAppointments()
        {

            string jsonAppointments = null;
            try
            {
                jsonAppointments = File.ReadAllText("Appointments.json");
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }

            if (jsonAppointments != null)
            {
                List<Appointment> allAppointments = JsonConvert.DeserializeObject<List<Appointment>>(jsonAppointments);
                sessionUserAppointments = allAppointments.Where(x => x.participants.Contains(sessionUser.username)).ToList();
            }
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            int nextMonth = 1;
            calendarDate = calendarDate.AddMonths(nextMonth);
            UpdateCalendar();
        }

        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            int previousMonth = -1;
            calendarDate = calendarDate.AddMonths(previousMonth);
            UpdateCalendar();
        }

        private void GoToWeek(object sender, RoutedEventArgs e)
        {
            var weekView = new WeekWindow();
            weekView.Show();
            this.Close();
        }

        public void GoToAppointmentsView(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            List<Appointment> appointments = (List<Appointment>)button.Tag;
            Debug.WriteLine(appointments[0].title);
            foreach (Appointment appointment in appointments)
            {
                var AppointmentView = new AppointmentWindow(appointment);
                AppointmentView.Show();
            }
        }

        private void GoToAppointmentForm(object sender, RoutedEventArgs e)
        {
            var AppointmentFormView = new AppointmentForm(false, null);
            AppointmentFormView.Show();
        }

        private void UpdateWeekendRectangle()
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
            int year = calendarDate.Year;
            int month = calendarDate.Month;
            int firstDay = 1;
            DateTime firstDayOfMonth = new DateTime(year, month, firstDay);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            Debug.WriteLine(firstDayOfWeek);
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

        private void UpdateDayButtons()
        {
            int sunday = 7;
            int year = calendarDate.Year;
            int month = calendarDate.Month;
            int firstDay = 1;
            DateTime firstDayOfMonth = new DateTime(year, month, firstDay);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
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
                DateTime currentDate = firstDayOfMonth.AddDays(day - firstDay);
                Button dayButton = new Button();
                int buttonSize = 30;
                List<Appointment> dayAppointments = sessionUserAppointments.FindAll(appointment => (appointment.startDate.Date == currentDate.Date));
                if (dayAppointments.Count > 0)
                {
                    dayButton.ToolTip = "Click to open all appointments for the day.";
                    dayButton.Background = Brushes.Salmon;
                    dayButton.BorderThickness = new Thickness(0,0,0,0);
                    dayButton.Content = dayAppointments.Count;
                    dayButton.Tag = dayAppointments;
                    dayButton.SetValue(Grid.RowProperty, week);
                    dayButton.SetValue(Grid.ColumnProperty, weekDay);
                    dayButton.Height = buttonSize;
                    dayButton.Width = buttonSize;
                    dayButton.Click += new RoutedEventHandler(GoToAppointmentsView);
                    MonthView.Children.Add(dayButton);
                }
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
            string title = calendarDate.ToString("MMMM") + " " + calendarDate.Year;
            Title.Text = title;
        }
    }
}
