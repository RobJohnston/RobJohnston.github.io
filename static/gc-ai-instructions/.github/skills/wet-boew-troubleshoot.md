# WET-BOEW Troubleshooting Skill

## Purpose
Diagnose and resolve common WET-BOEW (Web Experience Toolkit) issues in Government of Canada web applications.

## Activation
Use this skill when encountering:
- WET-BOEW components not rendering correctly
- JavaScript functionality not working
- Accessibility issues with WET-BOEW components
- Bilingual content display problems
- CSS styling conflicts

## Common Issues and Solutions

### Issue 1: WET-BOEW JavaScript Not Initializing

**Symptoms**:
- Tabs, date picker, or other interactive components appear as plain HTML
- Console error: `wet is not defined` or `wb is not defined`
- Components lack WET-BOEW styling

**Diagnostic Steps**:

1. Check if WET-BOEW scripts are loaded:
```html
<!-- These should be at the end of <body>, in this order -->
<script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/wet-boew.min.js"></script>
<script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/theme.min.js"></script>
```

2. Verify scripts load before `</body>` close tag
3. Check browser console for 404 errors on script files
4. Ensure no Content Security Policy (CSP) blocking script execution

**Solutions**:

**Solution A**: Add missing scripts
```html
<body vocab="http://schema.org/" typeof="WebPage">
    <!-- Your content -->

    <!-- WET-BOEW JavaScript (must be before </body>) -->
    <script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/wet-boew.min.js"></script>
    <script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/theme.min.js"></script>
</body>
```

**Solution B**: Fix CSP headers if blocking scripts
```csharp
// In SecurityHeadersMiddleware or Startup.cs
context.Response.Headers.Add("Content-Security-Policy",
    "default-src 'self'; script-src 'self' www.canada.ca https://www.canada.ca; style-src 'self' 'unsafe-inline' www.canada.ca https://www.canada.ca");
```

**Solution C**: Self-host WET-BOEW if CDN blocked
```bash
# Install via npm
npm install wet-boew

# Reference local files
<script src="/wet-boew/js/wet-boew.min.js"></script>
<script src="/wet-boew/js/theme.min.js"></script>
```

### Issue 2: Forms Not Validating

**Symptoms**:
- Form submits without client-side validation
- Validation messages not appearing
- Required field indicators not styled correctly

**Diagnostic Steps**:

1. Check if form has `class="wb-frmvld"`:
```html
<form action="/submit" method="post" class="wb-frmvld">
```

2. Verify WET-BOEW JavaScript loaded (see Issue 1)
3. Check console for jQuery errors (WET-BOEW requires jQuery)
4. Inspect form inputs for proper `required` attributes

**Solutions**:

**Solution A**: Add validation class to form
```html
<!-- ❌ BAD: Missing wb-frmvld class -->
<form action="/submit" method="post">
    <input type="email" required />
</form>

<!-- ✅ GOOD: Has wb-frmvld class -->
<form action="/submit" method="post" class="wb-frmvld">
    <input type="email" required data-msg="Please enter a valid email" />
</form>
```

**Solution B**: Add proper validation attributes
```html
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
```

**Solution C**: Ensure jQuery loads before WET-BOEW
```html
<!-- jQuery is bundled with WET-BOEW, but if loading separately: -->
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
<script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/wet-boew.min.js"></script>
```

### Issue 3: Date Picker Not Appearing

**Symptoms**:
- Input shows as plain text field
- No calendar icon appears
- Clicking input doesn't open date picker

**Diagnostic Steps**:

1. Check input type is `date`:
```html
<input type="date" id="date-birth" name="date_birth" class="form-control" />
```

2. Verify WET-BOEW JavaScript loaded
3. Check if browser supports native date input (most modern browsers do)
4. Look for JavaScript errors in console

**Solutions**:

**Solution A**: Use correct input type
```html
<!-- ✅ GOOD: Uses type="date" -->
<div class="form-group">
    <label for="date-birth">Date of birth</label>
    <input type="date" id="date-birth" name="date_birth" class="form-control" />
</div>
```

