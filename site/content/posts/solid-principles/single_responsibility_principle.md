+++
title = "One Class, One Job: Managing Scientific Code Complexity"
date = "2026-01-05"
publishDate = "2026-01-05"
draft = true
showAuthor = false
sharingLinks = true
categories = [
    "SOLID Principles",
]
tags = [
    "Single Repository Principle",
]
image = "/images/solid/sample_spectrum_analysis.png"
description = "Your spectroscopy script is now 2,000 lines and breaks when you change anything. Learn the Single Responsibility Principle with real scientific code examples."
toc = true
+++

*Part 1 of the SOLID Principles for Scientific Programmers series*

## The Monolithic Monster

You started with a simple Python script to analyze some experimental data. A few hundred lines to load a CSV file, clean the data, run some statistics, and generate a plot. It worked perfectly!

Then your collaborator asked if you could add error bars. Then your advisor wanted the plots in a different format. Then you needed to support a different instrument's output format. Then someone found a bug in the outlier detection, and fixing it somehow broke the plotting code. Six months later, you have a 2,000-line file called `analysis.py` that nobody wants to touch because changing anything might break everything.

Sound familiar?

This is what happens when code lacks the **Single Responsibility Principle (SRP)**: the first and most fundamental of the SOLID principles.

## What Is the Single Responsibility Principle?

Robert C. Martin (Uncle Bob) originally stated it as:

> **A class should have only one reason to change.**  [^1]

He later clarified what he meant by 'reason to change'.  In a 2014 blog post, he stated:

> **Gather together the things that change for the same reasons. Separate those things that change for different reasons.**  [^2]

[^1]: *Martin, Robert C.*, **Agile Software Development: Principles, Patterns, and Practices.** Pearson Education, 2003.
[^2]: *Martin, Robert C.*, "[The Single Responsibility Principle](https://blog.cleancoder.com/uncle-bob/2014/05/08/SingleReponsibilityPrinciple.html)". The Clean Code Blog, 8 May 2014.

More practically for scientists: Each piece of code should do one thing and do it well. When you need to modify your code, you should be able to pinpoint exactly where to make the change, and that change shouldn't have ripple effects throughout your entire codebase.

## Before You Refactor: Is It Worth It?

Before we dive into the solution, let's talk about when this is worth doing.  SRP takes time to implement. Don't refactor everything at once. Prioritize based on:

1. **How often does the code change?** High-change code benefits most from SRP
2. **How many people work on it?** Team code needs clearer boundaries
3. **How long will it be maintained?** Throwaway scripts don't need perfect structure
4. **Is it causing real pain?** If it works and nobody is suffering, maybe leave it alone

## A Real Example: The Problem

Let's look at what could be a typical spectroscopy analysis script.  Don't worry about understanding every detail—focus on the structure and the tangled responsibilities. These concepts apply to any object-oriented language.

