using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json; //Build the project to cause Visual Studio to load this external NuGet package.

namespace CodeLou.CSharp.Week3.Challenge
{
    public class AppointmentRepository : ICalendarItemRepository<Appointment>
    {
        //Info: This is a neat type that allows you to lookup items by ID, be careful not to ask for an item that isn't there.
        private readonly Dictionary<int, Appointment> _dictionary;

        public AppointmentRepository()
        {
            _dictionary = new Dictionary<int, Appointment>();
        }

        public Appointment Create()
        {
            //Challenge: Can you find a more efficient way to do this?
            var nextAvailableId = 0;
            if (_dictionary.Count > 0)
            {
                nextAvailableId = _dictionary.Keys.Max() + 1;
            }

            var appointment = new Appointment();
            appointment.Id = nextAvailableId;
            _dictionary.Add(nextAvailableId, appointment);

            return appointment;
        }

        //Callenge: Are you finding that you are writing this same code many times? Is there a better way? 
        //Could you use inheritance?
        public Appointment FindById(int id)
        {
            if (id > -1)
            {
                try
                {
                    var appointment = new Appointment();
                    appointment = _dictionary[id];
                    return appointment;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public Appointment Update(Appointment item)
        {
            if (FindById(item.Id) != null)
            {
                _dictionary[item.Id] = item;
                return _dictionary[item.Id];
            }
            else
            {
                return null;
            }
        }

        public void Delete(Appointment item)
        {
            if (item != null)
            {
                _dictionary.Remove(item.Id);
            }
        }

        public IEnumerable<Appointment> FindByDate(DateTime date)
        {
            IEnumerable<Appointment> appointments = new List<Appointment>();

            appointments = from item in _dictionary
                        where (item.Value.StartTime.Date == date.Date)
                        select item.Value;

            return appointments;
        }

        public IEnumerable<Appointment> GetAllItems()
        {
            IEnumerable<Appointment> appointments = _dictionary.Values.ToList();

            return appointments;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_dictionary, Formatting.Indented);
        }

        public void LoadFromJson(string json)
        {
            var dictionary = JsonConvert.DeserializeObject<Dictionary<int, Appointment>>(json);
            foreach (var item in dictionary)
            {
                //This will add or update an item
                _dictionary[item.Key] = item.Value;
            }
        }

        public void Display()
        {
            Display(_dictionary.Values.ToList());
        }

        public void Display(List<Appointment> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("Appointment: " + item.StartTime + " - " + item.EndTime + " at " + item.Location);
            }
        }
    }
}

