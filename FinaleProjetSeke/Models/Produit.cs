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
    
    public partial class Produit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produit()
        {
            this.locations = new HashSet<location>();
            this.Reservations = new HashSet<Reservation>();
            this.Ventes = new HashSet<Vente>();
        }
    
        public string CodeProduit { get; set; }
        public string Nom { get; set; }
        public Nullable<double> PrixAchat { get; set; }
        public Nullable<int> QuantiteStock { get; set; }
        public string Photo { get; set; }
        public Nullable<decimal> Note { get; set; }
        public Nullable<decimal> RabaisVente { get; set; }
        public Nullable<decimal> RabaisLocation { get; set; }
        public Nullable<System.DateTime> DateSortie { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<location> locations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vente> Ventes { get; set; }
    }
}
