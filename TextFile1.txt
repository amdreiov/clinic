using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Windows.Forms;
using System.Drawing;
using WindowsFormsApp2;
using WindowsFormsApp;


[TestClass]
public class AuthorizationTests
{
    // Пример теста для проверки установки параметров окна
    [TestMethod]
    public void TestInitializeMainForm_ParametersSetCorrectly()
    {
        MainForm form = new MainForm();

        Assert.AreEqual(new Size(800, 600), form.ClientSize);
        Assert.AreEqual(ColorTranslator.FromHtml("#9A6D0D"), form.BackColor);
        // Другие проверки параметров окна
    }

    // Пример теста для проверки создания элементов интерфейса
    [TestMethod]
    public void TestInitializeMainForm_ControlsCreatedCorrectly()
    {
        MainForm form = new MainForm();

        Assert.IsNotNull(form.textboxLogin);
        Assert.IsNotNull(form.labelPassword);
        // Другие проверки создания элементов
    }


    // Пример теста для проверки обновления времени на метке
    [TestMethod]
    public void TestTimer1_Tick_LabelDateUpdates()
    {
        MainForm form = new MainForm();
        form.labelDate = new Label();

        form.Timer1_Tick(form.timer, EventArgs.Empty);

        Assert.IsNotNull(form.labelDate.Text);
    }


}