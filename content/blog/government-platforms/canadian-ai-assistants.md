+++
title = "Why Canada's Digital Government Infrastructure Is Perfect for AI-Assisted Development"
date = "2026-02-11"
draft = true
description = "Canada's WET-BOEW toolkit, GC Design System, Content Style Guide, and Digital Standards create the perfect ecosystem for AI-assisted government software development"
category = "government"
tags = ["government", "ai", "accessibility", "standards", "web-development", "content-design"]
image = "/images/canadian-government-platforms.jpg"
mermaid = true
+++

Picture this: You just won a contract to build a web application for the Government of Canada. Exciting, right? Then reality hits.

You need WCAG 2.1 AA accessibility compliance—not as an afterthought, but baked into every component. The entire application must work flawlessly in both English and French. You'll need to navigate the Standard on Web Accessibility, understand Protected B data classification, and ensure your code can pass security review and Threat and Risk Assessment (TRA). Oh, and your fixed-price contract means every hour spent figuring out "how things are done around here" cuts directly into your margins.

You open the Web Experience Toolkit (WET-BOEW) documentation, the GC Design System guide, the Canada.ca Content Style Guide, and the Digital Standards page. It's overwhelming. Where do you even start?

Here's what most contractors don't realize: **Canada has already built the infrastructure that makes AI coding assistants incredibly effective.** The same standards and toolkits that seem daunting at first are exactly what AI assistants need to help you build compliant government applications faster than you thought possible.

## What Makes AI Coding Assistants Effective?

Before we dive into Canada's digital ecosystem, let's talk about what AI assistants actually need to be useful—especially in constrained, regulated environments like government software development.

**AI coding assistants work best when they have structured context, not just raw code.** They need:

1. **Bounded vocabulary**: A limited, well-defined set of components and patterns rather than infinite possibilities
2. **Predictable patterns**: Clear conventions for how things should be done
3. **Explicit guardrails**: Institutional knowledge encoded as machine-readable guidance

Think about it: When you ask an AI assistant to "build a contact form," it could generate thousands of different implementations. But when you ask it to "build a WET-BOEW compliant contact form for a Government of Canada website," suddenly there's structure. There's a right way to do it. The assistant can reference actual components, follow established accessibility patterns, and ensure bilingual support—because those constraints are part of the platform.

```mermaid
sequenceDiagram
    participant Dev as Developer
    participant AI as AI Assistant
    participant Inst as Instruction Files<br/>(WET-BOEW, GC Design,<br/>Content Style, Standards)
    participant Code as Generated Code

    Dev->>AI: "Create a contact form"
    AI->>Inst: Load GC standards & patterns
    Inst-->>AI: WET-BOEW components<br/>WCAG 2.1 AA patterns<br/>Bilingual requirements<br/>Protected B security<br/>API standards
    AI->>Code: Generate compliant code
    Code-->>Dev: ✅ Accessible<br/>✅ Bilingual<br/>✅ Secure<br/>✅ Standards-compliant

    Note over Dev,Code: Result: Compliant code from day one,<br/>not after multiple review cycles
```

This can be understood as a three-layer abstraction:

1. **Cloud infrastructure layer**: The foundational compute, storage, and networking (in Canada, [Shared Services Canada brokers access](https://www.canada.ca/en/shared-services/corporate/publications/2024-25/evaluation-ssc-cloud-services.html) to commercial cloud providers like AWS, Azure, and Google Cloud)
2. **Deployment and operations layer**: How applications get deployed, monitored, and maintained
3. **Platform knowledge layer**: The institutional knowledge about how to build things the "right way"—encoded as AI instruction files

```mermaid
graph TB
    subgraph "Three-Layer Architecture"
        PK[Platform Knowledge Layer<br/>AI Instruction Files<br/>WET-BOEW, GC Design System,<br/>Content Style Guide, Standards]
        DO[Deployment & Operations Layer<br/>CI/CD, Monitoring, Maintenance]
        CI[Cloud Infrastructure Layer<br/>SSC-Brokered Cloud<br/>AWS, Azure, Google Cloud]
    end

    PK --> DO
    DO --> CI

    DEV[Developer + AI Assistant] --> PK

    style PK fill:#e1f5ff
    style DO fill:#fff4e1
    style CI fill:#ffe1e1
    style DEV fill:#e1ffe1
```

That third layer is the breakthrough. By encoding platform knowledge as machine-readable instruction files, AI assistants can automatically apply institutional standards and patterns.

Here's the insight that changed my perspective: **Canada has been building this ecosystem for over a decade, since WET-BOEW launched in 2010.** We just haven't optimized it for AI consumption yet.

## Canada's Digital Government Ecosystem

The Government of Canada's digital infrastructure consists of two complementary layers: **Standards & Frameworks** that define how to build compliant services, and **Platform Providers** that deliver reusable tools and services.

```mermaid
graph TB
    subgraph "Standards & Frameworks<br/>(What you must follow)"
        WET[WET-BOEW<br/>Accessible Components]
        CSG[Content Style Guide<br/>Writing Standards]
        DS[Digital Standards<br/>Design Principles]
        API[Standards on APIs<br/>Technical Specs]
        GG[GC Cloud Guardrails<br/>Security Baselines]
        CCCS[CCCS Frameworks<br/>ITSG-33, CMVP]
    end

    subgraph "Platform Providers<br/>(Organizations providing tools & services)"
        CDS[Canadian Digital Service<br/>GC Design System, GC Notify,<br/>GC Forms, GC Sign-in,<br/>GC Issue & Verify, Toolkit]
        SSC[Shared Services Canada<br/>Cloud Brokering<br/>AWS, Azure, GCP]
        TB[Translation Bureau<br/>TERMIUM Plus<br/>Translation Services]
    end

    WET --> AI[AI-Assisted Development<br/>Fast, Compliant, Accessible]
    CSG --> AI
    DS --> AI
    API --> AI
    GG --> AI
    CCCS --> AI
    CDS --> AI
    SSC --> AI
    TB --> AI

    style AI fill:#90EE90
    style WET fill:#E8F4F8
    style CSG fill:#E8F4F8
    style DS fill:#E8F4F8
    style API fill:#E8F4F8
    style GG fill:#E8F4F8
    style CCCS fill:#E8F4F8
    style CDS fill:#FFE8E8
    style SSC fill:#FFE8E8
    style TB fill:#FFE8E8
```

### Standards & Frameworks: What You Must Follow

These are the official standards, toolkits, and security frameworks that define how Government of Canada digital services should be built. They provide the bounded vocabulary, predictable patterns, and explicit guardrails that make AI-assisted development effective.

#### 1. Web Experience Toolkit (WET-BOEW)

WET-BOEW is an open-source code library for building accessible, usable, interoperable government websites. It's been the standard for federal web presence since 2010, which means there's over 15 years of institutional knowledge embedded in its patterns.

What makes WET-BOEW special:
- **WCAG 2.1 AA compliance is built-in**: You don't guess at accessibility requirements; components are already compliant
- **Reusable components with clear APIs**: Date pickers, form validation, multimedia players—all documented and tested
- **WAI-ARIA support**: Screen reader compatibility isn't an afterthought
- **Open source on GitHub**: The code is public, which means AI assistants can learn from actual implementations

#### 2. Canada.ca Content Style Guide

The [Canada.ca Content Style Guide](https://design.canada.ca/style-guide/) provides the writing and content standards for all Government of Canada web content. Recently updated to align with ISO plain language standards, it ensures consistency across the entire Canada.ca ecosystem.

What makes the Content Style Guide essential:
- **Plain language principles**: Clear, simple writing that citizens can understand
- **Structured content patterns**: Standard formats for headings, lists, tables, and links
- **Tone and voice guidance**: How to write in a consistent, citizen-centered way
- **Bilingual writing conventions**: Patterns for presenting both official languages
- **SEO and findability**: Content optimization for search engines
- **Formatting standards**: Typography, capitalization, punctuation rules

The Content Style Guide is particularly powerful for AI assistance because it provides **bounded vocabulary for content creation**, not just code. An AI assistant with Content Style Guide context can help draft Canada.ca-compliant web content that's automatically plain language, properly formatted, and bilingual-ready.

#### 3. Digital Standards

The [10 Digital Standards](https://www.canada.ca/en/government/system/digital-government/government-canada-digital-standards.html) are the philosophical foundation—the principles that guide how digital services should be built. Key standards include:

- **Design with Users** (#1): Put user needs first
- **Work in the Open by Default** (#3): Share code, plans, and research
- **Build in Accessibility from the Start** (#6): Not as a retrofit
- **Design Ethical Services** (#9): Consider the broader impact
- **Collaborate Widely** (#10): Work across organizational boundaries

#### 4. Standards on APIs

The [Standards on APIs](https://www.canada.ca/en/government/system/digital-government/digital-government-innovations/government-canada-standards-apis.html) are the technical requirements for building and consuming Government of Canada APIs. While the 10 Digital Standards provide philosophical guidance, the Standards on APIs provide concrete technical specifications.

**Key requirements**:

- **Architecture**: RESTful model by default, JSON message format (UTF-8 encoding), resource-oriented URLs (nouns, not verbs)
- **Security**: TLS 1.2 or higher, JWT (JSON Web Token) for authentication, API keys in headers (never in URLs)
- **Data Standards**: ISO 8601 datetime format in UTC (yyyy-mm-ddThh:mm:ssZ), JSON responses must be objects (not arrays), consistent casing
- **Versioning**: Format `v<Major>.<Minor>.<Patch>`, URL reflects major version only (e.g., `/v3/`), support at least one previous major version
- **Error Handling**: HTTP status codes for REST, abstract internal technical details (no stack traces in responses), consistent error response format
- **Performance**: Pagination required for large result sets, restrict wildcard queries, load testing and published benchmarks
- **Documentation**: OpenAPI/Swagger specifications for REST APIs, published to API Store for discovery, include test data and examples

**Why this matters for AI assistants**: When you're building a Government of Canada application with backend APIs (ASP.NET Core, Node.js, etc.), the Standards on APIs provide the bounded vocabulary for API design. An AI assistant with Standards on APIs context knows that API endpoints should use `/api/v1/applications` (not `/api/getApplications`), that datetime fields must be ISO 8601 in UTC, and that error responses should abstract technical details.

This is particularly important because many GC web applications have both a frontend (using WET-BOEW) and a backend API (providing data to the frontend or external integrations). The Standards on APIs ensure consistency across all government backend services—making it easier to integrate services, consume open data APIs, and build coherent digital ecosystems.

#### 5. GC Cloud Guardrails

[GC Cloud Guardrails](https://canada-ca.github.io/cloud-guardrails/) are mandatory baseline security controls for cloud deployments. Departments must implement these guardrails within 30 business days of getting cloud access. These prescriptive security requirements (available as [open-source on GitHub](https://github.com/canada-ca/cloud-guardrails)) cover:

- Identity and access management
- Network security and segmentation
- Data protection and encryption
- Logging and monitoring
- Incident response procedures

**Why this matters for AI assistants**: These are explicit, documented security controls—perfect candidates for encoding as AI instruction files. Instead of guessing at cloud security requirements, AI assistants can reference the specific guardrails (e.g., "Enable MFA for all accounts" or "Encrypt data at rest using approved algorithms").

#### 6. CCCS Security Frameworks

The [Canadian Centre for Cyber Security (CCCS)](https://www.cyber.gc.ca/en/government-institutions) provides the security frameworks that underpin government IT security:

- **[ITSG-33](https://www.cyber.gc.ca/en/guidance/it-security-risk-management-lifecycle-approach-itsg-33)**: IT Security Risk Management framework with detailed security controls
- **[Cloud Security Profile](https://www.cyber.gc.ca/en/guidance/cloud-security-guidance)**: Guidance for assessing cloud service providers
- **[Cryptographic Module Validation Program (CMVP)](https://www.cyber.gc.ca/en/cryptographic-module-validation-program)**: Certification for encryption products

**Why this matters for AI assistants**: CCCS guidance provides authoritative security patterns. Instead of developers guessing at encryption requirements, instruction files can encode CCCS-approved approaches (e.g., "Use CMVP-validated encryption for Protected B data" or "Follow ITSG-33 AC-2 access control patterns").

### Platform Providers: Organizations Delivering Tools & Services

While standards define *what* to build, platform providers deliver reusable tools and services that accelerate development. Three organizations play central roles in Canada's digital ecosystem.

#### 1. Canadian Digital Service (CDS)

The [Canadian Digital Service](https://digital.canada.ca/) is an organization within the Treasury Board of Canada Secretariat that builds and operates modern digital tools for government. CDS plays a central role in Canada's digital ecosystem, providing reusable services that help departments deliver better citizen experiences.

**GC Design System**: The [GC Design System](https://design-system.alpha.canada.ca/) is CDS's design language for government services. It provides:
- **Pre-built page templates**: Landing pages, service initiation flows, confirmation pages—all following Canada.ca patterns
- **Design tokens**: Standardized colors, spacing, typography (not arbitrary values)
- **Component library**: Breadcrumbs, buttons, cards, forms, navigation—all consistent with the broader Canada.ca experience
- **CSS utility classes**: A consistent styling vocabulary that speeds up development
- **Bilingual support built into the core**: Not bolted on, but fundamental to every component

**Why this matters for AI assistants**: The GC Design System provides predictable patterns—established conventions that eliminate arbitrary design decisions. AI assistants can reference design tokens, page templates, and utility classes to generate consistent, Canada.ca-compliant interfaces automatically.

**GC Notify**: A [free notification service](https://notification.canada.ca/) for sending emails and SMS. GC Notify provides departments with a standardized API for sending up to 20 million emails and 100,000 texts per year, with bilingual defaults and Federal Identity Program compliance built-in.

**GC Forms**: A [platform for building accessible, bilingual forms](https://articles.alpha.canada.ca/forms-formulaires/) without custom development. It standardizes the form-building process across departments.

**GC Sign-in**: The next-generation [authentication service](https://digital.canada.ca/2025/02/12/streamlining-government-services-introducing-gc-sign-in/) (piloting in 2025) that modernizes citizen login with passwordless authentication, passkeys, and self-serve integration tools—addressing the current complexity of 270+ online services with 60+ different sign-in methods.

**GC Issue and Verify**: An upcoming digital credentials service that will enable departments to issue and verify digital credentials securely—supporting use cases like digital licenses, certificates, and other government-issued credentials that citizens can store and present digitally.

**Service Digital Toolkit**: A [collection of practical resources](https://digital.canada.ca/service-digital-toolkit/), templates, and guidance for designing and delivering digital services. This toolkit helps teams translate the high-level Digital Standards into concrete practices and workflows.

**Why CDS matters for AI-assisted development**: CDS provides a bounded vocabulary of reusable services. Instead of building custom notification systems, authentication flows, or form builders, developers integrate standardized services with clear APIs—exactly the kind of structure AI assistants excel at generating integration code for.

#### 2. Shared Services Canada (SSC)

[Shared Services Canada's Cloud Brokering Service](https://www.canada.ca/en/shared-services/corporate/publications/2024-25/evaluation-ssc-cloud-services.html) (launched 2017) acts as an intermediary between federal departments and commercial cloud providers. Rather than running a single government platform, SSC maintains framework agreements with 8 cloud providers (AWS, Azure, Google Cloud, etc.) and provides standardized procurement, security profiles, and deployment patterns.

**Why this matters for AI-assisted development**: SSC-brokered cloud providers create opportunities for standardized instruction files. Instead of each department creating their own cloud deployment patterns, SSC-published instruction files could provide canonical examples for each approved provider (e.g., "Deploy ASP.NET Core to AWS with Protected B controls following SSC framework agreement"). [AWS is approved for Protected B data](https://aws.amazon.com/blogs/publicsector/aws-now-able-to-host-protected-b-data-for-the-government-of-canada/), and [Microsoft 365 E5 is being standardized](https://www.canada.ca/en/shared-services/corporate/about-us/publications/2025-26/2025-26-departmental-plan.html) across the Government of Canada—both enabling consistent deployment patterns.

#### 3. Translation Bureau

The [Translation Bureau](https://www.canada.ca/en/translation-bureau.html) provides linguistic services across 101 languages and operates [TERMIUM Plus®](https://www.btb.termiumplus.gc.ca/tpv2alpha/alpha-eng.html?lang=eng), one of the world's largest terminology databases with millions of terms in English, French, Spanish, and Portuguese.

**Why this matters for AI assistants**: TERMIUM Plus provides **official government terminology** as a bounded vocabulary for bilingual translation. Instead of AI assistants guessing at French translations or using inconsistent terminology, they can reference the authoritative database that standardizes terms across the federal public service. The Translation Bureau also developed [GCtranslate](https://www.canada.ca/en/public-services-procurement/news/2025/09/gctranslate-using-artificial-intelligence-to-build-a-more-agile-modern-and-bilingual-public-service.html), an AI-powered translation service for government content up to Protected B—demonstrating the government is already successfully using AI for bilingual content creation.

### The Ecosystem Working Together

This two-layer architecture—**Standards & Frameworks** defining requirements plus **Platform Providers** delivering reusable tools—creates a comprehensive ecosystem. These weren't built with AI assistants in mind. But they provide exactly the structure AI assistants need: bounded vocabulary in each domain, predictable patterns to follow, and explicit guardrails for compliance.

## How WET-BOEW Provides "Bounded Vocabulary"

Let's talk about the first requirement for effective AI assistance: **bounded vocabulary**.

In software development, "unbounded vocabulary" is when there are infinite ways to solve a problem. Need an accessible date picker? You could build it dozens of different ways—each with its own accessibility considerations, browser compatibility quirks, and keyboard navigation patterns. An AI assistant asked to "create an accessible date picker" has to make countless assumptions.

**Bounded vocabulary** means there's a limited, well-defined set of solutions. WET-BOEW provides this through its component library.

### The WET-BOEW Date Picker Example

Here's what happens when you ask an AI assistant to "create an accessible bilingual date picker for a Government of Canada form" without WET-BOEW context:

The assistant generates generic HTML5 `<input type="date">`, maybe adds some ARIA attributes if it's sophisticated, but:
- It might miss the requirement for bilingual date format labels
- It probably won't include proper error message patterns for both official languages
- It may not account for keyboard navigation patterns that match the rest of Canada.ca
- It likely won't include the correct WET-BOEW classes that tie into the centralized accessibility framework

Now, give that same AI assistant WET-BOEW context—tell it you're building on WET-BOEW 4.0.x, you need the standard Canada.ca theme, and you're following GC form patterns. Watch what happens:

```html
<div class="form-group">
    <label for="date-birth">
        <span class="field-name">Date of birth</span>
        <span lang="fr">Date de naissance</span>
        <strong class="required">(required)</strong>
    </label>
    <input
        class="form-control"
        id="date-birth"
        name="date-birth"
        type="date"
        data-rule-required="true"
        data-msg-required="This field is required."
        data-msg-required-fr="Ce champ est obligatoire."
    />
</div>
```

The AI assistant now:
- Uses the correct `form-group` wrapper (WET-BOEW convention)
- Includes bilingual labels using `lang` attributes (Official Languages Act compliance)
- Applies proper `required` indicator styling (Canada.ca pattern)
- Adds WET-BOEW's form validation data attributes
- Provides bilingual error messages following the established convention

This isn't magic—it's structure. The bounded vocabulary of WET-BOEW components eliminates ambiguity.

### Accessibility Compliance Built-In

Here's the real power: When you use WET-BOEW components, WCAG 2.1 AA compliance isn't something you achieve—it's something you inherit.

Consider a common scenario: You need a modal dialog for terms and conditions. Building an accessible modal from scratch requires understanding:
- Focus management (trapping focus within the modal)
- Keyboard navigation (Escape to close, Tab cycling)
- ARIA attributes (`role="dialog"`, `aria-modal="true"`, `aria-labelledby`)
- Screen reader announcements
- Focus return to trigger element on close

WET-BOEW's [lightbox component](https://wet-boew.github.io/wet-boew/demos/lightbox/lightbox-en.html) handles all of this. An AI assistant with WET-BOEW context can generate:

```html
<a href="#terms-modal"
   class="wb-lbx"
   title="Terms and Conditions">
    View Terms
</a>

<section id="terms-modal" class="mfp-hide modal-dialog modal-content overlay-def">
    <header class="modal-header">
        <h2 class="modal-title">Terms and Conditions</h2>
    </header>
    <div class="modal-body">
        <!-- Terms content -->
    </div>
</section>
```

All the accessibility complexity—focus trapping, ARIA, keyboard navigation—is handled by WET-BOEW's JavaScript. The AI assistant just needs to know the component pattern.

This is bounded vocabulary in action: Instead of "figure out how to build an accessible modal," it's "use the WET-BOEW lightbox component."

## How CDS's GC Design System Provides "Predictable Patterns"

The second requirement for effective AI assistance is **predictable patterns**—established conventions that eliminate arbitrary decisions. The Canadian Digital Service's GC Design System delivers this through design tokens, templates, and utility classes.

Here's a question that has no right answer without context: "How much spacing should there be between the page title and the first content section?"

Without standards, a developer (or AI assistant) has to guess. 20 pixels? 32 pixels? 2 rem? The choice is arbitrary, which means it's inconsistent across the application and across government services.

### Design Tokens: The Language of Consistency

The GC Design System solves this with **design tokens**—standardized values for spacing, colors, typography, and other design properties.

For spacing, there's a defined scale:
- `spacer-0-5`: 0.25 rem (4px)
- `spacer-1`: 0.5 rem (8px)
- `spacer-2`: 1 rem (16px)
- `spacer-3`: 1.5 rem (24px)
- `spacer-4`: 2 rem (32px)
- `spacer-5`: 3 rem (48px)

Now the question has an answer: "Use `spacer-4` (2 rem) between the page title and content section—that's the Canada.ca standard for major content breaks."

An AI assistant with GC Design System context can generate:

```html
<h1 class="gc-h1">Apply for a Social Insurance Number</h1>
<div class="mt-4">  <!-- margin-top: 2rem via spacer-4 -->
    <p>A Social Insurance Number (SIN) is a nine-digit number...</p>
</div>
```

Instead of guessing at margins, the assistant uses the established utility class (`mt-4` for margin-top using the spacer-4 value). Every service built this way looks visually consistent with Canada.ca.

### Pre-built Page Templates

The real power of predictable patterns shows up in the GC Design System's page templates. These aren't just UI mockups—they're complete structural patterns for common government service pages.

**Service Initiation Template**: Starting a new digital service? There's a template for that:

```html
<div class="container">
    <div class="row">
        <div class="col-md-8">
            <h1>Apply for Employment Insurance (EI) benefits</h1>

            <!-- Service description -->
            <section class="gc-srvinfo">
                <h2>What this service offers</h2>
                <p>Employment Insurance (EI) provides temporary financial assistance...</p>
            </section>

            <!-- Eligibility checker -->
            <section class="well">
                <h2>Check your eligibility</h2>
                <p>Before you start, make sure you meet these requirements:</p>
                <ul class="list-unstyled">
                    <li><span class="far fa-check-circle text-success"></span> Lost your job through no fault of your own</li>
                    <li><span class="far fa-check-circle text-success"></span> Haven't worked for at least 7 consecutive days</li>
                    <li><span class="far fa-check-circle text-success"></span> Worked the required hours in the last 52 weeks</li>
                </ul>
            </section>

            <!-- Before you start -->
            <section>
                <h2>What you need before you start</h2>
                <ul>
                    <li>Your Social Insurance Number (SIN)</li>
                    <li>Your banking information for direct deposit</li>
                    <li>Details about your last employer</li>
                </ul>
            </section>

            <!-- Call to action -->
            <section>
                <a href="/apply-start" class="btn btn-primary btn-lg">Start your application</a>
                <p class="mrgn-tp-lg">
                    <small>Estimated time to complete: 45 minutes</small>
                </p>
            </section>
        </div>

        <div class="col-md-4">
            <!-- Related links sidebar -->
            <section class="lnkbx">
                <h2>Related links</h2>
                <ul>
                    <li><a href="/ei-calculator">EI benefit calculator</a></li>
                    <li><a href="/ei-reporting">Report your income</a></li>
                    <li><a href="/ei-contact">Contact us about EI</a></li>
                </ul>
            </section>
        </div>
    </div>
</div>
```

This template provides:
1. **Standard layout**: 8-column main content, 4-column sidebar (responsive breakpoints included)
2. **Information hierarchy**: Service description → Eligibility → Requirements → Action
3. **Visual patterns**: Well component for eligibility, icons for checklist items, prominent CTA
4. **Accessibility built-in**: Proper heading structure, semantic HTML, ARIA landmarks via roles
5. **Bilingual structure ready**: Content areas designed for both official languages

An AI assistant with this template context doesn't need to decide "how should a service start page be structured?"—it already knows. The pattern is predictable.

### CSS Utility Classes

The GC Design System includes utility classes that create a consistent styling vocabulary:

```html
<!-- Margin utilities -->
<div class="mt-3">  <!-- margin-top: 1.5rem -->
<div class="mb-4">  <!-- margin-bottom: 2rem -->
<div class="mx-2">  <!-- margin left and right: 1rem -->

<!-- Padding utilities -->
<div class="p-3">   <!-- padding all sides: 1.5rem -->
<div class="py-2">  <!-- padding top and bottom: 1rem -->

<!-- Color utilities -->
<p class="text-danger">Error message</p>
<div class="bg-light">Light background section</div>

<!-- Display utilities -->
<div class="d-flex justify-content-between align-items-center">
    <span>Label</span>
    <button>Action</button>
</div>
```

This is like giving the AI assistant a consistent language to speak. Instead of generating arbitrary inline styles (`style="margin-top: 25px"`), it uses standardized utilities that match the rest of Canada.ca.

## How the Canada.ca Content Style Guide Provides "Bounded Vocabulary for Content"

We've talked about bounded vocabulary for code (WET-BOEW components) and design (GC Design System tokens). But what about the actual words on the page?

This is where the **Canada.ca Content Style Guide** becomes powerful for AI assistance. It provides structured patterns for writing web content—turning the infinite possibilities of how to phrase something into a well-defined set of conventions.

### Plain Language Patterns

The Content Style Guide mandates plain language—not as a suggestion, but as a standard. An AI assistant with this context won't generate bureaucratic jargon or complex sentence structures.

**Without Content Style Guide context**, ask an AI to write an eligibility statement:
> "Individuals who have attained the age of majority in their province or territory of residence and who are currently experiencing involuntary cessation of employment may be eligible to receive financial assistance through the Employment Insurance program, provided they have accumulated the requisite number of insurable hours during the qualifying period."

**With Content Style Guide context**, the AI knows Canada.ca patterns:
> "You may be eligible for Employment Insurance (EI) if you:
> - are at least 18 years old (or the age of majority in your province)
> - lost your job through no fault of your own
> - worked enough insurable hours in the past year"

The difference? The Content Style Guide provides clear rules:
- Use bullet points for eligibility criteria (not paragraph text)
- Write in second person ("you") to speak directly to citizens
- Keep sentences short (aim for 20 words or less)
- Define acronyms on first use
- Front-load information (most important first)

### Bilingual Content Patterns

The Content Style Guide also provides patterns for presenting bilingual content. An AI assistant learns standard approaches rather than inventing inconsistent patterns.

**Bilingual page titles** (Content Style Guide pattern):
```html
<h1 property="name" id="wb-cont">Apply for Employment Insurance (EI)</h1>
<p lang="fr"><strong>Français :</strong> <a href="?lang=fr">Demander l'assurance-emploi (AE)</a></p>
```

**Bilingual contact information** (Content Style Guide pattern):
```html
<section>
    <h2>Contact us</h2>
    <p>
        <strong>Telephone:</strong> 1-800-622-6232<br>
        <strong lang="fr">Téléphone :</strong> 1-800-622-6232
    </p>
    <p>
        <strong>Hours:</strong> Monday to Friday, 8:30 am to 4:30 pm (EST)<br>
        <strong lang="fr">Heures :</strong> Du lundi au vendredi, de 8 h 30 à 16 h 30 (HNE)
    </p>
</section>
```

These aren't invented patterns—they're documented standards. An AI assistant generates them consistently.

### Structured Content Templates

The Content Style Guide provides templates for common content types:

**Steps in a process**:
```markdown
## How to apply

1. **Gather your documents**

   You'll need:
   - Your Social Insurance Number (SIN)
   - Your employment details from the past year
   - Your direct deposit information

2. **Complete the online form**

   The form takes about 45 minutes to complete. You can save and return later.

3. **Submit your application**

   After you submit, you'll get a confirmation number. Keep this for your records.

4. **Wait for a decision**

   We'll contact you within 28 days.
```

Note the pattern:
- Numbered steps with bold headings
- Sub-bullets for details
- Specific timeframes ("45 minutes", "28 days")
- "You" language throughout
- Action-oriented headings ("Gather", "Complete", "Submit", "Wait")

An AI assistant with Content Style Guide context generates this structure automatically.

### The Power for Government Content

Here's the real benefit: Government websites contain a lot of procedural content—how to apply for services, what documents you need, what happens next. This content follows predictable patterns.

With the Content Style Guide encoded as instructions, an AI assistant can:
- Draft service pages following Canada.ca patterns
- Convert policy documents into plain language
- Generate bilingual content structures (English content with French equivalents marked)
- Suggest appropriate headings and content organization
- Flag jargon or overly complex sentences
- **Use official government terminology from TERMIUM Plus®** for consistent bilingual translations

### TERMIUM Plus®: Official Terminology for AI Assistants

The Translation Bureau's [TERMIUM Plus®](https://www.btb.termiumplus.gc.ca/tpv2alpha/alpha-eng.html?lang=eng) provides another layer of bounded vocabulary—official government terminology across millions of terms.

**Without TERMIUM Plus** context, an AI assistant might translate "Employment Insurance" inconsistently:
- "Assurance d'emploi" (literal translation, but incorrect)
- "Assurance contre le chômage" (old term, no longer used)
- "Assurance-emploi" (correct official term)

**With TERMIUM Plus** context, the AI assistant knows the official term is "**assurance-emploi**" and uses it consistently.

**Example**: Translating a technical government term like "Protected B" classification:

```
User: "How do I say 'Protected B information' in French?"

AI Assistant (with TERMIUM Plus):
The official Government of Canada term is:
- English: "Protected B"
- French: "Protégé B"

Note: According to TERMIUM Plus, security classifications (Unclassified,
Protected A/B/C, Classified, Secret, Top Secret) are not translated in
official GC usage—they remain in their original language to avoid confusion.
```

This prevents inconsistencies where some developers write "Protégé B", others write "Protégée B", and others leave it untranslated—TERMIUM Plus provides the authoritative answer.

AI assistants with TERMIUM Plus access could:
- Suggest official French equivalents when generating bilingual forms
- Flag incorrect terminology usage in code comments or UI text
- Ensure consistency with government standards across translations
- Generate bilingual error messages using approved terminology

**Example prompt to AI assistant**:
> "Write a service page for renewing a passport. Include eligibility, required documents, processing times, and how to apply."

**AI assistant with Content Style Guide context generates**:
```markdown
# Renew your passport

You can renew your passport if it's expired or will expire in the next year.

## Eligibility

You can renew your passport if you:
- are a Canadian citizen
- have a passport that expired less than 15 years ago
- were at least 16 when your passport was issued
- still have the same name (or can provide legal proof of name change)

[... continues following Content Style Guide patterns ...]
```

This is bounded vocabulary for content—just like WET-BOEW provides bounded vocabulary for components.

## How Digital Standards Provide "Explicit Guardrails"

The fourth requirement for effective AI assistance is **explicit guardrails**—institutional knowledge about what should and shouldn't be done.

This is where the 10 Digital Standards come in. They're not technical specifications—they're principles that guide decisions. But here's what makes them powerful for AI assistance: **They can be encoded into instruction files that AI assistants automatically apply.**

### Standard #6: Build in Accessibility from the Start

This standard means accessibility isn't a retrofit—it's a foundational requirement. An AI assistant with this guardrail won't suggest "let's build it first, then add accessibility later." It knows that's unacceptable.

When you ask for a data table, an AI with Digital Standard #6 context generates:

```html
<table class="table table-striped">
    <caption>Monthly expenses by category</caption>
    <thead>
        <tr>
            <th scope="col">Category</th>
            <th scope="col" class="text-right">Amount</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row">Housing</th>
            <td class="text-right">$1,200</td>
        </tr>
        <tr>
            <th scope="row">Transportation</th>
            <td class="text-right">$450</td>
        </tr>
        <tr>
            <th scope="row">Food</th>
            <td class="text-right">$600</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <th scope="row">Total</th>
            <td class="text-right"><strong>$2,250</strong></td>
        </tr>
    </tfoot>
</table>
```

Note what the AI included automatically:
- `<caption>` for context (WCAG 2.1 requirement)
- `scope` attributes on headers (helps screen readers associate data with headers)
- Semantic structure with `<thead>`, `<tbody>`, `<tfoot>`
- Right-alignment for numbers (accessibility best practice for readability)
- Appropriate use of `<th>` vs `<td>`

This isn't the AI being clever—it's the AI following the guardrail: "Build in accessibility from the start."

### Standard #5: Address Security and Privacy Risks

An AI assistant aware of this standard won't casually suggest storing sensitive data in browser localStorage or logging personally identifiable information (PII).

Ask it to build a form that collects personal information, and it might generate:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GC.BenefitsPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly ILogger<ApplicationController> _logger;
        private readonly IApplicationService _applicationService;

        public ApplicationController(
            ILogger<ApplicationController> logger,
            IApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        /// <summary>
        /// Process application submission with Protected B data.
        ///
        /// GC Compliance:
        ///     - Standard #5: Address security and privacy risks
        ///     - Protected B classification handling
        ///     - No PII in application logs
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitApplication(
            [FromBody] ApplicationSubmissionDto request)
        {
            // Log the event WITHOUT PII
            _logger.LogInformation(
                "Application submitted: {ApplicationId}, Service: {ServiceType}, Method: {Method}",
                request.ApplicationId,
                request.ServiceType,
                "web_form");

            try
            {
                // Process the application (PII handled in secure processing, not logged)
                var result = await _applicationService.ProcessApplication(
                    request.SocialInsuranceNumber,
                    request.FullName,
                    request);

                // Return confirmation (no PII in response for browser console exposure)
                return Ok(new
                {
                    Status = "success",
                    ConfirmationNumber = result.ConfirmationNumber,
                    NextStepsUrl = "/application/next-steps"
                });
            }
            catch (Exception ex)
            {
                // Log error WITHOUT exposing PII
                _logger.LogError(ex,
                    "Application processing failed: {ApplicationId}, ErrorType: {ErrorType}",
                    request.ApplicationId,
                    ex.GetType().Name);

                return StatusCode(500, new
                {
                    Status = "error",
                    Message = "We could not process your application. Please try again or contact support."
                });
            }
        }
    }
}
```

The guardrail is explicit: Don't log PII, handle Protected B data appropriately, provide safe error messages. The AI assistant encodes this into the generated code.

### Standard #9: Design Ethical Services

This standard asks: "Just because we can collect this data, should we?"

An AI assistant with Standard #9 context will question data collection requirements. Ask it to build a registration form that collects race, gender, and income information, and it might respond:

> "I can build this form, but per Digital Standard #9 (Design Ethical Services), we should first clarify:
>
> 1. **Necessity**: Is this demographic information required to deliver the service, or is it for analytics?
> 2. **Consent**: If for analytics, users should be able to opt out of providing this information.
> 3. **Privacy impact**: Has this data collection been reviewed via a Privacy Impact Assessment (PIA)?
> 4. **Transparency**: Will users understand why we're collecting this information and how it will be used?
>
> If this data is genuinely necessary, I'll include:
> - Clear explanations for why each field is requested
> - Optional vs. required field distinctions
> - A privacy notice linking to how data will be used
> - Option to skip demographic questions if they're for research purposes only"

This is the AI assistant acting as a guardrail—not just implementing what you asked, but questioning whether it aligns with ethical service design.

## The Problem: Buried Departmental Standards

We've talked about the official, published standards—WET-BOEW, GC Design System, Content Style Guide, Digital Standards. These are well-documented, publicly available, and (relatively) easy to find.

But every government developer knows there's another layer: **departmental and project-level standards that live in the shadows.**

### The SQL Standards Document Nobody Can Find

You start a new contract at a department. On day three, someone mentions in passing: "Oh, we have SQL coding standards. The DBAs wrote them a few years ago."

Where are they? Nobody's quite sure. You check:
- The project SharePoint (maybe in the "Archive" folder? Or was it "Documentation"?)
- The department wiki (if you can remember the URL)
- That network drive everyone shares (somewhere in `P:\IT\Database\Standards\Old\Maybe?`)
- Someone's OneDrive they shared once

You finally find a Word document: `SQL_Coding_Standards_v2_FINAL_revised_2019.docx`

You open it. It says:
- All table names must use `tbl_` prefix
- All stored procedures must use `usp_` prefix
- All column names must be PascalCase
- No use of SELECT * in production code
- All queries must be parameterized (no dynamic SQL)

**Questions immediately arise:**

1. **Is this still enforced?** The document is from 2019. Do new projects follow this?
2. **What about Entity Framework?** The standards assume raw SQL, but your project uses EF. Does that count? Can you use LINQ queries or are you supposed to write stored procedures?
3. **Do the prefixes still apply?** Modern database design typically avoids Hungarian notation prefixes. Is `tbl_` still required or is this outdated?
4. **Who enforces this?** Is there a code review checklist? Will the DBAs reject your pull request if you don't follow this?
5. **What's the penalty for non-compliance?** Is this a hard requirement or a "nice to have" guideline?

You ask the tech lead. They shrug: "I think we used to follow that, but the last project didn't. Just be consistent, I guess?"

### Other Buried Standards

SQL isn't the only example. Common buried standards include:

**Git branching strategy**:
- Some Word doc that says "use GitFlow" but half the team uses trunk-based development
- Nobody knows if feature branches should be `feature/TICKET-123` or `feature/ticket-123` or `TICKET-123-feature`
- Unclear if you should delete branches after merging

**Code review checklist**:
- Exists somewhere, but nobody references it during reviews
- Inconsistently applied (some reviewers are strict, others rubber-stamp)
- Outdated rules (still mentions Internet Explorer 9 compatibility)

**API naming conventions**:
- RESTful endpoints should be `/api/v1/resources` but some APIs use `/services/resource`
- Query parameters: camelCase or snake_case? (Both exist in production)
- Date formats: ISO 8601 or `YYYY-MM-DD` or Unix timestamps? (All three in use)

**Database naming conventions**:
- Schema names: `dbo`, department code (`ESDC_`), or application name (`benefits_`)?
- Foreign key naming: `FK_Table1_Table2` or `fk_table1_table2_id` or `table2_id`?
- Index naming conventions exist but nobody follows them

**Deployment procedures**:
- A 47-step checklist in a spreadsheet
- Steps reference tools that were replaced two years ago
- "Contact John to approve production deployment" (John left in 2022)

**Security review requirements**:
- Submit threat and risk assessment (TRA) to security team (which form? where?)
- Include security controls documentation (which template? 2020 version or 2023?)
- Threat model required (what format?)

### Why This Matters for AI Assistants

Here's the problem: **AI assistants can't help with standards they can't find.**

An AI assistant can read WET-BOEW documentation on GitHub because it's public, current, and clearly the authoritative source. But it can't:
- Find your department's SQL standards in SharePoint
- Know whether that 2019 document is still enforced
- Understand which coding patterns are actually used vs. officially documented
- Tell you if Entity Framework is allowed or banned

So you get inconsistent help:
- AI generates SQL with no `tbl_` prefix (because modern best practices avoid it)
- You get feedback in code review: "Needs tbl_ prefix per departmental standards"
- You update the code manually
- AI doesn't learn because the standard isn't in its context

**Every developer goes through this same cycle.** New contractors waste hours finding, reading, and clarifying buried standards. The AI assistant that could help accelerate their work doesn't know these standards exist.

### The Bigger Problem: Tribal Knowledge

Worse than buried standards is **tribal knowledge**—the unwritten rules that exist only in people's heads:

- "We don't actually use stored procedures anymore, even though the standard says to"
- "Ignore the tbl_ prefix rule, nobody's enforced that since 2020"
- "Always have Marie review database changes because she's the only one who understands the replication setup"
- "You can't deploy on Fridays" (learned from a production incident three years ago)
- "Use the staging environment, not the test environment—test hasn't worked since the migration"

An AI assistant has zero access to tribal knowledge. It will confidently suggest things that violate unwritten rules. You'll learn through trial and error and code review feedback—the same painful way everyone before you learned.

### The Cost

This isn't just frustrating—it's expensive:

**Onboarding time**: New contractors spend their first week figuring out "how things are done here." At $800/day, that's $4,000 per developer in wasted time.

**Inconsistent codebases**: Without clear, enforced standards, every developer follows their own patterns. Code reviews become arguments about style instead of substance.

**Repeated questions**: Tech leads spend hours answering the same questions: "What's the branching strategy?" "Do we use the tbl_ prefix?" "Is Entity Framework allowed?"

**AI assistants can't help**: The tools that could accelerate development are blind to your actual standards, so they generate code that doesn't match your patterns.

**Knowledge loss**: When the senior DBA who wrote the SQL standards retires, nobody knows if those standards were cargo cult or carefully reasoned requirements.

### What We Need

Departmental and project-level standards need to be:

1. **Findable**: Not buried in SharePoint—in the repository with the code
2. **Current**: Version-controlled, updated when practices change
3. **Clear**: Explicit about what's mandatory vs. guideline
4. **Enforced**: Automated checks where possible, clear code review criteria
5. **Machine-readable**: Structured so AI assistants can apply them automatically

This is exactly what the next section addresses.

## The Missing Piece: Machine-Readable Instructions

Here's where we get practical. Canada has all the pieces—WET-BOEW provides bounded vocabulary, the GC Design System provides predictable patterns, and the Digital Standards provide guardrails. But there's a problem:

**These resources exist as human-readable documentation, not machine-readable instruction files optimized for AI consumption.**

An AI assistant can technically read the WET-BOEW documentation on GitHub or the GC Design System website. But it's doing this on-demand, every time you ask a question, with no persistence or structure. It's like hiring a contractor who has to re-read the entire building code every time they install a light switch.

### Machine-Readable Instruction Files

The solution is to encode platform knowledge as structured instruction files that AI assistants can load automatically. An open-source template for this approach is available (MIT licensed, at [github.com/adhocteam/cloud.gov-instructions](https://github.com/adhocteam/cloud.gov-instructions)) and can be adapted for Canadian government development.

**The Canadian Cloud Model:**

Canada's approach through Shared Services Canada provides brokered access to multiple commercial cloud providers (AWS, Azure, Google Cloud) rather than a single unified platform. This makes instruction files **particularly valuable**—departments need clear patterns for each SSC-approved provider.

**Key patterns for effective instruction files:**

1. **Structured instruction files**: Organize platform knowledge into domain-specific instruction files (deployment, security, logging, etc.) that live in `.github/instructions/`

2. **Context-aware loading**: Use YAML frontmatter (`applyTo: "**/*.yml"`) to automatically load relevant instructions when developers work with specific file types

3. **Safety guardrails**: Explicitly categorize operations as "always confirm," "confirm in production," or "safe to run" so AI assistants know when to ask before executing destructive commands

4. **Automated compliance documentation**: Scan code annotations (like `/// ITSG-33: AC-2`) to generate security documentation automatically, reducing manual compliance burden

For Canadian government development, adapt these patterns by using SSC-brokered cloud providers (AWS, Azure), applying TBS IT Security Framework controls (ITSG-33), and encoding WET-BOEW/GC Design System patterns. The structure and approach remain the same.

## The Canadian Adaptation

Now let's bring this home. What would this instruction file structure look like adapted for Canadian government development?

### Proposed File Structure

```
.github/
├── copilot-instructions.md              # Project context (SSC provider, classification)
├── instructions/
│   ├── wet-boew.instructions.md         # WET-BOEW component patterns
│   ├── gc-design-system.instructions.md # GC Design System utilities and templates
│   ├── accessibility.instructions.md    # WCAG 2.1 AA compliance patterns
│   ├── bilingual.instructions.md        # Official Languages Act compliance
│   ├── api.instructions.md              # Standards on APIs compliance
│   ├── security-protected-b.instructions.md  # Protected B handling
│   ├── aws-deployment.instructions.md   # AWS deployment patterns (if using AWS)
│   └── azure-deployment.instructions.md # Azure deployment patterns (if using Azure)
├── agents/
│   └── security-controls.agent.md       # Security controls documentation generation
└── skills/
    └── wet-boew-troubleshoot.md         # Common WET-BOEW debugging workflows
```

**Note**: The cloud provider instruction files (AWS, Azure) would be specific to which SSC-brokered service your department uses. SSC could publish canonical versions for each approved provider.

Let's walk through what each of these files would contain, with concrete examples.

### 1. Repository-Level Context: copilot-instructions.md

This file provides the high-level context for the entire project.

**Example snippet** (from `.github/copilot-instructions.md`):

```markdown
# Government of Canada Web Application

This application is deployed via SSC Cloud Brokering Service and serves Canadian citizens via the Canada.ca domain.

## Classification
- **Security**: Protected B
- **Deployment environment**: AWS GovCloud (SSC-brokered)
- **Accessibility standard**: WCAG 2.1 AA (mandatory)
- **Language requirements**: Bilingual (English/French) per Official Languages Act
- **Framework**: WET-BOEW 4.0.x with Canada.ca theme

[Key Standards, Project Stack, Important Paths, and Development Workflow sections...]
```

**Full file**: See [copilot-instructions.md](/gc-ai-instructions/.github/copilot-instructions.md)

This gives the AI assistant the fundamental context: what kind of project this is, what standards apply, and what constraints must be respected.

### 2. WET-BOEW Instructions

This file tells the AI assistant how to use WET-BOEW components correctly.

**Example snippet** (from `.github/instructions/wet-boew.instructions.md`):

```markdown
---
applyTo: "**/*.html,**/*.cshtml,**/Views/**"
---

# WET-BOEW Component Instructions

All Government of Canada web applications must use WET-BOEW for WCAG 2.1 AA compliance.

## Base Template Structure

Every HTML page must use this structure...

[Forms with Validation, Date Picker, Alerts, Tabs, Tables, Bilingual Content, Accessibility Requirements, Testing Checklist, and Common Mistakes sections...]
```

**Full file**: See [wet-boew.instructions.md](/gc-ai-instructions/.github/instructions/wet-boew.instructions.md)

This instruction file gives the AI assistant concrete, copy-paste-ready examples of how to use WET-BOEW correctly. The `applyTo` frontmatter means these instructions load automatically whenever the developer is editing HTML or template files.

### 3. API Standards Instructions

When building Government of Canada applications with backend APIs, the Standards on APIs provide the technical requirements for API design.

**Example snippet** (from `.github/instructions/api.instructions.md`):

```markdown
---
applyTo: "**/Controllers/**,**/Api/**,**/*Controller.cs,**/*ApiClient.cs"
---

# REST API Design Instructions

Government of Canada APIs must follow REST principles, be well-documented, secure, and accessible.

## Government of Canada Standards on APIs

All Government of Canada APIs must comply with the **[Standards on APIs](https://www.canada.ca/en/government/system/digital-government/digital-government-innovations/government-canada-standards-apis.html)**.

**Key requirements**:

1. **Architecture**: RESTful model by default, JSON message format (UTF-8), resource-oriented URLs
2. **Security**: TLS 1.2+, JWT for authentication, API keys in headers (not URLs)
3. **Data Standards**: ISO 8601 datetime format in UTC, JSON responses as objects (not arrays)
4. **Versioning**: Format `v<Major>.<Minor>.<Patch>`, major version in URL (e.g., `/v3/`)
5. **Error Handling**: HTTP status codes, abstract internal details, consistent error format
6. **Performance**: Pagination required, restrict wildcard queries, published benchmarks
7. **Documentation**: OpenAPI specifications, published to API Store

[URL Structure, Response Format, Error Handling, Pagination, Versioning, Authentication sections...]
```

**Full file**: See [api.instructions.md](/gc-ai-instructions/.github/instructions/api.instructions.md)

This instruction file ensures that when developers build backend APIs for their GC applications (ASP.NET Core controllers, Express.js routes, etc.), the AI assistant automatically generates code that complies with the Standards on APIs. The `applyTo` frontmatter means these instructions load when working with API controllers or API client code.

### 4. Security Instructions for Protected B

This is where Canadian-specific compliance gets encoded.

**Example snippet** (from `.github/instructions/security-protected-b.instructions.md`):

```markdown
---
applyTo: "src/**/*.cs,src/**/*.js"
---

# Protected B Data Handling Instructions

This application handles Protected B information per TBS security classification.

## What is Protected B?

Information that could cause serious injury to individuals or organizations if compromised. Examples:
- Social Insurance Numbers (SIN)
- Medical records
- Financial information
- Personal contact information combined with identifiers

## Logging Requirements

**CRITICAL**: Never log Protected B information. Use structured logging with PII redaction...

[Logging examples, Data Storage, Session Management, Access Control, Input Validation, Database Queries, Security Headers, Incident Response, Testing Checklist, and Common Mistakes sections...]
```

**Full file**: See [security-protected-b.instructions.md](/gc-ai-instructions/.github/instructions/security-protected-b.instructions.md)

This instruction file provides concrete security patterns that the AI assistant can reference when generating code. Every code example includes GC security references, making it easy for developers to understand *why* each pattern is required. The patterns follow [Canadian Centre for Cyber Security (CCCS) guidance](https://www.cyber.gc.ca/en/guidance) on cryptography (CMVP-validated encryption modules), access controls (ITSG-33 controls), and secure cloud deployments (Cloud Security Profile requirements for Protected B data).

### 5. The Security Controls Documentation Agent

This is where automation gets powerful. Here's a Canadian-focused approach to automated security documentation:

**Example snippet** (from `.github/agents/security-controls.agent.md`):

```markdown
# Security Controls Documentation Agent

## Purpose
Generate security controls documentation by scanning the codebase for security implementation patterns. Supports Threat and Risk Assessments (TRA), security architecture reviews, and ITSG-33 compliance documentation.

## Supported Frameworks
- **ITSG-33**: IT Security Risk Management framework controls
- **TBS Security Policy**: Treasury Board of Canada Secretariat security requirements
- **CCCS Cloud Security Profile**: Canadian Centre for Cyber Security cloud-specific controls

## How It Works

### 1. Annotate Code with Security Implementation Notes

Add security control references in XML documentation comments...

[Code annotation examples, run commands, generated documentation including Security Controls Inventory, Security Implementation Summary, Threat Coverage Analysis sections, Usage Patterns, Control Reference Format, Benefits, Limitations, and Next Steps...]
```

**Full file**: See [security-controls.agent.md](/gc-ai-instructions/.github/agents/security-controls.agent.md)

This agent reduces the security documentation burden for government projects. Instead of manually writing security implementation descriptions, developers annotate their code and the AI assistant generates documentation automatically. This documentation supports Threat and Risk Assessments (TRA), security reviews, and any formal security assessment processes your department uses.

## Real-World Benefits for Government Contractors

Let's get practical. What does all this actually mean for someone bidding on or delivering a Government of Canada project?

### Benefit #1: Faster Onboarding

**The old way**: New contractor joins the project. Spends the first week reading WET-BOEW documentation, GC Design System guides, ITSG-33 controls, security assessment requirements, Protected B handling procedures, and accessibility standards. Still doesn't really understand "how things are done around here."

**With AI-friendly instructions**: New contractor clones the repository, and their AI assistant already knows the project context. They ask: "Create a new page for the eligibility checker" and the AI generates a WET-BOEW template with proper bilingual structure, correct accessibility attributes, and GC Design System CSS utilities—no manual reading required. They're productive on day one.

**Time saved**: Easily 3-5 days of onboarding per developer. On a team of 5 contractors at $800/day, that's $12,000-$20,000 saved per project just in reduced ramp-up time.

### Benefit #2: Automatic Compliance

**The old way**: Developer builds a feature, submits for accessibility review, fails WCAG 2.1 AA in 15 places. Spends 2 days fixing: adding ARIA labels, fixing keyboard navigation, adjusting color contrast, adding bilingual error messages. Resubmits. Still has 3 failures. Repeat.

**With AI-friendly instructions**: AI assistant generates code with WCAG 2.1 AA patterns built-in from the start. Uses WET-BOEW components that are already compliant. Suggests proper ARIA attributes automatically. Includes bilingual error messages because that's in the instruction files.

**Result**: First accessibility review passes with minor feedback. No rework cycle.

**Time saved**: 2-3 days per feature cycle. On a 6-month project with 20 features, that's 40-60 days saved. At $800/day, that's $32,000-$48,000.

### Benefit #3: Faster Iteration

**The old way**: Product owner says "Let's add a confirmation step with a summary of what they entered." Developer spends an hour figuring out the right WET-BOEW components, another hour getting the bilingual labels right, 30 minutes ensuring keyboard navigation works, 30 minutes fixing the layout to match Canada.ca patterns. Total: 3 hours for a simple confirmation page.

**With AI-friendly instructions**: "Create a confirmation page showing the user's application summary with WET-BOEW styling." AI assistant generates the page in 2 minutes with correct structure, bilingual labels, accessible summary tables, and a Canada.ca-compliant layout. Developer reviews, makes minor adjustments. Total: 20 minutes.

**Time saved**: 2.5 hours per page. Over a project with 30 pages, that's 75 hours saved—nearly 10 full work days. At $800/day, that's $7,500.

### Benefit #4: Knowledge Distribution

**The problem**: Your senior developer who understands WET-BOEW, accessibility, and GC patterns is expensive ($1,200/day). Your junior developers ($600/day) are capable but need constant guidance on "how to do things the GC way."

**The old way**: Junior developer asks senior developer 5-10 questions per day. Senior developer spends 2 hours/day answering questions and reviewing code for GC compliance. Senior developer's productivity drops 25%.

**With AI-friendly instructions**: Junior developer's AI assistant provides GC-specific guidance automatically. "Should I use a `<div>` or `<button>` for this?" Assistant: "Use `<button>` for keyboard accessibility (WCAG 2.1 requirement)." "How do I add bilingual labels?" Assistant shows the exact pattern from the instruction file.

**Result**: Senior developer spends 30 minutes/day on guidance instead of 2 hours. Junior developer works more independently.

**Time saved**: 1.5 hours/day of senior developer time. Over a 120-day project, that's 180 hours = 22.5 days. At $1,200/day, that's $27,000 saved (plus improved senior developer utilization).

### Benefit #5: Reduced Security Documentation Burden

**The old way**: Security review is coming up. Team spends 2 weeks creating security documentation, mapping code to ITSG-33 controls, generating evidence, writing implementation descriptions for the TRA. Much of this is tedious copying from code comments into Word documents.

**With the security controls agent**: Run `@security-controls scan`. Agent generates 70% of the security documentation automatically by reading code annotations. Team spends 3 days reviewing and adding manual sections (physical security, organizational policies, etc.) instead of 2 weeks creating from scratch.

**Time saved**: 7 days of team effort. On a 3-person team at $800/day, that's $16,800 saved.

**Better outcome**: Documentation stays in sync with code because it's generated from code, not manually written. Security reviews go faster because evidence trails are clear, and you have documentation ready for TRA or whatever security assessment process your department uses.

### Benefit #6: Consistent Code Quality

**The problem**: Team of 8 contractors over 18 months, rotating every 6 months. Coding styles vary. Some contractors use WET-BOEW patterns correctly, others invent custom solutions. Code reviews are slow because reviewers have to check for GC compliance in addition to functionality.

**With AI-friendly instructions**: All contractors' AI assistants use the same instruction files. Code generated follows consistent WET-BOEW patterns, security practices, and GC Design System conventions—regardless of who's writing it.

**Result**: Faster code reviews (less variance to check), more maintainable codebase, easier contractor transitions.

**Time saved**: Estimate 30 minutes/day on code reviews across the team. Over 300 days, that's 150 hours = 19 days. At $800/day, that's $15,200.

### Total Potential Savings

Let's add it up for a typical 6-month Government of Canada web application project:

| Benefit | Time Saved | Dollar Value (at $800/day) |
|---------|------------|----------------------------|
| Faster onboarding | 15-25 days | $12,000-$20,000 |
| Automatic compliance | 40-60 days | $32,000-$48,000 |
| Faster iteration | 10 days | $7,500 |
| Knowledge distribution | 22.5 days | $27,000 (senior dev time) |
| Reduced security documentation | 7 days | $16,800 |
| Consistent code quality | 19 days | $15,200 |
| **Total** | **113-144 days** | **$110,500-$134,500** |

On a typical $500,000 contract, that's a **22-27% cost reduction** or profit margin improvement.

But the benefits aren't just financial:

- **Higher quality deliverables**: Fewer accessibility failures, better security compliance, more consistent UX
- **Happier clients**: Faster delivery, fewer rework cycles, better documentation
- **Happier developers**: Less time reading docs, more time building features, clearer guidance on "the right way"

## Practical Steps: What You Can Do Today

This all sounds great in theory. But how do you actually start using these patterns on your next Government of Canada project?

### For Individual Developers/Contractors

If you're a developer or contractor working on a GC project, here's how to get started:

#### Step 1: Create Repository Instructions (10 minutes)

Create `.github/copilot-instructions.md` in your project repository:

```markdown
# [Your Project Name] - Government of Canada

This is a Protected B web application for [Department Name], deployed via SSC Cloud Brokering Service to AWS.

## Framework & Standards
- WET-BOEW 4.0.x with Canada.ca theme
- WCAG 2.1 AA accessibility compliance (mandatory)
- Bilingual (English/French) per Official Languages Act
- GC Design System CSS utilities
- Protected B data classification

## Key Technologies
- Frontend: WET-BOEW, vanilla JavaScript
- Backend: [Your stack: ASP.NET Core, Node.js, etc.]
- Database: [MS SQL Server, Oracle, etc.]
- Deployment: AWS (SSC-brokered) or Azure Government

## Important Notes
- All user-facing text must be bilingual
- No PII in application logs (see src/Logging/AuditLogger.cs for patterns)
- 15-minute session timeout for Protected B compliance
- Security review and TRA required before production deployment

## Before Committing
- Run accessibility tests: npm run test:a11y
- Verify bilingual content (EN + FR)
- Check for PII in logs: npm run test:pii-detection
```

This gives your AI assistant immediate project context.

#### Step 2: Create Domain Instruction Files (30-60 minutes)

Create `.github/instructions/` directory with key instruction files. Start with the most impactful ones:

**WET-BOEW basics** (`wet-boew.instructions.md`):
- Copy the WET-BOEW template structure from earlier in this post
- Add the 3-5 components you use most (forms, alerts, tables, tabs)
- Include bilingual patterns for your specific content
- Add `applyTo: "**/*.html,**/*.cshtml"` frontmatter

**Security for Protected B** (`security-protected-b.instructions.md`):
- Copy the logging patterns (no PII in logs)
- Include your session configuration
- Add your specific encryption patterns if applicable
- Add `applyTo: "src/**/*.cs,src/**/*.js"` frontmatter

**Accessibility checklist** (`accessibility.instructions.md`):
- List your most common WCAG 2.1 AA patterns
- Include keyboard navigation requirements
- Add color contrast standards
- Add `applyTo: "**/*.html,**/*.css"` frontmatter

**Departmental coding standards** (`database.instructions.md`):
- **This is the key one!** Dig up those buried SQL standards and encode them as instruction files
- Include database naming conventions, query patterns, ORM usage rules
- Make them findable and enforceable
- Add `applyTo: "**/*.sql,**/Models/**,**/Repositories/**"` frontmatter

**Example snippet** (from `.github/instructions/database.instructions.md`):

```markdown
---
applyTo: "**/*.sql,**/models/**,**/repositories/**,**/migrations/**"
---

# Department Database Standards

These standards were established by the DBA team and are enforced in code reviews.

## Database Platform

**Primary database**: MS SQL Server 2019 (on-premises) / Azure SQL Database (cloud)
**Legacy systems**: Oracle 12c (being migrated to SQL Server)

Entity Framework Core supports both platforms via provider packages:
- SQL Server: `Microsoft.EntityFrameworkCore.SqlServer`
- Oracle: `Oracle.EntityFrameworkCore`

[Connection configuration, EF Core Usage, Naming Conventions, Query Requirements, Migrations, Performance Guidelines, and Code Review Checklist sections...]
```

**Full file**: See [database.instructions.md](/gc-ai-instructions/.github/instructions/database.instructions.md)

**Why this works**:
- ✅ **Findable**: In the repository, not buried in SharePoint
- ✅ **Current**: Shows deprecated rules (~~tbl_ prefix~~), approved tools (EF Core OK as of 2023)
- ✅ **Clear**: Explicit about what's mandatory (parameterization) vs. guideline (naming)
- ✅ **Enforced**: Code review checklist included
- ✅ **Machine-readable**: AI assistant automatically applies these patterns when you work with database code

Now when AI assistants generate database code, they'll:
- Use EF Core (approved tool)
- Follow department naming conventions
- Use parameterized queries
- Include proper navigation properties
- Generate code that passes review the first time

#### Step 3: Add Safety Guardrails (15 minutes)

Create `.github/AGENTS.md`:

```markdown
# Safety Guardrails

## Always Confirm Before Running

- `aws s3 rm --recursive` - Deletes S3 bucket contents (data loss)
- `az group delete` - Deletes entire Azure resource group (data loss)
- `dotnet ef database drop` - Drops database (permanent data loss)
- `git push --force` - Overwrites remote history
- `rm -rf` - Recursive file deletion

## Confirm in Production

- `dotnet publish` - Builds for production deployment
- `az webapp deploy` - Deploys application to Azure App Service
- `aws lambda update-function-code` - Updates AWS Lambda function
- `git push origin main` - Pushes to main branch (triggers CI/CD)
- `dotnet ef database update` - Runs database migrations (may affect production data)

## Safe to Run

- `az webapp log tail` - View logs (Azure)
- `aws cloudwatch tail` - View logs (AWS)
- `git status` - Show repository status
- `dotnet test` - Run tests
- `az resource list` - List Azure resources
```

#### Step 4: Test with Your AI Assistant

Now ask your AI assistant to generate code:

- "Create a contact form page using WET-BOEW"
- "Add logging for user registration without including PII"
- "Generate an accessible data table showing application status"

Watch how the AI assistant incorporates the patterns from your instruction files.

#### Step 5: Iterate and Improve

As you work, add more patterns to your instruction files:
- Component patterns you use frequently
- Bilingual text examples for your domain
- Security patterns specific to your application
- Troubleshooting steps for common issues

Total time investment: **1-2 hours**
Payoff: Weeks of time saved over the project lifecycle

### For Project Teams

If you're a team lead or senior developer, here's how to scale this across your team:

#### Step 1: Create Shared Instruction Repository (2-4 hours)

Create a team-level instruction repository that all projects can use:

```
gc-instructions/
├── README.md
├── wet-boew/
│   ├── base-template.instructions.md
│   ├── forms.instructions.md
│   ├── alerts.instructions.md
│   ├── tables.instructions.md
│   └── bilingual.instructions.md
├── security/
│   ├── protected-b.instructions.md
│   ├── logging.instructions.md
│   └── session-management.instructions.md
├── accessibility/
│   ├── wcag-checklist.instructions.md
│   └── testing.instructions.md
├── gc-design-system/
│   ├── utilities.instructions.md
│   └── page-templates.instructions.md
├── cloud/
│   ├── aws-protected-b.instructions.md     # AWS deployment for Protected B (if team uses AWS)
│   └── azure-protected-b.instructions.md   # Azure deployment for Protected B (if team uses Azure)
└── agents/
    └── security-controls.agent.md
```

This becomes your team's shared knowledge base. The `cloud/` directory contains provider-specific instructions based on which SSC-brokered service your team uses.

#### Step 2: Adopt in New Projects (30 minutes per project)

For each new GC project:

1. Fork or copy the `gc-instructions` repository
2. Customize for project-specific context (department, classification level, tech stack)
3. Add project-specific patterns as they emerge
4. Contribute general patterns back to the shared repository

#### Step 3: Training and Onboarding (1 hour per developer)

When new contractors join:

1. Show them the instruction files: "This is how we encode GC compliance patterns"
2. Demonstrate AI assistant usage: "Watch how it generates WET-BOEW components automatically"
3. Explain the safety guardrails: "Always confirm destructive operations"
4. Review the security documentation workflow: "Annotate your code with ITSG-33 control references for security reviews"

New contractors are productive immediately because the AI assistant guides them.

#### Step 4: Code Review Standards

Update your code review checklist:

- ✅ Uses WET-BOEW components (not custom implementations)
- ✅ Includes bilingual content (EN + FR)
- ✅ No PII in logs
- ✅ Security control annotations in code
- ✅ Follows GC Design System patterns

The AI-generated code should already meet most criteria, making reviews faster.

#### Step 5: Iterate as a Team

Hold a monthly retrospective:
- What patterns did we learn this month?
- What should we add to instruction files?
- What WCAG issues came up in accessibility reviews?
- What security patterns are we using repeatedly?

Continuously improve your instruction files based on real project experience.

## Caveats and Considerations

Before you rush off to implement all of this, let's address some important caveats.

### AI Assistants Are Tools, Not Replacements

This is critical: **You are still accountable for the code your AI assistant generates.**

AI assistants can:
- Generate boilerplate code following established patterns
- Suggest component usage based on instruction files
- Identify missing accessibility attributes
- Format bilingual content correctly
- Reference security controls in code comments

AI assistants cannot:
- Understand your users' needs (that's your job)
- Make architectural decisions without context
- Determine if a feature actually solves the problem
- Know if generated code has subtle security flaws
- Replace human code review and testing

**Always review AI-generated code.** Check for:
- Security vulnerabilities (injection attacks, XSS, CSRF)
- Logic errors (does it actually do what you need?)
- Accessibility issues (automated tools catch ~30% of WCAG issues)
- Bilingual accuracy (is the French translation correct, not just present?)
- Performance implications (is this efficient?)

Think of AI assistants as **very knowledgeable junior developers**: They know the patterns and can write syntactically correct code quickly, but they need senior review and guidance.

### Context Still Matters

Instruction files help AI assistants understand GC standards, but they don't replace understanding your specific project context.

**Unique project requirements** still need human judgment:
- Novel UX patterns that don't fit standard templates
- Complex business logic specific to your domain
- Integration with legacy systems
- Performance optimization for high-traffic services
- Custom accessibility accommodations beyond WCAG 2.1 AA

When you encounter these situations, the AI assistant can help with implementation details, but the design decisions are still yours.

### User Research Is Still Required

Digital Standard #1 is "Design with Users." AI assistants can't do user research for you.

Even if an AI assistant generates a perfectly WCAG 2.1 AA compliant, bilingual form using WET-BOEW components and GC Design System styling—**that doesn't mean users will understand it or be able to complete their task.**

You still need to:
- Conduct user research to understand needs
- Test prototypes with real users
- Iterate based on feedback
- Validate that your service is actually usable

AI assistants accelerate the **implementation** of user-centered designs, but they don't create user-centered designs.

### Keep Standards Updated

WET-BOEW, the GC Design System, and security standards evolve. Your instruction files need to evolve with them.

**Outdated instruction files** can be worse than no instructions at all—your AI assistant will confidently generate code using deprecated patterns.

Establish a maintenance schedule:
- Review instruction files quarterly
- Update when WET-BOEW releases new versions
- Revise when accessibility standards change (WCAG 2.2, for example)
- Adjust when security requirements evolve

If you're maintaining team or department-level instruction files, assign someone to keep them current.

### Government-Specific Challenges

There are real constraints in government that affect AI assistant usage:

**Security classification**: Many AI coding assistants are cloud-based (GitHub Copilot, ChatGPT, Claude). If you're working with **Protected B or higher data**, you may not be able to use these tools at all without violating security policy.

**Options for Protected environments**:
- Use AI assistants that run locally (like locally-hosted code models)
- Use cloud-based assistants in development environment with synthetic data (never paste real PII)
- Wait for government-approved AI tools (currently in procurement at SSC)

**Tool approval**: Not all AI coding assistants are on approved software lists. Check with your department's IT security team before using any AI tool.

### Ethical Considerations

Using AI assistants in government software development raises ethical questions:

**Accountability**: If AI-generated code has a bug that affects citizens, who's responsible? (Answer: You are. Always.)

**Bias**: AI models are trained on existing code, which may contain biases. Be especially careful when AI assistants generate:
- Language detection algorithms (might not handle Indigenous languages)
- Name validation (might reject non-Western name formats)
- Address validation (might not handle Canadian address diversity)

**Transparency**: Should citizens know that AI assistants helped build their government services? How do we communicate this? There's no clear answer yet, but it's worth discussing with your team.

**Job impact**: Will AI assistants reduce the need for junior developers? How do we ensure opportunities for career growth? These are industry-wide questions, but they matter in government too.

Per Digital Standard #9 (Design Ethical Services), these considerations should be part of your project planning.

## Conclusion: Canada Is Ahead of the Curve (We Just Need to Realize It)

Here's the key insight: **Canada didn't build its digital government infrastructure for AI assistants—but it turns out this is exactly what AI assistants need to excel.**

For over 15 years, the Government of Canada has been building a two-layer ecosystem that creates effective AI assistance:

### Standards & Frameworks Layer

✅ **WET-BOEW**: Bounded vocabulary for accessible, bilingual UI components
✅ **Canada.ca Content Style Guide**: Bounded vocabulary for plain language, citizen-centered content
✅ **Digital Standards**: Explicit guardrails for ethical, accessible, secure service design
✅ **Standards on APIs**: Technical specifications for consistent API design
✅ **GC Cloud Guardrails**: Mandatory security baselines for cloud deployments
✅ **CCCS Security Frameworks**: Authoritative guidance (ITSG-33, CMVP, Cloud Security Profile)

### Platform Providers Layer

✅ **Canadian Digital Service (CDS)**: The central driver of digital modernization
  - **GC Design System**: Predictable patterns through design tokens, templates, and utilities
  - **GC Notify**: Standardized notification service
  - **GC Forms**: Accessible, bilingual form-building platform
  - **GC Sign-in**: Next-generation authentication service
  - **GC Issue and Verify**: Digital credentials service
  - **Service Digital Toolkit**: Practical guidance for implementing Digital Standards

✅ **Shared Services Canada (SSC)**: Cloud brokering with framework agreements for AWS, Azure, Google Cloud

✅ **Translation Bureau**: TERMIUM Plus® official terminology database and GCtranslate AI-powered translation

Other countries are scrambling to figure out how to make AI coding assistants work in government. Canada already has the pieces—we just need to make them machine-readable.

### The Opportunity

By adding **machine-readable instruction files** to Canada's mature digital ecosystem, developers can dramatically improve their productivity on government projects.

Imagine:
- Onboarding to new GC projects in hours instead of weeks
- Security documentation generating automatically from code annotations, supporting TRA and security reviews
- Accessibility compliance is the default, not a struggle
- Getting GC-standards guidance from AI assistants as you code
- Delivering Canada.ca services faster with higher quality

This isn't science fiction. This pattern is already being used successfully and is ready to be adapted for Canadian context.

### The Path Forward

**For individual developers**: Start today. Create `.github/copilot-instructions.md` in your current project. Add basic WET-BOEW patterns. Test with your AI assistant. Iterate. (Time investment: 1-2 hours. Payoff: Weeks saved over the project.)

**For project teams**: Create shared instruction repositories. Build your team's GC compliance knowledge base. Train team members on AI-assisted development. Contribute patterns back to the community. (Time investment: 2-4 hours initially. Payoff: 20-30% cost reduction across projects.)

### Final Thought

The future of government software development isn't about AI replacing developers—it's about developers and AI working together within well-designed, standards-based platforms.

Canada already has those platforms. We built them over 15 years, one component at a time, one standard at a time. Now we just need to make them AI-friendly by adding machine-readable instruction files.

That's not a radical transformation. It's a natural evolution that makes your job easier.

Start with a simple `.github/copilot-instructions.md` file in your next GC project. Add a few WET-BOEW patterns. Watch how much faster you can build compliant government applications.

Let's get to work.

---

## Your Turn

Have you worked on Government of Canada projects? What patterns would you add to GC instruction files? Have you tried using AI assistants for WET-BOEW or accessibility compliance?

I'd love to hear your experiences. Drop a comment below or reach out—let's build this together.

---

*This post was inspired by Ad Hoc's excellent article "[How government platforms can make AI coding assistants more effective](https://adhoc.team/2025/01/14/ai-coding-instructions/)" and their open-source [cloud.gov-instructions repository](https://github.com/adhocteam/cloud.gov-instructions). Thanks to the Ad Hoc team for showing what's possible.*
