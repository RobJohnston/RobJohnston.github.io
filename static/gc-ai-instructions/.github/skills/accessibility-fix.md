# Accessibility Quick Fix Skill

## Purpose
Quick fixes for the top 10 most common WCAG 2.1 AA violations in GC applications.

## Quick Fixes

### 1. Missing Alt Text (WCAG 1.1.1)

**Problem**: Images without alt attributes

**Fix**:
```html
<!-- ❌ Before -->
<img src="/logo.png">

<!-- ✅ After -->
<img src="/logo.png" alt="Government of Canada logo">

<!-- Decorative images -->
<img src="/decoration.png" alt="">
```

### 2. Missing Form Labels (WCAG 1.3.1)

**Problem**: Form inputs without labels

**Fix**:
```html
<!-- ❌ Before -->
<input type="email" name="email" placeholder="Email">

<!-- ✅ After -->
<label for="email">Email address</label>
<input type="email" id="email" name="email">
```

### 3. Low Color Contrast (WCAG 1.4.3)

**Problem**: Text doesn't meet 4.5:1 contrast ratio

**Fix**:
```css
/* ❌ Before - 3.2:1 contrast */
color: #777777;
background: #ffffff;

/* ✅ After - 7.0:1 contrast */
color: #333333;
background: #ffffff;
```

**Tool**: Use browser DevTools contrast checker or https://webaim.org/resources/contrastchecker/

### 4. Skipped Heading Levels (WCAG 1.3.1)

**Problem**: Heading hierarchy jumps levels

**Fix**:
```html
<!-- ❌ Before -->
<h1>Page Title</h1>
<h3>Section</h3> <!-- Skipped h2 -->

<!-- ✅ After -->
<h1>Page Title</h1>
<h2>Section</h2>
<h3>Subsection</h3>
```

### 5. Missing Skip Links (WCAG 2.4.1)

**Problem**: No way to skip to main content

**Fix**:
```html
<body>
  <!-- Add skip links FIRST -->
  <nav>
    <ul id="wb-tphp">
      <li class="wb-slc">
        <a class="wb-sl" href="#wb-cont">Skip to main content</a>
      </li>
    </ul>
  </nav>

  <header>...</header>

  <main id="wb-cont">
    <h1>Page Title</h1>
  </main>
</body>
```

### 6. Non-Descriptive Link Text (WCAG 2.4.4)

**Problem**: Generic "click here" links

**Fix**:
```html
<!-- ❌ Before -->
To learn more, <a href="/info">click here</a>.

<!-- ✅ After -->
<a href="/info">Learn more about Employment Insurance benefits</a>

<!-- Or with aria-label -->
<a href="/form.pdf" aria-label="Download Employment Insurance application form (PDF, 2 MB)">
  Download form
</a>
```

### 7. Missing Table Headers (WCAG 1.3.1)

**Problem**: Tables without proper headers

**Fix**:
```html
<!-- ❌ Before -->
<table>
  <tr>
    <td>Service</td>
    <td>Time</td>
  </tr>
</table>

<!-- ✅ After -->
<table class="table wb-tables">
  <caption>Processing times</caption>
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

### 8. Removed Focus Indicators (WCAG 2.4.7)

**Problem**: :focus outline removed

**Fix**:
```css
/* ❌ Before */
*:focus {
  outline: none;
}

/* ✅ After */
a:focus,
button:focus,
input:focus {
  outline: 3px solid #0535d2;
  outline-offset: 2px;
}
```

### 9. Non-Keyboard Accessible Elements (WCAG 2.1.1)

**Problem**: Click handlers on divs

**Fix**:
```html
<!-- ❌ Before -->
<div onclick="doSomething()">Click me</div>

<!-- ✅ After - Use button -->
<button type="button" onclick="doSomething()">Click me</button>

<!-- ⚠️ If must use div -->
<div role="button" tabindex="0" 
     onclick="doSomething()" 
     onkeydown="if(event.key==='Enter' || event.key===' ') doSomething()">
  Click me
</div>
```

### 10. Missing Language Attribute (WCAG 3.1.1)

**Problem**: HTML lang attribute missing

**Fix**:
```html
<!-- ❌ Before -->
<!DOCTYPE html>
<html>

<!-- ✅ After -->
<!DOCTYPE html>
<html lang="en">

<!-- French page -->
<!DOCTYPE html>
<html lang="fr">
```

## Testing Checklist

After applying fixes:
- [ ] Run axe DevTools scan (0 violations)
- [ ] Test keyboard navigation (Tab, Enter, Space, Esc)
- [ ] Test with screen reader (NVDA or JAWS)
- [ ] Check color contrast (DevTools)
- [ ] Validate HTML (https://validator.w3.org/)

## Quick Commands

```bash
# Run automated accessibility test
npm run test:a11y

# Or with axe-core CLI
npx @axe-core/cli http://localhost:5000

# Check specific page
npx @axe-core/cli http://localhost:5000/apply --tags wcag2aa
```

Last updated: 2025-02-11
