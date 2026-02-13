# GC Design System - Page Templates Instructions

## Overview

The GC Design System provides pre-built page templates for common Government of Canada service pages. These templates follow Canada.ca patterns and provide consistent structure, information hierarchy, and user experience.

## Service Initiation Page Template

Use this template for pages where users start a service or application:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>Apply for [Service Name] - Canada.ca</title>
    <!-- WET-BOEW CSS -->
</head>
<body vocab="http://schema.org/" typeof="WebPage">
    <div id="wb-main">
        <main property="mainContentOfPage" class="container">
            <div class="row">
                <div class="col-md-8">
                    <h1 property="name" id="wb-cont">Apply for [Service Name]</h1>

                    <!-- Service description -->
                    <p>Brief description of what this service does and who it's for.</p>

                    <!-- Eligibility section -->
                    <section>
                        <h2>Eligibility</h2>
                        <p>You may be eligible if you:</p>
                        <ul>
                            <li>Eligibility criterion 1</li>
                            <li>Eligibility criterion 2</li>
                            <li>Eligibility criterion 3</li>
                        </ul>
                    </section>

                    <!-- What you need section -->
                    <section>
                        <h2>What you need to apply</h2>
                        <p>Before you start, make sure you have:</p>
                        <ul>
                            <li>Required document 1</li>
                            <li>Required document 2</li>
                            <li>Required information 3</li>
                        </ul>
                    </section>

                    <!-- How to apply section -->
                    <section>
                        <h2>How to apply</h2>
                        <p>Processing time: [X weeks]</p>
                        <a href="/apply" class="btn btn-primary btn-lg">Start application</a>
                    </section>
                </div>

                <!-- Sidebar -->
                <div class="col-md-4">
                    <section class="panel panel-default">
                        <header class="panel-heading">
                            <h2 class="panel-title">Related links</h2>
                        </header>
                        <div class="panel-body">
                            <ul>
                                <li><a href="#">Related service 1</a></li>
                                <li><a href="#">Related service 2</a></li>
                            </ul>
                        </div>
                    </section>

                    <section class="panel panel-default">
                        <header class="panel-heading">
                            <h2 class="panel-title">Contact us</h2>
                        </header>
                        <div class="panel-body">
                            <p>Phone: 1-800-XXX-XXXX</p>
                            <p>Email: <a href="mailto:service@example.gc.ca">service@example.gc.ca</a></p>
                        </div>
                    </section>
                </div>
            </div>
        </main>
    </div>
</body>
</html>
```

## Confirmation Page Template

Use this template for confirmation pages after form submission:

```html
<div class="container">
    <div class="row">
        <div class="col-md-8">
            <!-- Success alert -->
            <div class="alert alert-success">
                <h2>Application submitted successfully</h2>
                <p><strong>Confirmation number:</strong> APP-2024-001234</p>
            </div>

            <!-- What happens next -->
            <section>
                <h2>What happens next</h2>
                <ol>
                    <li>We'll review your application (typically 5-10 business days)</li>
                    <li>You'll receive an email confirmation at: user@example.com</li>
                    <li>If approved, you'll receive [outcome] within [timeframe]</li>
                </ol>
            </section>

            <!-- Application summary -->
            <section>
                <h2>Application summary</h2>
                <dl class="dl-horizontal">
                    <dt>Application ID:</dt>
                    <dd>APP-2024-001234</dd>

                    <dt>Date submitted:</dt>
                    <dd>February 13, 2024</dd>

                    <dt>Applicant:</dt>
                    <dd>John Smith</dd>
                </dl>
            </section>

            <!-- Actions -->
            <section>
                <h2>What you can do</h2>
                <ul>
                    <li><a href="/check-status">Check application status</a></li>
                    <li><a href="/print-confirmation">Print this confirmation</a></li>
                    <li><a href="/">Return to homepage</a></li>
                </ul>
            </section>
        </div>

        <div class="col-md-4">
            <!-- Contact information if questions -->
            <section class="panel panel-default">
                <header class="panel-heading">
                    <h3 class="panel-title">Need help?</h3>
                </header>
                <div class="panel-body">
                    <p>If you have questions about your application:</p>
                    <p>Phone: 1-800-XXX-XXXX<br>
                    Email: <a href="mailto:help@example.gc.ca">help@example.gc.ca</a></p>
                </div>
            </section>
        </div>
    </div>
