using Quince.Admin.Core.Clients;
using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Contracts;
using Quince.Admin.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Managers
{
    public class ApiManager
    {
        public static IEnumerable<ApiProgressModel> UpdateETenders(int onPage, int requests)
        {
            var tenderUrl = "http://etender.gov.md/json/tenderList?rows=" + onPage;
            var index = 0;
            for (var request = 1; request <= requests; request++)
            {
                var context = new AdminDbContext();
                var tenderType = context.EntityTypes.FirstOrDefault(et => et.Code.Equals(3));
                var bankType = context.EntityTypes.FirstOrDefault(et => et.Code.Equals(9));
                var accountOwnerRelationType = context.RelationTypes.First(rt => rt.Code.Equals(11));
                var tenderCreatorType = context.RelationTypes.First(rt => rt.Code.Equals(5));


                var url = tenderUrl + "&page=" + request;
                yield return new ApiProgressModel { Message = "Start to collect data" };
                var tenderResultObject = APIClient.GetApiObject<Models.ETender.RootObject>(url);
                var totalRecords = tenderResultObject.rows.Count();
                yield return new ApiProgressModel { Message = "Got " + totalRecords + " records" };

                foreach (var row in tenderResultObject.rows)
                {
                    index++;
                    var progress = new ApiProgressModel();
                    if (!context.Entities.Any(e => e.TypeId.Equals(tenderType.Id) && e.Attributes.Any(attr => attr.Name.Equals("ETender Id") && attr.Value.Equals(row.id.ToString()))))
                    {
                        var entity = new Entity();
                        entity.TypeId = tenderType.Id;
                        entity.Name = row.tenderData.goodsDescr;
                        context.Entities.Add(entity);
                        yield return new ApiProgressModel { Message = "Creating new Tender entity " + entity.Name };
                        entity.Attributes.Add(new EntityAttribute() { Name = "ETender Id", Value = row.id.ToString() });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Tender type", Value = row.tenderType.mdValue ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Tender status at " + row.refCurrentStatusDate, Value = row.tenderStatus.mdValue });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Tender open date", Value = row.refTendeOpenDate ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Registration number ", Value = row.regNumber ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Goods", Value = row.tenderData.goods.mdValue ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Tender status", Value = row.tenderData.status.mdValue ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Language", Value = row.tenderData.language.mdValue ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Place offers at", Value = row.tenderData.pressOffersPlace ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Place offers until", Value = row.tenderData.pressOffersDate ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Open date time", Value = row.tenderData.deliveryTerms.mdValue ?? string.Empty });
                        entity.Attributes.Add(new EntityAttribute() { Name = "Notes", Value = row.notes ?? string.Empty });
                        entity.References.Add(new EntityReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/proceduricard?pid=" + row.id });

                        var entityCode = row.stateOrg.orgLegalForm != null ? row.stateOrg.orgLegalForm.id : 590;
                        var entityType = context.EntityTypes.FirstOrDefault(et => et.Code.Equals(entityCode));
                        if (entityType == null)
                        {
                            entityType = new EntityType() { Code = row.stateOrg.orgLegalForm.id, Name = row.stateOrg.orgLegalForm.mdValue };
                            context.EntityTypes.Add(entityType);
                            yield return new ApiProgressModel { Message = "New entity type added  " + entityType.Name};
                        }
                        var stateEntity = context.Entities.FirstOrDefault(e => e.Type.Code.Equals(entityType.Code) && e.Attributes.Any(attr => attr.Name.Equals("Code") && attr.Value.Equals(row.stateOrg.code)));

                        if (stateEntity == null)
                        {
                            stateEntity = new Entity();
                            stateEntity.Name = row.stateOrg.orgName;
                            stateEntity.Type = entityType;
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Code", Value = row.stateOrg.code ?? string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "ETender Id", Value = row.stateOrg.id.ToString() });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Address", Value = row.stateOrg.address ?? string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Account", Value = row.stateOrg.account ?? string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Treasuty account", Value = row.stateOrg.treasutyAcc ?? string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Fiscal code", Value = row.stateOrg.fiscalCode != null ? row.stateOrg.fiscalCode.ToString() : string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Account", Value = row.stateOrg.account ?? string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Bank account", Value = row.stateOrg.bankAccount!=null?row.stateOrg.bankAccount.ToString():string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Phone", Value = row.stateOrg.phone ?? string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Fax", Value = row.stateOrg.fax ?? string.Empty });
                            stateEntity.Attributes.Add(new EntityAttribute() { Name = "Email", Value = row.stateOrg.email ?? string.Empty });
                            stateEntity.References.Add(new EntityReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/proceduricard?pid=" + row.id });

                            context.Entities.Add(stateEntity);
                            yield return new ApiProgressModel { Message = "Creating " + entityType.Name + " " + stateEntity.Name };
                        }

                        if (row.stateOrg.bank != null)
                        {
                            var bank = context.Entities.FirstOrDefault(e => e.TypeId.Equals(bankType.Id) && e.Attributes.Any(attr => attr.Name.Equals("BIC") && attr.Value.Equals(row.stateOrg.bank.sapiCode)));
                            if (bank == null)
                            {
                                bank = new Entity();
                                bank.Name = row.stateOrg.bank.mdShort;
                                bank.Type = bankType;
                                bank.Attributes.Add(new EntityAttribute() { Name = "Short Name", Value = row.stateOrg.bank.mdShort ?? string.Empty });
                                bank.Attributes.Add(new EntityAttribute() { Name = "mfo Code", Value = row.stateOrg.bank.mfoCode ?? string.Empty });
                                bank.Attributes.Add(new EntityAttribute() { Name = "Fiscal Code", Value = row.stateOrg.bank.fiscalCode ?? string.Empty });
                                bank.Attributes.Add(new EntityAttribute() { Name = "BIC", Value = row.stateOrg.bank.sapiCode ?? string.Empty });
                                bank.References.Add(new EntityReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/proceduricard?pid=" + row.id });
                                context.Entities.Add(bank);
                                yield return new ApiProgressModel { Message = "Creating entity Bank" + bank.Name };
                            }
                            
                            if (!context.Relations.Any(r => r.TypeId.Equals(accountOwnerRelationType.Id) && r.RelationEntities.Any(re => re.EntityId.Equals(stateEntity.Id) && r.RelationEntities.Any(re1 => re1.EntityId.Equals(bank.Id)))))
                            {
                                var bankRelation = new Relation();
                                bankRelation.TypeId = accountOwnerRelationType.Id;
                                bankRelation.Type = accountOwnerRelationType;
                                bankRelation.RelationEntities.Add(new RelationEntity { Entity = bank });
                                bankRelation.RelationEntities.Add(new RelationEntity { Entity = stateEntity });
                                bankRelation.Attributes.Add(new RelationAttribute { Name = "Account", Value = row.stateOrg.account ?? string.Empty });
                                bankRelation.References.Add(new RelationReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/proceduricard?pid=" + row.id });
                                context.Relations.Add(bankRelation);
                            }
                        }
                        var relation = new Relation();
                        relation.Type = tenderCreatorType;
                        relation.RelationEntities.Add(new RelationEntity { Entity = stateEntity });
                        relation.RelationEntities.Add(new RelationEntity { Entity = entity });
                        relation.References.Add(new RelationReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/proceduricard?pid=" +row.id});
                        context.Relations.Add(relation);
                        context.SaveChanges();
                        yield return new ApiProgressModel { Message = index +" Saved!" };
                        
                    }
                    else
                    {
                        yield return new ApiProgressModel { Message="Tender with ETender Id = " + row.id };
                    }
                }
               
            }
            yield return new ApiProgressModel { Message = "Processing data finished !!!!" };
        }
        public static IEnumerable<ApiProgressModel> UpdateETenderContracts(int onPage, int requests)
        {
            var tenderUrl = "http://etender.gov.md/json/contractList?rows=" + onPage;
            var index = 0;
            for (var request = 1; request <= requests; request++)
            {
                var context = new AdminDbContext();
                var tenderType = context.EntityTypes.FirstOrDefault(et => et.Code == 3);
                var contractRelationType = context.RelationTypes.FirstOrDefault(et => et.Code == 1);
                var bankType = context.EntityTypes.FirstOrDefault(et => et.Code.Equals(9));
                var accountOwnerRelationType = context.RelationTypes.First(rt => rt.Code.Equals(11));
                var tenderCreatorType = context.RelationTypes.First(rt => rt.Code.Equals(5));

                var personType = context.EntityTypes.First(rt => rt.Code.Equals(1));
                var companyType = context.EntityTypes.First(rt => rt.Code.Equals(2));

                var url = tenderUrl + "&page=" + request;
                yield return new ApiProgressModel { Message = "Start to collect data" };
                var tenderResultObject = APIClient.GetApiObject<Models.ETender.Contracts.RootObject>(url);
                var totalRecords = tenderResultObject.rows.Count();
                yield return new ApiProgressModel { Message = "Got " + totalRecords + " records" };

                foreach (var row in tenderResultObject.rows)
                {
                    var progress = new ApiProgressModel();
                    if (!context.Relations.Any(e => e.TypeId.Equals(contractRelationType.Id) && e.Attributes.Any(attr => attr.Name.Equals("ETender Id") && attr.Value.Equals(row.id.ToString()))))
                    {
                        var contractRelation = new Relation();
                        contractRelation.TypeId = contractRelationType.Id;
                        context.Relations.Add(contractRelation);
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "Amount", Value = row.amount.ToString() });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "Contract type", Value = row.contractType.mdValue ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "Contract date", Value = row.contractDate ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "State", Value = row.status.mdValue ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "State date", Value = row.currentStatusDate ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "Contract number", Value = row.contractNumber ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "ETender offer id", Value = row.fkOfferId.ToString() });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "ETender decision id", Value = row.currentStatusDate ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "Register date", Value = row.registerDate ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "Final date", Value = row.finalDate ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "Contract real number", Value = row.contractRealNumber ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "Note", Value = row.note ?? string.Empty });
                        contractRelation.Attributes.Add(new RelationAttribute() { Name = "ETender Id", Value = row.id.ToString() });
                        contractRelation.References.Add(new RelationReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/contracte" });

                        var stateOrgTypeCode = row.tender.stateOrg != null ? row.tender.stateOrg.orgLegalForm!=null?row.tender.stateOrg.orgLegalForm.id:590 : 590;
                        var stateOrgType = context.EntityTypes.FirstOrDefault(et => et.Code.Equals(stateOrgTypeCode));
                        if (stateOrgType == null)
                        {
                            stateOrgType = new EntityType() { Code = row.tender.stateOrg.orgLegalForm.id, Name = row.tender.stateOrg.orgLegalForm.mdValue };
                            context.EntityTypes.Add(stateOrgType);
                            yield return new ApiProgressModel { Message = "New entity type added  " + stateOrgType.Name };
                        }
                        var stateOrgEntity = context.Entities.FirstOrDefault(e => e.Type.Code.Equals(stateOrgType.Code) && e.Attributes.Any(attr => attr.Name.Equals("Code") && attr.Value.Equals(row.tender.stateOrg.code)));
                        if (stateOrgEntity == null)
                        {
                            stateOrgEntity = new Entity();
                            stateOrgEntity.Name = row.tender.stateOrg.orgName;
                            stateOrgEntity.Type = stateOrgType;
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Code", Value = row.tender.stateOrg.code ?? string.Empty });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "ETender Id", Value = row.tender.stateOrg.id.ToString() });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Address", Value = row.tender.stateOrg.address ?? string.Empty });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Account", Value = row.tender.stateOrg.account ?? string.Empty });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Treasuty account", Value = row.tender.stateOrg.treasutyAcc ?? string.Empty });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Fiscal code", Value = row.tender.stateOrg.fiscalCode != null ? row.tender.stateOrg.fiscalCode.ToString() : string.Empty });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Account", Value = row.tender.stateOrg.account ?? string.Empty });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Bank account", Value =row.tender.stateOrg.bankAccount!=null?row.tender.stateOrg.bankAccount.ToString() : string.Empty });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Phone", Value = row.tender.stateOrg.phone ?? string.Empty });
                            stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Fax", Value = row.tender.stateOrg.fax ?? string.Empty });
                            if (row.addAccordReason != null)
                            {
                                stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Accord reason", Value = row.addAccordReason.ToString() });
                            }
                            if (row.addAccordReasonDocuments != null)
                            {
                                stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Accord reason", Value = row.addAccordReasonDocuments.ToString() });
                            }
                            if (row.addAccordShortDescr != null)
                            {
                                stateOrgEntity.Attributes.Add(new EntityAttribute() { Name = "Accord reason short description", Value = row.addAccordShortDescr.ToString() });
                            }

                            stateOrgEntity.References.Add(new EntityReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/contracte" });
                            context.Entities.Add(stateOrgEntity);

                            yield return new ApiProgressModel { Message = "Creating " + stateOrgType.Name + " " + stateOrgEntity.Name };
                        }

                        if (row.tender.stateOrg.bank != null)
                        {
                            var bank = context.Entities.FirstOrDefault(e => e.TypeId.Equals(bankType.Id) && e.Attributes.Any(attr => attr.Name.Equals("BIC") && attr.Value.Equals(row.tender.stateOrg.bank.sapiCode)));
                            if (bank == null)
                            {
                                bank = new Entity();
                                bank.Name = row.tender.stateOrg.bank.mdShort;
                                bank.Type = bankType;
                                bank.Attributes.Add(new EntityAttribute() { Name = "Short Name", Value = row.tender.stateOrg.bank.mdShort ?? string.Empty });
                                bank.Attributes.Add(new EntityAttribute() { Name = "mfo Code", Value = row.tender.stateOrg.bank.mfoCode ?? string.Empty });
                                bank.Attributes.Add(new EntityAttribute() { Name = "Fiscal Code", Value = row.tender.stateOrg.bank.fiscalCode ?? string.Empty });
                                bank.Attributes.Add(new EntityAttribute() { Name = "BIC", Value = row.tender.stateOrg.bank.sapiCode ?? string.Empty });
                                bank.References.Add(new EntityReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/contracte" });
                                context.Entities.Add(bank);
                                yield return new ApiProgressModel { Message = "Creating entity Bank " + bank.Name };
                            }

                            if (!context.Relations.Any(r => r.TypeId.Equals(accountOwnerRelationType.Id) && r.RelationEntities.Any(re => re.EntityId.Equals(stateOrgEntity.Id) && r.RelationEntities.Any(re1 => re1.EntityId.Equals(bank.Id)))))
                            {
                                var bankRelation = new Relation();
                                bankRelation.TypeId = accountOwnerRelationType.Id;
                                bankRelation.Type = accountOwnerRelationType;
                                bankRelation.RelationEntities.Add(new RelationEntity { Entity = bank, MemberType= 4});
                                bankRelation.RelationEntities.Add(new RelationEntity { Entity = stateOrgEntity, MemberType = 3 });
                                bankRelation.Attributes.Add(new RelationAttribute { Name = "Account", Value = row.tender.stateOrg.account ?? string.Empty });
                                bankRelation.References.Add(new RelationReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/contracte" });
                                context.Relations.Add(bankRelation);
                            }
                        }

                        var tenderEntity = context.Entities.FirstOrDefault(e => e.TypeId.Equals(tenderType.Id) && e.Attributes.Any(attr => attr.Name.Equals("ETender Id") && attr.Value.Equals(row.tender.id.ToString())));
                        if (tenderEntity == null)
                        {
                            tenderEntity = new Entity();
                            tenderEntity.TypeId = tenderType.Id;
                            tenderEntity.Name = row.tender.tenderData.goodsDescr;
                            context.Entities.Add(tenderEntity);
                            yield return new ApiProgressModel { Message = "Creating new Tender entity " + tenderEntity.Name };
                            tenderEntity.Attributes.Add(new EntityAttribute() { Name = "ETender Id", Value = row.id.ToString() });
                            tenderEntity.Attributes.Add(new EntityAttribute() { Name = "Registration number ", Value = row.tender.regNumber ?? string.Empty });
                            tenderEntity.Attributes.Add(new EntityAttribute() { Name = "Goods", Value = row.tender.tenderData.goodsDescr ?? string.Empty });
                            tenderEntity.References.Add(new EntityReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/contracte" });
                            context.Entities.Add(tenderEntity);
                            var relation = new Relation();

                            relation.Type = tenderCreatorType;
                            relation.RelationEntities.Add(new RelationEntity { Entity = stateOrgEntity });
                            relation.RelationEntities.Add(new RelationEntity { Entity = tenderEntity });
                            context.Relations.Add(relation);
                        }

                        var seller = context.Entities.FirstOrDefault(e => e.TypeId.Equals(companyType.Id) && e.Name.Equals( row.participant.fullName));
                        if (seller == null)
                        {
                            seller = context.Entities.FirstOrDefault(e => e.TypeId.Equals(personType.Id) && e.Name.Equals(row.participant.fullName));
                        }
                        if (seller == null)
                        {
                            seller = new Entity();
                            
                            seller.Name = row.participant.fullName;
                            seller.Attributes.Add(new EntityAttribute() { Name = "ETender Id", Value = row.participant.id.ToString() });
                            if (row.participant.juristicPerson == 1)
                            {
                                seller.Attributes.Add(new EntityAttribute() { Name = "Juridical name", Value = row.participant.jurName ?? string.Empty });
                                seller.Type = companyType;
                            }
                            else {
                                seller.Type = personType;
                                seller.Attributes.Add(new EntityAttribute() { Name = "First name", Value = row.participant.name ?? string.Empty });
                                seller.Attributes.Add(new EntityAttribute() { Name = "Last name", Value = row.participant.lastName ?? string.Empty });
                                seller.Attributes.Add(new EntityAttribute() { Name = "Patronymic", Value = row.participant.patronymic ?? string.Empty });
                            }
                            seller.References.Add(new EntityReference() { Title = "Registrul de stat al achizițiilor publice", Link = "http://etender.gov.md/contracte" });
                            context.Entities.Add(seller);
                        }
                        
                        contractRelation.RelationEntities.Add(new RelationEntity { Entity = tenderEntity });
                        contractRelation.RelationEntities.Add(new RelationEntity { Entity = stateOrgEntity, MemberType = 1 });
                        contractRelation.RelationEntities.Add(new RelationEntity { Entity = seller, MemberType = 2 });
                        context.Relations.Add(contractRelation);
                        context.SaveChanges();
                        index++;
                        yield return new ApiProgressModel { Message = index+ "  Saved!!!" };
                    }
                    else
                    {
                        yield return new ApiProgressModel { Message = "Contract with ETender Id = " + row.id + " exists" };
                    }
                }

            }
            yield return new ApiProgressModel { Message = "Processing data finished !!!!" };
        }
    }
}
