using NUnit.Framework;
using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp;

namespace WindowsFormsApp2
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void ButtonSignIn_Click_EmptyLoginAndPassword_ShouldShowWarningMessage()
        {

            var form = new MainForm();
            form.textboxLogin.Text = string.Empty;
            form.textboxPassword.Text = string.Empty;


            form.ButtonSignIn_Click(null, null);


            Assert.That(form.exx, Is.EqualTo("Введите логин и пароль!"));
        }

        [Test]
        public void SetElementBounds_ResizeForm_ElementsRepositionedCorrectly()
        {

            var form = new MainForm
            {
                ClientSize = new Size(800, 600)
            };


            form.ClientSize = new Size(1000, 800);
            form.SetElementBounds();


            Assert.That(form.buttonSignIn.Width, Is.GreaterThan(100)); // Пример проверки размера кнопки
        }

        [Test]
        public void Timer1_Tick_LabelDateShouldUpdate()
        {

            var form = new MainForm
            {
                labelDate = new Label()
            };
            string initialText = form.labelDate.Text;


            form.Timer1_Tick(null, null);


            Assert.That(initialText, Is.Not.EqualTo(form.labelDate.Text));
        }

        [Test]
        public void Timer1_Tick_ShouldUpdateLabelDate()
        {

            var form = new MainForm();
            var signIn = new SingIn(form, "Врач", "Иван Иванович")
            {
                labelDate = new Label { Text = "Старое время" }
            };


            signIn.GetType()
                .GetMethod("Timer1_Tick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(signIn, new object[] { null, null });


            Assert.That(signIn.labelDate.Text, Is.Not.EqualTo("Старое время"));
            Assert.That(signIn.labelDate.Text, Does.Match(@"\d{2}\.\d{2}\.\d{4}       \d{2}:\d{2}:\d{2}"));
        }

        private MainForm mockMainForm;
        private SingIn mockSignInForm;
        private Patients patients;

        [SetUp]
        public void TestInitialize()
        {
            mockMainForm = new MainForm();
            mockSignInForm = new SingIn(mockMainForm, "Администратор", "Test Admin");
            patients = new Patients(mockMainForm, mockSignInForm, "Администратор", "Test Admin", "1");
        }

        [Test]
        public void ShowPatients_DisplaysCorrectControlsForAdmin()
        {

            patients.ShowPatients("patient");


            Assert.That(mockMainForm.Text, Is.EqualTo("Список пациентов"));
        }

        [Test]
        public void HandleCellClick_TriggersCorrectActions()
        {

            DataGridView gridView = new DataGridView();
            gridView.Columns.Add("id", "Номер");
            gridView.Columns.Add("name", "ФИО");
            gridView.Columns.Add("button", "История");
            gridView.Rows.Add("1", "Test Patient", "История");
            patients.ShowPatients("patient");

            // Simulate cell click on the "История" column
            var cellEventArgs = new DataGridViewCellEventArgs(2, 0);


            patients.HandleCellClick(gridView, cellEventArgs, gridView);


            Assert.That(patients.idd, Is.EqualTo("1"));
        }

        [Test]
        public void ButtonNewPatient_Click_AddsNewPatient()
        {

            patients.ShowPatients("patient");
            Button newPatientButton = new Button();
            newPatientButton.Click += (s, e) => patients.ShowPatients("patient");


            newPatientButton.PerformClick();


            Assert.That(patients.forrm, Is.EqualTo("patient"));
        }
        [Test]

        public void ButtonSignIn_Click_ValidLoginAndPassword_ShouldProceedToNextForm()
        {

            var form = new MainForm();
            form.textboxLogin.Text = "validUser";
            form.textboxPassword.Text = "validPassword";


            form.ButtonSignIn_Click(null, null);


            Assert.That(form.exx, Is.Null); // Проверяем, что сообщения об ошибке нет
        }
        [Test]
        public void HandleCellClick_TriggersCorrectActions1()
        {

            var form = new MainForm();
            var signInForm = new SingIn(form, "Администратор", "Test Admin");
            var patients = new Patients(form, signInForm, "Администратор", "Test Admin", "1");

            DataGridView gridView = new DataGridView();
            gridView.Columns.Add("id", "Номер");
            gridView.Columns.Add("name", "ФИО");
            gridView.Columns.Add("button", "История");
            gridView.Rows.Add("1", "Test Patient", "История");

            // Simulate cell click on the "История" column
            var cellEventArgs = new DataGridViewCellEventArgs(2, 0);


            patients.HandleCellClick(gridView, cellEventArgs, gridView);


            Assert.That(patients.idd, Is.EqualTo("1"));
        }
        [Test]
        public void ButtonNewPatient_Click_AddsNewPatient1()
        {

            var form = new MainForm();
            var signInForm = new SingIn(form, "Администратор", "Test Admin");
            var patients = new Patients(form, signInForm, "Администратор", "Test Admin", "1");

            Button newPatientButton = new Button();
            newPatientButton.Click += (s, e) => patients.ShowPatients("patient");


            newPatientButton.PerformClick();


            Assert.That(patients.forrm, Is.EqualTo("patient"));
        }





    }
}
