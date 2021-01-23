using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinaleProjetSeke
{
    public class PanierLoc
    {

        private String NomProd;
        private String CodeProduit;
        private String photoProd;
        private Double PrixLoc;
        private int QuantiteProd;
        private double PrixTotale;
        private String DescProd;
        private String CodeLoc;
        private int? QuantiteStock;
        private String DateLoc;
        private String DateRetour;
        private string Code;

        public PanierLoc(string nomProd,string CodeLoc,string codeProduit, string photoProd, double prixLoc, int quantiteProd, double prixTotale, string descProd, int? quantiteStock, string dateLoc)
        {
            NomProd1 = nomProd;
            CodeProduit1 = codeProduit;
            this.PhotoProd = photoProd;
            PrixLoc1 = prixLoc;
            QuantiteProd1 = quantiteProd;
            PrixTotale1 = prixTotale;
            DescProd1 = descProd;
            CodeLoc1 = CodeLoc;
            QuantiteStock1 = quantiteStock;
            DateLoc1 = dateLoc;
        }

        public PanierLoc(string nomProd, string CodeLoc, string codeProduit, string photoProd, double prixLoc, int quantiteProd, double prixTotale, string descProd, int? quantiteStock, string dateLoc, string dateRetour,string Code)
        {
            NomProd1 = nomProd;
            CodeProduit1 = codeProduit;
            this.PhotoProd = photoProd;
            PrixLoc1 = prixLoc;
            QuantiteProd1 = quantiteProd;
            PrixTotale1 = prixTotale;
            DescProd1 = descProd;
            CodeLoc1 = CodeLoc;
            QuantiteStock1 = quantiteStock;
            DateLoc1 = dateLoc;
            DateRetour1 = dateRetour;
            this.Code1 = Code;
        }

        public string NomProd1 { get => NomProd; set => NomProd = value; }
        public string CodeProduit1 { get => CodeProduit; set => CodeProduit = value; }
        public string PhotoProd { get => photoProd; set => photoProd = value; }
        public double PrixLoc1 { get => PrixLoc; set => PrixLoc = value; }
        public int QuantiteProd1 { get => QuantiteProd; set => QuantiteProd = value; }
        public double PrixTotale1 { get => PrixTotale; set => PrixTotale = value; }
        public string DescProd1 { get => DescProd; set => DescProd = value; }
        public string CodeLoc1 { get => CodeLoc; set => CodeLoc = value; }
        public int? QuantiteStock1 { get => QuantiteStock; set => QuantiteStock = value; }
        public string DateLoc1 { get => DateLoc; set => DateLoc = value; }
        public string DateRetour1 { get => DateRetour; set => DateRetour = value; }
        public string Code1 { get => Code; set => Code = value; }
    }
}