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
using System.Threading;

namespace DigitacaoAuto
{
    public partial class Visita_Domiciliar : Form
    {
        IWebDriver e;
        List<VisitaDomiciliar> Lista;
        List<IWebElement> inputs;
        public Visita_Domiciliar(IWebDriver _e)
        {
            InitializeComponent();
            e = _e;
        }
        private void Log(string texto)
        {
            this.richTextBox1.BeginInvoke(new Action(() =>
            {
                this.richTextBox1.AppendText(Environment.NewLine + texto);
            }));
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => { Task1(); });
        }

        private void Task1()
        {
            ler_Arquivo_txt();
            for (int t = 0; t < Lista.Count; t++)
            {
                if (Check_Pagina("/cds/user/visitaDomiciliar/detail?", "/cds/user/visitaDomiciliar/detail?"))
                {
                    if (ClickButton("//button[contains(text(), 'Adicionar')]", 0))
                    {
                        if (Check_Pagina("/cds/user/visitaDomiciliar/detail/visitaDomiciliarChild", "/cds/user/visitaDomiciliar/detail/visitaDomiciliarChild"))
                        {
                            Log("Prontuario " + Lista[t].prontuario);
                            Robo(Lista[t]);
                        }

                    }
                }
            }

        }
        private void Robo(VisitaDomiciliar a)
        {
            try
            {
                inputs = new List<IWebElement>();
                inputs.AddRange(e.FindElements(OpenQA.Selenium.By.XPath("//input[contains(@id, 'ext-comp')]")));
                Log("id "+inputs[0]);
                var random = new Random();
                var r1 = random.Next(0, 2);
                if (r1 == 1)
                {
                    inputs[5].Click();//dia
                }
                else
                {
                    inputs[6].Click();//tard
                }

                inputs[8].SendKeys(a.area);
                inputs[10].SendKeys("DOMICÍLIO");
                    Thread.Sleep(1000);
                vixi:
                    var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-item') and text() = '01 - DOMICÍLIO']"));
                    if (local(abc.ToList(), "DOMIC")) { }
                    else { goto vixi; }

                    inputs[11].SendKeys(a.prontuario);

                if (a.cns.Length == 15) { inputs[12].SendKeys(a.cns); } else { }
                inputs[13].SendKeys(a.nascimento);
                if (a.sexo == "0") { inputs[14].Click(); } else { inputs[15].Click(); }


                vav(a.vcv);
                busca_ativa(a.buscaAtiva);
                acompanhamento(a.acompanhamento);
                controle_vetorial(a.controle);
                ECOO(a.ecoo);

                
                if (a.peso == "0") { } else { inputs[53].SendKeys(a.peso); }
                if (a.altura == "0") { } else { inputs[54].SendKeys(a.altura); }

                if (a.desfecho =="1") { inputs[55].Click(); }
                else if (a.desfecho == "0") { inputs[57].Click(); }
                else  { inputs[56].Click(); }
               
                ClickButton("//button[contains(text(), 'Confirmar')]", 0);
            }
            catch (Exception ex) { MessageBox.Show("Erro " + ex.ToString()); }
        }
        private void acompanhamento(string txt)
        {
            if (txt.Contains("GES")) { inputs[23].Click(); }
            else if (txt.Contains("RECEM")) { inputs[25].Click(); }
            else if (txt.Contains("CRIANCA")) { inputs[26].Click(); }
            else if (txt.Contains("DEF")) { inputs[28].Click(); }
            else if (txt.Contains("HAS")) { inputs[29].Click(); }
            else if (txt.Contains("DM")) { inputs[30].Click(); }
            else if (txt.Contains("TAB")) { inputs[38].Click(); }
            else if (txt.Contains("BF")) { inputs[41].Click(); }
            else if (txt.Contains("ALC")) { inputs[43].Click(); }
        }
        private void vav(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[16].Click(); break;
                    case "2": inputs[17].Click(); break;
                    case "3": inputs[18].Click(); break;
                    
                }
            }

        }
        private void busca_ativa(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[19].Click(); break;
                    case "2": inputs[20].Click(); break;
                    case "3": inputs[21].Click(); break;
                    case "4": inputs[22].Click(); break;

                }
            }

        }
        private void ECOO(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[49].Click(); break;
                    case "2": inputs[50].Click(); break;
                    case "3": inputs[51].Click(); break;
                    case "4": inputs[52].Click(); break;

                }
            }

        }
        private void controle_vetorial(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[45].Click(); break;
                    case "2": inputs[46].Click(); break;
                    case "3": inputs[47].Click(); break;
                    case "4": inputs[48].Click(); break;

                }
            }

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
        private bool Check_Pagina(string url, string urll)
        {
            try
            {
            primeiro:
                Thread.Sleep(500);
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
        private bool ler_Arquivo_txt()
        {
            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                Log(desktop);
                string arquivo = "visitadomiciliar.txt";
                Log(arquivo);
                string path = Path.Combine(desktop, arquivo);
                if (File.Exists(path))
                {
                    Lista = new List<VisitaDomiciliar>();
                    using (StreamReader file = new StreamReader(path))
                    {
                        string[] ln;
                        string ll;
                        VisitaDomiciliar a;
                        while ((ll = file.ReadLine()) != null)
                        {
                            ln = ll.Split('#');
                            a = new VisitaDomiciliar();
                            a.area = ln[0];
                            a.prontuario = ln[1];
                            a.cns = ln[2];
                            a.nascimento = ln[3].Replace("/","");
                            a.sexo = ln[4];
                            a.vcv = ln[5];
                            a.buscaAtiva = ln[6];                            
                            a.acompanhamento = ln[7];
                            a.controle = ln[8];
                            a.ecoo = ln[9];
                            a.peso = ln[10];
                            a.altura = ln[11];
                            a.desfecho = ln[12];                           
                            Lista.Add(a);
                        }
                        file.Close();
                        if (Lista.Count > 0) { Log("Total visitas " + Lista.Count); return true; }
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
    class VisitaDomiciliar
    {
        public string area;
        public string prontuario;
        public string cns;
        public string nascimento;
        public string sexo;        
        public string vcv;
        public string buscaAtiva;
        public string acompanhamento;
        public string controle;
        public string ecoo;
        public string peso;
        public string altura;
        public string desfecho;        
    }
}
