   
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsApp;


namespace WindowsFormsApp2
{
    public class NewList
    {
        private MainForm mainForm;
        private SingIn form;
        private Patients patientsForm;
        private Password ps;
        private Timer timer1;
        private Label labelDate;
        private string role, ids, from, name;
        private ComboBox comboBoxDiagnos;
        private TextBox textBoxRecomendation;
        private string connectionString = "Data Source=PC-ANDREI\\SQLEXPRESS;Initial Catalog=УП;Integrated Security=True";

        public NewList(MainForm form1, SingIn signInForm, Patients patientsForms, Password pss, string role,string name)
        {
            mainForm = form1;
            form = signInForm;
            patientsForm = patientsForms;
            this.role = role;
            this.name = name;
            ps= pss;
            timer1 = new Timer { Interval = 1000 };
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        public NewList(MainForm form1, SingIn signInForm,  Password pss, string role, string name)
        {
            mainForm = form1;
            form = signInForm;
            this.role = role;
            this.name = name;
            ps = pss;
            timer1 = new Timer { Interval = 1000 };
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        public void ShowNewListForm(string idd, string from, string idf, string rolef, string login, string password, string isActive)
        {
            ids= idd;
            this.from = from;
            mainForm.Controls.Clear();
            mainForm.ClientSize = new Size(1200, 800);
            mainForm.create_logo();
            mainForm.Text = "Новая запись";

            Label labelName = CreateLabel($"Имя {role}а:\n {name}", "#FFFFFF", 12, ContentAlignment.MiddleRight);
            labelDate = CreateLabel(string.Empty, "#FFFFFF", 16, ContentAlignment.TopLeft, ColorTranslator.FromHtml("#7608AA"));


            Button buttonMain = CreateButton("Главная", "#FFFFFF", "#6500B0", ButtonMain_Click);
            Button buttonExit = CreateButton("Выход", "#FFFFFF", "#6500B0", ButtonExit_Click);
            Button buttonBack = CreateButton("Назад", "#FFFFFF", "#6500B0", ButtonBack_Click);

            Panel formPanel = CreatePanel(ColorTranslator.FromHtml("#6F2A5F"));

            if (role == "Администратор")
            {
                if (from == "patient")
                {
                    ShowNewPatientForm(formPanel);
                }
                else if (from == "Schedule")
                {
                    ShowNewSchedule(formPanel);
                }
            }
            if (role == "Врач")
            {
                ShowPatients(idd,  formPanel);
           
            }
            if (role == "Сис. администратор")
            {
                if (from=="save")
                {
                    ShowPassword(formPanel, null, null, null, null, null);
                }
                else if (from == "edit")
                {
                    ShowPassword(formPanel,  idf,  rolef,  login,  password,  isActive);
                }

            }
             

            mainForm.Controls.Add(labelName);
            mainForm.Controls.Add(labelDate);
;
            mainForm.Controls.Add(buttonMain);
            mainForm.Controls.Add(buttonExit);
            mainForm.Controls.Add(buttonBack);
            mainForm.Controls.Add(formPanel);

            SetElementBounds(labelName, labelDate , buttonMain, buttonExit, buttonBack, formPanel);
            mainForm.Resize += (s, e) => SetElementBounds(labelName, labelDate, buttonMain, buttonExit, buttonBack, formPanel);
        }
        public void ShowPatients(string idd,  Panel formPanel)
        {
            ids =idd;
            Label nameOfPacient = CreateLabel("", "#FFFFFF", 16, ContentAlignment.TopLeft);
            Label dateOfBirthsday = CreateLabel("", "#FFFFFF", 16, ContentAlignment.TopLeft);
            Label ladelAdres = CreateLabel("", "#FFFFFF", 16, ContentAlignment.TopLeft);
            comboBoxDiagnos = CreateComboBox(16, "#000000", "#FFFFFF");
            textBoxRecomendation = CreateTextBox();

            Button buttonSave = CreateButton("Сохранить", "#FFFFFF", "#6500B0", ButtonSave_Click);
            buttonSave.FlatAppearance.BorderSize = 0;
            Button buttonReset = CreateButton("Отчистить", "#FFFFFF", "#6500B0", (sender, e) => { textBoxRecomendation.Clear(); comboBoxDiagnos.Text = ""; });
            buttonReset.FlatAppearance.BorderSize = 0;


            formPanel.Controls.Add(nameOfPacient);
            formPanel.Controls.Add(dateOfBirthsday);
            formPanel.Controls.Add(ladelAdres);
            formPanel.Controls.Add(comboBoxDiagnos);
            formPanel.Controls.Add(textBoxRecomendation);
            formPanel.Controls.Add(buttonReset);
            formPanel.Controls.Add(buttonSave);


            // Устанавливаем размеры и позиции элементов
            SetElementBounds(  nameOfPacient, dateOfBirthsday, ladelAdres, comboBoxDiagnos, textBoxRecomendation, buttonSave, buttonReset);

            mainForm.Resize += (s, e) => SetElementBounds ( nameOfPacient, dateOfBirthsday, ladelAdres, comboBoxDiagnos, textBoxRecomendation, buttonSave, buttonReset);

            string query = "SELECT Пациенты.ФИО, Пациенты.Дата_рождения, Пациенты.Адрес FROM Пациенты WHERE Пациенты.Номер =" + idd + "; ";
            string query1 = "SELECT Название FROM Диагнозы";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nameOfPacient.Text = reader["ФИО"].ToString();
                            dateOfBirthsday.Text = reader["Дата_рождения"].ToString();
                            ladelAdres.Text = reader["Адрес"].ToString();
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query1, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxDiagnos.Items.Add(reader["Название"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }

        }

        private void ShowNewPatientForm(Panel parentPanel)
        {
            Label lblName = CreateLabel("ФИО:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            TextBox txtName = CreateTextBox();

            Label lblBirthDate = CreateLabel("Дата рождения:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            TextBox txtBirthDate = CreateTextBox();

            Label lblPhone = CreateLabel("Телефон:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            TextBox txtPhone = CreateTextBox();

            Label lblAddress = CreateLabel("Адрес:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            TextBox txtAddress = CreateTextBox();

            Button btnSave = CreateButton("Сохранить", "#FFFFFF", "#6500B0", (s, e) =>
            {
                SaveNewPatient(txtName.Text, txtBirthDate.Text, txtPhone.Text, txtAddress.Text);
            });

            parentPanel.Controls.Add(lblName);
            parentPanel.Controls.Add(txtName);
            parentPanel.Controls.Add(lblBirthDate);
            parentPanel.Controls.Add(txtBirthDate);
            parentPanel.Controls.Add(lblPhone);
            parentPanel.Controls.Add(txtPhone);
            parentPanel.Controls.Add(lblAddress);
            parentPanel.Controls.Add(txtAddress);
            parentPanel.Controls.Add(btnSave);

            SetNewPatientElementBounds(parentPanel, lblName, txtName, lblBirthDate, txtBirthDate, lblPhone, txtPhone, lblAddress, txtAddress, btnSave);
            
        }

        private void SaveNewPatient(string name, string birthDate, string phone, string address)
        {
            string query = "INSERT INTO Пациенты (ФИО, Дата_рождения, Телефон, Адрес) VALUES (@Name, @BirthDate, @Phone, @Address)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@BirthDate", birthDate);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Address", address);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Пациент успешно добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void ShowNewSchedule(Panel parentPanel)
        {
            Label lblName = CreateLabel("ФИО:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            ComboBox comboBoxName = CreateComboBox(16, "#000000", "#FFFFFF");

            Label lblhDate = CreateLabel("дата:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            TextBox comboBoxDate = CreateTextBox();

            Label lblstrtTm = CreateLabel("Время начала:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            ComboBox comboBoxStrtTm = CreateComboBox(16, "#000000", "#FFFFFF");

            Label lblFnshTm = CreateLabel("Время конца:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            ComboBox comboBoxsFnshTm = CreateComboBox(16, "#000000", "#FFFFFF");

            Label lblRoom = CreateLabel("Кабинет:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            ComboBox comboBoxRoom = CreateComboBox(16, "#000000", "#FFFFFF");

            Button btnSave = CreateButton("Сохранить", "#FFFFFF", "#6500B0", (s, e) =>
            {
                SaveNewSchedule(comboBoxName.Text, comboBoxDate.Text, comboBoxStrtTm.Text, comboBoxsFnshTm.Text, comboBoxRoom.Text);
            });

            parentPanel.Controls.Add(lblName);
            parentPanel.Controls.Add(comboBoxName);
            parentPanel.Controls.Add(lblhDate);
            parentPanel.Controls.Add(comboBoxDate);
            parentPanel.Controls.Add(lblstrtTm);
            parentPanel.Controls.Add(comboBoxStrtTm);
            parentPanel.Controls.Add(lblFnshTm);
            parentPanel.Controls.Add(comboBoxsFnshTm);
            parentPanel.Controls.Add(lblRoom);
            parentPanel.Controls.Add(comboBoxRoom);
            parentPanel.Controls.Add(btnSave);
            insertToCombobox( comboBoxName,  comboBoxStrtTm,  comboBoxsFnshTm,  comboBoxRoom);

            SetNewScheduleElementBounds(lblName, comboBoxName, lblhDate, comboBoxDate, lblstrtTm, comboBoxStrtTm, lblFnshTm, comboBoxsFnshTm, lblRoom,comboBoxRoom , btnSave);
        }
        private void insertToCombobox(ComboBox comboBoxName, ComboBox comboBoxStrtTm, ComboBox comboBoxsFnshTm, ComboBox comboBoxRoom)
        {
            comboBoxName.Items.Clear();
            string query2 = @"SELECT ФИО FROM Врачи";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string doctorName = reader["ФИО"].ToString();
                                comboBoxName.Items.Add(doctorName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Заполнение времени начала
            DateTime dt = DateTime.ParseExact("07:30", "HH:mm", null);
            TimeSpan interval = TimeSpan.FromMinutes(30);
            while (dt.TimeOfDay <= TimeSpan.Parse("21:00"))
            {
                comboBoxStrtTm.Items.Add(dt.ToString("HH:mm"));
                dt = dt.Add(interval);
            }

            // Заполнение времени окончания
            dt = DateTime.ParseExact("07:30", "HH:mm", null).Add(interval); // Начало с 8:00
            while (dt.TimeOfDay <= TimeSpan.Parse("21:30")) // Время конца на 30 минут позже
            {
                comboBoxsFnshTm.Items.Add(dt.ToString("HH:mm"));
                dt = dt.Add(interval);
            }

            // Заполнение комнат
            for (int i = 1; i <= 100; i++)
            {
                comboBoxRoom.Items.Add(i);
            }
        }

        private void SaveNewSchedule(string name, string date, string StrtTm, string FnshTm, string room)
        {
            string query = "INSERT INTO Расписание (Номер_врача, Дата, Время_начала, Время_концв, Кабинет) " +
                "VALUES (    (SELECT Номер FROM Врачи WHERE ФИО = @DoctorName), " +
                "   @Date, " +
                "    @StartTime, " +
                "   @EndTime, " +
                "    @Cabinet);";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorName", name);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@StartTime", StrtTm);
                        command.Parameters.AddWithValue("@EndTime", FnshTm);
                        command.Parameters.AddWithValue("@Cabinet", room);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Пациент успешно добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void ShowPassword(Panel parentPanel,string idf, string rolef, string login, string password, string isActive)
        {
            parentPanel.Controls.Clear();
            Label lblName = CreateLabel("ФИО:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            ComboBox comboBoxName = CreateComboBox(16, "#000000", "#FFFFFF");

            Label lblRole = CreateLabel("Роль:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            ComboBox comboBoxRoole = CreateComboBox(16, "#000000", "#FFFFFF");

            Label lbllogin = CreateLabel("Логин:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            TextBox TxtBxLogin = CreateTextBox();

            Label lblPsswrd = CreateLabel("Пароль:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            TextBox TxtBxPsswrd = CreateTextBox();

            Label lblTrue = CreateLabel("Действующий:", "#FFFFFF", 14, ContentAlignment.MiddleLeft);
            ComboBox comboBoxTrue = CreateComboBox(16, "#000000", "#FFFFFF");

            Button btnSave;
            string namef="";
            if(from == "edit")
            {
                string query = @" Select Врачи.ФИО
                From Врачи
                where Врачи.Номер =@id";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", idf);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    namef= reader["ФИО"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
                comboBoxName.Text = namef;
                comboBoxRoole.Text = rolef;
                TxtBxLogin.Text = login;
                TxtBxPsswrd.Text = password;
                comboBoxTrue.Text = isActive;
                btnSave = CreateButton("Сохранить", "#FFFFFF", "#6500B0", (s, e) =>
                {
                    UpdatePassword(idf, namef, comboBoxRoole.Text, TxtBxLogin.Text, TxtBxPsswrd.Text, comboBoxTrue.Text);
                    ; 
                });


            }
            else
            {
                 btnSave = CreateButton("Сохранить", "#FFFFFF", "#6500B0", (s, e) =>
                {
                    SaveNewPassword(comboBoxName.Text, comboBoxRoole.Text, TxtBxLogin.Text, TxtBxPsswrd.Text, comboBoxTrue.Text);
                });
            }


            // Заполнение комбобоксов
            insertCombobox(comboBoxName, comboBoxRoole, comboBoxTrue);

            // Добавление элементов в панель
            parentPanel.Controls.Add(lblName);
            parentPanel.Controls.Add(comboBoxName);
            parentPanel.Controls.Add(lblRole);
            parentPanel.Controls.Add(comboBoxRoole);
            parentPanel.Controls.Add(lbllogin);
            parentPanel.Controls.Add(TxtBxLogin);
            parentPanel.Controls.Add(lblPsswrd);
            parentPanel.Controls.Add(TxtBxPsswrd);
            parentPanel.Controls.Add(lblTrue);
            parentPanel.Controls.Add(comboBoxTrue);
            parentPanel.Controls.Add(btnSave);

            // Установка размеров и позиций элементов
            SetElementBounds(lblName, comboBoxName, lblRole, comboBoxRoole, lbllogin, TxtBxLogin, lblPsswrd, TxtBxPsswrd, lblTrue, comboBoxTrue, btnSave);
        }




        private void SaveNewPassword(string name, string role, string login, string password, string isActive)
        {
            string query = @"
        IF NOT EXISTS (
            SELECT 1 
            FROM Пароли 
            WHERE Номер_врача = (SELECT Номер FROM Врачи WHERE ФИО = @DoctorName)
        )
        BEGIN
            INSERT INTO Пароли (Номер_врача, Роль, Логин, Пароль, Действующий)
            VALUES (
                (SELECT Номер FROM Врачи WHERE ФИО = @DoctorName), 
                @Role, 
                @Login, 
                @Password, 
                @IsActive
            )
        END";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorName", name);
                        command.Parameters.AddWithValue("@Role", role);
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@IsActive", isActive);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно добавлена.");
                        }
                        else
                        {
                            MessageBox.Show("Запись для указанного врача уже существует.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            ps.ShowPassword();
        }
        private void UpdatePassword(string id, string name, string role, string login, string password, string isActive)
        {
            string query = @"
        UPDATE Пароли 
         SET   Логин = @Login,
             Роль = @Role, 
            Пароль = @Password, 
            Действующий = @IsActive 
            WHERE Номер = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", ids);
                        command.Parameters.AddWithValue("@Role", role);
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@IsActive", isActive);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно обновлена.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось найти запись для обновления.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void insertCombobox(ComboBox comboBoxName, ComboBox comboBoxRoole, ComboBox comboBoxTrue)
        {
            comboBoxName.Items.Clear();
            string query2 = @"SELECT ФИО  FROM Врачи";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string doctorName = reader["ФИО"].ToString();
                                comboBoxName.Items.Add(doctorName);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            query2 = @"SELECT Роль, COUNT(*) AS Количество 
                    FROM Пароли
                    GROUP BY Роль;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string doctorName = reader["Роль"].ToString();
                                comboBoxRoole.Items.Add(doctorName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            comboBoxTrue.Items.Add(0);
            comboBoxTrue.Items.Add(1);

        }

        private Label CreateLabel(string text, string textColor, int fontSize, ContentAlignment alignment, Color? backColor = null)
        {
            return new Label
            {
                Text = text,
                AutoSize = false,
                Font = new Font("Times New Roman", fontSize, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml(textColor),
                BackColor = backColor ?? Color.Transparent,
                TextAlign = alignment
            };
        }

        private TextBox CreateTextBox()
        {
            return new TextBox
            {
                Font = new Font("Times New Roman", 14, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.Black,
                AutoSize = false
            };
        }

        private Button CreateButton(string text, string textColor, string backgroundColor, EventHandler clickHandler)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml(textColor),
                BackColor = ColorTranslator.FromHtml(backgroundColor),
                FlatStyle = FlatStyle.Flat
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += clickHandler;
            return button;
        }

        private Panel CreatePanel(Color backColor)
        {
            return new Panel { BackColor = backColor };
        }
        public static ComboBox CreateComboBox( int fontSize, string foreColorHex , string backColorHex )
        {
            return new ComboBox
            {
                AutoSize = false,
                Font = new Font("Times New Roman", fontSize, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml(foreColorHex),
                BackColor = ColorTranslator.FromHtml(backColorHex),
            };
        }

        private void SetElementBounds(Label labelName, Label labelDate, Button buttonMain, Button buttonExit, Button buttonBack, Panel formPanel)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;

            mainForm.pictureBox.SetBounds((int)(width * 0.03), (int)(height * 0.03), (int)(width * 0.1), (int)(height * 0.13));
            mainForm.labelLogo.SetBounds((int)(width * 0.15), (int)(height * 0.05), (int)(width * 0.3), (int)(height * 0.07));
            labelName.SetBounds((int)(width * 0.4), (int)(height * 0.04), (int)(width * 0.33), (int)(height * 0.07));
            labelDate.SetBounds((int)(width * 0.03), (int)(height * 0.17), (int)(width * 0.3), (int)(height * 0.05));

            buttonMain.SetBounds((int)(width * 0.75), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            buttonExit.SetBounds((int)(width * 0.85), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            buttonBack.SetBounds((int)(width * 0.85), (int)(height * 0.85), (int)(width * 0.07), (int)(height * 0.05));

            formPanel.SetBounds((int)(width * 0.1), (int)(height * 0.25), (int)(width * 0.8), (int)(height * 0.58));
        }
        private void SetElementBounds( Label nameOfPacient,
        Label dateOfBirthsday, Label ladelAdres, ComboBox comboBoxDiagnos, TextBox textBoxRecomendation, Button buttonSave, Button buttonReset)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;


            nameOfPacient.SetBounds((int)(width * 0.13), (int)(height * 0.02), (int)(width * 0.33), (int)(height * 0.05));
            dateOfBirthsday.SetBounds((int)(width * 0.13), (int)(height * 0.09), (int)(width * 0.33), (int)(height * 0.05));
            ladelAdres.SetBounds((int)(width * 0.13), (int)(height * 0.16), (int)(width * 0.33), (int)(height * 0.05));
            comboBoxDiagnos.SetBounds((int)(width * 0.13), (int)(height * 0.23), (int)(width * 0.33), (int)(height * 0.10));
            buttonSave.SetBounds((int)(width * 0.5), (int)(height * 0.50), (int)(width * 0.1), (int)(height * 0.05));
            buttonReset.SetBounds((int)(width * 0.61), (int)(height * 0.50), (int)(width * 0.1), (int)(height * 0.05));
            textBoxRecomendation.SetBounds((int)(width * 0.13), (int)(height * 0.40), (int)(width * 0.33), (int)(height * 0.05));
        }
        private void SetNewPatientElementBounds(Panel parentPanel, Label lblName, TextBox txtName, Label lblBirthDate, TextBox txtBirthDate, Label lblPhone, TextBox txtPhone, Label lblAddress, TextBox txtAddress, Button btnSave)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;

            lblName.SetBounds((int)(width * 0.01), (int)(height * 0.02), (int)(width * 0.13), (int)(height * 0.06));
            txtName.SetBounds((int)(width * 0.15), (int)(height * 0.02), (int)(width * 0.33), (int)(height * 0.05));

            lblBirthDate.SetBounds((int)(width * 0.01), (int)(height * 0.09), (int)(width * 0.13), (int)(height * 0.05));
            txtBirthDate.SetBounds((int)(width * 0.15), (int)(height * 0.09), (int)(width * 0.33), (int)(height * 0.05));

            lblPhone.SetBounds((int)(width * 0.01), (int)(height * 0.16), (int)(width * 0.13), (int)(height * 0.05));
            txtPhone.SetBounds((int)(width * 0.15), (int)(height * 0.16), (int)(width * 0.33), (int)(height * 0.05));

            lblAddress.SetBounds((int)(width * 0.01), (int)(height * 0.23), (int)(width * 0.13), (int)(height * 0.05));
            txtAddress.SetBounds((int)(width * 0.15), (int)(height * 0.23), (int)(width * 0.33), (int)(height * 0.05));

            btnSave.SetBounds((int)(width * 0.13), (int)(height * 0.40), (int)(width * 0.33), (int)(height * 0.05));
        }
        private void SetNewScheduleElementBounds(Label labelName, ComboBox comboBoxName, Label lblhDate, TextBox comboBoxDate, Label lblstrtTm, ComboBox comboBoxStrtTm, Label lblFnshTm,
            ComboBox comboBoxsFnshTm, Label lblRoom, ComboBox comboBoxRoom , Button btnSave)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;

            labelName.SetBounds((int)(width * 0.01), (int)(height * 0.22), (int)(width * 0.13), (int)(height * 0.07));
            comboBoxName.SetBounds((int)(width * 0.15), (int)(height * 0.02), (int)(width * 0.33), (int)(height * 0.05));

            lblhDate.SetBounds((int)(width * 0.01), (int)(height * 0.09), (int)(width * 0.13), (int)(height * 0.05));
            comboBoxDate.SetBounds((int)(width * 0.15), (int)(height * 0.09), (int)(width * 0.33), (int)(height * 0.05));

            lblstrtTm.SetBounds((int)(width * 0.01), (int)(height * 0.16), (int)(width * 0.13), (int)(height * 0.05));
            comboBoxStrtTm.SetBounds((int)(width * 0.15), (int)(height * 0.16), (int)(width * 0.33), (int)(height * 0.05));

            lblFnshTm.SetBounds((int)(width * 0.01), (int)(height * 0.23), (int)(width * 0.13), (int)(height * 0.05));
            comboBoxsFnshTm.SetBounds((int)(width * 0.15), (int)(height * 0.23), (int)(width * 0.33), (int)(height * 0.05));

            lblRoom.SetBounds((int)(width * 0.01), (int)(height * 0.30), (int)(width * 0.13), (int)(height * 0.05));
            comboBoxRoom.SetBounds((int)(width * 0.15), (int)(height * 0.30), (int)(width * 0.33), (int)(height * 0.05));

            btnSave.SetBounds((int)(width * 0.13), (int)(height * 0.47), (int)(width * 0.33), (int)(height * 0.05));
        }
        private void SetElementBounds(Label lblName, ComboBox comboBoxName, Label lblRole, ComboBox comboBoxRoole, 
            Label lbllogin, TextBox txtBxLogin, Label lblPsswrd, TextBox txtBxPsswrd, Label lblTrue, ComboBox comboBoxTrue, Button btnSave)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;


            lblName.SetBounds((int)(width * 0.01), (int)(height * 0.03), (int)(width * 0.13), (int)(height * 0.05));
            comboBoxName.SetBounds((int)(width * 0.15), (int)(height * 0.03), (int)(width * 0.33), (int)(height * 0.06));

            lblRole.SetBounds((int)(width * 0.01), (int)(height * 0.09), (int)(width * 0.13), (int)(height * 0.05));
            comboBoxRoole.SetBounds((int)(width * 0.15), (int)(height * 0.09), (int)(width * 0.33), (int)(height * 0.05));

            lbllogin.SetBounds((int)(width * 0.01), (int)(height * 0.16), (int)(width * 0.13), (int)(height * 0.05));
            txtBxLogin.SetBounds((int)(width * 0.15), (int)(height * 0.16), (int)(width * 0.33), (int)(height * 0.05));

            lblPsswrd.SetBounds((int)(width * 0.01), (int)(height * 0.23), (int)(width * 0.13), (int)(height * 0.05));
            txtBxPsswrd.SetBounds((int)(width * 0.15), (int)(height * 0.23), (int)(width * 0.33), (int)(height * 0.05));

            lblTrue.SetBounds((int)(width * 0.01), (int)(height * 0.30), (int)(width * 0.13), (int)(height * 0.05));
            comboBoxTrue.SetBounds((int)(width * 0.15), (int)(height * 0.30), (int)(width * 0.33), (int)(height * 0.05));

            btnSave.SetBounds((int)(width * 0.13), (int)(height * 0.50), (int)(width * 0.33), (int)(height * 0.05));
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (labelDate != null)
            {
                labelDate.Text = DateTime.Now.ToString("dd.MM.yyyy       HH:mm:ss");
            }
        }
        public void ButtonSave_Click(object sender, EventArgs e)
        {
            string query2 = "INSERT INTO Истории_болезней (Номер_врача, Номер_пациента, Дата_явки, Диагноз, Рекомендации) VALUES (@DoctorId, @PatientId, @Date, @Diagnos, @Recommendation)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorId", mainForm.idDoctor);
                        command.Parameters.AddWithValue("@PatientId", ids);
                        command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Diagnos", comboBoxDiagnos.SelectedIndex + 1);
                        command.Parameters.AddWithValue("@Recommendation", textBoxRecomendation.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Данные успешно сохранены.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}");
            }
        }


        private void ButtonMain_Click(object sender, EventArgs e) { mainForm.Controls.Clear();form.SignIn(); }

        private void ButtonExit_Click(object sender, EventArgs e) {
            mainForm.Controls.Clear(); mainForm.InitializeMainForm(); }

        private void ButtonBack_Click(object sender, EventArgs e) {mainForm.Controls.Clear(); form.SignIn();}
    }
}



