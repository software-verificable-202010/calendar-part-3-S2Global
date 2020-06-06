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
    /// Interaction logic for WeekWindow.xaml
    /// </summary>
    public partial class WeekWindow : Window
    {
        private int weekLength = 7;
        private Appointment[] appointments;

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
        }

        private void NextWeekClick(object sender, RoutedEventArgs e)
        {
            MainWindow.CalendarDate = MainWindow.CalendarDate.AddDays(weekLength);
            UpdateWeekView();
        }

        private void PreviousWeekClick(object sender, RoutedEventArgs e)
        {
            MainWindow.CalendarDate = MainWindow.CalendarDate.AddDays(-weekLength);
            UpdateWeekView();
        }

        private void GoToCalendar(object sender, RoutedEventArgs e)
        {
            var calendarView = new MainWindow(MainWindow.CalendarDate);
            calendarView.Show();
            this.Close();
        }

        private void UpdateDayNumbers()
        {
            int sunday = 7;
            int dayOffset = 1;
            DateTime dayTracker = MainWindow.CalendarDate;
            int dayOfWeek = (int)dayTracker.DayOfWeek;
            dayTracker = dayTracker.AddDays(-dayOfWeek + dayOffset);
            System.Diagnostics.Debug.WriteLine(dayOfWeek);
            if (dayOfWeek == 0)
            {
                dayOfWeek = sunday;
            }
            int loopStart = 1;
            for (int i = loopStart; i < weekLength+1; i++)
            {
                int dayOfPosition = dayTracker.Day;
                System.Diagnostics.Debug.WriteLine(dayOfPosition);
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
            string title = MainWindow.CalendarDate.ToString("MMMM") + " " + MainWindow.CalendarDate.Year;
            Title.Text = title;
        }

        private void UpdateApointments()
        {
            
            foreach (Appointment appointment in appointments)
            {
                if(appointment.startDate >= MainWindow.CalendarDate  && appointment.endDate < MainWindow.CalendarDate.AddDays(weekLength))
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
                    WeekView.Children.Add(weekendHighlight);
                }
            }
        }
    }
}
