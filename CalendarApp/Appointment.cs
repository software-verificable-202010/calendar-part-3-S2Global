using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Xml;

namespace CalendarApp
{
    [TestFixture]
    public class Appointment
    {
        public User creator;
        public List<string> participants;
        public string title;
        public string description;
        public DateTime startDate;
        public DateTime endDate;

        [JsonConstructor]
        public Appointment(string title, string description, DateTime startDate, DateTime endDate, User creator, List<string> participants)
        {
            this.title = title;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
            this.creator = creator;
            this.participants = participants;
        }

        public bool SaveNewAppointment()
        {
            string jsonStringToAdd = null;
            try
            {
                jsonStringToAdd = File.ReadAllText("Appointments.json");
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }

            if (jsonStringToAdd != null)
            {
                List<Appointment> appointmentList = JsonConvert.DeserializeObject<List<Appointment>>(jsonStringToAdd);
                if (appointmentList.Find(appointment => appointment.title == this.title) == null && !HasOverlap(appointmentList))
                {
                    appointmentList.Add(this);
                    var convertedJson = JsonConvert.SerializeObject(appointmentList, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("Appointments.json", convertedJson);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                List<Appointment> list = new List<Appointment> { this };
                var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("Appointments.json", convertedJson);
            }
            return true;
        }

        public bool HasOverlap(List<Appointment> appointmentList)
        {
            if(appointmentList.Find(appointment => appointment.title == this.title) != null)
            {
                return true;
            }
            foreach(string participant in this.participants)
            {
                IEnumerable<Appointment> overlappingAppointments = appointmentList.Where(appointment => (appointment.participants.Contains(participant) && (((appointment.startDate >= this.startDate) && (appointment.startDate <= this.endDate)) || ((appointment.endDate >= this.startDate) && (appointment.endDate <= this.endDate)))));
                if (overlappingAppointments.Any())
                {
                    return true;
                }
            }
            return false;
        }

        [TestCase]
        public void Update(User user, string title, string description, DateTime startDate, DateTime endDate, List<string> participants)
        {
            if(user.username == creator.username)
            {
                string jsonStringToRemove = null;
                try
                {
                    jsonStringToRemove = File.ReadAllText("Appointments.json");
                }
                catch (Exception e)
                {
                    Debug.Write(e);
                }
                if (jsonStringToRemove != null)
                {
                    List<Appointment> appointmentList = JsonConvert.DeserializeObject<List<Appointment>>(jsonStringToRemove);
                    Appointment appointmentToRemove = appointmentList.Find(appointment => appointment.title == this.title);
                    var convertedJson = JsonConvert.SerializeObject(appointmentList, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("Appointments.json", convertedJson);
                }
                this.description = description;
                this.startDate = startDate;
                this.endDate = endDate;
                this.participants = participants;
            }
            else
            {
                MessageBox.Show("Not Creator!", "User Error");
            }
        }

        public bool SaveUpdatedAppointment()
        {
            string jsonStringToAdd = null;
            try
            {
                jsonStringToAdd = File.ReadAllText("Appointments.json");
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }

            if (jsonStringToAdd != null)
            {
                List<Appointment> appointmentList = JsonConvert.DeserializeObject<List<Appointment>>(jsonStringToAdd);
                if (!HasOverlap(appointmentList))
                {
                    appointmentList.Add(this);
                    var convertedJson = JsonConvert.SerializeObject(appointmentList, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("Appointments.json", convertedJson);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                List<Appointment> list = new List<Appointment> { this };
                var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("Appointments.json", convertedJson);
            }
            return true;
        }

        public void Delete()
        {
            if (MainWindow.sessionUser.username == creator.username)
            {
                string jsonStringToRemove = null;
                try
                {
                    jsonStringToRemove = File.ReadAllText("Appointments.json");
                }
                catch (Exception e)
                {
                    Debug.Write(e);
                }
                if (jsonStringToRemove != null)
                {
                    List<Appointment> appointmentList = JsonConvert.DeserializeObject<List<Appointment>>(jsonStringToRemove);
                    Appointment appointmentToRemove = appointmentList.Find(appointment => appointment.title == this.title);
                    var convertedJson = JsonConvert.SerializeObject(appointmentList, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("Appointments.json", convertedJson);
                }
            }
            else
            {
                MessageBox.Show("Not Creator!", "User Error");
            }
        }
    }
}