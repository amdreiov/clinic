using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp;

namespace WindowsFormsApp2
{
    public class Histories
    {
        private MainForm mainForm;
        private SingIn form;
        private Timer timer1;
        private Label labelDate;
        private Patients patientsForm;
        private NewList newListForm;
        private NewListToDoctor newListToDoctorForm;
        private string name, id, role, names;
        private DataGridView dataGridView;

        public Histories(MainForm form1, SingIn signInForm, Patients patientsForms,string role, string names)
        {
            this.names = names;
            mainForm = form1;
            form = signInForm;
            patientsForm = patientsForms;
            this.role=role;
            InitializeTimer();
            newListToDoctorForm = new NewListToDoctor(mainForm, form, patientsForm, this, names);
            newListForm = new NewList(form1, signInForm, patientsForms, null,  role, name);
        }

        private void InitializeTimer()
        {
            timer1 = new Timer
            {
                Interval = 1000
            };
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        public void ShowPatients(string idd, string namee)
        {
            name = namee;
            id = idd;

            ConfigureForm();
            CreateControls();
            LoadPatientHistory();
        }

        private void ConfigureForm()
        {
            mainForm.ClientSize = new Size(1200, 800);
            mainForm.Controls.Clear();
            mainForm.Text = $"История пациента: {name}";
            mainForm.create_logo();
        }

        private void CreateControls()
        {
            Label labelName = CreateLabel($"Имя {role}а:\n {names}", new Font("Times New Roman", 12, FontStyle.Bold), ContentAlignment.MiddleRight, Color.White, ColorTranslator.FromHtml("#9A6D0D"));
            labelDate = CreateLabel(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), new Font("Times New Roman", 16, FontStyle.Bold), ContentAlignment.MiddleCenter, Color.White, ColorTranslator.FromHtml("#7608AA"));
            Label labelList = CreateLabel("Вкладка | История", new Font("Times New Roman", 16, FontStyle.Bold), ContentAlignment.MiddleCenter, Color.White, ColorTranslator.FromHtml("#A60800"));
            Label nameOfPacient = CreateLabel(name, new Font("Times New Roman", 16, FontStyle.Bold), ContentAlignment.MiddleLeft, Color.White);
            Button buttonNewList;
            Button buttonNewListToDoctor;
            if (role != "Администратор")
            {
                buttonNewList = CreateButton("Новая запись", new Font("Times New Roman", 16, FontStyle.Bold), ColorTranslator.FromHtml("#A60800"));
                buttonNewList.Click += (sender, e) => { mainForm.Controls.Clear(); newListForm.ShowNewListForm(id, "", null, null, null, null, null); };
                mainForm.Controls.Add(buttonNewList);
                buttonNewListToDoctor = CreateButton("Новая запись на прием", new Font("Times New Roman", 16, FontStyle.Bold), ColorTranslator.FromHtml("#A60800"));
                buttonNewListToDoctor.Click += (sender, e) => {
                    mainForm.Controls.Clear(); newListToDoctorForm.ShowPatients(id, name, "histories", role);
                };
                mainForm.Controls.Add(buttonNewListToDoctor);
            }
            else if (role == "Глав. врач")
            {
                buttonNewListToDoctor = null;
                buttonNewList = null;
            }
            else
            {
                buttonNewListToDoctor = CreateButton("Новая запись к врачу", new Font("Times New Roman", 16, FontStyle.Bold), ColorTranslator.FromHtml("#A60800"));
                buttonNewListToDoctor.Click += (sender, e) => { mainForm.Controls.Clear(); newListToDoctorForm.ShowPatients(id, name, "histories", role);
                    mainForm.Controls.Add(buttonNewListToDoctor);
                };
                buttonNewList = null;
            }


            Button buttonMain = CreateButton("Главная", new Font("Times New Roman", 11, FontStyle.Bold), ColorTranslator.FromHtml("#6500B0"));
            buttonMain.Click += (sender, e) => { mainForm.Controls.Clear(); form.SignIn(); };

            Button buttonExit = CreateButton("Выход", new Font("Times New Roman", 11, FontStyle.Bold), ColorTranslator.FromHtml("#6500B0"));
            buttonExit.Click += (sender, e) => { mainForm.Controls.Clear(); mainForm.InitializeMainForm(); };

            Button buttonBack = CreateButton("Назад", new Font("Times New Roman", 11, FontStyle.Bold), ColorTranslator.FromHtml("#6500B0"));
            buttonBack.Click += (sender, e) => { mainForm.Controls.Clear(); patientsForm.ShowPatients("patient"); };

            // Создаем DataGridView
            DataGridView dataGridView = CreateDataGridView();

            // Добавляем элементы на форму
            mainForm.Controls.Add(labelName);
            mainForm.Controls.Add(labelDate);
            mainForm.Controls.Add(labelList);

            
            mainForm.Controls.Add(buttonMain);
            mainForm.Controls.Add(buttonExit);
            mainForm.Controls.Add(dataGridView);
            mainForm.Controls.Add(nameOfPacient);
            mainForm.Controls.Add(buttonBack);

            // Устанавливаем размеры и позиции элементов
            SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, buttonNewList, buttonNewListToDoctor, nameOfPacient);

