using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinaleProjetSeke
{
    public class Panier
    {
        private String NomProd;
        private String CodeProduit;
        private String photoProd;
        private Double PrixVente;
        private int QuantiteProd;
        private double PrixTotale;
        private String DescProd;
        private String idVente;
        private int ?QuantiteStock;
        private String DateAchat;
        private int ?StatutP;
        private String Code;

        public Panier( String NomProd,String idVente,string codeProduit, string photoProd, double prixVente, int quantiteProd, string descProd, int? QuantiteStock, String DateAchat,int ?StatutP)
        {
           this.idVente = idVente;
           this.CodeProduit = codeProduit;
            this.photoProd = photoProd;
            this.PrixVente = prixVente;
            this.QuantiteProd = quantiteProd;
            this.DescProd = descProd;
            this.QuantiteStock = QuantiteStock;
            this.DateAchat = DateAchat;
            this.NomProd1 = NomProd;
            this.StatutP1 = StatutP;
           
        }

        public Panier(String NomProd,String idVente,string codeProduit, string photoProd, double prixVente,int QuantiteProd,double PrixTotale,String DescProd, int? QuantiteStock,String DateAchat,int ? StatutP, String Code)
        {

            this.idVente = idVente;
            this.CodeProduit = codeProduit;
            this.PhotoProd = photoProd;
            this.PrixVente1 = prixVente;
            this.QuantiteProd = QuantiteProd;
            this.PrixTotale = PrixTotale;
            this.DescProd1 = DescProd;
            this.QuantiteStock1 = QuantiteStock;
            this.DateAchat1 = DateAchat;
            this.NomProd1 = NomProd;
            this.StatutP1 = StatutP;
            this.Code1 = Code;

        }
        public Panier(String NomProd, String idVente, string codeProduit, string photoProd, double prixVente, int QuantiteProd, double PrixTotale, String DescProd, int? QuantiteStock, String DateAchat)
        {

            this.idVente = idVente;
            this.CodeProduit = codeProduit;
            this.PhotoProd = photoProd;
            this.PrixVente1 = prixVente;
            this.QuantiteProd = QuantiteProd;
            this.PrixTotale = PrixTotale;
            this.DescProd1 = DescProd;
            this.QuantiteStock1 = QuantiteStock;
            this.DateAchat1 = DateAchat;
            this.NomProd1 = NomProd;
        }

        public string CodeProduit1 { get => CodeProduit; set => CodeProduit = value; }
        public string PhotoProd { get => photoProd; set => photoProd = value; }
        public double PrixVente1 { get => PrixVente; set => PrixVente = value; }
        public int QuantiteProd1 { get => QuantiteProd; set => QuantiteProd = value; }
        public double PrixTotale1 { get => PrixTotale; set => PrixTotale = value; }
        public string DescProd1 { get => DescProd; set => DescProd = value; }
        public String IdVente1 { get => idVente; set => idVente = value; }
        public int? QuantiteStock1 { get => QuantiteStock; set => QuantiteStock = value; }
        public String DateAchat1 { get => DateAchat; set => DateAchat = value; }
        public string NomProd1 { get => NomProd; set => NomProd = value; }
        public int? StatutP1 { get => StatutP; set => StatutP = value; }
        public string Code1 { get => Code; set => Code = value; }

        public double CalculePrixTotale(int QuantiteProd,Double PrixProd)
        {
            Double prixTotale=QuantiteProd*PrixProd;
            return prixTotale; 
        }

        public override bool Equals(object obj)
        {
            return obj is Panier panier &&
                   idVente == panier.idVente;
        }
    }
}