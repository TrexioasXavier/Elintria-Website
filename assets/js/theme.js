/* Dark by default; the toggle stores the choice per browser. Loaded in <head> so the page never flashes the wrong theme. */
(function () {
  var KEY = "elintria-theme";
  function stored() { try { return localStorage.getItem(KEY); } catch (e) { return null; } }
  function save(v) { try { localStorage.setItem(KEY, v); } catch (e) { } }
  function apply(t) {
    document.documentElement.setAttribute("data-theme", t);
    document.querySelectorAll(".theme-toggle").forEach(function (b) {
      b.setAttribute("aria-label", t === "dark" ? "Switch to light theme" : "Switch to dark theme");
      b.setAttribute("title", t === "dark" ? "Light theme" : "Dark theme");
    });
  }
  apply(stored() === "light" ? "light" : "dark");
  window.ElintriaTheme = {
    toggle: function () {
      var next = document.documentElement.getAttribute("data-theme") === "dark" ? "light" : "dark";
      save(next); apply(next);
    }
  };
  document.addEventListener("DOMContentLoaded", function () {
    apply(document.documentElement.getAttribute("data-theme"));
    document.querySelectorAll(".theme-toggle").forEach(function (b) {
      b.addEventListener("click", window.ElintriaTheme.toggle);
    });
  });
})();
