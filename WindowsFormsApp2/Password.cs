using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp;

namespace WindowsFormsApp2
{
    public class Password
    {
        private MainForm mainForm;
        private SingIn form;
        private Histories historiesForm;
        private Timer timer2;
        private Label labelDate;
        private NewList newListForm;
        private NewListToDoctor newListToDoctorForm;
        public string name, role, forrm;

        public Password(MainForm form1, SingIn signInForm, string role)
        {
            mainForm = form1;
            form = signInForm;
            // Инициализация таймера

            this.role = role;
            newListForm = new NewList(mainForm, signInForm, this,  role, name);
        }
        public void ShowPassword()
        {
            // Настраиваем размеры формы и очищаем интерфейс
            mainForm.ClientSize = new Size(1200, 800);
            mainForm.Controls.Clear();
            mainForm.Text = "Пароли";
            mainForm.create_logo();

            // Создаем элементы интерфейса
            Label labelName = CreateLabel("Имя врача: Иван Иванович", "#FFFFFF", 12, ContentAlignment.MiddleRight);;
            Label labelList;

                labelList = CreateLabel("Вкладка | Пациенты", "#FFFFFF", 16, ContentAlignment.MiddleCenter);
                labelList.BackColor = ColorTranslator.FromHtml("#A60800");
                mainForm.Controls.Add(labelList);


            Button buttonMain = CreateButton("Главная", "#FFFFFF", "#6500B0", ButtonMain_Click);
            Button buttonExit = CreateButton("Выход", "#FFFFFF", "#6500B0", ButtonExit_Click);
            Button buttonBack = CreateButton("Назад", "#FFFFFF", "#6500B0", ButtonBack_Click);
            Button buttonNew = CreateButton("Новая запись", "#FFFFFF", "#6500B0", ButtonNew_Click);
            DataGridView dataGridView = CreateDataGridView();

            // Загружаем данные из базы данных
            LoadPatientData(dataGridView);
            dataGridView.CellClick += (s, e) => HandleCellClick(s, e, dataGridView);

            // Добавляем элементы на форму
            mainForm.Controls.Add(labelName);
            mainForm.Controls.Add(labelDate);

            mainForm.Controls.Add(buttonNew);

            mainForm.Controls.Add(buttonMain);
            mainForm.Controls.Add(buttonExit);
            mainForm.Controls.Add(dataGridView);
            mainForm.Controls.Add(buttonBack);

            // Устанавливаем размеры и позиции элементов
            SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList,  buttonNew);
            mainForm.Resize += (s, e) => SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, buttonNew);
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
            dataGridView.Columns.Add("id_doctor", "Номер_врача");
            dataGridView.Columns.Add("role", "Роль");
            dataGridView.Columns.Add("login", "Логин");
            dataGridView.Columns.Add("password", "Пароль");
            dataGridView.Columns.Add("usees", "Действующий");
            dataGridView.Columns.Add(CreateButtonColumn("Изменить"));

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

            string query= "SELECT * FROM Пароли";

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
                            dataGridView.Rows.Add(reader["Номер"], reader["Номер_врача"], reader["Роль"], reader["Логин"], reader["Пароль"], reader["Действующий"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }
        }

        private void HandleCellClick(object sender, DataGridViewCellEventArgs e, DataGridView dataGridView)
        {
            if (e.RowIndex >= 0)
            {
                string id = dataGridView.Rows[e.RowIndex].Cells["id"].Value.ToString();
                if (e.ColumnIndex == 6) // Колонка "Изменить"
                {
                    // Передать данные в ShowPassword
                    string idf = dataGridView.Rows[e.RowIndex].Cells["id_doctor"].Value.ToString();
                    string role = dataGridView.Rows[e.RowIndex].Cells["role"].Value.ToString();
                    string login = dataGridView.Rows[e.RowIndex].Cells["login"].Value.ToString();
                    string password = dataGridView.Rows[e.RowIndex].Cells["password"].Value.ToString();
                    string isActive = dataGridView.Rows[e.RowIndex].Cells["usees"].Value.ToString();

                    newListForm.ShowNewListForm(id, "edit", idf, role, login, password, isActive);
                }
            }
        }


        private void ButtonMain_Click(object sender, EventArgs e) => form.SignIn();
        private void ButtonNew_Click(object sender, EventArgs e) => newListForm.ShowNewListForm("", "save", null, null, null, null, null); 
        private void ButtonExit_Click(object sender, EventArgs e) { mainForm.Controls.Clear(); mainForm.InitializeMainForm(); }
        private void ButtonBack_Click(object sender, EventArgs e) => form.SignIn();

        private void SetElementBounds(Label labelName, Label labelDate, Button buttonMain, Button buttonExit, DataGridView dataGridView, Button buttonBack, Label labelList, Button buttonNew)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;

            mainForm.pictureBox.SetBounds((int)(width * 0.03), (int)(height * 0.03), (int)(width * 0.1), (int)(height * 0.13));
            mainForm.labelLogo.SetBounds((int)(width * 0.15), (int)(height * 0.05), (int)(width * 0.35), (int)(height * 0.07));
            labelName.SetBounds((int)(width * 0.4), (int)(height * 0.04), (int)(width * 0.33), (int)(height * 0.03));
            buttonMain.SetBounds((int)(width * 0.75), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            buttonExit.SetBounds((int)(width * 0.85), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            labelList.SetBounds((int)(width * 0.75), (int)(height * 0.13), (int)(width * 0.24), (int)(height * 0.05));
            buttonNew.SetBounds((int)(width * 0.75), (int)(height * 0.24), (int)(width * 0.24), (int)(height * 0.05));
            dataGridView.SetBounds((int)(width * 0.05), (int)(height * 0.3), (int)(width * 0.9), (int)(height * 0.5));
            buttonBack.SetBounds((int)(width * 0.8), (int)(height * 0.85), (int)(width * 0.1), (int)(height * 0.07));
        }
    }
}
