using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;



namespace DigitacaoAuto
{
    public partial class Form1 : Form
    {
        IWebDriver driver;
        List<IWebElement> input16 = new List<IWebElement>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var driverService = ChromeDriverService.CreateDefaultService();
                Log("\r\n"+ driverService.HostName+"\r\n"+driverService.IsRunning);
                driverService.HideCommandPromptWindow = true;
                driver = new ChromeDriver(driverService, new ChromeOptions());                
                driver.Navigate().GoToUrl("http://localhost:8090/esus/#/cds/user");
                Log("title " +driver.Title+" URL "+driver.Url+" wh "+driver.WindowHandles+" cwh "+driver.CurrentWindowHandle);
            }

            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            
        }

        private void Log(string texto)
        {
            richTextBox1.AppendText(Environment.NewLine + texto);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                driver.Close();
                driver.Quit();
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var data = driver.FindElement(By.XPath("//input[contains(@id, 'ext-comp')]")).GetAttribute("outerHTML");
            //List<IWebElement> input15 = new List<IWebElement>(); input15.AddRange(driver.FindElements(By.TagName("input")));
            if (input16.Count > 0) { input16.Clear(); }
            input16.AddRange(driver.FindElements(By.XPath("//input[contains(@id, 'ext-comp')]")));

            List<string> xt = new List<string>();
            for (int b = 0; b < input16.Count; b++)
            {
                //xt.Add(input15[b].GetAttribute("id"));
                //xt.Add(input15[b].GetAttribute("text"));
                //xt.Add(input15[b].GetAttribute("name"));
                //xt.Add(input15[b].GetAttribute("class"));
                //xt.Add(input15[b].GetAttribute("value"));
                //xt.Add(input15[b].GetAttribute("type"));
                xt.Add(input16[b].GetAttribute("outerHTML"));
            }
            label1.Text = "total itens" + xt.Count;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (input16.Count > 0)
                {
                    int num = Convert.ToInt32(numericUpDown1.Value);
                    if (input16.ElementAtOrDefault(num) != null)
                    {
                        input16[num].Click();
                        label2.Text = "click" + input16[num].ToString();
                    }
                }
            }

            catch(Exception ex){MessageBox.Show(ex.ToString());}
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form_Procedimentos f = new Form_Procedimentos(driver);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }


        //btn_atendimento_individual
        private void button6_Click(object sender, EventArgs e)
        {
            //this.Hide();
            Form_Atendimento_Individual f = new Form_Atendimento_Individual(driver);
            this.Hide();
            f.ShowDialog();
            this.Show();
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form_Odontologia f = new Form_Odontologia(driver);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            Form_Atividades_Coletivas f = new Form_Atividades_Coletivas(driver);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Visita_Domiciliar f = new Visita_Domiciliar(driver);            
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
