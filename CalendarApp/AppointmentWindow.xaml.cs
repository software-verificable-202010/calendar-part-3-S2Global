using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalendarApp
{
    /// <summary>
    /// Interaction logic for AppointmentWindow.xaml
    /// </summary>
    public partial class AppointmentWindow : Window
    {
        public AppointmentWindow(Appointment appointment)
        {
            InitializeComponent();
            UpdateAppointmentView(appointment);
        }

        public void UpdateAppointmentView(Appointment appointment)
        {

            TextBlock title = new TextBlock();
            TextBlock creator = new TextBlock();
            TextBlock startDate = new TextBlock();
            TextBlock endDate = new TextBlock();
            TextBlock participants = new TextBlock();
            TextBlock description = new TextBlock();

            string participantString = "";

            title.Text = appointment.title;
            creator.Text = appointment.creator.username;
            startDate.Text = appointment.startDate.ToString();
            endDate.Text = appointment.endDate.ToString();
            foreach (User participant in appointment.participants)
            {
                participantString += participant.username + " ";
            }
            participants.Text = participantString;
            description.Text = appointment.description;

            title.FontSize = 16;
            creator.FontSize = 16;
            startDate.FontSize = 16;
            endDate.FontSize = 16;
            participants.FontSize = 16;
            description.FontSize = 16;

            int titleRowPosition = 0;
            int creatorRowPosition = 1;
            int startDateRowPosition = 2;
            int endDateRowPosition = 3;
            int participantsRowPosition = 4;
            int descriptionRowPosition = 5;

            title.SetValue(Grid.RowProperty, titleRowPosition);
            creator.SetValue(Grid.RowProperty, creatorRowPosition);
            startDate.SetValue(Grid.RowProperty, startDateRowPosition);
            endDate.SetValue(Grid.RowProperty, endDateRowPosition);
            participants.SetValue(Grid.RowProperty, participantsRowPosition);
            description.SetValue(Grid.RowProperty, descriptionRowPosition);

            int columnPosition = 1;

            title.SetValue(Grid.ColumnProperty, columnPosition);
            creator.SetValue(Grid.ColumnProperty, columnPosition);
            startDate.SetValue(Grid.ColumnProperty, columnPosition);
            endDate.SetValue(Grid.ColumnProperty, columnPosition);
            participants.SetValue(Grid.ColumnProperty, columnPosition);
            description.SetValue(Grid.ColumnProperty, columnPosition);

            AppointmentView.Children.Add(title);
            AppointmentView.Children.Add(creator);
            AppointmentView.Children.Add(startDate);
            AppointmentView.Children.Add(endDate);
            AppointmentView.Children.Add(participants);
            AppointmentView.Children.Add(description);
        }
    }
}
