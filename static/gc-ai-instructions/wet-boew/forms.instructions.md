# WET-BOEW Forms Instructions

## Overview

This file provides detailed guidance on creating accessible, bilingual forms using WET-BOEW components for Government of Canada applications.

## Basic Form Structure

All forms must follow WET-BOEW patterns for WCAG 2.1 AA compliance:

```html
<form method="post" action="/submit" class="wb-frmvld">
    <div class="form-group">
        <label for="name">
            <span class="field-name">Full name</span>
            <span lang="fr">Nom complet</span>
            <strong class="required">(required)</strong>
        </label>
        <input
            class="form-control"
            id="name"
            name="name"
            type="text"
            data-rule-required="true"
            data-msg-required="This field is required."
            data-msg-required-fr="Ce champ est obligatoire."
        />
    </div>
</form>
```

## Form Validation

WET-BOEW provides built-in validation through the `wb-frmvld` class:

- Add `data-rule-required="true"` for required fields
- Use `data-rule-email="true"` for email validation
- Use `data-rule-digits="true"` for numeric fields
- Provide bilingual error messages with `data-msg-*` and `data-msg-*-fr` attributes

## Required Field Indicators

Always mark required fields with:
```html
<strong class="required">(required)</strong>
```

## Input Types

Use semantic HTML5 input types:
- `type="email"` for email addresses
- `type="tel"` for phone numbers
- `type="date"` for dates
- `type="number"` for numeric input

## Accessibility Requirements

- Every input must have a corresponding `<label>` with matching `for` attribute
- Group related fields using `<fieldset>` and `<legend>`
- Provide clear error messages that identify the field and explain how to fix the error
- Ensure keyboard navigation works properly (tab order is logical)

## Bilingual Forms

All form labels, placeholders, error messages, and help text must be provided in both English and French:

```html
<label for="email">
    <span class="field-name">Email address</span>
    <span lang="fr">Adresse courriel</span>
</label>
```

## Submit Buttons

```html
<div class="form-group">
    <button type="submit" class="btn btn-primary">
        <span lang="en">Submit</span>
        <span lang="fr">Soumettre</span>
    </button>
</div>
```

## See Also

- [WET-BOEW Form Validation](https://wet-boew.github.io/wet-boew/demos/formvalid/formvalid-en.html)
- [bilingual.instructions.md](/gc-ai-instructions/wet-boew/bilingual.instructions.md)
- [accessibility.instructions.md](/gc-ai-instructions/accessibility/accessibility.instructions.md)
