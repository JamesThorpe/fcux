ko.bindingHandlers.number = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        $(element).change(() => {
            valueAccessor()(parseInt($(element).val(), 10));
        });
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        const value = ko.unwrap(valueAccessor());
        $(element).val(value);
    }
};