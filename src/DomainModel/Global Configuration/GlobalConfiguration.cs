using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Global_Configuration
{
    public class GlobalConfiguration
    {
        public int? tcId_int { get; set; }
        [Required]
        public string actorId_txt { get; set; }
        [Required]
        public int? paymentTextLimit_int { get; set; }
        public string tcName_txt { get; set; }
        public int? eaWriteOffLimit_int { get; set; }
        public bool isLesseeApplicableInAutosys_bool { get; set; }
        [Required]
        public string phoneNumber_txt { get; set; }
        [Required]
        public string emailAddress_txt { get; set; }
        [Required]
        public string webAddress_txt { get; set; }
        public string faxNumber_txt { get; set; }
        [Required]
        public string address1_txt { get; set; }
        public string address2_txt { get; set; }
        [Required]
        public string postalCode_txt { get; set; }
        [Required]
        public string postalDistrict_txt { get; set; }
        [Required]
        public string orgNumber_txt { get; set; }
        [Required]
        public string vatId_txt { get; set; }
        [Required]
        public string invoiceHotelAccessToken_txt { get; set; }
        [Required]
        public string invoiceHotelSecretKey_txt { get; set; }
        [Required]
        public string accountNumber_txt { get; set; }
        [Required]
        public string ibanNumber_txt { get; set; }
        [Required]
        public string swiftNumber_txt { get; set; }
        public int? tifMaxNoOfDays_int { get; set; }
        public int? minAutosysWaitDays_int { get; set; }
        [MinLength(4), MaxLength(4)]
        public string eaContractNumber_txt { get; set; }
        public int? eaLevelOfService_int { get; set; }

        [Required]
        public decimal? tspIssuerFee_dec { get; set; }
        [Required]
        public decimal? tspIssuerFeePercentage_dec { get; set; }
        [Required]
        public int? tspCustomerSequenceMinValue_int { get; set; }
        [Required]
        public int? tspCustomerSequenceMaxValue_int { get; set; }
        [Required]
        public int? invoiceHeaderTextLimit_int { get; set; }
        [Required]
        public int? invoiceSummaryTextLimit_int { get; set; }
        [Required]
        public int? customerSequenceMinValue_int { get; set; }
        [Required]
        public int? customerSequenceMaxValue_int { get; set; }
        [Required]
        public string bicCode_txt { get; set; }
        [Required]
        public string tcBankAgreementNumber_txt { get; set; }
        [Required]
        [MaxLength(400)]
        public string exportAccountingHeaderText_txt { get; set; }
        [Required]
        [MaxLength(20)]
        public string exportAccountingExporterId_txt { get; set; }
        [Required]
        public bool isFileCompressionEnabled_bool { get; set; }
        [Required]
        public string invoiceHotelUrl_txt { get; set; }
        [Required]
        public int? kidActorNumber_int { get; set; }
        public int tifMinTimeDifferenceMinute_int { get; set; }
        public string tifListFormatVersion_txt { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Value must be greater than zero.")]
        public int? dcRequestBatchSize_int { get; set; }
        [Required]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Value must be greater than zero.")]
        public int? autosysBatchSize_int { get; set; }
        public string remittanceText_txt { get; set; }
        [Required]
        [MinLength(2), MaxLength(2)]
        public string transitType_txt { get; set; }
        [Required]
        [MinLength(1), MaxLength(35)]
        public string contactPersonFirstName_txt { get; set; }
        [Required]
        [MinLength(1), MaxLength(70)]
        public string contactPersonLastName_txt { get; set; }
        public int? autosysReIdentificationWaitDays_int {get;set;}
    }

}