**Solution B**: Add polyfill for older browsers
```html
<!-- WET-BOEW includes date picker polyfill automatically -->
<!-- Ensure wet-boew.min.js loads correctly -->
```

**Solution C**: Manually initialize if dynamic content
```javascript
// If adding date picker via JavaScript after page load
document.getElementById('date-birth').setAttribute('type', 'date');
// Trigger WET-BOEW initialization
if (typeof wb !== 'undefined') {
    wb.doc.trigger('wb-init.wb-date');
}
```

### Issue 4: Tabs Showing as Accordions on Desktop

**Symptoms**:
- Tabs display as accordions (details/summary) on desktop screens
- Expected horizontal tab navigation not appearing

**Diagnostic Steps**:

1. Check HTML structure matches WET-BOEW pattern:
```html
<div class="wb-tabs">
    <div class="tabpanels">
        <details id="tab1">
            <summary>Tab 1</summary>
            <!-- content -->
        </details>
    </div>
</div>
```

2. Verify `class="wb-tabs"` on outer div
3. Check CSS files loaded correctly
4. Inspect for JavaScript errors

**Solutions**:

**Solution A**: Correct HTML structure
```html
<!-- ✅ GOOD: Proper structure -->
<div class="wb-tabs">
    <div class="tabpanels">
        <details id="tab1">
            <summary>Overview</summary>
            <h2 class="wb-inv">Overview</h2>
            <p>Tab 1 content...</p>
        </details>
        <details id="tab2">
            <summary>Details</summary>
            <h2 class="wb-inv">Details</h2>
            <p>Tab 2 content...</p>
        </details>
    </div>
</div>
```

**Solution B**: Ensure theme CSS loaded
```html
<link rel="stylesheet" href="https://www.canada.ca/etc/designs/canada/wet-boew/css/theme.min.css">
```

**Solution C**: Force carousel style (optional)
```html
<!-- Force carousel style instead of accordion on mobile -->
<div class="wb-tabs carousel-s2">
    <div class="tabpanels">
        <!-- tabs -->
    </div>
</div>
```

### Issue 5: Language Toggle Not Working

**Symptoms**:
- Clicking language toggle doesn't switch language
- URL changes but page content remains in same language
- French version shows English content

**Diagnostic Steps**:

1. Check if language toggle links to correct URL:
```html
<section id="wb-lng">
    <h2 class="wb-inv">Language selection</h2>
    <ul class="list-inline margin-bottom-none">
        <li><a lang="fr" href="?lang=fr">Français</a></li>
    </ul>
</section>
```

2. Verify server-side language switching logic
3. Check `lang` attribute on `<html>` tag matches current language
4. Ensure French content exists for the page

**Solutions**:

**Solution A**: Implement server-side language detection
```csharp
// In ASP.NET Core Controller
public IActionResult Index(string lang = "en")
{
    // Set language cookie or session
    Response.Cookies.Append("language", lang, new CookieOptions
    {
        Expires = DateTimeOffset.UtcNow.AddYears(1)
    });

    ViewBag.CurrentLanguage = lang;
    return View();
}
```

**Solution B**: Update HTML lang attribute dynamically
```cshtml
@* In Razor view *@
@{
    var lang = ViewBag.CurrentLanguage ?? "en";
}
<!DOCTYPE html>
<html class="no-js" lang="@lang" dir="ltr">
```

**Solution C**: Link to separate French page
```html
<!-- If using separate files instead of query parameter -->
<section id="wb-lng">
    <h2 class="wb-inv">Language selection</h2>
    <ul class="list-inline margin-bottom-none">
        <li><a lang="fr" href="/fr/page-name">Français</a></li>
    </ul>
</section>
```

### Issue 6: Accessibility Scan Failing

**Symptoms**:
- Automated tools (axe, WAVE) report WCAG violations
- Screen reader testing reveals navigation issues
- Keyboard navigation not working

**Diagnostic Steps**:

1. Run automated scan:
```bash
npm run test:a11y
# or
npx @axe-core/cli https://localhost:5000
```

