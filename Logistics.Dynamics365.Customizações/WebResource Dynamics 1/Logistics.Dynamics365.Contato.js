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
        if (campoCPF && campoCPF.getValue()) {
            var valorCPF = campoCPF.getValue().replace(/\D/g, '');
            console.log("Valor do CPF antes da validação: ", valorCPF);

            if (valorCPF && !this.validaCPF(valorCPF)) {
                console.log("CPF inválido, mostrando alerta.");
                Logistics.Util.Alerta("O CPF informado é inválido.");
                campoCPF.setValue("");
            } else {
                console.log("CPF válido, formatando.");
                this.formatarCPF(context, "lgs_cpf");
            }
        } else {
            console.log("Campo CPF é nulo ou vazio.");
        }
    },

};

