# GC AI Instructions - Example Repository

This repository contains example AI instruction files for Canadian Government software development, as described in the blog post ["How Canadian Government Platforms Make AI Coding Assistants More Effective"](/blog/government-platforms/canadian-ai-assistants/).

## Purpose

These instruction files help AI coding assistants generate code that follows Government of Canada standards for:
- Accessibility (WCAG 2.1 AA)
- Bilingual requirements (Official Languages Act)
- Security (Protected B data handling)
- WET-BOEW component usage
- GC Design System patterns

## Repository Structure

```
.github/
├── copilot-instructions.md           # Repository-level context
├── instructions/
│   ├── wet-boew.instructions.md      # WET-BOEW component patterns
│   ├── security-protected-b.instructions.md  # Protected B security
│   ├── database.instructions.md      # Database standards and EF Core
│   └── aws-protected-b.instructions.md      # AWS deployment via SSC
├── agents/
│   └── security-controls.agent.md    # Security controls documentation generation
└── skills/
    └── (future troubleshooting skills)
```

## Usage

1. Copy the relevant instruction files to your project's `.github/` directory
2. Customize for your specific department, technology stack, and classification level
3. Your AI assistant will automatically apply these patterns when working with matching files

## License

MIT License - Free to use and adapt for your Government of Canada projects

## Contributing

This is an example repository. For production use, adapt these files to your department's specific requirements.
