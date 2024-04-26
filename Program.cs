const int physicalSize = 31;
int datasize = 0;
string[] datafile;
string[] salesdata = new string[physicalSize];
double[] saleamount = new double[physicalSize];
string input = "";
string Filename = "";
string filePath = "";
bool notvalid = true;

loadMenu();
while(notvalid) {
  try {
    
    input = Prompt("Enter Choice ('M' to display menu): ").ToUpper();
    if(input == "L") {
      datasize = loadData();
    } else if(input == "S") {
      saveData();
    } else if(input == "D") {
      displayData();
    } else if(input == "A") {
      datasize = AddData();
    } else if(input == "E") {
      editData();
    } else if(input == "M") {
      loadMenu();
    } else if(input == "R") {

      while(true) {
        if(datasize == 0) {
          throw new Exception("No entries loaded from {Filename}. Please load a file into memory");
        } else {
          loadSubMenu();
          string subinput = Prompt("\nEnter an Analysis Menu Choice: ").ToUpper();
          if(subinput == "A") {
            double avesale = saleamount.Sum()/ datasize;
            Console.WriteLine($"Average amount in sales is: {avesale:c2}");
          } else if(subinput == "H") {
            double highestSales = saleamount.Max();
            Console.WriteLine($"Highest amount in sales is: {highestSales:c2}");
          } else if(subinput == "L") {
            double lowestSales = saleamount.Min();
            Console.WriteLine($"Lowest amount in sales is: {lowestSales:c2}");
          } else if(subinput == "G") {
            createGraph();
          } else if(subinput == "R") {
            throw new Exception("Returning to Main Menu");
          }
        }
      }

    } else if (input == "Q") {
      notvalid = false;
      throw new Exception("Thank you for using this application. Come back anytime.");
    }


  } catch (Exception ex) {
    Console.WriteLine(ex.Message);
  }
}

void loadMenu() {
  Console.WriteLine(" MENU OPTION ");  
	Console.WriteLine("L) Load Values from File to Memory");
	Console.WriteLine("S) Save Values from Memory to File");
	Console.WriteLine("D) Display Values in Memory");
	Console.WriteLine("A) Add Value in Memory");
	Console.WriteLine("E) Edit Value in Memory");
	Console.WriteLine("R) Analysis Menu");
  Console.WriteLine("M) Display Main Menu");
	Console.WriteLine("Q) Quit");
}

void loadSubMenu() {
  Console.WriteLine(" ANALYSIS MENU OPTIONS "); 
  Console.WriteLine($"A) Get Average of Values in Memory");
	Console.WriteLine($"H) Get Highest Value in Memory");
	Console.WriteLine($"L) Get Lowest Value in Memory");
	Console.WriteLine($"G) Graph Values in Memory");
	Console.WriteLine($"R) Return to Main Menu");
}

string GetFileName()
{
  string Filename = "";
  do
  {
    Filename = Prompt("Enter file name including .csv or .txt: ");
  } while (string.IsNullOrWhiteSpace(Filename));
  return Filename;
}


int loadData() {
  Filename = GetFileName();
  filePath = $"data/{Filename}";
  datasize = 0;
  datafile = File.ReadAllLines(filePath);
  
  for(int i = 0; i < datafile.Length; i++) {
    string[] items = datafile[i].Split(',');
    if(i != 0) {
      salesdata[datasize] = items[0];
      saleamount[datasize] = double.Parse(items[1]);
      datasize++;
    }
  }

  Console.WriteLine($"\nLoad complete. {Filename} has {datasize} data entries");
  return datasize;
}

void saveData() {
  datasize++;
  filePath = $"data/{Filename}";
  string[] items = new string[datasize];
  int itemnum = 0;
  items[0] = "Entry Date, Amount";
  for(int i = 1; i < datasize; i++) {
    items[i] = $"{salesdata[itemnum]}, {saleamount[itemnum]}";
    itemnum++;
  }
  File.WriteAllLines(filePath,items);
  Console.WriteLine($"All Data successfully written to file at: {Path.GetFullPath(filePath)}");
}