```python
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy import signal, stats
import os

class SpectroscopyAnalysis:
    """Does everything for spectroscopy analysis."""

    def __init__(self, filename):
        self.filename = filename
        self.data = None
        self.processed_data = None
        self.peaks = None
        self.results = {}

    def run_complete_analysis(self):
        """Run the entire analysis pipeline."""

        # Load data from CSV
        print(f"Loading data from {self.filename}")
        try:
            self.data = pd.read_csv(self.filename, skiprows=2)
            # Handle different column names from different instruments
            if 'Wavelength' in self.data.columns:
                self.data.rename(columns={'Wavelength': 'wavelength',
                                         'Intensity': 'intensity'}, inplace=True)
            elif 'wl' in self.data.columns:
                self.data.rename(columns={'wl': 'wavelength',
                                         'int': 'intensity'}, inplace=True)
        except Exception as e:
            print(f"Error loading file: {e}")
            return None

        # Remove baseline
        print("Processing data...")
        baseline = np.polyfit(self.data['wavelength'],
                             self.data['intensity'], 2)
        baseline_curve = np.polyval(baseline, self.data['wavelength'])
        self.data['corrected'] = self.data['intensity'] - baseline_curve

        # Remove outliers
        z_scores = np.abs(stats.zscore(self.data['corrected']))
        self.data = self.data[z_scores < 3]

        # Smooth data
        self.processed_data = signal.savgol_filter(
            self.data['corrected'], 51, 3
        )

        # Find peaks
        print("Finding peaks...")
        peak_indices, properties = signal.find_peaks(
            self.processed_data,
            height=np.mean(self.processed_data) + 2*np.std(self.processed_data),
            distance=20
        )
        self.peaks = self.data.iloc[peak_indices]['wavelength'].values

        # Calculate statistics
        print("Calculating statistics...")
        self.results['mean_intensity'] = np.mean(self.processed_data)
        self.results['std_intensity'] = np.std(self.processed_data)
        self.results['num_peaks'] = len(self.peaks)
        self.results['peak_positions'] = self.peaks
        self.results['peak_intensities'] = self.processed_data[peak_indices]

        # Generate plots
        print("Generating plots...")
        fig, (ax1, ax2) = plt.subplots(2, 1, figsize=(10, 8))

        # Raw data plot
        ax1.plot(self.data['wavelength'], self.data['intensity'],
                'b-', label='Raw', alpha=0.5)
        ax1.plot(self.data['wavelength'], baseline_curve,
                'r--', label='Baseline')
        ax1.set_xlabel('Wavelength (nm)')
        ax1.set_ylabel('Intensity (a.u.)')
        ax1.legend()
        ax1.set_title(f'Analysis of {os.path.basename(self.filename)}')

        # Processed data plot
        ax2.plot(self.data['wavelength'], self.processed_data, 'g-')
        ax2.plot(self.peaks, self.processed_data[peak_indices],
                'ro', label='Peaks')
        ax2.set_xlabel('Wavelength (nm)')
        ax2.set_ylabel('Corrected Intensity (a.u.)')
        ax2.legend()

        plt.tight_layout()

        # Save plot
        output_filename = self.filename.replace('.csv', '_analysis.png')
        plt.savefig(output_filename, dpi=300)
        print(f"Plot saved to {output_filename}")

        # Save results to file
        results_filename = self.filename.replace('.csv', '_results.txt')
        with open(results_filename, 'w') as f:
            f.write("Spectroscopy Analysis Results\n")
            f.write("="*50 + "\n")
            f.write(f"File: {self.filename}\n")
            f.write(f"Number of peaks: {self.results['num_peaks']}\n")
            f.write(f"Mean intensity: {self.results['mean_intensity']:.2f}\n")
            f.write(f"Std intensity: {self.results['std_intensity']:.2f}\n")
            f.write("\nPeak positions (nm):\n")
            for i, (pos, intensity) in enumerate(zip(self.results['peak_positions'],
                                                     self.results['peak_intensities'])):
                f.write(f"  Peak {i+1}: {pos:.2f} nm (intensity: {intensity:.2f})\n")
        print(f"Results saved to {results_filename}")

        return self.results

# Usage
analysis = SpectroscopyAnalysis('sample_spectrum.csv')
results = analysis.run_complete_analysis()
```

This code works, but it's a nightmare to maintain. Let's count the responsibilities:

1. **File I/O**: Loading CSV files with different formats
2. **Data validation**: Handling missing columns, errors
3. **Baseline correction**: Polynomial fitting and subtraction
4. **Outlier removal**: Z-score filtering
5. **Smoothing**: Savitzky-Golay filter
6. **Peak detection**: Finding and characterizing peaks
7. **Statistical analysis**: Computing summary statistics
8. **Plotting**: Creating visualizations
9. **Report generation**: Saving results to text files

That's at least 9 different reasons this class might need to change!

## Why This Is a Problem

**Scenario 1**: Your collaborator needs to change the plot style (use different colors, add a legend in a different position).

*Problem*: The plotting code is tangled with data processing. You have to read through the entire method to find the plotting section.

