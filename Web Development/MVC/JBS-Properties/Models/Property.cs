namespace John_Baxter_Schilling.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;

    public class Property : DbContext
    {
        // Your context has been configured to use a 'Property' connection string from your application's
        // configuration file (App.config or Web.config). By default, this connection string targets the
        // 'John_Baxter_Schilling.Data.Property' database on your LocalDb instance.
        //
        // If you wish to target a different database and/or database provider, modify the 'Property'
        // connection string in the application configuration file.
        public Property()
            : base("name=Property")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Dwelling> Dwellings { get; set; }
    }

    public class Dwelling
    {
        public int Id { get; set; }

        [Display(Name = "Unit / Address Number")]
        public int PropertyAddressNumber { get; set; }

        [Display(Name = "Availability?")]
        public bool? Availability { get; set; }

        [Display(Name = "Currently Rented?")]
        public bool? CurrentlyRented { get; set; }

        [Display(Name = "Street")]
        [DataType(DataType.Text)]
        public string PropertyStreetName { get; set; }

        [Display(Name = "City")]
        [DataType(DataType.Text)]
        public string PropertyCity { get; set; }

        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public int PropertyPostalCode { get; set; }

        [Display(Name = "Purchase Price")]
        [DataType(DataType.Currency)]
        public int? PropertyPurchaseAmount { get; set; }

        [Display(Name = "Date of Purchase")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Names of Lease")]
        [DataType(DataType.Text)]
        public string NamesOnLease { get; set; }

        [Display(Name = "Tennet's Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumberForTennet { get; set; }

        [Display(Name = "Tennet's Email")]
        [DataType(DataType.EmailAddress)]
        public string PrimaryTennetEmailAddress { get; set; }

        [Display(Name = "Cashflow Unit Amount per Cycle")]
        [DataType(DataType.Currency)]
        public short? CurrentMonthlyRentAmountSignedForThisDwelling;

        [Display(Name = "Lease Renewal Date")]
        [DataType(DataType.Date)]
        public DateTime? LeaseRenewalDate { get; set; }

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        protected string ProprietoryNotes { get; set; }

        public enum StateAbbreviation
        {
            KY = 0,
            IN = 1,
            AA = 2,
            AE = 3,
            AK = 4,
            AL = 5,
            AP = 6,
            AR = 7,
            AS = 8,
            AZ = 9,
            CA = 10,
            CO = 11,
            CT = 12,
            DC = 13,
            DE = 14,
            FL = 15,
            FM = 16,
            GA = 17,
            GU = 18,
            HI = 19,
            IA = 20,
            ID = 21,
            IL = 22,
            KS = 23,
            LA = 24,
            MA = 25,
            MD = 26,
            ME = 27,
            MH = 28,
            MI = 29,
            MN = 30,
            MO = 31,
            MP = 32,
            MS = 33,
            MT = 34,
            NC = 35,
            ND = 36,
            NE = 37,
            NH = 38,
            NJ = 39,
            NM = 40,
            NV = 41,
            NY = 42,
            OH = 43,
            OK = 44,
            OR = 45,
            PA = 46,
            PR = 47,
            PW = 48,
            RI = 49,
            SC = 50,
            SD = 51,
            TN = 52,
            TX = 53,
            UT = 54,
            VA = 55,
            VI = 56,
            VT = 57,
            WA = 58,
            WI = 59,
            WV = 60,
            WY = 61
        };

        public string iStateAbbreviation
        {
            set
            {
                foreach (StateAbbreviation iStateAbbreviation in Enum.GetValues(typeof(StateAbbreviation)))
                {
                    iStateAbbreviation.ToString();
                }
            }
        }

        [Display(Name = "Address")]
        public string OfficialAddress => PropertyAddressNumber + " " + PropertyStreetName + " " + PropertyCity + " " + "KY" + " " + PropertyPostalCode;
    }
}