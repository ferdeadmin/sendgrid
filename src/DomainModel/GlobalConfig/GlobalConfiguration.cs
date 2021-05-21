using System.Collections.Generic;

namespace DomainModel.GlobalConfig
{
    public class TcGlobalConfiguration
    {
        public int tcDetailIdPk_int { get; set; }
        public string tcName_txt { get; set; }
        public string phoneNumber_txt { get; set; }
        public string emailAddress_txt { get; set; }
        public string webAddress_txt { get; set; }
        public string faxNumber_txt { get; set; }
        public string address1_txt { get; set; }
        public string address2_txt { get; set; }
        public string postalCode_txt { get; set; }
        public string postalDistrict_txt { get; set; }
        public string orgNumber_txt { get; set; }
        public string vatId_txt { get; set; }
        public string accountNumber_txt { get; set; }
        public string ibanNumber_txt { get; set; }
        public string swiftNumber_txt { get; set; }
        public int tifMinTimeDiffMinute_int { get; set; }
        public int tifMaxNoOfDays_int { get; set; }
        public int minAutosysWaitDays_int { get; set; }
        public string eaContractNumber_txt { get; set; }
        public int eaLevelOfservice_int { get; set; }
        public int tcActorID_int { get; set; }
        public int invoiceHeaderTextLimit_int { get; set; }
        public int invoiceSummaryTextLimit_int { get; set; }
        public int customerSequenceMinValue_int { get; set; }
        public int customerSequenceMaxValue_int { get; set; }
        public string bicCode_txt { get; set; }
        public string tcBankAgreementNumber_txt { get; set; }
        public int autosysAgreementId_txt { get; set; }
        public string exportAccountingHeaderText_txt { get; set; }
        public string exportAccountingExporterId_txt { get; set; }
        public int kidActorNumber_int { get; set; }
        public string passingImageLocationUserName_txt { get; set; }
        public string passingImageLocationPassword_txt { get; set; }
        public string passingImageLocationURL_txt { get; set; }
        public string autosysResponseUserName_txt { get; set; }
        public string autosysResponsePassword_txt { get; set; }
        public string autosysResponseURL_txt { get; set; }
        public int paymentTextLimit_int { get; set; }
        public bool isFileCompressionEnabled_bool { get; set; }
    }

    public class GlobalConfiguration
    {
        public int totalCount { get; set; }
        public int startIndex { get; set; }
        public int batchSize { get; set; }
        public string sort { get; set; }
        public List<TcGlobalConfiguration> tcGlobalConfiguration { get; set; }
    }
}
