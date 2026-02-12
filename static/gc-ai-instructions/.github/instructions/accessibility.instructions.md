---
applyTo: "**/*.html,**/*.cshtml,**/*.css,**/*.js,**/Views/**"
---

# Accessibility Instructions (WCAG 2.1 AA)

All Government of Canada web applications must meet **WCAG 2.1 Level AA** compliance as mandated by the Standard on Web Accessibility.

## Legal Requirements

- **Standard on Web Accessibility** (Treasury Board of Canada Secretariat)
- **Accessible Canada Act** (Bill C-81, 2019)
- **WCAG 2.1 Level AA** is the mandatory minimum

## Core Principles (POUR)

1. **Perceivable** - Information must be presentable to users in ways they can perceive
2. **Operable** - Interface components must be operable
3. **Understandable** - Information and operation must be understandable
4. **Robust** - Content must be robust enough for assistive technologies

## Essential Accessibility Patterns

### 1. Semantic HTML

Use proper HTML5 semantic elements:

```html
<!-- ✅ GOOD: Semantic HTML -->
<header>
    <nav>
        <ul>
            <li><a href="/home">Home</a></li>
        </ul>
    </nav>
</header>

<main>
    <article>
        <h1>Article Title</h1>
        <p>Article content...</p>
    </article>
</main>

<footer>
    <p>&copy; 2025 Government of Canada</p>
</footer>

<!-- ❌ BAD: Non-semantic divs -->
<div class="header">
    <div class="nav">
        <div class="link"><a href="/home">Home</a></div>
    </div>
</div>

<div class="content">
    <div class="title">Article Title</div>
    <div class="text">Article content...</div>
</div>
```

### 2. Heading Hierarchy

Headings must follow logical order (h1 → h2 → h3, no skipping):

```html
<!-- ✅ GOOD: Proper hierarchy -->
<h1>Page Title</h1>
    <h2>Section 1</h2>
        <h3>Subsection 1.1</h3>
        <h3>Subsection 1.2</h3>
    <h2>Section 2</h2>
        <h3>Subsection 2.1</h3>

<!-- ❌ BAD: Skipped heading level -->
<h1>Page Title</h1>
    <h3>Section 1</h3> <!-- Skipped h2 -->
        <h4>Subsection 1.1</h4>
```

**Why it matters**: Screen readers use headings for navigation. Skipping levels confuses users about content structure.

### 3. Form Labels

Every form input MUST have an associated label:

```html
<!-- ✅ GOOD: Explicit label with for/id -->
<label for="email">Email address</label>
<input type="email" id="email" name="email" />

<!-- ✅ GOOD: Implicit label (wrapping) -->
<label>
    Email address
    <input type="email" name="email" />
</label>

<!-- ❌ BAD: No label -->
<input type="email" name="email" placeholder="Enter your email" />

<!-- ❌ BAD: Using placeholder as label -->
<input type="email" name="email" placeholder="Email address" />
```

**Required field indicators**:
```html
<label for="sin" class="required">
    <span class="field-name">Social Insurance Number</span>
    <strong class="required">(required)</strong>
</label>
<input type="text" id="sin" name="sin" required aria-required="true" />
```

### 4. Alt Text for Images

All images must have alt attributes:

```html
<!-- ✅ GOOD: Descriptive alt text -->
<img src="/logo.png" alt="Government of Canada logo" />

<!-- ✅ GOOD: Informative image -->
<img src="/chart.png" alt="Bar chart showing unemployment rate decreased from 8% to 6% between 2023 and 2024" />

<!-- ✅ GOOD: Decorative image (empty alt) -->
<img src="/decorative-line.png" alt="" />

<!-- ❌ BAD: Missing alt attribute -->
<img src="/logo.png" />

<!-- ❌ BAD: Redundant alt text -->
<img src="/photo123.jpg" alt="image" />
<img src="/photo123.jpg" alt="photo123.jpg" />
```

**Complex images** (charts, diagrams):
```html
<figure>
    <img src="/complex-chart.png" alt="Employment statistics by province" />
    <figcaption>
        <details>
            <summary>Text description</summary>
            <p>This chart shows employment rates by province...</p>
            <table>
                <caption>Employment data</caption>
                <thead>
                    <tr>
                        <th>Province</th>
                        <th>Employment Rate</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Ontario</td>
                        <td>94.2%</td>
                    </tr>
                    <!-- more rows -->
                </tbody>
            </table>
        </details>
    </figcaption>
</figure>
```

### 5. Color Contrast

Minimum contrast ratios (WCAG 2.1 AA):
- **Normal text**: 4.5:1
- **Large text** (18pt+ or 14pt+ bold): 3:1
- **UI components and graphics**: 3:1

