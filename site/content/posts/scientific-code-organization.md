+++
title = "Organizing Source Code for Scientific Programmers: A Practical Guide"
date = "2025-12-20"
draft = false
sharingLinks = true
image = ""
description = ""
tags = [
    "project organization"
]
categories = [
]
toc = false
+++

If you've ever opened a scientific code repository and found yourself lost in a maze of notebooks, scripts, data files, and outputs all jumbled together, you're not alone. While line-of-business developers have established conventions (like [David Fowler's .NET project structure](https://gist.github.com/davidfowl/ed7564297c61fe9ab814)), scientific computing has its own unique needs that require a different approach.

Scientific code repositories face challenges that traditional software projects don't: managing raw and processed data, organizing analysis notebooks alongside production scripts, handling computational experiments, and ensuring reproducibility. This post provides practical guidance on organizing your scientific code repositories effectively.

## Why Organization Matters in Scientific Computing

Poor organization doesn't just create inconvenience—it creates real problems. Lost data, irreproducible analyses, and hours wasted searching for files are common consequences. A well-organized repository accelerates science, facilitates collaboration, and makes your research reproducible.

As researchers at UC Berkeley's D-Lab note, structuring repositories according to predictable templates makes science easier, cleaner, and more reproducible. When everyone on your team can predict where files should be, collaboration becomes intuitive rather than frustrating.

## The Fundamental Principle: Separate by Type and Purpose

The key organizing principle for scientific repositories is to structure by file type and purpose. This creates consistency across projects and makes it intuitive to find what you need. Here's a recommended structure:

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
├── environment.yml
├── requirements.txt
├── README.md
├── LICENSE
└── .gitignore
```

Let's break down each component and when to use it.

## Core Directories

### `data/` - The Sacred Ground

Your data directory should have two subdirectories:

- **`data/raw/`**: Original, unmodified data files. Treat this as **immutable**. Never, ever modify files here. You can even set these files as read-only to prevent accidents.
- **`data/processed/`**: Cleaned, transformed, or analyzed versions of your data.

This separation is crucial for reproducibility. Anyone should be able to run your processing scripts on the raw data and regenerate your processed data.

**Important note**: For large datasets, you may want to store only metadata or download scripts in version control rather than the actual data files. Consider using services like OSF, Zenodo, or institutional repositories for large data files.

### `src/` - Your Core Analysis Code

This is where reusable, production-quality code lives. Think of this as the "scientific guts" of your project. Code here should be:

- Organized into logical modules or packages
- Well-documented with docstrings
- Testable and (ideally) tested
- Importable from notebooks or scripts

A typical structure might be:

```
src/
├── __init__.py
├── data_processing.py
├── statistical_analysis.py
├── plotting.py
└── utils.py
```

Making your `src/` directory an installable package is highly recommended. This allows you to import your functions anywhere without worrying about Python paths or working directories:

```python
# After installing your package
from myproject.analysis import run_analysis
```

### `notebooks/` - Exploration and Prototyping

Jupyter notebooks are fantastic for interactive exploration, but they come with challenges: they encourage non-modular code, can become difficult to test, and tend to accumulate cruft over time.

**Best practices for notebooks:**

1. Use notebooks for exploration, visualization, and prototyping
2. Keep notebooks focused on specific questions or analyses
3. Name them clearly with numbers for ordering: `01-data-exploration.ipynb`, `02-initial-modeling.ipynb`
4. Transition mature code from notebooks to the `src/` directory
5. Import functions from `src/` rather than copying code between notebooks

When a notebook starts to feel unwieldy (typically over 10-15 cells of complex logic), extract the reusable parts into modules in `src/`.

### `scripts/` - Automation and Batch Processing

Scripts are for automated, reproducible workflows. These are Python (or R, bash, etc.) files that:

- Run without interaction
- Accept command-line arguments
- Can be executed on computing clusters or in pipelines
- Orchestrate complete analyses from start to finish

Example use cases:
- Data download and preprocessing pipelines
- Running models with different parameters
- Generating all figures for a paper
- Batch processing multiple datasets

A controller script (e.g., `run_all.py` or `pipeline.sh`) that executes the entire analysis workflow in order is extremely valuable for reproducibility.

### `results/` - Analysis Outputs

Store generated outputs here, not in version control (add to `.gitignore`):

```
results/
├── figures/        # Plots, visualizations
├── output/         # Tables, statistics, processed results
└── models/         # Trained models, fitted parameters
```

Why separate from data? Results are *generated* by code and should be reproducible. If you lose them, you can regenerate them. Data (especially raw data) cannot be regenerated.

### `docs/` - Documentation

Include:
- Project documentation
- Analysis notes or lab notebook entries
- Manuscript drafts
- Supplementary materials
- API documentation (if auto-generated)

### `tests/` - Test Code

Yes, even scientific code should have tests! At minimum, include:
- Unit tests for core functions
- Integration tests for workflows
- Assertions within notebooks that verify expected behavior

Testing helps ensure correctness and catches bugs when you modify code.

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

Include files that specify your computational environment:
- **`environment.yml`** (for conda)
- **`requirements.txt`** (for pip)
- **`Dockerfile`** (for containerized environments)

Virtual environments are essential. They ensure that your project's dependencies don't conflict with other projects and make your work reproducible. Use conda environments (recommended for scientific computing) or Python virtual environments for each project.

### LICENSE

If your project is open source, include a license. Common choices for scientific code include MIT, BSD, or GPL licenses.

### .gitignore

Prevent clutter by ignoring:
- `data/raw/*` (if files are large)
- `results/*`
- `.ipynb_checkpoints/`
- `__pycache__/`
- `*.pyc`
- `.DS_Store`
- Virtual environment directories

## The Notebook vs. Script Decision

One of the most common questions in scientific computing is when to use notebooks versus scripts. Here's a decision framework:

**Use notebooks when:**
- Exploring data interactively
- Creating visualizations for papers
- Prototyping new analyses
- Communicating results to collaborators
- Teaching or documenting workflows

**Use scripts when:**
- Running analyses on many parameter sets
- Processing multiple files in batch
- Running long computations on clusters
- Automating reproducible workflows
- Code needs to be modular and testable

**The golden rule**: Start in notebooks for exploration, but transition mature code to scripts and modules as your project develops. Many projects will have both—notebooks that import from well-organized modules in `src/`.

## Advanced: Converting Notebooks to Scripts

When you need to scale up from interactive work to batch processing, you have several options:

1. **Manual extraction**: Copy code from notebooks into Python files, adding command-line argument parsing
2. **Automatic conversion**: Use `jupyter nbconvert --to script notebook.ipynb`
3. **Parameterized notebooks**: Use tools like Papermill to execute notebooks with different parameters
4. **Hybrid approach**: Design notebooks to detect if they're running as scripts using tools like `nbscript`

## Organizing for Different Project Scales

### Small Projects (Single Analysis → Single Paper)

For small projects, a simplified structure works well:

```
project/
├── data/
├── notebooks/
├── results/
├── environment.yml
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

## Self-Contained Projects

Strive for self-containedness: everything needed to reproduce your analysis should be in the project directory. This sometimes conflicts with avoiding duplication (e.g., when multiple projects share data), but for publishable research, self-containedness usually wins.

A colleague should be able to:
1. Clone your repository
2. Create the environment (`conda env create -f environment.yml`)
3. Run your analysis scripts
4. Reproduce your results

## Version Control Best Practices

### What to Track

- All code (scripts, notebooks, source files)
- Documentation
- Environment specifications
- Small data files (< 100MB)
- Metadata about large data files

### What NOT to Track

- Large data files (use data repositories instead)
- Generated results and figures
- Temporary files
- Virtual environment directories
- IDE-specific settings (unless shared intentionally)

### Commit Messages for Science

Good commit messages in scientific projects should describe *what changed scientifically*, not just what changed in the code:

- ❌ "Updated analysis.py"
- ✅ "Added bootstrap confidence intervals to treatment effect estimates"

## Common Pitfalls and Solutions

### Pitfall 1: The Notebook Sprawl

**Problem**: 50 notebooks with names like "Untitled1.ipynb", "Copy of Final_Analysis_v3.ipynb"

**Solution**:
- Name notebooks descriptively with numbered prefixes
- Regularly archive or delete obsolete notebooks
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
- Use `pathlib` for cross-platform compatibility
- Consider configuration files for environment-specific settings

### Pitfall 4: Missing Dependencies

**Problem**: "It works on my machine" but fails everywhere else

**Solution**:
- Maintain updated `requirements.txt` or `environment.yml`
- Document non-Python dependencies
- Consider using containers (Docker) for complex environments

## Making Your Code a Package

For projects that will be reused or shared, consider making your `src/` directory an installable package. Create a minimal `setup.py`:

```python
from setuptools import setup, find_packages

setup(
    name="myproject",
    version="0.1.0",
    packages=find_packages(where="src"),
    package_dir={"": "src"},
    install_requires=[
        "numpy",
        "pandas",
        "matplotlib",
    ],
)
```

Then install in development mode:
```bash
pip install -e .
```

Now you can import your code from anywhere: `from myproject import analysis`

## Collaborative Workflows

When working in teams:

1. **Establish conventions early**: Agree on directory structure before starting
2. **Document everything**: A well-maintained README is invaluable
3. **Use branches**: Keep experimental work in branches, merge stable code to main
4. **Review code**: Even informal code reviews catch bugs and improve quality
5. **Automate testing**: Run tests automatically on commit (continuous integration)

## Publishing Your Code

When publishing research, your code repository becomes part of your scientific output:

1. **Clean up**: Remove dead code, document everything
2. **Add a LICENSE**: Make it clear how others can use your code
3. **Archive a release**: Use Zenodo to get a DOI for the exact version used in your paper
4. **Link from your paper**: Include repository URL and DOI in your manuscript
5. **Consider a CITATION file**: Tell users how to cite your code

## Adapting Conventions to Your Field

Different scientific fields have their own conventions:

- **Computational biology**: Often uses Snakemake or Nextflow for pipeline management
- **Physics simulations**: May need specialized organization for parameter sweeps
- **Machine learning**: Often separates data preprocessing, model definitions, training scripts, and evaluation
- **Social sciences**: May emphasize reproducible reports using R Markdown or Quarto

The core principles remain the same: organize by purpose, maintain raw data separately, make code modular and testable, and document everything.

## Conclusion

Good code organization isn't about following rules dogmatically—it's about making your life easier and your science better. Start with these principles:

1. **Separate concerns**: Data, code, and results in different directories
2. **Preserve raw data**: Never modify original data files
3. **Make code modular**: Extract reusable functions to modules
4. **Document everything**: Future you will thank present you
5. **Use version control**: Track changes and enable collaboration
6. **Enable reproduction**: Someone else should be able to reproduce your work

Remember: "Consistency and predictability are more important than hairsplitting." Pick a structure that makes sense for your project, document it in your README, and stick to it.

Your repository is part of your scientific legacy. Organize it like you'd want to find it if you returned to it in five years—because you probably will.

## Further Resources

- **"Good Enough Practices in Scientific Computing"** by Wilson et al. - Comprehensive guide to scientific computing practices
- **Software Carpentry** - Workshops on version control, testing, and project organization
- **"Ten Simple Rules" series** in PLOS Computational Biology - Including rules for taking advantage of Git and GitHub
- **Cookie Cutter Data Science** - A standardized but flexible project structure for data science
- **The Turing Way** - Handbook for reproducible, ethical, and collaborative data science

Remember: the goal isn't perfection, it's progress. Start organizing better today, and you'll thank yourself tomorrow.
