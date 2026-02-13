# Accessibility Instructions - WCAG 2.1 AA

## Overview

All Government of Canada web applications must meet WCAG 2.1 Level AA accessibility standards. This file provides practical guidance for building accessible applications.

## Four Principles of Accessibility (POUR)

1. **Perceivable** - Information must be presentable to users in ways they can perceive
2. **Operable** - User interface components must be operable
3. **Understandable** - Information and operation must be understandable
4. **Robust** - Content must be robust enough to work with assistive technologies

## Semantic HTML

Use semantic HTML elements for their intended purpose:

```html
<!-- Good: Semantic HTML -->
<header>
    <nav>
        <ul>
            <li><a href="/">Home</a></li>
        </ul>
    </nav>
</header>
<main>
    <article>
        <h1>Page Title</h1>
        <p>Content...</p>
    </article>
</main>
<footer>
    <p>&copy; 2024 Government of Canada</p>
</footer>

<!-- Bad: Generic divs -->
<div class="header">
    <div class="nav">
        <div class="link">Home</div>
    </div>
</div>
```

## Headings

Maintain proper heading hierarchy:

```html
<!-- Good: Logical hierarchy -->
<h1>Page Title</h1>
    <h2>Section 1</h2>
        <h3>Subsection 1.1</h3>
        <h3>Subsection 1.2</h3>
    <h2>Section 2</h2>

<!-- Bad: Skipping levels -->
<h1>Page Title</h1>
    <h3>Section 1</h3>  <!-- Skipped h2 -->
```

**Rules:**
- Only one H1 per page
- Don't skip heading levels
- Don't choose headings based on visual appearance (use CSS for styling)

## Images and Alternative Text

All images must have appropriate alt text:

```html
<!-- Informative images -->
<img src="chart.png" alt="Bar chart showing 60% increase in applications from 2023 to 2024">

<!-- Decorative images -->
<img src="decorative-border.png" alt="" role="presentation">

<!-- Functional images (links/buttons) -->
<a href="/home">
    <img src="logo.png" alt="Government of Canada - Home">
</a>

<!-- Complex images -->
<figure>
    <img src="process-diagram.png" alt="Application process diagram">
    <figcaption>
        Detailed description: The application process consists of three steps...
    </figcaption>
</figure>
```

**Alt text guidelines:**
- Be concise but descriptive
- Don't start with "image of" or "picture of"
- For decorative images, use `alt=""` (empty alt text)
- For complex images, provide detailed description in caption or adjacent text

## Links

Links must be descriptive and make sense out of context:

```html
<!-- Good: Descriptive link text -->
<p>Read the <a href="/guide.pdf">Applicant's Guide (PDF, 2MB)</a> for more information.</p>

<!-- Bad: Generic link text -->
<p>For more information, <a href="/guide.pdf">click here</a>.</p>

<!-- Good: Link indicates file format and size -->
<a href="/form.pdf">Application Form (PDF, 500KB)</a>

<!-- Links that open in new window/tab -->
<a href="https://external-site.com" target="_blank">
    External Website
    <span class="sr-only">(opens in new window)</span>
    <span aria-hidden="true" class="glyphicon glyphicon-new-window"></span>
</a>
```

## Buttons vs Links

Use the correct element for the action:

```html
<!-- Buttons: For actions that change state -->
<button type="submit">Submit Application</button>
<button type="button" onclick="openModal()">Open Dialog</button>

<!-- Links: For navigation -->
<a href="/contact">Contact Us</a>
<a href="#main-content">Skip to main content</a>
```

## Forms

### Labels

Every input must have an associated label:

```html
<!-- Good: Explicit label association -->
<label for="email">Email address</label>
<input type="email" id="email" name="email">

<!-- Good: Implicit label (less common) -->
<label>
    Email address
    <input type="email" name="email">
</label>

<!-- Bad: No label -->
<input type="email" name="email" placeholder="Email address">  <!-- Placeholder is not a label! -->
```

### Required Fields

Indicate required fields clearly:

