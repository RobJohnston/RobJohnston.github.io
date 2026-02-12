---
applyTo: "**/*.html,**/*.cshtml,**/*.json,**/Views/**,**/Resources/**"
---

# Bilingual Content Instructions (Official Languages Act)

All Government of Canada web services must be available in both English and French per the **Official Languages Act**.

## Legal Requirements

- **Official Languages Act** (1985, c. 31)
- **Policy on Official Languages** (Treasury Board)
- **Standard on Web Accessibility** (includes language requirements)

**Key principle**: Both official languages must have **equal quality, meaning, and prominence**.

## Language Implementation Strategies

### Strategy 1: Separate Pages (Recommended)

Most common for Government of Canada sites:

```
/en/services/apply           (English page)
/fr/services/demander        (French page)
```

**Advantages**:
- SEO-friendly (separate URLs for each language)
- Clean URL structure
- Easy to deploy/cache separately

**HTML structure**:
```html
<!-- English page: /en/services/apply -->
<!DOCTYPE html>
<html class="no-js" lang="en" dir="ltr">
<head>
    <meta charset="utf-8">
    <title>Apply for Employment Insurance - Canada.ca</title>
    <link rel="alternate" hreflang="fr" href="/fr/services/demander" />
</head>
<body>
    <!-- Language toggle -->
    <section id="wb-lng" class="col-xs-3 col-sm-12 text-right">
        <h2 class="wb-inv">Language selection</h2>
        <ul class="list-inline margin-bottom-none">
            <li><a lang="fr" href="/fr/services/demander">Français</a></li>
        </ul>
    </section>

    <main>
        <h1>Apply for Employment Insurance</h1>
        <p>Employment Insurance (EI) provides temporary financial assistance...</p>
    </main>
</body>
</html>

<!-- French page: /fr/services/demander -->
<!DOCTYPE html>
<html class="no-js" lang="fr" dir="ltr">
<head>
    <meta charset="utf-8">
    <title>Demander l'assurance-emploi - Canada.ca</title>
    <link rel="alternate" hreflang="en" href="/en/services/apply" />
</head>
<body>
    <!-- Sélection de la langue -->
    <section id="wb-lng" class="col-xs-3 col-sm-12 text-right">
        <h2 class="wb-inv">Sélection de la langue</h2>
        <ul class="list-inline margin-bottom-none">
            <li><a lang="en" href="/en/services/apply">English</a></li>
        </ul>
    </section>

    <main>
        <h1>Demander l'assurance-emploi</h1>
        <p>L'assurance-emploi (AE) fournit une aide financière temporaire...</p>
    </main>
</body>
</html>
```

### Strategy 2: Query Parameter

Alternative approach using `?lang=` parameter:

```
/services/apply?lang=en      (English)
/services/apply?lang=fr      (French)
```

**Server-side language detection** (ASP.NET Core):
```csharp
public class LanguageController : Controller
{
    public IActionResult Index(string lang = "en")
    {
        // Validate language
        if (lang != "en" && lang != "fr")
            lang = "en";

        // Store language preference
        Response.Cookies.Append("language", lang, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            HttpOnly = false,
            IsEssential = true
        });

        // Set culture for localization
        var culture = new System.Globalization.CultureInfo(lang == "fr" ? "fr-CA" : "en-CA");
        System.Globalization.CultureInfo.CurrentCulture = culture;
        System.Globalization.CultureInfo.CurrentUICulture = culture;

        ViewBag.CurrentLanguage = lang;
        return View();
    }
}
```

**Razor view** (.cshtml):
```cshtml
@{
    var lang = ViewBag.CurrentLanguage ?? "en";
    var isEnglish = lang == "en";
}
<!DOCTYPE html>
<html class="no-js" lang="@lang" dir="ltr">
<head>
    <meta charset="utf-8">
    <title>@(isEnglish ? "Apply for Employment Insurance" : "Demander l'assurance-emploi") - Canada.ca</title>
</head>
<body>
    <!-- Language toggle -->
    <section id="wb-lng" class="col-xs-3 col-sm-12 text-right">
        <h2 class="wb-inv">@(isEnglish ? "Language selection" : "Sélection de la langue")</h2>
        <ul class="list-inline margin-bottom-none">
            <li><a lang="@(isEnglish ? "fr" : "en")" href="?lang=@(isEnglish ? "fr" : "en")">
                @(isEnglish ? "Français" : "English")
            </a></li>
        </ul>
    </section>

    <main>
        <h1>@(isEnglish ? "Apply for Employment Insurance" : "Demander l'assurance-emploi")</h1>
        <p>@(isEnglish ? "Employment Insurance (EI) provides..." : "L'assurance-emploi (AE) fournit...")</p>
    </main>
</body>
</html>
```

## Content Organization

### Resource Files (.NET)

Use resource files for translation strings:

**Resources/Strings.resx** (English - default):
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="PageTitle" xml:space="preserve">
    <value>Apply for Employment Insurance</value>
  </data>
  <data name="SubmitButton" xml:space="preserve">
    <value>Submit application</value>
  </data>
  <data name="RequiredField" xml:space="preserve">
    <value>(required)</value>
  </data>
