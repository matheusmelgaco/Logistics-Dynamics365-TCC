if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.Cotacao) === "undefined") Logistics.Cotacao = {};

Logistics.Cotacao = {
    isOnLoadComplete: false,

    consultarCEP: function (cepValue, formContext, isBillingAddress) {
        console.log("Consultando CEP: ", cepValue, " IsBillingAddress: ", isBillingAddress);
        var globalContext = Xrm.Utility.getGlobalContext();
        var serverURL = globalContext.getClientUrl();
        var actionName = "lgs_ConsultaCep";

        var req = new XMLHttpRequest();
        req.open("POST", serverURL + "/api/data/v9.2/" + actionName, true);
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("Accept", "application/json");

        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                req.onreadystatechange = null;
                if (this.status === 200 || this.status === 204) {
                    var result = JSON.parse(this.response);
                    console.log("Resposta completa do servidor:", result);

                    // Usando formatCep para o CEP formatado
                    var formattedCep = result["formatCep"];
                    console.log("Atualizando campo CEP com:", formattedCep);
                    var fieldPrefix = isBillingAddress ? "billto_" : "shipto_";

                    // Ajuste para usar os nomes corretos das propriedades da resposta
                    formContext.getAttribute(isBillingAddress ? "lgs_cep" : "lgs_cep1").setValue(formattedCep);
                    formContext.getAttribute(fieldPrefix + "line1").setValue(result["rua"]);
                    formContext.getAttribute(fieldPrefix + "line2").setValue(result["bairro"]);
                    formContext.getAttribute(fieldPrefix + "city").setValue(result["cidade"]);
                    formContext.getAttribute(fieldPrefix + "stateorprovince").setValue(result["estado"]);
                    formContext.getAttribute(fieldPrefix + "country").setValue("Brasil");
                } else {
                    console.error("Falha na requisição AJAX, status: ", this.status);
                    Xrm.Utility.alertDialog("Não foi possível consultar o CEP. Por favor, tente novamente.");
                }
            }
        };
        req.send(JSON.stringify({ cep: cepValue }));
    },

    OnChangeCEPShipping: function (context) {
        console.log("OnChangeCEPShipping chamado");
        var formContext = context.getFormContext();
        var cepField = formContext.getAttribute("lgs_cep1");
        var cepValue = cepField.getValue();

        if (cepValue) {
            cepValue = cepValue.replace("-", "");
            if (/^([0-9]{8})$/.test(cepValue)) {
                this.consultarCEP(cepValue, formContext, false);
            } else {
                Xrm.Utility.alertDialog("CEP inválido! Insira um CEP válido.");
                cepField.setValue(null);
            }
        }
    },

    OnChangeCEPBilling: function (context) {
        console.log("OnChangeCEPBilling chamado");
        var formContext = context.getFormContext();
        var cepField = formContext.getAttribute("lgs_cep");
        var cepValue = cepField.getValue();

        if (cepValue) {
            cepValue = cepValue.replace("-", "");
            if (/^([0-9]{8})$/.test(cepValue)) {
                this.consultarCEP(cepValue, formContext, true);
            } else {
                Xrm.Utility.alertDialog("CEP inválido! Insira um CEP válido.");
                cepField.setValue(null);
            }
        }
    }
};
