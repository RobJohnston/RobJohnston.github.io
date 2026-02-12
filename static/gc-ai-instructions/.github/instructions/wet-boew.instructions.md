---
applyTo: "**/*.html,**/*.cshtml,**/Views/**"
---

# WET-BOEW Component Instructions

All Government of Canada web applications must use WET-BOEW for WCAG 2.1 AA compliance.

## Base Template Structure

Every HTML page must use this structure:

```html
<!DOCTYPE html>
<html class="no-js" lang="en" dir="ltr">
<head>
    <meta charset="utf-8">
    <title>Page Title - Canada.ca</title>
    <meta content="width=device-width,initial-scale=1" name="viewport">
    <link rel="stylesheet" href="https://www.canada.ca/etc/designs/canada/wet-boew/css/theme.min.css">
    <noscript>
        <link rel="stylesheet" href="https://www.canada.ca/etc/designs/canada/wet-boew/css/noscript.min.css">
    </noscript>
</head>
<body vocab="http://schema.org/" typeof="WebPage">
    <!-- Skip to content links (WCAG requirement for keyboard navigation) -->
    <nav>
        <ul id="wb-tphp">
            <li class="wb-slc">
                <a class="wb-sl" href="#wb-cont">Skip to main content</a>
            </li>
            <li class="wb-slc visible-sm visible-md visible-lg">
                <a class="wb-sl" href="#wb-info">Skip to "About this site"</a>
            </li>
        </ul>
    </nav>

    <!-- Standard GC header with language toggle -->
    <header role="banner">
        <div id="wb-bnr">
            <div class="container">
                <div class="row">
                    <div class="col-xs-5 col-md-4" property="publisher" typeof="GovernmentOrganization">
                        <a href="https://www.canada.ca/en.html" property="url">
                            <img src="https://www.canada.ca/etc/designs/canada/wet-boew/assets/sig-blk-en.svg"
                                 alt="Government of Canada"
                                 property="logo" />
                        </a>
                    </div>
                    <section class="wb-mb-links col-xs-4 col-sm-3" id="wb-glb-mn">
                        <h2>Search and menus</h2>
                        <!-- Menu content -->
                    </section>
                    <section id="wb-lng" class="col-xs-3 col-sm-12 text-right">
                        <h2 class="wb-inv">Language selection</h2>
                        <ul class="list-inline margin-bottom-none">
                            <li><a lang="fr" href="?lang=fr">Français</a></li>
                        </ul>
                    </section>
                </div>
            </div>
        </div>
    </header>

    <!-- Breadcrumb navigation -->
    <nav id="wb-bc" property="breadcrumb">
        <h2>You are here:</h2>
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="https://www.canada.ca/en.html">Home</a></li>
                <li><a href="/service">Service name</a></li>
            </ol>
        </div>
    </nav>

    <!-- Main content area -->
    <main role="main" property="mainContentOfPage" class="container">
        <h1 id="wb-cont" property="name">Page Title</h1>
        <!-- Your content here -->
    </main>

    <!-- Standard GC footer -->
    <footer role="contentinfo" id="wb-info">
        <div class="landscape">
            <nav class="container wb-navcurr">
                <h2 class="wb-inv">About government</h2>
                <!-- Footer links -->
            </nav>
        </div>
        <div class="brand">
            <div class="container">
                <div class="row">
                    <nav class="col-md-10 ftr-urlt-lnk">
                        <h2 class="wb-inv">About this site</h2>
                        <ul>
                            <li><a href="https://www.canada.ca/en/social.html">Social media</a></li>
                            <li><a href="https://www.canada.ca/en/mobile.html">Mobile applications</a></li>
                            <li><a href="https://www.canada.ca/en/government/about.html">About Canada.ca</a></li>
                            <li><a href="https://www.canada.ca/en/transparency/terms.html">Terms and conditions</a></li>
                            <li><a href="https://www.canada.ca/en/transparency/privacy.html">Privacy</a></li>
                        </ul>
                    </nav>
                    <div class="col-xs-6 visible-sm visible-xs tofpg">
                        <a href="#wb-cont">Top of Page <span class="glyphicon glyphicon-chevron-up"></span></a>
                    </div>
                    <div class="col-xs-6 col-md-2 text-right">
                        <img src="https://www.canada.ca/etc/designs/canada/wet-boew/assets/wmms-blk.svg"
                             alt="Symbol of the Government of Canada">
                    </div>
                </div>
            </div>
        </div>
    </footer>

    <!-- WET-BOEW JavaScript -->
    <script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/wet-boew.min.js"></script>
    <script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/theme.min.js"></script>
</body>
</html>
```

## Common Components

### Forms with Validation

WET-BOEW includes automatic form validation. Use these patterns:

