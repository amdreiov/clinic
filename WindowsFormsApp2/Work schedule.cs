using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp;

namespace WindowsFormsApp2
{
    internal class workSchedule
    {
       private MainForm mainForm;
       private SingIn form;
       private Timer timer1;
       private Label labelDate;
       private NewList newListForm;
       private string name, role;
       private DataGridView dataGridView;

       public workSchedule(MainForm form1, SingIn signInForm, string role, string name)
       {
            this.role = role;
           mainForm = form1;
           form = signInForm;
            this.name =name;
            newListForm = new NewList(mainForm, form, null, null, role, name);
           InitializeTimer();
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

       public void ShowShedule(string name)
       {
           this.name = name;

           ConfigureForm();
           CreateControls();
           LoadPatientHistory();
       }

       private void ConfigureForm()
       {
           mainForm.ClientSize = new Size(1200, 800);
           mainForm.Controls.Clear();
           mainForm.Text = $"Расписание";
           mainForm.create_logo();
       }

       private void CreateControls()
       {
           Label labelName = CreateLabel($"Имя {role}а:\n {name}", new Font("Times New Roman", 12, FontStyle.Bold), ContentAlignment.MiddleRight, Color.White, ColorTranslator.FromHtml("#9A6D0D"));
            labelDate = CreateLabel(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), new Font("Times New Roman", 16, FontStyle.Bold), ContentAlignment.MiddleCenter, Color.White, ColorTranslator.FromHtml("#7608AA"));
           Label labelList = CreateLabel("Вкладка | График работы", new Font("Times New Roman", 14, FontStyle.Bold), ContentAlignment.MiddleCenter, Color.White, ColorTranslator.FromHtml("#A60800"));


           Button buttonMain = CreateButton("Главная", new Font("Times New Roman", 11, FontStyle.Bold), ColorTranslator.FromHtml("#6500B0"));
           buttonMain.Click += (sender, e) => { mainForm.Controls.Clear(); form.SignIn(); };

           Button buttonExit = CreateButton("Выход", new Font("Times New Roman", 11, FontStyle.Bold), ColorTranslator.FromHtml("#6500B0"));
           buttonExit.Click += (sender, e) => { mainForm.Controls.Clear(); mainForm.InitializeMainForm(); };

           Button buttonBack = CreateButton("Назад", new Font("Times New Roman", 11, FontStyle.Bold), ColorTranslator.FromHtml("#6500B0"));
           buttonBack.Click += (sender, e) => { mainForm.Controls.Clear(); form.SignIn(); };

           // Создаем DataGridView
           DataGridView dataGridView = CreateDataGridView();
           dataGridView.CellClick += (s, e) => HandleCellClick(s, e, dataGridView);

            // Добавляем элементы на форму
            mainForm.Controls.Add(labelName);
           mainForm.Controls.Add(labelDate);
           mainForm.Controls.Add(labelList);
           mainForm.Controls.Add(buttonMain);
           mainForm.Controls.Add(buttonExit);
           mainForm.Controls.Add(dataGridView); 

            mainForm.Controls.Add(buttonBack);

            if (role == "Администратор")
            {
                Button btnNewLst = CreateButton("Новая Запись", new Font("Times New Roman", 11, FontStyle.Bold), ColorTranslator.FromHtml("#6500B0"));
                mainForm.Controls.Add(btnNewLst);
                btnNewLst.Click += (sender, e) => { mainForm.Controls.Clear(); newListForm.ShowNewListForm("", "Schedule", null, null, null, null, null); };
                SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, btnNewLst);

                mainForm.Resize += (s, e) => SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, btnNewLst);
            }
            else
            {
                SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, null);

                mainForm.Resize += (s, e) => SetElementBounds(labelName, labelDate, buttonMain, buttonExit, dataGridView, buttonBack, labelList, null);
            }

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
            dataGridView.Columns.Add("name", "ФИО");
            dataGridView.Columns.Add("specialization", "Специализация");
            dataGridView.Columns.Add("time_start", "Время начала приема");
           dataGridView.Columns.Add("time_finish", "Время конца приема");
           dataGridView.Columns.Add("room", "Кабинет");

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
        private void HandleCellClick(object sender, DataGridViewCellEventArgs e, DataGridView dataGridView)
        {
            if (e.RowIndex >= 0)
            {
                string id = dataGridView.Rows[e.RowIndex].Cells["id"].Value.ToString();
                if (e.ColumnIndex == 5) ; //для изменения
            }
        }
        private void LoadPatientHistory()
       {
            string query;
           string connectionString = "Data Source=PC-ANDREI\\SQLEXPRESS;Initial Catalog=УП;Integrated Security=True";
            if (role=="Администратор" || role == "Глав. врач")
            {
                 query = @"SELECT Расписание.Номер, Врачи.ФИО, Расписание.Дата, Расписание.Время_начала, Расписание.Время_концв, Расписание.Кабинет, Специализации.Название AS Специализация,  Врачи_Специализации.Номер_специализации
                          FROM Расписание
                          LEFT JOIN Врачи ON Врачи.Номер = Расписание.Номер_врача
						  LEFT JOIN Врачи_Специализации ON Врачи.Номер = Врачи_Специализации.Номер_врача
						  LEFT JOIN Специализации ON Специализации.ID = Врачи_Специализации.Номер_специализации;";

            }
            else {
                query = @"SELECT Расписание.Номер, Врачи.ФИО, Расписание.Дата, Расписание.Время_начала, Расписание.Время_концв, Расписание.Кабинет, Специализации.Название AS Специализация,  Врачи_Специализации.Номер_специализации
                          FROM Расписание
                          LEFT JOIN Врачи ON Врачи.Номер = Расписание.Номер_врача
						  LEFT JOIN Врачи_Специализации ON Врачи.Номер = Врачи_Специализации.Номер_врача
						  LEFT JOIN Специализации ON Специализации.ID = Врачи_Специализации.Номер_специализации
                          WHERE Врачи.ФИО = '" + name+"';";
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
                           // Добавляем данные в DataGridView
                           dataGridView.Rows.Add(reader["Номер"], reader["Дата"], reader["ФИО"], reader["Специализация"], reader["Время_начала"], reader["Время_концв"], reader["Кабинет"]);
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
           }
       }

       private void SetElementBounds(Label labelName, Label labelDate, Button buttonMain, Button buttonExit, DataGridView dataGridView, Button buttonBack, Label labelList, Button btnNewLst)
       {
           int width = mainForm.ClientSize.Width;
           int height = mainForm.ClientSize.Height;

           mainForm.pictureBox.SetBounds((int)(width * 0.03), (int)(height * 0.03), (int)(width * 0.1), (int)(height * 0.13));
           mainForm.labelLogo.SetBounds((int)(width * 0.15), (int)(height * 0.05), (int)(width * 0.4), (int)(height * 0.07));
           labelName.SetBounds((int)(width * 0.4), (int)(height * 0.04), (int)(width * 0.33), (int)(height * 0.06));
           buttonMain.SetBounds((int)(width * 0.75), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
           buttonExit.SetBounds((int)(width * 0.85), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));
            if (role == "Администратор")
            {
                btnNewLst.SetBounds((int)(width * 0.75), (int)(height * 0.24), (int)(width * 0.24), (int)(height * 0.05));
            }
            labelList.SetBounds((int)(width * 0.75), (int)(height * 0.13), (int)(width * 0.24), (int)(height * 0.05));
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

