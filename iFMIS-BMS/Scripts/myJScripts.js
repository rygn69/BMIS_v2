
jQuery.fn.serializeJSON = function () {
    var json = {};
    jQuery.map(jQuery(this).serializeArray(), function (n, i) {
        var _ = n.name.indexOf('[');
        if (_ > -1) {
            var o = json;
            _name = n.name.replace(/\]/gi, '').split('[');
            for (var i = 0, len = _name.length; i < len; i++) {
                if (i == len - 1) {
                    if (o[_name[i]]) {
                        if (typeof o[_name[i]] == 'string') {
                            o[_name[i]] = [o[_name[i]]];
                        }
                        o[_name[i]].push(n.value);
                    }
                    else o[_name[i]] = n.value || '';
                }
                else o = o[_name[i]] = o[_name[i]] || {};
            }
        }
        else {
            if (json[n.name] !== undefined) {
                if (!json[n.name].push) {
                    json[n.name] = [json[n.name]];
                }
                json[n.name].push(n.value || '');
            }
            else json[n.name] = n.value || '';
        }
    });
    return json;
};

jQuery.fn.NumericOnly = function () { return this.each(function () { $(this).keydown(function (a) { a = a.charCode || a.keyCode || 0; return 188 == a || 190 == a || 8 == a || 9 == a || 46 == a || 37 <= a && 40 >= a || 48 <= a && 57 >= a || 96 <= a && 105 >= a }) }) };
jQuery.fn.IntOnly = function () { return this.each(function () { $(this).keydown(function (a) { a = a.charCode || a.keyCode || 0; return 188 == a || 8 == a || 9 == a || 46 == a || 37 <= a && 40 >= a || 48 <= a && 57 >= a || 96 <= a && 105 >= a }) }) };
jQuery.fn.NumericNegOnly = function () { return this.each(function () { $(this).keydown(function (a) { a = a.charCode || a.keyCode || 0; return 188 == a || 190 == a || 189 == a || 8 == a || 9 == a || 46 == a || 37 <= a && 40 >= a || 48 <= a && 57 >= a || 96 <= a && 105 >= a }) }) };


$.fn["radiovalue"] = function () {
    var val = 0;
    val = $("input[name=" + $(this).attr("id") + "]:checked").val();

    return val;
}

