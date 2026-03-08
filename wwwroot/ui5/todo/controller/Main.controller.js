sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/json/JSONModel"
], function (Controller, JSONModel) {

    "use strict";

    return Controller.extend("todo.controller.Main", {

        onInit: function () {

            const oModel = new JSONModel({
                todos: [],
                page: 1,
                loading: false
            });

            this.getView().setModel(oModel, "main");

            this.pageSize = 10;

            this.loadTodos();

        },

        loadTodos: function (title = "") {

            const oModel = this.getView().getModel("main");
            const page = oModel.getProperty("/page");

            oModel.setProperty("/loading", true);

            let url = `http://localhost:5198/todos?page=${page}&pageSize=${this.pageSize}`;

            if (title) {
                url += `&title=${encodeURIComponent(title)}`;
            }

            fetch(url)
                .then(res => res.json())
                .then(data => {

                    oModel.setProperty("/todos", data);
                    oModel.setProperty("/loading", false);

                })
                .catch(() => {
                    oModel.setProperty("/loading", false);
                });

        },

        onNextPage: function () {

            const oModel = this.getView().getModel("main");
            let page = oModel.getProperty("/page");

            page++;

            oModel.setProperty("/page", page);

            this.loadTodos();

        },

        onPrevPage: function () {

            const oModel = this.getView().getModel("main");
            let page = oModel.getProperty("/page");

            if (page > 1) {
                page--;
                oModel.setProperty("/page", page);
                this.loadTodos();
            }

        },

        onSearch: function (oEvent) {

            const value = oEvent.getParameter("newValue");

            clearTimeout(this._searchTimeout);

            this._searchTimeout = setTimeout(() => {

                const oModel = this.getView().getModel("main");
                oModel.setProperty("/page", 1);

                this.loadTodos(value);

            }, 500);

        },
        onDetails: function (oEvent) {

            const oContext = oEvent.getSource().getBindingContext("main");
            const id = oContext.getProperty("id");

            const router = sap.ui.core.UIComponent.getRouterFor(this);

            router.navTo("detail", {
                id: id
            });

        }

    });
    

});
