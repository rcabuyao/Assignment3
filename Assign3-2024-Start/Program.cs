int physicalSize = 31;
int logicalSize = 0;

double minValue = 0.0;
double maxValue = 14.0;
double[] values = new double[physicalSize];
string[] dates = new string[physicalSize];

bool loopAgain = true;
  while (loopAgain)
  {
    try
    {
      DisplayMainMenu();
      string mainMenuChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();
      if (mainMenuChoice == "L")
        logicalSize = LoadFileValuesToMemory(dates, values);
      if (mainMenuChoice == "S")
        SaveMemoryValuesToFile(dates, values, logicalSize);
      if (mainMenuChoice == "D")
        DisplayMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "A")
        logicalSize = AddMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "E")
        EditMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "Q")
      {
        goAgain = false;
        throw new Exception("Bye, hope to see you again.");
      }
      if (mainMenuChoice == "R")
      {
        while (true)
        {
          if (logicalSize == 0)
			throw new Exception("No entries loaded. Please load a file into memory or add an entry to memory");
          DisplayAnalysisMenu();
          string analysisMenuChoice = Prompt("\nEnter an Analysis Menu Choice: ").ToUpper();
          if (analysisMenuChoice == "A")
            FindAverageOfValuesInMemory(values, logicalSize);
          if (analysisMenuChoice == "H")
            FindHighestValueInMemory(values, logicalSize);
          if (analysisMenuChoice == "L")
            FindLowestValueInMemory(values, logicalSize);
          if (analysisMenuChoice == "G")
            GraphValuesInMemory(dates, values, logicalSize);
          if (analysisMenuChoice == "R")
            throw new Exception("Returning to Main Menu");
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"{ex.Message}");
    }
  }

void DisplayMainMenu()
{
	Console.WriteLine("\nMain Menu");
	Console.WriteLine("L) Load Values from File to Memory");
	Console.WriteLine("S) Save Values from Memory to File");
	Console.WriteLine("D) Display Values in Memory");
	Console.WriteLine("A) Add Value in Memory");
	Console.WriteLine("E) Edit Value in Memory");
	Console.WriteLine("R) Analysis Menu");
	Console.WriteLine("Q) Quit");
}

void DisplayAnalysisMenu()
{
	Console.WriteLine("\nAnalysis Menu");
	Console.WriteLine("A) Find Average of Values in Memory");
	Console.WriteLine("H) Find Highest Value in Memory");
	Console.WriteLine("L) Find Lowest Value in Memory");
	Console.WriteLine("G) Graph Values in Memory");
	Console.WriteLine("R) Return to Main Menu");
}

string Prompt(string prompt)
{
	bool inValidInput = true;
	string myString = "";
	while (inValidInput)
	{
		try
		{
		Console.Write(prompt);
		myString = Console.Readline().Trim();
		if(string.IsNullOrEmpty(myString))
			throw new Exception($"Empty Input: Please enter something.");
		inValidInput = false;
		}
		catch (Exception ex)
		{
			Console.Writeline(ex.Message);
		}
	}
	return myString;
}

int LoadFileValuesToMemory(string[] dates, double[] values)
{
	string fileName = Prompt("Enter file name including .csv or .txt: ");
	int logicalSize = 0;
	string filePath = $"./data/{fileName}";
	if (!File.Exists(filePath))
		throw new Exception($"The file {fileName} does not exist.");
	string[] csvFileInput = File.ReadAllLines(filePath);
	for(int i = 0; i < csvFileInput.Length; i++)
	{
		Console.WriteLine($"lineIndex: {i}; line: {csvFileInput[i]}");
		string[] items = csvFileInput[i].Split(',');
		for(int j = 0; j < items.Length; j++)
		{
			Console.WriteLine($"itemIndex: {j}; item: {items[j]}");
		}
		if(i != 0)
		{
		dates[logicalSize] = items[0];
        values[logicalSize] = double.Parse(items[1]);
        logicalSize++;
		}
	}
    Console.WriteLine($"Load complete. {fileName} has {logicalSize} data entries");
	return logicalSize;
}

