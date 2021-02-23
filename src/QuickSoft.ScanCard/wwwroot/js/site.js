function _ajax_request(url, data, callback, method) {
    return $.ajax({
        url:  url,
        type: method,
        data: data,
        success: callback
    });
}

$.extend({
    put: function (url, data, callback) {
        return _ajax_request(url, data, callback, 'PUT');
    }
});
$.extend({
    delete: function (url, data, callback) {
        return _ajax_request(url, data, callback, 'DELETE');
    }
});
$.extend({
    post: function (url, data, callback) {
        return _ajax_request(url, data, callback, 'POST');
    }
});
$.extend({
    get: function (url, data, callback) {
        return _ajax_request(url, data, callback, 'GET');
    }
});

$(document).ajaxStart(function() { Pace.restart(); });

$.extend({
    spinner: function (displayOnPage) {
        let spinner = $('#spinner');
        let spinnerClass = 'lds-roller';
        if (displayOnPage) {
            let divLength = 8;
            for (let i = 0; i < divLength; i++) {
                spinner.append($('<div>'));
            }
            spinner.before($('<div class="overlay">'))
            spinner.addClass(spinnerClass);
        } else {
            spinner.removeClass(spinnerClass);
            spinner.html('');
            spinner.siblings('.overlay').remove();
        }
    }
});

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

window.addEventListener('DOMContentLoaded', function() {
    console.log('window - DOMContentLoaded - capture'); // 1st
}, true);
document.addEventListener('DOMContentLoaded', function() {
    console.log('document - DOMContentLoaded - capture'); // 2nd
}, true);
document.addEventListener('DOMContentLoaded', function() {
    console.log('document - DOMContentLoaded - bubble'); // 2nd
});
window.addEventListener('DOMContentLoaded', function() {
    console.log('window - DOMContentLoaded - bubble'); // 3rd
});

window.addEventListener('load', function() {
    console.log('window - load - capture'); // 4th
}, true);
document.addEventListener('load', function(e) {
    /* Filter out load events not related to the document */
    if(['style','script'].indexOf(e.target.tagName.toLowerCase()) < 0)
        console.log('document - load - capture'); // DOES NOT HAPPEN
}, true);
document.addEventListener('load', function() {
    console.log('document - load - bubble'); // DOES NOT HAPPEN
});
window.addEventListener('load', function() {
    console.log('window - load - bubble'); // 4th
});

window.onload = function() {
    console.log('window - onload'); // 4th
};
document.onload = function() {
    console.log('document - onload'); // DOES NOT HAPPEN
};