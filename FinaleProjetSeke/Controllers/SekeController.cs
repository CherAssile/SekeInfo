using FinaleProjetSeke.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinaleProjetSeke.Controllers
{
    public class SekeController : Controller
    {
        // GET: Seke

        DataClasses1DataContext db = new DataClasses1DataContext();

        //************* Fonctionnalite Membres  *****************//


        public ActionResult IndexMembre()
        {
            {
                if (Session["username"] == null)
                {
                    return RedirectToAction("Login", "Seke");
                }
                else
                {
                    ViewBag.user = Session["username"];




                    DataClasses1DataContext db = new DataClasses1DataContext();
                    var Produitsdetails = from x in db.Produit select x;
                    ViewData["ProduitTotale"] = Produitsdetails.ToList();

                    var ProduitsVedette = from x in db.Produit orderby x.RabaisVente descending select x;
                    var TopProduitVedette = Produitsdetails.Take(3);

                    ViewData["ProduitVedette"] = TopProduitVedette.ToList();
                    //Produit par Nombre de Vente 


                    //Produit Nouveaute
                    var ProduitsNew = from x in db.Produit orderby x.DateSortie descending select x;
                    var TopProduitNew = ProduitsNew.Take(3);
                    ViewData["ProduitNew"] = TopProduitNew.ToList();
                    //Plus bas Prix

                    var ProduitsBasPrix = from x in db.Produit orderby x.DateSortie descending select x;
                    var TopBasPrix = ProduitsBasPrix.Take(1);
                    ViewData["ProduitbasPrix"] = TopBasPrix.ToList();
                    //Top Vente
                    //var ProduitsBestVente = from x in db.Produit orderby x.DateSortie descending select x;
                    // var TopVente = ProduitsBestVente.Take(1);
                    var ProduitsBestVente = (from x in db.Produit join y in db.Vente on x.CodeProduit equals y.CodeProduit orderby y.CodeProduit descending select x);
                    var TopVente = ProduitsBestVente.Take(3);
                    String Code = (string)HttpContext.Session["username"];

                    var Ventedetails1 = from x in db.Vente where x.Code == Code && x.StatutP == 0 select x;
                    var Ventedetails2 = from x in db.location where x.Code == Code select x;

                    ViewData["NbrPanier"] = Ventedetails1.ToList();
                    ViewData["NbrPanier2"] = Ventedetails2.ToList();
                    ViewData["ProduitTopVente"] = TopVente.ToList();

                    return View();
                }
            }
        }
        //************************************


        public ActionResult ProfilMembre()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                ViewBag.user = Session["username"];
                ViewBag.a = Session["nom"];
            }

            return View();
        }
        public ActionResult NewProfilMembre()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }

            else
            {
                ViewBag.user = Session["username"];
                ViewBag.a = Session["nom"];
            }

            return View();
        }
        [HttpPost]
        public ActionResult NewProfilMembre(String Code, Utilisateur collect)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {

                Admin emp = new Admin();
                var Update = (from x in db.Utilisateurs where x.Code == Code select x);
                //Employe emp = db.Employes.Single(x => x.CodeEmployes == CodeEmployes);
                foreach (Utilisateur empl in Update)
                {
                    empl.Nom = collect.Nom;
                    empl.Prenom = collect.Prenom;
                    empl.Rue = collect.Rue;
                    empl.password = collect.password;
                }
                ViewBag.message = "Votre Profil a ete mis a jour correctement, Ce Changement sera appliquer a partir de la nouvelle Connexion!";
                ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                //db.SaveChanges(Update);
                db.SubmitChanges();
                return View();
                //}
            }
            catch
            {
                //ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                return View();
            }
        }

        public ActionResult Blok()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }

            else
            {
                ViewBag.user = Session["username"];
                ViewBag.a = Session["nom"];
            }

            return View();
        }
        [HttpPost]
        public ActionResult blok(String Code, Utilisateur collect)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {


                var Update = (from x in db.Utilisateurs where x.Code == Code select x);
                //Employe emp = db.Employes.Single(x => x.CodeEmployes == CodeEmployes);
                foreach (Utilisateur empl in Update)
                {
                    empl.Statut = 4;
                }
                ViewBag.message = "Votre Compte a ete bloque, Vous ne pourrez plus vous connecter! ";

                db.SubmitChanges();
                return View();
                //}
            }
            catch
            {
                //ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                return View();
            }
        }
        //****************************************************************************//
        public ActionResult MesAchats()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            var mesachats = from y in db.Vente select y;
            List<Vente> list = mesachats.ToList();
            ViewBag.UtilisateurList = list;
            return View(mesachats);
        }
        public ActionResult suivireservationMem()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            // DateTime a = Convert.ToDateTime("2020-04-25 20:09:52");
            var myoldprode = from y in db.Reservations select y;
            List<Reservation> list = myoldprode.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprode);
        }

        public ActionResult GetSearchRecordMem(string SearchText)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            List<Reservation> list = new List<Reservation>();
            var query = from z in db.Reservations where z.NomProduit.Contains(SearchText) || z.NumeroR.Contains(SearchText) select z;
            //var query = from db.Utilisateurs.Where(x => x.Nom.Contains(SearchText) || x.Email.Contains(SearchText)).Select(x => new Utilisateur { Nom = x.Nom, Email = x.Email }).ToList();

            foreach (var z in query)
            {
                list.Add(z);
            }

            if (list.Count.Equals(0))
            {
                return PartialView("View", null);
            }
            else
            {
                return PartialView("View", list);
            }

        }
        //***********************************************************//


        public ActionResult MesFac()
        {
            location l = new location();
            DataClasses1DataContext db = new DataClasses1DataContext();
            // r.Montant = r.QuantiteReserve * r.PrixReserve;
            //  ViewBag.Montant = r.Montant;

            var meslocation = from x in db.Vente select x;
            List<Vente> list = meslocation.ToList();
            ViewBag.reservationlist = list;

            //ViewData["utilsateurtotal"] = utilisateur.ToList();
            return View();
        }




        // ******************* fin fonctionnalite membres ************//





















        //*********************** Fonctionnalite Employes ***************************//

        public ActionResult IndexEmployes()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                if (Session["username"] == null)
                {
                    return RedirectToAction("Login", "Seke");
                }
                else
                {
                    ViewBag.user = Session["username"];




                    DataClasses1DataContext db = new DataClasses1DataContext();
                    var Produitsdetails = from x in db.Produit select x;
                    ViewData["ProduitTotale"] = Produitsdetails.ToList();

                    var ProduitsVedette = from x in db.Produit orderby x.RabaisVente descending select x;
                    var TopProduitVedette = Produitsdetails.Take(3);

                    ViewData["ProduitVedette"] = TopProduitVedette.ToList();
                    //Produit par Nombre de Vente 


                    //Produit Nouveaute
                    var ProduitsNew = from x in db.Produit orderby x.DateSortie descending select x;
                    var TopProduitNew = ProduitsNew.Take(3);
                    ViewData["ProduitNew"] = TopProduitNew.ToList();
                    //Plus bas Prix

                    var ProduitsBasPrix = from x in db.Produit orderby x.DateSortie descending select x;
                    var TopBasPrix = ProduitsBasPrix.Take(1);
                    ViewData["ProduitbasPrix"] = TopBasPrix.ToList();
                    //Top Vente
                    //var ProduitsBestVente = from x in db.Produit orderby x.DateSortie descending select x;
                    // var TopVente = ProduitsBestVente.Take(1);
                    var ProduitsBestVente = (from x in db.Produit join y in db.Vente on x.CodeProduit equals y.CodeProduit orderby y.CodeProduit descending select x);
                    var TopVente = ProduitsBestVente.Take(3);
                    ViewData["ProduitTopVente"] = TopVente.ToList();
                    String Code = (string)HttpContext.Session["username"];
                    var Ventedetails = from x in db.Vente where x.Code == Code select x;
                    ViewData["NbrPanier"] = Ventedetails.ToList();
                    return View();
                }
            }

        }
        //***********************fin indexEmployes ************////////////////////////////////////////


        [HttpGet]
        public ActionResult AjoutProduit2()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                ViewBag.user = Session["username"];
            }

            return View();
        }

        [HttpPost]
        public ActionResult AjoutProduit2(Produit p, String PrixAchat, String Nom, HttpPostedFileBase Photo)
        {
            Random r = new Random();
            int random = r.Next();
            string path = uploadimage(Photo);
            for (int i = 1; i < 1000; i++) ;

            if (path.Equals("-1"))
            {

            }

            else
            {
                if (PrixAchat == "" || Nom == "")
                {
                    ViewBag.Message2 = "Assurez-vous que tous les champs sont remplis ";
                }
                else
                {
                    p.CodeProduit = "PROD" + p.Nom.Substring(0, 3) + random;
                    p.PrixAncien = p.PrixAchat + 10;
                    p.Note = 5;
                    p.DateSortie = @DateTime.Now;
                    p.Photo = path;
                    db.Produit.InsertOnSubmit(p);
                    db.SubmitChanges();
                    ViewBag.Message = "Le Produit a ete Ajouté : ";
                    ViewBag.Message1 = "Vous Pouvez Plus Modifier ou Supprimer Ce Produit! ";
                    ViewBag.code = p.CodeProduit;
                    ViewBag.nom = p.Nom;
                    ViewBag.prix = p.PrixAchat;
                    ViewBag.date = p.DateSortie;

                }
            }
            return View();
        }
        //***********************fin ajout produit*************************//
        public ActionResult Inscription2()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Inscription2(Utilisateur p, String Nom, String Prenom, String Rue, String Ville, HttpPostedFileBase Photo)
        {
            Random r = new Random();
            int random = r.Next();
            string path = uploadimage(Photo);
            for (int i = 1; i < 1000; i++) ;

            if (path.Equals("-1"))
            {

            }
            else
            {
                if (Prenom == "" || Nom == "" || Rue == "" || Ville == "")
                {
                    ViewBag.Message2 = "Assurez-vous que tous les champs sont remplis ";
                }
                else
                {
                    p.Code = "Mem" + p.Nom.Substring(0, 3) + p.Prenom.Substring(0, 3);
                    p.password = "Utilisa001";
                    p.Photo = path;
                    db.Utilisateurs.InsertOnSubmit(p);
                    db.SubmitChanges();
                    ViewBag.Message = "Le Membre a été inscrit avec succes : ";

                    ViewBag.code = p.Code;
                    ViewBag.nom = p.Nom;
                    ViewBag.prix = p.Rue;
                    ViewBag.date = p.Ville;

                }
            }
            return View();
        }
        //************************************//
        public ActionResult suivilocation2()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            // DateTime a = Convert.ToDateTime("2020-04-25 20:09:52");
            var myoldprode = from y in db.location select y;
            List<location> list = myoldprode.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprode);
        }
        public ActionResult GetSearchRecord62(string SearchText)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            List<location> list = new List<location>();
            var query = from x in db.location where x.NomProduit.Contains(SearchText) || x.CodeLocation.Contains(SearchText) select x;
            //var query = from db.Utilisateurs.Where(x => x.Nom.Contains(SearchText) || x.Email.Contains(SearchText)).Select(x => new Utilisateur { Nom = x.Nom, Email = x.Email }).ToList();

            foreach (var x in query)
            {
                list.Add(x);
            }

            if (list.Count.Equals(0))
            {
                return PartialView("SearchPartial22", null);
            }
            else
            {
                return PartialView("SearchPartial22", list);
            }

        }
        //***************************************************************//
        public ActionResult reservation2()
        {
            return View();
        }
        [HttpPost]
        public ActionResult reservation2(Reservation R, String Nom)
        {
            Random r = new Random();
            if (R.DateRecuperation > R.DateRetour)
            {

                ViewBag.date = "Les Dates ne sont pas correctes!";
            }
            else if (R.Montant < 250)
            {
                ViewBag.error = "N'est Pas Une Personne de Confiance! ";
            }
            else
            {
                int random = r.Next();
                R.NumeroR = "Reser" + R.Nom + R.NomProduit + random;
                R.NumeroCard = "********87451";
                R.DateReservation = @DateTime.Now;

                R.Rabais = 35;
                R.Montant = R.QuantiteReserve * R.PrixReserve - R.Rabais;
                R.Taxe = R.Montant * 15 / 100;
                R.Ttc = R.Montant + R.Taxe;
                db.Reservations.InsertOnSubmit(R);
                db.SubmitChanges();
                ViewBag.Message = "Votre Produit a ete bien reserve! Vous Pouvez le recuperer le " + R.DateRecuperation + " et le retourner le " + R.DateRetour + "! Suivez egalement votre produit avec ce numero " + R.NumeroR;
                ViewBag.att = "Attention! si le produit n'est pas retourne a sa date, une penalite de 3% serait applique au prix total par jour!";
            }
            return View();

        }

        //*********************************************************//
        public ActionResult suivireservation2()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            // DateTime a = Convert.ToDateTime("2020-04-25 20:09:52");
            var myoldprode = from y in db.Reservations select y;
            List<Reservation> list = myoldprode.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprode);
        }


        //******************************************************//






        public ActionResult ListeUtilisateur2()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var mesutilisateur = from x in db.Utilisateurs select x;
            List<Utilisateur> list = mesutilisateur.ToList();
            ViewBag.UtilisateurList = list;
            //ViewData["utilsateurtotal"] = utilisateur.ToList();
            return View();
        }

        public ActionResult test2()
        {
            Reservation r = new Reservation();
            DataClasses1DataContext db = new DataClasses1DataContext();
            // r.Montant = r.QuantiteReserve * r.PrixReserve;
            //  ViewBag.Montant = r.Montant;

            var mesreserves = from x in db.Reservations select x;
            List<Reservation> list = mesreserves.ToList();
            ViewBag.reservationlist = list;

            //ViewData["utilsateurtotal"] = utilisateur.ToList();
            return View();

        }
        //***************************************//
        public ActionResult locationfac2()
        {
            location l = new location();
            DataClasses1DataContext db = new DataClasses1DataContext();
            // r.Montant = r.QuantiteReserve * r.PrixReserve;
            //  ViewBag.Montant = r.Montant;

            var meslocation = from x in db.location select x;
            List<location> list = meslocation.ToList();
            ViewBag.locationlist = list;

            //ViewData["utilsateurtotal"] = utilisateur.ToList();
            return View();
        }

        //******************* Fin Fonctionnalite Employes **************************//















        public ActionResult DeleteProduit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteProduit(Produit p, String CodeProduit, String Nom, String MotifSuppression)
        {


            //p.CodeProduit = "PROD" + p.Nom.Substring(0, 3);
            //p.PrixAncien = p.PrixAchat + 10;
            //p.Note = 5;
            //p.DateSortie = @DateTime.Now;
            //// p.Photo = path;
            // var DateSuppression = u.Nom;
            //var nom = u.c;
            var delete = from x in db.Produit where x.CodeProduit == p.CodeProduit select x;

            var y = from c in db.Reservations where c.CodeProduit == p.CodeProduit select c;
            var z = from g in db.location where g.CodeProduit == p.CodeProduit select g;
            int a = y.Count();
            int b = z.Count();

            //if (CodeProduit == "" || Nom == "" || MotifSuppression == "")
            //{
            //    ViewBag.Message4 = "Vous devez remplir tous les champs svp!";
            //}
            if (a == 0 && b == 0)



            {
                db.Produit.DeleteAllOnSubmit(delete);
                int i = db.GetChangeSet().Deletes.Count;
                db.SubmitChanges();
                if (i == 0)
                {
                    ViewBag.msg = "la suppression n'a pas fonctionnee! Verifier le Code! Si le probleme persiste, Appeler La Compagnie!";
                }
                else
                {
                    ViewBag.Message = "Ce Produit n'est plus sur ce site! Vous avez toujours des infos sur ce Produit : ";
                    ViewBag.Message1 = "Vous Pouvez le mettre en ligne plutard ";
                    ViewBag.code = p.CodeProduit;
                    return View();
                }
            }
            else
            {
                //db.Produits.DeleteAllOnSubmit(delete);
                //int i = db.GetChangeSet().Deletes.Count;
                //db.SubmitChanges();
                //if (i == 0)
                //{
                ViewBag.msg = "la suppression n'a pas fonctionnée! Le produit est actuellement en location ou en cours de reservation!";
                //    }
                //    else
                //    {
                //        ViewBag.Message = "Ce Produit n'est plus sur ce site! Vous avez toujours des infos sur ce Produit : ";
                //        ViewBag.Message1 = "Vous Pouvez le mettre en ligne plutard ";
                //        ViewBag.code = p.CodeProduit;
                //        return View();
                //    }
            }

            return View();
        }
        //********************************************//
        //Annuler reservation:
        public ActionResult AnnulerReser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AnnulerReser(Reservation R, String CodeProduit, String NumeroR, String MotifSuppression)
        {
            var delete = from x in db.Reservations where x.CodeProduit == R.CodeProduit || x.NumeroR == R.NumeroR select x;
            db.Reservations.DeleteAllOnSubmit(delete);
            int i = db.GetChangeSet().Deletes.Count;
            db.SubmitChanges();
            if (i == 0)
            {
                ViewBag.msg = "la suppression n'a pas fonctionnee! Verifier le Code ou Votre mot de passe, si le probleme persiste, Appeler La Compagnie!";
            }
            else
            {
                ViewBag.Message = "La reservation est annulée!";
                ViewBag.Message1 = "Le client ne recevra plus ce produit!";
                ViewBag.code = R.CodeProduit;
                return View();



            }
            return View();
        }
        //*********************************************//
        //**********suppression de produit fin *********************************************//

        public ActionResult ProfilAdmin()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                ViewBag.user = Session["username"];
                ViewBag.a = Session["nom"];
            }

            return View();
        }
        public ActionResult NewProfilAdmin()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }

            else
            {
                ViewBag.user = Session["username"];
                ViewBag.a = Session["nom"];
            }

            return View();
        }
        [HttpPost]
        public ActionResult NewProfilAdmin(String Code, Utilisateur collect)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {

                Admin emp = new Admin();
                var Update = (from x in db.Utilisateurs where x.Code == Code select x);
                //Employe emp = db.Employes.Single(x => x.CodeEmployes == CodeEmployes);
                foreach (Utilisateur empl in Update)
                {
                    empl.Nom = collect.Nom;
                    empl.Prenom = collect.Prenom;
                    empl.Rue = collect.Rue;
                    empl.password = collect.password;
                }
                ViewBag.message = "Votre Profil a ete mis a jour correctement, Ce Changement sera appliquer a partir de la nouvelle Connexion!";
                ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                //db.SaveChanges(Update);
                db.SubmitChanges();
                return View();
                //}
            }
            catch
            {
                //ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                return View();
            }
        }

        //****************************************************
        public ActionResult MajProduitProduit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MajProduitProduit(String CodeProduit, String Nom, Produit collect)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {

                Produit emp = new Produit();
                var Update = (from x in db.Produit where x.CodeProduit == CodeProduit select x);
                //Employe emp = db.Employes.Single(x => x.CodeEmployes == CodeEmployes);
                foreach (Produit empl in Update)
                {
                    empl.Nom = collect.Nom;
                    empl.PrixAchat = collect.PrixAchat;
                    empl.QuantiteStock = collect.QuantiteStock;
                }
                ViewBag.message = "les informations du produit ont ete mises a jour correctement! Les changements ne sont pas appliqués aux anciennes factures!";
                // ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                //db.SaveChanges(Update);
                db.SubmitChanges();
                return View();
                //}
            }
            catch
            {
                //ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                return View();
            }
        }
        //*****************************************************
        public ActionResult Inscription()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Inscription(Utilisateur p, String Nom, String Prenom, String Rue, String Ville, HttpPostedFileBase Photo)
        {
            Random r = new Random();
            int random = r.Next();
            string path = uploadimage(Photo);
            for (int i = 1; i < 1000; i++) ;

            if (path.Equals("-1"))
            {

            }
            else
            {
                if (Prenom == "" || Nom == "" || Rue == "" || Ville == "")
                {
                    ViewBag.Message2 = "Assurez-vous que tous les champs sont remplis ";
                }
                else
                {
                    p.Code = "Mem" + p.Nom.Substring(0, 3) + p.Prenom.Substring(0, 3);
                    p.password = "Utilisa001";
                    p.Photo = path;
                    db.Utilisateurs.InsertOnSubmit(p);
                    db.SubmitChanges();
                    ViewBag.Message = "Le Membre a été inscrit avec succes : ";

                    ViewBag.code = p.Code;
                    ViewBag.nom = p.Nom;
                    ViewBag.prix = p.Rue;
                    ViewBag.date = p.Ville;

                }
            }
            return View();
        }

        //****************************************************

        //****************************************************
        public ActionResult MajMembre()
        {

            return View();
        }
        [HttpPost]

        public ActionResult MajMembre(String Code, Utilisateur collect)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {

                Utilisateur emp = new Utilisateur();
                var Update = (from x in db.Utilisateurs where x.Code == Code select x);
                //Employe emp = db.Employes.Single(x => x.CodeEmployes == CodeEmployes);
                foreach (Utilisateur empl in Update)
                {

                    empl.Statut = collect.Statut;
                }
                int i = db.GetChangeSet().Updates.Count;

                if (i == 0)
                {
                    ViewBag.msg = "la Mise a jour n'a pas fonctionnee! Verifier le Code! Si le probleme persiste, Appeler la compagnie";
                }
                else
                {
                    ViewBag.message = "les informations de l'utilisateur ont ete mises a jour correctement!";
                    // ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                    //db.SaveChanges(Update);
                    db.SubmitChanges();
                    return View();
                }

            }
            catch
            {
                //ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                return View();
            }
            return View();
        }


        //                          Fin FonctionnaliteAdministrateur                                                   //
        //***********************************************************************************************
        public ActionResult suivireser()
        {

            return View();
        }
        public ActionResult about()
        {

            return View();
        }

        public ActionResult Apropos()
        {
            return View();
        }
        public ActionResult locationfac()
        {
            location l = new location();
            DataClasses1DataContext db = new DataClasses1DataContext();
            // r.Montant = r.QuantiteReserve * r.PrixReserve;
            //  ViewBag.Montant = r.Montant;

            //$codeloc = $_SESSION["login"];
            var meslocation = from x in db.location select x;
            List<location> list = meslocation.ToList();
            //ViewData["utilsateurtotal"] = utilisateur.ToList();
            return View();
        }

        public ActionResult NouveauProfil()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }

            else
            {
                ViewBag.user = Session["username"];
                ViewBag.a = Session["nom"];
            }

            return View();
        }
        [HttpPost]
        public ActionResult NouveauProfil(String Code, Utilisateur collect)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {

                Employe emp = new Employe();
                var Update = (from x in db.Utilisateurs where x.Code == Code select x);
                //Employe emp = db.Employes.Single(x => x.CodeEmployes == CodeEmployes);
                foreach (Utilisateur empl in Update)
                {
                    empl.Nom = collect.Nom;
                    empl.Prenom = collect.Prenom;
                    empl.Rue = collect.Rue;
                    empl.password = collect.password;
                }
                ViewBag.message = "Votre Profil a ete mis a jour correctement, Ce Changement sera appliquer a partir de la nouvelle Connexion!";
                ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                //db.SaveChanges(Update);
                db.SubmitChanges();
                return View();
                //}
            }
            catch
            {
                //ViewBag.error = "Le Profil n'a pas pu etre mis a jour! Veuillez verifier l'orthographe des mots ou Contacter l'administration!";
                return View();
            }
        }

        public ActionResult Contact()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                ViewBag.user = Session["username"];
                ViewBag.a = Session["nom"];
            }

            return View();
        }
        /// <summary>
        /// ////////////////////////////
        /// </summary>
        /// <returns></returns>
        public ActionResult reservation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult reservation(Reservation R, String Nom)
        {
            Random r = new Random();
            if (R.DateRecuperation > R.DateRetour)
            {

                ViewBag.date = "Les Dates ne sont pas correctes!";
            }
            else if (R.Montant < 250)
            {
                ViewBag.error = "N'est Pas Une Personne de Confiance! ";
            }
            else
            {
                int random = r.Next();
                R.NumeroR = "Reser" + R.Nom + R.NomProduit + random;
                R.NumeroCard = "********87451";
                R.DateReservation = @DateTime.Now;

                R.Rabais = 35;
                R.Montant = R.QuantiteReserve * R.PrixReserve - R.Rabais;
                R.Taxe = R.Montant * 15 / 100;
                R.Ttc = R.Montant + R.Taxe;
                db.Reservations.InsertOnSubmit(R);
                db.SubmitChanges();
                ViewBag.Message = "Votre Produit a ete bien reserve! Vous Pouvez le recuperer le " + R.DateRecuperation + " et le retourner le " + R.DateRetour + "! Suivez egalement votre produit avec ce numero " + R.NumeroR;
                ViewBag.att = "Attention! si le produit n'est pas retourne a sa date, une penalite de 3% serait applique au prix total par jour!";
            }
            return View();

        }
        public ActionResult Location()
        {
            return View();
        }
        [HttpPost]
        public ActionResult location(location R, String Nom)
        {
            Random r = new Random();
            if (R.DateRetour < R.DateRetoureff)
            {
                int random = r.Next();
                R.CodeLocation = "Loc" + R.NomLocataire.Substring(0, 3) + R.NomProduit.Substring(0, 3) + random;
                R.FraisRetard = 35;
                //R.DateRetoureff = @DateTime.Now;
                R.Rabais = 35;
                R.TotalHT = R.QuantiteLocation * R.PrixLocation - R.Rabais + R.FraisRetard;
                R.Taxe = R.TotalHT * 15 / 100;
                R.Ttc = R.TotalHT + R.Taxe;
                db.location.InsertOnSubmit(R);
                db.SubmitChanges();
                //ViewBag.Message = "Votre Produit a ete bien reserve! Vous Pouvez le recuperer le " + "R.DateRecuperation + " et le retourner le " + R.DateRetour + "! Suivez egalement votre produit avec ce numero " + R.NumeroR;
                ViewBag.att = "Attention! si le produit n'est pas retourne a sa date, une penalite de 3% serait applique au prix total par jour!";
            }
            else
            {

                int random = r.Next();
                R.CodeLocation = "Loc" + R.NomLocataire.Substring(0, 3) + R.NomProduit.Substring(0, 3) + random;
                R.FraisRetard = 0;
                R.DateLocation = @DateTime.Now;
                R.Rabais = 35;
                R.TotalHT = R.QuantiteLocation * R.PrixLocation - R.Rabais + R.FraisRetard;
                R.Taxe = R.TotalHT * 15 / 100;
                R.Ttc = R.TotalHT + R.Taxe;
                db.location.InsertOnSubmit(R);
                db.SubmitChanges();
                //ViewBag.Message = "Votre Produit a ete bien reserve! Vous Pouvez le recuperer le " + "R.DateRecuperation + " et le retourner le " + R.DateRetour + "! Suivez egalement votre produit avec ce numero " + R.NumeroR;
                ViewBag.att = "Attention! si le produit n'est pas retourne a sa date, une penalite de 3% serait applique au prix total par jour!";
            }
            return View();

        }

        /// <summary>
        /// /////////////////////////////
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        //// Fonction Login Pour Membre ET Employes
        public ActionResult Login(string user, string mdp)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            Utilisateur u = new Utilisateur();
            Employe P = new Employe();
            int i = 3;

            //si c'estun utilisateur/membre
            // if (mdp = "" && user = ""){

            int? statut = 0;
            if (user != null)
            {

                //var userconnecte = db.Utilisateurs.SingleOrDefault(data => data.Code == user && data.password == mdp);
                var userconnecte = from x in db.Utilisateurs where x.Code == user && x.password == mdp select x;
                if (userconnecte != null)
                {
                    var ListStatutCode = (from x in db.Utilisateurs where x.Code == user select x.Statut).First();

                    statut = ListStatutCode;


                    if (statut == 3)
                    {




                        ViewBag.message = "Connecte!";
                        ViewBag.tentative = "ok";

                        foreach (Utilisateur p in userconnecte)
                        {
                            Session["login"] = p;
                        }

                        Session["username"] = user;


                        return RedirectToAction("IndexMembre", "Seke", new { username = user, nom = u.Nom });


                    }
                    //si c'est u employes
                    else if (statut == 2)
                    {


                        ViewBag.message = "Connecte!";
                        ViewBag.tentative = "ok";

                        foreach (Utilisateur p in userconnecte)
                        {
                            Session["login"] = p;
                        }

                        Session["username"] = user;


                        return RedirectToAction("IndexEmployes", "Seke", new { username = user, nom = u.Nom });


                    }
                    else if (statut == 1)
                    {

                        ViewBag.message = "Connecte!";
                        ViewBag.tentative = "ok";

                        foreach (Utilisateur p in userconnecte)
                        {
                            Session["login"] = p;
                        }

                        Session["username"] = user;


                        return RedirectToAction("IndexAdmin", "Seke", new { username = user, nom = u.Nom });


                    }

                    else
                    {
                        ViewBag.message = "Connecte!";
                        ViewBag.tentative = "ok";

                        foreach (Utilisateur p in userconnecte)
                        {
                            Session["login"] = p;
                        }

                        Session["username"] = user;


                        return RedirectToAction("IndexMem", "Seke", new { username = user, nom = u.Nom });


                    }
                }
                else
                {

                    i--;
                    ViewBag.i = i;
                    ViewBag.tentative = "ok";
                    return View();

                }

            }
            else
            {
                return View();
            }


        }
        // FIN FONCTIONNALITE LOGIN
        //FONCTIONNALITE -----------------------------EMPLOYES
        // MIS A JOUR DU PRODUIT POUR EMPLOYES
        public ActionResult UpdateProduit()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                ViewBag.user = Session["username"];
            }
            return View();
        }
        // FIN UPDATE
        //------------------------------------//
        //suppresiion

        public ActionResult SupprimeProduit()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                ViewBag.user = Session["username"];



            }
            return View();
        }


        // FIN SUPPRESSION


        // AJOUT PRODUIT
        [HttpGet]
        public ActionResult AjoutProduit()
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                ViewBag.user = Session["username"];
            }

            return View();
        }

        [HttpPost]
        public ActionResult AjoutProduit(Produit p, String PrixAchat, String Nom, HttpPostedFileBase Photo)
        {
            Random r = new Random();
            int random = r.Next();
            string path = uploadimage(Photo);
            for (int i = 1; i < 1000; i++) ;

            if (path.Equals("-1"))
            {

            }

            else
            {
                if (PrixAchat == "" || Nom == "")
                {
                    ViewBag.Message2 = "Assurez-vous que tous les champs sont remplis ";
                }
                else
                {
                    p.CodeProduit = "PROD" + p.Nom.Substring(0, 3) + random;
                    p.PrixAncien = p.PrixAchat + 10;
                    p.Note = 5;
                    p.DateSortie = @DateTime.Now;
                    p.Photo = path;
                    db.Produit.InsertOnSubmit(p);
                    db.SubmitChanges();
                    ViewBag.Message = "Le Produit a ete Ajouté : ";
                    ViewBag.Message1 = "Vous Pouvez Plus Modifier ou Supprimer Ce Produit! ";
                    ViewBag.code = p.CodeProduit;
                    ViewBag.nom = p.Nom;
                    ViewBag.prix = p.PrixAchat;
                    ViewBag.date = p.DateSortie;

                }
            }
            return View();
        }

        //k//////////////////////////////////////
        public string uploadimage(HttpPostedFileBase file)

        {

            //  Random r = new Random();
            Random r = new Random();

            string path = "-1";

            int random = r.Next();

            if (file != null && file.ContentLength > 0)

            {

                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))

                {

                    try

                    {
                        path = Path.Combine(Server.MapPath("../images"), random + Path.GetFileName(file.FileName));

                        file.SaveAs(path);

                        path = "../images/" + random + Path.GetFileName(file.FileName);
                        //  ViewBag.Message = "File uploaded successfully";

                    }

                    catch (Exception ex)

                    {
                        path = "-1";
                    }

                }
                else

                {
                    // Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                    ViewBag.Mes = "Juste les formats jpg, jpeg ou png sont accepter!";
                }

            }
            else

            {
                // Response.Write("<script>alert('Please select a file'); </script>");
                //ViewBag.Mes1 = " Veuillez Selectionner un fichier, Seul l'admin peut sauter cette etape! ";
                //path = "-1";

                path = "../images/avatar.png";

            }

            return path;

        }
        ///fin aAJOUT PRODUIT ////////////////////
        ///CrUD POUR LE MEMBRE--------------------


        public ActionResult news()
        {
            return View();

        }

        public ActionResult insertvente()
        {
            return View();

        }

        public ActionResult DeleteMembre()
        {
            return View();

        }
        [HttpPost]
        public ActionResult DeleteMembre(Utilisateur u, String Code, String Email, String MotifSuppression)

        {

            //p.CodeProduit = "PROD" + p.Nom.Substring(0, 3);
            //p.PrixAncien = p.PrixAchat + 10;
            //p.Note = 5;
            //p.DateSortie = @DateTime.Now;
            //// p.Photo = path;
            var DateSuppression = u.Nom;
            //var nom = u.c;
            var delete = from x in db.Utilisateurs where x.Code == u.Code select x;
            /////////////////////////////////////////
            //////////////////////////////
            if (Code == "" || Email == "" || MotifSuppression == "")
            {
                ViewBag.Message4 = "Vous devez remplir tous les champs svp!";
            }
            else
            {
                db.Utilisateurs.DeleteAllOnSubmit(delete);
                int i = db.GetChangeSet().Deletes.Count;
                db.SubmitChanges();
                if (i == 0)
                {
                    ViewBag.msg = "la suppression n'a pas fonctionnee! Verifier le Code! Si le probleme persiste, Appeler l'admin";
                }
                else
                {
                    ViewBag.Message = "Ce Compte est maintenant désactivé : ";
                    ViewBag.Message1 = "Un Message sera envoye a L'utilisateur et une notification a L'admin ";
                    ViewBag.code = u.Code;
                    return View();
                }
            }

            return View();
        }
        //

        public ActionResult Deconnexion()
        {
            // Session.Remove("username");
            Session["username"] = null;

            return RedirectToAction("Login", "Seke");
        }
        // permet d'affichage tous les utilisateurs!!!
        public ActionResult test()
        {
            Reservation r = new Reservation();
            DataClasses1DataContext db = new DataClasses1DataContext();
            // r.Montant = r.QuantiteReserve * r.PrixReserve;
            //  ViewBag.Montant = r.Montant;

            var mesreserves = from x in db.Reservations select x;
            List<Reservation> list = mesreserves.ToList();
            ViewBag.reservationlist = list;

            //ViewData["utilsateurtotal"] = utilisateur.ToList();
            return View();

        }

        public ActionResult GetSearchRecord3(string SearchText)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            List<Reservation> list = new List<Reservation>();
            var query = from x in db.Reservations where x.Nom.Contains(SearchText) || x.NumeroR.Contains(SearchText) || x.Adresse.Contains(SearchText) || x.Telephone.Contains(SearchText) select x;

            foreach (var x in query)
            {
                list.Add(x);
            }

            if (list.Count.Equals(0))
            {
                return PartialView("reservepartial", null);
            }
            else
            {
                return PartialView("reservepartial", list);
            }

        }
        public ActionResult ListeUtilisateur()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var mesutilisateur = from x in db.Utilisateurs select x;
            List<Utilisateur> list = mesutilisateur.ToList();
            ViewBag.UtilisateurList = list;
            //ViewData["utilsateurtotal"] = utilisateur.ToList();
            return View();
        }
        //*************************************************//


        public PartialViewResult Top3()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var mesutilisateur = from x in db.Produit select x;
            List<Produit> list = mesutilisateur.ToList();
            ViewBag.UtilisateurList = list;
            //ViewData["utilsateurtotal"] = utilisateur.ToList();
            return PartialView("IndexEmployess");
        }
        //test affichage par categories//**********************//
        public ActionResult Listeoldpic()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            DateTime a = Convert.ToDateTime("2020-03-29 20:09:52");
            var myoldprod = from y in db.Produit where y.DateSortie < a select y;
            List<Produit> list = myoldprod.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprod);
        }
        public ActionResult moinsVendue()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            int a = 25;
            var myoldprod = from y in db.Produit where y.QuantiteStock > a select y;
            List<Produit> list = myoldprod.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprod);
        }

        public ActionResult plusVendue()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            int a = 15;
            var myoldprod = from y in db.Produit where y.QuantiteStock < a select y;
            List<Produit> list = myoldprod.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprod);
        }
        public ActionResult presfini()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            int a = 5;
            var myoldprod = from y in db.Produit where y.QuantiteStock < a select y;
            List<Produit> list = myoldprod.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprod);
        }
        public ActionResult presperimes()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            DateTime a = Convert.ToDateTime("2020-04-25 20:09:52");
            var myoldprod = from y in db.Produit where y.DateSortie < a select y;
            List<Produit> list = myoldprod.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprod);
        }
        //*************************************************************************//
        // suivi de Location 
        public ActionResult suivilocation()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            // DateTime a = Convert.ToDateTime("2020-04-25 20:09:52");
            var myoldprode = from y in db.location select y;
            List<location> list = myoldprode.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprode);
        }
        public ActionResult GetSearchRecord6(string SearchText)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            List<location> list = new List<location>();
            var query = from x in db.location where x.NomProduit.Contains(SearchText) || x.CodeLocation.Contains(SearchText) select x;
            //var query = from db.Utilisateurs.Where(x => x.Nom.Contains(SearchText) || x.Email.Contains(SearchText)).Select(x => new Utilisateur { Nom = x.Nom, Email = x.Email }).ToList();

            foreach (var x in query)
            {
                list.Add(x);
            }

            if (list.Count.Equals(0))
            {
                return PartialView("SearchPartial2", null);
            }
            else
            {
                return PartialView("SearchPartial2", list);
            }

        }
        // suivi de reservation
        public ActionResult suivireservation()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            // DateTime a = Convert.ToDateTime("2020-04-25 20:09:52");
            var myoldprode = from y in db.Reservations select y;
            List<Reservation> list = myoldprode.ToList();
            ViewBag.UtilisateurList = list;
            return View(myoldprode);
        }

        public ActionResult GetSearchRecord7(string SearchText)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            List<Reservation> list = new List<Reservation>();
            var query = from x in db.Reservations where x.NumeroR.Contains(SearchText) || x.Code.Contains(SearchText) select x;
            //var query = from db.Utilisateurs.Where(x => x.Nom.Contains(SearchText) || x.Email.Contains(SearchText)).Select(x => new Utilisateur { Nom = x.Nom, Email = x.Email }).ToList();

            foreach (var x in query)
            {
                list.Add(x);
            }

            if (list.Count.Equals(0))
            {
                return PartialView("SearchPartial3", null);
            }
            else
            {
                return PartialView("SearchPartial3", list);
            }

        }
        //**************************************************************************//
        // fin de l'affichage uilisateurs
        // permet de faire la recherche pour l'utilisateur!!!

        public ActionResult GetSearchRecord(string SearchText)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            List<Utilisateur> list = new List<Utilisateur>();
            var query = from x in db.Utilisateurs where x.Nom.Contains(SearchText) || x.Prenom.Contains(SearchText) || x.Email.Contains(SearchText) || x.Province.Contains(SearchText) select x;
            //var query = from db.Utilisateurs.Where(x => x.Nom.Contains(SearchText) || x.Email.Contains(SearchText)).Select(x => new Utilisateur { Nom = x.Nom, Email = x.Email }).ToList();

            foreach (var x in query)
            {
                list.Add(x);
            }

            if (list.Count.Equals(0))
            {
                return PartialView("SearchPartial", null);
            }
            else
            {
                return PartialView("SearchPartial", list);
            }

        }


        // fin de la recherche utilisateur!
        // permet d'afficher tous les produits dela base de donnees!
        public ActionResult Produit()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            List<Vente> list1 = new List<Vente>();

            if (Session["Panier"] == null)
            {

                Session["Panier"] = list1;


            }
            else
            {


                list1 = (List<Vente>)Session["Panier"];

            }


            var Produitsdetails = from x in db.Produit select x;
            ViewData["ProduitTotale"] = Produitsdetails.ToList();

            var ProduitsVedette = from x in db.Produit orderby x.RabaisVente descending select x;
            var TopProduitVedette = Produitsdetails.Take(3);

            ViewData["ProduitVedette"] = TopProduitVedette.ToList();

            //Produit par Nombre de Vente 
            //Produit Nouveaute
            var ProduitsNew = from x in db.Produit orderby x.DateSortie descending select x;
            var TopProduitNew = ProduitsNew.Take(3);
            ViewData["ProduitNew"] = TopProduitNew.ToList();
            //Plus bas Prix
            var ProduitsBasPrix = from x in db.Produit orderby x.DateSortie descending select x;
            var TopBasPrix = ProduitsBasPrix.Take(1);
            ViewData["ProduitbasPrix"] = TopBasPrix.ToList();
            //Top Vente
            //var ProduitsBestVente = from x in dc.Produit orderby x.DateSortie descending select x;
            // var TopVente = ProduitsBestVente.Take(1);
            var ProduitsBestVente = (from x in db.Produit join y in db.Vente on x.CodeProduit equals y.CodeProduit orderby y.CodeProduit descending select x);
            var TopVente = ProduitsBestVente.Take(3);
            ViewData["ProduitTopVente"] = TopVente.ToList();
            List<Produit> list = Produitsdetails.ToList();
            ViewBag.ProduitsList = list;


            ViewData["NbrPanier"] = list1.ToList();


            return View();
        }

        public ActionResult GetSearchRecord2(string SearchText)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            List<Produit> list = new List<Produit>();
            var query = from c in db.Produit where c.Nom.Contains(SearchText) || c.Category.Contains(SearchText) /*|| c.DateSortie.Contains(SearchText) */select c;
            //var query = from db.Utilisateurs.Where(x => x.Nom.Contains(SearchText) || x.Email.Contains(SearchText)).Select(x => new Utilisateur { Nom = x.Nom, Email = x.Email }).ToList();

            foreach (var c in query)
            {
                list.Add(c);
            }

            if (list.Count.Equals(0))
            {
                return PartialView("partview", null);
            }
            else
            {
                return PartialView("partview", list);
            }

        }
        public ActionResult cat()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            List<Produit> list = new List<Produit>();
            var query = from c in db.Produit where c.Category == "A" /*|| c.DateSortie.Contains(SearchText) */select c;
            //var query = from db.Utilisateurs.Where(x => x.Nom.Contains(SearchText) || x.Email.Contains(SearchText)).Select(x => new Utilisateur { Nom = x.Nom, Email = x.Email }).ToList();

            foreach (var c in query)
            {
                list.Add(c);
            }

            if (list.Count.Equals(0))
            {
                return PartialView("cat2", null);
            }
            else
            {
                return PartialView("cat2", list);
            }

        }
        // fin affichage produit!!!!!!!!!!!!!!!!!!

        //Produit en vedette Affichage
        public ActionResult ProduitVedette()
        {
            //Produit new 
            var ProduitNew = from x in db.Produit orderby x.DateSortie descending select x;
            var TopProduitNew = ProduitNew.Take(3);
            ViewData["ProduitNew"] = TopProduitNew.ToList();
            //var getPorduitsdetails = dc.Produit.Single(x => x.CodeProduit == CodeProduit);
            var Produitsdetails = from x in db.Produit orderby x.RabaisVente descending select x;

            var ProduitVedette = Produitsdetails.Take(2);
            ViewData["ProduitTotale"] = Produitsdetails.ToList();
            ViewData["ProduitVedette"] = ProduitVedette.ToList();
            //Meilleure Vente
            var BestVente = from y in db.Vente.GroupBy(y => y.CodeProduit).OrderByDescending(Vente => Vente.Select(y => y.QuantiteVente).Count()) select y;



            var ProduitsBestVente = from y in BestVente join x in db.Produit on y.Key equals x.CodeProduit select x;
            var TopVente = ProduitsBestVente;
            ViewData["ProduitTopVente"] = TopVente.ToList();


            return View();
        }

        public ActionResult IndexAdmin()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Seke");
            }
            else
            {
                ViewBag.user = Session["username"];
                DataClasses1DataContext db = new DataClasses1DataContext();
                var Produitsdetails = from x in db.Produit select x;
                ViewData["ProduitTotale"] = Produitsdetails.ToList();

                var ProduitsVedette = from x in db.Produit orderby x.RabaisVente descending select x;
                var TopProduitVedette = Produitsdetails.Take(3);

                ViewData["ProduitVedette"] = TopProduitVedette.ToList();
                //Produit par Nombre de Vente 


                //Produit Nouveaute
                var ProduitsNew = from x in db.Produit orderby x.DateSortie descending select x;
                var TopProduitNew = ProduitsNew.Take(3);
                ViewData["ProduitNew"] = TopProduitNew.ToList();
                //Plus bas Prix

                var ProduitsBasPrix = from x in db.Produit orderby x.DateSortie descending select x;
                var TopBasPrix = ProduitsBasPrix.Take(1);
                ViewData["ProduitbasPrix"] = TopBasPrix.ToList();
                //Top Vente
                //var ProduitsBestVente = from x in dc.Produit orderby x.DateSortie descending select x;
                // var TopVente = ProduitsBestVente.Take(1);
                var ProduitsBestVente = (from x in db.Produit join y in db.Vente on x.CodeProduit equals y.CodeProduit orderby y.CodeProduit descending select x);
                var TopVente = ProduitsBestVente.Take(3);
                ViewData["ProduitTopVente"] = TopVente.ToList();
                List<Produit> list = Produitsdetails.ToList();
                ViewBag.ProduitsList = list;
                return View();
            }


        }
        public ActionResult VenteEmp()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var Produitsdetails = from x in db.Produit select x;
            var Ventesdetails = from x in db.Vente select x;
            var Utilisateurdetails = from x in db.Utilisateurs select x;
            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            ListTot.setProduit(Produitsdetails.ToList());
            ListTot.setVente(Ventesdetails.ToList());
            ListTot.setUtilisateur(Utilisateurdetails.ToList());
            if (ViewData["ProduitSelect"] == null)
            {
                Produit Prodetails = (from x in db.Produit select x).First();
                ViewData["ProduitSelect"] = Prodetails;
            }


            ViewData["ProduitCreate"] = ListTot;
            return View(ListTot);
        }

        [HttpPost]
        public ActionResult VenteEmp(String CodeProduit, String Code, float PrixVente, int QuantiteVente, int? Rabais, DateTime DateVente)
        {

            //String CodeProduit, String Code, String PrixVente, String QuantiteVente, String Rabais, DateTime DateVente
            DataClasses1DataContext db = new DataClasses1DataContext();



            Vente collection = new Vente();
            int LastIdVente;

            LastIdVente = 1;


            collection.IdVente = LastIdVente.ToString();
            collection.CodeEmployes = (string)Session["username"];
            collection.CodeProduit = CodeProduit;
            collection.Code = Code;
            collection.DateVente = DateVente;
            collection.PrixVente = PrixVente;
            collection.QuantiteVente = QuantiteVente;
            collection.Rabais = Rabais;
            collection.StatutP = 1;



            var Produitsdetails = from x in db.Produit select x;
            var Ventesdetails = from x in db.Vente select x;
            var Utilisateurdetails = from x in db.Utilisateurs select x;
            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            ListTot.setProduit(Produitsdetails.ToList());
            ListTot.setVente(Ventesdetails.ToList());
            ListTot.setUtilisateur(Utilisateurdetails.ToList());
            String RetourVente;

            ViewData["ProduitCreate"] = ListTot;
            try
            {



                db.Vente.InsertOnSubmit(collection);



                db.SubmitChanges();
                RetourVente = "Vente Effectue avec suceee";

                ViewData["EtatVente"] = RetourVente;

                return RedirectToAction("VenteEmp");

            }
            catch (Exception e)
            {
                if (ViewData["ProduitSelect"] == null)
                {
                    Produit Prodetails = (from x in db.Produit select x).First();
                    ViewData["ProduitSelect"] = Prodetails;
                }
                RetourVente = "Oups Petit Bobo Tu as oublie quelque Chose";

                ViewData["EtatVente"] = RetourVente;
                return View(ListTot);
            }
        }





        //Loc Employe


        public ActionResult LocEmp()
        {

            var Produitsdetails = from x in db.Produit select x;
            var Locdetails = from x in db.location select x;
            var Utilisateurdetails = from x in db.Utilisateurs select x;
            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            ListTot.setProduit(Produitsdetails.ToList());
            ListTot.setLocation(Locdetails.ToList());
            ListTot.setUtilisateur(Utilisateurdetails.ToList());
            if (ViewData["ProduitSelect"] == null)
            {
                Produit Prodetails = (from x in db.Produit select x).First();
                ViewData["ProduitSelect"] = Prodetails;
            }

            ViewData["ProduitCreate"] = ListTot;
            return View(ListTot);
        }
        //POST: COOP/Create
        [HttpPost]
        public ActionResult LocEmp(String CodeProduit, String Code, float PrixLocation, int QuantiteLocation, float? Rabais, DateTime DateLocation, DateTime DateRetour)
        { //String CodeProduit, String Code, String PrixVente, String QuantiteVente, String Rabais, DateTime DateVente

            DateTime DateV = new DateTime(2016, 01, 01);


            location collection = new location();
            int LastCodeLoc = 1;


            collection.CodeLocation = LastCodeLoc.ToString();
            collection.CodeEmployes = (string)Session["username"];
            collection.CodeProduit = CodeProduit;
            collection.Code = Code;
            collection.DateLocation = DateLocation;
            collection.DateRetour = DateRetour;
            collection.PrixLocation = PrixLocation;
            collection.QuantiteLocation = QuantiteLocation;
            collection.Rabais = Rabais;



            var Produitsdetails = from x in db.Produit select x;
            var Ventesdetails = from x in db.Vente select x;
            var Utilisateurdetails = from x in db.Utilisateurs select x;
            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            ListTot.setProduit(Produitsdetails.ToList());
            ListTot.setVente(Ventesdetails.ToList());
            ListTot.setUtilisateur(Utilisateurdetails.ToList());


            ViewData["ProduitCreate"] = ListTot;
            try
            {



                db.location.InsertOnSubmit(collection);



                db.SubmitChanges();




                return RedirectToAction("Produit");

            }
            catch (Exception e)
            {
                return View(ListTot);
            }
        }

        public ActionResult Panier()
        {
            String NomProd;
            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            var Panier1 =
                from x in db.Vente
                join y in db.Produit on x.CodeProduit equals y.CodeProduit
                where x.Code == (string)HttpContext.Session["username"] && x.StatutP == 0
                select new Panier(y.Nom, x.IdVente, y.CodeProduit, y.Photo, (double)x.PrixVente, (int)x.QuantiteVente, (double)(y.PrixAncien * x.QuantiteVente), y.Desc, y.QuantiteStock, x.DateVente.ToString(), x.StatutP, x.Code);
            var Panier2 =
            from x in db.location
            join y in db.Produit on x.CodeProduit equals y.CodeProduit
            where x.Code == (string)HttpContext.Session["username"]
            select new PanierLoc(y.Nom, x.CodeLocation, y.CodeProduit, y.Photo, (double)x.PrixLocation, (int)x.QuantiteLocation, (double)(y.PrixLoc * x.QuantiteLocation), y.Desc, y.QuantiteStock, x.DateLocation.ToString(), x.DateRetour.ToString(), x.Code);





            String CodeUser = (string)HttpContext.Session["username"];
            //var Ventedetails = from x in db.Ventes where x.Code == CodeUser select x;
            var Ventedetails = from x in db.Vente select x;
            //var Produitdetails = from x in db.Produits where x.Nom == NomProd select x;
            ListTot.setVente(Ventedetails.ToList());
            ListTot.setPanier(Panier1.ToList());
            ListTot.setPanLoc(Panier2.ToList());

            // ListTot.setProduit(Produitdetails.ToList());
            String Link;

            String Code = (string)HttpContext.Session["username"];






            Link = "IndexMembre";
            ViewData["LinkIndex"] = Link;

            var Ventedetails1 = from x in db.Vente where x.Code == Code && x.StatutP == 0 select x;
            var Ventedetails2 = from x in db.location where x.Code == Code select x;

            ViewData["NbrPanier"] = Ventedetails1.ToList();
            ViewData["NbrPanier2"] = Ventedetails2.ToList();





            return View(ListTot);


        }



        [HttpPost]
        public ActionResult Panier(String CodeProduit, Double prixAncien, float? RabaisVente)
        {

            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            String Code = (string)HttpContext.Session["username"]; ;
            var Ventedetails = from x in db.Vente where x.Code == Code select x;

            //  var Produitdetails = from x in db.Produit where x.CodeProduit == CodeProduit select x;
            ListTot.setVente(Ventedetails.ToList());
            // ListTot.setProduit(Produitdetails.ToList());




            int LastVente = 1;
            Vente collection = new Vente();
            collection.IdVente = LastVente.ToString();

            collection.CodeProduit = CodeProduit;
            collection.Code = (string)HttpContext.Session["username"];
            collection.DateVente = DateTime.Now;
            collection.PrixVente = prixAncien;
            collection.QuantiteVente = 1;
            collection.Rabais = RabaisVente;
            collection.StatutP = 0;



            var Produitsdetails = from x in db.Produit select x;
            var Ventesdetails = from x in db.Vente select x;
            var Utilisateurdetails = from x in db.Utilisateurs select x;


            ListTot.setProduit(Produitsdetails.ToList());
            ListTot.setVente(Ventesdetails.ToList());
            ListTot.setUtilisateur(Utilisateurdetails.ToList());


            ViewData["ProduitCreate"] = ListTot;
            try
            {

                db.Vente.InsertOnSubmit(collection);



                db.SubmitChanges();


                return RedirectToAction("IndexMembre");




            }
            catch (Exception e)
            {

                return RedirectToAction("IndexMembre");


            }


        }

        public ActionResult VenteClient()
        {
            List<Vente> list1 = new List<Vente>();
            if (Session["Panier"] == null)
            {

                Session["Panier"] = list1;


            }
            else
            {


                list1 = (List<Vente>)Session["Panier"];

            }

            ListeTotale ListTot = new ListeTotale();



            List<Panier> Panier1 = new List<Panier>();
            foreach (Vente vente in list1)
            {
                String codeProd = vente.CodeProduit;
                int? QteVente = vente.QuantiteVente;


                var ListeProd = from y in db.Produit
                                where y.CodeProduit == codeProd
                                select new Panier(y.Nom, vente.IdVente, y.CodeProduit, y.Photo, (double)y.PrixAncien, (int)QteVente, (double)(y.PrixAncien * QteVente), y.Desc, y.QuantiteStock, vente.DateVente.ToString());

                // Panier pan1 = new Panier("prod1", "fef", 20, 30, 300.20, "blablabla");
                //Panier1.Add(pan1);
                ViewData["NbrPanier"] = list1.ToList();

                Panier1.Add((Panier)ListeProd.First());


            }


            ListTot.setVente(list1);
            ListTot.setPanier(Panier1.ToList());

            return View(ListTot);

        }

        [HttpPost]

        public ActionResult VenteClient(String CodeProduit, Double prixAncien, float? RabaisVente)
        {

            List<Vente> list1 = new List<Vente>();
            if (Session["Panier"] == null)
            {

                Session["Panier"] = list1;


            }
            else
            {


                list1 = (List<Vente>)Session["Panier"];

            }
            ListeTotale ListTot = new ListeTotale();



            ListTot.setVente(list1);



            Vente collection = new Vente();
            int LastVente = 1;
            collection.IdVente = LastVente.ToString();
            collection.CodeProduit = CodeProduit;
            collection.Code = "Client";
            collection.DateVente = DateTime.Now;
            collection.PrixVente = prixAncien;
            collection.QuantiteVente = 1;
            collection.Rabais = RabaisVente;

            list1.Add(collection);

            Session["Panier"] = list1;

            var Produitsdetails = from x in db.Produit select x;
            ListTot.setProduit(Produitsdetails.ToList());



            return RedirectToAction("Produit");


        }

        public ActionResult Product()
        {


            String codeProd = "PRODCra";
            var Produitsdetails = from x in db.Produit where x.CodeProduit == codeProd select x;

            return View(Produitsdetails);
            //   var Produitsdetails = from x in db.Produit select x ;




            return View();
        }
        [HttpPost]
        public ActionResult Product(String CodeProduit)
        {

            var Produitsdetails = from x in db.Produit where x.CodeProduit == CodeProduit select x;

            return View(Produitsdetails);
        }

        [HttpPost]
        public ActionResult EditPanierQte(int IdVente, String CodeProduit, int? Quantite)
        {
            int? StatutCode = 0;
            String Code = (string)HttpContext.Session["username"];
            if (Code != null)
            {
                var ListStatutCode = (from x in db.Utilisateurs where x.Code == Code select x.Statut).First();

                StatutCode = ListStatutCode;
            }
            else
            {
                StatutCode = 4;

            }


            if (StatutCode == 1 || StatutCode == 3)
            {
                Vente ven1 = db.Vente.Single(x => x.CodeProduit == CodeProduit && x.IdVente == "1" && x.Code == Code);


                if (Quantite == 0)
                {
                    db.Vente.DeleteOnSubmit(ven1);
                    db.SubmitChanges();
                    return RedirectToAction("Panier");
                }
                else
                {
                    ven1.QuantiteVente = Quantite;

                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);

                    }

                    return RedirectToAction("Panier");
                }

            }
            else
            {
                List<Vente> list1 = new List<Vente>();
                if (Session["Panier"] == null)
                {

                    Session["Panier"] = list1;


                }
                else
                {


                    list1 = (List<Vente>)Session["Panier"];

                }


                foreach (Vente vente in list1)
                {
                    if (vente.CodeProduit == CodeProduit && vente.IdVente == "1")
                    {
                        if (Quantite == 0)
                        {
                            list1.Remove(vente);
                            return RedirectToAction("VenteClient");
                        }
                        else
                        {
                            vente.QuantiteVente = Quantite;
                        }

                    }
                }
                Session["Panier"] = list1;

                return RedirectToAction("VenteClient");





            }



        }


        [HttpPost]
        public ActionResult ProdVenteEmp(String CodeProduit)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var Produitsdetails = from x in db.Produit select x;
            var Ventesdetails = from x in db.Vente select x;
            var Utilisateurdetails = from x in db.Utilisateurs select x;
            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            ListTot.setProduit(Produitsdetails.ToList());
            ListTot.setVente(Ventesdetails.ToList());
            ListTot.setUtilisateur(Utilisateurdetails.ToList());
            Produit Prodetails = (from x in db.Produit where x.CodeProduit == CodeProduit select x).FirstOrDefault();
            ViewData["ProduitSelect"] = Prodetails;


            ViewData["ProduitCreate"] = ListTot;
            return View("VenteEmp", ListTot);
        }
        [HttpPost]
        public ActionResult ProdLocEmp(String CodeProduit)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();


            var Produitsdetails = from x in db.Produit select x;
            var Locdetails = from x in db.location select x;
            var Utilisateurdetails = from x in db.Utilisateurs select x;
            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            ListTot.setProduit(Produitsdetails.ToList());
            ListTot.setLocation(Locdetails.ToList());
            ListTot.setUtilisateur(Utilisateurdetails.ToList());
            Produit Prodetails = (from x in db.Produit where x.CodeProduit == CodeProduit select x).FirstOrDefault();
            ViewData["ProduitSelect"] = Prodetails;

            ViewData["ProduitCreate"] = ListTot;
            return View("LocEmp", ListTot);
        }


        public ActionResult PanierEmp()
        {
            String NomProd;
            List<Vente> list1 = new List<Vente>();
            ListeTotale ListTot = new ListeTotale();
            var Panier1 =
                from x in db.Vente
                join y in db.Produit on x.CodeProduit equals y.CodeProduit

                select new Panier(y.Nom, x.IdVente, y.CodeProduit, y.Photo, (double)x.PrixVente, (int)x.QuantiteVente, (double)(y.PrixAncien * x.QuantiteVente), y.Desc, y.QuantiteStock, x.DateVente.ToString(), x.StatutP, x.Code);
            var Panier2 =
            from x in db.location
            join y in db.Produit on x.CodeProduit equals y.CodeProduit

            select new PanierLoc(y.Nom, x.CodeLocation, y.CodeProduit, y.Photo, (double)x.PrixLocation, (int)x.QuantiteLocation, (double)(y.PrixLoc * x.QuantiteLocation), y.Desc, y.QuantiteStock, x.DateLocation.ToString(), x.DateRetour.ToString(), x.Code);





            String CodeUser = (string)HttpContext.Session["username"];
            //var Ventedetails = from x in db.Ventes where x.Code == CodeUser select x;
            var Ventedetails = from x in db.Vente select x;
            //var Produitdetails = from x in db.Produits where x.Nom == NomProd select x;
            ListTot.setVente(Ventedetails.ToList());
            ListTot.setPanier(Panier1.ToList());
            ListTot.setPanLoc(Panier2.ToList());

            // ListTot.setProduit(Produitdetails.ToList());
            String Link;

            String Code = (string)HttpContext.Session["username"];





            Link = "IndexEmployes";
            ViewData["LinkIndex"] = Link;

            var Ventedetails1 = from x in db.Vente select x;

            var Ventedetails2 = from x in db.location select x;

            ViewData["NbrPanier"] = Ventedetails1.ToList();
            ViewData["NbrPanier2"] = Ventedetails2.ToList();




            return View(ListTot);


        }
        
        [HttpPost]
        public ActionResult DeletePanier()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            int? StatutCode = 0;
            String Code = (string)HttpContext.Session["username"];
            if (Code != null)
            {
                var ListStatutCode = (from x in db.Utilisateurs where x.Code == Code select x.Statut).First();

                StatutCode = ListStatutCode;
            }
            else
            {
                StatutCode = 4;

            }


            if (StatutCode == 1 || StatutCode == 2)
            {
                var VenteSupr = from x in db.Vente where x.Code == Code && x.StatutP == 0 select x;



                db.Vente.DeleteAllOnSubmit(VenteSupr);
                db.SubmitChanges();
                return RedirectToAction("PanierEmp");



            }
            else if (StatutCode == 3)
            {
                var VenteSupr = from x in db.Vente where x.Code == Code && x.StatutP == 0 select x;



                db.Vente.DeleteAllOnSubmit(VenteSupr);
                db.SubmitChanges();
                return RedirectToAction("Panier");

            }
            else
            {
                List<Vente> list1 = new List<Vente>();



                Session["Panier"] = list1;





                return RedirectToAction("VenteClient");
            }


        }
    }
}