2. Test keyboard navigation (Tab, Shift+Tab, Enter, Space, Arrow keys)
3. Test with screen reader (NVDA on Windows, JAWS)
4. Check common issues:
   - Missing alt text on images
   - Form inputs without labels
   - Improper heading hierarchy
   - Insufficient color contrast
   - Missing skip links

**Solutions**:

**Solution A**: Fix missing form labels
```html
<!-- ❌ BAD: No label -->
<input type="text" name="name" placeholder="Enter your name" />

<!-- ✅ GOOD: Proper label -->
<label for="full-name">Full name</label>
<input type="text" id="full-name" name="name" />
```

**Solution B**: Add alt text to images
```html
<!-- ❌ BAD: Missing alt -->
<img src="/logo.png">

<!-- ✅ GOOD: Descriptive alt text -->
<img src="/logo.png" alt="Government of Canada logo">

<!-- ✅ GOOD: Decorative image (empty alt) -->
<img src="/decorative-line.png" alt="">
```

**Solution C**: Fix heading hierarchy
```html
<!-- ❌ BAD: Skipped heading level -->
<h1>Page Title</h1>
<h3>Section Title</h3> <!-- Skipped h2 -->

<!-- ✅ GOOD: Proper hierarchy -->
<h1>Page Title</h1>
<h2>Section Title</h2>
<h3>Subsection Title</h3>
```

**Solution D**: Ensure skip links work
```html
<!-- Skip links must be first focusable element -->
<body vocab="http://schema.org/" typeof="WebPage">
    <nav>
        <ul id="wb-tphp">
            <li class="wb-slc">
                <a class="wb-sl" href="#wb-cont">Skip to main content</a>
            </li>
        </ul>
    </nav>
    <!-- rest of page -->
    <main role="main" property="mainContentOfPage" class="container">
        <h1 id="wb-cont">Page Title</h1>
        <!-- content -->
    </main>
</body>
```

### Issue 7: Tables Not Sortable/Searchable

**Symptoms**:
- Data table displays but lacks sort/search functionality
- Clicking column headers doesn't sort
- No search box appears

**Diagnostic Steps**:

1. Check if table has `class="wb-tables"`:
```html
<table class="table wb-tables">
```

2. Verify WET-BOEW JavaScript loaded
3. Check if table has proper structure (thead, tbody)
4. Look for JavaScript errors

**Solutions**:

**Solution A**: Add wb-tables class
```html
<!-- ✅ GOOD: Has wb-tables class -->
<table class="table table-striped table-hover wb-tables">
    <caption>Processing times by service type</caption>
    <thead>
        <tr>
            <th scope="col">Service</th>
            <th scope="col">Processing Time</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row">Passport renewal</th>
            <td>20 business days</td>
        </tr>
    </tbody>
</table>
```

**Solution B**: Ensure proper table structure
```html
<!-- Table must have <caption>, <thead>, and <tbody> -->
<table class="wb-tables">
    <caption>Table Title (required for accessibility)</caption>
    <thead>
        <tr>
            <th scope="col">Column 1</th>
            <th scope="col">Column 2</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
        </tr>
    </tbody>
</table>
```

**Solution C**: Configure table options
```html
<!-- Custom configuration via data attributes -->
<table class="wb-tables table" data-wb-tables='{
    "paging": true,
    "info": true,
    "searching": true,
    "ordering": true
}'>
    <!-- table content -->
</table>
```

### Issue 8: CSS Conflicts with Custom Styles

**Symptoms**:
- WET-BOEW components look broken or misaligned
- Custom styles overriding WET-BOEW styles incorrectly
- Responsive breakpoints not working

**Diagnostic Steps**:

1. Check CSS load order (WET-BOEW should load before custom CSS):
```html
<link rel="stylesheet" href="https://www.canada.ca/etc/designs/canada/wet-boew/css/theme.min.css">
<link rel="stylesheet" href="/css/custom.css">
```

2. Use browser DevTools to inspect computed styles
3. Check for `!important` in custom CSS overriding WET-BOEW
4. Verify no inline styles conflicting with WET-BOEW