```html
<!-- ✅ GOOD: Sufficient contrast -->
<p style="color: #000000; background: #ffffff;">Black text on white (21:1)</p>
<p style="color: #284162; background: #ffffff;">Dark blue on white (8.6:1)</p>

<!-- ❌ BAD: Insufficient contrast -->
<p style="color: #777777; background: #ffffff;">Light grey on white (4.4:1 - fails)</p>
<p style="color: #ff9900; background: #ffffff;">Orange on white (2.9:1 - fails)</p>
```

**Testing tools**:
- Browser DevTools (Chrome, Firefox) - built-in contrast checker
- WebAIM Contrast Checker: https://webaim.org/resources/contrastchecker/
- Colour Contrast Analyser (desktop app)

**Don't rely on color alone**:
```html
<!-- ❌ BAD: Color only -->
<p style="color: red;">Required field</p>

<!-- ✅ GOOD: Color + text + icon -->
<p>
    <span class="glyphicon glyphicon-exclamation-sign text-danger" aria-hidden="true"></span>
    <span class="text-danger"><strong>Required field</strong></span>
</p>
```

### 6. Keyboard Navigation

All interactive elements must be keyboard accessible:

```html
<!-- ✅ GOOD: Native button (keyboard accessible by default) -->
<button type="button" onclick="doSomething()">Click me</button>

<!-- ✅ GOOD: Link (keyboard accessible by default) -->
<a href="/page">Go to page</a>

<!-- ❌ BAD: Div with click handler (not keyboard accessible) -->
<div onclick="doSomething()">Click me</div>

<!-- ⚠️ ACCEPTABLE: Div with proper ARIA and keyboard handler -->
<div role="button" tabindex="0" onclick="doSomething()" onkeydown="handleKeyboard(event)">
    Click me
</div>
```

**Keyboard event handling**:
```javascript
function handleKeyboard(event) {
    // Enter or Space should activate
    if (event.key === 'Enter' || event.key === ' ') {
        event.preventDefault();
        doSomething();
    }
}
```

**Focus indicators** (never remove!):
```css
/* ❌ BAD: Removing focus indicators */
*:focus {
    outline: none;
}

/* ✅ GOOD: Enhanced focus indicators */
a:focus,
button:focus,
input:focus {
    outline: 3px solid #0535d2;
    outline-offset: 2px;
}
```

### 7. Skip Links

Skip links must be the first focusable element:

```html
<body vocab="http://schema.org/" typeof="WebPage">
    <!-- Skip links FIRST -->
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

    <!-- Then header -->
    <header role="banner">
        <!-- Header content -->
    </header>

    <!-- Main content with matching ID -->
    <main role="main" property="mainContentOfPage" class="container">
        <h1 id="wb-cont">Page Title</h1>
        <!-- Content -->
    </main>

    <!-- Footer with matching ID -->
    <footer role="contentinfo" id="wb-info">
        <!-- Footer content -->
    </footer>
</body>
```

### 8. ARIA Landmarks

Use ARIA roles and HTML5 semantic elements:

```html
<!-- ✅ GOOD: Semantic HTML provides landmarks automatically -->
<header role="banner">
    <nav role="navigation" aria-label="Main navigation">
        <!-- Navigation -->
    </nav>
</header>

<main role="main">
    <!-- Main content -->
</main>

<aside role="complementary" aria-label="Related information">
    <!-- Sidebar content -->
</aside>

<footer role="contentinfo">
    <!-- Footer -->
</footer>
```

**Search landmark**:
```html
<form role="search" action="/search" method="get">
    <label for="search-input">Search</label>
    <input type="search" id="search-input" name="q" />
    <button type="submit">Search</button>
</form>
```

### 9. Link Text

Links must make sense out of context:

```html
<!-- ✅ GOOD: Descriptive link text -->
<a href="/benefits">View available benefits</a>
<a href="/form.pdf">Download application form (PDF, 2 MB)</a>

<!-- ❌ BAD: Generic link text -->
<a href="/benefits">Click here</a>
<p>To learn more, <a href="/info">click here</a>.</p>

<!-- ⚠️ ACCEPTABLE: Generic text with context -->
<a href="/benefits" aria-label="Learn more about available benefits">Learn more</a>
```

**File downloads**:
```html
<!-- Include file type and size -->
<a href="/guide.pdf">
    Employment Insurance Guide (PDF, 1.5 MB)
</a>

<a href="/form.docx">
    Application Form (DOCX, 120 KB)
</a>
```

### 10. Data Tables

Tables must have proper structure and captions:

```html
<!-- ✅ GOOD: Accessible table -->
<table class="table table-striped wb-tables">
    <caption>Processing times by service type</caption>
    <thead>
        <tr>
            <th scope="col">Service</th>
            <th scope="col">Standard Processing</th>
            <th scope="col">Express Processing</th>
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

<!-- ❌ BAD: Missing caption, no scope attributes -->
<table>
    <tr>
        <td><strong>Service</strong></td>
        <td><strong>Processing Time</strong></td>
    </tr>
    <tr>
        <td>Passport renewal</td>
        <td>20 business days</td>
    </tr>
</table>
```

