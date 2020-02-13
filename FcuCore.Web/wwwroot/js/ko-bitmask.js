ko.bindingHandlers.bitmask = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        const mask = allBindings.get('bit');

        $(element).change(() => {
            let value = ko.unwrap(valueAccessor());
            if ($(element).prop('checked')) {
                value = value | mask;
            } else {
                let invertedMask = 0xFF ^ mask;
                value = value & invertedMask;
            }
            valueAccessor()(value);
        });
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        const mask = allBindings.get('bit');
        const value = ko.unwrap(valueAccessor());
        $(element).prop('checked', (value & mask) === mask);
    }
};