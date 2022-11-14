using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenQA.Selenium;
using System.Threading;

namespace DigitacaoAuto
{
    public class ClassProcedimentos
    {
        List<IWebElement> inputs = new List<IWebElement>();
        List<Procediemntos> Lista = new List<Procediemntos>();
        List<string> l = new List<string>();
        public bool lertxt()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string arquivo = "procedimentos.txt";
                string path = Path.Combine(desktop, arquivo);
                if (File.Exists(path))
                {
                    using (StreamReader file = new StreamReader(path))
                    {
                        string ln;
                        while ((ln = file.ReadLine()) != null)
                        {
                            l.Add(ln);
                        }
                        file.Close();
                        if (l.Count > 0) { Processamento(); return true; }
                        else { return false; }
                    }
                }
                else { return false; }
        }
        private void Processamento()
        {
            Procediemntos p;
            for(int a = 0; a < l.Count;a++)
            {
                p = new Procediemntos();
                string [] v = l[a].Split('#');
                p.nome = v[0];
                p.turno = v[1];
                p.cns = v[2];
                p.nascimento = v[3];
                p.sexo = v[4];
                p.local = v[5];
                p.escuta = v[6];
                p.procedimento = v[7];
                p.teste = v[8];
                p.admin = v[9];
                Lista.Add(p);
            }            
        }

        public void robo(OpenQA.Selenium.IWebDriver e)
        {
            for (int t = 0; t < Lista.Count;t++)
            {
                if (e.Url.Contains("/cds/user/procedimentos/detail?"))
                {
                primeiro:
                    var itens = e.FindElements(By.XPath("//button[contains(text(), 'Adicionar')]"));
                    if (itens.Count > 0)
                    {
                        itens[0].Click();

                    page2:
                        if (e.Url.Contains("procedimentosChild"))
                        {
                            inputs.Clear();
                            inputs.AddRange(e.FindElements(OpenQA.Selenium.By.XPath("//input[contains(@id, 'ext-comp')]")));

                            if (Lista[t].turno == "1") { inputs[5].Click(); } else { inputs[6].Click(); }
                            if (Lista[t].cns == "0") { inputs[9].Click(); } else { inputs[9].SendKeys(Lista[t].cns); }
                            inputs[10].SendKeys(Lista[t].nascimento);
                            if ((Lista[t].sexo == "1") || (Lista[t].sexo.ToUpper() == "M")) { inputs[12].Click(); } else { inputs[11].Click(); } //1=m
                            if (Lista[t].local == "1")
                            {
                                inputs[13].SendKeys("UBS");
                                Thread.Sleep(1000);
                            vixi:
                                var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-item') and text() = '01 - UBS']"));                                
                                if (dropBox(abc.ToList(),"UBS")) {}
                                else { goto vixi; }                                
                            }
                            else
                            {
                                inputs[13].SendKeys("DOMICILIO");
                            vixi:
                                var abc = e.FindElements(By.XPath("//div[contains(@class, 'x-combo-list-item') and text() = '04 - DOMICÍLIO']"));
                                if (dropBox(abc.ToList(),"DOMIC")) { }
                                else { goto vixi; }     
                            }
                            if (Lista[t].escuta == "1") { inputs[14].Click(); }
                            proced(Lista[t].procedimento);
                            testeRapido(Lista[t].teste);
                            adminitacao(Lista[t].admin);
                            e.FindElement(By.XPath("//button[contains(text(), 'Confirmar')]")).Click();   
                        }
                        else
                        {                            
                            goto page2;
                        }
                    }
                    else
                    {                        
                        goto primeiro;
                    }
                }
                            
            }
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
        private bool dropBox(List<IWebElement> lst,string aa)
        {
            for (int tt = 0; tt < lst.Count; tt++)
            {
                if (lst[tt].Text.Contains(aa))
                {
                    lst[tt].Click(); return true;
                }
            }
            return false;

        }
    }

   
}