```html
<form action="/submit" method="post" class="wb-frmvld">
    <div class="form-group">
        <label for="email" class="required">
            <span class="field-name">Email address</span>
            <strong class="required">(required)</strong>
        </label>
        <input
            type="email"
            id="email"
            name="email"
            class="form-control"
            required
            data-rule-email="true"
            data-msg="Please enter a valid email address"
        />
    </div>

    <div class="form-group">
        <label for="postal-code" class="required">
            <span class="field-name">Postal code</span>
            <strong class="required">(required)</strong>
        </label>
        <input
            type="text"
            id="postal-code"
            name="postal_code"
            class="form-control"
            pattern="[A-Za-z][0-9][A-Za-z] [0-9][A-Za-z][0-9]"
            required
            data-msg="Please enter a valid Canadian postal code (e.g., K1A 0B1)"
        />
    </div>

    <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

**Important**:
- Always use `class="wb-frmvld"` on the `<form>` element to enable WET-BOEW validation
- Required fields must have both `required` attribute AND `<strong class="required">` label indicator
- Provide clear error messages via `data-msg` attributes
- Bilingual error messages: use `data-msg` for English, `data-msg-fr` for French

### Date Picker

```html
<div class="form-group">
    <label for="date-birth">
        <span class="field-name">Date of birth</span>
        <strong class="required">(required)</strong>
    </label>
    <input
        type="date"
        id="date-birth"
        name="date_birth"
        class="form-control"
        required
        data-rule-required="true"
        min="1900-01-01"
        max="2024-12-31"
    />
</div>
```

### Alerts and Notifications

```html
<!-- Success message -->
<section class="alert alert-success">
    <h3>Application submitted successfully</h3>
    <p>Your confirmation number is: <strong>AB-1234-5678</strong></p>
</section>

<!-- Warning message -->
<section class="alert alert-warning">
    <h3>Important information</h3>
    <p>You must complete this application within 30 days.</p>
</section>

<!-- Error message -->
<section class="alert alert-danger">
    <h3>There was a problem</h3>
    <p>We could not process your payment. Please check your credit card information and try again.</p>
</section>

<!-- Info message -->
<section class="alert alert-info">
    <h3>Did you know?</h3>
    <p>You can save your progress and return later.</p>
</section>
```

### Tabs (for organizing content)

```html
<div class="wb-tabs">
    <div class="tabpanels">
        <details id="tab1">
            <summary>Overview</summary>
            <h2 class="wb-inv">Overview</h2>
            <p>Content for overview tab...</p>
        </details>

        <details id="tab2">
            <summary>Eligibility</summary>
            <h2 class="wb-inv">Eligibility</h2>
            <p>Content for eligibility tab...</p>
        </details>

        <details id="tab3">
            <summary>How to apply</summary>
            <h2 class="wb-inv">How to apply</h2>
            <p>Content for how to apply tab...</p>
        </details>
    </div>
</div>
```

**Note**: WET-BOEW tabs automatically handle:
- Keyboard navigation (arrow keys, Tab, Enter)
- ARIA attributes for screen readers
- Mobile-friendly accordion view on small screens

### Data Tables

```html
<table class="table table-striped table-hover wb-tables">
    <caption>Processing times by service type</caption>
    <thead>
        <tr>
            <th scope="col">Service</th>
            <th scope="col">Standard processing</th>
            <th scope="col">Express processing</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row">Passport renewal</th>
            <td>20 business days</td>
            <td>10 business days</td>
        </tr>
        <tr>
            <th scope="row">New passport</th>
            <td>25 business days</td>
            <td>10 business days</td>
        </tr>
    </tbody>
</table>
```

Add `class="wb-tables"` for enhanced features:
- Sorting (click column headers)
- Filtering (search box automatically added)
- Pagination for large tables
- All features are keyboard accessible and screen-reader friendly

## Bilingual Content

Every user-facing page must be available in both English and French:

```html
<!-- English version: page.html -->
<h1>Apply for Employment Insurance</h1>
<p>Employment Insurance (EI) provides temporary financial assistance...</p>

<!-- French version: page.html?lang=fr or page-fr.html -->
<h1>Demander l'assurance-emploi</h1>
<p>L'assurance-emploi (AE) fournit une aide financière temporaire...</p>
```

**Language toggle** must be present on every page:
```html
<section id="wb-lng" class="col-xs-3 col-sm-12 text-right">
    <h2 class="wb-inv">Language selection</h2>
    <ul class="list-inline margin-bottom-none">
        <li><a lang="fr" href="?lang=fr">Français</a></li>
    </ul>
</section>
```

## Accessibility Requirements

All WET-BOEW components are WCAG 2.1 AA compliant when used correctly. Key requirements:

1. **Semantic HTML**: Use proper heading hierarchy (h1 → h2 → h3, no skipping levels)
2. **Form labels**: Every input must have an associated `<label>` element
3. **Alt text**: All images must have descriptive `alt` attributes
4. **Keyboard navigation**: All interactive elements must be keyboard accessible
5. **Color contrast**: Minimum 4.5:1 for normal text, 3:1 for large text
6. **Skip links**: Must be first focusable element on page

## Testing Checklist

Before committing changes to templates:
- [ ] Test with keyboard only (no mouse)
- [ ] Test with NVDA or JAWS screen reader
- [ ] Run automated accessibility scan: `npm run test:a11y`
- [ ] Verify bilingual content (English and French versions)
- [ ] Check color contrast with browser DevTools
- [ ] Validate HTML: https://validator.w3.org/

## Common Mistakes to Avoid

❌ **Don't create custom accessible components**—use WET-BOEW components
❌ **Don't inline styles**—use WET-BOEW CSS classes
❌ **Don't skip the language toggle**—required for Official Languages Act compliance
❌ **Don't forget `<caption>` on data tables**—WCAG 2.1 requirement
❌ **Don't use `div` for clickable elements**—use `<button>` or `<a>` for accessibility
