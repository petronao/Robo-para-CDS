using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;
using System.Threading;

namespace DigitacaoAuto
{
    public partial class Form_Odontologia : Form
    {
        IWebDriver e;
        List<Odonto> Lista;
        List<IWebElement> inputs;

        public Form_Odontologia(IWebDriver iw)
        {
            InitializeComponent();
            e = iw;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ler_Arquivo_txt())
            {
                for (int t = 0; t < Lista.Count; t++)
                {
                    if (Check_Pagina("atendimentoOdontologico/detail?"))
                    {
                        if (ClickButton("//button[contains(text(), 'Adicionar')]", 0))
                        {
                            if (Check_Pagina("atendimentoOdontologico/detail/atendiOdontoChild?"))
                            {
                                Robo(Lista[t]);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("1 pagina não encontrada.");
                        return;
                    }
                }

            }
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
                string arquivo = "odontologia.txt";
                string path = System.IO.Path.Combine(desktop, arquivo);
                if (System.IO.File.Exists(path))
                {
                    Lista = new List<Odonto>();
                    using (System.IO.StreamReader file = new System.IO.StreamReader(path))
                    {
                        string[] ln;
                        string linha;
                        Odonto a;
                        while ((linha = file.ReadLine()) != null)
                        {
                            ln = linha.Split('#');
                            a = new Odonto();
                            //0 = nome
                            a.cns = ln[1];
                            a.nascimento = ln[2];
                            a.sexo = ln[3];
                            a.local = ln[4];
                            a.tipo_atendimento = ln[5];
                            a.consulta = ln[6];
                            a.vigilancia = ln[7];
                            a.fluo = ln[8];
                            a.orientacao = ln[9];
                            a.bacteria =ln[10];
                            a.anteriror=ln[11];
                            a.posterior=ln[12];
                            a.selamento=ln[13];
                            a.fim=ln[14];
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
        private bool Check_Pagina(string url)
        {
            try
            {
            primeiro:
                Thread.Sleep(500);
                if (e.Url.Contains(url))
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

        private void Robo(Odonto a)
        {
            try
            {
                inputs = new List<IWebElement>();
                inputs.AddRange(e.FindElements(OpenQA.Selenium.By.XPath("//input[contains(@id, 'ext-comp')]")));
                var random = new Random();
                var r1 = random.Next(0, 2);
                if (r1 == 1)
                {
                    inputs[9].Click();//dia
                }
                else
                {
                    inputs[10].Click();//tard
                }

                //13 cns
                if (a.cns.Length == 15) { inputs[13].SendKeys(a.cns); } else { }
                inputs[14].SendKeys(a.nascimento);
                if (a.sexo == "0") { inputs[15].Click(); } else { inputs[16].Click(); }

                if (a.local == "1")
                {
                    inputs[17].SendKeys("UBS");
                    Thread.Sleep(1000);
                vixi:
                    var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-item') and text() = '01 - UBS']"));
                    if (local(abc.ToList(), "UBS")) { }
                    else { goto vixi; }
                }
                else if (a.local == "4")
                {
                    inputs[17].SendKeys("DOMICILIO");
                    Thread.Sleep(1000);
                vixi:
                    var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-item') and text() = '04 - DOMICÍLIO']"));
                    if (local(abc.ToList(), "DOMIC")) { }
                    else { goto vixi; }
                }
                else
                {
                    inputs[17].SendKeys("OUTROS");
                    Thread.Sleep(1000);
                vixi:
                    var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-item') and text() = '06 - OUTROS']"));
                    if (local(abc.ToList(), "OUTROS")) { }
                    else { goto vixi; }
                }

                tipo_atendimento(a.tipo_atendimento); // inputs[20].Click(); // tipo atendimento
                tipo_consulta(a.consulta);// inputs[25].Click(); // tipo consulta
                vigilancia(a.vigilancia);// inputs[33].Click(); // nao identificado

                if (a.fluo == "0") {} else { inputs[38].SendKeys(a.fluo); } // fluoo
                if (a.orientacao == "0"){} else { inputs[48].SendKeys(a.orientacao);}  // orientacao
                if (a.bacteria == "0") {} else{ inputs[49].SendKeys(a.bacteria);  }//bacteria
                if (a.anteriror == "0") {} else{ inputs[55].SendKeys(a.anteriror);  }//anterior
                if (a.posterior == "0") {} else{ inputs[56].SendKeys(a.posterior);  }//posterior
                if (a.selamento == "0") { } else { inputs[58].SendKeys(a.selamento); }//selamento
                if (a.fim == "1") { inputs[66].Click(); } // retorno agendado
                else { inputs[70].Click(); } // tratamento concluido
               
                
                ClickButton("//button[contains(text(), 'Confirmar')]", 1);
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
        private void tipo_atendimento(string txt)
        {
            switch (txt)
            {       case "1": inputs[20].Click(); break;
                    case "2": inputs[21].Click(); break;
                    case "3": inputs[22].Click(); break;
                    case "4": inputs[23].Click(); break;                    
               
            }

        }
        private void tipo_consulta(string txt)
        {
            switch (txt)
            {
                    case "1": inputs[24].Click(); break;
                    case "2": inputs[25].Click(); break;
                    case "3": inputs[26].Click(); break;
            }

        }
        private void vigilancia(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[27].Click(); break;
                    case "2": inputs[28].Click(); break;
                    case "3": inputs[29].Click(); break;
                    case "4": inputs[30].Click(); break;
                    case "5": inputs[31].Click(); break;
                    case "6": inputs[32].Click(); break;
                    case "7": inputs[33].Click(); break;
                }
            }

        }
    }
    class Odonto
    {
        public string cns;
        public string nascimento;
        public string sexo;
        public string local;
        public string tipo_atendimento;
        public string consulta;
        public string vigilancia;
        public string fluo;
        public string orientacao;
        public string bacteria;
        public string anteriror;
        public string posterior;
        public string selamento;
        public string fim;
    }
}