</root>
```

**Resources/Strings.fr.resx** (French):
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="PageTitle" xml:space="preserve">
    <value>Demander l'assurance-emploi</value>
  </data>
  <data name="SubmitButton" xml:space="preserve">
    <value>Soumettre la demande</value>
  </data>
  <data name="RequiredField" xml:space="preserve">
    <value>(obligatoire)</value>
  </data>
</root>
```

**Using in C# code**:
```csharp
using Resources;

public class ApplicationController : Controller
{
    public IActionResult Index()
    {
        ViewBag.PageTitle = Strings.PageTitle;
        ViewBag.SubmitButton = Strings.SubmitButton;
        return View();
    }
}
```

**Using in Razor views**:
```cshtml
@using Resources

<h1>@Strings.PageTitle</h1>
<button type="submit">@Strings.SubmitButton</button>
```

### JSON Translation Files

Alternative approach using JSON:

**translations/en.json**:
```json
{
  "page_title": "Apply for Employment Insurance",
  "submit_button": "Submit application",
  "required_field": "(required)",
  "validation": {
    "email_invalid": "Please enter a valid email address",
    "field_required": "This field is required"
  }
}
```

**translations/fr.json**:
```json
{
  "page_title": "Demander l'assurance-emploi",
  "submit_button": "Soumettre la demande",
  "required_field": "(obligatoire)",
  "validation": {
    "email_invalid": "Veuillez entrer une adresse courriel valide",
    "field_required": "Ce champ est obligatoire"
  }
}
```

## Common Bilingual Patterns

### Form Labels

```html
<!-- English -->
<label for="email" class="required">
    <span class="field-name">Email address</span>
    <strong class="required">(required)</strong>
</label>
<input type="email" id="email" name="email" required
       data-msg="Please enter a valid email address" />

<!-- French -->
<label for="email" class="required">
    <span class="field-name">Adresse courriel</span>
    <strong class="required">(obligatoire)</strong>
</label>
<input type="email" id="email" name="email" required
       data-msg="Veuillez entrer une adresse courriel valide" />
```

### Buttons

```html
<!-- English -->
<button type="submit" class="btn btn-primary">Submit</button>
<button type="button" class="btn btn-default">Cancel</button>
<a href="/apply" class="btn btn-call-to-action">Start your application</a>

<!-- French -->
<button type="submit" class="btn btn-primary">Soumettre</button>
<button type="button" class="btn btn-default">Annuler</button>
<a href="/demander" class="btn btn-call-to-action">Commencer votre demande</a>
```

### Alerts

```html
<!-- English -->
<section class="alert alert-success">
    <h3>Application submitted successfully</h3>
    <p>Your confirmation number is: <strong>AB-1234-5678</strong></p>
</section>

<!-- French -->
<section class="alert alert-success">
    <h3>Demande soumise avec succès</h3>
    <p>Votre numéro de confirmation est : <strong>AB-1234-5678</strong></p>
</section>
```

### Date Formats

```html
<!-- English (YYYY-MM-DD or Month DD, YYYY) -->
<p>Application date: <time datetime="2025-02-11">February 11, 2025</time></p>

<!-- French (YYYY-MM-DD or DD month YYYY) -->
<p>Date de la demande : <time datetime="2025-02-11">11 février 2025</time></p>
```

**C# formatting**:
```csharp
// English
var dateEn = DateTime.Now.ToString("MMMM dd, yyyy", new CultureInfo("en-CA"));
// Output: February 11, 2025

// French
var dateFr = DateTime.Now.ToString("dd MMMM yyyy", new CultureInfo("fr-CA"));
// Output: 11 février 2025
```

### Numbers and Currency

```html
<!-- English -->
<p>Total amount: $1,234.56</p>
<p>Quantity: 1,000 items</p>

<!-- French -->
<p>Montant total : 1 234,56 $</p>
<p>Quantité : 1 000 articles</p>
```

**C# formatting**:
```csharp
// English
var amountEn = 1234.56m.ToString("C", new CultureInfo("en-CA"));
// Output: $1,234.56

// French
var amountFr = 1234.56m.ToString("C", new CultureInfo("fr-CA"));
// Output: 1 234,56 $
```

## Translation Quality Standards

### 1. Accuracy
- French translation must convey the same meaning as English
- Technical terms must be accurate (use TERMIUM Plus®)
- Legal terms require legal equivalents (not literal translation)

### 2. Clarity
- Use plain language in both languages
- Avoid jargon unless necessary
- Define acronyms on first use

### 3. Cultural Appropriateness
- Adapt examples to Canadian context
- Use appropriate forms of address ("vous" vs "tu" - always use "vous")
- Consider Quebec vs other francophone regions

### 4. Consistency
- Use consistent terminology throughout application
- Reference TERMIUM Plus® for approved government terminology
- Maintain consistent tone and style

## TERMIUM Plus® Integration