</div>
```

## Error Page Template

Use this template for error pages (404, 500, etc.):

```html
<div class="container">
    <div class="row">
        <div class="col-md-8">
            <h1>We couldn't find that page</h1>

            <p>We're sorry you ended up here. Sometimes a page gets moved or deleted, but hopefully we can help you find what you're looking for.</p>

            <section>
                <h2>What to do</h2>
                <ul>
                    <li>Return to the <a href="/">home page</a></li>
                    <li>Use the search box above to find what you're looking for</li>
                    <li><a href="/contact">Contact us</a> if you continue to have problems</li>
                </ul>
            </section>
        </div>
    </div>
</div>
```

## Form Page Template

Use this template for multi-step forms:

```html
<div class="container">
    <div class="row">
        <div class="col-md-8">
            <h1>Apply for [Service Name]</h1>

            <!-- Progress indicator -->
            <div class="wb-steps">
                <ol>
                    <li>Personal information</li>
                    <li class="active">Contact details</li>
                    <li>Review and submit</li>
                </ol>
            </div>

            <!-- Form section -->
            <form method="post" action="/submit" class="wb-frmvld">
                <fieldset>
                    <legend>Contact details</legend>

                    <!-- Form fields here -->

                </fieldset>

                <!-- Navigation buttons -->
                <div class="form-group">
                    <a href="/step-1" class="btn btn-default">Previous</a>
                    <button type="submit" class="btn btn-primary">Next</button>
                </div>
            </form>
        </div>

        <!-- Sidebar with help -->
        <div class="col-md-4">
            <section class="panel panel-default">
                <header class="panel-heading">
                    <h2 class="panel-title">Need help?</h2>
                </header>
                <div class="panel-body">
                    <p>If you're having trouble completing this form:</p>
                    <p>Phone: 1-800-XXX-XXXX</p>
                </div>
            </section>
        </div>
    </div>
</div>
```

## Standard Page Layout Structure

All GC Design System pages follow this structure:

### Layout Grid
- **8-column main content + 4-column sidebar** for most service pages
- **12-column full width** for landing pages, confirmation pages
- **10-column centered** for long-form content (articles, guides)

### Information Hierarchy
1. **Page title (H1)** - Clear, descriptive page title
2. **Introduction** - Brief description (1-2 sentences)
3. **Main sections (H2)** - Organized by user task flow
4. **Subsections (H3)** - Additional detail as needed
5. **Call to action** - Primary button or link

### Sidebar Content
Common sidebar elements:
- Related links
- Contact information
- Additional resources
- Important notices

## Responsive Considerations

All templates use responsive grid classes:
- **Mobile (default)**: Single column (col-12)
- **Tablet+ (col-md-8/col-md-4)**: Two columns
- **Desktop**: Same as tablet (GC Design System prioritizes tablet+ layouts)

## Accessibility Requirements

All page templates must include:
- Proper heading hierarchy (H1 → H2 → H3, no skipping)
- Landmark regions (`<main>`, `<nav>`, `<section>`)
- Skip links for keyboard navigation
- Bilingual content (separate pages or inline)
- Sufficient color contrast
- Focus indicators for keyboard navigation

## Bilingual Templates

For bilingual implementations, create separate pages:
- `service-en.html` and `service-fr.html`
- Include language toggle in header
- Maintain consistent structure across languages
- Translate all content, including alt text and ARIA labels

## See Also

- [GC Design System - Page Templates](https://design-system.canada.ca/)
- [Canada.ca Content and Information Architecture Specification](https://design.canada.ca/architecture/canada-content-information-architecture-specification.html)
- [utilities.instructions.md](/gc-ai-instructions/gc-design-system/utilities.instructions.md)
- [accessibility.instructions.md](/gc-ai-instructions/accessibility/accessibility.instructions.md)
