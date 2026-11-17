    using PrijavaIspita.Models;
    using PrijavaIspita.Models.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    namespace PrijavaIspita.Models.EFRepository
    {
        public class PredmetRepository : IPredmetRepository
        {
            private PrijavaIspitaContext prijavaIspitaEntities = new PrijavaIspitaContext();

            // Brisanje odabranih predmeta
            public void Delete(int[] predmetiIds)
            {
                foreach (int predmetId in predmetiIds)
                {
                    Predmet predmet = prijavaIspitaEntities.Predmet.Single(t => t.PredmetId == predmetId);

                    // Provera da li predmet ima prijava, ako ima ne moze da se brise
                    if (!predmet.Prijavas.Any())
                    {
                        prijavaIspitaEntities.Predmet.Remove(predmet);
                    }
                }
                prijavaIspitaEntities.SaveChanges();
            }

        // Dohvatanje predmeta koji nemaju prijava
        public IEnumerable<PredmetBO> GetAllUnused()
        {
            var predmets = prijavaIspitaEntities.Predmet.Where(t => !t.Prijavas.Any())
                .Select(predmet => new PredmetBO
                {
                    Id = predmet.PredmetId,
                    Naziv = predmet.Naziv,
                    ProfesorID = predmet.ProfesorID ?? 0,
                    ProfesorIme = predmet.Profesor.Ime,
                    ProfesorPrezime = predmet.Profesor.Prezime,
                    SmerId = predmet.SmerId
                })
                .ToList();

            return predmets;
        }

        //dodavanje novog predmeta
        public void Add(PredmetBO model)
        {
            using (var context = new PrijavaIspitaContext())
            {
                var profesor = context.Korisnici.FirstOrDefault(k => k.KorisnikID == model.ProfesorID);
                if (profesor == null)
                    throw new Exception("Profesor ne postoji");

                var predmet = new Predmet
                {
                    Naziv = model.Naziv,
                    SmerId = model.SmerId,
                    ProfesorID = profesor.KorisnikID
                };

                context.Predmet.Add(predmet);
                context.SaveChanges();
            }
        }

        // Dohvatanje predmeta po ID-u
        public PredmetBO? GetById(int id)
            {
                using (var context = new PrijavaIspitaContext())
                {
                    var p = context.Predmet
                        .Where(x => x.PredmetId == id)
                        .Select(x => new PredmetBO
                        {
                            Id = x.PredmetId,
                            Naziv = x.Naziv,
                            ProfesorID = (int)x.ProfesorID,
                            ProfesorIme = x.Profesor.Ime,
                            ProfesorPrezime = x.Profesor.Prezime,
                            SmerId = x.SmerId
                        })
                        .FirstOrDefault();

                    return p;
                }
            }

            // Update postojećeg predmeta
            public void Update(PredmetBO model)
            {
                using (var context = new PrijavaIspitaContext())
                {
                    var predmet = context.Predmet.FirstOrDefault(p => p.PredmetId == model.Id);
                    if (predmet != null)
                    {
                        predmet.Naziv = model.Naziv;
                        predmet.ProfesorID = model.ProfesorID;
                        predmet.SmerId = model.SmerId;
                        context.SaveChanges();
                    }
                }
            }
            
            // Lista svih smerova
            public IEnumerable<SmerBO> GetAllSmerovi()
            {
                return prijavaIspitaEntities.Smer
                    .Select(s => new SmerBO
                    {
                        Id = s.SmerId,
                        Naziv = s.Naziv
                    }).ToList();
            }
            //svi korisnici sa rolom - profesor
            public List<KorisnikBO> GetAllProfesori()
            {
                using (var context = new PrijavaIspitaContext())
                {
                    var profesori = (from u in context.UserRoles
                                     join k in context.Korisnici on u.KorisnikID equals k.KorisnikID
                                     join r in context.Roles on u.RoleID equals r.RoleID
                                     where r.RoleName == "Profesor"
                                     select new KorisnikBO
                                     {
                                         Id = k.KorisnikID,
                                         Ime = k.Ime,
                                         Prezime = k.Prezime,
                                         Email = k.Email
                                     }).ToList();

                    return profesori;
                }
            }

            //svi predmeti sa odgvarajucim profesorom i smerom
            public IEnumerable<PredmetBO> GetAll()
            {
                return prijavaIspitaEntities.Predmet
                    .Select(predmet => new PredmetBO
                    {
                        Id = predmet.PredmetId,
                        Naziv = predmet.Naziv,
                        ProfesorID = predmet.ProfesorID ?? 0, //ako predmet nema dodeljenog profesora (null) pise se 0.
                        ProfesorIme = predmet.Profesor.Ime,
                        ProfesorPrezime = predmet.Profesor.Prezime,
                        SmerId = predmet.SmerId
                    })
                    .ToList();
            }

    }
    }