**TERMIUM Plus®** is the Government of Canada's official terminology database.

**Access**: https://www.btb.termiumplus.gc.ca/

**Common Government Terms**:

| English | French (TERMIUM Plus®) |
|---------|------------------------|
| Apply | Demander / Faire une demande |
| Application | Demande |
| Submit | Soumettre / Présenter |
| Eligibility | Admissibilité |
| Benefits | Prestations |
| Employment Insurance | Assurance-emploi |
| Social Insurance Number | Numéro d'assurance sociale |
| Confirmation number | Numéro de confirmation |
| Processing time | Délai de traitement |
| Required field | Champ obligatoire |
| Email address | Adresse courriel (NOT "adresse e-mail") |
| Sign in | Ouvrir une session (NOT "se connecter") |
| Sign out | Fermer la session |

## Language Toggle Requirements

### Placement
Must appear in the same location on every page (top right header area):

```html
<section id="wb-lng" class="col-xs-3 col-sm-12 text-right">
    <h2 class="wb-inv">Language selection</h2>
    <ul class="list-inline margin-bottom-none">
        <li><a lang="fr" href="/fr/page-name">Français</a></li>
    </ul>
</section>
```

### Link to Equivalent Page
Language toggle must link to the **equivalent page** in the other language, not the home page:

```html
<!-- ✅ GOOD: Links to equivalent page -->
<!-- On /en/services/apply -->
<a lang="fr" href="/fr/services/demander">Français</a>

<!-- On /fr/services/demander -->
<a lang="en" href="/en/services/apply">English</a>

<!-- ❌ BAD: Links to home page -->
<!-- On /en/services/apply -->
<a lang="fr" href="/fr/">Français</a>
```

### Visual Prominence
Language toggle must be equally prominent in both languages:
- Same font size
- Same positioning
- Same visual weight

## JavaScript Localization

For client-side messages:

```javascript
const translations = {
    en: {
        confirm_delete: 'Are you sure you want to delete this item?',
        cancel: 'Cancel',
        delete: 'Delete',
        success: 'Item deleted successfully'
    },
    fr: {
        confirm_delete: 'Êtes-vous sûr de vouloir supprimer cet élément?',
        cancel: 'Annuler',
        delete: 'Supprimer',
        success: 'Élément supprimé avec succès'
    }
};

// Get current language from HTML lang attribute
const currentLang = document.documentElement.lang || 'en';

function t(key) {
    return translations[currentLang][key] || key;
}

// Usage
if (confirm(t('confirm_delete'))) {
    deleteItem();
    alert(t('success'));
}
```

## Logging and Error Messages

Error messages in logs should be in English (for developer consumption):

```csharp
// ✅ GOOD: English log messages
_logger.LogError("Failed to process application for user {UserId}", userId);
_logger.LogWarning("Invalid email format: {Email}", email);

// User-facing error messages use localization
var userMessage = _localizer["ApplicationProcessingError"];
return BadRequest(new { error = userMessage });
```

## Testing Checklist

Before deploying bilingual content:
- [ ] Both English and French versions exist
- [ ] Content is equivalent (not just translated, but equivalent in meaning)
- [ ] Language toggle works and links to equivalent page
- [ ] Forms validate with bilingual error messages
- [ ] Date and number formats are culturally appropriate
- [ ] TERMIUM Plus® terminology is used
- [ ] Both versions tested with screen readers
- [ ] Both versions meet WCAG 2.1 AA
- [ ] Both versions have same functionality
- [ ] Email notifications are bilingual
- [ ] Confirmation messages are bilingual

## Common Mistakes to Avoid

❌ **Don't use machine translation without review** - Google Translate is not acceptable for GC content
❌ **Don't translate literally** - Adapt to cultural context
❌ **Don't forget form validation messages** - Must be bilingual
❌ **Don't link to English home from French pages** - Language toggle must preserve context
❌ **Don't use "e-mail"** - Use "courriel" in French
❌ **Don't use "login"** - Use "ouvrir une session" in French
❌ **Don't forget alt text** - Must be translated
❌ **Don't ignore date/number formats** - Use culturally appropriate formats
❌ **Don't forget metadata** - Page titles, descriptions must be translated

## Resources

- **TERMIUM Plus®**: https://www.btb.termiumplus.gc.ca/
- **Canada.ca Content Style Guide**: https://www.canada.ca/en/treasury-board-secretariat/services/government-communications/canada-content-style-guide.html
- **Official Languages Act**: https://laws-lois.justice.gc.ca/eng/acts/O-3.01/
- **Translation Bureau**: https://www.noslangues-ourlanguages.gc.ca/

## Professional Translation Services

For official GC content, use approved translation services:
- **Translation Bureau** (PSPC): translation.bureau-traduction@tpsgc-pwgsc.gc.ca
- Maintain translation memory for consistency
- Use qualified translators (not machine translation)

## Version Information

This guidance is current for:
- Official Languages Act (1985, c. 31)
- Policy on Official Languages (2012)
- Canada.ca Content Style Guide (2023)

Last updated: 2025-02-11
