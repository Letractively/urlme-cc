﻿/// Knockout Mapping plugin v2.3.2
/// (c) 2012 Steven Sanderson, Roy Jacobs - http://knockoutjs.com/
/// License: MIT (http://www.opensource.org/licenses/mit-license.php)
(function (d) { "function" === typeof require && "object" === typeof exports && "object" === typeof module ? d(require("knockout"), exports) : "function" === typeof define && define.amd ? define(["knockout", "exports"], d) : d(ko, ko.mapping = {}) })(function (d, e) {
    function w(b, c) {
        for (var a in c) if (c.hasOwnProperty(a) && c[a]) if (a && b[a] && "array" !== e.getType(b[a])) w(b[a], c[a]); else if ("array" === e.getType(b[a]) && "array" === e.getType(c[a])) {
            for (var d = b, g = a, i = b[a], r = c[a], t = {}, h = i.length - 1; 0 <= h; --h) t[i[h]] = i[h]; for (h = r.length - 1; 0 <= h; --h) t[r[h]] =
            r[h]; i = []; r = void 0; for (r in t) i.push(t[r]); d[g] = i
        } else b[a] = c[a]
    } function D(b, c) { var a = {}; w(a, b); w(a, c); return a } function A(b, c) { for (var a = D({}, b), d = K.length - 1; 0 <= d; d--) { var e = K[d]; a[e] && (a[""] instanceof Object || (a[""] = {}), a[""][e] = a[e], delete a[e]) } c && (a.ignore = p(c.ignore, a.ignore), a.include = p(c.include, a.include), a.copy = p(c.copy, a.copy)); a.ignore = p(a.ignore, j.ignore); a.include = p(a.include, j.include); a.copy = p(a.copy, j.copy); a.mappedProperties = a.mappedProperties || {}; return a } function p(b, c) {
        "array" !==
        e.getType(b) && (b = "undefined" === e.getType(b) ? [] : [b]); "array" !== e.getType(c) && (c = "undefined" === e.getType(c) ? [] : [c]); return d.utils.arrayGetDistinctValues(b.concat(c))
    } function E(b, c, a, f, g, i, r) {
        var t = "array" === e.getType(d.utils.unwrapObservable(c)), i = i || ""; if (e.isMapped(b)) var h = d.utils.unwrapObservable(b)[q], a = D(h, a); var m = function () { return a[f] && a[f].create instanceof Function }, j = function (b) {
            var e = F, h = d.dependentObservable; d.dependentObservable = function (a, b, c) {
                c = c || {}; a && "object" == typeof a && (c = a);
                var f = c.deferEvaluation, L = !1; c.deferEvaluation = !0; a = new G(a, b, c); if (!f) { var g = a, f = d.dependentObservable; d.dependentObservable = G; a = d.isWriteableObservable(g); d.dependentObservable = f; a = G({ read: function () { L || (d.utils.arrayRemoveItem(e, g), L = !0); return g.apply(g, arguments) }, write: a && function (a) { return g(a) }, deferEvaluation: !0 }); e.push(a) } return a
            }; d.dependentObservable.fn = G.fn; d.computed = d.dependentObservable; b = d.utils.unwrapObservable(g) instanceof Array ? a[f].create({ data: b || c, parent: r, skip: M }) : a[f].create({
                data: b ||
                c, parent: r
            }); d.dependentObservable = h; d.computed = d.dependentObservable; return b
        }, x = function () { return a[f] && a[f].update instanceof Function }, y = function (b, e) { var g = { data: e || c, parent: r, target: d.utils.unwrapObservable(b) }; d.isWriteableObservable(b) && (g.observable = b); return a[f].update(g) }; if (h = H.get(c)) return h; f = f || ""; if (t) {
            var t = [], s = !1, k = function (a) { return a }; a[f] && a[f].key && (k = a[f].key, s = !0); d.isObservable(b) || (b = d.observableArray([]), b.mappedRemove = function (a) {
                var c = typeof a == "function" ? a : function (b) {
                    return b ===
                    k(a)
                }; return b.remove(function (a) { return c(k(a)) })
            }, b.mappedRemoveAll = function (a) { var c = B(a, k); return b.remove(function (a) { return d.utils.arrayIndexOf(c, k(a)) != -1 }) }, b.mappedDestroy = function (a) { var c = typeof a == "function" ? a : function (b) { return b === k(a) }; return b.destroy(function (a) { return c(k(a)) }) }, b.mappedDestroyAll = function (a) { var c = B(a, k); return b.destroy(function (a) { return d.utils.arrayIndexOf(c, k(a)) != -1 }) }, b.mappedIndexOf = function (a) { var c = B(b(), k), a = k(a); return d.utils.arrayIndexOf(c, a) }, b.mappedCreate =
            function (a) { if (b.mappedIndexOf(a) !== -1) throw Error("There already is an object with the key that you specified."); var c = m() ? j(a) : a; if (x()) { a = y(c, a); d.isWriteableObservable(c) ? c(a) : c = a } b.push(c); return c }); var h = B(d.utils.unwrapObservable(b), k).sort(), n = B(c, k); s && n.sort(); var s = d.utils.compareArrays(h, n), h = {}, p, z = d.utils.unwrapObservable(c), v = {}, w = !0, n = 0; for (p = z.length; n < p; n++) { var o = k(z[n]); if (void 0 === o || o instanceof Object) { w = !1; break } v[o] = z[n] } var z = [], A = 0, n = 0; for (p = s.length; n < p; n++) {
                var o = s[n],
                l, u = i + "[" + n + "]"; switch (o.status) { case "added": var C = w ? v[o.value] : I(d.utils.unwrapObservable(c), o.value, k); l = E(void 0, C, a, f, b, u, g); m() || (l = d.utils.unwrapObservable(l)); u = N(d.utils.unwrapObservable(c), C, h); l === M ? A++ : z[u - A] = l; h[u] = !0; break; case "retained": C = w ? v[o.value] : I(d.utils.unwrapObservable(c), o.value, k); l = I(b, o.value, k); E(l, C, a, f, b, u, g); u = N(d.utils.unwrapObservable(c), C, h); z[u] = l; h[u] = !0; break; case "deleted": l = I(b, o.value, k) } t.push({ event: o.status, item: l })
            } b(z); a[f] && a[f].arrayChanged && d.utils.arrayForEach(t,
            function (b) { a[f].arrayChanged(b.event, b.item) })
        } else if (O(c)) { b = d.utils.unwrapObservable(b); if (!b) { if (m()) return s = j(), x() && (s = y(s)), s; if (x()) return y(s); b = {} } x() && (b = y(b)); H.save(c, b); if (x()) return b; P(c, function (f) { var e = i.length ? i + "." + f : f; if (-1 == d.utils.arrayIndexOf(a.ignore, e)) if (-1 != d.utils.arrayIndexOf(a.copy, e)) b[f] = c[f]; else { var g = H.get(c[f]), h = E(b[f], c[f], a, f, b, e, b), g = g || h; if (d.isWriteableObservable(b[f])) b[f](d.utils.unwrapObservable(g)); else b[f] = g; a.mappedProperties[e] = !0 } }) } else switch (e.getType(c)) {
            case "function": x() ?
            d.isWriteableObservable(c) ? (c(y(c)), b = c) : b = y(c) : b = c; break; default: if (d.isWriteableObservable(b)) return l = x() ? y(b) : d.utils.unwrapObservable(c), b(l), l; b = m() ? j() : d.observable(d.utils.unwrapObservable(c))
        } return b
    } function N(b, c, a) { for (var d = 0, e = b.length; d < e; d++) if (!0 !== a[d] && b[d] === c) return d; return null } function Q(b, c) { var a; c && (a = c(b)); "undefined" === e.getType(a) && (a = b); return d.utils.unwrapObservable(a) } function I(b, c, a) {
        for (var b = d.utils.unwrapObservable(b), f = 0, e = b.length; f < e; f++) {
            var i = b[f]; if (Q(i,
            a) === c) return i
        } throw Error("When calling ko.update*, the key '" + c + "' was not found!");
    } function B(b, c) { return d.utils.arrayMap(d.utils.unwrapObservable(b), function (a) { return c ? Q(a, c) : a }) } function P(b, c) { if ("array" === e.getType(b)) for (var a = 0; a < b.length; a++) c(a); else for (a in b) c(a) } function O(b) { var c = e.getType(b); return ("object" === c || "array" === c) && null !== b } function S() {
        var b = [], c = []; this.save = function (a, f) { var e = d.utils.arrayIndexOf(b, a); 0 <= e ? c[e] = f : (b.push(a), c.push(f)) }; this.get = function (a) {
            a =
            d.utils.arrayIndexOf(b, a); return 0 <= a ? c[a] : void 0
        }
    } function R() { var b = {}, c = function (a) { var c; try { c = a } catch (d) { c = "$$$" } a = b[c]; void 0 === a && (a = new S, b[c] = a); return a }; this.save = function (a, b) { c(a).save(a, b) }; this.get = function (a) { return c(a).get(a) } } var q = "__ko_mapping__", G = d.dependentObservable, J = 0, F, H, K = ["create", "update", "key", "arrayChanged"], M = {}, v = { include: ["_destroy"], ignore: [], copy: [] }, j = v; e.isMapped = function (b) { return (b = d.utils.unwrapObservable(b)) && b[q] }; e.fromJS = function (b) {
        if (0 == arguments.length) throw Error("When calling ko.fromJS, pass the object you want to convert.");
        window.setTimeout(function () { J = 0 }, 0); J++ || (F = [], H = new R); var c, a; 2 == arguments.length && (arguments[1][q] ? a = arguments[1] : c = arguments[1]); 3 == arguments.length && (c = arguments[1], a = arguments[2]); a && (c = D(c, a[q])); c = A(c); var d = E(a, b, c); a && (d = a); --J || window.setTimeout(function () { for (; F.length;) { var a = F.pop(); a && a() } }, 0); d[q] = D(d[q], c); return d
    }; e.fromJSON = function (b) { var c = d.utils.parseJson(b); arguments[0] = c; return e.fromJS.apply(this, arguments) }; e.updateFromJS = function () {
        throw Error("ko.mapping.updateFromJS, use ko.mapping.fromJS instead. Please note that the order of parameters is different!");
    }; e.updateFromJSON = function () { throw Error("ko.mapping.updateFromJSON, use ko.mapping.fromJSON instead. Please note that the order of parameters is different!"); }; e.toJS = function (b, c) {
        j || e.resetDefaultOptions(); if (0 == arguments.length) throw Error("When calling ko.mapping.toJS, pass the object you want to convert."); if ("array" !== e.getType(j.ignore)) throw Error("ko.mapping.defaultOptions().ignore should be an array."); if ("array" !== e.getType(j.include)) throw Error("ko.mapping.defaultOptions().include should be an array.");
        if ("array" !== e.getType(j.copy)) throw Error("ko.mapping.defaultOptions().copy should be an array."); c = A(c, b[q]); return e.visitModel(b, function (a) { return d.utils.unwrapObservable(a) }, c)
    }; e.toJSON = function (b, c) { var a = e.toJS(b, c); return d.utils.stringifyJson(a) }; e.defaultOptions = function () { if (0 < arguments.length) j = arguments[0]; else return j }; e.resetDefaultOptions = function () { j = { include: v.include.slice(0), ignore: v.ignore.slice(0), copy: v.copy.slice(0) } }; e.getType = function (b) {
        if (b && "object" === typeof b) {
            if (b.constructor ==
            (new Date).constructor) return "date"; if ("[object Array]" === Object.prototype.toString.call(b)) return "array"
        } return typeof b
    }; e.visitModel = function (b, c, a) {
        a = a || {}; a.visitedObjects = a.visitedObjects || new R; var f, g = d.utils.unwrapObservable(b); if (O(g)) a = A(a, g[q]), c(b, a.parentName), f = "array" === e.getType(g) ? [] : {}; else return c(b, a.parentName); a.visitedObjects.save(b, f); var i = a.parentName; P(g, function (b) {
            if (!(a.ignore && -1 != d.utils.arrayIndexOf(a.ignore, b))) {
                var j = g[b], h = a, m = i || ""; "array" === e.getType(g) ? i &&
                (m += "[" + b + "]") : (i && (m += "."), m += b); h.parentName = m; if (!(-1 === d.utils.arrayIndexOf(a.copy, b) && -1 === d.utils.arrayIndexOf(a.include, b) && g[q] && g[q].mappedProperties && !g[q].mappedProperties[b] && "array" !== e.getType(g))) switch (e.getType(d.utils.unwrapObservable(j))) { case "object": case "array": case "undefined": h = a.visitedObjects.get(j); f[b] = "undefined" !== e.getType(h) ? h : e.visitModel(j, c, a); break; default: f[b] = c(j, a.parentName) }
            }
        }); return f
    }
});