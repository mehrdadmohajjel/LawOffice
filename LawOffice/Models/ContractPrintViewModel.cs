namespace LawOffice.Models
{
    public class ContractPrintViewModel
    {
        // مشخصات قرارداد
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; }
        public string FeeAmount { get; set; }

        // مشخصات وکیل (کارآموز)
        public string LawyerName { get; set; }
        public string LawyerLastName { get; set; }
        public string LawyerFatherName { get; set; }
        public string LawyerNationalCode { get; set; }
        public string LawyerLicenseNumber { get; set; }
        public string LawyerLevel { get; set; } // پایه: کارآموز وکالت یا وکیل پایه یک

        // مشخصات موکل
        public string ClientName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientFatherName { get; set; }
        public string ClientNationalCode { get; set; }
        public string ClientAddress { get; set; }
        public string ClientPhone { get; set; }
    }
}
