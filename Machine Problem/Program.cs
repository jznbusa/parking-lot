using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace MachineProblem
{
    class Widgets
    {
        public Label Lbl(Control windowName, string text, int xpoint, int ypoint, int fontSize)
        {
            Label label = new Label
            {
                Text = text,
                Width = 100,
                Height = 100,
                AutoSize = true,
                Location = new Point(xpoint, ypoint),
                Font = new Font("Bahnschrift SemiLight", fontSize)
            };
            windowName.Controls.Add(label);
            return label;
        }
        public Button Btn(Control windowName, string name, string text, Color color, int width, int height, int xpoint, int ypoint, int fontSize)
        {
            Button button = new Button
            {
                Name = name,
                Size = new Size(width, height),
                AutoSize = true,
                Location = new Point(xpoint, ypoint),
                Text = text,
                Font = new Font("Bahnschrift SemiLight", fontSize),
                BackColor = color
            };
            windowName.Controls.Add(button);
            return button;
        }
    }
    class LoginForm
    {
        TextBox Username;
        TextBox Password;
        Form LoginWin;
        Widgets ww = new Widgets();
        public void Login()
        {
            LoginWin = new Form
            {
                Text = "Parking Ticket System",
                BackColor = Color.Linen,
                Width = 700,
                Height = 500,
                StartPosition = FormStartPosition.CenterScreen
            };
            ww.Lbl(LoginWin, "LOGIN", 300, 50, 20);
            ww.Lbl(LoginWin, "Email", 250, 100, 15);
            ww.Lbl(LoginWin, "Password", 250, 180, 15);

            Username = new TextBox()
            {
                Size = new Size(100, 100),
                Width = 180,
                Height = 100,
                Location = new Point(250, 130),
                AutoSize = true,
                Font = new Font("Bahnschrift SemiLight", 15)
            };
            Password = new TextBox()
            {
                Size = new Size(100, 100),
                Width = 180,
                Height = 100,
                Location = new Point(250, 210),
                AutoSize = true,
                Font = new Font("Bahnschrift SemiLight", 15)
            };
            Button LoginButton = ww.Btn(LoginWin, "Login", "Login", Color.PeachPuff, 150, 40, 265, 290, 15);
            LoginButton.Click += (object sender, EventArgs e) => CheckCredentials();

            LoginWin.Controls.Add(Username);
            LoginWin.Controls.Add(Password);
            LoginWin.ShowDialog();
        }
        public void CheckCredentials()
        {
            string Username = this.Username.Text;
            string Password = this.Password.Text;
            StreamReader reader = new StreamReader("login.txt", true);
            try
            {
                string content = reader.ReadToEnd();
                string[] lines = content.Split('\n');
                foreach (string line in lines)
                {
                    string[] account = line.Split(',');

                    if (Username == account[0].Replace("\r", "") && Password == account[1].Replace("\r", ""))
                    {
                        MessageBox.Show("Login successful.");
                        reader.Close();
                        LoginWin.Hide();
                        MainMenu MenuWindow = new MainMenu();
                        MenuWindow.MyMenu();
                        break;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Login failed.");
            }
            finally
            {
                reader.Close();
            }
        }
    }
    class MainMenu
    {
        Button ManageAccountBtn;
        Button ManageParkBtn;
        Button ClearParkBtn;
        Button ParkSpaceBtn;
        Button ExitBtn;

        Form menuWin;
        Widgets ww = new Widgets();
        public void MyMenu()
        {
            menuWin = new Form
            {
                Text = "Parking Ticket System",
                BackColor = Color.Linen,
                Width = 700,
                Height = 500,
                StartPosition = FormStartPosition.CenterScreen
            };
            ww.Lbl(menuWin, "MENU", 300, 50, 20);

            ManageAccountBtn = ww.Btn(menuWin, "Manage Account", "Manage Account", Color.PeachPuff, 200, 30, 240, 100, 15);
            ManageParkBtn = ww.Btn(menuWin, "Manage Parking Lot", "Manage Parking Lot", Color.PeachPuff, 200, 30, 240, 150, 15);
            ClearParkBtn = ww.Btn(menuWin, "Clear Parking Lot", "Clear Parking Lot", Color.PeachPuff, 200, 30, 240, 200, 15);
            ParkSpaceBtn = ww.Btn(menuWin, "Check Space", "Check Space", Color.PeachPuff, 200, 30, 240, 250, 15);
            ExitBtn = ww.Btn(menuWin, "Exit", "Exit", Color.PeachPuff, 200, 30, 240, 300, 15);

            ManageAccountBtn.Click += (object sender, EventArgs e) => ManageAcc();
            ManageParkBtn.Click += (object sender, EventArgs e) => ManagePark();
            ClearParkBtn.Click += (object sender, EventArgs e) => ClearParkingLot();
            ParkSpaceBtn.Click += (object sender, EventArgs e) => CheckParkSpace();
            ExitBtn.Click += (object sender, EventArgs e) => Exit();

            menuWin.ShowDialog();
        }

        public void ManageAcc()
        {
            //display the manage acc window
            menuWin.Hide();
            ManageAccount ManAcc = new ManageAccount();
            ManAcc.ManageAcc();
        }
        public void ManagePark()
        {
            //parking window
            menuWin.Hide();
            ParkingLot ParkLot = new ParkingLot();
            ParkLot.Floor("Floor 1");
        }
        public double CheckTotal()
        {
            string lines;
            double Sum = 0;
            // show total earning
            try
            {
                using (StreamReader reader = new StreamReader("Fees.txt"))
                {
                    while ((lines = reader.ReadLine()) != null)
                    {
                        Sum = Sum + double.Parse(lines);
                    }
                }
            }
            catch
            {
                MessageBox.Show("File does not exist.");
            }
            return Sum;
        }
        public void ClearParkingLot()
        {
            double Total = CheckTotal();
            string msg = string.Format("{0:N2}", Total);
            //get date
            DateTime today = DateTime.Today;
            if (Total == 0)
            {
                string display = string.Format("Date: {0}\n" +
                    "There are no earnings yet...", today.ToString("MM/dd/yyyy"));
                MessageBox.Show(display);
            }
            else
            {
                if (File.Exists("Floor 1.txt"))
                {
                    //clear parking lot
                    File.WriteAllText("Floor 1.txt", String.Empty);
                    //also clear fees
                    File.WriteAllText("Fees.txt", String.Empty);
                }
                if (File.Exists("Floor 2.txt"))
                {
                    File.WriteAllText("Floor 2.txt", String.Empty);
                    File.WriteAllText("Fees.txt", String.Empty);
                }
                if (File.Exists("Floor 3.txt"))
                {
                    File.WriteAllText("Floor 3.txt", String.Empty);
                    File.WriteAllText("Fees.txt", String.Empty);
                }
                if (File.Exists("Floor 4.txt"))
                {
                    File.WriteAllText("Floor 4.txt", String.Empty);
                    File.WriteAllText("Fees.txt", String.Empty);
                }
                if (File.Exists("Floor 5.txt"))
                {
                    File.WriteAllText("Floor 5.txt", String.Empty);
                    File.WriteAllText("Fees.txt", String.Empty);
                }

                string display = string.Format("Parking lot cleared.\n" +
                    "Date: {0}\n" +
                    "Total earning of the day is: P {1}", today.ToString("MM/dd/yyyy"), msg);

                MessageBox.Show(display);
            }
        }
        // this method will count the data in the file. These data are the occupied lots
        public int CheckFiles(string FileName)
        {
            string[] file = File.ReadAllLines(FileName);
            int count = 0;
            foreach (string line in file)
            {
                string[] info = line.Split(',');
                string Status = info[3];
                if (Status == "Occupied")
                {
                    count++;
                }
            }
            return count;
        }
        public void CheckParkSpace()
        {
            int Occupied = 0;
            if (File.Exists("Floor 1.txt"))
            {
                Occupied = CheckFiles("Floor 1.txt");
            }
            if (File.Exists("Floor 2.txt"))
            {
                Occupied = CheckFiles("Floor 2.txt");
            }
            if (File.Exists("Floor 3.txt"))
            {
                Occupied = CheckFiles("Floor 3.txt");
            }
            if (File.Exists("Floor 4.txt"))
            {
                Occupied = CheckFiles("Floor 4.txt");
            }
            if (File.Exists("Floor 5.txt"))
            {
                Occupied = CheckFiles("Floor 5.txt");
            }

            int Space = 100 - Occupied;
            MessageBox.Show("Available car space per floor: " + Space);
        }
        public void Exit()
        {
            menuWin.Close();
            //exit window
        }
    }
    class ManageAccount
    {
        Form accWin;
        Widgets ww = new Widgets();
        public void ManageAcc()
        {
            accWin = new Form
            {
                Text = "Parking Ticket System",
                BackColor = Color.Linen,
                Width = 700,
                Height = 500,
                StartPosition = FormStartPosition.CenterScreen
            };
            ww.Lbl(accWin, "MANAGE ACCOUNT", 225, 50, 20);

            Button CreateAcc = ww.Btn(accWin, "Create Account", "Create Account", Color.PeachPuff, 200, 30, 240, 125, 15);
            Button DelAcc = ww.Btn(accWin, "Delete Account", "Delete Account", Color.PeachPuff, 200, 30, 240, 200, 15);
            Button Back = ww.Btn(accWin, "Back", "Back", Color.LightBlue, 60, 40, 10, 350, 10);

            CreateAcc.Click += (object sender, EventArgs e) => NewAccount();
            DelAcc.Click += (object sender, EventArgs e) => RemoveAcc();
            Back.Click += (object sender, EventArgs e) => Return();

            accWin.ShowDialog();
        }

        public void NewAccount()
        {
            accWin.Hide();
            NewAccount Create = new NewAccount();
            Create.NewAccPage();
        }
        public void RemoveAcc()
        {
            accWin.Hide();
            DelAccount Del = new DelAccount();
            Del.Remove();
        }
        public void Return()
        {
            accWin.Hide();
            MainMenu MainPage = new MainMenu();
            MainPage.MyMenu();
        }
    }
    class NewAccount
    {
        TextBox Username;
        TextBox Password;
        Form CreateNew;
        Button CreateBtn;
        Button Back;
        Widgets ww = new Widgets();
        public void NewAccPage()
        {
            CreateNew = new Form
            {
                Text = "Parking Ticket System",
                BackColor = Color.Linen,
                Width = 700,
                Height = 500,
                StartPosition = FormStartPosition.CenterScreen
            };
            ww.Lbl(CreateNew, "CREATE ACCOUNT", 225, 50, 20);

            ww.Lbl(CreateNew, "Email", 250, 100, 15);
            ww.Lbl(CreateNew, "Password", 250, 180, 15);

            Username = new TextBox()
            {
                Size = new Size(100, 100),
                Width = 180,
                Height = 100,
                Location = new Point(250, 130),
                AutoSize = true,
                Font = new Font("Bahnschrift SemiLight", 15)
            };
            Password = new TextBox()
            {
                Size = new Size(100, 100),
                Width = 180,
                Height = 100,
                Location = new Point(250, 210),
                AutoSize = true,
                Font = new Font("Bahnschrift SemiLight", 15)
            };
            CreateBtn = ww.Btn(CreateNew, "Create account", "Create account", Color.PeachPuff, 150, 40, 265, 290, 15);
            Back = ww.Btn(CreateNew, "Back", "Back", Color.LightBlue, 60, 40, 10, 350, 10);

            CreateBtn.Click += (object sender, EventArgs e) => CreateBtn_Click();
            Back.Click += (object sender, EventArgs e) => Return();

            CreateNew.Controls.Add(Username);
            CreateNew.Controls.Add(Password);

            CreateNew.ShowDialog();
        }

        public void CreateBtn_Click()
        {
            string Username = this.Username.Text;
            string Password = this.Password.Text;
            try
            {
                using (StreamWriter writer = new StreamWriter("login.txt", true))
                {
                    string content = string.Format("{0},{1}", Username, Password);
                    writer.WriteLine(content);
                }
                MessageBox.Show("Registration successful.");
                this.Username.Clear();
                this.Password.Clear();
            }
            catch
            {
                MessageBox.Show("Registration successful.");
            }
        }
        public void Return()
        {
            CreateNew.Hide();
            ManageAccount ManagePage = new ManageAccount();
            ManagePage.ManageAcc();
        }

    }
    class DelAccount
    {
        Form Delete;
        Button Back;
        Button DelAcc;
        TextBox Input_Line;
        Label mylabel;
        Widgets ww = new Widgets();
        string Content;
        int LineNum = 0;

        public void Remove()
        {
            Delete = new Form
            {
                Text = "Parking Ticket System",
                BackColor = Color.Linen,
                Width = 700,
                Height = 500,
                StartPosition = FormStartPosition.CenterScreen
            };
            ww.Lbl(Delete, "REMOVE ACCOUNT", 225, 50, 20);
            Back = ww.Btn(Delete, "Back", "Back", Color.LightBlue, 60, 40, 10, 350, 10);
            Back.Click += (object sender, EventArgs e) => Return();
            string login = "login.txt";
            try
            {
                Display(login);
            }
            catch
            {
                ww.Lbl(Delete, "Empty...", 225, 100, 15);
            }
            Delete.ShowDialog();
        }
        public Label Display(string txtName)
        {
            int ypointnum = 100;
            string[] lines = File.ReadAllLines(txtName);
            foreach (var line in lines)
            {
                var array = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                Content = string.Format("Line {0}   {1},{2}", LineNum, array[0], array[1]);
                mylabel = ww.Lbl(Delete, Content, 230, ypointnum, 13);
                ypointnum += 30;
                LineNum++;
            }

            ww.Lbl(Delete, "Line to delete:", 150, 350, 13);
            ww.Lbl(Delete, "ENTER ONLY Username,Password", 150, 330, 13);
            Input_Line = new TextBox()
            {
                Width = 150,
                Height = 100,
                Location = new Point(270, 350),
                AutoSize = true,
                Font = new Font("Bahnschrift SemiLight", 13)
            };
            DelAcc = ww.Btn(Delete, "Delete", "Delete", Color.PeachPuff, 150, 30, 450, 350, 15);
            DelAcc.Click += (object sender, EventArgs e) => DelAcc_Click();

            Delete.Controls.Add(Input_Line);

            return mylabel;
        }
        public void DelAcc_Click()
        {

            try
            {

                string Line_to_delete = Input_Line.Text;

                string[] oldLogin = File.ReadAllLines("login.txt");
                foreach (string line in oldLogin)
                {
                    Console.WriteLine(line);
                }
                StreamWriter writer = new StreamWriter("login.txt");
                foreach (string line in oldLogin)
                {
                    if (line == Line_to_delete)
                    {
                        continue;
                    }
                    writer.WriteLine(line);
                }
                writer.Close();

                Input_Line.Clear();
                //Returns to previous page to avoid having to reshow the accounts               
                MessageBox.Show("Account successfully deleted.");
                Return();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public void Return()
        {
            Delete.Hide();
            ManageAccount ManagePage = new ManageAccount();
            ManagePage.ManageAcc();
        }
    }
    class ParkingLot
    {
        TextBox PlateNo;
        TextBox TimeIn;
        Form ParkingFloor;

        Button Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9, Button10;
        Button Button11, Button12, Button13, Button14, Button15, Button16, Button17, Button18, Button19, Button20;

        Button Floor1, Floor2, Floor3, Floor4, Floor5, Back;

        double Fee = 20;

        Widgets ww = new Widgets();

        public void Floor(string floorNum)
        {
            ParkingFloor = new Form
            {
                Text = "Parking Ticket System",
                BackColor = Color.Linen,
                Width = 700,
                Height = 500,
                StartPosition = FormStartPosition.CenterScreen
            };

            ww.Lbl(ParkingFloor, floorNum, 10, 10, 15);
            ww.Lbl(ParkingFloor, "PARKING LOT", 290, 10, 20);
            ww.Lbl(ParkingFloor, DateTime.Now.ToString("HH:mm"), 510, 10, 17);
            ww.Lbl(ParkingFloor, DateTime.Today.ToString("MM/dd/yy"), 570, 10, 17);

            Button1 = ww.Btn(ParkingFloor, "Button1", "Lot 1", Color.DarkSeaGreen, 100, 50, 120, 50, 10);
            Button2 = ww.Btn(ParkingFloor, "Button2", "Lot 2", Color.DarkSeaGreen, 100, 50, 230, 50, 10);
            Button3 = ww.Btn(ParkingFloor, "Button3", "Lot 3", Color.DarkSeaGreen, 100, 50, 340, 50, 10);
            Button4 = ww.Btn(ParkingFloor, "Button4", "Lot 4", Color.DarkSeaGreen, 100, 50, 450, 50, 10);
            Button5 = ww.Btn(ParkingFloor, "Button5", "Lot 5", Color.DarkSeaGreen, 100, 50, 570, 50, 10);

            Button6 = ww.Btn(ParkingFloor, "Button6", "Lot 6", Color.DarkSeaGreen, 100, 50, 120, 120, 10);
            Button7 = ww.Btn(ParkingFloor, "Button7", "Lot 7", Color.DarkSeaGreen, 100, 50, 230, 120, 10);
            Button8 = ww.Btn(ParkingFloor, "Button8", "Lot 8", Color.DarkSeaGreen, 100, 50, 340, 120, 10);
            Button9 = ww.Btn(ParkingFloor, "Button9", "Lot 9", Color.DarkSeaGreen, 100, 50, 450, 120, 10);
            Button10 = ww.Btn(ParkingFloor, "Button10", "Lot 10", Color.DarkSeaGreen, 100, 50, 570, 120, 10);

            Button11 = ww.Btn(ParkingFloor, "Button11", "Lot 11", Color.DarkSeaGreen, 100, 50, 120, 190, 10);
            Button12 = ww.Btn(ParkingFloor, "Button12", "Lot 12", Color.DarkSeaGreen, 100, 50, 230, 190, 10);
            Button13 = ww.Btn(ParkingFloor, "Button13", "Lot 13", Color.DarkSeaGreen, 100, 50, 340, 190, 10);
            Button14 = ww.Btn(ParkingFloor, "Button14", "Lot 14", Color.DarkSeaGreen, 100, 50, 450, 190, 10);
            Button15 = ww.Btn(ParkingFloor, "Button15", "Lot 15", Color.DarkSeaGreen, 100, 50, 570, 190, 10);

            Button16 = ww.Btn(ParkingFloor, "Button16", "Lot 16", Color.DarkSeaGreen, 100, 50, 120, 260, 10);
            Button17 = ww.Btn(ParkingFloor, "Button17", "Lot 17", Color.DarkSeaGreen, 100, 50, 230, 260, 10);
            Button18 = ww.Btn(ParkingFloor, "Button18", "Lot 18", Color.DarkSeaGreen, 100, 50, 340, 260, 10);
            Button19 = ww.Btn(ParkingFloor, "Button19", "Lot 19", Color.DarkSeaGreen, 100, 50, 450, 260, 10);
            Button20 = ww.Btn(ParkingFloor, "Button20", "Lot 20", Color.DarkSeaGreen, 100, 50, 570, 260, 10);

            Floor1 = ww.Btn(ParkingFloor, "Floor1", "Floor 1", Color.PeachPuff, 30, 30, 10, 50, 10);
            Floor2 = ww.Btn(ParkingFloor, "Floor2", "Floor 2", Color.PeachPuff, 30, 30, 10, 110, 10);
            Floor3 = ww.Btn(ParkingFloor, "Floor3", "Floor 3", Color.PeachPuff, 30, 30, 10, 170, 10);
            Floor4 = ww.Btn(ParkingFloor, "Floor4", "Floor 4", Color.PeachPuff, 30, 30, 10, 230, 10);
            Floor5 = ww.Btn(ParkingFloor, "Floor5", "Floor 5", Color.PeachPuff, 30, 30, 10, 290, 10);
            Back = ww.Btn(ParkingFloor, "Back", "Back", Color.LightBlue, 60, 40, 10, 350, 10);

            InputData(); //textbox designs here

            Button1.Click += (object sender, EventArgs e) => Buttons_Click(Button1, floorNum);
            Button2.Click += (object sender, EventArgs e) => Buttons_Click(Button2, floorNum);
            Button3.Click += (object sender, EventArgs e) => Buttons_Click(Button3, floorNum);
            Button4.Click += (object sender, EventArgs e) => Buttons_Click(Button4, floorNum);
            Button5.Click += (object sender, EventArgs e) => Buttons_Click(Button5, floorNum);
            Button6.Click += (object sender, EventArgs e) => Buttons_Click(Button6, floorNum);
            Button7.Click += (object sender, EventArgs e) => Buttons_Click(Button7, floorNum);
            Button8.Click += (object sender, EventArgs e) => Buttons_Click(Button8, floorNum);
            Button9.Click += (object sender, EventArgs e) => Buttons_Click(Button9, floorNum);
            Button10.Click += (object sender, EventArgs e) => Buttons_Click(Button10, floorNum);
            Button11.Click += (object sender, EventArgs e) => Buttons_Click(Button11, floorNum);
            Button12.Click += (object sender, EventArgs e) => Buttons_Click(Button12, floorNum);
            Button13.Click += (object sender, EventArgs e) => Buttons_Click(Button13, floorNum);
            Button14.Click += (object sender, EventArgs e) => Buttons_Click(Button14, floorNum);
            Button15.Click += (object sender, EventArgs e) => Buttons_Click(Button15, floorNum);
            Button16.Click += (object sender, EventArgs e) => Buttons_Click(Button16, floorNum);
            Button17.Click += (object sender, EventArgs e) => Buttons_Click(Button17, floorNum);
            Button18.Click += (object sender, EventArgs e) => Buttons_Click(Button18, floorNum);
            Button19.Click += (object sender, EventArgs e) => Buttons_Click(Button19, floorNum);
            Button20.Click += (object sender, EventArgs e) => Buttons_Click(Button20, floorNum);

            Floor1.Click += (object sender, EventArgs e) => Floor_Click("Floor 1");
            Floor2.Click += (object sender, EventArgs e) => Floor_Click("Floor 2");
            Floor3.Click += (object sender, EventArgs e) => Floor_Click("Floor 3");
            Floor4.Click += (object sender, EventArgs e) => Floor_Click("Floor 4");
            Floor5.Click += (object sender, EventArgs e) => Floor_Click("Floor 5");
            Back.Click += (object sender, EventArgs e) => Return();

            ParkingFloor.ShowDialog();
        }

        public void InputData()
        {
            ww.Lbl(ParkingFloor, "Plate no.:", 100, 350, 13);
            PlateNo = new TextBox()
            {
                Width = 150,
                Height = 100,
                Location = new Point(200, 345),
                AutoSize = true,
                Font = new Font("Bahnschrift SemiLight", 13)
            };
            ww.Lbl(ParkingFloor, "Time in:", 400, 350, 13);
            TimeIn = new TextBox()
            {
                Width = 150,
                Height = 100,
                Location = new Point(500, 345),
                AutoSize = true,
                Font = new Font("Bahnschrift SemiLight", 13)
            };
            ParkingFloor.Controls.Add(PlateNo);
            ParkingFloor.Controls.Add(TimeIn);
        }
        public void First_Click(Button BtnNo, string floorNum)
        {
            string Plate = PlateNo.Text;
            string Time = TimeIn.Text;
            string Filename = floorNum + ".txt";
            string name = BtnNo.Name;

            if (Time == "" || Plate == "")
            {
                //do nothing so that an empty string wouldn't save to text file
            }
            else
            {
                try
                {
                    using (StreamWriter write = new StreamWriter(Filename, true))
                    {
                        string data = string.Format("{0},{1},{2},Occupied", Plate, Time, name);
                        write.WriteLine(data);
                    }
                    MessageBox.Show("Data saved successfully.");
                    //clear textboxes
                    PlateNo.Clear();
                    TimeIn.Clear();
                    //change state of button
                    BtnNo.BackColor = Color.PaleVioletRed;
                }
                catch
                {
                    MessageBox.Show("Failed to save data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void Second_Click(Button BtnNo, string floorNum)
        {
            BtnNo.BackColor = Color.DarkSeaGreen;
            string Filename = floorNum + ".txt";
            string TimeIn;
            string TimeOut = DateTime.Now.ToString("HH:mm");
            TimeSpan HourDifference;
            double Sum;
            try
            {

                string[] oldFile = File.ReadAllLines(Filename);
                foreach (string line in oldFile)
                {
                    string[] info = line.Split(',');
                    TimeIn = info[1];
                    //compute
                    HourDifference = Convert.ToDateTime(TimeIn) - Convert.ToDateTime(TimeOut);
                    Sum = (HourDifference.TotalHours * Fee) * -1;
                    string Total = string.Format("{0:N2}", Sum);

                    //Write total to Fees.txt
                    using (StreamWriter write = new StreamWriter("Fees.txt", true))
                    {
                        write.WriteLine(Total);
                    }
                    // display the parking fee
                    MessageBox.Show("The parking fee is " + Total);
                    break;
                }
                // Rewrite the file but replace the occupied with unoccupied
                StreamWriter writer = new StreamWriter(Filename);
                foreach (string line in oldFile)
                {
                    string[] info = line.Split(',');
                    string Button = info[2];
                    if (Button == BtnNo.Name)
                    {
                        string Status = info[3];
                        if (Status == "Occupied")
                        {
                            writer.WriteLine(line.Replace("Occupied", "Unoccupied"));
                            continue;
                        }
                    }
                    writer.WriteLine(line);
                }
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Buttons_Click(Button BtnName, string floorNum)
        {
            First_Click(BtnName, floorNum);
            BtnName.Click += (object sender, EventArgs e) => Second_Click(BtnName, floorNum);

        }
        public void Floor_Click(string floorNum)
        {
            ParkingFloor.Hide();
            //add text file parameter to retrieve data
            Floor(floorNum);
        }
        public void Return()
        {
            ParkingFloor.Hide();
            MainMenu MainPage = new MainMenu();
            MainPage.MyMenu();
        }

    }

    class Program : Form
    {
        static void Main(string[] args)
        {
            LoginForm login = new LoginForm();
            login.Login();

        }
    }
}
