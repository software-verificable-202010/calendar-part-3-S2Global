using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for AppointmentForm.xaml
    /// </summary>
    public partial class AppointmentForm : Window
    {
        public AppointmentForm(bool isUpdate, Appointment appointment)
        {
            InitializeComponent();
            if (isUpdate)
            {
                titleBox.Text = appointment.title;
                titleBox.IsReadOnly = true;
                titleBox.IsEnabled = false;
                titleBox.ToolTip = "Cannot change title.";
                descriptionBox.Text = appointment.description;
                startDateBox.Value = appointment.startDate;
                endDateBox.Value = appointment.endDate;
                participantBox.Text = string.Join(" ", appointment.participants);
                submitButton.Tag = appointment;
                submitButton.Click += new RoutedEventHandler(UpdateAppointment);
                deleteButton.Tag = appointment;
                deleteButton.Click += new RoutedEventHandler(DeleteAppointment);
            }
            else
            {
                submitButton.Tag = null;
                submitButton.Click += new RoutedEventHandler(CreateAppointment);
                deleteButton.IsEnabled = false;
            }
        }

        public void CreateAppointment(object sender, RoutedEventArgs e)
        {
            if(titleBox.Text.Length > 0 && descriptionBox.Text.Length > 0 && startDateBox.Value.HasValue && endDateBox.Value.HasValue && participantBox.Text.Length > 0)
            {
                List<string> participants = participantBox.Text.Split().ToList();
                Appointment appointment = new Appointment(titleBox.Text, descriptionBox.Text, (DateTime)startDateBox.Value, (DateTime)endDateBox.Value, MainWindow.sessionUser, participants);
                if (appointment.SaveNewAppointment())
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Appointment overlap!", "Overlap Error");
                }
            }
            else
            {
                MessageBox.Show("Data Missing!", "Data Error");
            }
        }

        public void UpdateAppointment(object sender, RoutedEventArgs e)
        {
            if (titleBox.Text.Length > 0 && descriptionBox.Text.Length > 0 && startDateBox.Value.HasValue && endDateBox.Value.HasValue && participantBox.Text.Length > 0)
            {
                List<string> participants = participantBox.Text.Split().ToList();
                Appointment appointment = MainWindow.sessionUserAppointments.Find(appointment => appointment.title == titleBox.Text);
                appointment.Update(MainWindow.sessionUser, descriptionBox.Text, (DateTime)startDateBox.Value, (DateTime)endDateBox.Value, participants);
                appointment.SaveUpdatedAppointment();
                this.Close();
            }
            else
            {
                MessageBox.Show("Data Missing!", "Data Error");
            }
        }

        public void DeleteAppointment(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are You Sure?", "Delete Appointment", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var button = sender as Button;
                Appointment appointment = (Appointment)button.Tag;
                appointment.Delete();
                this.Close();
            }
        }
    }
}
