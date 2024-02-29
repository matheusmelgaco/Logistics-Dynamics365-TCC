if (typeof Logistics === "undefined") Logistics = {};
if (typeof Logistics.Conta === "undefined") Logistics.Conta = {};

Logistics.Conta = {
    isOnLoadComplete: false,

    validaCNPJ: function (cnpj) {
        console.log("Validando CNPJ: ", cnpj);
        cnpj = cnpj.replace(/[^\d]+/g, '');

        if (cnpj == '') return false;
        if (cnpj.length != 14) return false;
        if (/^(\d)\1+$/.test(cnpj)) return false;

        let tamanho = cnpj.length - 2;
        let numeros = cnpj.substring(0, tamanho);
        let digitos = cnpj.substring(tamanho);
        let soma = 0;
        let pos = tamanho - 7;

        for (let i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2) pos = 9;
        }

        let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0)) return false;

        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (let i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2) pos = 9;
        }

        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        const isValid = resultado == digitos.charAt(1);
        console.log("CNPJ é válido? ", isValid);
        return isValid;
    },

    formatarCNPJ: function (context, campoId) {
        console.log("Formatando CNPJ para o campo: ", campoId);
        var formContext = context.getFormContext ? context.getFormContext() : context;
        var campoCNPJ = formContext.getAttribute(campoId);
        var cnpj = campoCNPJ.getValue().replace(/\D/g, '');

        if (cnpj.length === 14) {
            cnpj = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, "$1.$2.$3/$4-$5");
            campoCNPJ.setValue(cnpj);
        }
        console.log("CNPJ formatado: ", cnpj);
    },

    onCnpjChange: function (context) {
      
        var formContext = context.getFormContext ? context.getFormContext() : context;
        var campoCNPJ = formContext.getAttribute("lgs_cnpj");
        var valorCNPJ = campoCNPJ.getValue().replace(/\D/g, '');
        console.log("Valor do CNPJ antes da validação: ", valorCNPJ);

        if (valorCNPJ && !this.validaCNPJ(valorCNPJ)) {
            console.log("CNPJ inválido, mostrando alerta.");
            Logistics.Util.Alerta("O CNPJ informado é inválido.");
            campoCNPJ.setValue("");
        } else {
            console.log("CNPJ válido, formatando.");
            this.formatarCNPJ(context, "lgs_cnpj");
        }
    },
    onNameChange: function (context) {
        var formContext = context.getFormContext ? context.getFormContext() : context;
        var campoNomeConta = formContext.getAttribute("name");
        var nomeConta = campoNomeConta.getValue();

        if (!nomeConta) return;

        var nomeFormatado = nomeConta.toLowerCase().split(' ').map(function (palavra) {
            return palavra.charAt(0).toUpperCase() + palavra.slice(1);
        }).join(' ');

        campoNomeConta.setValue(nomeFormatado);
    },
};
