using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.ETender.Contracts
{

    public class Status
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public object enValue { get; set; }
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

    public class TenderData
    {
        public int id { get; set; }
        public string goodsDescr { get; set; }
    }

    public class Tender
    {
        public int id { get; set; }
        public StateOrg stateOrg { get; set; }
        public TenderData tenderData { get; set; }
        public string regNumber { get; set; }
    }

    public class ContractType
    {
        public int id { get; set; }
        public string created { get; set; }
        public object endDate { get; set; }
        public string mdValue { get; set; }
        public string ruValue { get; set; }
        public object enValue { get; set; }
    }

    public class Participant
    {
        public int id { get; set; }
        public int juristicPerson { get; set; }
        public string jurName { get; set; }
        public string lastName { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string fullName { get; set; }
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

    public class Row
    {
        public int id { get; set; }
        public Status status { get; set; }
        public string currentStatusDate { get; set; }
        public int fkCurrentStatusId { get; set; }
        public Tender tender { get; set; }
        public ContractType contractType { get; set; }
        public object fkBaseContractId { get; set; }
        public string contractNumber { get; set; }
        public string contractDate { get; set; }
        public double amount { get; set; }
        public Participant participant { get; set; }
        public int? fkOfferId { get; set; }
        public int? fkDecisionId { get; set; }
        public string registerDate { get; set; }
        public string finalDate { get; set; }
        public string contractRealNumber { get; set; }
        public object addAccordReasonDocuments { get; set; }
        public object addAccordReason { get; set; }
        public object addAccordShortDescr { get; set; }
        public Goods goods { get; set; }
        public string note { get; set; }

    }

    public class RootObject
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public List<Row> rows { get; set; }
    }
}

