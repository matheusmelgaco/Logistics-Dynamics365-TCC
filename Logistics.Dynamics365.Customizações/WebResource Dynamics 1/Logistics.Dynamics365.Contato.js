if (typeof Logistics === "undefined") Logistics = {};
if (typeof Logistics.Contato === "undefined") Logistics.Contato = {};

Logistics.Contato = {
    isOnLoadComplete: false,

    validaCPF: function (cpf) {
        cpf = cpf.replace(/[^\d]+/g, '');
        if (cpf == '') return false;
        if (cpf.length != 11 || /^(\d)\1+$/.test(cpf)) return false;

        for (let t = 9; t < 11; t++) {
            let d = 0;
            for (let c = 0; c < t; c++) {
                d += cpf[c] * ((t + 1) - c);
            }
            d = ((10 * d) % 11) % 10;
            if (cpf[t] != d) {
                return false;
            }
        }
        return true;
    },
    onNameChange: function (context) {

        var formContext = context.getFormContext ? context.getFormContext() : context;
        var campoFirstName = formContext.getAttribute("firstname");
        var campoLastName = formContext.getAttribute("lastname");

        if (campoFirstName) {
            var nomeFirstName = campoFirstName.getValue();
            if (nomeFirstName) {
                var nomeFormatadoFirstName = nomeFirstName.toLowerCase().split(' ').map(function (palavra) {
                    return palavra.charAt(0).toUpperCase() + palavra.slice(1);
                }).join(' ');
                campoFirstName.setValue(nomeFormatadoFirstName);
            }
        }

        if (campoLastName) {
            var nomeLastName = campoLastName.getValue();
            if (nomeLastName) {
                var nomeFormatadoLastName = nomeLastName.toLowerCase().split(' ').map(function (palavra) {
                    return palavra.charAt(0).toUpperCase() + palavra.slice(1);
                }).join(' ');
                campoLastName.setValue(nomeFormatadoLastName);
            }
        }
    },


    formatarCPF: function (context, campoId) {
        var formContext = context.getFormContext ? context.getFormContext() : context;
        var campoCPF = formContext.getAttribute(campoId);
        var cpfValue = campoCPF.getValue();
        if (cpfValue) {
            var cpf = cpfValue.replace(/\D/g, '');

            if (cpf.length === 11) {
                cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
                campoCPF.setValue(cpf);
            }
        }
    },


    onCpfChange: function (context) {
        var formContext = context.getFormContext ? context.getFormContext() : context;
        var campoCPF = formContext.getAttribute("lgs_cpf");
        if (campoCPF) {
            var valorCPF = campoCPF.getValue();
            if (valorCPF) {
                valorCPF = valorCPF.replace(/\D/g, '');
                console.log("Valor do CPF antes da validação: ", valorCPF);

                if (!/^\d{11}$/.test(valorCPF)) {
                    console.log("CPF inválido ou formato incorreto, mostrando alerta.");
                    Logistics.Util.Alerta("O CPF informado é inválido ou está no formato incorreto.");
                    campoCPF.setValue(""); 
                    return;
                }

                if (!this.validaCPF(valorCPF)) {
                    console.log("CPF inválido, mostrando alerta.");
                    Logistics.Util.Alerta("O CPF informado é inválido.");
                    campoCPF.setValue("");
                } else {
                    console.log("CPF válido, formatando.");
                    this.formatarCPF(context, "lgs_cpf");
                }
            } else {
                console.log("Campo CPF é nulo ou vazio.");
                campoCPF.setValue("");
            }
        }
    },




};