**Scenario 2**: You want to use the same peak detection on a different type of data (Raman instead of infrared absorption).

*Problem*: The peak detection is hardwired to this specific class. You'd have to copy-paste the code or run the entire analysis pipeline.

**Scenario 3**: You need to support a new instrument's file format.

*Problem*: The file loading logic is mixed with column renaming and error handling, all in one giant method.

**Scenario 4**: A bug in the baseline correction is causing problems.

*Problem*: Fixing the baseline might accidentally break the plotting code because they're in the same method.

## The Solution: Single Responsibility Principle

Let's refactor this into classes that each have a single, well-defined responsibility:

```ascii
BEFORE:                    AFTER:
┌─────────────────────┐   ┌──────────┐  ┌───────────┐
│ SpectroscopyAnalysis│   │  Loader  │  │ Processor │
│ - loads             │   └──────────┘  └───────────┘
│ - validates         │   ┌──────────┐  ┌───────────┐
│ - processes         │   │  Peaks   │  │   Stats   │
│ - finds peaks       │   └──────────┘  └───────────┘
│ - plots             │   ┌──────────┐  ┌───────────┐
│ - saves             │   │ Plotter  │  │ Exporter  │
│ (9 responsibilities)│   └──────────┘  └───────────┘
└─────────────────────┘         ↓
                         ┌───────────────┐
                         │   Pipeline    │
                         │ (orchestrator)│
                         └───────────────┘
```

When we're finished, the `SpectroscopyPipeline` class will still depend on all the other classes, but we'll see how to address that in Part 5 (Dependency Inversion Principle) which will make the solution even more flexible.  The class will even have some file management mixed in, but at this point we can't let perfect be the enemy of good.

```python
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy import signal, stats
from pathlib import Path
from typing import Dict, List, Tuple
from dataclasses import dataclass

# DATA STRUCTURE: Simple container for spectrum data
@dataclass  # Automatically generates __init__, __repr__, and comparison methods
class Spectrum:
    """Represents a spectrum with wavelength and intensity data."""
    wavelength: np.ndarray
    intensity: np.ndarray

    def __len__(self):
        return len(self.wavelength)
```

```python
# RESPONSIBILITY 1: Loading data from files
class SpectrumLoader:
    """Loads spectrum data from various file formats."""

    def load_csv(self, filename: str) -> Spectrum:
        """Load spectrum from CSV file."""
        data = pd.read_csv(filename, skiprows=2)

        # Normalize column names
        wavelength_col = self._find_wavelength_column(data)
        intensity_col = self._find_intensity_column(data)

        return Spectrum(
            wavelength=data[wavelength_col].values,
            intensity=data[intensity_col].values
        )

    def _find_wavelength_column(self, data: pd.DataFrame) -> str:
        """Find the wavelength column regardless of naming convention."""
        possible_names = ['wavelength', 'Wavelength', 'wl', 'lambda', 'nm']
        for name in possible_names:
            if name in data.columns:
                return name
        raise ValueError("Could not find wavelength column")

    def _find_intensity_column(self, data: pd.DataFrame) -> str:
        """Find the intensity column regardless of naming convention."""
        possible_names = ['intensity', 'Intensity', 'int', 'counts', 'signal']
        for name in possible_names:
            if name in data.columns:
                return name
        raise ValueError("Could not find intensity column")
```

```python
# RESPONSIBILITY 2: Processing spectrum data
class SpectrumProcessor:
    """Processes spectrum data (baseline correction, smoothing, etc.)."""

    def remove_baseline(self, spectrum: Spectrum, poly_order: int = 2) -> Spectrum:
        """Remove polynomial baseline from spectrum."""
        coeffs = np.polyfit(spectrum.wavelength, spectrum.intensity, poly_order)
        baseline = np.polyval(coeffs, spectrum.wavelength)
        corrected_intensity = spectrum.intensity - baseline

        return Spectrum(spectrum.wavelength, corrected_intensity)

    def remove_outliers(self, spectrum: Spectrum,
                       threshold: float = 3.0) -> Spectrum:
        """Remove outliers using z-score method."""
        z_scores = np.abs(stats.zscore(spectrum.intensity))
        mask = z_scores < threshold

        return Spectrum(
            spectrum.wavelength[mask],
            spectrum.intensity[mask]
        )

    def smooth(self, spectrum: Spectrum,
              window_length: int = 51,
              poly_order: int = 3) -> Spectrum:
        """Smooth spectrum using Savitzky-Golay filter."""
        smoothed_intensity = signal.savgol_filter(
            spectrum.intensity,
            window_length,
            poly_order
        )

        return Spectrum(spectrum.wavelength, smoothed_intensity)
```