**Solutions**:

**Solution A**: Use more specific selectors instead of !important
```css
/* ❌ BAD: Using !important */
.btn {
    background-color: red !important;
}

/* ✅ GOOD: More specific selector */
.custom-section .btn-primary {
    background-color: red;
}
```

**Solution B**: Extend WET-BOEW classes instead of overriding
```css
/* ✅ GOOD: Extend existing styles */
.btn-custom {
    /* Inherits from .btn via HTML: <button class="btn btn-custom"> */
    background-color: #custom-color;
}
```

**Solution C**: Use GC Design System utilities
```html
<!-- Use built-in utility classes instead of custom CSS -->
<div class="mrgn-tp-lg mrgn-bttm-md">
    <!-- Content with top-large and bottom-medium margins -->
</div>
```

## Advanced Troubleshooting

### Debug Mode

Enable WET-BOEW debug mode to see detailed console logs:

```html
<!-- Add data-wb-debug="true" to see plugin initialization -->
<html class="no-js" lang="en" dir="ltr" data-wb-debug="true">
```

### Manual Plugin Initialization

If components added via JavaScript after page load:

```javascript
// Trigger WET-BOEW to initialize new components
if (typeof wb !== 'undefined') {
    // Re-initialize all plugins
    wb.doc.trigger('wb-init.wb');

    // Or initialize specific plugin
    wb.doc.trigger('wb-init.wb-tables');
    wb.doc.trigger('wb-init.wb-tabs');
    wb.doc.trigger('wb-init.wb-frmvld');
}
```

### Check WET-BOEW Version

```javascript
// In browser console
console.log(wb.version);
```

Ensure version matches documentation (4.0.x for Canada.ca theme).

## Prevention Best Practices

1. **Always use the standard WET-BOEW template** from canada.ca/en/government/about/design-system/pattern-library.html
2. **Test across browsers**: Chrome, Firefox, Safari, Edge
3. **Run accessibility scans** before committing: `npm run test:a11y`
4. **Test keyboard navigation** on every page
5. **Validate HTML**: https://validator.w3.org/
6. **Use WET-BOEW components** instead of custom implementations
7. **Keep WET-BOEW updated** to latest stable version
8. **Test with screen readers** at least monthly

## Getting Help

If issues persist after troubleshooting:

1. **WET-BOEW GitHub Issues**: https://github.com/wet-boew/wet-boew/issues
2. **GC Design System Support**: Ask in #wtw-bew channel on GCcollab
3. **Stack Overflow**: Tag questions with `wet-boew` and `canada.ca`
4. **Official Documentation**: https://wet-boew.github.io/wet-boew/index-en.html

## Quick Reference

### Essential WET-BOEW Classes

- `wb-frmvld` - Form validation
- `wb-tables` - Enhanced data tables
- `wb-tabs` - Tab interface
- `wb-slc` - Skip links container
- `wb-inv` - Invisible to visual users, visible to screen readers
- `required` - Required field indicator

### Essential HTML Patterns

```html
<!-- Form with validation -->
<form class="wb-frmvld">...</form>

<!-- Enhanced table -->
<table class="wb-tables">...</table>

<!-- Tab interface -->
<div class="wb-tabs">...</div>

<!-- Skip link -->
<a class="wb-sl" href="#wb-cont">Skip to main content</a>

<!-- Screen reader only text -->
<h2 class="wb-inv">Invisible heading</h2>
```

### Diagnostic Commands

```bash
# Install WET-BOEW locally
npm install wet-boew

# Run accessibility tests
npx @axe-core/cli http://localhost:5000

# Validate HTML
curl -s https://validator.w3.org/nu/?out=gnu https://yoursite.gc.ca

# Check for broken links
npx broken-link-checker http://localhost:5000
```

## Version Information

This troubleshooting guide is current for:
- WET-BOEW 4.0.x
- Canada.ca theme (GCWeb)
- WCAG 2.1 AA compliance standards

Last updated: 2025-02-11
