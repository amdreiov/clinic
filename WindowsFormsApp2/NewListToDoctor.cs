using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp;

namespace WindowsFormsApp2
{
    internal class NewListToDoctor
    {
        private MainForm mainForm;
        private SingIn form;
        private Timer timer1;
        private Label labelDate;
        private Patients patientsForm;
        private Histories historiesForms;
        string name, id, date, adress, idDoctor, nform, role, names;
        string connectionString = "Data Source=PC-ANDREI\\SQLEXPRESS;Initial Catalog=УП;Integrated Security=True";
        public NewListToDoctor(MainForm form1, SingIn signInForm, Patients patientsForms, Histories historiessForm, string names)
        {
            patientsForm = patientsForms;
            this.names = names; 
            mainForm = form1;
            form = signInForm;
            historiesForms = historiessForm;
            timer1 = new Timer
            {
                Interval = 1000
            };
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }
        public void ShowPatients(string idd,string namee, string nforn, string role)
        {
            id = idd;
            name = namee;
            nform=nforn;
            // Настраиваем размеры формы и очищаем интерфейс
            mainForm.ClientSize = new Size(1200, 800);
            mainForm.Controls.Clear();
            mainForm.Text = "История пациента" + name;
            mainForm.create_logo();

            // Создаем элементы интерфейса
            Label labelName = new Label
            {
                Text = $"Имя {role}а:\n {names}",
                AutoSize = false,
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                TextAlign = ContentAlignment.MiddleRight
            };

            labelDate = new Label
            {
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                BackColor = ColorTranslator.FromHtml("#7608AA"),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label labelList = new Label
            {
                Text = "Вкладка | Новая запись к врачу",
                AutoSize = false,
                Font = new Font("Times New Roman", 14, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                BackColor = ColorTranslator.FromHtml("#A60800"),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Button buttonMain = new Button
            {
                Text = "Главная",
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                BackColor = ColorTranslator.FromHtml("#6500B0"),
                FlatStyle = FlatStyle.Flat
            };
            buttonMain.FlatAppearance.BorderSize = 0;
            buttonMain.Click += (sender, e) => { mainForm.Controls.Clear(); form.SignIn(); };

            Button buttonExit = new Button
            {
                Text = "Выход",
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                BackColor = ColorTranslator.FromHtml("#6500B0"),
                FlatStyle = FlatStyle.Flat
            };
            buttonExit.FlatAppearance.BorderSize = 0;
            buttonExit.Click += (sender, e) => { mainForm.Controls.Clear(); mainForm.InitializeMainForm(); };

            Panel backgroundPanel = new Panel
            {
                BackColor = ColorTranslator.FromHtml("#6F2A5F"),

            };

            Label nameOfPacient = new Label()
            {
                Text = "dbksbfkhsbdfksbkdf",
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
            };

            Label dateOfBirthsday = new Label()
            {
                Text = "srbjsbfhbdsfsbdfkhsd",
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
            };
            Label ladelAdres = new Label()
            {
                Text = "srbjsbfhbdsfsbdfkhsd",
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
            };

            ComboBox comboSpecialization = new ComboBox()
            {
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#000000"),
                BackColor = ColorTranslator.FromHtml("#FFFFFF"),
            };
            

            ComboBox comboDoctor = new ComboBox()
            {
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#000000"),
                BackColor = ColorTranslator.FromHtml("#FFFFFF"),
            };


            comboSpecialization.SelectedIndexChanged += (sender, e) =>
            {
                OnSpecializationSelectedIndexChanged(comboSpecialization, comboDoctor);
            };

            ComboBox comboDate = new ComboBox()
            {
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#000000"),
                BackColor = ColorTranslator.FromHtml("#FFFFFF"),
            };

            comboDoctor.SelectedIndexChanged += (sender, e) =>
            {
                onComboDateSelectedIndexChanged(comboDoctor, comboDate);
            };

            ComboBox comboTime = new ComboBox()
            {
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#000000"),
                BackColor = ColorTranslator.FromHtml("#FFFFFF"),
            };
            comboDate.SelectedIndexChanged += (sender, e) =>
            {
                onComboTimeSelectedIndexChangedSelectedIndexChanged(comboDate, comboTime);
            };

            Button buttonBack = new Button
            {
                Text = "Назад",
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                BackColor = ColorTranslator.FromHtml("#6500B0"),
                FlatStyle = FlatStyle.Flat
            };
            buttonBack.FlatAppearance.BorderSize = 0;
            buttonBack.Click += (sender, e) => { mainForm.Controls.Clear();
                if (nform == "patient patient")
                { 
                    patientsForm.ShowPatients("patient");
                }
                else if (nform == "patient today")
                {
                    patientsForm.ShowPatients("today");
                }
                else
                {
                    historiesForms.ShowPatients(id, name);
                }
            };

            Button buttonSave = new Button
            {
                Text = "Сохранить",
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                BackColor = ColorTranslator.FromHtml("#6500B0"),
                FlatStyle = FlatStyle.Flat
            };
            buttonSave.FlatAppearance.BorderSize = 0;

            buttonSave.Click += (sender, e) =>
            {
                // Проверяем заполнение обязательных полей
                if (string.IsNullOrEmpty(comboDate.Text) || string.IsNullOrEmpty(comboSpecialization.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query2 = @"INSERT INTO Записи (Номер_пациента, Номер_врача, Дата, Время)
                      VALUES (@PatientId, @DoctorId, @Date, @Time);";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query2, connection))
                        {
                            // Добавляем параметры
                            command.Parameters.AddWithValue("@PatientId", id); // ID пациента
                            command.Parameters.AddWithValue("@DoctorId", idDoctor); // ID врача

                            // Преобразуем дату
                            DateTime selectedDate;
                            if (!DateTime.TryParse(comboDate.Text, out selectedDate))
                            {
                                MessageBox.Show("Некорректный формат даты!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            command.Parameters.AddWithValue("@Date", selectedDate);

                            // Проверяем корректность времени
                            string selectedTime = comboTime.Text.Trim(); // Убираем лишние пробелы
                            TimeSpan time;
                            if (!TimeSpan.TryParse(selectedTime, out time))
                            {
                                MessageBox.Show("Некорректный формат времени!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            command.Parameters.AddWithValue("@Time", time);

                            // Выполняем запрос
                            command.ExecuteNonQuery();
                            MessageBox.Show("Данные успешно сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };




            Button buttonReset = new Button     //изменить
            {
                Text = "Отчистить",
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                BackColor = ColorTranslator.FromHtml("#6500B0"),
                FlatStyle = FlatStyle.Flat
            };
            buttonReset.FlatAppearance.BorderSize = 0;
            buttonReset.Click += (sender, e) => {  
                comboSpecialization.Items.Clear();
                comboDoctor.Items.Clear();
                comboTime.Items.Clear();
                comboDate.Items.Clear();
                comboDoctor.Text = "";
                comboTime.Text = "";
                comboDate.Text = "";
            };


            mainForm.Controls.Add(backgroundPanel);
            mainForm.Controls.Add(labelName);
            mainForm.Controls.Add(labelDate);
            mainForm.Controls.Add(labelList);
            backgroundPanel.Controls.Add(nameOfPacient);
            backgroundPanel.Controls.Add(dateOfBirthsday);
            backgroundPanel.Controls.Add(ladelAdres);
            backgroundPanel.Controls.Add(comboSpecialization);
            backgroundPanel.Controls.Add(comboDoctor);
            backgroundPanel.Controls.Add(comboTime);
            backgroundPanel.Controls.Add(comboDate);
            backgroundPanel.Controls.Add(buttonReset);
            backgroundPanel.Controls.Add(buttonSave);
            mainForm.Controls.Add(buttonMain);
            mainForm.Controls.Add(buttonExit);
            mainForm.Controls.Add(buttonBack);

            // Устанавливаем размеры и позиции элементов
            SetElementBounds(labelName, labelDate, buttonMain, buttonExit, buttonBack, labelList, backgroundPanel, nameOfPacient, 
                dateOfBirthsday, ladelAdres, comboSpecialization, comboDoctor, comboTime, comboDate,  buttonSave, buttonReset);

            mainForm.Resize += (s, e) => SetElementBounds(labelName, labelDate, buttonMain, buttonExit, buttonBack, labelList, backgroundPanel, nameOfPacient,
                dateOfBirthsday, ladelAdres, comboSpecialization, comboDoctor, comboTime, comboDate,  buttonSave, buttonReset);

            string query = "SELECT Пациенты.ФИО, Пациенты.Дата_рождения, Пациенты.Адрес FROM Пациенты WHERE Пациенты.Номер ='" + id + "'; ";
            string query1 = "SELECT Название FROM Специализации";

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
                            comboSpecialization.Items.Add(reader["Название"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }



        }
        // Метод для обработки события изменения выбранного элемента в comboSpecialization
        private void OnSpecializationSelectedIndexChanged(ComboBox comboSpecialization, ComboBox comboDoctor)
        {
            comboDoctor.Items.Clear(); 
            if (string.IsNullOrEmpty(comboSpecialization.Text))
            {
                MessageBox.Show("Выберите специализацию!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query2 = @"SELECT Врачи.ФИО, Врачи.Номер
                      FROM Врачи 
                      LEFT JOIN Врачи_Специализации ON Врачи.Номер = Врачи_Специализации.Номер_врача 
                      LEFT JOIN Специализации ON Специализации.ID = Врачи_Специализации.Номер_специализации 
                      WHERE Специализации.Название = @SpecializationName";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@SpecializationName", comboSpecialization.Text);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                string doctorName =reader["ФИО"].ToString();
                                comboDoctor.Items.Add(doctorName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void onComboDateSelectedIndexChanged(ComboBox comboDoctor, ComboBox comboDate)
        {
            comboDate.Items.Clear();
            if (string.IsNullOrEmpty(comboDoctor.Text))
            {
                MessageBox.Show("Выберите доктора!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query2 = @"SELECT Расписание.Дата, Врачи.Номер
                      FROM Расписание  
                      INNER JOIN Врачи ON Врачи.Номер = Расписание.Номер_врача 
                      WHERE Врачи.ФИО = @DoctorName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorName", comboDoctor.Text);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idDoctor = reader["Номер"].ToString();
                                string date = reader["Дата"].ToString();
                                comboDate.Items.Add(date);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void onComboTimeSelectedIndexChangedSelectedIndexChanged(ComboBox comboDate, ComboBox comboTime )
        {
            comboTime.Items.Clear();
            if (string.IsNullOrEmpty(comboDate.Text))
            {
                MessageBox.Show("Выберите доктора!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query2 = @"
                            WITH РасписаниеВрача AS (
                                SELECT 
                                    [Время_начала],
                                    [Время_концв]
                                FROM [УП].[dbo].[Расписание]
                                WHERE [Дата] = @Дата AND [Номер_врача] = @Номер_врача
                            ),
                            ВсеИнтервалы AS (
                                SELECT 
                                    DATEADD(MINUTE, 15 * (n.n - 1), r.[Время_начала]) AS Интервал_Начало,
                                    DATEADD(MINUTE, 15 * n.n, r.[Время_начала]) AS Интервал_Конец
                                FROM РасписаниеВрача r
                                CROSS APPLY (
                                    SELECT TOP ((DATEDIFF(MINUTE, r.[Время_начала], r.[Время_концв]) / 15) + 1)
                                        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
                                    FROM master.dbo.spt_values
                                ) n
                            ),

                            ЗанятыеИнтервалы AS (
                                SELECT 
                                    [Время] AS Занято_Начало
                                FROM [УП].[dbo].[Записи]
                                WHERE [Дата] = @Дата AND [Номер_врача] = @Номер_врача
                            )

                            SELECT 
                                Интервал_Начало,
                                Интервал_Конец
                            FROM ВсеИнтервалы i
                            WHERE NOT EXISTS (
                                SELECT 1
                                FROM ЗанятыеИнтервалы z
                                WHERE z.Занято_Начало = i.Интервал_Начало
                            )
                            ORDER BY Интервал_Начало;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@Дата", comboDate.Text);
                        command.Parameters.AddWithValue("@Номер_врача", Convert.ToInt32(idDoctor));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string time = reader["Интервал_Начало"].ToString();
                                comboTime.Items.Add(time);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
            private void SetElementBounds(Label labelName, Label labelDate, Button buttonMain, Button buttonExit, Button buttonBack, Label labelList, Panel backgroundPanel, Label nameOfPacient,
            Label dateOfBirthsday, Label ladelAdres, ComboBox comboSpecialization, ComboBox comboDoctor, ComboBox comboTime, ComboBox comboDate,  Button buttonSave, Button buttonReset)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;

            mainForm.pictureBox.SetBounds((int)(width * 0.03), (int)(height * 0.03), (int)(width * 0.1), (int)(height * 0.13));
            mainForm.labelLogo.SetBounds((int)(width * 0.15), (int)(height * 0.05), (int)(width * 0.3), (int)(height * 0.07));
            labelName.SetBounds((int)(width * 0.4), (int)(height * 0.04), (int)(width * 0.33), (int)(height * 0.06));
            buttonMain.SetBounds((int)(width * 0.75), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            buttonExit.SetBounds((int)(width * 0.85), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            labelList.SetBounds((int)(width * 0.75), (int)(height * 0.13), (int)(width * 0.24), (int)(height * 0.05));
            labelDate.SetBounds((int)(width * 0.03), (int)(height * 0.17), (int)(width * 0.30), (int)(height * 0.05));
            buttonBack.SetBounds((int)(width * 0.8), (int)(height * 0.85), (int)(width * 0.1), (int)(height * 0.07));
            backgroundPanel.SetBounds((int)(width * 0.1), (int)(height * 0.25), (int)(width * 0.8), (int)(height * 0.58));
            nameOfPacient.SetBounds((int)(width * 0.13), (int)(height * 0.02), (int)(width * 0.33), (int)(height * 0.05));
            dateOfBirthsday.SetBounds((int)(width * 0.13), (int)(height * 0.09), (int)(width * 0.33), (int)(height * 0.05));
            ladelAdres.SetBounds((int)(width * 0.13), (int)(height * 0.16), (int)(width * 0.33), (int)(height * 0.05));
            comboSpecialization.SetBounds((int)(width * 0.13), (int)(height * 0.23), (int)(width * 0.33), (int)(height * 0.10));
            comboDoctor.SetBounds((int)(width * 0.13), (int)(height * 0.30), (int)(width * 0.33), (int)(height * 0.10));
            comboTime.SetBounds((int)(width * 0.13), (int)(height * 0.44), (int)(width * 0.33), (int)(height * 0.10));
            comboDate.SetBounds((int)(width * 0.13), (int)(height * 0.37), (int)(width * 0.33), (int)(height * 0.10));
            buttonSave.SetBounds((int)(width * 0.5), (int)(height * 0.50), (int)(width * 0.1), (int)(height * 0.05));
            buttonReset.SetBounds((int)(width * 0.61), (int)(height * 0.50), (int)(width * 0.1), (int)(height * 0.05));
        }



        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (labelDate != null)
            {
                labelDate.Text = DateTime.Now.ToString("dd.MM.yyyy       HH:mm:ss");
            }
        }
    }
}