```html
<label for="name">
    Full Name
    <strong class="required">(required)</strong>
</label>
<input type="text" id="name" name="name" required aria-required="true">
```

### Error Messages

Error messages must be clear and associated with fields:

```html
<!-- Field with error -->
<div class="form-group has-error">
    <label for="email">Email address</label>
    <input
        type="email"
        id="email"
        name="email"
        aria-invalid="true"
        aria-describedby="email-error"
    >
    <span id="email-error" class="error-message">
        Please enter a valid email address.
    </span>
</div>
```

### Fieldsets and Legends

Group related form fields:

```html
<fieldset>
    <legend>Contact Information</legend>

    <label for="phone">Phone</label>
    <input type="tel" id="phone" name="phone">

    <label for="email">Email</label>
    <input type="email" id="email" name="email">
</fieldset>

<fieldset>
    <legend>Mailing Address</legend>
    <!-- address fields -->
</fieldset>
```

## Tables

Use proper table structure:

```html
<table class="table">
    <!-- Caption describes table purpose -->
    <caption>Application Status Summary</caption>

    <thead>
        <tr>
            <th scope="col">Application ID</th>
            <th scope="col">Status</th>
            <th scope="col">Date</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>APP-001</td>
            <td>Approved</td>
            <td>2024-01-15</td>
        </tr>
    </tbody>
</table>
```

**Required elements:**
- `<caption>` - Describes the table
- `<thead>`, `<tbody>`, optionally `<tfoot>` - Structure
- `scope` attribute on `<th>` elements (`scope="col"` or `scope="row"`)

## Keyboard Accessibility

All interactive elements must be keyboard accessible:

```html
<!-- Good: Native button is keyboard accessible -->
<button type="button">Click Me</button>

<!-- Bad: Div acting as button (not keyboard accessible without extra work) -->
<div onclick="doSomething()">Click Me</div>

<!-- If you must use non-button element, make it accessible -->
<div
    role="button"
    tabindex="0"
    onclick="doSomething()"
    onkeypress="handleKeyPress(event)">
    Click Me
</div>
```

**Keyboard navigation requirements:**
- Tab: Move forward through interactive elements
- Shift+Tab: Move backward
- Enter/Space: Activate buttons and links
- Arrow keys: Navigate within components (menus, tabs, etc.)
- Escape: Close dialogs/modals

## Focus Indicators

Ensure visible focus indicators:

```css
/* Good: Visible focus indicator */
a:focus, button:focus, input:focus {
    outline: 2px solid #0535d2;
    outline-offset: 2px;
}

/* Bad: Removing focus indicator without replacement */
button:focus {
    outline: none;  /* Never do this without custom focus styling! */
}
```

## Color Contrast

Ensure sufficient color contrast (WCAG 2.1 AA requirements):

- **Normal text**: Minimum 4.5:1 contrast ratio
- **Large text** (18pt+ or 14pt+ bold): Minimum 3:1 contrast ratio
- **User interface components**: Minimum 3:1 contrast ratio

