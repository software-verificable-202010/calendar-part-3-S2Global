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
using System.Xml;

namespace CalendarApp
{
    [TestFixture]
    public class Appointment
    {
        public User creator;
        public List<User> participants;
        public string title;
        public string description;
        public DateTime startDate;
        public DateTime endDate;

        [JsonConstructor]
        public Appointment(string title, string description, DateTime startDate, DateTime endDate, User creator, List<User> participants)
        {
            this.title = title;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
            this.creator = creator;
            this.participants = participants;
        }

        public Appointment(string title, string description, DateTime startDate, DateTime endDate, User creator)
        {
            this.title = title;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
            this.creator = creator;
            List<User> participants = new List<User>();
            participants.Add(creator);
            this.participants = participants;
        }

        public void Save()
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
                var list = JsonConvert.DeserializeObject<List<Appointment>>(jsonStringToAdd);
                if (!list.Contains(this))
                {
                    list.Add(this);
                    var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("Appointments.json", convertedJson);
                }
            }
            else
            {
                List<Appointment> list = new List<Appointment> { this };
                var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("Appointments.json", convertedJson);
            }
        }

        [TestCase]
        public void Update(User user, string title, string description, DateTime startDate, DateTime endDate, List<User> participants)
        {
            if(user == creator)
            {
                string jsonStringToAdd = File.ReadAllText("Appointments");
                var list = JsonConvert.DeserializeObject<List<Appointment>>(jsonStringToAdd);

                if (list.Contains(this))
                {
                    this.title = title;
                    this.description = description;
                    this.startDate = startDate;
                    this.endDate = endDate;
                    this.participants = participants;
                    
                    list.Add(this);
                    var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("Appointments", convertedJson);
                }
            }
        }
    }
}