if (typeof (Logistics) === "undefined") Logistics = {} 
if (typeof (Logistics.Cep) === "undefined") Logistics.Cep = {} 


Logistics.Cep = {
    OnChangeCEP: function (context) {

        const formContext = context.getFormContext() 

        const cep = formContext.getAttribute("lgs_cep") 
        const rua = formContext.getAttribute("lgs_rua") 
        const bairro = formContext.getAttribute("lgs_bairro")
        const cidade = formContext.getAttribute("lgs_cidade")
        const estado = formContext.getAttribute("lgs_uf")
        const ibge = formContext.getAttribute("lgs_ibge")
        const ddd = formContext.getAttribute("lgs_ddd")

        let formatCep = ""
	if (cep.getValue() != null) {
		formatCep = cep.getValue().replace("-", "") 
	}

        const validCepNumbers = /^(([0-9]{8}))$/ 

        if (validCepNumbers.test(formatCep) == true) {

            var globalContext = Xrm.Utility.getGlobalContext();
            var serverURL = globalContext.getClientUrl();

            var actionName = "lgs_ConsultaCep"

            var parameters = {};
            parameters.cep = formatCep

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

                        cep.setValue(result["formatCep"])
                        rua.setValue(result["rua"])
                        bairro.setValue(result["bairro"])
                        cidade.setValue(result["cidade"])
                        estado.setValue(result["estado"])
                        ibge.setValue(result["ibge"])
                        ddd.setValue(result["ddd"])
                       
                    } else {

                        Logistics.Util.Alerta("Não foi possível consultar o CEP", "Por favor, tente novamente.")

                    }
                }
            };
            req.send(JSON.stringify(parameters));

        } else if (validCepNumbers.test(formatCep) == false) {

            Logistics.Util.Alerta("CEP invalido!", "Insira um CEP valido.")
            cep.setValue(null)

        }
    },
} 



