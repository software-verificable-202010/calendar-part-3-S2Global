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
    /// Interaction logic for AppointmentForm.xaml
    /// </summary>
    public partial class AppointmentForm : Window
    {
        public AppointmentForm()
        {
            InitializeComponent();
        }

        public void CreateAppointment(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment(titleBox.Text, descriptionBox.Text, (DateTime)startDateBox.Value, (DateTime)endDateBox.Value, MainWindow.sessionUser);
            appointment.Save();
            this.Close();
        }
    }
}
