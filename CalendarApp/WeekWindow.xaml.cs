using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for WeekWindow.xaml
    /// </summary>
    public partial class WeekWindow : Window
    {
        private int weekLength = 7;

        public WeekWindow()
        {
            System.Globalization.Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            InitializeComponent();
            UpdateWeekView();
        }

        public void UpdateWeekView()
        {
            WeekView.Children.Clear();
            DayNumbers.Children.Clear();
            UpdateTitle();
            UpdateDayNumbers();
            UpdateTimes();
            UpdateDayAppointments();
        }

        private void NextWeekClick(object sender, RoutedEventArgs e)
        {
            MainWindow.calendarDate = MainWindow.calendarDate.AddDays(weekLength);
            UpdateWeekView();
        }

        private void PreviousWeekClick(object sender, RoutedEventArgs e)
        {
            MainWindow.calendarDate = MainWindow.calendarDate.AddDays(-weekLength);
            UpdateWeekView();
        }

        private void GoToCalendar(object sender, RoutedEventArgs e)
        {
            var calendarView = new MainWindow(MainWindow.calendarDate);
            calendarView.Show();
            this.Close();
        }

        public void GoToAppointmentView(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Appointment appointment = (Appointment)button.Tag;
            var AppointmentView = new AppointmentWindow(appointment);
            AppointmentView.Show();
        }

        private void GoToAppointmentForm(object sender, RoutedEventArgs e)
        {
            var AppointmentFormView = new AppointmentForm(false, null);
            AppointmentFormView.Show();
        }

        private void UpdateDayNumbers()
        {
            int sunday = 7;
            int dayOffset = 1;
            DateTime dayTracker = MainWindow.calendarDate;
            int dayOfWeek = (int)dayTracker.DayOfWeek;
            dayTracker = dayTracker.AddDays(-dayOfWeek + dayOffset);
            if (dayOfWeek == 0)
            {
                dayOfWeek = sunday;
            }
            for (int i = 1; i < weekLength + dayOffset; i++)
            {
                int dayOfPosition = dayTracker.Day;
                int rowPosition = 0;
                TextBlock dayNumber = new TextBlock();
                dayNumber.Text = dayOfPosition.ToString();
                dayNumber.FontSize = 16;
                dayNumber.SetValue(Grid.RowProperty, rowPosition);
                dayNumber.SetValue(Grid.ColumnProperty, i);
                dayNumber.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Center);
                dayNumber.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                DayNumbers.Children.Add(dayNumber);
                dayTracker = dayTracker.AddDays(1); 
            }
        }

        private void UpdateDayAppointments()
        {
            int sunday = 7;
            int dayOffset = 1;
            DateTime dayTracker = MainWindow.calendarDate;
            int dayOfWeek = (int)dayTracker.DayOfWeek;
            dayTracker = dayTracker.AddDays(-dayOfWeek + dayOffset);
            if (dayOfWeek == 0)
            {
                dayOfWeek = sunday;
            }
            for (int i = 0; i < weekLength; i++)
            {
                List<Appointment> dayAppointments = MainWindow.sessionUserAppointments.FindAll(appointment => (appointment.startDate.Date == dayTracker.Date));
                if (dayAppointments.Count > 0)
                {
                    foreach (Appointment appointment in dayAppointments)
                    {
                        CreateButton(appointment, i);
                    }
                }
                dayTracker = dayTracker.AddDays(dayOffset);
            }
        }

        private void CreateButton(Appointment appointment, int i)
        {
            int dayOffset = 1;
            Button appointmentButtton = new Button();
            appointmentButtton.Content = appointment.title;
            appointmentButtton.ToolTip = "Click to open.";
            appointmentButtton.Background = Brushes.Salmon;
            appointmentButtton.BorderThickness = new Thickness(0, 0, 0, 0);
            appointmentButtton.FontSize = 16;
            appointmentButtton.SetValue(Grid.ColumnProperty, i + dayOffset);
            appointmentButtton.SetValue(Grid.RowProperty, appointment.startDate.Hour);
            if (appointment.endDate.Date != appointment.startDate.Date)
            {
                int fullDay = 24;
                appointmentButtton.SetValue(Grid.RowSpanProperty, fullDay);
            }
            else
            {
                appointmentButtton.SetValue(Grid.RowSpanProperty, appointment.endDate.Hour - appointment.startDate.Hour + dayOffset);
            }
            appointmentButtton.Click += new RoutedEventHandler(GoToAppointmentView);
            appointmentButtton.Tag = appointment;
            appointmentButtton.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            appointmentButtton.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            WeekView.Children.Add(appointmentButtton);
        }

        private void UpdateTimes()
        {
            int loopStart = 0;
            int loopEnd = 23;
            for (int i = loopStart; i < loopEnd + 1; i++)
            {
                int columnPosition = 0;
                TextBlock time = new TextBlock();
                time.Text = "" + i + ":00";
                time.FontSize = 12;
                time.SetValue(Grid.RowProperty, i);
                time.SetValue(Grid.ColumnProperty, columnPosition);
                time.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Center);
                time.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                WeekView.Children.Add(time);
            }
        }

        private void UpdateTitle()
        {
            string title = MainWindow.calendarDate.ToString("MMMM") + " " + MainWindow.calendarDate.Year;
            Title.Text = title;
        }
    }
}