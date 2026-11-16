/* Site-wide settings. Change these when the hosting or the download moves. */
window.ELINTRIA_SITE = {
  name: "Elintria Engine",
  version: "2026.11",
  // The public website repository on GitHub. Its Releases page carries the engine download.
  repo: "https://github.com/TrexioasXavier/elintria-website",
  // Latest release's ElintriaEngine.zip (GitHub redirects /latest/download/<file> to the newest release).
  download: "https://github.com/TrexioasXavier/elintria-website/releases/latest/download/ElintriaEngine.zip",
  releases: "https://github.com/TrexioasXavier/elintria-website/releases",
  site: "https://trexioasxavier.github.io/elintria-website/"
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
