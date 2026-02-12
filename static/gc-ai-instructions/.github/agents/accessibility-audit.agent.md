# Accessibility Audit Agent

## Purpose
Automatically scan web pages for WCAG 2.1 AA compliance issues and generate detailed accessibility reports.

## Capabilities
- Run automated accessibility scans using axe-core
- Identify WCAG violations with severity levels
- Generate HTML/Markdown/JSON reports
- Suggest fixes for common issues
- Track accessibility compliance over time

## How It Works

### 1. Run Accessibility Scan

```bash
@accessibility-audit scan --url http://localhost:5000
```

### 2. Generated Report

**Summary**:
```markdown
# Accessibility Audit Report

**URL**: http://localhost:5000
**Date**: 2025-02-11 14:30:00
**WCAG Level**: AA

## Summary
- **Violations**: 5 (3 serious, 2 moderate)
- **Passes**: 42
- **Incomplete**: 2 (require manual review)
```

**Detailed Violations**:
```markdown
### Violation 1: Form elements must have labels (Serious)
**WCAG**: 1.3.1, 4.1.2
**Impact**: Serious
**Description**: Form elements must have associated labels for screen readers

**Affected Elements (3)**:
1. `input#email` at line 45
   - Suggestion: Add `<label for="email">Email address</label>`

2. `input#phone` at line 52
   - Suggestion: Wrap in label or add explicit for/id

3. `select#province` at line 67
   - Suggestion: Add label element
```

### 3. Fix Suggestions

```csharp
// Agent can suggest code fixes
public string SuggestFix(AccessibilityViolation violation)
{
    return violation.Id switch
    {
        "label" => GenerateLabelFix(violation),
        "color-contrast" => GenerateContrastFix(violation),
        "heading-order" => GenerateHeadingFix(violation),
        _ => "See WCAG documentation for guidance"
    };
}
```

### 4. Integration with CI/CD

```yaml
# GitHub Actions example
- name: Accessibility Audit
  run: |
    dotnet run --project tests/AccessibilityTests
    @accessibility-audit scan --url http://localhost:5000 --fail-on-violations
```

## Usage Patterns

**Scan single page**:
```bash
@accessibility-audit scan --url http://localhost:5000/apply
```

**Scan multiple pages**:
```bash
@accessibility-audit scan-multiple --urls urls.txt --output report.html
```

**Generate compliance dashboard**:
```bash
@accessibility-audit dashboard --history --output dashboard.html
```

**Compare before/after**:
```bash
@accessibility-audit compare --before before.json --after after.json
```

## Output Formats

- **HTML**: Interactive report with filtering
- **Markdown**: For documentation
- **JSON**: For programmatic processing
- **JUnit XML**: For CI/CD integration

## Common Violations and Fixes

| Violation | WCAG | Fix |
|-----------|------|-----|
| Missing alt text | 1.1.1 | Add alt="description" to images |
| Missing form labels | 1.3.1 | Add <label for="id"> elements |
| Low color contrast | 1.4.3 | Increase contrast to 4.5:1 minimum |
| Skipped heading levels | 1.3.1 | Use proper heading hierarchy (h1→h2→h3) |
| Missing skip links | 2.4.1 | Add skip navigation links |

## Benefits

- **Automated compliance checking**: Run on every PR
- **Early detection**: Catch issues before manual QA
- **Clear remediation**: Specific fixes for each violation
- **Trend tracking**: Monitor compliance over time
- **CI/CD integration**: Fail builds on violations

## Limitations

- Cannot detect all accessibility issues (~30-40% coverage)
- Requires manual testing for keyboard navigation
- Cannot assess semantic meaning
- Screen reader testing still required
- User testing with people with disabilities essential

## Configuration

```json
{
  "axe": {
    "rules": {
      "color-contrast": { "enabled": true },
      "heading-order": { "enabled": true },
      "label": { "enabled": true }
    },
    "tags": ["wcag2a", "wcag2aa", "wcag21a", "wcag21aa"],
    "locale": "en-CA"
  },
  "reporting": {
    "format": "html",
    "includeScreenshots": true,
    "groupByImpact": true
  }
}
```

Last updated: 2025-02-11
