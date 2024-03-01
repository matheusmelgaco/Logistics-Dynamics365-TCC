if (typeof (Logistics) === "undefined") Logistics = {}
if (typeof (Logistics.Opportunity) === "undefined") Logistics.Opportunity = {} 


Logistics.Opportunity.Clone = {
    OnClickMain: function (context) {

        var formContext = primaryControl
        var id = formContext.getControl("opportunityid").getAttribute().getValue()
        var action = "alf_CloneOpportunity"

        var globalContext = Xrm.Utility.getGlobalContext()
        var serverURL = globalContext.getClientUrl()

        var req = new XMLHttpRequest()
        req.open(
            "POST",
            ` ${serverURL}/api/data/v9.2/opportunities(${id})/Microsoft.Dynamics.CRM.${action}`,
            true
        )
        req.setRequestHeader("OData-MaxVersion", "4.0")
        req.setRequestHeader("OData-Version", "4.0")
        req.setRequestHeader("Content-Type", "application/json  charset=utf-8")
        req.setRequestHeader("Accept", "application/json")
        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                req.onreadystatechange = null
                if (this.status === 200 || this.status === 204) {
                    var result = JSON.parse(this.response)
                    console.log(result)
                    var newid = result["newid"]
                } else {
                    console.log(this.responseText)
                }
            }
        }
        req.send()
        formContext.ui.setFormNotification( `Oportunidade clonada com o id: ${newid}`, "INFO" )
    }
} 



