# Government of Canada Web Application

This application is deployed via SSC Cloud Brokering Service and serves Canadian citizens via the Canada.ca domain.

## Classification
- **Security**: Protected B
- **Deployment environment**: AWS GovCloud (SSC-brokered)
- **Accessibility standard**: WCAG 2.1 AA (mandatory)
- **Language requirements**: Bilingual (English/French) per Official Languages Act
- **Framework**: WET-BOEW 4.0.x with Canada.ca theme

## Key Standards
- Treasury Board Standard on Web Accessibility
- Treasury Board Standard on Web Usability
- Treasury Board Standard on Web Interoperability
- Government of Canada Digital Standards (all 10 standards apply)
- ITSG-33 security assessment process required

## Project Stack
- Frontend: WET-BOEW 4.0.x, GC Design System CSS utilities, vanilla JavaScript
- Backend: ASP.NET Core 8.0 with C#
- Database: Azure SQL Database (SSC-brokered) or on-premises MS SQL Server
- Session storage: Azure Cache for Redis (SSC-brokered)
- Logging: Structured JSON to stdout (captured by AWS CloudWatch or Azure Monitor)

## Important Paths
- `wet-boew/`: WET-BOEW library files (kept up-to-date with official releases)
- `GCWeb/`: The WET-BOEW theme files customized for Canada.ca
- `Views/`: Razor views (.cshtml) with WET-BOEW structure
- `src/`: C# application code
- `tests/`: Automated tests (unit, integration, accessibility via axe-core)
- `docs/security/`: Security documentation (TRA, security controls, etc.)

## Development Workflow
1. All changes require WCAG 2.1 AA compliance testing (automated via axe-core)
2. All user-facing text must be bilingual (English in templates, French in `translations/fr.json`)
3. Protected B data handling requires security review before deployment
4. Deployment to production requires security team sign-off (contact: security-team@example.gc.ca)
