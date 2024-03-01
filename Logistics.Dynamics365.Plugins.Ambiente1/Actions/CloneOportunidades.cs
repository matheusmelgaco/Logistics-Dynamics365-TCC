using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Principal;

namespace Logisitcs.Dynamics365.Clone.Oportunidades
{
    public class CloneOportunidades : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            EntityReference opportunity = (EntityReference)context.InputParameters["Target"];
            var id = opportunity.Id.ToString();

            
            //var getOpportunity = service.Retrieve("opportunity", id, new ColumnSet(true));

            /*
            var clonedOpportunity = getOpportunity.Clone();
            clonedOpportunity = getOpportunity;
            clonedOpportunity.EntityState = null;
            clonedOpportunity.Attributes.Remove(getOpportunity.LogicalName + "id");
            clonedOpportunity.Id = Guid.NewGuid();
            service.Create(clonedOpportunity);
            */


            QueryExpression opportunityQuery = new QueryExpression("opportunity");
            opportunityQuery.ColumnSet.AddColumns(
                "name", 
                "parentcontactid",
                "budgetamount",
                "purchasetimeframe",
                "purchaseprocess",
                "msdyn_forecastcategory",
                "pricelevelid",
                "parentaccountid"
                );
            opportunityQuery.Criteria.AddCondition("opportunityid", ConditionOperator.Equal, id);
            EntityCollection filterOpportunity = service.RetrieveMultiple(opportunityQuery);

            Entity clonedOpportunity = new Entity("opportunity");
            clonedOpportunity.Id = Guid.NewGuid();
            clonedOpportunity["name"] = filterOpportunity.Entities.First()["name"] + "(Copy)";
            clonedOpportunity["parentcontactid"] = filterOpportunity.Entities.First()["parentcontactid"];
            clonedOpportunity["budgetamount"] = filterOpportunity.Entities.First()["budgetamount"];
            clonedOpportunity["purchasetimeframe"] = filterOpportunity.Entities.First()["purchasetimeframe"];
            clonedOpportunity["purchaseprocess"] = filterOpportunity.Entities.First()["purchaseprocess"];
            clonedOpportunity["msdyn_forecastcategory"] = filterOpportunity.Entities.First()["msdyn_forecastcategory"];
            clonedOpportunity["pricelevelid"] = filterOpportunity.Entities.First()["pricelevelid"];
            clonedOpportunity["parentaccountid"] = filterOpportunity.Entities.First()["parentaccountid"];
            var clonedOpportunityId = service.Create(clonedOpportunity);

            QueryExpression productQuery = new QueryExpression("opportunityproduct");
            productQuery.ColumnSet.AddColumns(
                "productid",
                "opportunityid",
                "ispriceoverridden",
                "uomid",
                "quantity",
                "manualdiscountamount",
                "tax",
                "parentbundleid"
                
                );
            productQuery.Criteria.AddCondition("opportunityid", ConditionOperator.Equal, id);
            EntityCollection filterProduct = service.RetrieveMultiple(productQuery);

            Entity clonedProduct = new Entity("opportunityproduct");
            clonedProduct.Id = Guid.NewGuid();
            clonedProduct["productid"] = filterProduct.Entities.First()["productid"];
            clonedProduct["opportunityid"] = new EntityReference("opportunity", clonedOpportunityId);
            clonedProduct["ispriceoverridden"] = filterProduct.Entities.First()["ispriceoverridden"];
            clonedProduct["uomid"] = filterProduct.Entities.First()["uomid"];
            clonedProduct["quantity"] = filterProduct.Entities.First()["quantity"];
            var clonedProductId = service.Create(clonedProduct);

            context.OutputParameters["newid"] = $"Opportunity id: {clonedOpportunityId}, Product id: {clonedProductId}";
        }
    }
}
