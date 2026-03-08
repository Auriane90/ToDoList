sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/json/JSONModel"
], function (Controller, JSONModel) {

    "use strict";

    return Controller.extend("todo.controller.Detail", {

        onInit: function () {

            const router = sap.ui.core.UIComponent.getRouterFor(this);

            router.getRoute("detail").attachPatternMatched(this._onObjectMatched, this);

        },

        _onObjectMatched: function (oEvent) {

            const id = oEvent.getParameter("arguments").id;

            fetch(`http://localhost:5198/todos/${id}`)
                .then(res => res.json())
                .then(data => {

                    const model = new JSONModel(data);
                    this.getView().setModel(model, "detail");

                });

        },

        onNavBack: function () {

            const router = sap.ui.core.UIComponent.getRouterFor(this);
            router.navTo("main");

        }

    });

});