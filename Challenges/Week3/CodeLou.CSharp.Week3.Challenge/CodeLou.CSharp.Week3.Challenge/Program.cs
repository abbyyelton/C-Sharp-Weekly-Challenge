using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeLou.CSharp.Week3.Challenge
{
	class Program
	{
		static void Main(string[] args)
		{
			// Overview:
			// In this assignment, you will be creating a calendar application that will load and save data. An example of loading and saving data has been provided.
			// This calendar application will accept multiple event types which will be represented by their own class types. 
			// You will be dealing with Appointments, Meetings, and Reminders.

			// Task 1:
			// Create new classes that will represent the calendar items that you will be using. 
			// Each of your classes will inherit from the CalendarItemBase abstract class.
			// Reminders have already been created as an example.
			
			// Task 2:
			// Define Your Data
			// Appointments need to be assigned a start date and time, an end date and time, and a location.
			// Meetings need to be assigned a start date and time, an end date and time, a location, and attendees. You can decide what data you need for attendees.
			// Reminders need to be assigned a start date and time.
			// Hint: Use inheritance to make your life easier.

            // Task 3:
            // Add the missing code to the ReminderRepository. Hint: Look for instances of NotImplementedException.
            // Create repository classes for Appointments and Meetings. Use the ReminderRepository as an example.

			// Task 4:
			// We want our application to load data and to save data. The process for reminders has already been created. You will need to do the same thing
			// for the other data types.
			var reminderRepository = new ReminderRepository(); 
			if (File.Exists("Reminders.json")) //Note: these files are created in the same folder as your .exe
				//Note: What happens when this file is improperly formatte? Can you handle this case?
				reminderRepository.LoadFromJson(File.ReadAllText("Reminders.json"));
            var appointmentRepository = new AppointmentRepository();
            if (File.Exists("Appointments.json")) //Note: these files are created in the same folder as your .exe
                                                  //Note: What happens when this file is improperly formatte? Can you handle this case?
                appointmentRepository.LoadFromJson(File.ReadAllText("Appointments.json"));
            var meetingRepository = new MeetingRepository();
            if (File.Exists("Meetings.json")) //Note: these files are created in the same folder as your .exe
                                                  //Note: What happens when this file is improperly formatte? Can you handle this case?
                meetingRepository.LoadFromJson(File.ReadAllText("Meetings.json"));

            // Hint: var appointmentRepository = new AppointmentRepository(); etc...

            // Task 5:
            // Fill in the missing options A, V, F, D for all classes
            var sessionEnded = false;
			while (!sessionEnded)
			{
                Console.Clear();
				Console.WriteLine("Q: save and quit");
				Console.WriteLine("A: add item");
				Console.WriteLine("V: view all");
				Console.WriteLine("F: find by date");
				Console.WriteLine("D: delete an item");
				Console.WriteLine();

				Console.Write("Select an action: ");
				var selectedOption = Console.ReadKey().KeyChar;
				Console.Clear();

				switch (selectedOption)
				{
					case ('Q'):
						//End the session when they select q
						sessionEnded = true;
						break;
					case ('A'):
						Console.WriteLine("A: Appointment");
						Console.WriteLine("M: Meeting");
						Console.WriteLine("R: Reminder");
						Console.WriteLine();
						Console.Write("Select a type:");
                        var selectedType = Console.ReadKey().KeyChar;
						Console.Clear();

						switch (selectedType)
						{//switch statements require a "break;", be careful not to experience this error
							case ('A'):
                                var newAppointment = appointmentRepository.Create();
                                Console.Write("What Date/Time Does the appointment start? (Format MM/DD/YYYY HH:mm) : ");
                                string startTime = Console.ReadLine();
                                try
                                {
                                    newAppointment.StartTime = DateTime.ParseExact(startTime, "MM/dd/yyyy HH:mm", null);
                                }
                                catch (Exception)
                                { }
                                Console.Write("What Date/Time Does the appointment end? (Format MM/DD/YYYY HH:mm) : ");
                                string endTime = Console.ReadLine();
                                try
                                {
                                    newAppointment.EndTime = DateTime.ParseExact(endTime, "MM/dd/yyyy HH:mm", null);
                                }
                                catch (Exception)
                                { }
                                Console.Write("Where is the appointment?: ");
                                newAppointment.Location = Console.ReadLine();
                                break;
                            case ('M'):
                                var newMeeting = meetingRepository.Create();
                                Console.Write("What Date/Time Does the appointment start? (Format MM/DD/YYYY HH:mm) : ");
                                startTime = Console.ReadLine();
                                try
                                {
                                    newMeeting.StartTime = DateTime.ParseExact(startTime, "MM/dd/yyyy HH:mm", null);
                                }
                                catch (Exception)
                                { }
                                Console.Write("What Date/Time Does the appointment end? (Format MM/DD/YYYY HH:mm) : ");
                                endTime = Console.ReadLine();
                                try
                                {
                                    newMeeting.EndTime = DateTime.ParseExact(endTime, "MM/dd/yyyy HH:mm", null);
                                }
                                catch (Exception)
                                { }
                                Console.Write("Where is the appointment?: ");
                                newMeeting.Location = Console.ReadLine();
                                Console.Write("Who is attending? List all names separated by comma: ");
                                string attendees = Console.ReadLine();
                                newMeeting.Attendees = attendees.Split(',').Select( a => a.Trim()).ToArray(); 
                                break;
                            case ('R'):
                                var newReminder = reminderRepository.Create();
                                Console.Write("What Date/Time? (Format MM/DD/YYYY HH:mm) : ");
                                string reminderTime = Console.ReadLine();
                                try
                                {
                                    newReminder.StartTime = DateTime.ParseExact(reminderTime, "MM/dd/yyyy HH:mm", null);
                                }
                                catch(Exception)
                                { }
                                break;
							default:
                                //Note: The $"abc {variable} def" syntax below is new syntactic sugar in C# 6.0 that can be used 
                                //in place of string.Format() in previous versions of C#.
                                Console.WriteLine($"Invalid Type {selectedType}, press any key to continue.");
                                Console.ReadKey();
                                break;
						}
						
						break;
					case ('V'):
                        Console.Clear();
                        reminderRepository.Display();
                        appointmentRepository.Display();
                        meetingRepository.Display();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ('F'):
                        Console.Clear();
                        Console.Write("What date? (Format MM/DD/YYYY) : ");
                        string dateString = Console.ReadLine();
                        try
                        {
                            DateTime date = DateTime.Parse(dateString);
                            List<Reminder> reminders = reminderRepository.FindByDate(date).ToList();
                            List<Appointment> appointments = appointmentRepository.FindByDate(date).ToList();
                            List<Meeting> meetings = meetingRepository.FindByDate(date).ToList();
                            Console.Clear();
                            if ( reminders.Count == 0 && appointments.Count == 0 && meetings.Count == 0 )
                            {
                                Console.WriteLine("Nothing could be found for " + dateString + ". Press any key to continue.");
                                Console.ReadKey();
                            }
                            else
                            {
                                reminderRepository.Display(reminders);
                                appointmentRepository.Display(appointments);
                                meetingRepository.Display(meetings);
                                Console.WriteLine("Press any key to continue.");
                                Console.ReadKey();
                            }
                        }
                        catch(Exception)
                        {
                            Console.WriteLine($"Invalid Date {dateString}, press any key to continue.");
                            Console.ReadKey();
                            break;
                        }
                        break;
					case ('D'):
                        Console.WriteLine("A: Appointment");
                        Console.WriteLine("M: Meeting");
                        Console.WriteLine("R: Reminder");
                        Console.WriteLine();
                        Console.Write("Select a type to delete: ");
                        selectedType = Console.ReadKey().KeyChar;
                        Console.Clear();
                        Console.Write("What is the ID of the item you'd like to delete? ");
                        string idString = Console.ReadLine();
                        int id = -1;
                        try
                        {
                            id = int.Parse(idString);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Invalid ID {idString}, press any key to continue.");
                            Console.ReadKey();
                            break;
                        }
                        switch (selectedType)
                        {//switch statements require a "break;", be careful not to experience this error
                            case ('A'):
                                Appointment appointment = appointmentRepository.FindById(id);
                                if (appointment != null)
                                {
                                    appointmentRepository.Delete(appointment);
                                }
                                else
                                {
                                    Console.WriteLine($"Appointment not found for ID {idString}, press any key to continue.");
                                    Console.ReadKey();
                                    break;
                                }
                                break;
                            case ('M'):
                                Meeting meeting = meetingRepository.FindById(id);
                                if (meeting != null)
                                {
                                    meetingRepository.Delete(meeting);
                                }
                                else
                                {
                                    Console.WriteLine($"Meeting not found for ID {idString}, press any key to continue.");
                                    Console.ReadKey();
                                    break;
                                }
                                break;
                            case ('R'):
                                Reminder reminder = reminderRepository.FindById(id);
                                if (reminder != null)
                                {
                                    reminderRepository.Delete(reminder);
                                }
                                else
                                {
                                    Console.WriteLine($"Reminder not found for ID {idString}, press any key to continue.");
                                    Console.ReadKey();
                                    break;
                                }
                                break;
                        }
                        break;
					default:
						Console.WriteLine($"Invalid Option {selectedOption}, press any key to continue.");
                        Console.ReadKey();
                        break;
				}
			}
			File.WriteAllText("Reminders.json", reminderRepository.ToJson());
            File.WriteAllText("Appointments.json", appointmentRepository.ToJson());
            File.WriteAllText("Meetings.json", meetingRepository.ToJson());
        }
	}
}
