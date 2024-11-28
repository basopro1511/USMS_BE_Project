using AddSchedule;
using System.Text.Json;

class Program
{
	static List<Schedule> schedules = new List<Schedule>();

	static async Task Main()
	{
		Console.WriteLine("Schedule System\n=================================================================");

		while (true)
		{
			Console.WriteLine("\nChoose an option:");
			Console.WriteLine("1. View Schedule");
			Console.WriteLine("2. Create Schedule");
			Console.WriteLine("3. Assign teacher");
			Console.WriteLine("4. Exit");
			Console.Write("Your choice: ");

			if (!int.TryParse(Console.ReadLine(), out int option))
			{
				Console.WriteLine("Invalid input. Please enter a valid number.");
				continue;
			}

			switch (option)
			{
				case 1:
					ViewSchedule();
					break;

				case 2:
					await CreateSchedule();
					break;
				case 3:
					AddTeacherToSchedule();
					break;
				case 4:
					Console.WriteLine("\nExiting the program. Goodbye!");
					return;

				default:
					Console.WriteLine("\nInvalid choice. Please enter a number between 1 and 3.");
					break;
			}
		}
	}
	static void AddTeacherToSchedule()
	{
		if (!schedules.Any())
		{
			Console.WriteLine("No schedules available to add teachers.");
			return;
		}

		Console.WriteLine("\nChoose a schedule to add a teacher:");

		for (int i = 0; i < schedules.Count; i++)
		{
			var schedule = schedules[i];
			Console.WriteLine($"{i + 1}. Subject ID: {schedule.SubjectID}, Date: {schedule.Date}, Slot: {schedule.SlotID}");
		}

		int scheduleChoice;
		while (true)
		{
			Console.Write("Select a schedule by number: ");
			if (int.TryParse(Console.ReadLine(), out scheduleChoice) && scheduleChoice > 0 && scheduleChoice <= schedules.Count)
			{
				break;
			}
			Console.WriteLine("Invalid choice. Please select a valid schedule number.");
		}

		Console.WriteLine("\nSelect a teacher:");
		Console.WriteLine("1. Nam");
		Console.WriteLine("2. Hoang");
		Console.WriteLine("3. An");
		Console.WriteLine("4. Thang");
		Console.WriteLine("5. Thinh");

		int teacherChoice;
		while (true)
		{
			Console.Write("Select a teacher by number: ");
			if (int.TryParse(Console.ReadLine(), out teacherChoice) && teacherChoice >= 1 && teacherChoice <= 5)
			{
				string teacherName;

				switch (teacherChoice)
				{
					case 1:
						teacherName = "Nam";
						break;
					case 2:
						teacherName = "Hoang";
						break;
					case 3:
						teacherName = "An";
						break;
					case 4:
						teacherName = "Thang";
						break;
					case 5:
						teacherName = "Thinh";
						break;
					default:
						Console.WriteLine("Invalid choice. Please select a valid teacher number.");
						continue;
				}

				schedules[scheduleChoice - 1].Teacher = teacherName;
				Console.WriteLine($"Teacher {teacherName} added to the schedule.");
				break;
			}
			else
			{
				Console.WriteLine("Invalid choice. Please select a valid teacher number.");
			}
		}
	}
	static void ViewSchedule()
	{
		if (!schedules.Any())
		{
			Console.WriteLine("No schedules available.");
			return;
		}

		Console.WriteLine("\nCurrent Schedules:");
		foreach (var schedule in schedules)
		{
			Console.WriteLine("---------------------------------------");
			Console.WriteLine($"Subject ID: {schedule.SubjectID}");
			if (schedule.IsOnline)
			{
				Console.WriteLine($"URL: {schedule.URL}");
			}
			else
			{
				Console.WriteLine($"Room: {schedule.RoomID}");
			}
			Console.WriteLine($"Teacher: {schedule.Teacher ?? "Nobody here"}");
			Console.WriteLine($"Date: {schedule.Date}");
			Console.WriteLine($"Slot: {schedule.SlotID}");
			Console.WriteLine($"Status: {(schedule.Status == 1 ? "Active" : "Inactive")}");
		}
	}

