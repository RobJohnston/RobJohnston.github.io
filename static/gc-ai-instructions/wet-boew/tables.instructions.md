# WET-BOEW Tables Instructions

## Overview

This file provides guidance on creating accessible data tables using WET-BOEW for Government of Canada applications.

## Basic Table Structure

All tables must include proper semantic markup for WCAG 2.1 AA compliance:

```html
<table class="table table-striped">
    <caption>
        <span lang="en">Application Status</span>
        <span lang="fr">État de la demande</span>
    </caption>
    <thead>
        <tr>
            <th scope="col">
                <span lang="en">Application ID</span>
                <span lang="fr">Numéro de demande</span>
            </th>
            <th scope="col">
                <span lang="en">Status</span>
                <span lang="fr">État</span>
            </th>
            <th scope="col">
                <span lang="en">Date Submitted</span>
                <span lang="fr">Date de soumission</span>
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>APP-2024-001</td>
            <td>
                <span lang="en">Approved</span>
                <span lang="fr">Approuvé</span>
            </td>
            <td>2024-01-15</td>
        </tr>
    </tbody>
</table>
```

## Table Styles

WET-BOEW provides several table styling options:

### Striped Tables
```html
<table class="table table-striped">
    <!-- table content -->
</table>
```

### Bordered Tables
```html
<table class="table table-bordered">
    <!-- table content -->
</table>
```

### Condensed Tables
```html
<table class="table table-condensed">
    <!-- table content -->
</table>
```

### Responsive Tables

For tables that may overflow on small screens:

```html
<div class="table-responsive">
    <table class="table">
        <!-- table content -->
    </table>
</div>
```

## Accessibility Requirements

### Always Include:
- `<caption>` element describing the table content (bilingual)
- `<thead>`, `<tbody>`, and optionally `<tfoot>` for structure
- `scope="col"` on header cells in `<thead>` for column headers
- `scope="row"` on header cells in first column for row headers (if applicable)

### For Complex Tables:
- Use `id` and `headers` attributes to associate cells with headers
- Consider breaking into multiple simpler tables if possible

## Sortable Tables

WET-BOEW provides sortable table functionality:

```html
<table class="wb-tables table table-striped" data-wb-tables='{"paging": false}'>
    <thead>
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Date</th>
        </tr>
    </thead>
    <tbody>
        <!-- data rows -->
    </tbody>
</table>
```

## Data Tables with Pagination

For large datasets:

```html
<table class="wb-tables table table-striped" data-wb-tables='{"paging": true, "pageLength": 25}'>
    <!-- table content -->
</table>
```

## Bilingual Headers

All column headers must be bilingual:

```html
<th scope="col">
    <span lang="en">Full Name</span> /
    <span lang="fr">Nom complet</span>
</th>
```

Or using separate spans:

```html
<th scope="col">
    <span lang="en">Status</span>
    <span lang="fr">État</span>
</th>
```

## Empty State

When tables have no data:

```html
<table class="table">
    <caption>Applications</caption>
    <thead>
        <!-- headers -->
    </thead>
    <tbody>
        <tr>
            <td colspan="3" class="text-center">
                <span lang="en">No applications found.</span>
                <span lang="fr">Aucune demande trouvée.</span>
            </td>
        </tr>
    </tbody>
</table>
```

## See Also

- [WET-BOEW Tables](https://wet-boew.github.io/wet-boew/demos/tables/tables-en.html)
- [WET-BOEW Data Tables](https://wet-boew.github.io/wet-boew/demos/data-tables/data-tables-en.html)
- [accessibility.instructions.md](/gc-ai-instructions/accessibility/accessibility.instructions.md)
