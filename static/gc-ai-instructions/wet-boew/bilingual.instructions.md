# WET-BOEW Bilingual Content Instructions

## Overview

All Government of Canada web content must be available in both English and French per the Official Languages Act. This file provides patterns for implementing bilingual content using WET-BOEW.

## Language Attribute

Always specify the primary language of the page in the `<html>` tag:

```html
<!-- English page -->
<html lang="en">

<!-- French page -->
<html lang="fr">
```

## Inline Bilingual Content

For small amounts of bilingual content on the same page:

### Using `lang` Attribute

```html
<h1>
    <span lang="en">Contact Us</span>
    <span lang="fr">Contactez-nous</span>
</h1>
```

### Using Separator

```html
<label for="name">
    Full Name / <span lang="fr">Nom complet</span>
</label>
```

## Separate Pages Pattern

For most Government of Canada websites, use separate pages for each language:

### English Page (`contact-en.html`)
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Contact Us - Department Name</title>
</head>
<body>
    <!-- Language toggle -->
    <a href="contact-fr.html" lang="fr">Français</a>

    <h1>Contact Us</h1>
    <!-- English content -->
</body>
</html>
```

### French Page (`contact-fr.html`)
```html
<!DOCTYPE html>
<html lang="fr">
<head>
    <title>Contactez-nous - Nom du ministère</title>
</head>
<body>
    <!-- Language toggle -->
    <a href="contact-en.html" lang="en">English</a>

    <h1>Contactez-nous</h1>
    <!-- French content -->
</body>
</html>
```

## Language Toggle Link

Always provide a language toggle link in the header:

```html
<div id="wb-lng">
    <h2>Language selection</h2>
    <ul class="list-inline">
        <li>
            <a lang="fr" href="contact-fr.html">Français</a>
        </li>
    </ul>
</div>
```

## Form Labels (Bilingual)

### Inline Bilingual Labels
```html
<label for="email">
    <span class="field-name">Email address</span>
    <span lang="fr">Adresse courriel</span>
</label>
```

### Separate Page Forms

For forms on separate language pages, use single-language labels:

**English Page:**
```html
<label for="email">Email address</label>
```

**French Page:**
```html
<label for="email">Adresse courriel</label>
```

## Error Messages (Bilingual)

When using WET-BOEW form validation on bilingual pages:

```html
<input
    type="email"
    id="email"
    data-rule-required="true"
    data-msg-required="This field is required."
    data-msg-required-fr="Ce champ est obligatoire."
    data-rule-email="true"
    data-msg-email="Please enter a valid email address."
    data-msg-email-fr="Veuillez entrer une adresse courriel valide."
/>
```

## Button Labels

### Inline Bilingual
```html
<button type="submit" class="btn btn-primary">
    <span lang="en">Submit</span> /
    <span lang="fr">Soumettre</span>
</button>
```

### Separate Pages
Use language-appropriate text on each page.

## Navigation Menus

For bilingual menus, ensure menu items are translated:

**English:**
```html
<nav>
    <ul>
        <li><a href="home-en.html">Home</a></li>
        <li><a href="services-en.html">Services</a></li>
        <li><a href="contact-en.html">Contact</a></li>
    </ul>
</nav>
```

**French:**
```html
<nav>
    <ul>
        <li><a href="home-fr.html">Accueil</a></li>
        <li><a href="services-fr.html">Services</a></li>
        <li><a href="contact-fr.html">Contactez-nous</a></li>
    </ul>
</nav>
```

## Official Terminology

Use **TERMIUM Plus®** ([https://www.btb.termiumplus.gc.ca/](https://www.btb.termiumplus.gc.ca/)) to verify official Government of Canada terminology.

Common terms:
- Social Insurance Number → Numéro d'assurance sociale (NAS)
- Employment Insurance → Assurance-emploi
- Canada Revenue Agency → Agence du revenu du Canada
- Service Canada → Service Canada

## Content Translation Guidelines

1. **Never use machine translation alone** - always have translations reviewed by qualified bilingual staff
2. **Maintain equivalent meaning** - translations should convey the same information, not just literal word-for-word translation
3. **Consider cultural context** - some concepts may need to be explained differently in French
4. **Keep formatting consistent** - headings, lists, and structure should match across languages
5. **Match tone and reading level** - if English is grade 8 reading level, French should be equivalent

## Accessibility Considerations

- Screen readers use the `lang` attribute to pronounce text correctly
- Always specify `lang` when switching languages inline
- Ensure language toggle is keyboard accessible
- Provide clear indication of which language is currently active

## Testing Checklist

- [ ] All user-facing text is available in both languages
- [ ] `lang` attributes are correctly applied
- [ ] Language toggle link works correctly
- [ ] Error messages appear in the correct language
- [ ] Form validation messages are bilingual (if using inline bilingual forms)
- [ ] Navigation is consistent across both languages
- [ ] Official terminology verified using TERMIUM Plus®

## See Also

- [TERMIUM Plus® - Official Terminology Database](https://www.btb.termiumplus.gc.ca/)
- [Canada.ca Content Style Guide - Bilingual Content](https://design.canada.ca/style-guide/)
- [Official Languages Act Requirements](https://www.canada.ca/en/canadian-heritage/services/official-languages-bilingualism.html)
- [forms.instructions.md](/gc-ai-instructions/wet-boew/forms.instructions.md)
