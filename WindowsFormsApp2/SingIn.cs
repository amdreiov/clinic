using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp;

namespace WindowsFormsApp2
{
    public class SingIn
    {
        private MainForm mainForm;
        private Patients pt;
        private Password ps;
        private workSchedule ws;
        public Timer timer1;
        public Label labelDate;
        public string role, name;

        public SingIn(MainForm form, string role, string name)
        {
            mainForm = form;
            timer1 = new Timer { Interval = 1000 };
            timer1.Tick += Timer1_Tick;
            timer1.Start();
            this.role = role;
            this.name = name;
            pt = new Patients(form, this, role, name, "");
            ps = new Password(form, this, role);
            ws = new workSchedule(form, this, role, name);
        }

        public void SignIn()
        {
            mainForm.Controls.Clear();
            // Установка заголовка
            bool result = SetFormTitle();
            if (result==true)
            {
                mainForm.InitTimer();
                mainForm.ClientSize = new Size(1200, 800);
                mainForm.create_logo();

            // Создание элементов интерфейса
            Label labelName = CreateLabel($"Имя {role}а: {name}", new Font("Times New Roman", 12, FontStyle.Bold), ContentAlignment.MiddleRight);
            labelDate = CreateLabel("dd.MM.yyyy       HH:mm:ss", new Font("Times New Roman", 16, FontStyle.Bold), ContentAlignment.MiddleCenter);
            Button buttonExit = CreateButton("Выход", "#FFFFFF", "#6500B0", ButtonExit_Click);

            // Создание кнопок на основе роли
            Button buttonPatients = null, buttonPointsToday = null, buttonSchedule = null;

            if (role != "Сис. администратор")
            {
                buttonPatients = CreateButton("Пациенты", "#FFFFFF", "#A60800", ButtonPatients_Click);
                buttonPointsToday = CreateButton("Записи на сегодня", "#FFFFFF", "#A60800", ButtonPointsToday_Click);
                buttonSchedule = CreateButton("График работы", "#FFFFFF", "#A60800", ButtonSchedule_Click);

                mainForm.Controls.Add(buttonPatients);
                mainForm.Controls.Add(buttonPointsToday);
                mainForm.Controls.Add(buttonSchedule);
            }
            else
            {
                buttonPatients = CreateButton("Пароли", "#FFFFFF", "#A60800", ButtonPasswords_Click);
                mainForm.Controls.Add(buttonPatients);
            }

            mainForm.Controls.Add(labelName);
            mainForm.Controls.Add(labelDate);
            mainForm.Controls.Add(buttonExit);

            // Установка размеров и позиций
            SetElementBounds(labelName, labelDate, buttonExit, buttonPatients, buttonPointsToday, buttonSchedule);

            // Подписка на изменение размера формы
            mainForm.Resize += (sender, e) =>
                SetElementBounds(labelName, labelDate, buttonExit, buttonPatients, buttonPointsToday, buttonSchedule);
            }
            else { mainForm.InitializeMainForm();}
        }

        private bool SetFormTitle()
        {
            bool result;
             switch(role)
            {
                case "Врач" :mainForm.Text = "Личный кабинет врача"; result =true; break;
                case "Администратор":mainForm.Text =  "Личный кабинет администратора"; result = true; break;
                case "Сис. администратор" : mainForm.Text = "Личный кабинет системного администратора"; result = true; break;
                case "Глав. врач" : mainForm.Text = "Личный кабинет главного врача"; result = true; break;
                default: MessageBox.Show("Пользователь не найден!"); result = false; break; 
            };
            return result;
        }

        public Label CreateLabel(string text, Font font, ContentAlignment alignment)
        {
            return new Label
            {
                Text = text,
                AutoSize = false,
                Font = font,
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                TextAlign = alignment
            };
        }

        private Button CreateButton(string text, string textColor, string backgroundColor, EventHandler clickHandler)
        {
            var button = new Button
            {
               Text = text,
               Font = new Font("Times New Roman", 14, FontStyle.Bold),
               ForeColor = ColorTranslator.FromHtml(textColor),
               BackColor = ColorTranslator.FromHtml(backgroundColor),
               FlatStyle = FlatStyle.Flat
            };
             button.FlatAppearance.BorderSize = 0;

            // Добавляем обработчик события
            if (clickHandler != null)
            {
                button.Click += clickHandler;
            }

            return button;
        }

        private void SetElementBounds(Label labelName, Label labelDate, Button buttonExit, Button buttonPatients, Button buttonPointsToday, Button buttonSchedule)
        {
            int width = mainForm.ClientSize.Width;
            int height = mainForm.ClientSize.Height;

            mainForm.pictureBox.SetBounds((int)(width * 0.03), (int)(height * 0.03), (int)(width * 0.1), (int)(height * 0.13));
            mainForm.labelLogo.SetBounds((int)(width * 0.15), (int)(height * 0.05), (int)(width * 0.25), (int)(height * 0.15));

            labelName.SetBounds((int)(width * 0.4), (int)(height * 0.04), (int)(width * 0.33), (int)(height * 0.03));
            labelDate.SetBounds((int)(width * 0.3), (int)(height * 0.2), (int)(width * 0.4), (int)(height * 0.05));
            buttonExit.SetBounds((int)(width * 0.85), (int)(height * 0.04), (int)(width * 0.07), (int)(height * 0.05));

            buttonPatients?.SetBounds((int)(width * 0.34), (int)(height * 0.4), (int)(width * 0.15), (int)(height * 0.1));
            buttonPointsToday?.SetBounds((int)(width * (0.34 + 0.01 + 0.15)), (int)(height * 0.4), (int)(width * 0.15), (int)(height * 0.1));
            buttonSchedule?.SetBounds((int)(width * (0.34 + 0.085)), (int)(height * 0.6), (int)(width * 0.15), (int)(height * 0.1));
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (labelDate != null)
            {
                labelDate.Text = DateTime.Now.ToString("dd.MM.yyyy       HH:mm:ss");
            }
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            mainForm.Controls.Clear();
            mainForm.InitializeMainForm();
        }

        private void ButtonPatients_Click(object sender, EventArgs e)
        {
            mainForm.Controls.Clear();
            string forrm = "patient";
            pt.ShowPatients(forrm);
        }

        private void ButtonPointsToday_Click(object sender, EventArgs e)
        {
            mainForm.Controls.Clear();
            string forrm = "today";
            pt.ShowPatients(forrm);
        }

        private void ButtonSchedule_Click(object sender, EventArgs e)
        {
            mainForm.Controls.Clear();
            ws.ShowShedule(name);
        }

        private void ButtonPasswords_Click(object sender, EventArgs e)
        {
            mainForm.Controls.Clear();
            ps.ShowPassword();
        }
    }
}