void displayData() {
  if(datasize == 0) {
    throw new Exception($"No Entries loaded from {Filename}. Please load a file to memory or add a value in memory");
  } else {
    Console.Clear();
    Console.WriteLine($"\nCurrent Loaded Entries:  {datasize}\n");
    Console.WriteLine("{0,4} {1,-15} {2,10:}\n", "[#]", "Entry Date", "Amount");
    for (int i = 0; i < datasize; i++) {
      Console.WriteLine("{0,4} {1,-15} {2,10:f2}", "["+i+"]", salesdata[i], saleamount[i]);
    }
  }
}

int AddData() {
  if(datasize >= 31) {
    Console.WriteLine($"Data is already full. Can't add anymore entry");
  } else {
    Console.WriteLine($"Number of Data: {datasize}");
    string inputDate = checkData("date");
    string inputSales = checkData("amount");

    salesdata[datasize] = inputDate;
    saleamount[datasize] = double.Parse(inputSales);
    datasize++;

    Console.WriteLine($"\nSuccessfully added to temporary memory. \n{inputDate}, {inputSales:c2}");
  }
  
  return datasize;
}

void editData() {
  if(datasize == 0) {
    throw new Exception($"No Entries loaded from {Filename}. Please load a file to memory or add a value in memory");
  } else {
    displayData();
    while(true) {
      try { 
        Console.Write($"Choose index of data to edit [0-{datasize-1}]: ");
        int datanumb = int.Parse(Console.ReadLine().Trim());
        if(datanumb >=0 && datanumb < datasize) {
          Console.WriteLine($"\nYou are editing this data: \n{salesdata[datanumb],-15} {saleamount[datanumb], 10:c2}");

          Console.Write($"Enter Amount of Sales: (0-1000): ");
          double inputSales = double.Parse(Console.ReadLine());
          saleamount[datanumb] = inputSales;
          break;
        }
      } catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }
    }
    Console.WriteLine($"Successfully updated data.");
  }
  
}

void createGraph() {
  Console.WriteLine($"=== Sales of the month of {Filename} ===");

  Console.WriteLine($"money");
  Array.Sort(salesdata, saleamount, 0, datasize);

  int money = 1000;
  string perLine = "";

  while(money >= 0 ) {
    Console.Write($"{money, 4}|");

    string[] saleday = salesdata[0].Split('-');

    for(int i = 1; i <= physicalSize; i++) {
      string formatDay = i.ToString("00");
      int dayIndex = Array.IndexOf(salesdata, $"{saleday[0]}-{formatDay}-{saleday[2]}"); 

      if(dayIndex != -1) {
        if(saleamount[dayIndex] >= money && saleamount[dayIndex] <= (money + 49)) {
          perLine += $"{saleamount[dayIndex], 7:f2}";
          break;
        } else {
          perLine += $"{' ', 5}";
        }
      } else {
        perLine += $"{' ', 5}";
      }  
    }
    Console.WriteLine($"{perLine}");
    perLine = "";
    money -= 50;
  }

  string line = "-----";
  string days = "";

  for(int i = 1; i <= physicalSize; i++) {
    string formatDay = i.ToString("00");
    line += "----";
    days += $"{formatDay, 5}";
  }
  Console.WriteLine($"{line}");
  Console.Write($"Date|");
  Console.Write($"{days}");
  Console.WriteLine();
  
}

string Prompt(string prompt) {
  string response = "";
  Console.Write(prompt);
  response = Console.ReadLine().Trim();
  return response;
}

string checkData(string dataType) {
  string myData = "";
  while(true) {
    try {
      if(dataType == "date") {
        Console.Write($"Enter Date of Sales: (MM-dd-YYYY): ");
        myData = Console.ReadLine();
        if(salesdata.Contains(myData)) {
          Console.WriteLine($"Data already exist on this date. Please enter another date or choose Edit Data");
        } else {
          break;
        }
      } else if(dataType == "amount") {
        Console.Write($"Enter Amount of Sales: (0-1000): ");
        myData = Console.ReadLine();

        if(double.Parse(myData) < 0 || double.Parse(myData) > 1000) {
          Console.WriteLine($"Invalid sales amount. Must be between 0 and 1000");
        } else {
          break;
        }
      }
    } catch (Exception ex) {
      Console.WriteLine(ex.Message);
    }
  } 
  return myData;
}

