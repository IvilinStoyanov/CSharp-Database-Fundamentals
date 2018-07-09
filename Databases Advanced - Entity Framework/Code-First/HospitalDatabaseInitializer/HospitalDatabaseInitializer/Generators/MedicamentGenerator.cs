namespace P01_HospitalDatabase.Generators
{
    //using System.IO;

    using P01_HospitalDatabase.Data;
    using P01_HospitalDatabase.Data.Models;

   public class MedicamentGenerator
    {
        internal static Medicament[] InitialMedicamentSeed(HospitalContext context)
        {
            Medicament[] medicamentNames = new Medicament[]
             {
               new Medicament() {MedicamentId = 1, Name = "Biseptol", },
               new Medicament() {MedicamentId = 2, Name = "Ciclobenzaprina" },
               new Medicament() {MedicamentId = 3, Name = "Curam" },
               new Medicament() {MedicamentId = 4, Name = "Diclofenaco" },
               new Medicament() {MedicamentId = 5, Name = "Disflatyl" },
               new Medicament() {MedicamentId = 6, Name = "Duvadilan" },
               new Medicament() {MedicamentId = 7, Name = "Efedrin" },
               new Medicament() {MedicamentId = 8, Name = "Flanax" },
               new Medicament() {MedicamentId = 9, Name = "Fluimucil" },
               new Medicament() {MedicamentId = 10, Name = "Navidoxine" },
               new Medicament() {MedicamentId = 11, Name = "Nistatin" },
               new Medicament() {MedicamentId = 12, Name = "Olfen" },
               new Medicament() {MedicamentId = 13, Name = "Pentrexyl" },
               new Medicament() {MedicamentId = 14, Name = "Primolut Nor" },
               new Medicament() {MedicamentId = 15, Name = "Primperan" },
               new Medicament() {MedicamentId = 16, Name = "Propoven" },
               new Medicament() {MedicamentId = 17, Name = "Reglin" },
               new Medicament() {MedicamentId = 18, Name = "Terramicina Oftalmica" },
               new Medicament() {MedicamentId = 19, Name = "Ultran" },
               new Medicament() {MedicamentId = 20, Name = "Viartril-S"}
             };

            return medicamentNames;

        }

        public static void Generate(string medicamentName, HospitalContext context)
        {
            context.Medicaments.Add(new Medicament() { Name = medicamentName });
        }
    }
}