```python
# RESPONSIBILITY 3: Finding and analyzing peaks
@dataclass
class Peak:
    """Represents a single peak in the spectrum."""
    position: float
    intensity: float

class PeakFinder:
    """Finds and characterizes peaks in spectrum data."""

    def find_peaks(self, spectrum: Spectrum,
                   prominence_threshold: float = None,
                   min_distance: int = 20) -> List[Peak]:
        """Find peaks in the spectrum."""

        if prominence_threshold is None:
            prominence_threshold=np.std(spectrum.intensity)

        peak_indices, properties = signal.find_peaks(
            spectrum.intensity,
            prominence=prominence_threshold,
            distance=min_distance
        )

        peaks = []
        for idx in peak_indices:
            peaks.append(Peak(
                position=spectrum.wavelength[idx],
                intensity=spectrum.intensity[idx]
            ))

        return peaks
```

```python
# RESPONSIBILITY 4: Computing statistics
@dataclass
class SpectrumStatistics:
    """Statistics computed from spectrum data."""
    mean_intensity: float
    std_intensity: float
    min_intensity: float
    max_intensity: float
    num_peaks: int
    peak_positions: List[float]

class StatisticalAnalyzer:
    """Computes statistical measures for spectrum data."""

    def analyze(self, spectrum: Spectrum, peaks: List[Peak]) -> SpectrumStatistics:
        """Compute comprehensive statistics."""
        return SpectrumStatistics(
            mean_intensity=np.mean(spectrum.intensity),
            std_intensity=np.std(spectrum.intensity),
            min_intensity=np.min(spectrum.intensity),
            max_intensity=np.max(spectrum.intensity),
            num_peaks=len(peaks),
            peak_positions=[p.position for p in peaks]
        )
```

```python
# RESPONSIBILITY 5: Creating visualizations
class SpectrumPlotter:
    """Creates plots for spectrum visualization."""

    def plot_analysis(self, raw_spectrum: Spectrum,
                     processed_spectrum: Spectrum,
                     peaks: List[Peak],
                     title: str = "Spectrum Analysis") -> plt.Figure:
        """Create a comprehensive analysis plot."""

        fig, (ax1, ax2) = plt.subplots(2, 1, figsize=(10, 8))

        # Raw data
        ax1.plot(raw_spectrum.wavelength, raw_spectrum.intensity,
                'b-', label='Raw Data', alpha=0.5)
        ax1.set_xlabel('Wavelength (nm)')
        ax1.set_ylabel('Intensity (a.u.)')
        ax1.legend()
        ax1.set_title(title)
        ax1.grid(True, alpha=0.3)

        # Processed data with peaks
        ax2.plot(processed_spectrum.wavelength, processed_spectrum.intensity,
                'g-', label='Processed')

        if peaks:
            peak_positions = [p.position for p in peaks]
            peak_intensities = [p.intensity for p in peaks]
            ax2.plot(peak_positions, peak_intensities,
                    'ro', markersize=8, label=f'{len(peaks)} Peaks')

        ax2.set_xlabel('Wavelength (nm)')
        ax2.set_ylabel('Corrected Intensity (a.u.)')
        ax2.legend()
        ax2.grid(True, alpha=0.3)

        plt.tight_layout()
        return fig
```

