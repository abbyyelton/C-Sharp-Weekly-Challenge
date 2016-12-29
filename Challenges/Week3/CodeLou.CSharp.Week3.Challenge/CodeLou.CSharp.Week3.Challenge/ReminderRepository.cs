using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json; //Build the project to cause Visual Studio to load this external NuGet package.

namespace CodeLou.CSharp.Week3.Challenge
{
	public class ReminderRepository: ICalendarItemRepository<Reminder>
	{
		//Info: This is a neat type that allows you to lookup items by ID, be careful not to ask for an item that isn't there.
		private readonly Dictionary<int, Reminder> _dictionary; 

		public ReminderRepository()
		{
			_dictionary = new Dictionary<int, Reminder>();
		}

		public Reminder Create()
		{
            //Challenge: Can you find a more efficient way to do this?
            var nextAvailableId = 0;
            if ( _dictionary.Count > 0 )
            {
                nextAvailableId = _dictionary.Keys.Max() + 1;
            }

			var reminder = new Reminder();
			reminder.Id = nextAvailableId;
			_dictionary.Add(nextAvailableId, reminder);

			return reminder;
		}

        //Callenge: Are you finding that you are writing this same code many times? Is there a better way? 
        //Could you use inheritance?
		public Reminder FindById(int id)
		{
            if (id > -1)
            {
                try
                {
                    var reminder = new Reminder();
                    reminder = _dictionary[id];
                    return reminder;
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

		public Reminder Update(Reminder item)
		{
            if ( FindById(item.Id) != null )
            {
                _dictionary[item.Id] = item;
                return _dictionary[item.Id];
            }
            else
            {
                return null;
            }
		}

		public void Delete(Reminder item)
		{
            if (item != null)
            {
                _dictionary.Remove(item.Id);
            }
		}

		public IEnumerable<Reminder> FindByDate(DateTime date)
		{
            IEnumerable<Reminder> reminders = new List<Reminder>();

            reminders = from item in _dictionary
                       where (item.Value.StartTime.Date == date.Date)
                       select item.Value;

            return reminders;
        }

		public IEnumerable<Reminder> GetAllItems()
		{
            IEnumerable<Reminder> reminders = _dictionary.Values.ToList();
            
            return reminders;
        }

		public string ToJson()
		{
			return JsonConvert.SerializeObject(_dictionary, Formatting.Indented);
		}

		public void LoadFromJson(string json)
		{
			var dictionary = JsonConvert.DeserializeObject<Dictionary<int, Reminder>>(json);
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

        public void Display(List<Reminder> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("Reminder: " + item.StartTime);
            }
        }

    }
}
