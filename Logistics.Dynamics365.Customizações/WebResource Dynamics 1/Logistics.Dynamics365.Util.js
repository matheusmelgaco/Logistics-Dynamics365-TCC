if (typeof Logistics === "undefined") Logistics = {};
if (typeof Logistics.Util === "undefined") Logistics.Util = {};

Logistics.Util = {

    Alerta: function (descricao, titulo) {

        const configuracaoTexto = {
            confirmButtonLabel: "OK",
            tittle: titulo,
            text: descricao
        };

        const configuracaoOpcoes = {
            height: 200,
            width: 300
        };


        Xrm.Navigation.openAlertDialog(configuracaoTexto, configuracaoOpcoes);

    }

};