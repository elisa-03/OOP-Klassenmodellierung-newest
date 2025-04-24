using System.ComponentModel.DataAnnotations;

namespace OOP_Klassenmodellierung_newest
{
    //Test etwas zu commiten ... halloo
    internal class Postamt
    {
        public int Kapazität { get; private set; }
        
        public string Name { get; }
        public string Adress { get; }

        public Postfach[] postfächerDesAmtes;

        public Postamt(string name, string adress, int kapazität)
        {
            Name = name;
            Adress = adress;
            Kapazität = kapazität;
            postfächerDesAmtes = new Postfach[kapazität];
        }

        public Postfach Mieten(Firma firma)
        {
            for(int i=0; i<postfächerDesAmtes.Length; i++)
            {
                if (postfächerDesAmtes[i]==null|| postfächerDesAmtes[i].Firma==null)
                {
                    Postfach neuesPostfach = new(firma,this);    //neues Objekt Postfach wird erstellt

                    postfächerDesAmtes[i] = neuesPostfach;  //Postfach in interne Liste des Postamtes aufnehmen

                    return neuesPostfach;
                }
            }
            return null;    // kein postfach verfügbar, also wird keins zurückgegeben und platz in array der firma bleibt null
        }

        public bool Kündigen(Firma firma)
        {
            for (int i=0; i<postfächerDesAmtes.Length;i++)
            {
                if (postfächerDesAmtes[i].Firma==firma)
                {
                    
                    postfächerDesAmtes[i].Firma = null;
                    return true;

                }
            }
            return false;
        }
        public void PostfächerBefüllen()    //alle postfächer eines Amtes werden befüllt (Keine optimale Lösung!)
        {
            for(int i=0; i<postfächerDesAmtes.Length; i++)
            {
                postfächerDesAmtes[i].PostfachVoll = true;

            }
        }
        public override string ToString()
        {
            return $"Das Postamt {Name} in {Adress} hat Kapazität für {Kapazität} Postfächer.";
        }
    }
    internal class Postfach
    {
        public Postamt? Postamt { get; set; }
        private static int postfachnummer = 1;
        public bool PostfachVoll { get; set; }
        public int Postfachnummer { get; }
        public Firma? Firma { get; set; }
        static public int zähler = 0;

        public Postfach(Firma firma, Postamt posti)
        {
            Postfachnummer = postfachnummer++;
            Firma = firma;
            Postamt = posti;
            

        }
        public override string ToString()
        {
            return $"Es gibt {postfachnummer} Postfächer, davon sind {zähler} Postfächer vermietet.";
            
        }

    }
    internal class Firma
    {
        private Postfach[] gemietetePostfächer;
        public string Name { get; }
        public string Adress { get; }
        private int anzahlPostfächer = 0;
        
        public Firma(string name, string adress)
        {
            Name = name;
            Adress = adress;
            gemietetePostfächer = new Postfach[10]; //statisch festgelegt, dass keine Firma mehr als 10 Postfächer haben darf,
                                                    //mir fällt keine bessere Methode ein...mit variabel großen arrays funktionierts nicht
        }
        
        public bool PostfachAnmieten(Postamt amt)   // es besteht die Möglichkeit, bei verschiedenen Postämtern Postfächer zu mieten
        {

            for (int i=0; i<gemietetePostfächer.Length;i++)
            {
                if (gemietetePostfächer[i] == null)
                {
                    Postfach neues = amt.Mieten(this);  // Mieten geht nur übers Postamt, deswegen entsprechender Aufruf.
                    if (neues!=null)                    // Prüfen ob übergebenes postfach eines ist, da so implementiert
                                                        // dass bei nicht erfolg postfach mieten im Amt null zurückgegeben wird
                    {
                        gemietetePostfächer[i] = neues;
                        anzahlPostfächer++;
                        Postfach.zähler++;
                        return true;
                    }
                      
                      
                }
            }
            return false;
        }
        public bool PostfachKündigen(Postamt Amt)
        {
            for (int i=0;i<gemietetePostfächer.Length;i++)
            {
                if (gemietetePostfächer[i]!=null)
                {
                    Amt.Kündigen(this);
                    gemietetePostfächer[i] = null;
                    anzahlPostfächer--;
                    Postfach.zähler--;
                    return true;

                }
            }
            return false;
        }

        public void PostfachLeeren()
        {
            // deine Implementierung von Briefen
            // Zugriff aufs Postfach über gemietetePostfächer, da ja alle geleert werden sollen 
            // fehlt: wer die Postfächer befüllt, besser möglích über deine Lösung
            // Meine Lösung mit bool werten: true= Postfach voll, false= Postfach leer
            int postfächerGeleert = 0;
            for(int i=0; i<gemietetePostfächer.Length; i++)
            {
                if (gemietetePostfächer[i].PostfachVoll==true)
                {
                    gemietetePostfächer[i].PostfachVoll = false;
                    postfächerGeleert++;
                }
            }
            Console.WriteLine($"Die Firma {this.Name} hat {postfächerGeleert} Postfächer geleert.");

        }
        
        
        public override string ToString()
        {
            return $"Die Firma {Name} in {Adress} hat {anzahlPostfächer} Postfächer gemietet.";


        }

    }
    class Briefträger
    {
        public Postamt Bereich { get; set; }    // Briegträger ist einem Postamt zugeteilt, leert nur dessen Postfächer
        public int Dienstnummer { get; }
        private int idNummer = 1;
        public string Name { get; }
        

        public Briefträger(string name,Postamt bereich)
        {
            Dienstnummer = idNummer++;
            Bereich = bereich;
            Name = name;
        }
        public void PostZustellen() //leert die Postfächer
        {
            for (int i=0; i<Bereich.postfächerDesAmtes.Length;i++)  //leert alle Postfächer seines zugeteilten Amtes (Eigenschaft des Briefträgers!)
            {
                if (Bereich.postfächerDesAmtes[i].PostfachVoll == true)
                {
                    Bereich.postfächerDesAmtes[i].PostfachVoll = false;
                    

                }
                
            }

            Console.WriteLine($"Der Briefträger {this.Name} hat alle Postfächer des Postamtes {Bereich.Name} geleert.");
        }
        public override string ToString()
        {
            return $"Briefträger {Name} mit Dienstnummer {Dienstnummer} arbeitet für das Postamt {Bereich.Name}";
        }

    }







    internal class Program
    {
        static void Main(string[] args)
        {
            Postamt nürnberg = new("Postamt Nürnberg", "Hbf", 10);
            Firma lego = new("LEGO", "Hamburg");
            lego.PostfachAnmieten(nürnberg);    // bsp: firma lego will beim postamt nürnberg ein postfach anmieten
        }
            
    }
}
// Klasse Briefträger und einige Funktionen bzgl. Postfach leeren/füllen etc. müssen wahrscheinlich komplett überarbeitet werden durch dann neue Klasse Brief
// Habe noch ein paar override ToString() Methoden, wenn auch nicht nötig aber vlt indirekt verlangt   