void DisplayMemoryValues(string[] dates, double[] values, int logicalSize)
{
	if(logicalSize == 0)
		throw new Exception($"No Entries loaded. Please load a file to memory or add a value in memory");
	Array.Sort(dates, values, 0, logicalSize);
	Console.WriteLine($"\nCurrent Loaded Entries: {logicalSize}");
	Console.WriteLine($"   Date     Value");
	for (int i = 0; i < logicalSize; i++)
		Console.WriteLine($"{dates[i]}   {values[i]}");
}

double PromptDoubleBetweenMinMax(string prompt, double min, double max)
{
	bool inValidInput = true;
	double num = 0;
	while (inValidInput)
	{
		try
		{
			Consle.Write($"{prompt} between {min:n2} and {max:n2}: ");
			num = double.Parse(Console.Readline());
			if (num < min || num > max)
				throw new Exception($"Invalid. Must be between {min} and {max}.");
			inValidInput = false;
		}
		catch (Exception ex)
		{
			Console.Writeline($"{ex.Message}");
		}
	}
	return num;
}

string PromptDate(string prompt)
{
	bool inValidInput = true;
	DateTime date = DateTime.Today;
	Console.Writeline(date);
	while (inValidInput)
	{
		try
		{
			Console.Write(prompt);
			date = DateTime.Parse(Console.Readline());
			Console.Writeline(date);
			inValidInput = false;
		}
		catch (Exception ex)
		{
			Console.Writeline($"{ex.Message}");
		}
	}
	return date.ToString("MM-dd-yyyy");
}

int AddMemoryValues(string[] dates, double[] values, int logicalSize)
{
	double value = 0.0;
	string dateString = "";

	dateString = PromptDate("Enter date format mm-dd-yyyy (eg 11-23-2023): ");
	bool found = false;
	for (int i = 0; i < logicalSize; i++)
		if (dates[i].Equals(dateString))
			found = true;
	if(found == true)
		throw new Exception($"{dateString} is already in memory. Edit entry instead.");
	value = PromptDoubleBetweenMinMaxMinMax($"Enter a double value", minValue, maxValue);
	dates[logicalSize] = dateString;
	values[logicalSize] = value;
	logicalSize++;
	return logicalSize;
}

void EditMemoryValues(string[] dates, double[] values, int logicalSize)
{
	double value = 0.0;
	string dateString = "";
	int foundIndex = 0;

	if(logicalSize == 0)
		throw new Exception($"No Entries loaded. Please load a file to memory or add a value in memory");
	dateString = PromptDate("Enter date format mm-dd-yyyy (eg 11-23-2023): ");
	bool found = false;
	for (int i = 0; i < logicalSize; i++)
		if (dates[i].Equals(dateString))
		{
			found = true;
			foundIndex = i;
		}
	if(found == false)
		throw new Exception($"{dateString} is not in memory. Add entry instead.");
	value = PromptDoubleBetweenMinMax($"Enter a double value", minValue, maxValue);
	values[foundIndex] = value;
}

double FindLowestValueInMemory(double[] values, int logicalSize)
{
	double min = values[0];
      for (int i = 0; i < values.Length; i++)
      {
        if (values[i] < min)
          min = values[i];
      }
	  return min;
}

double FindHighestValueInMemory(double[] values, int logicalSize)
{
	double max = values[0];
      for (int i = 0; i < values.Length; i++)
      {
        if (values[i] > max)
          max = values[i];
      }
      return max;
}

void FindAverageOfValuesInMemory(double[] values, int logicalSize)
{

}

void SaveMemoryValuesToFile(string[] dates, double[] values, int logicalSize)
{
	string fileName = Prompt("Enter file name including .csv or .txt: ");
	string filePath = $"./data/{fileName}";
	if (logicalSize == 0)
		throw new Exception("No entries loaded. Please load a file into memory");
	if (logicalSize > 1)
		Array.Sort (dates, values, 0, logicalSize);
	string[] csvLines = new string[logicalSize + 1];
	csvLines[0] = "dates,values";
	for (int i = 1; i <= logicalSize; i++)
	{
		csvLines[i] = $"{dates[i-1]},{values[i-1].ToString()}";
	}
	fileName.WriteAllLines(filePath, csvLines);
	Console.Writeline($"Save complete. {fileName} has {logicalSize} entries.");
}

void GraphValuesInMemory(string[] dates, double[] values, int logicalSize)
{
	Console.WriteLine("Not Implemented Yet");
	//TODO: Replace this code with yours to implement this function.
}