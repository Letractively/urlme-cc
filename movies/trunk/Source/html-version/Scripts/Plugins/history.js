// grabbed from: http://plugins.jquery.com/project/history, TWEAKED BY ANDREW DUNCAN

/*
* jQuery history plugin
*
* Copyright (c) 2006 Taku Sano (Mikage Sawatari)
* Licensed under the MIT License:
*   http://www.opensource.org/licenses/mit-license.php
*
* Modified by Lincoln Cooper to add Safari support and only call the callback once during initialization
* for msie when no initial hash supplied.
* API rewrite by Lauris Buk?is-Haberkorns
*/

(function($) {

    function History() {
        this._curHash = '';
        this._callback = function(hash) { };
    };

    $.extend(History.prototype, {

        init: function(callback) {
            this._callback = callback;
            this._curHash = location.hash;

            this._callback(this._curHash.replace(/^#/, ''));
            setInterval(this._check, 100);
        },

        add: function(hash) {
            // This makes the looping function do something
            this._historyBackStack.push(hash);

            this._historyForwardStack.length = 0; // clear forwardStack (true click occured)
            this._isFirst = true;
        },

        _check: function() {
                var current_hash = location.hash;
                if (current_hash != $.history._curHash) {
                    $.history._curHash = current_hash;
                    $.history._callback(current_hash.replace(/^#/, ''));
                }
        },

        load: function(hash) {
            var newhash;
                newhash = '#' + hash;
                location.hash = newhash;
            this._curHash = newhash;
                this._callback(hash);
            }
    });

    $(document).ready(function() {
        $.history = new History(); // singleton instance
    });

})(jQuery);