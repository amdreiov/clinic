using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp;

namespace WindowsFormsApp2
{
    public class Patients
    {
        private MainForm mainForm;
        private SingIn form;
        private Histories historiesForm;
        private Timer timer1;
        private Label labelDate;
        private NewList newListForm;
        private NewListToDoctor newListToDoctorForm;
        public string name, role, forrm, idd;

        public Patients(MainForm form1, SingIn signInForm, string rolee, string namee, string id)
        {
            mainForm = form1;
            form = signInForm;
            name = namee;
            role = rolee;
            idd=id;
            // Инициализация таймера
            timer1 = new Timer
            {
                Interval = 1000
            };
            timer1.Tick += Timer1_Tick;
            timer1.Start();

            // Инициализация вспомогательных форм
            historiesForm = new Histories(mainForm, signInForm, this, role, name);
            newListForm = new NewList(mainForm, signInForm, this, null, role, name);
            newListToDoctorForm = new NewListToDoctor(mainForm, form, this, historiesForm, name);
        }

        public void ShowPatients(string FORM)
        {
            forrm = FORM;
            // Настраиваем размеры формы и очищаем интерфейс
            mainForm.ClientSize = new Size(1200, 800);
            mainForm.Controls.Clear();
            mainForm.Text = "Список пациентов";
            mainForm.create_logo();

            // Создаем элементы интерфейса
            Label labelName = CreateLabel($"Имя {role}а:\n {name}", "#FFFFFF",12 , ContentAlignment.MiddleRight);
            labelDate = CreateLabel(string.Empty, "#FFFFFF", 16, ContentAlignment.MiddleCenter);
            labelDate.BackColor = ColorTranslator.FromHtml("#7608AA");
            Label labelList;
            if (forrm =="patient")
            {
                labelList = CreateLabel("Вкладка | Пациенты", "#FFFFFF", 16, ContentAlignment.MiddleCenter);
                labelList.BackColor = ColorTranslator.FromHtml("#A60800");
                mainForm.Controls.Add(labelList);
            }
            else
            {
                labelList = CreateLabel("Вкладка | Записи на сегодня", "#FFFFFF", 14, ContentAlignment.MiddleCenter);
                labelList.BackColor = ColorTranslator.FromHtml("#A60800");
                mainForm.Controls.Add(labelList);
            }
            

            Button buttonMain = CreateButton("Главная", "#FFFFFF", "#6500B0", ButtonMain_Click);
            Button buttonExit = CreateButton("Выход", "#FFFFFF", "#6500B0", ButtonExit_Click);
            Button buttonBack = CreateButton("Назад", "#FFFFFF", "#6500B0", ButtonBack_Click);
            Button buttonNewPatient;
            if (role !="Администратор")
            {
                buttonNewPatient=null;
            }
            else if(forrm=="patient")
            {
                buttonNewPatient = CreateButton("Новый пациент", "#FFFFFF", "#6500B0", ButtonNewPatient_Click);
            }
            else
            {
                buttonNewPatient=null;
            }
            

            // Создаем DataGridView
            DataGridView dataGridView = CreateDataGridView();

            // Загружаем данные из базы данных
            LoadPatientData(dataGridView);

            // Обработка нажатий на кнопки внутри таблицы
            dataGridView.CellClick += (s, e) => HandleCellClick(s, e, dataGridView);

            // Добавляем элементы на форму
            mainForm.Controls.Add(labelName);
            mainForm.Controls.Add(labelDate);

                mainForm.Controls.Add(buttonNewPatient);
            mainForm.Controls.Add(buttonMain);
            mainForm.Controls.Add(buttonExit);
            mainForm.Controls.Add(dataGridView);
            mainForm.Controls.Add(buttonBack);

            // Устанавливаем размеры и позиции элементов
            SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, buttonNewPatient);
            mainForm.Resize += (s, e) => SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, buttonNewPatient);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (labelDate != null)
            {
                labelDate.Text = DateTime.Now.ToString("dd.MM.yyyy       HH:mm:ss");
            }
        }

        private Label CreateLabel(string text, string textColor, int fontSize, ContentAlignment alignment)
        {
            return new Label
            {
                Text = text,
                AutoSize = false,
                Font = new Font("Times New Roman", fontSize, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml(textColor),
                TextAlign = alignment
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

        private DataGridView CreateDataGridView()
        {
            var dataGridView = new DataGridView
            {
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Times New Roman", 12),
                BackgroundColor = ColorTranslator.FromHtml("#A60800"),
                ForeColor = Color.White,
                RowHeadersVisible = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#A60800"),
                ForeColor = Color.White,
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            dataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#A60800"),
                ForeColor = Color.White,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                WrapMode = DataGridViewTriState.True
            };

            // Добавление основных колонок
            dataGridView.Columns.Add("id", "Номер");
            dataGridView.Columns.Add("Name", "ФИО");
            dataGridView.Columns.Add("Phone", "Телефон");
            dataGridView.Columns.Add("Address", "Адрес");

            // Добавление кнопок в зависимости от роли
            dataGridView.Columns.Add(CreateButtonColumn("История"));

            if (role == "Администратор")
            {
                dataGridView.Columns.Add(CreateButtonColumn("Новая запись на приём"));
            }
             if (role == "Врач")
            {
                dataGridView.Columns.Add(CreateButtonColumn("Новая запись на приём"));
                dataGridView.Columns.Add(CreateButtonColumn("Новая запись"));
            }

            // Настройка ширины столбца id
            var idColumn = dataGridView.Columns["id"];
            if (idColumn != null)
            {
                idColumn.Width = 70;
                idColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            return dataGridView;
        }

        private DataGridViewButtonColumn CreateButtonColumn(string headerText)
        {
            return new DataGridViewButtonColumn
            {
                HeaderText = headerText,
                Text = headerText,
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = ColorTranslator.FromHtml("#A60800"),
                    ForeColor = Color.White,
                    Font = new Font("Times New Roman", 12, FontStyle.Regular),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
        }


        private void LoadPatientData(DataGridView dataGridView)
        {
            string connectionString = "Data Source=PC-ANDREI\\SQLEXPRESS;Initial Catalog=УП;Integrated Security=True";

            string query ;
            if (forrm=="patient")
            {
                query = "SELECT Номер, ФИО, Телефон, Адрес FROM Пациенты";
            }
            else if (role=="Администратор" || role == "Глав. врач")
            {
                query = "SELECT Пациенты.Номер, Пациенты.ФИО, Пациенты.Телефон, Пациенты.Адрес,  " +
                    "       Записи.Номер_пациента, Записи.Дата, Записи.Номер_врача, Врачи.ФИО " +
                    "FROM Пациенты " +
                    "LEFT JOIN Записи ON Пациенты.Номер = Записи.Номер_пациента " +
                    "LEFT JOIN Врачи ON Записи.Номер_врача = Врачи.Номер " +
                    "WHERE Записи.Дата = '" + DateTime.Now.ToString("dd.MM.yyyy") + "';";
            }
            else
            {
                query = "SELECT Пациенты.Номер, Пациенты.ФИО, Пациенты.Телефон, Пациенты.Адрес,  " +
                    "       Записи.Номер_пациента, Записи.Дата, Записи.Номер_врача, Врачи.ФИО " +
                    "FROM Пациенты " +
                    "LEFT JOIN Записи ON Пациенты.Номер = Записи.Номер_пациента " +
                    "LEFT JOIN Врачи ON Записи.Номер_врача = Врачи.Номер " +
                    "WHERE Записи.Дата = '" + DateTime.Now.ToString("dd.MM.yyyy") + "' AND Врачи.ФИО = '" + name + "';";
            }

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
                            dataGridView.Rows.Add(reader["Номер"], reader["ФИО"], reader["Телефон"], reader["Адрес"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }
        }

        public void HandleCellClick(object sender, DataGridViewCellEventArgs e, DataGridView dataGridView)
        {
            if (e.RowIndex >= 0)
            {
                string id = dataGridView.Rows[e.RowIndex].Cells["id"].Value.ToString();
                idd=id;
                string name = dataGridView.Rows[e.RowIndex].Cells["name"].Value.ToString();
                if (e.ColumnIndex ==4 ) historiesForm.ShowPatients(id, name);
                else if (e.ColumnIndex == 5) newListToDoctorForm.ShowPatients(id,name, "patient"+" "+forrm, role);
                else if (e.ColumnIndex == 6) newListForm.ShowNewListForm(id, "", null, null, null, null, null);
            }
        }

        private void ButtonMain_Click(object sender, EventArgs e) => form.SignIn();
        private void ButtonExit_Click(object sender, EventArgs e) { mainForm.Controls.Clear(); mainForm.InitializeMainForm(); }
        private void ButtonBack_Click(object sender, EventArgs e) => form.SignIn();
        private void ButtonNewPatient_Click(object sender, EventArgs e) => newListForm.ShowNewListForm(idd, "patient", null, null, null, null, null);

        private void SetElementBounds(Label labelName, Label labelDate, Button buttonMain, Button buttonExit, 
            DataGridView dataGridView, Button buttonBack, Label labelList, Button buttonNewPatient)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;

            mainForm.pictureBox.SetBounds((int)(width * 0.03), (int)(height * 0.03), (int)(width * 0.1), (int)(height * 0.13));
            mainForm.labelLogo.SetBounds((int)(width * 0.15), (int)(height * 0.05), (int)(width * 0.3), (int)(height * 0.07));
            labelName.SetBounds((int)(width * 0.4), (int)(height * 0.04), (int)(width * 0.33), (int)(height * 0.06));
            buttonMain.SetBounds((int)(width * 0.75), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            buttonExit.SetBounds((int)(width * 0.85), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            labelList.SetBounds((int)(width * 0.75), (int)(height * 0.13), (int)(width * 0.24), (int)(height * 0.05));
            if (role == "Администратор"&&forrm=="patient")
            {
                 buttonNewPatient.SetBounds((int)(width * 0.75), (int)(height * 0.244), (int)(width * 0.24), (int)(height * 0.05));
            }
           
            labelDate.SetBounds((int)(width * 0.03), (int)(height * 0.17), (int)(width * 0.30), (int)(height * 0.05));
            dataGridView.SetBounds((int)(width * 0.05), (int)(height * 0.3), (int)(width * 0.9), (int)(height * 0.5));
            buttonBack.SetBounds((int)(width * 0.8), (int)(height * 0.85), (int)(width * 0.1), (int)(height * 0.07));
        }
    }
}
