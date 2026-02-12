---
applyTo: "**/*.html,**/*.cshtml,**/*.css,**/Views/**"
---

# GC Design System Instructions

The GC Design System provides standardized UI components, design tokens, and CSS utilities for consistent, accessible Government of Canada web services.

## Overview

**Official Resource**: https://design.canada.ca/

The GC Design System works alongside WET-BOEW to provide:
- Pre-built page templates
- Design tokens (colors, spacing, typography)
- CSS utility classes
- Component patterns
- Layout systems

## Design Tokens

Design tokens ensure visual consistency across GC services.

### Colors

**Primary palette** (defined by canada.ca):
```css
/* Government of Canada brand colors */
--gc-red: #af3c43;           /* Primary brand color */
--gc-white: #ffffff;
--gc-black: #000000;
--gc-grey: #e1e4e7;          /* Light grey for backgrounds */
--gc-text: #333333;          /* Body text */
--gc-link: #284162;          /* Link color */
--gc-link-hover: #0535d2;    /* Link hover state */
```

**Usage in HTML**:
```html
<!-- Use standard Bootstrap/WET-BOEW classes -->
<div class="bg-white text-black">
    <a href="#" class="text-primary">Government of Canada link</a>
</div>
```

**Status colors**:
```css
--gc-success: #278400;  /* Success messages */
--gc-warning: #ff9900;  /* Warning messages */
--gc-danger: #d3080c;   /* Error messages */
--gc-info: #269abc;     /* Informational messages */
```

### Typography

**Font family**: Use the Canada.ca theme default fonts
```css
font-family: "Lato", "Noto Sans", sans-serif;
```

**Font sizes** (use WET-BOEW classes):
```html
<h1 class="gc-thickline">Page Title (h1)</h1>
<h2>Section Title (h2)</h2>
<h3>Subsection Title (h3)</h3>
<p>Body text (16px base)</p>
<p class="small">Small text (14px)</p>
```

### Spacing

**Margin and padding utilities** (WET-BOEW provides these):
```html
<!-- Margins -->
<div class="mrgn-tp-0">No top margin</div>
<div class="mrgn-tp-sm">Small top margin (15px)</div>
<div class="mrgn-tp-md">Medium top margin (20px)</div>
<div class="mrgn-tp-lg">Large top margin (30px)</div>
<div class="mrgn-tp-xl">Extra large top margin (50px)</div>

<!-- Bottom margins -->
<div class="mrgn-bttm-sm">Small bottom margin</div>
<div class="mrgn-bttm-md">Medium bottom margin</div>
<div class="mrgn-bttm-lg">Large bottom margin</div>

<!-- Padding -->
<div class="brdr-0">No border</div>
<div class="brdr-tp">Top border</div>
<div class="brdr-bttm">Bottom border</div>
```

**Full spacing reference**:
- `0` - No spacing (0px)
- `sm` - Small (15px)
- `md` - Medium (20px)
- `lg` - Large (30px)
- `xl` - Extra large (50px)

## Page Templates

### Standard Content Page

```html
<!DOCTYPE html>
<html class="no-js" lang="en" dir="ltr">
<head>
    <meta charset="utf-8">
    <title>Page Title - Canada.ca</title>
    <meta content="width=device-width,initial-scale=1" name="viewport">
    <link rel="stylesheet" href="https://www.canada.ca/etc/designs/canada/wet-boew/css/theme.min.css">
</head>
<body vocab="http://schema.org/" typeof="WebPage">
    <!-- Standard header -->
    <header><!-- WET-BOEW header --></header>

    <!-- Breadcrumb -->
    <nav id="wb-bc" property="breadcrumb">
        <h2>You are here:</h2>
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="https://www.canada.ca/en.html">Canada.ca</a></li>
                <li><a href="/service">Service name</a></li>
            </ol>
        </div>
    </nav>

    <!-- Main content -->
    <main role="main" property="mainContentOfPage" class="container">
        <h1 property="name" id="wb-cont" class="gc-thickline">Page Title</h1>

        <section>
            <h2>Section Title</h2>
            <p>Content here...</p>
        </section>
    </main>

    <!-- Standard footer -->
    <footer><!-- WET-BOEW footer --></footer>

    <script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/wet-boew.min.js"></script>
    <script src="https://www.canada.ca/etc/designs/canada/wet-boew/js/theme.min.js"></script>
</body>
</html>
```

### Service Initiation Template

For pages where users start a government service (applications, forms):

