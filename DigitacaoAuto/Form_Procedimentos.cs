using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;
using System.IO;

namespace DigitacaoAuto
{
    public partial class Form_Procedimentos : Form
    {
        IWebDriver e;
        List<Procediemntos> Lista;
        List<IWebElement> inputs;

        public Form_Procedimentos(IWebDriver _e)
        {
            InitializeComponent();
            e = _e;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ler_Arquivo_txt();
            for (int t = 0; t < Lista.Count; t++)
            {
                if (Check_Pagina("/procedimentos/detail?", "/procedimentos/detail?"))
                {
                    if (ClickButton("//button[contains(text(), 'Adicionar')]", 0))
                    {
                        if (Check_Pagina("procedimentosChild", "procedimentosChild"))
                        {
                            Robo(Lista[t]);
                        }

                    }
                }
            }
        }
        private void Robo(Procediemntos a)
        {
            try
            {
                inputs = new List<IWebElement>();
                inputs.AddRange(e.FindElements(OpenQA.Selenium.By.XPath("//input[contains(@id, 'ext-comp')]")));
                //var random = new Random();
                //var r1 = random.Next(0, 2);
                if (a.turno == "1")
                {
                    inputs[5].Click();//dia
                }
                else
                {
                    inputs[6].Click();//tard
                }


                if (a.cns.Length == 15) { inputs[9].SendKeys(a.cns); } else { }
                inputs[10].SendKeys(a.nascimento);
                if (a.sexo == "0") { inputs[11].Click(); } else { inputs[12].Click(); }
                if (a.local == "1")
                {
                    inputs[13].SendKeys("UBS");
                    System.Threading.Thread.Sleep(1000);
                vixi:
                    var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-item') and text() = '01 - UBS']"));
                if (local(abc.ToList(), "UBS")) { }
                    else { goto vixi; }
                }
                else
                {
                    inputs[13].SendKeys("DOMICILIO");
                vixi:
                    var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-item') and text() = '04 - DOMICÍLIO']"));
                if (local(abc.ToList(), "DOMIC")) { }
                    else { goto vixi; }
                }
                if (a.escuta == "1") { inputs[14].Click(); }
                proced(a.procedimento);
                testeRapido(a.teste);
                adminitacao(a.admin);



                if (a.PA == "1")
                {
                    inputs[49].Click(); //PA
                    inputs[49].SendKeys("301100039");
                    System.Threading.Thread.Sleep(1000);
                    inputs[49].SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                    inputs[49].SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                    inputs[49].SendKeys(OpenQA.Selenium.Keys.Tab);

                }

                    ClickButton("//button[contains(text(), 'Confirmar')]", 0);
            }
            catch (Exception ex) { MessageBox.Show("Erro " + ex.ToString()); }
        }
        private bool local(List<IWebElement> lst, string _txt)
        {
            for (int tt = 0; tt < lst.Count; tt++)
            {
                if (lst[tt].Text.Contains(_txt))
                {
                    lst[tt].Click(); return true;
                }
            }
            return false;
        }
        private bool ClickButton(string xpath, int index)
        {
            try
            {
                var itens = e.FindElements(By.XPath(xpath));
                if (itens.Count > 0)
                {
                    itens[index].Click();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao click botao " + ex.ToString()); return false; }

        }
        private bool ler_Arquivo_txt()
        {
            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string arquivo = "procedimentos.txt";
                string path = Path.Combine(desktop, arquivo);
                if (File.Exists(path))
                {
                    Lista = new List<Procediemntos>();
                    using (StreamReader file = new StreamReader(path))
                    {
                        string[] v;
                        string ll;
                        Procediemntos a;
                        while ((ll = file.ReadLine()) != null)
                        {
                            v = ll.Split('#');
                            a = new Procediemntos();
                            a.turno = v[0];
                            a.nome = v[1];                            
                            a.cns = v[2];
                            a.nascimento = v[3];
                            a.sexo = v[4];
                            a.local = v[5];
                            a.escuta = v[6];
                            a.procedimento = v[7];
                            a.teste = v[8];
                            a.admin = v[9];
                            a.PA = v[10];
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
        private bool Check_Pagina(string url, string urll)
        {
            try
            {
            primeiro:
                System.Threading.Thread.Sleep(500);
                if (e.Url.Contains(url) || (e.Url.Contains(urll)))
                {
                    return true;
                }
                else
                {
                    goto primeiro;
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro metodo Check pagina. " + ex.ToString()); return false; }
        }
        private void proced(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[15].Click(); break;
                    case "2": inputs[16].Click(); break;
                    case "3": inputs[17].Click(); break;
                    case "4": inputs[18].Click(); break;
                    case "5": inputs[19].Click(); break;
                    case "6": inputs[20].Click(); break;
                    case "7": inputs[21].Click(); break;
                    case "8": inputs[22].Click(); break;
                    case "9": inputs[23].Click(); break;
                    case "10": inputs[24].Click(); break;
                    case "11": inputs[25].Click(); break;
                    case "12": inputs[26].Click(); break;
                    case "13": inputs[27].Click(); break;
                    case "14": inputs[28].Click(); break;
                    case "15": inputs[29].Click(); break;
                    case "16": inputs[30].Click(); break;
                    case "17": inputs[31].Click(); break;
                    case "18": inputs[32].Click(); break;
                    case "19": inputs[33].Click(); break;
                    case "20": inputs[34].Click(); break;
                    case "21": inputs[35].Click(); break;
                    case "22": inputs[36].Click(); break;
                }
            }

        }
        private void testeRapido(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[37].Click(); break;
                    case "2": inputs[38].Click(); break;
                    case "3": inputs[39].Click(); break;
                    case "4": inputs[40].Click(); break;
                    case "5": inputs[41].Click(); break;
                }
            }

        }
        private void adminitacao(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[42].Click(); break;
                    case "2": inputs[43].Click(); break;
                    case "3": inputs[44].Click(); break;
                    case "4": inputs[45].Click(); break;
                    case "5": inputs[46].Click(); break;
                    case "6": inputs[47].Click(); break;
                    case "7": inputs[48].Click(); break;

                }
            }

        }
    }
    public class Procediemntos
    {
        public string nome;
        public string turno;
        public string cns;
        public string nascimento;
        public string sexo;
        public string local;
        public string escuta;
        public string procedimento;
        public string teste;
        public string admin;
        public string PA;
    }
}