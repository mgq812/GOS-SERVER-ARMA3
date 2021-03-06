﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GOS_Server_A3.Classes;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace GOS_Server_A3.Classes
{
    //  Classe Combo BoxItem

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    class Interface
    {     
        static public void dessineInterface(string profil)
        {
            // 
            if (!Core.IsGOSInterface())
            {
                Var.fenetrePrincipale.tabControl2.TabPages.Remove(Var.fenetrePrincipale.NetWork);
                Var.fenetrePrincipale.tabControl1.TabPages.Remove(Var.fenetrePrincipale.TEMPLATE);
                Var.fenetrePrincipale.tabControl1.TabPages.Remove(Var.fenetrePrincipale.ISLANDS);
                Var.fenetrePrincipale.tabControl1.TabPages.Remove(Var.fenetrePrincipale.UNITS);
                Var.fenetrePrincipale.tabControl1.TabPages.Remove(Var.fenetrePrincipale.MATERIEL);
                Var.fenetrePrincipale.tabControl1.TabPages.Remove(Var.fenetrePrincipale.CLIENT);
                Var.fenetrePrincipale.tabControl1.TabPages.Remove(Var.fenetrePrincipale.TEST);
                Var.fenetrePrincipale.tabControl1.TabPages.Remove(Var.fenetrePrincipale.FRAMEWORK);
                Var.fenetrePrincipale.label77.Visible = false;
                Var.fenetrePrincipale.label75.Visible = false;
                Var.fenetrePrincipale.pictureBox6.Visible = false;
            }
            else
            {
                // Lance Serveur Remote GOS
               // new HttpServer("127.0.0.1", 9090);

            }
            // Genere Tabs 
            genereTab(profil);

            // Dessin Interface Constant
            Var.fenetrePrincipale.label31.Text = Var.VersionProgramme();
            Var.fenetrePrincipale.label76.Text = Var.VersionArma3Exe();

            // Genere Taille Mise a jour
            Var.fenetrePrincipale.label77.Text = "???";
            Core.synchroTailleRsync();

            // Determine presence ARMA 3 64 bit
            if (File.Exists(Var.fenetrePrincipale.textBox18.Text + @"\arma3server_x64.exe"))
            {
                Var.fenetrePrincipale.checkBox_Arma364bit.Enabled = true;
            }
            else
            {
                Var.fenetrePrincipale.checkBox_Arma364bit.Enabled = false;
                Var.fenetrePrincipale.checkBox_Arma364bit.Enabled = false;
            }

            // Bouton Lancer Serveur
            if (!System.IO.File.Exists(Var.fenetrePrincipale.textBox18.Text + @"\arma3server.exe"))
            {
                Var.fenetrePrincipale.button16.Enabled = false;
            }
            else
            {
                Var.fenetrePrincipale.button16.Enabled = true;

            }

            // Determine Langage choisi 
            Language.CheckButtonLanguageOption();
        }
 

        // TABS
        // GESTION TAB MODS 
        static public void genereTab(string profil)
        {
            effaceTousItemsOnglets();
            GestionProfil.ChargeProfilServer(profil);
            
            // @GOS 
            ListeTab(Var.fenetrePrincipale.checkedListBox7, "@TEMPLATE", profil);
            ListeTab(Var.fenetrePrincipale.checkedListBox8, "@FRAMEWORK", profil);
            ListeTab(Var.fenetrePrincipale.checkedListBox1, "@ISLANDS", profil);
            ListeTab(Var.fenetrePrincipale.checkedListBox2, "@UNITS", profil);
            ListeTab(Var.fenetrePrincipale.checkedListBox3, "@MATERIEL", profil);
            ListeTab(Var.fenetrePrincipale.checkedListBox6, "@CLIENT", profil);

            ListeTab(Var.fenetrePrincipale.checkedListBox4, "@TEST", profil);
            // @Autre
            ListeTab(Var.fenetrePrincipale.checkedListBox5, "", profil);
            //genereTabMods();
            genereTabParam(profil);
            TabPriority.genereTabPriorite(profil);
            TabMissions.genereTabMissions(profil);
        }
        static public void effaceTousItemsOnglets()
        {
            Var.fenetrePrincipale.comboBox_ListeApparence.Items.Clear();
            Var.fenetrePrincipale.comboBox_ListeApparence.Items.Add("");
            Var.fenetrePrincipale.checkBox_ToutesApparences.Checked = false;
            Var.fenetrePrincipale.radioButton20.Enabled = false;
            Var.fenetrePrincipale.radioButton20.Checked = false;
            Var.fenetrePrincipale.radioButton21.Enabled = false;
            Var.fenetrePrincipale.radioButton21.Checked = false;
            Var.fenetrePrincipale.pictureBox1.Image = GOS_Server_A3.Properties.Resources.logo_GOS;
            Var.fenetrePrincipale.checkedListBox8.Items.Clear();
            Var.fenetrePrincipale.checkedListBox7.Items.Clear();
            Var.fenetrePrincipale.checkedListBox1.Items.Clear();
            Var.fenetrePrincipale.checkedListBox2.Items.Clear();
            Var.fenetrePrincipale.checkedListBox3.Items.Clear();
            Var.fenetrePrincipale.checkedListBox6.Items.Clear();
            Var.fenetrePrincipale.checkedListBox4.Items.Clear();
            Var.fenetrePrincipale.checkedListBox5.Items.Clear();
        }
        static public List<string> GenereListeGOS(string repertoireSource)
        {
            List<string> listeRepertoire = new List<string>();
            if (!System.IO.Directory.Exists(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\" + repertoireSource))
            {
                return listeRepertoire;
            }



            string[] tableauRepertoire = Directory.GetDirectories(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\" + repertoireSource + @"\", "Add*", SearchOption.AllDirectories);

            foreach (var ligne in tableauRepertoire)
            {
                // Genere les Tab Specifiques pour les tenues GOS
                if ((ligne.IndexOf(@"\@GOS\@TEMPLATE\@GOSSkin_") != -1) && (ligne.IndexOf(@"\@GOS\@TEMPLATE\@GOSUnits_Cfg") == -1))
                {
                    string addons = ligne.Replace(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\@TEMPLATE\@GOSSkin_", "");
                    Var.fenetrePrincipale.comboBox_ListeApparence.Items.Add(addons.Replace(@"\addons", ""));
                }
                else
                    if (ligne.IndexOf(@"\@GOS\@TEMPLATE\@GOSUnit_Helmets") != -1)
                    {
                        if ((ligne.IndexOf(@"@GOS\@TEMPLATE\@GOSUnit_HelmetsST\") != -1)) { Var.fenetrePrincipale.radioButton20.Enabled = true; }
                        if ((ligne.IndexOf(@"@GOS\@TEMPLATE\@GOSUnit_HelmetsXT\") != -1)) { Var.fenetrePrincipale.radioButton21.Enabled = true; }
                    }
                    else
                    {
                        string menuRepertoire = System.IO.Directory.GetParent(ligne).ToString();
                        listeRepertoire.Add(menuRepertoire.Replace(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\" + repertoireSource + @"\", ""));
                    }
            }
            return listeRepertoire;

        }
        static public List<string> GenereListeAUTRE(string repertoireSource)
        {

            List<string> listeRepertoire = new List<string>();
            try
            {
                string[] tableauRepertoire = Directory.GetDirectories(Var.fenetrePrincipale.textBox18.Text, "Add*", SearchOption.AllDirectories);

                foreach (var ligne in tableauRepertoire)
                {
                    string menuRepertoire = System.IO.Directory.GetParent(ligne).ToString();
                    string nomAAjouter = menuRepertoire;
                    if ((nomAAjouter.IndexOf(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\@ISLANDS\") == -1)
                        && (nomAAjouter.IndexOf(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\@FRAMEWORK\") == -1)
                        && (nomAAjouter.IndexOf(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\@UNITS\") == -1)
                        && (nomAAjouter.IndexOf(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\@MATERIEL\") == -1)
                        && (nomAAjouter.IndexOf(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\@TEMPLATE\") == -1)
                        && (nomAAjouter.IndexOf(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\@CLIENT\") == -1)
                        && (nomAAjouter.IndexOf(Var.fenetrePrincipale.textBox18.Text + @"\@GOS\@TEST\") == -1)
                        && (nomAAjouter.IndexOf(".pack") == -1)
                        && (nomAAjouter.IndexOf(".rsync") == -1)
                        )
                    {

                        if ((menuRepertoire) != Var.fenetrePrincipale.textBox18.Text)
                        {
                            listeRepertoire.Add(menuRepertoire.Replace(Var.fenetrePrincipale.textBox18.Text + @"\", ""));
                        }

                    }
                }
            }
            catch
            {
            }
            return listeRepertoire;
        }
        static public void ListeTab(CheckedListBox Tab, string nomRep, string nomProfil)
        {
            List<string> tableauValeur;
            if (nomRep != "")
            {
                tableauValeur = GenereListeGOS(nomRep);
            }
            else
            {
                tableauValeur = GenereListeAUTRE(nomRep);
            }


            string tagNameXML;
            string filtreRepertoire;
            switch (nomRep)
            {
                case "":
                    tagNameXML = "AUTRES_MODS";
                    filtreRepertoire = "";
                    break;
                case "@TEMPLATE":
                    tagNameXML = "TEMPLATE";
                    filtreRepertoire = @"@GOS\@TEMPLATE\";
                    break;
                case "@FRAMEWORK":
                    tagNameXML = "FRAMEWORK";
                    filtreRepertoire = @"@GOS\@FRAMEWORK\";
                    break;
                case "@ISLANDS":
                    tagNameXML = "ISLANDS";
                    filtreRepertoire = @"@GOS\@ISLANDS\";
                    break;
                case "@UNITS":
                    tagNameXML = "UNITS";
                    filtreRepertoire = @"@GOS\@UNITS\";
                    break;
                case "@MATERIEL":
                    tagNameXML = "MATERIEL";
                    filtreRepertoire = @"@GOS\@MATERIEL\";
                    break;
                case "@CLIENT":
                    tagNameXML = "CLIENT";
                    filtreRepertoire = @"@GOS\@CLIENT\";
                    break;
                case "@TEST":
                    tagNameXML = "TEST";
                    filtreRepertoire = @"@GOS\@TEST\";
                    break;
                default:
                    tagNameXML = "AUTRES_MODS";
                    filtreRepertoire = "";
                    break;
            }
            XmlDocument fichierProfilXML = new XmlDocument();
            if (nomProfil == "") { nomProfil = "defaut"; };
            fichierProfilXML.Load(Var.RepertoireDeSauvegarde + nomProfil + ".profil.xml");


            foreach (var ligne in tableauValeur)
            {
                bool elementsProfilChecked = false;
                // Read the XmlDocument (Directory Node)
                XmlNodeList elemList = fichierProfilXML.GetElementsByTagName(tagNameXML);
                if (elemList.Count == 0) { Tab.Items.Add(ligne, elementsProfilChecked); };
                for (int i = 0; i < elemList.Count; i++)
                {
                    XmlNodeList eltList = elemList[i].ChildNodes;
                    for (int j = 0; j < eltList.Count; j++)
                    {
                        string repertoireAChercher = eltList[j].InnerXml;
                        if (repertoireAChercher.IndexOf(@"@GOS\@TEMPLATE\@GOSSkin_") != -1)
                        {
                            int indexApparence = 0;
                            foreach (string apparencePossible in Var.fenetrePrincipale.comboBox_ListeApparence.Items)
                            {
                                if (@"@GOS\@TEMPLATE\@GOSSkin_" + apparencePossible == repertoireAChercher)
                                {
                                    Var.fenetrePrincipale.comboBox_ListeApparence.SelectedIndex = indexApparence;
                                }
                                indexApparence++;
                            }

                        }
                        if (repertoireAChercher == @"@GOS\@TEMPLATE\@GOSUnit_HelmetsST") { Var.fenetrePrincipale.radioButton20.Checked = true; }
                        if (repertoireAChercher == @"@GOS\@TEMPLATE\@GOSUnit_HelmetsXT") { Var.fenetrePrincipale.radioButton21.Checked = true; }

                        if (filtreRepertoire + ligne == repertoireAChercher)
                        {
                            elementsProfilChecked = true;
                        }

                    }
                    Tab.Items.Add(ligne, elementsProfilChecked);
                }

            }



        }
        static public void SelectionneTous(CheckedListBox tab)
        {
            for (int x = 0; x <= tab.Items.Count - 1; x++)
            {
                tab.SetItemChecked(x, true);
            }
        }
        static public void InverseSelection(CheckedListBox tab)
        {
            for (int x = 0; x <= tab.Items.Count - 1; x++)
            {
                if (tab.GetItemChecked(x))
                {
                    tab.SetItemChecked(x, false);
                }
                else
                {
                    tab.SetItemChecked(x, true);
                }
            }
        }
        static public void topSelection(CheckedListBox tab)
        {

            if (tab.SelectedIndex.ToString() != "-1")
            {
                bool etatItem = tab.GetItemChecked(tab.SelectedIndex);
                int index;
                string valeur;
                while (tab.SelectedIndex > 0)
                {
                    valeur = tab.SelectedItem.ToString();
                    index = tab.SelectedIndex;
                    tab.Items.RemoveAt(index);
                    tab.Items.Insert(index - 1, valeur);
                    tab.SetSelected(index - 1, true);
                }
                tab.SetItemChecked(tab.SelectedIndex, etatItem);
            }
        }
        static public void downSelection(CheckedListBox tab)
        {
            if (tab.SelectedIndex.ToString() != "-1")
            {
                bool etatItem = tab.GetItemChecked(tab.SelectedIndex);
                
                while (tab.SelectedIndex < tab.Items.Count - 1)
                {
                    string valeur = tab.SelectedItem.ToString();
                    int index = tab.SelectedIndex;
                    tab.Items.RemoveAt(index);
                    tab.Items.Insert(index + 1, valeur);
                    tab.SetSelected(index + 1, true);
                }
                tab.SetItemChecked(tab.SelectedIndex, etatItem);
            }
        }
        static public void augmenteSelection(CheckedListBox tab)
        {
            if (tab.SelectedIndex.ToString() != "-1")
            {
                string valeur;
                int index;
                bool etatItem = tab.GetItemChecked(tab.SelectedIndex);
                if (tab.SelectedIndex > 0)
                {
                    valeur = tab.SelectedItem.ToString();
                    index = tab.SelectedIndex;
                    tab.Items.RemoveAt(index);
                    tab.Items.Insert(index - 1, valeur);
                    tab.SetItemChecked(index - 1, etatItem);
                    tab.SetSelected(index - 1, true);
                }
            }
        }
        static public void diminueSelection(CheckedListBox tab)
        {
            if (tab.SelectedIndex.ToString() != "-1")
            {
                if (tab.SelectedIndex < tab.Items.Count - 1)
                {
                    bool etatItem = tab.GetItemChecked(tab.SelectedIndex);
                    string valeur = tab.SelectedItem.ToString();
                    int index = tab.SelectedIndex;
                    tab.Items.RemoveAt(index);
                    tab.Items.Insert(index + 1, valeur);
                    tab.SetItemChecked(index + 1, etatItem);
                    tab.SetSelected(index + 1, true);
                }
            }
        }

        // GESTION TABS PARAM

        static public void effaceTousparamsOnglet()
        {
            Var.fenetrePrincipale.checkBox1.Checked = false;
            Var.fenetrePrincipale.checkBox2.Checked = false;
            Var.fenetrePrincipale.checkBox3.Checked = false;
            Var.fenetrePrincipale.checkBox4.Checked = false;
            Var.fenetrePrincipale.checkBox5.Checked = false;
            Var.fenetrePrincipale.checkBox6.Checked = false;
            Var.fenetrePrincipale.checkBox7.Checked = false;
            Var.fenetrePrincipale.checkBox8.Checked = false;
            Var.fenetrePrincipale.checkBox9.Checked = false;
            Var.fenetrePrincipale.checkBox10.Checked = false;
            Var.fenetrePrincipale.checkBox11.Checked = false;
            Var.fenetrePrincipale.checkBox19.Checked = false;
            Var.fenetrePrincipale.checkBox22.Checked = false;
            Var.fenetrePrincipale.checkBox23.Checked = false;
            Var.fenetrePrincipale.checkBox21.Checked = false;
            Var.fenetrePrincipale.checkBox24.Checked = false;
            Var.fenetrePrincipale.checkBox_enableHT.Checked = false;
            Var.fenetrePrincipale.textBox22.Text = "";
            Var.fenetrePrincipale.checkBox_Arma364bit.Checked = false;

        }
        static public void genereTabParam(string profil)
        {
            effaceTousparamsOnglet();
            if (profil == "") return;
            XmlTextReader fichierProfilXML = new XmlTextReader(Var.RepertoireDeSauvegarde + profil + ".profil.xml");
                fichierProfilXML.ReadToFollowing("winXP");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox9.Checked = true; }
                fichierProfilXML.ReadToFollowing("showScriptErrors");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox5.Checked = true; }
                fichierProfilXML.ReadToFollowing("worldEmpty");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox4.Checked = true; }
                fichierProfilXML.ReadToFollowing("noPause");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox2.Checked = true; }
                fichierProfilXML.ReadToFollowing("nosplash");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox1.Checked = true; }
                fichierProfilXML.ReadToFollowing("window");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox3.Checked = true; }
                fichierProfilXML.ReadToFollowing("maxMem");
                string maxmem = fichierProfilXML.ReadString();
                if (maxmem != "") { Var.fenetrePrincipale.checkBox6.Checked = true; Var.fenetrePrincipale.trackBar1.Value = int.Parse(maxmem); Var.fenetrePrincipale.textBox5.Text = maxmem; }
                fichierProfilXML.ReadToFollowing("cpuCount");
                string cpucount = fichierProfilXML.ReadString();
                if (cpucount != "") { Var.fenetrePrincipale.checkBox7.Checked = true; Var.fenetrePrincipale.trackBar2.Value = int.Parse(cpucount); Var.fenetrePrincipale.textBox6.Text = cpucount; }
                fichierProfilXML.ReadToFollowing("noCB");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox8.Checked = true; }
                fichierProfilXML.ReadToFollowing("minimize");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox19.Checked = true; }
                fichierProfilXML.ReadToFollowing("noFilePatching");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox23.Checked = true; }
                fichierProfilXML.ReadToFollowing("VideomaxMem");
                string Videomaxmem = fichierProfilXML.ReadString();
                if (Videomaxmem != "") { Var.fenetrePrincipale.checkBox22.Checked = true; Var.fenetrePrincipale.trackBar3.Value = int.Parse(Videomaxmem); Var.fenetrePrincipale.textBox20.Text = Videomaxmem; }
                fichierProfilXML.ReadToFollowing("threadMax");
                string threadMax = fichierProfilXML.ReadString();
                if (threadMax != "") { Var.fenetrePrincipale.checkBox21.Checked = true; Var.fenetrePrincipale.comboBox3.SelectedIndex = int.Parse(threadMax); }
                fichierProfilXML.ReadToFollowing("adminmode");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox24.Checked = true; }
                fichierProfilXML.ReadToFollowing("nologs");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox10.Checked = true; }
                fichierProfilXML.ReadToFollowing("customCMDLine");
                string customCMDLine = fichierProfilXML.ReadString();
                if (customCMDLine != "") { Var.fenetrePrincipale.checkBox11.Checked = true; Var.fenetrePrincipale.textBox22.Text = customCMDLine; } else { Var.fenetrePrincipale.checkBox11.Checked = false; Var.fenetrePrincipale.textBox22.Text = "";  }
                fichierProfilXML.ReadToFollowing("enableHT");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox_enableHT.Checked = true; }
                fichierProfilXML.ReadToFollowing("toutesApparences");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox_ToutesApparences.Checked = true; }
                fichierProfilXML.ReadToFollowing("arma364bit");
                if (fichierProfilXML.ReadString() == "true") { Var.fenetrePrincipale.checkBox_Arma364bit.Checked = true; }

            fichierProfilXML.Close();
        }
        static public void AjouteComboNomProfil(int index, string nomProfil)
        {
            ComboboxItem item = new ComboboxItem();
            string textAffiche = nomProfil;
            if (nomProfil == "defaut")
            {
                string langue = Core.GetKeyValue(@"Software\Clan GOS\GOS Server A3\", "langage");
                switch (langue)
                {
                    case "en-US":
                        textAffiche = "default";
                        break;
                    case "ru-RU":
                        textAffiche = "умолчание";
                        break;
                    case "de-DE":
                        textAffiche = "Vorgabe";
                        break;
                    case "el-GR":
                        textAffiche = "Προεπιλογή";
                        break;
                    default:
                        textAffiche = "defaut";
                        break;
                }

            }

            item.Text = textAffiche;
            item.Value = nomProfil;
            Var.fenetrePrincipale.comboBox4.Items.Add(item);

        }
        static public void AjouteListeBoxNomProfil(int index, string nomProfil)
        {
            ComboboxItem item = new ComboboxItem();
            string textAffiche = nomProfil;
            if (nomProfil == "defaut")
            {
                string langue = Core.GetKeyValue(@"Software\Clan GOS\GOS Server A3\", "langage");
                switch (langue)
                {
                    case "en-US":
                        textAffiche = "default";
                        break;
                    case "ru-RU":
                        textAffiche = "умолчание";
                        break;
                    case "de-DE":
                        textAffiche = "Vorgabe";
                        break;
                    case "el-GR":
                        textAffiche = "Προεπιλογή";
                        break;
                    default:
                        textAffiche = "defaut";
                        break;
                }

            }

            item.Text = textAffiche;
            item.Value = nomProfil;
            Var.fenetrePrincipale.listBox1.Items.Add(item);
        }
        // GESTION Lock
        static public void UnlockGOSServer()
        {
            Form dialogue = new DIAL_Unlock();
            dialogue.ShowDialog();
            Application.Restart();
        }
        static public void ActualiseValeurAffinity()
        {
            int valeur = 0;
            if (Var.fenetrePrincipale.checkBox_proc0.Checked) { valeur += 1; };
            if (Var.fenetrePrincipale.checkBox_proc1.Checked) { valeur += 2; };
            if (Var.fenetrePrincipale.checkBox_proc2.Checked) { valeur += 4; };
            if (Var.fenetrePrincipale.checkBox_proc3.Checked) { valeur += 8; };
            if (Var.fenetrePrincipale.checkBox_proc4.Checked) { valeur += 16; };
            if (Var.fenetrePrincipale.checkBox_proc5.Checked) { valeur += 32; };
            if (Var.fenetrePrincipale.checkBox_proc6.Checked) { valeur += 64; };
            if (Var.fenetrePrincipale.checkBox_proc7.Checked) { valeur += 128; };
            Var.fenetrePrincipale.textBox_ValueAffinity.Text = valeur.ToString("X");
        }



    }
}