```python
# RESPONSIBILITY 6: Saving results
class ResultsExporter:
    """Exports analysis results to various formats."""

    def save_plot(self, fig: plt.Figure, filename: str, dpi: int = 300):
        """Save a matplotlib figure to file."""
        fig.savefig(filename, dpi=dpi, bbox_inches='tight')

    def save_report(self, filename: str,
                   stats: SpectrumStatistics,
                   peaks: List[Peak],
                   source_file: str):
        """Save a text report of the analysis."""
        with open(filename, 'w') as f:
            f.write("Spectroscopy Analysis Results\n")
            f.write("=" * 50 + "\n\n")
            f.write(f"Source file: {source_file}\n\n")

            f.write("Summary Statistics:\n")
            f.write(f"  Mean intensity: {stats.mean_intensity:.2f}\n")
            f.write(f"  Std intensity: {stats.std_intensity:.2f}\n")
            f.write(f"  Min intensity: {stats.min_intensity:.2f}\n")
            f.write(f"  Max intensity: {stats.max_intensity:.2f}\n\n")

            f.write(f"Peak Analysis:\n")
            f.write(f"  Number of peaks: {stats.num_peaks}\n\n")

            if peaks:
                f.write("  Peak Details:\n")
                for i, peak in enumerate(peaks, 1):
                    f.write(f"    Peak {i}: {peak.position:.2f} nm "
                           f"(intensity: {peak.intensity:.2f})\n")
```

```python
# RESPONSIBILITY 7: Orchestrating the analysis
class SpectroscopyPipeline:
    """Coordinates the complete analysis workflow."""

    def __init__(self):
        self.loader = SpectrumLoader()
        self.processor = SpectrumProcessor()
        self.peak_finder = PeakFinder()
        self.analyzer = StatisticalAnalyzer()
        self.plotter = SpectrumPlotter()
        self.exporter = ResultsExporter()

    def analyze_file(self, filename: str,
                    output_dir: str = None) -> SpectrumStatistics:
        """Run complete analysis pipeline on a file."""

        # Set up output directory
        if output_dir is None:
            output_dir = Path(filename).parent
        output_dir = Path(output_dir)
        base_name = Path(filename).stem

        # Load data
        print(f"Loading {filename}...")
        raw_spectrum = self.loader.load_csv(filename)

        # Process data
        print("Processing spectrum...")
        spectrum = self.processor.remove_baseline(raw_spectrum)
        spectrum = self.processor.remove_outliers(spectrum)
        spectrum = self.processor.smooth(spectrum)

        # Find peaks
        print("Finding peaks...")
        peaks = self.peak_finder.find_peaks(spectrum)

        # Compute statistics
        print("Computing statistics...")
        stats = self.analyzer.analyze(spectrum, peaks)

        # Create visualizations
        print("Generating plots...")
        fig = self.plotter.plot_analysis(
            raw_spectrum, spectrum, peaks,
            title=f"Analysis of {Path(filename).name}"
        )

        # Save results
        print("Saving results...")
        self.exporter.save_plot(
            fig,
            output_dir / f"{base_name}_analysis.png"
        )
        self.exporter.save_report(
            output_dir / f"{base_name}_results.txt",
            stats, peaks, filename
        )

        plt.close(fig)
        print("Analysis complete!")

        return stats
```

```python
# Usage - Same simplicity, much better design!
pipeline = SpectroscopyPipeline()
results = pipeline.analyze_file('sample_spectrum.csv')
```

## Why This Is Better

### 1. Easy to Modify Individual Components

**Want to change the plot style?** Only touch `SpectrumPlotter`:

```python
class CustomPlotter(SpectrumPlotter):
    """Custom plotting style for publications."""

    def plot_analysis(self, raw_spectrum, processed_spectrum, peaks, title):
        # Use your preferred style
        plt.style.use('seaborn-paper')
        fig, ax = plt.subplots(figsize=(6, 4))
        # Custom implementation
        return fig

# Use it
pipeline = SpectroscopyPipeline()
pipeline.plotter = CustomPlotter()  # Easy swap!
```