            mainForm.Resize += (s, e) => SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, buttonNewList, buttonNewListToDoctor, nameOfPacient);
        }

        private Label CreateLabel(string text, Font font, ContentAlignment textAlign, Color foreColor, Color backColor = default)
        {
            return new Label
            {
                Text = text,
                Font = font,
                TextAlign = textAlign,
                ForeColor = foreColor,
                BackColor = backColor == default ? Color.Transparent : backColor,
                AutoSize = false
            };
        }

        private Button CreateButton(string text, Font font, Color backColor)
        {
            return new Button
            {
                Text = text,
                Font = font,
                ForeColor = Color.White,
                BackColor = backColor,
                AutoSize = false,
                FlatStyle = FlatStyle.Flat
            };
        }

        private DataGridView CreateDataGridView()
        {
             dataGridView = new DataGridView
            {
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Times New Roman", 12),
                BackgroundColor = ColorTranslator.FromHtml("#A60800"),
                ForeColor = Color.White,
                RowHeadersVisible = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = ColorTranslator.FromHtml("#A60800"),
                    ForeColor = Color.White,
                    Font = new Font("Times New Roman", 12, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = ColorTranslator.FromHtml("#A60800"),
                    ForeColor = Color.White,
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    WrapMode = DataGridViewTriState.True
                }
            };

            // Добавляем столбцы
            dataGridView.Columns.Add("id", "Номер");
            dataGridView.Columns.Add("Date", "Дата");
            dataGridView.Columns.Add("Spechical", "Специализация");
            dataGridView.Columns.Add("Address", "Врач");
            dataGridView.Columns.Add("Diagnoz", "Диагноз");
            dataGridView.Columns.Add("Recomendation", "Рекомендации");

            return dataGridView;
        }

        private void LoadPatientHistory()
        {
            string connectionString = "Data Source=PC-ANDREI\\SQLEXPRESS;Initial Catalog=УП;Integrated Security=True";
            string query = "SELECT Истории_болезней.Номер , Истории_болезней.Дата_явки, Диагнозы.Название AS Диагноз, Врачи.ФИО AS Врач, Специализации.Название AS Специализация, Истории_болезней.Рекомендации AS Рекомендации FROM Истории_болезней LEFT JOIN Врачи ON Истории_болезней.Номер_врача = Врачи.Номер LEFT JOIN Врачи_специализации ON Врачи.Номер = Врачи_специализации.Номер_врача LEFT JOIN Специализации ON Врачи_специализации.Номер_специализации = Специализации.ID LEFT JOIN Диагнозы ON Истории_болезней.Диагноз = Диагнозы.Номер WHERE Истории_болезней.Номер_пациента=" + id + ";";

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
                            // Добавляем данные в DataGridView
                            dataGridView.Rows.Add(reader["Номер"], reader["Дата_явки"], reader["Специализация"], reader["Врач"], reader["Диагноз"], reader["Рекомендации"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }
        }

        private void SetElementBounds(Label labelName, Label labelDate, Button buttonMain, Button buttonExit, DataGridView dataGridView, Button buttonBack, Label labelList, Button buttonNewList, Button buttonNewListToDoctor, Label nameOfPacient)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;

            mainForm.pictureBox.SetBounds((int)(width * 0.03), (int)(height * 0.03), (int)(width * 0.1), (int)(height * 0.13));
            mainForm.labelLogo.SetBounds((int)(width * 0.15), (int)(height * 0.05), (int)(width * 0.3), (int)(height * 0.07));
            labelName.SetBounds((int)(width * 0.4), (int)(height * 0.04), (int)(width * 0.33), (int)(height * 0.07));
            buttonMain.SetBounds((int)(width * 0.75), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            buttonExit.SetBounds((int)(width * 0.85), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            labelList.SetBounds((int)(width * 0.75), (int)(height * 0.13), (int)(width * 0.24), (int)(height * 0.05));
            nameOfPacient.SetBounds((int)(width * 0.03), (int)(height * 0.23), (int)(width * 0.4), (int)(height * 0.05));
            if (role == "Врач")
            {
                buttonNewListToDoctor.SetBounds((int)(width * 0.75), (int)(height * 0.19), (int)(width * 0.2), (int)(height * 0.05));
                buttonNewList.SetBounds((int)(width * 0.75), (int)(height * 0.245), (int)(width * 0.2), (int)(height * 0.05));
            }
            else if (role == "Администратор")
            {
                buttonNewListToDoctor.SetBounds((int)(width * 0.75), (int)(height * 0.19), (int)(width * 0.2), (int)(height * 0.05));
            }
            
            
            labelDate.SetBounds((int)(width * 0.03), (int)(height * 0.165), (int)(width * 0.30), (int)(height * 0.05));
            dataGridView.SetBounds((int)(width * 0.05), (int)(height * 0.3), (int)(width * 0.9), (int)(height * 0.5));
            buttonBack.SetBounds((int)(width * 0.8), (int)(height * 0.85), (int)(width * 0.1), (int)(height * 0.07));
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (labelDate != null)
            {
                labelDate.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            }
        }
    }
}
