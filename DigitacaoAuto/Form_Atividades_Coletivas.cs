using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;

namespace DigitacaoAuto
{
    public partial class Form_Atividades_Coletivas : Form
    {
        IWebDriver driver;
        List<Atv_Col> Lista;
        List<IWebElement> inputs;

        public Form_Atividades_Coletivas(IWebDriver d)
        {
            InitializeComponent();
            driver = d;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ler_Arquivo_txt())
            {
                inputs = new List<IWebElement>();
                inputs.AddRange(driver.FindElements(OpenQA.Selenium.By.XPath("//input[contains(@id, 'ext-comp')]")));
                var botoes = driver.FindElements(OpenQA.Selenium.By.XPath("//button[contains(text(), 'Confirmar')]"));
                for (int t = 0; t < Lista.Count; t++)
                {
                    try
                    {
                        if (Lista[t].cns.Length == 15) { inputs[78].SendKeys(Lista[t].cns); } else { }
                        inputs[79].SendKeys(Lista[t].nascimento);
                        if (Lista[t].sexo == "0") { inputs[80].Click(); } else { inputs[81].Click(); }
                        if (Lista[t].alterado == "1") { inputs[82].Click(); } else { }
                        if (Lista[t].peso == "0") { } else { inputs[83].SendKeys(Lista[t].peso); }
                        if (Lista[t].altura == "0") { } else { inputs[84].SendKeys(Lista[t].altura); }
                        botoes[1].Click();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                }
            }
            else
            {

            }
        }
        private bool ler_Arquivo_txt()
        {
            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string arquivo = "atividades.txt";
                string path = System.IO.Path.Combine(desktop, arquivo);
                if (System.IO.File.Exists(path))
                {
                    Lista = new List<Atv_Col>();
                    using (System.IO.StreamReader file = new System.IO.StreamReader(path))
                    {
                        string[] ln;
                        string linha;
                        Atv_Col a;
                        while ((linha = file.ReadLine()) != null)
                        {
                            ln = linha.Split('#');
                            a = new Atv_Col();
                            a.cns = ln[1];
                            a.nascimento = ln[2];
                            a.sexo = ln[3];                            
                            a.peso = ln[4];
                            a.altura = ln[5];
                            a.alterado = ln[6];                            
                            Lista.Add(a);
                        }
                        file.Close();
                        if (Lista.Count > 0) { return true; }
                        else { return false; }
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex) { MessageBox.Show("Erro metodo ler arquivo. " + ex.ToString()); return false; }
        }
        
    }
    class Atv_Col
    {
        public string cns;
        public string nascimento;
        public string sexo;        
        public string peso;
        public string altura;
        public string alterado;
    }
}
