+++
title = "Organizing Source Code for Scientific Programmers: Let's Start a Conversation"
date = "2025-12-20"
publishDate = 2025-12-22
draft = false
description = "How should scientists organize their code repositories? This post proposes a language-agnostic structure and asks the community to share their approaches."
category = "architecture"
tags = ["project organization"]
image = "/images/scientific-code-organization-header.png"
aliases = ['/posts/scientific-code-organization/']
+++

If you've ever opened a scientific code repository and found yourself lost in a maze of analysis files, scripts, data, and outputs all jumbled together, you're not alone. While line-of-business developers have established conventions (like [David Fowler's .NET project structure](https://gist.github.com/davidfowl/ed7564297c61fe9ab814)), scientific computing has its own unique needs that require a different approach.

Scientific code repositories face challenges that traditional software projects don't: managing raw and processed data, organizing interactive analyses alongside automated scripts, handling computational experiments, and ensuring reproducibility across different computing environments and languages.

**This post proposes a starting point for discussion.** I'd love to hear from the community: How do *you* organize your scientific codebases? What works? What doesn't? What am I missing?

## Why Organization Matters in Scientific Computing

Poor organization doesn't just create inconvenience—it creates real problems. Lost data, irreproducible analyses, and hours wasted searching for files are common consequences. A well-organized repository accelerates science, facilitates collaboration, and makes your research reproducible.

When everyone on your team can predict where files should be, collaboration becomes intuitive rather than frustrating. But what's the right structure?

## The Fundamental Principle: Separate by Type and Purpose

The key organizing principle for scientific repositories is to structure by file type and purpose. This creates consistency across projects and makes it intuitive to find what you need. Here's a proposed structure:

```
project-name/
├── data/
│   ├── raw/
│   └── processed/
├── src/
│   ├── data_processing/
│   ├── analysis/
│   └── visualization/
├── notebooks/
├── scripts/
├── results/
│   ├── figures/
│   └── output/
├── docs/
├── tests/
├── environment/
├── README.md
├── LICENSE
└── .gitignore
```

Let's break down each component and when to use it. But remember: **this is a starting point for discussion, not a rigid prescription.**

## Core Directories

### `data/` - The Sacred Ground

Your data directory should have two subdirectories:

- **`data/raw/`**: Original, unmodified data files. Treat this as **immutable**. Never, ever modify files here. You can even set these files as read-only to prevent accidents.
- **`data/processed/`**: Cleaned, transformed, or analyzed versions of your data.

This separation is crucial for reproducibility. Anyone should be able to run your processing code on the raw data and regenerate your processed data.

**Important note**: For large datasets, you may want to store only metadata or download scripts in version control rather than the actual data files. Consider using services like OSF, Zenodo, or institutional repositories for large data files.

**Question for the community**: Do you organize data differently? How do you handle intermediate processing stages? Do you have a `data/interim/` directory?

### `src/` - Your Core Analysis Code

This is where reusable, production-quality code lives. Think of this as the "scientific guts" of your project. Code here should be:

- Organized into logical modules or packages
- Well-documented
- Testable and (ideally) tested
- Importable from interactive environments or scripts

**Question for the community**: How do you organize multi-language projects? Do you have separate directories for different languages, or do you mix them in `src/`?

### `notebooks/` - Exploration and Prototyping

Interactive environments (Jupyter notebooks, R Markdown, Pluto.jl notebooks, MATLAB Live Scripts, Mathematica notebooks) are fantastic for exploration, but they come with challenges: they can encourage non-modular code, become difficult to test, and tend to accumulate cruft over time.

**Best practices:**

1. Use interactive environments for exploration, visualization, and prototyping
2. Keep them focused on specific questions or analyses
3. Name them clearly with numbers for ordering: `01-data-exploration.ipynb`, `02-initial-modeling.Rmd`, `03-sensitivity-analysis.jl`
4. Transition mature code to the `src/` directory
5. Import functions from `src/` rather than copying code between notebooks

When an interactive session starts to feel unwieldy, extract the reusable parts into modules in `src/`.

**Question for the community**: Some researchers prefer to keep all work in notebooks/scripts. Others prefer to move everything to modules. What's your philosophy? Does it depend on the project stage?

### `scripts/` - Automation and Batch Processing

Scripts are for automated, reproducible workflows. These files:

- Run without interaction
- Accept command-line arguments or configuration files
- Can be executed on computing clusters or in pipelines
- Orchestrate complete analyses from start to finish

Example use cases:

- Data download and preprocessing pipelines
- Running models with different parameters
- Generating all figures for a paper
- Batch processing multiple datasets

A controller script (e.g., `run_all.sh`, `Makefile`, `Snakefile`) that executes the entire analysis workflow in order is extremely valuable for reproducibility.

**Question for the community**: Do you use workflow managers (Make, Snakemake, Nextflow, Drake, Luigi)? How do you organize pipeline definitions?

### `results/` - Analysis Outputs

Store generated outputs here, not in version control (add to `.gitignore`):

```
results/
├── figures/        # Plots, visualizations
├── output/         # Tables, statistics, processed results
└── models/         # Trained models, fitted parameters
```

Why separate from data? Results are *generated* by code and should be reproducible. If you lose them, you can regenerate them. Data (especially raw data) cannot be regenerated.

**Question for the community**: Do you version control any results? How do you handle results that take days/weeks to generate?

### `docs/` - Documentation

Include:

- Project documentation
- Analysis notes or lab notebook entries
- Manuscript drafts
- Supplementary materials
- API documentation (if auto-generated)

**Question for the community**: Where do you keep your manuscript? In the repo? In a separate repo? In Overleaf or Google Docs?

### `tests/` - Test Code

Yes, even scientific code should have tests! At minimum, include:

- Unit tests for core functions
- Integration tests for workflows
- Validation tests that check against known results

Testing helps ensure correctness and catches bugs when you modify code.

**Question for the community**: What's your testing philosophy for scientific code? Do you test everything, just critical functions, or not at all?

## Essential Files in the Root

### README.md

Your README is the front door to your project. It should include:

- Project overview and goals
- Installation instructions
- Quick start guide
- Project structure explanation
- How to reproduce key results
- Dependencies and requirements
- Citation information
- Contact information

### Environment Management

Include files that specify your computational environment. The specifics depend on your language and tools:

**Python:**

- `environment.yml` (for conda)
- `requirements.txt` (for pip)
- `pyproject.toml` (for modern Python packaging)

**R:**

- `renv.lock` (for renv)
- `DESCRIPTION` (for R packages)
- `install.R` (installation script)

**Julia:**

- `Project.toml` and `Manifest.toml`

**MATLAB:**

- Dependency list in README or separate documentation

**Multi-language projects:**

- `Dockerfile` (for containerized environments)
- Separate environment files for each language
- Shell script that sets up entire environment

**Question for the community**: How do you handle dependencies that span multiple languages? Do you use containers? Virtual machines? Detailed documentation?

### LICENSE

If your project is open source, include a license. Common choices for scientific code include MIT, BSD, or GPL licenses.

### .gitignore

Prevent clutter by ignoring generated files. Language-specific examples can be found at https://github.com/github/gitignore/tree/main.

**All projects:**

```
data/raw/*
results/*
.DS_Store (for the macOS operating system)
```

## Organizing for Different Project Scales

### Small Projects (Single Analysis → Single Paper)

For small projects, a simplified structure works well:

```
project/
├── data/
├── analysis/
├── results/
├── environment/
└── README.md
```

This is fine for exploratory work or class projects where you don't expect significant expansion.

### Large Projects (Multiple Studies, Multiple Papers)

For field studies, multi-year projects, or projects with multiple outputs:

```
project/
├── data/
│   ├── study1/
│   ├── study2/
│   └── shared/
├── src/
│   ├── preprocessing/
│   ├── analysis_core/
│   └── utils/
├── analyses/
│   ├── paper1/
│   ├── paper2/
│   └── exploratory/
├── docs/
└── manuscripts/
    ├── paper1/
    └── paper2/
```

The key is organizing analyses by the output (paper, report) they support while keeping shared code in a central `src/` directory.

**Question for the community**: How do you organize multi-year, multi-paper projects? One repo or many? How do you handle shared code?

## Self-Contained Projects

Strive for self-containedness: everything needed to reproduce your analysis should be in the project directory. This sometimes conflicts with avoiding duplication (e.g., when multiple projects share data), but for publishable research, self-containedness usually wins.

A colleague should be able to:

1. Clone your repository
2. Set up the environment
3. Run your analysis scripts
4. Reproduce your results

**Question for the community**: How do you balance self-containedness with sharing code/data between projects?

## Version Control Best Practices

### What to Track

- All code (scripts, interactive analyses, source files)
- Documentation
- Environment specifications
- Small data files (< 100MB)
- Metadata about large data files

### What NOT to Track

- Large data files (use data repositories instead)
- Generated results and figures
- Temporary files
- Language-specific artifacts (compiled binaries, caches)
- IDE-specific settings (unless shared intentionally)

### Commit Messages

Good commit messages should describe *what changed scientifically*, not just what changed in the code:

> - ❌ "Updated analysis"

> - ✅ "Added bootstrap confidence intervals to treatment effect estimates"

**Question for the community**: Do you have commit message conventions for scientific projects?

## Common Pitfalls and Solutions

### Pitfall 1: The Analysis File Sprawl

**Problem**: 50 files with names like "Untitled1", "Copy of Final_Analysis_v3"

**Solution**:

- Name files descriptively with numbered prefixes
- Regularly archive or delete obsolete files
- Transition stable code to modules

### Pitfall 2: Data Soup

**Problem**: Raw data, processed data, results all mixed together

**Solution**:

- Strict separation of raw/processed/results
- Document data provenance
- Use metadata files to track processing steps

### Pitfall 3: Hardcoded Paths

**Problem**: Absolute paths that only work on one person's computer

**Solution**:

- Use relative paths from the project root
- Use path manipulation libraries (pathlib in Python, file.path in R, etc.)
- Consider configuration files for environment-specific settings

### Pitfall 4: Missing Dependencies

**Problem**: "It works on my machine" but fails everywhere else

**Solution**:

- Maintain updated environment specifications
- Document system dependencies
- Consider using containers (Docker, Singularity) for complex environments

**Question for the community**: What other pitfalls do you encounter? What solutions work for you?

## Making This Real: A Call for Examples

Theory is great, but examples are better. I'd love to see:

- Links to well-organized public scientific repositories
- Templates or cookiecutter projects for different fields
- Adaptations of this structure for specific use cases
- Counter-arguments: When does this structure NOT work?

## Conclusion: The Conversation Starts Here

Good code organization isn't about following rules dogmatically—it's about making your life easier and your science better. The structure I've proposed here is a starting point, not a final answer.

**Core principles that seem universal:**

1. **Separate concerns**: Data, code, and results in different directories
2. **Preserve raw data**: Never modify original data files
3. **Make code modular**: Extract reusable functionality
4. **Document everything**: Future you will thank present you
5. **Use version control**: Track changes and enable collaboration
6. **Enable reproduction**: Someone else should be able to reproduce your work

But the *implementation* of these principles will vary based on:

- Your programming language(s)
- Your field's conventions
- Your team's preferences
- Your project's scale and complexity
- Your computing environment (laptop, HPC cluster, cloud)

## Your Turn: Join the Discussion

I'd love to hear from you:

1. **What works?** How do you organize your scientific code? What's your directory structure?
2. **What doesn't work?** What have you tried that failed? What pain points remain?
3. **What's missing?** What essential aspects of scientific code organization did I overlook?
4. **Language-specific tips?** What works particularly well in your language of choice?
5. **Field-specific conventions?** What are the norms in your discipline?

Please share your experiences in the comments. Let's build a community knowledge base of what actually works in practice.

## Further Resources

>- **"Good Enough Practices in Scientific Computing"** by Wilson et al.[^1] - Comprehensive guide to scientific computing practices

>- **Software Carpentry** - Workshops on version control, testing, and project organization[^2]

>- **"Ten Simple Rules" series** in PLOS Computational Biology[^3] - Including rules for taking advantage of Git and GitHub

>- **Cookie Cutter Data Science** - A standardized project structure template[^4]

>- **The Turing Way** - Handbook for reproducible, ethical, and collaborative data science[^5]

Remember: the goal isn't perfection, it's progress. Start organizing better today, and iterate as you learn what works for you and your team.

---

> *What's your approach to organizing scientific code? Share your structure, tips, or questions in the comments below!*

[^1]: Wilson G, Bryan J, Cranston K, Kitzes J, Nederbragt L, Teal TK (2017) Good enough practices in scientific computing. *PLOS Computational Biology* 13(6): e1005510. https://doi.org/10.1371/journal.pcbi.1005510

[^2]: Software Carpentry. "Lessons." https://software-carpentry.org/lessons/

[^3]: Perez-Riverol Y, Gatto L, Wang R, Sachsenberg T, Uszkoreit J, Leprevost FdV, et al. (2016) Ten Simple Rules for Taking Advantage of Git and GitHub. *PLOS Computational Biology* 12(7): e1004947. https://doi.org/10.1371/journal.pcbi.1004947

[^4]: DrivenData. "Cookiecutter Data Science." https://cookiecutter-data-science.drivendata.org/

[^5]: The Turing Way Community. (2022). *The Turing Way: A handbook for reproducible, ethical and collaborative research*. Zenodo. https://doi.org/10.5281/zenodo.3233853
