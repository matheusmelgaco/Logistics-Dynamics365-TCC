if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.Cotacao) === "undefined") Logistics.Cotacao = {};

Logistics.Cotacao.OnChangeCEPBilling = function (context) {
    Logistics.Cotacao.OnChangeCEP(context, true);
};

Logistics.Cotacao.OnChangeCEPShipping = function (context) {
    Logistics.Cotacao.OnChangeCEP(context, false);
};

Logistics.Cotacao.OnChangeCEP = function (context, isBillingAddress) {
    const formContext = context.getFormContext();
    const cepFieldName = isBillingAddress ? "lgs_cep" : "lgs_cep1";
    const cepField = formContext.getAttribute(cepFieldName);
    let cepValue = cepField.getValue();

    if (cepValue) {
        cepValue = cepValue.replace("-", "");
        const validCepNumbers = /^(([0-9]{8}))$/;

        if (validCepNumbers.test(cepValue)) {
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

                        var formattedCep = result["cep"].substr(0, 5) + "-" + result["cep"].substr(5, 3);

                        const fieldPrefix = isBillingAddress ? "billto_" : "shipto_";

                       
                        cepField.setValue(formattedCep);
                        formContext.getAttribute(fieldPrefix + "line1").setValue(result["rua"]);
                        formContext.getAttribute(fieldPrefix + "line2").setValue(result["bairro"]);
                        formContext.getAttribute(fieldPrefix + "city").setValue(result["cidade"]);
                        formContext.getAttribute(fieldPrefix + "stateorprovince").setValue(result["estado"]);
                        formContext.getAttribute(fieldPrefix + "country").setValue("Brasil");

                    } else {
                        Xrm.Utility.alertDialog("Não foi possível consultar o CEP. Por favor, tente novamente.");
                    }
                }
            };
            req.send(JSON.stringify({ cep: cepValue }));
        } else {
            Xrm.Utility.alertDialog("CEP inválido! Insira um CEP válido.");
            cepField.setValue(null);
        }
    }
};