**Complex tables**:
```html
<!-- Use id/headers for complex relationships -->
<table>
    <caption>Budget by department and category</caption>
    <thead>
        <tr>
            <th id="dept">Department</th>
            <th id="salary">Salaries</th>
            <th id="travel">Travel</th>
            <th id="total">Total</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th id="it" headers="dept">IT Department</th>
            <td headers="it salary">$500,000</td>
            <td headers="it travel">$50,000</td>
            <td headers="it total">$550,000</td>
        </tr>
    </tbody>
</table>
```

## Testing Procedures

### Automated Testing

```bash
# Install axe-core for automated accessibility testing
npm install --save-dev @axe-core/cli

# Run automated scan
npx @axe-core/cli http://localhost:5000

# Add to package.json scripts
"scripts": {
    "test:a11y": "axe http://localhost:5000 --exit"
}

# Run before committing
npm run test:a11y
```

**CI/CD integration**:
```yaml
# GitHub Actions example
- name: Accessibility Tests
  run: |
    npm install -g @axe-core/cli
    npm start &
    sleep 5
    axe http://localhost:5000 --exit
```

### Manual Testing

**1. Keyboard navigation** (15 minutes):
- Unplug your mouse
- Navigate using Tab, Shift+Tab, Enter, Space, Arrow keys
- Verify all interactive elements are reachable
- Check focus indicators are visible
- Ensure no keyboard traps

**2. Screen reader testing** (30 minutes):
- **Windows**: NVDA (free) or JAWS
- **macOS**: VoiceOver (built-in)
- Navigate by headings (H key in NVDA/JAWS)
- Navigate by landmarks (D key for landmarks)
- Navigate by forms (F key for form fields)
- Verify all content is announced correctly

**3. Zoom testing** (10 minutes):
- Zoom to 200% (Ctrl/Cmd + plus key)
- Verify content remains readable
- Check horizontal scrolling is not required
- Ensure no content is cut off

**4. Color contrast** (5 minutes):
- Use browser DevTools contrast checker
- Verify all text meets 4.5:1 ratio (or 3:1 for large text)
- Check UI components meet 3:1 ratio

**5. HTML validation** (2 minutes):
- https://validator.w3.org/
- Fix any errors (errors can break assistive technologies)

## Common Accessibility Issues

### Issue: Missing Form Labels
**Impact**: Screen readers can't identify form fields
**Fix**: Add explicit labels with for/id association

### Issue: Low Color Contrast
**Impact**: Users with low vision can't read text
**Fix**: Use darker colors or lighter backgrounds

### Issue: Images Without Alt Text
**Impact**: Screen readers can't describe images
**Fix**: Add descriptive alt attributes

### Issue: Keyboard Traps
**Impact**: Keyboard users get stuck in modals/menus
**Fix**: Ensure Esc key closes modals, focus returns to trigger

### Issue: Non-Semantic HTML
**Impact**: Screen readers can't navigate efficiently
**Fix**: Use proper HTML5 elements (header, nav, main, etc.)

## Accessibility Checklist

Before deploying:
- [ ] All images have alt text
- [ ] All form inputs have labels
- [ ] Color contrast meets 4.5:1 (normal text) or 3:1 (large text)
- [ ] Keyboard navigation works for all interactive elements
- [ ] Focus indicators are visible
- [ ] Heading hierarchy is logical (no skipped levels)
- [ ] Skip links are present and work
- [ ] Tables have captions and proper scope attributes
- [ ] Link text is descriptive
- [ ] ARIA landmarks are used correctly
- [ ] Automated scan passes (axe-core or similar)
- [ ] Manual keyboard testing passes
- [ ] Screen reader testing passes
- [ ] HTML validates without errors

## Resources

- **WCAG 2.1 Guidelines**: https://www.w3.org/WAI/WCAG21/quickref/
- **WebAIM**: https://webaim.org/
- **Standard on Web Accessibility**: https://www.tbs-sct.gc.ca/pol/doc-eng.aspx?id=23601
- **axe DevTools**: Browser extension for accessibility testing
- **NVDA Screen Reader**: https://www.nvaccess.org/ (free, Windows)
- **Colour Contrast Analyser**: https://www.tpgi.com/color-contrast-checker/

## Training

- **CSPS Accessibility Training**: https://catalogue.csps-efpc.gc.ca/
- **WebAIM Training**: https://webaim.org/training/
- **Deque University**: https://dequeuniversity.com/

## Support

For accessibility questions:
- **WET-BOEW GitHub**: https://github.com/wet-boew/wet-boew/issues
- **GC Accessibility Office**: accessibility-accessibilite@tbs-sct.gc.ca

## Version Information

This guidance is current for:
- WCAG 2.1 Level AA
- Standard on Web Accessibility (2011, amended 2013)
- Accessible Canada Act (2019)

Last updated: 2025-02-11
