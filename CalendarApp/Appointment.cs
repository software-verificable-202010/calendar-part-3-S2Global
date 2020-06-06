using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
        public User[] participants;
        public string title;
        public string description;
        public DateTime startDate;
        public DateTime endDate;
        
        public Appointment(string title, string description, DateTime startDate, DateTime endDate, User creator, User[] participants)
        {
            this.title = title;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
            this.creator = creator;
            this.participants = participants;

            string jsonStringToAdd = File.ReadAllText("Appointments");
            var list = JsonConvert.DeserializeObject<List<Appointment>>(jsonStringToAdd);
            list.Add(this);
            var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText("Appointments", convertedJson);
        }

        [TestCase]
        public void Update(User user, string title, string description, DateTime startDate, DateTime endDate, User[] participants)
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