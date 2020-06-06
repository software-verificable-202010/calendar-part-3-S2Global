using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CalendarApp
{
    public class User
    {
        public string username;
         
        public User(string username)
        {
            this.username = username;
        }

        public void Save()
        {
            string jsonStringToAdd = null;
            try
            {
                jsonStringToAdd = File.ReadAllText("Users.json");
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }

            if (jsonStringToAdd != null)
            {
                var list = JsonConvert.DeserializeObject<List<string>>(jsonStringToAdd);
                if (!list.Contains(this.username))
                {
                    list.Add(this.username);
                    var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("Users.json", convertedJson);
                }
            }
            else
            {
                List<string> list = new List<string> { this.username };
                var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("Users.json", convertedJson);
            }
        }
    }
}