### 2. Easy to Reuse Components

**Want to use the peak finder on different data?**

```python
# Works with any Spectrum object!
finder = PeakFinder()
peaks = finder.find_peaks(my_raman_spectrum)
peaks = finder.find_peaks(my_xrd_pattern)
```

### 3. Easy to Test

Each component can be tested independently with synthetic data—no real files, no real instruments, no complex setup. For example:

```python
import unittest

class TestPeakFinder(unittest.TestCase):
    def test_finds_obvious_peak(self):
        # Create synthetic data with known peak
        wavelength = np.linspace(400, 700, 100)
        intensity = np.exp(-(wavelength - 550)**2 / 100)
        spectrum = Spectrum(wavelength, intensity)

        finder = PeakFinder()
        peaks = finder.find_peaks(spectrum)

        self.assertEqual(len(peaks), 1)
        self.assertAlmostEqual(peaks[0].position, 550, delta=5)
```

You can test the processor with known baselines, the loader with mock files, and the statistics with hand-calculated values. In the monolithic version, testing anything required the entire pipeline to work.

### 4. Easy to Extend

**Need to support a new file format?**

```python
class SpectrumLoader:
    def load_csv(self, filename: str) -> Spectrum:
        # existing code
        pass

    def load_json(self, filename: str) -> Spectrum:
        """New method for JSON files."""
        import json
        with open(filename) as f:
            data = json.load(f)
        return Spectrum(
            wavelength=np.array(data['wavelength']),
            intensity=np.array(data['intensity'])
        )
```

**Need a different baseline correction method?**

```python
class SpectrumProcessor:
    def remove_baseline(self, spectrum: Spectrum, poly_order: int = 2):
        # existing polynomial method
        pass

    def remove_baseline_als(self, spectrum: Spectrum,
                           lam: float = 1e5, p: float = 0.01):
        """Alternative: Asymmetric Least Squares baseline."""
        # New implementation
        pass
```

### 5. Clear Documentation and Understanding

Each class has a single, clear purpose:

- **SpectrumLoader**: "I load spectra from files"
- **SpectrumProcessor**: "I clean and process spectrum data"
- **PeakFinder**: "I find peaks in spectra"
- **StatisticalAnalyzer**: "I compute statistics"
- **SpectrumPlotter**: "I create plots"
- **ResultsExporter**: "I save results to files"

New collaborators can immediately understand what each piece does.

## Red Flags That You Need SRP

- Methods longer than 50 lines
- Changing one thing breaks another
- Can't write a test without mocking 5 dependencies
- Classes or functions with 'and' in their description
- You can't reuse a piece without bringing the whole system along
- The same code is repeated in multiple places with slight variations
- You're afraid to refactor

## Common Mistakes: Over-Engineering

The most common mistake I see is going too far. You don't need a separate class for every single function. Here's my rule of thumb:

**When to create a new class:**

- The responsibility is complex enough to warrant multiple methods
- You might want to swap implementations (different file formats, algorithms)
- The code could be reused in different contexts
- Testing would be easier with isolation

**When to keep it as a simple function:**

- It's a simple calculation or transformation
- It's only used in one place
- It has no internal state to manage
- It's obvious what it does

For example, computing a simple average doesn't need its own class—just use `np.mean()`. But if you're computing a suite of related statistics, a `StatisticalAnalyzer` makes sense.

## Performance Notes

You might worry about the overhead of creating multiple objects. In practice, this is negligible for typical scientific workflows. The real performance bottleneck is your algorithms (peak finding, smoothing), not object instantiation.

## Your Turn

Right now, open your largest Python file. Count how many different concerns it handles. If the answer is more than three, you've found your refactoring target. Start with the piece that causes the most pain—usually file I/O or visualization—extract it, test it, and feel the relief of having one less thing to worry about.

Next week, we'll tackle the **Open/Closed Principle**: how to add new analysis methods without touching (and potentially breaking) your validated code.

---

*Wrestling with monolithic scientific code? Share your experiences in the comments!*
