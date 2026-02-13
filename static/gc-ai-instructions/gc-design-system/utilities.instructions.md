# GC Design System - Utilities Instructions

## Overview

The GC Design System provides CSS utility classes for spacing, typography, and layout. These utilities create predictable patterns and eliminate arbitrary design decisions.

## Spacing Utilities

The GC Design System uses a consistent spacing scale based on rem units:

### Margin Utilities

```html
<!-- Margin all sides -->
<div class="m-0">No margin</div>
<div class="m-1">0.25rem margin</div>
<div class="m-2">0.5rem margin</div>
<div class="m-3">1rem margin</div>
<div class="m-4">2rem margin</div>
<div class="m-5">4rem margin</div>

<!-- Margin specific sides -->
<div class="mt-3">Margin top: 1rem</div>
<div class="mb-3">Margin bottom: 1rem</div>
<div class="ml-3">Margin left: 1rem</div>
<div class="mr-3">Margin right: 1rem</div>
<div class="mx-3">Margin horizontal: 1rem</div>
<div class="my-3">Margin vertical: 1rem</div>
```

### Padding Utilities

```html
<!-- Padding all sides -->
<div class="p-0">No padding</div>
<div class="p-1">0.25rem padding</div>
<div class="p-2">0.5rem padding</div>
<div class="p-3">1rem padding</div>
<div class="p-4">2rem padding</div>
<div class="p-5">4rem padding</div>

<!-- Padding specific sides -->
<div class="pt-3">Padding top: 1rem</div>
<div class="pb-3">Padding bottom: 1rem</div>
<div class="pl-3">Padding left: 1rem</div>
<div class="pr-3">Padding right: 1rem</div>
<div class="px-3">Padding horizontal: 1rem</div>
<div class="py-3">Padding vertical: 1rem</div>
```

## Typography Utilities

### Heading Styles

Use GC Design System heading classes for consistent typography:

```html
<h1 class="gc-h1">Page Title</h1>
<h2 class="gc-h2">Section Heading</h2>
<h3 class="gc-h3">Subsection Heading</h3>
<h4 class="gc-h4">Minor Heading</h4>
```

### Text Utilities

```html
<!-- Font weight -->
<p class="font-weight-normal">Normal weight</p>
<p class="font-weight-bold">Bold weight</p>

<!-- Text alignment -->
<p class="text-left">Left aligned</p>
<p class="text-center">Center aligned</p>
<p class="text-right">Right aligned</p>

<!-- Text transformation -->
<p class="text-uppercase">Uppercase text</p>
<p class="text-lowercase">Lowercase text</p>
<p class="text-capitalize">Capitalized text</p>
```

## Color Utilities

Use semantic color utilities for text and backgrounds:

```html
<!-- Text colors -->
<p class="text-primary">Primary text color</p>
<p class="text-secondary">Secondary text color</p>
<p class="text-success">Success text color</p>
<p class="text-danger">Danger/error text color</p>
<p class="text-warning">Warning text color</p>
<p class="text-info">Info text color</p>
<p class="text-muted">Muted/gray text</p>

<!-- Background colors -->
<div class="bg-primary">Primary background</div>
<div class="bg-light">Light background</div>
<div class="bg-dark">Dark background</div>
```

## Display Utilities

```html
<!-- Display types -->
<div class="d-none">Hidden element</div>
<div class="d-block">Block display</div>
<div class="d-inline">Inline display</div>
<div class="d-inline-block">Inline-block display</div>
<div class="d-flex">Flexbox container</div>

<!-- Responsive display -->
<div class="d-none d-md-block">Hidden on mobile, visible on tablet+</div>
<div class="d-block d-md-none">Visible on mobile, hidden on tablet+</div>
```

## Flexbox Utilities

```html
<!-- Flex container -->
<div class="d-flex">
    <div>Item 1</div>
    <div>Item 2</div>
</div>

<!-- Flex direction -->
<div class="d-flex flex-row">Horizontal layout</div>
<div class="d-flex flex-column">Vertical layout</div>

<!-- Justify content -->
<div class="d-flex justify-content-start">Start</div>
<div class="d-flex justify-content-center">Center</div>
<div class="d-flex justify-content-end">End</div>
<div class="d-flex justify-content-between">Space between</div>
<div class="d-flex justify-content-around">Space around</div>

<!-- Align items -->
<div class="d-flex align-items-start">Align start</div>
<div class="d-flex align-items-center">Align center</div>
<div class="d-flex align-items-end">Align end</div>
```

## Grid System

The GC Design System uses a 12-column responsive grid:

```html
<div class="container">
    <div class="row">
        <div class="col-12 col-md-8">
            <!-- Main content (8 columns on tablet+) -->
        </div>
        <div class="col-12 col-md-4">
            <!-- Sidebar (4 columns on tablet+) -->
        </div>
    </div>
</div>
```

### Common Layouts

**Two-column layout:**
```html
<div class="row">
    <div class="col-md-6">Column 1</div>
    <div class="col-md-6">Column 2</div>
</div>
```

**Three-column layout:**
```html
<div class="row">
    <div class="col-md-4">Column 1</div>
    <div class="col-md-4">Column 2</div>
    <div class="col-md-4">Column 3</div>
</div>
```

**Service page layout (8-column main + 4-column sidebar):**
```html
<div class="row">
    <div class="col-md-8">
        <!-- Main service content -->
    </div>
    <div class="col-md-4">
        <!-- Related links, contact info -->
    </div>
</div>
```

## Border Utilities

```html
<!-- Add borders -->
<div class="border">All borders</div>
<div class="border-top">Top border only</div>
<div class="border-bottom">Bottom border only</div>

<!-- Remove borders -->
<div class="border-0">No border</div>

<!-- Border radius -->
<div class="rounded">Rounded corners</div>
<div class="rounded-circle">Circular</div>
```

## Width and Height Utilities

```html
<!-- Width -->
<div class="w-25">25% width</div>
<div class="w-50">50% width</div>
<div class="w-75">75% width</div>
<div class="w-100">100% width</div>

<!-- Max width -->
<div class="mw-100">Max width 100%</div>

<!-- Height -->
<div class="h-25">25% height</div>
<div class="h-50">50% height</div>
<div class="h-75">75% height</div>
<div class="h-100">100% height</div>
```

## Accessibility Considerations

- Use semantic HTML elements first, utilities second
- Don't rely solely on color to convey meaning
- Ensure sufficient color contrast (WCAG 2.1 AA minimum 4.5:1)
- Use `.sr-only` for screen-reader-only content:

```html
<span class="sr-only">Screen reader only text</span>
```

## Best Practices

1. **Use design tokens instead of arbitrary values** - `mt-4` (2rem) instead of custom margins
2. **Mobile-first responsive design** - Start with mobile layout, add responsive classes for larger screens
3. **Consistent spacing** - Use the spacing scale (0-5) consistently throughout the application
4. **Semantic class names** - Use utility classes for layout/spacing, semantic classes for components
5. **Limit utility class chaining** - If you're using 10+ utility classes, consider creating a component class

## See Also

- [GC Design System Documentation](https://design-system.canada.ca/)
- [page-templates.instructions.md](/gc-ai-instructions/gc-design-system/page-templates.instructions.md)
- [WET-BOEW Instructions](/gc-ai-instructions/.github/instructions/wet-boew.instructions.md)
