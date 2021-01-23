//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinaleProjetSeke.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vente
    {
        public string CodeProduit { get; set; }
        public string Code { get; set; }
        public System.DateTime DateVente { get; set; }
        public string CodeEmployes { get; set; }
        public Nullable<double> PrixVente { get; set; }
        public Nullable<int> QuantiteVente { get; set; }
        public Nullable<decimal> Rabais { get; set; }
    
        public virtual Employe Employe { get; set; }
        public virtual Produit Produit { get; set; }
        public virtual Utilisateur Utilisateur { get; set; }
    }
}