/* Site-wide settings. Change these when the hosting or the download moves -
   and bump the ?v= on this file in index.html and docs/index.html, or
   visitors keep their cached copy for a while. */
window.ELINTRIA_SITE = {
  name: "Elintria Engine",
  version: "2026.11",
  // The GitHub repository. Its Releases page carries the engine download.
  repo: "https://github.com/TrexioasXavier/Elintria-Website",
  // The newest release (GitHub redirects /releases/latest to it).
  download: "https://github.com/TrexioasXavier/Elintria-Website/releases/latest",
  releases: "https://github.com/TrexioasXavier/Elintria-Website/releases",
  site: "https://trexioasxavier.github.io/Elintria-Website/"
};

/* Wire every [data-link] element to the matching setting. */
document.addEventListener("DOMContentLoaded", function () {
  var cfg = window.ELINTRIA_SITE;
  document.querySelectorAll("[data-link]").forEach(function (a) {
    var key = a.getAttribute("data-link");
    if (cfg[key]) a.setAttribute("href", cfg[key]);
  });
  document.querySelectorAll("[data-text]").forEach(function (el) {
    var key = el.getAttribute("data-text");
    if (cfg[key]) el.textContent = cfg[key];
  });
});