```html
<main role="main" property="mainContentOfPage" class="container">
    <h1 property="name" id="wb-cont" class="gc-thickline">Apply for [Service Name]</h1>

    <!-- Most requested (if applicable) -->
    <section class="band band-default mrgn-tp-lg">
        <h2>Most requested</h2>
        <ul class="lst-spcd colcount-md-2">
            <li><a href="#">Common task 1</a></li>
            <li><a href="#">Common task 2</a></li>
            <li><a href="#">Common task 3</a></li>
        </ul>
    </section>

    <!-- Services and information -->
    <section class="mrgn-tp-lg">
        <h2>Before you start</h2>
        <p>Key information users need before starting...</p>

        <h3>Eligibility</h3>
        <ul>
            <li>Requirement 1</li>
            <li>Requirement 2</li>
        </ul>

        <h3>What you'll need</h3>
        <ul>
            <li>Document 1</li>
            <li>Document 2</li>
        </ul>
    </section>

    <div class="mrgn-tp-lg">
        <a href="/apply/start" class="btn btn-call-to-action">Start your application</a>
    </div>
</main>
```

### Confirmation Page Template

For successful completion of a service:

```html
<main role="main" property="mainContentOfPage" class="container">
    <div class="alert alert-success">
        <h2 class="h3">Application submitted successfully</h2>
    </div>

    <h1 property="name" id="wb-cont">Confirmation</h1>

    <section class="panel panel-primary mrgn-tp-lg">
        <header class="panel-heading">
            <h2 class="panel-title">Your confirmation number</h2>
        </header>
        <div class="panel-body">
            <p class="mrgn-tp-0"><strong class="text-primary h2">AB-1234-5678-9012</strong></p>
            <p>Save this number for your records.</p>
        </div>
    </section>

    <section class="mrgn-tp-lg">
        <h2>What happens next</h2>
        <ol>
            <li>We'll review your application</li>
            <li>You'll receive an email within 10 business days</li>
            <li>Check your status online</li>
        </ol>
    </section>

    <section class="mrgn-tp-lg">
        <h2>Contact us</h2>
        <p>If you have questions about your application:</p>
        <ul>
            <li>Phone: 1-800-XXX-XXXX</li>
            <li>Email: <a href="mailto:info@example.gc.ca">info@example.gc.ca</a></li>
        </ul>
    </section>
</main>
```

## Common Components

### Call-to-Action Buttons

```html
<!-- Primary action -->
<a href="/next-step" class="btn btn-primary">Continue</a>

<!-- Call-to-action (most important action on page) -->
<a href="/apply" class="btn btn-call-to-action">Start your application</a>

<!-- Secondary action -->
<a href="/details" class="btn btn-default">Learn more</a>

<!-- Link styled as button -->
<a href="/back" class="btn btn-link">Go back</a>
```

### Contextual Alerts

```html
<!-- Success -->
<section class="alert alert-success">
    <h3>Success heading</h3>
    <p>Your changes have been saved.</p>
</section>

<!-- Information -->
<section class="alert alert-info">
    <h3>Information heading</h3>
    <p>This service is also available by phone.</p>
</section>

<!-- Warning -->
<section class="alert alert-warning">
    <h3>Warning heading</h3>
    <p>You must submit by the deadline.</p>
</section>

<!-- Danger/Error -->
<section class="alert alert-danger">
    <h3>Error heading</h3>
    <p>There was a problem processing your request.</p>
</section>
```

### Panels (Highlighted Content)

```html
<!-- Primary panel -->
<section class="panel panel-primary">
    <header class="panel-heading">
        <h2 class="panel-title">Panel Title</h2>
    </header>
    <div class="panel-body">
        <p>Important content highlighted in a panel.</p>
    </div>
</section>

<!-- Default panel -->
<section class="panel panel-default">
    <header class="panel-heading">
        <h2 class="panel-title">Panel Title</h2>
    </header>
    <div class="panel-body">
        <p>Regular content in a panel.</p>
    </div>
</section>
```

### Wells (Background Highlighting)

```html
<!-- Default well (light grey background) -->
<div class="well">
    <h3>Highlighted Information</h3>
    <p>Content that needs visual separation from main content.</p>
</div>

<!-- Small well -->
<div class="well well-sm">
    <p>Compact highlighted content.</p>
</div>
```

### Badges and Labels

```html
<!-- Badges (numerical indicators) -->
<p>Messages <span class="badge">3</span></p>

<!-- Labels (status indicators) -->
<span class="label label-default">Draft</span>
<span class="label label-primary">Active</span>
<span class="label label-success">Approved</span>
<span class="label label-info">New</span>
<span class="label label-warning">Pending</span>
<span class="label label-danger">Rejected</span>
```

## Layout Utilities

### Grid System (Bootstrap 3 based)

```html
<div class="container">
    <div class="row">
        <div class="col-md-8">
            <p>Main content (8 columns)</p>
        </div>
        <div class="col-md-4">
            <p>Sidebar (4 columns)</p>
        </div>
    </div>
</div>

<!-- Responsive columns -->
<div class="row">
    <div class="col-xs-12 col-sm-6 col-md-4">
        <p>Responsive column</p>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-4">
        <p>Responsive column</p>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-4">
        <p>Responsive column</p>
    </div>
</div>
```

**Breakpoints**:
- `xs` - Extra small devices (< 768px) - phones
- `sm` - Small devices (≥ 768px) - tablets
- `md` - Medium devices (≥ 992px) - desktops
- `lg` - Large devices (≥ 1200px) - large desktops

