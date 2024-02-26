if (typeof Logistics === "undefined") Logistics = {};
if (typeof Logistics.Oportunidade === "undefined") Logistics.Oportunidade = {};

Logistics.Oportunidade = {
    isOnLoadComplete: false,

    bloquearEdicaoSeIntegrada: function (context) {
        var formContext = context.getFormContext ? context.getFormContext() : context;
        var integradaAttr = formContext.getAttribute("lgs_integrada");

        if (!integradaAttr) {
            console.error("Campo 'lgs_integrada' não encontrado.");
            return;
        }

        var isIntegrada = integradaAttr.getValue();

        if (isIntegrada) {
            Logistics.Util.Alerta("Esta oportunidade está integrada e não pode ser editada, mas é possível visualiza-la", "Oportunidade integrada")
            var attributes = formContext.data.entity.attributes.get();
            attributes.forEach(function (attribute, i) {
                attribute.controls.forEach(function (control, j) {
                    control.setDisabled(true);
                });
            });
        }

        Logistics.Oportunidade.isOnLoadComplete = true;
    },

    onLoad: function (context) {
        Logistics.Oportunidade.bloquearEdicaoSeIntegrada(context);
    }
};