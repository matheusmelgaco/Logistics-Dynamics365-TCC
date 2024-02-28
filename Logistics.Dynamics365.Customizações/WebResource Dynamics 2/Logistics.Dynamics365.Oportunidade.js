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
            formContext.ui.setFormNotification("Este é um registro de origem de integração e não pode ser editado.", "INFO", "integracaoOrigemNotificacao");
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