### Visibility Utilities

```html
<!-- Show only on specific screen sizes -->
<p class="visible-xs">Visible only on extra small screens</p>
<p class="visible-sm">Visible only on small screens</p>
<p class="visible-md">Visible only on medium screens</p>
<p class="visible-lg">Visible only on large screens</p>

<!-- Hide on specific screen sizes -->
<p class="hidden-xs">Hidden on extra small screens</p>
<p class="hidden-sm">Hidden on small screens</p>
<p class="hidden-md">Hidden on medium screens</p>
<p class="hidden-lg">Hidden on large screens</p>
```

### Text Utilities

```html
<!-- Alignment -->
<p class="text-left">Left aligned text</p>
<p class="text-center">Center aligned text</p>
<p class="text-right">Right aligned text</p>
<p class="text-justify">Justified text</p>

<!-- Transformation -->
<p class="text-lowercase">lowercase text</p>
<p class="text-uppercase">UPPERCASE TEXT</p>
<p class="text-capitalize">capitalized text</p>

<!-- Emphasis -->
<p class="text-muted">Muted text (light grey)</p>
<p class="text-primary">Primary color text</p>
<p class="text-success">Success color text</p>
<p class="text-info">Info color text</p>
<p class="text-warning">Warning color text</p>
<p class="text-danger">Danger color text</p>
```

## Lists

### Definition Lists

```html
<dl class="dl-horizontal">
    <dt>Term 1</dt>
    <dd>Definition 1</dd>

    <dt>Term 2</dt>
    <dd>Definition 2</dd>
</dl>
```

### List Styles

```html
<!-- Unstyled list (no bullets) -->
<ul class="list-unstyled">
    <li>Item 1</li>
    <li>Item 2</li>
</ul>

<!-- Inline list -->
<ul class="list-inline">
    <li>Item 1</li>
    <li>Item 2</li>
    <li>Item 3</li>
</ul>

<!-- Spaced list -->
<ul class="lst-spcd">
    <li>Item with more vertical spacing</li>
    <li>Item with more vertical spacing</li>
</ul>

<!-- Multi-column list -->
<ul class="colcount-md-2">
    <li>Item 1</li>
    <li>Item 2</li>
    <li>Item 3</li>
    <li>Item 4</li>
</ul>
```

## Icons (Font Awesome via WET-BOEW)

```html
<!-- Common icons -->
<span class="glyphicon glyphicon-ok" aria-hidden="true"></span> <!-- Checkmark -->
<span class="glyphicon glyphicon-remove" aria-hidden="true"></span> <!-- X mark -->
<span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span> <!-- Warning -->
<span class="glyphicon glyphicon-question-sign" aria-hidden="true"></span> <!-- Question -->
<span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span> <!-- Info -->
<span class="glyphicon glyphicon-envelope" aria-hidden="true"></span> <!-- Email -->
<span class="glyphicon glyphicon-phone" aria-hidden="true"></span> <!-- Phone -->
<span class="glyphicon glyphicon-download" aria-hidden="true"></span> <!-- Download -->

<!-- IMPORTANT: Always include descriptive text or aria-label for accessibility -->
<a href="/download">
    <span class="glyphicon glyphicon-download" aria-hidden="true"></span>
    <span class="wb-inv">Download application form</span>
</a>
```

## Best Practices

1. **Use standard templates** - Start with canada.ca templates, don't create custom layouts
2. **Apply utility classes** - Use WET-BOEW utilities (mrgn-*, brdr-*, etc.) instead of custom CSS
3. **Follow grid system** - Use responsive grid columns (col-xs-*, col-md-*, etc.)
4. **Maintain consistency** - Use standard components (buttons, alerts, panels) as defined
5. **Test responsiveness** - Verify layout works on mobile, tablet, and desktop
6. **Check accessibility** - All components must maintain WCAG 2.1 AA compliance

## Common Mistakes to Avoid

❌ **Don't create custom button styles** - Use btn-primary, btn-call-to-action, etc.
❌ **Don't use arbitrary spacing** - Use margin/padding utilities (mrgn-tp-lg, etc.)
❌ **Don't break the grid** - Always wrap columns in `.row` and `.container`
❌ **Don't use inline styles** - Use utility classes instead
❌ **Don't forget mobile** - Test all layouts on small screens
❌ **Don't mix design systems** - Stick to GC Design System + WET-BOEW patterns

## Resources

- **Official Design System**: https://design.canada.ca/
- **WET-BOEW Components**: https://wet-boew.github.io/wet-boew/index-en.html
- **Canada.ca Content Style Guide**: https://www.canada.ca/en/treasury-board-secretariat/services/government-communications/canada-content-style-guide.html
- **Pattern Library**: https://www.canada.ca/en/government/about/design-system/pattern-library.html

## Version Information

This guidance is current for:
- GC Design System (canada.ca theme)
- WET-BOEW 4.0.x
- Bootstrap 3 grid system

Last updated: 2025-02-11
