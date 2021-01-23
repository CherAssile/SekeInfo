using FinaleProjetSeke;
using FinaleProjetSeke.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinaleProjetSeke
{
    public class ListeTotale
    {


        private IList<Produit> Prod;
        private IList<Vente> Ven;
        private IList<Utilisateur> uti;
        private IList<location> Loc;
        private IList<Panier> Pan;
        private IList<PanierLoc> Pan2;


        public ListeTotale()
        {
            this.Prod = null;
            this.Ven = null;
            this.uti = null;
            this.Loc = null;
            this.Pan = null;

        }

        public IList<Produit> GetProduits()
        {
            return Prod;
        }
        public IList<PanierLoc> GetPanLoc()
        {
            return Pan2;
        }
        public IList<Panier> GetPaniers()
        {
            return Pan;
        }

        public IList<location> GetLocations()
        {
            return Loc;
        }

        public IList<Utilisateur> GetUtiliateurs()
        {
            return uti;
        }
        public IList<Vente> GetVentes()
        {
            return Ven;
        }
        public void setProduit(List<Produit> Prod)
        {
            this.Prod = Prod;
        }
        public void setVente(List<Vente> Ven)
        {
            this.Ven = Ven;
        }
        public void setUtilisateur(List<Utilisateur> Uti)
        {
            this.uti = Uti;
        }
        public void setLocation(List<location> Loc)
        {
            this.Loc = Loc;
        }
        public void setPanier(List<Panier>Pan)
        {
            this.Pan = Pan;
        }
        public void setPanLoc(List<PanierLoc> Pan2)
        {
            this.Pan2 = Pan2;
        }
    }
}
    