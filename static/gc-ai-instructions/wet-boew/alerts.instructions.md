# WET-BOEW Alerts Instructions

## Overview

This file provides guidance on creating accessible alert messages using WET-BOEW for Government of Canada applications.

## Alert Types

WET-BOEW supports four alert types that follow WCAG 2.1 AA requirements:

### Success Alerts

Use for successful operations:

```html
<div class="alert alert-success">
    <h3>Success / <span lang="fr">Succès</span></h3>
    <p>
        <span lang="en">Your application has been submitted successfully.</span>
        <span lang="fr">Votre demande a été soumise avec succès.</span>
    </p>
</div>
```

### Info Alerts

Use for informational messages:

```html
<div class="alert alert-info">
    <h3>Information / <span lang="fr">Information</span></h3>
    <p>
        <span lang="en">Processing may take up to 5 business days.</span>
        <span lang="fr">Le traitement peut prendre jusqu'à 5 jours ouvrables.</span>
    </p>
</div>
```

### Warning Alerts

Use for warnings that require attention:

```html
<div class="alert alert-warning">
    <h3>Warning / <span lang="fr">Avertissement</span></h3>
    <p>
        <span lang="en">Your session will expire in 5 minutes.</span>
        <span lang="fr">Votre session expirera dans 5 minutes.</span>
    </p>
</div>
```

### Danger/Error Alerts

Use for errors or critical issues:

```html
<div class="alert alert-danger">
    <h3>Error / <span lang="fr">Erreur</span></h3>
    <p>
        <span lang="en">The form contains errors. Please review and try again.</span>
        <span lang="fr">Le formulaire contient des erreurs. Veuillez vérifier et réessayer.</span>
    </p>
</div>
```

## Dismissible Alerts

Add the ability to close alerts:

```html
<div class="alert alert-info alert-dismissible">
    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
        <span aria-hidden="true">&times;</span>
    </button>
    <h3>Information</h3>
    <p>This is a dismissible alert.</p>
</div>
```

## Accessibility Requirements

- Always include a heading (`<h3>`) to provide context
- Provide bilingual content for both the heading and message
- Use semantic HTML and appropriate ARIA attributes
- Ensure sufficient color contrast (built into WET-BOEW alert styles)
- For dynamically added alerts, consider using `role="alert"` to announce to screen readers

## Form Validation Errors

For form-specific errors, combine with a list of errors:

```html
<div class="alert alert-danger">
    <h3>Errors found / <span lang="fr">Erreurs détectées</span></h3>
    <ul>
        <li>
            <a href="#name">
                <span lang="en">Full name is required</span>
                <span lang="fr">Le nom complet est obligatoire</span>
            </a>
        </li>
        <li>
            <a href="#email">
                <span lang="en">Email address is invalid</span>
                <span lang="fr">L'adresse courriel est invalide</span>
            </a>
        </li>
    </ul>
</div>
```

## See Also

- [WET-BOEW Alerts](https://wet-boew.github.io/wet-boew/demos/alerts/alerts-en.html)
- [forms.instructions.md](/gc-ai-instructions/wet-boew/forms.instructions.md)
- [accessibility.instructions.md](/gc-ai-instructions/accessibility/accessibility.instructions.md)