**Tools for checking contrast:**
- [WebAIM Contrast Checker](https://webaim.org/resources/contrastchecker/)
- Browser DevTools accessibility panel
- [WAVE Browser Extension](https://wave.webaim.org/extension/)

```html
<!-- Good: Sufficient contrast -->
<p style="color: #000; background: #fff;">Black text on white (21:1 ratio)</p>

<!-- Bad: Insufficient contrast -->
<p style="color: #ccc; background: #fff;">Light gray on white (1.6:1 ratio)</p>
```

## ARIA (Accessible Rich Internet Applications)

Use ARIA attributes when semantic HTML isn't sufficient:

### Landmark Roles
```html
<header role="banner">
<nav role="navigation">
<main role="main">
<aside role="complementary">
<footer role="contentinfo">
```

### Live Regions
```html
<!-- For dynamic content updates -->
<div role="alert" aria-live="assertive">
    Your session will expire in 5 minutes.
</div>

<div role="status" aria-live="polite">
    Search returned 42 results.
</div>
```

### ARIA Labels
```html
<!-- When visible label isn't appropriate -->
<button aria-label="Close dialog">
    <span aria-hidden="true">&times;</span>
</button>

<!-- When you need additional description -->
<input
    type="text"
    id="search"
    aria-label="Search"
    aria-describedby="search-help"
>
<span id="search-help">Enter keywords to search applications</span>
```

### ARIA States
```html
<!-- Expanded/collapsed -->
<button aria-expanded="false" aria-controls="menu">
    Menu
</button>
<nav id="menu" hidden>
    <!-- menu items -->
</nav>

<!-- Selected item -->
<li role="tab" aria-selected="true">Tab 1</li>
<li role="tab" aria-selected="false">Tab 2</li>
```

## Skip Links

Provide skip links for keyboard users:

```html
<body>
    <a href="#main-content" class="skip-link">Skip to main content</a>

    <header>
        <!-- Header content -->
    </header>

    <main id="main-content" tabindex="-1">
        <!-- Main content -->
    </main>
</body>
```

```css
.skip-link {
    position: absolute;
    left: -10000px;
    width: 1px;
    height: 1px;
    overflow: hidden;
}

.skip-link:focus {
    position: static;
    width: auto;
    height: auto;
}
```

## Language

Specify document language:

```html
<!-- Document language -->
<html lang="en">

<!-- Inline language changes -->
<p>The term <span lang="fr">raison d'être</span> means reason for being.</p>
```

## Page Titles

Provide descriptive page titles:

```html
<!-- Good: Specific and descriptive -->
<title>Contact Us - Employment Insurance - Government of Canada</title>

<!-- Bad: Generic -->
<title>Contact</title>
```

## Testing Checklist

- [ ] All images have appropriate alt text
- [ ] Headings follow logical hierarchy (H1 → H2 → H3)
- [ ] All form inputs have associated labels
- [ ] Color contrast meets WCAG 2.1 AA (4.5:1 for normal text)
- [ ] All interactive elements are keyboard accessible
- [ ] Focus indicators are visible
- [ ] Skip links are present and functional
- [ ] Tables have captions and proper structure
- [ ] Links are descriptive
- [ ] Error messages are associated with form fields
- [ ] Page title is descriptive
- [ ] Document language is specified
- [ ] Content is readable without CSS
- [ ] Screen reader testing completed (NVDA, JAWS, or VoiceOver)

## Testing Tools

- [WAVE Browser Extension](https://wave.webaim.org/extension/) - Visual accessibility checker
- [axe DevTools](https://www.deque.com/axe/devtools/) - Automated accessibility testing
- [Lighthouse](https://developers.google.com/web/tools/lighthouse) - Built into Chrome DevTools
- [NVDA Screen Reader](https://www.nvaccess.org/) (Windows, free)
- [JAWS Screen Reader](https://www.freedomscientific.com/products/software/jaws/) (Windows, paid)
- [VoiceOver](https://www.apple.com/accessibility/voiceover/) (Mac/iOS, built-in)

## Common Mistakes to Avoid

1. ❌ Using placeholder as label
2. ❌ Removing focus indicators without replacement
3. ❌ Skipping heading levels
4. ❌ Using color alone to convey information
5. ❌ Making divs clickable instead of using buttons
6. ❌ Missing alt text on informative images
7. ❌ Generic link text ("click here", "read more")
8. ❌ Insufficient color contrast
9. ❌ Forms without labels
10. ❌ Tables without captions or proper structure

## See Also

- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
- [Standard on Web Accessibility - Government of Canada](https://www.tbs-sct.canada.ca/pol/doc-eng.aspx?id=23601)
- [WET-BOEW Accessibility Features](https://wet-boew.github.io/wet-boew/index-en.html)
- [forms.instructions.md](/gc-ai-instructions/wet-boew/forms.instructions.md)
- [tables.instructions.md](/gc-ai-instructions/wet-boew/tables.instructions.md)
