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
using OpenQA.Selenium.Support.UI;

namespace DigitacaoAuto
{
    public partial class Form_Atendimento_Individual : Form
    {
        IWebDriver e;
        List<atendimento_ind> Lista;
        List<IWebElement> inputs;
        public Form_Atendimento_Individual(IWebDriver d)
        {
            InitializeComponent();
            e = d;            
        }

        //BTN_START
        private void button1_Click(object sender, EventArgs ee)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => { Task1(); });
            
        }

        private void Task1()
        {

            if (ler_Arquivo_txt())
            {
                for (int t = 0; t < Lista.Count; t++)
                {
                    if (Check_Pagina("#/cds/user/atendimentoIndividual/detail?", "#/cds/user/atendimentoIndividual/detail?"))
                    {
                        if (ClickButton("//button[contains(text(), 'Adicionar')]", 0))
                        {
                            if (Check_Pagina("#/cds/user/atendimentoIndividual/detail/atendimentoDetailChild?", "#/cds/user/atendimentoIndividual/detail/atendimentoDetailChild?"))
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
                        MessageBox.Show("#/cds/user/atendimentoIndividual/detail? pagina não encontrada.");
                        return;
                    }
                }
            }

        }
        private void Robo(atendimento_ind a)
        {
            try
            {
                Log("Nº " +a.ordem+" cns "+a.cns+ " dt "+a.nascimento);
                inputs = new List<IWebElement>();
                inputs.AddRange(e.FindElements(OpenQA.Selenium.By.XPath("//input[contains(@id, 'ext-comp')]")));
               // var random = new Random();
               // var r1 = random.Next(0, 2);
                if (a.turno == "1") 
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
                else if(a.local == "4")
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

                tipo_atendimento(a.tip_atend);


                if (a.local == "4") { inputs[24].Click(); } // AD2
                if (a.peso == "0") { } else { inputs[28].SendKeys(a.peso);}
                if (a.altura == "0") { } else { inputs[29].SendKeys(a.altura);}
                if (a.condicao == "0") { } else { problema(a.condicao); }
                if(a.DUM =="xxxxxxxx.xx.x.x"){} else {inputs[30].Click(); DUM(a.DUM);}//já clica em vacina em dia e chama o métodos DUM()                
                if (a.ciap == "0") { } 
                else 
                {
                    string[] v = a.ciap.Split('.');
                    
                        inputs[61].SendKeys(v[0]);
                        Thread.Sleep(1000);
                        var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo')]"));
                        if (abc.Count > 0)
                        {
                            CIAP(abc.ToList(), a.ciap.ToUpper());
                        }

                    
                }
                if (a.cid == "0") { }                    //    if(v.Length > 1)
                    //{
                    //    inputs[62].SendKeys(v[1]);
                    //    Thread.Sleep(1000);
                    //    var abcc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo')]"));
                    //    if (abcc.Count > 0)
                    //    {
                    //        CIAP(abcc.ToList(), a.ciap.ToUpper());
                    //    }
                    //}
                else
                {
                    inputs[63].SendKeys(a.cid);
                    Thread.Sleep(1000);
                    ////div[contains(@class, 'x-combo-list-inner')]//div[contains(@id, 'ext')]
                    inputs[63].SendKeys(OpenQA.Selenium.Keys.Down);
                    inputs[63].SendKeys(OpenQA.Selenium.Keys.Down);
                    inputs[63].SendKeys(OpenQA.Selenium.Keys.Down);
                    inputs[63].SendKeys(OpenQA.Selenium.Keys.Down);
                    inputs[63].SendKeys(OpenQA.Selenium.Keys.Down);
                    inputs[63].SendKeys(OpenQA.Selenium.Keys.Tab);
                    //var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-inner')]"));
                   // var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo')]"));
                   // if (abc.Count > 0)
                   // {
                   //     CID(abc.ToList(), a.cid.ToUpper());
                  //  }
                }

                if (a.nasf == "0") { } else { nasf(a.nasf); }
                if (a.conduta == "0") { } else { conduta(a.conduta); }
                if (a.encaminhamento== "0") { } else { encaminhamento(a.encaminhamento); }
                ClickButton("//button[contains(text(), 'Confirmar')]",1);
            }
            catch (Exception ex) { MessageBox.Show("Erro " + ex.ToString()); }
        }
        
        
         private void encaminhamento(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[124].Click(); break;
                    case "2": inputs[125].Click(); break;
                    case "3": inputs[126].Click(); break;
                    case "4": inputs[127].Click(); break;
                    case "5": inputs[128].Click(); break;
                    case "6": inputs[129].Click(); break;
                    case "7": inputs[130].Click(); break;                   
                }
            }
        }
        private void conduta(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[119].Click(); break;
                    case "2": inputs[120].Click(); break;
                    case "3": inputs[121].Click(); break;
                    case "4": inputs[122].Click(); break;
                    case "5": inputs[123].Click(); break;
                }
            }

        }
        private void nasf(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[116].Click(); break;
                    case "2": inputs[117].Click(); break;
                    case "3": inputs[118].Click(); break;                    
                }
            }

        }
        private void problema(string txt)
        {
            string[] v = txt.Split('.');
            for (int t = 0; t < v.Length; t++)
            {
                switch (v[t])
                {
                    case "1": inputs[39].Click(); break;
                    case "2": inputs[40].Click(); break;
                    case "3": inputs[41].Click(); break;
                    case "4": inputs[42].Click(); break;
                    case "5": inputs[43].Click(); break;
                    case "6": inputs[44].Click(); break;
                    case "7": inputs[45].Click(); break;
                    case "8": inputs[46].Click(); break;
                    case "9": inputs[47].Click(); break;
                    case "10": inputs[48].Click(); break;
                    case "11": inputs[49].Click(); break;
                    case "12": inputs[50].Click(); break;
                    case "13": inputs[51].Click(); break;
                    case "14": inputs[52].Click(); break;
                    case "15": inputs[53].Click(); break;
                    case "74": inputs[74].Click(); break;
                }
            }

        }
        private void DUM(string texto)
        {
            string[] v = texto.Split('.');            
            if(v[0] =="xxxxxxxx"){}else{inputs[33].SendKeys(v[0]);} // DATA
            if(v[1] =="xx"){}else{inputs[36].SendKeys(v[1]);} //IDADE GESTACIONAL  
            if(v[2] =="xx"){}else{inputs[37].SendKeys(v[2]);} //GESTAS PREVIAS
            if(v[3] =="xx"){}else{inputs[38].SendKeys(v[3]);} //PARTOS                          
          
        }
        private void tipo_atendimento(string txt)
        {
            switch (txt)
            {
                case "1": inputs[18].Click(); break;
                case "2": inputs[19].Click(); break;
                case "3": inputs[20].Click(); break;
                case "4": inputs[21].Click(); break;
                case "5": inputs[22].Click(); break;
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
        private bool CID(List<IWebElement> lst, string _txt)
        {            
            for (int tt = 0; tt < lst.Count; tt++)
            {
                if (lst[tt].Text.Contains("-"))
                {
                    string v = lst[tt].Text.Split('-')[1];
                    if (v.Replace(" ", "") == (_txt))
                    {
                        lst[tt].Click(); return true;
                    }
                }
            }
            return false;
        }
        private bool CIAP(List<IWebElement> lst, string _txt)
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
        private bool ClickButton(string xpath,int index)
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
                string arquivo = "atendimentos.txt";
                string path = Path.Combine(desktop, arquivo);
                if (File.Exists(path))
                {
                    Lista = new List<atendimento_ind>();
                    using (StreamReader file = new StreamReader(path))
                    {
                        string[] ln;
                        string ll;
                        atendimento_ind a;
                        int count = 1;
                        while ((ll = file.ReadLine()) != null)
                        {
                            
                            ln = ll.Split('#');
                            a = new atendimento_ind();
                            a.ordem = count;
                            a.turno = ln[0];
                            a.nome = ln[1];
                            a.cns = ln[2];
                            a.nascimento = ln[3];
                            a.sexo = ln[4];
                            a.local = ln[5];
                            a.tip_atend = ln[6];
                            a.peso = ln[7];
                            a.altura = ln[8];
                            a.condicao = ln[9];
                            a.ciap = ln[10];
                            a.cid = ln[11];
                            a.nasf = ln[12];
                            a.conduta = ln[13];
                            a.encaminhamento = ln[14];
                            a.DUM = ln[15];
                            Lista.Add(a);
                            count++;
                        }
                        file.Close();
                        if (Lista.Count > 0) { Log("Total de itens "+Lista.Count); return true; }
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
        private bool Check_Pagina(string url,string urll)
        {
            try
            {                
            primeiro:
                Thread.Sleep(500);
                if (e.Url.Contains(url) ||(e.Url.Contains(urll)))
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

        private void button2_Click(object sender, EventArgs ee)
        {
            try
            {
                var aa = e.FindElements(By.XPath(textBox1.Text));
                SelectElement oSelect = new SelectElement(e.FindElement(By.XPath(textBox1.Text)));
            }
            catch(Exception ex){}
        }

        private void Log(string texto)
        {
            this.richTextBox1.BeginInvoke(new Action(() =>
            {
                this.richTextBox1.AppendText(Environment.NewLine + texto);
            }));
        }
    }

    class atendimento_ind
    {
        public int ordem;
        public string turno;
        public string nome;
        public string cns;
        public string nascimento;
        public string sexo;
        public string local;
        public string tip_atend;
        public string peso;
        public string altura;
        public string condicao;
        public string ciap;
        public string cid;
        public string nasf;
        public string conduta;
        public string encaminhamento;
        public string DUM; //xxxxxxxx.xx.x.x

    }
}
