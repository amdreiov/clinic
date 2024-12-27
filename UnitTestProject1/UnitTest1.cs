using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Windows.Forms;
using System.Drawing;
using WindowsFormsApp;


[TestClass]
public class AuthorizationTests
{
    [TestMethod]
    public void MainForm_InitializeMainForm_Test() 
        { var mainForm = new MainForm(); 
            // Проверка наличия основных компонентов на форме
            Assert.IsNotNull(mainForm.pictureBox); 
            Assert.IsNotNull(mainForm.labelLogin); 
            Assert.IsNotNull(mainForm.textboxLogin); 
            Assert.IsNotNull(mainForm.labelPassword); 
            Assert.IsNotNull(mainForm.textboxPassword); 
            Assert.IsNotNull(mainForm.buttonSignIn); 
             //Проверка параметров формы
             Assert.AreEqual(new Size(800, 600),
             mainForm.ClientSize);
             Assert.AreEqual(ColorTranslator.FromHtml("#9A6D0D"), mainForm.BackColor); 
    }

    
}