using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using WindowsFormsApp2;

namespace WindowsFormsApp
{
    public class MainForm : Form
    {
        public PictureBox pictureBox;
        public Label labelLogo, labelLogin, labelPassword, labelDate;
        public TextBox textboxLogin, textboxPassword;
        public Button buttonSignIn;
        public int idDoctor=1;
        public string role, name, exx;


        private SingIn ss;
        public Timer timer;
         


        public MainForm()
        {
            ss = new SingIn(this, role, name);
            InitializeMainForm();
            InitTimer();
        }

        public void InitializeMainForm()
        {
            // Устанавливаем параметры окна
            ClientSize = new Size(800, 600);
            BackColor = ColorTranslator.FromHtml("#9A6D0D");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Text = "Личный кабинет";
            StartPosition = FormStartPosition.CenterScreen;

            // Создаём элементы интерфейса
            create_logo();
            textboxLogin = new TextBox
            {
                BackColor = ColorTranslator.FromHtml("#A60800"),
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF")
            };

            labelPassword = new Label
            {
                Text = "Пароль:",
                AutoSize = false,
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF")
            };

            textboxPassword = new TextBox
            {
                BackColor = ColorTranslator.FromHtml("#A60800"),
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                UseSystemPasswordChar = true
            };

            buttonSignIn = new Button
            {
                Text = "Войти",
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
                BackColor = ColorTranslator.FromHtml("#6500B0"),
                FlatStyle = FlatStyle.Flat
            };
            buttonSignIn.FlatAppearance.BorderSize = 0;
            buttonSignIn.Click += ButtonSignIn_Click;

            // Добавляем элементы на форму

            Controls.Add(labelLogin);
            Controls.Add(textboxLogin);
            Controls.Add(labelPassword);
            Controls.Add(textboxPassword);
            Controls.Add(buttonSignIn);

            // Устанавливаем размеры и позиции
            SetElementBounds();

            Resize += (sender, e) => SetElementBounds();
        }

        public void create_logo()
        {
            pictureBox = new PictureBox
            {
                Image = Image.FromFile("C:\\Users\\syper\\source\\repos\\WindowsFormsApp2\\WindowsFormsApp2\\Picture\\krest.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            labelLogo = new Label
            {
                Text = "Личный кабинет\nсотрудника поликлиники",
                AutoSize = false,
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF"),
               
            };

            labelLogin = new Label
            {
                Text = "Логин:",
                AutoSize = false,
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFFFFF")
            };
            Controls.Add(pictureBox);
            Controls.Add(labelLogo);
        }

        public void SetElementBounds()
        {
            int width = ClientSize.Width;
            int height = ClientSize.Height;

            pictureBox.SetBounds((int)(width * 0.03), (int)(height * 0.03), (int)(width * 0.1), (int)(height * 0.13));
            labelLogo.SetBounds((int)(width * 0.15), (int)(height * 0.05), (int)(width * 0.7), (int)(height * 0.15));
            labelLogin.SetBounds((int)(width * 0.25), (int)(height * 0.3), (int)(width * 0.2), (int)(height * 0.05));
            textboxLogin.SetBounds((int)(width * 0.25), (int)(height * 0.35), (int)(width * 0.5), (int)(height * 0.05));
            labelPassword.SetBounds((int)(width * 0.25), (int)(height * 0.45), (int)(width * 0.2), (int)(height * 0.05));
            textboxPassword.SetBounds((int)(width * 0.25), (int)(height * 0.5), (int)(width * 0.5), (int)(height * 0.05));
            buttonSignIn.SetBounds((int)(width * 0.4), (int)(height * 0.6), (int)(width * 0.2), (int)(height * 0.07));
        }

        public void ButtonSignIn_Click(object sender, EventArgs e)
        {
            string login = textboxLogin.Text.Trim();
            string password = textboxPassword.Text.Trim();
            string connectionString = "Data Source=PC-ANDREI\\SQLEXPRESS;Initial Catalog=УП;Integrated Security=True";

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                exx= "Введите логин и пароль!";
                MessageBox.Show("Введите логин и пароль!", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT Врачи.ФИО, Номер_врача, Роль, Логин, Пароль, Действующий 
                                    FROM Пароли 
                                    LEFT JOIN Врачи ON Врачи.Номер = Пароли.Номер_врача
                                    WHERE Логин = @Login AND Пароль = @Password AND Действующий = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Получаем роль и имя пользователя из БД
                                string role = reader["Роль"].ToString();
                                string name = reader["ФИО"].ToString(); 

                                // Открываем личный кабинет
                                Controls.Clear();
                                ss = new SingIn(this, role, name);
                                ss.SignIn();
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль, либо ваша учетная запись неактивна.",
                                    "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exx= ex.Message;
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void InitTimer()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer1_Tick;
            timer.Start();
        }
        public void Timer1_Tick(object sender, EventArgs e)
        {
            if (labelDate != null)
            {
                // Обновляем текст метки с текущей датой и временем в нужном формате
                labelDate.Text = DateTime.Now.ToString("dd.MM.yyyy       HH:mm:ss");
            }
        }



        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
