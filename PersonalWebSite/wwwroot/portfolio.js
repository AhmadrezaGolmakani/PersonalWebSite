// این فایل رو در wwwroot/js/portfolio.js قرار بده و در index.html / _Host.cshtml قبل از بسته‌شدن </body> لینکش کن:
// <script src="js/portfolio.js"></script>

window.portfolioInterop = {
    initReveal: function () {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(e => {
                if (e.isIntersecting) {
                    e.target.classList.add('in');
                    observer.unobserve(e.target);
                }
            });
        }, { threshold: 0.15 });

        document.querySelectorAll('.reveal').forEach(el => observer.observe(el));
    }
};
