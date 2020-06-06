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

            try
            {
                string jsonStringToAdd = File.ReadAllText("Users");
                var list = JsonConvert.DeserializeObject<List<User>>(jsonStringToAdd);
                list.Add(this);
                var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("Users", convertedJson);
            }
            catch (IOException e)
            {
                Debug.Write(e);
                List<User> list = new List<User> { this };
                var convertedJson = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("Users", convertedJson);
            }
        }

        public void SignIn(string username)
        {
            try
            {
                string jsonStringToAdd = File.ReadAllText("Users");
                var list = JsonConvert.DeserializeObject<List<User>>(jsonStringToAdd);
            }
            catch (IOException e)
            {
                Debug.Write(e);
            }
        }
    }
}