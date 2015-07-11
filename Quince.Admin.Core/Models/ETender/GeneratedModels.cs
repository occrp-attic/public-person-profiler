using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.ETender
{
    public class TenderStatus
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public object enValue { get; set; }
    }

    public class TenderType
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public string enValue { get; set; }
    }

    public class OrgLegalForm
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public object enValue { get; set; }
    }

    public class Bank
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public string enValue { get; set; }
        public string mdShort { get; set; }
        public object ruShort { get; set; }
        public object enShort { get; set; }
        public string mfoCode { get; set; }
        public string fiscalCode { get; set; }
        public string sapiCode { get; set; }
    }

    public class StateOrg
    {
        public int id { get; set; }
        public string code { get; set; }
        public string orgName { get; set; }
        public string address { get; set; }
        public int? cuatm { get; set; }
        public OrgLegalForm orgLegalForm { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public Bank bank { get; set; }
        public string account { get; set; }
        public string treasutyAcc { get; set; }
        public object fiscalCode { get; set; }
        public object bankAccount { get; set; }
        public object fkRefTerDepTreasure { get; set; }
    }

    public class Goods
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public string enValue { get; set; }
        public string code { get; set; }
        public int parentId { get; set; }
        public int typeId { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public object enValue { get; set; }
    }

    public class Language
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public object enValue { get; set; }
    }

    public class DeliveryTerms
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public string enValue { get; set; }
    }

    public class TenderData
    {
        public int id { get; set; }
        public string goodsDescr { get; set; }
        public Goods goods { get; set; }
        public Status status { get; set; }
        public string forWhoPurchase { get; set; }
        public Language language { get; set; }
        public DeliveryTerms deliveryTerms { get; set; }
        public string pressOffersPlace { get; set; }
        public string pressOffersDate { get; set; }
        public string openDateTime { get; set; }
        public string deliveryDate { get; set; }
        public int offerEstimateByPrice { get; set; }
        public object forResidenceOnlyReason { get; set; }
    }

    public class Bulletin
    {
        public int id { get; set; }
        public string bulletinNumb { get; set; }
        public string publDate { get; set; }
        public int accessTime { get; set; }
    }

    public class Row
    {
        public int id { get; set; }
        public TenderStatus tenderStatus { get; set; }
        public string refCurrentStatusDate { get; set; }
        public int fkCurrentStatusId { get; set; }
        public TenderType tenderType { get; set; }
        public string refTendeOpenDate { get; set; }
        public string regNumber { get; set; }
        public StateOrg stateOrg { get; set; }
        public TenderData tenderData { get; set; }
        public int fkResponsibleId { get; set; }
        public int perspDataStatus { get; set; }
        public string notes { get; set; }
        public int complaintCount { get; set; }
        public Bulletin bulletin { get; set; }
        public int? fkAnnulBulletinId { get; set; }
        public int fkDataIdAtApprov { get; set; }
        public int contractsCount { get; set; }
        public int questionCount { get; set; }
    }

    public class RootObject
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public List<Row> rows { get; set; }
    }
}