	static async Task<IEnumerable<SubjectModel>?> GetSubjects()
	{
		string apiUrl = "https://localhost:7286/api/Subjects";

		using (HttpClient client = new HttpClient())
		{
			try
			{
				HttpResponseMessage response = await client.GetAsync(apiUrl);

				string responseData = await response.Content.ReadAsStringAsync();

				var data = JsonSerializer.Deserialize<APIResponse>(responseData);
				if (data?.result is JsonElement jsonElement)
				{
					var subjects = JsonSerializer.Deserialize<List<SubjectModel>>(jsonElement.GetRawText());
					return subjects ?? Enumerable.Empty<SubjectModel>();
				}
				return null;
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Error calling API: {ex.Message}");
				return null;
			}
		}
	}

	static async Task CreateSchedule()
	{
		Schedule schedule = new Schedule();

		IEnumerable<SubjectModel> subjects = await GetSubjects();
		if (subjects == null || !subjects.Any())
		{
			Console.WriteLine("No subjects available.");
			return;
		}

		Console.WriteLine("\nAvailable Subjects:");
		int index = 1;
		foreach (var subject in subjects)
		{
			Console.WriteLine($"{index++}. {subject.subjectId} - {subject.subjectName}");
		}

		while (true)
		{
			Console.Write("Select a subject by number: ");
			if (int.TryParse(Console.ReadLine(), out int chosenSubject) && chosenSubject > 0 && chosenSubject <= subjects.Count())
			{
				schedule.SubjectID = chosenSubject;
				Console.WriteLine($"You selected: {subjects.ElementAt(chosenSubject - 1).subjectName}");
				break;
			}
			Console.WriteLine("Invalid subject selection. Please try again.");
		}

		Console.Write("Online (1 = True, 0 = False): ");
		bool isValid = false;
		while (!isValid)
		{
			string input = Console.ReadLine() ?? "0";
			if (input == "1")
			{
				schedule.IsOnline = true;
				isValid = true;
			}
			else if (input == "0")
			{
				schedule.IsOnline = false;
				isValid = true;
			}
			else
			{
				Console.WriteLine("Invalid input. Please enter 1 for True or 0 for False.");
			}
		}


		if (schedule.IsOnline == false)
		{
			int[] availableRooms = { 101, 201, 301 };

			Console.Write("1. 101\n2. 201\n3. 301\nSelect a room by number: ");

			int roomChoice;
			while (true)
			{
				if (int.TryParse(Console.ReadLine(), out roomChoice) && roomChoice >= 1 && roomChoice <= availableRooms.Length)
				{
					schedule.RoomID = availableRooms[roomChoice - 1];
					break;
				}
				else
				{
					Console.WriteLine("Invalid choice. Please select a room between 1 and 3.");
				}
			}

		}
		else
		{
			Console.Write("Enter URL: ");
			schedule.URL = Console.ReadLine();
		}

		while (true)
		{
			Console.Write("Enter Date (yyyy-MM-dd): ");
			if (DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
			{
				schedule.Date = date;
				break;
			}
			Console.WriteLine("Invalid date format. Please try again.");
		}

		Console.WriteLine("1. 7h-9h15\n2. 9h30-11h45\n3. 13h-15h15\n4. 15h30-17h45");
		while (true)
		{
			Console.Write("Enter Slot ID: ");
			if (int.TryParse(Console.ReadLine(), out int slotId) && slotId >= 1 && slotId <= 4)
			{
				schedule.SlotID = slotId;
				break;
			}
			Console.WriteLine("Invalid Slot ID. Please enter a number between 1 and 4.");
		}

		if (schedule.IsOnline == false)
		{
			if (IsOccupied(schedule.RoomID, null, schedule.Date, schedule.SlotID))
			{
				Console.WriteLine($"Room {schedule.RoomID} is already occupied on {schedule.Date} during Slot {schedule.SlotID}.");
				return;
			}
		}
		else
		{
			if (IsOccupied(null, schedule.URL, schedule.Date, schedule.SlotID))
			{
				Console.WriteLine($"URL {schedule.URL} is already occupied on {schedule.Date} during Slot {schedule.SlotID}.");
				return;
			}
		}

		Console.Write("Enter Status (1 = Active, 0 = Inactive): ");
		schedule.Status = int.Parse(Console.ReadLine() ?? "0");

		schedules.Add(schedule);

		Console.WriteLine("\nSchedule Created Successfully:");
	}

	static bool IsOccupied(int? roomId, string? url, DateOnly date, int slotId)
	{
		if (roomId != null)
		{
			return schedules.Any(s => s.RoomID == roomId && s.Date == date && s.SlotID == slotId);
		}
		if (url != null)
		{
			return schedules.Any(s => s.URL == url && s.Date == date && s.SlotID == slotId);
		}
		return false;
	}
}
